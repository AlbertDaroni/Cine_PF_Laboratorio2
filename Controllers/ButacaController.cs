
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Proyecto_Final.Models;
using Proyecto_Final.Data;

namespace Proyecto_Final.Controllers;

[Authorize(Roles = "Administrador,Empleado")]
public class ButacaController : Controller
{
    private readonly BD_Context _context;

    public ButacaController(BD_Context context)
    {
        _context = context;
    }

    // Listado de butacas
    [HttpGet]
    public async Task<IActionResult> Indice(int? salaID)
    {
        var consulta = _context.Butacas
            .Include(b => b.Sala)
                .ThenInclude(s => s!.Cine)
            .AsNoTracking()
            .AsQueryable();

        if (salaID.HasValue)
        {
            consulta = consulta.Where(b => b.SalaID == salaID.Value);
        }

        ViewBag.SalaID = salaID;

        var butacas = await consulta
            .OrderBy(b => b.SalaID)
            .ThenBy(b => b.Numero)
            .ToListAsync();

        return View(butacas);
    }

    // Crear - GET
    [HttpGet]
    public async Task<IActionResult> Crear()
    {
        return View(new Butaca());
    }

    // Crear - POST
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(
        [Bind("Numero,SalaID")] Butaca butaca)
    {
        butaca.Numero = butaca.Numero?.Trim().ToUpperInvariant()
                        ?? string.Empty;

        var sala = await _context.Salas
            .AsNoTracking()
            .Include(s => s.Cine)
            .FirstOrDefaultAsync(s => s.ID == butaca.SalaID);

        ViewBag.SalaSeleccionada = sala == null
            ? ""
            : $"{sala.Cine?.Nombre} — Sala {sala.Numero} ({sala.Tipo})";

        if (sala == null || !sala.Estado)
        {
            ModelState.AddModelError(
                nameof(Butaca.SalaID), "Seleccioná una sala activa.");
        }
        else
        {
            bool repetida = await _context.Butacas.AnyAsync(b =>
                b.SalaID == butaca.SalaID &&
                b.Numero == butaca.Numero);

            if (repetida)
                ModelState.AddModelError(
                    nameof(Butaca.Numero),
                    "Ya existe una butaca con ese número en la sala.");

            int cantidad = await _context.Butacas
                .CountAsync(b => b.SalaID == butaca.SalaID);

            if (cantidad >= sala.Capacidad)
                ModelState.AddModelError(
                    nameof(Butaca.SalaID),
                    "La sala ya alcanzó su capacidad máxima.");
        }

        if (!ModelState.IsValid)
        {
            return View(butaca);
        }

        // No marcamos la butaca como ocupada permanentemente.
        // La disponibilidad por función depende de las reservas.
        butaca.Ocupado = false;

        _context.Butacas.Add(butaca);
        await _context.SaveChangesAsync();

        TempData["Mensaje"] = "Butaca creada correctamente.";
        return RedirectToAction(nameof(Indice));
    }

    // Editar - GET
    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        var butaca = await _context.Butacas
            .AsNoTracking()
            .Include(b => b.Sala)
                .ThenInclude(s => s!.Cine)
            .FirstOrDefaultAsync(b => b.ID == id);

        if (butaca == null) return NotFound();

        return View(butaca);
    }

    // Editar - POST
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, string Numero)
    {
        var butaca = await _context.Butacas
            .Include(b => b.Sala)
                .ThenInclude(s => s!.Cine)
            .FirstOrDefaultAsync(b => b.ID == id);
        if (butaca == null) return NotFound();

        Numero = Numero?.Trim().ToUpperInvariant() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(Numero) || Numero.Length > 3)
            ModelState.AddModelError(
                nameof(Butaca.Numero),
                "Ingresá un número de hasta 3 caracteres.");

        bool repetida = await _context.Butacas.AnyAsync(b =>
            b.ID != id &&
            b.SalaID == butaca.SalaID &&
            b.Numero == Numero);

        if (repetida)
            ModelState.AddModelError(
                nameof(Butaca.Numero),
                "Ya existe otra butaca con ese número en la sala.");

        if (!ModelState.IsValid)
        {
            return View(butaca);
        }

        butaca.Numero = Numero;
        await _context.SaveChangesAsync();

        TempData["Mensaje"] = "Butaca modificada correctamente.";
        return RedirectToAction(nameof(Indice));
    }

    // Eliminar - GET
    [HttpGet]
    public async Task<IActionResult> Eliminar(int id)
    {
        var butaca = await _context.Butacas
            .Include(b => b.Sala)
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.ID == id);

        if (butaca == null) return NotFound();

        ViewBag.TieneReservas = await _context.DetallesReservas
            .AnyAsync(d => d.ButacaID == id);

        return View(butaca);
    }

    // Eliminar - POST
    [HttpPost, ValidateAntiForgeryToken, ActionName("Eliminar")]
    public async Task<IActionResult> ConfirmarEliminar(int id)
    {
        var butaca = await _context.Butacas
            .Include(b => b.Sala)
            .FirstOrDefaultAsync(b => b.ID == id);

        if (butaca == null) return NotFound();

        bool tieneReservas = await _context.DetallesReservas
            .AnyAsync(d => d.ButacaID == id);

        if (tieneReservas)
        {
            ViewBag.TieneReservas = true;
            ModelState.AddModelError("",
                "No se puede eliminar una butaca con historial de reservas.");

            return View("Eliminar", butaca);
        }

        _context.Butacas.Remove(butaca);
        await _context.SaveChangesAsync();

        TempData["Mensaje"] = "Butaca eliminada correctamente.";
        return RedirectToAction(nameof(Indice));
    }    
}