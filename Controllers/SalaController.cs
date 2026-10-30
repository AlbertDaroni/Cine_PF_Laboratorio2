using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Proyecto_Final.Models;
using Proyecto_Final.Data;

namespace Proyecto_Final.Controllers;

[Authorize(Roles = "Administrador, Empleado")]
public class SalaController : Controller
{
    private readonly BD_Context _context;
    public SalaController(BD_Context context) { this._context = context; }

    [HttpGet]
    public async Task<IActionResult> BuscarCines(string? termino)
    {
        termino = termino?.Trim();


        if (string.IsNullOrEmpty(termino) || termino.Length < 2)
        {
            return Json(Array.Empty<object>());
        }

        if (termino.Length > 100)
        {
            return BadRequest("La busqueda es demasiado larga,");
        }


        var cines = await _context.Cines
            .AsNoTracking()
            .Where(c => c.Nombre.Contains(termino)
                || c.Direccion.Contains(termino))
            .OrderBy(c => c.Nombre)
            .ThenBy(c => c.ID)
            .Select(c => new
            {
                id = c.ID,
                nombre = c.Nombre,
                direccion = c.Direccion
            })
            .Take(10)
            .ToListAsync();


        return Json(cines);
    }

    // -----------------------------------------------------------------------------------
    // Funciones para Índice
    [HttpGet, AllowAnonymous]
    public async Task<IActionResult> Indice(int pagina = 1)
    {
        const int tamanioPagina = 10;


        var consulta = _context.Salas.AsNoTracking();


        int total = await consulta.CountAsync();


        int totalPaginas = Math.Max(
            1,
            (int)Math.Ceiling(total / (double)tamanioPagina));


        pagina = Math.Clamp(pagina, 1, totalPaginas);


        var salas = await consulta
            .Include(s => s.Cine)
            .OrderBy(s => s.CineID)
            .ThenBy(s => s.Numero)
            .ThenBy(s => s.ID)
            .Skip((pagina - 1) * tamanioPagina)
            .Take(tamanioPagina)
            .ToListAsync();


        ViewBag.Pagina = pagina;
        ViewBag.totalPaginas = totalPaginas;


        return View(salas);
    }
    // -----------------------------------------------------------------------------------
    // Funciones para Crear
    [HttpGet]
    public IActionResult Crear() { return View(new Sala()); }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(
        [Bind("Numero,Tipo,Capacidad,Estado,CineID")] Sala sala)
    {
        var cine = await _context.Cines
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.ID == sala.CineID);


        if (cine == null)
        {
            ModelState.AddModelError("CineID", "Selecciona un cine valido.");
        }
        else
        {
            ViewBag.CineSeleccionado = cine.Nombre;


            var numeroRepetido = await _context.Salas
                .AnyAsync(s => s.CineID == sala.CineID
                    && s.Numero == sala.Numero);


            if (numeroRepetido)
            {
                ModelState.AddModelError(
                    "Numero", "Ese cine ya tiene una sala con ese numero.");
            }
        }


        if (!ModelState.IsValid)
            return View(sala);


        _context.Salas.Add(sala);
        await _context.SaveChangesAsync();


        return RedirectToAction(nameof(Indice));
    }

    // -----------------------------------------------------------------------------------
    // Funciones para Editar
    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        var sala = await _context.Salas
            .AsNoTracking()
            .Include(s => s.Cine)
            .FirstOrDefaultAsync(s => s.ID == id);


        if (sala == null)
            return NotFound();


        return View(sala);
    }

    [HttpPost, ValidateAntiForgeryToken, ActionName("Editar")]
    public async Task<IActionResult> GuardarEditar(int id)
    {
        var sala = await _context.Salas
            .Include(s => s.Cine)
            .FirstOrDefaultAsync(s => s.ID == id);


        if (sala == null)
            return NotFound();


        bool datosValidos = await TryUpdateModelAsync<Sala>(
            sala,
            "",
            s => s.Numero,
            s => s.Tipo,
            s => s.Estado);


        if (!datosValidos)
            return View(sala);


        bool numeroRepetido = await _context.Salas
            .AnyAsync(s =>
                s.CineID == sala.CineID
                && s.Numero == sala.Numero
                && s.ID != sala.ID);


        if (numeroRepetido)
        {
            ModelState.AddModelError(
                "Numero", "Ese cine ya tiene una sala con ese numero.");


            return View(sala);
        }

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Indice));
    }

    // -----------------------------------------------------------------------------------
    // Funciones para Eliminar
    [HttpGet]
    public async Task<IActionResult> Eliminar(int id)
    {
        var sala = await _context.Salas
            .AsNoTracking()
            .Include(s => s.Cine)
            .FirstOrDefaultAsync(s => s.ID == id);

        if (sala == null) return NotFound();

        ViewBag.TieneFunciones = await _context.Funciones
            .AnyAsync(f => f.SalaID == id);

        return View(sala);
    }

    [HttpPost, ValidateAntiForgeryToken, ActionName("Eliminar")]
    public async Task<IActionResult> ConfirmarEliminar(int id)
    {
        var sala = await _context.Salas
            .Include(s => s.Cine)
            .FirstOrDefaultAsync(s => s.ID == id);

        if (sala == null) return NotFound();

        bool tieneFunciones = await _context.Funciones
            .AnyAsync(f => f.SalaID == id);

        if (tieneFunciones)
        {
            ViewBag.TieneFunciones = true;

            ModelState.AddModelError(
                "",
                "No se puede eliminar esta sala porque tiene funciones asociadas. Podés desactivarla desde Editar.");

            return View("Eliminar", sala);
        }

        _context.Salas.Remove(sala);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Indice));
    }
}