using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Proyecto_Final.Models;
using Proyecto_Final.Data;

namespace Proyecto_Final.Controllers;

[Authorize(Roles = "Administrador, Empleado")]
public class PagoController : Controller {
    private readonly BD_Context _context;
    public PagoController(BD_Context context) { this._context = context; }

    [HttpGet]
    public async Task<IActionResult> BuscarReservas(string? termino) {
        termino = termino?.Trim();

        if (string.IsNullOrEmpty(termino) || termino.Length < 2) return Json(Array.Empty<object>());
        if (termino.Length > 100) return BadRequest("La búsqueda es demasiado larga.");

        int.TryParse(termino, out int reservaID);

        var reservas = await _context.Reservas
            .AsNoTracking()
            .Where(r => r.Estado
                && !_context.Pagos.Any(p => p.ReservaID == r.ID)
                && (
                    r.ID == reservaID
                    || (r.Usuario != null
                        && (
                            r.Usuario.Nombre.Contains(termino)
                            || r.Usuario.Apellido.Contains(termino)
                            || r.Usuario.Correo.Contains(termino)
                        ))
                ))
            .OrderByDescending(r => r.ID)
            .Select(r => new {
                id = r.ID,
                nombre = r.Usuario != null ? r.Usuario.Nombre + " " + r.Usuario.Apellido : "Sin usuario",
                pelicula = r.Funcion != null && r.Funcion.Pelicula != null ? r.Funcion.Pelicula.Titulo : "Sin película",
                monto = r.Monto
            })
            .Take(10)
            .ToListAsync()
        ;

        return Json(reservas);
    }

    // -----------------------------------------------------------------------------------
    // Funciones para Índice
    public async Task<IActionResult> Indice() { return View(await _context.Pagos.AsNoTracking().ToListAsync()); }

    // -----------------------------------------------------------------------------------
    // Funciones para Crear
    [HttpGet,]
    public IActionResult Crear() { return View(new Pago()); }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear([Bind("Concepto,ReservaID")] Pago pago) {
        var reserva = await _context.Reservas
            .AsNoTracking()
            .Include(r => r.Usuario)
            .FirstOrDefaultAsync(r => r.ID == pago.ReservaID)
        ;

        if (reserva == null || !reserva.Estado) ModelState.AddModelError("ReservaID", "Seleccioná una reserva activa.");
        if (await _context.Pagos.AnyAsync(p => p.ReservaID == pago.ReservaID)) ModelState.AddModelError("ReservaID", "Esta reserva ya tiene un pago registrado.");
        if (reserva != null && reserva.Monto <= 0) ModelState.AddModelError("ReservaID", "La reserva debe tener un monto mayor que cero.");

        ViewBag.ReservaSeleccionada = reserva == null ? "" : $"Reserva {reserva.ID} — {reserva.Usuario?.Nombre} {reserva.Usuario?.Apellido}";
        ViewBag.MontoReserva = reserva?.Monto;

        if (!ModelState.IsValid) return View(pago);

        pago.Importe = reserva!.Monto;
        pago.Fecha_Pago = DateTime.Now;
        pago.Fecha_Anulacion = null;
        pago.Estado = true;

        _context.Pagos.Add(pago);

        try {
            await _context.SaveChangesAsync();
        } catch (DbUpdateException) {
            // La restricción UNIQUE también evita pagos duplicados
            // si dos personas intentan registrar uno al mismo tiempo.
            bool yaTienePago = await _context.Pagos
                .AsNoTracking()
                .AnyAsync(p => p.ReservaID == pago.ReservaID)
            ;

            if (!yaTienePago) throw;

            ModelState.AddModelError("ReservaID", "Otra operación ya registró el pago de esta reserva.");

            return View(pago);
        }

        return RedirectToAction(nameof(Indice));
    }

    // -----------------------------------------------------------------------------------
    // Funciones para Editar
    [HttpGet]
    public async Task<IActionResult> Editar(int id) {
        var pago = await _context.Pagos
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.ID == id)
        ;

        if (pago == null) return NotFound();

        return View(pago);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, [Bind("ID,Concepto,Estado")] Pago datos) {
        if (id != datos.ID) return BadRequest();

        var pago = await _context.Pagos.FirstOrDefaultAsync(p => p.ID == id);
        if (pago == null) return NotFound();

        // Un pago anulado no se reactiva desde esta edición.
        if (!pago.Estado && datos.Estado) ModelState.AddModelError("Estado", "No se puede reactivar un pago anulado.");
        if (!ModelState.IsValid) {
            pago.Concepto = datos.Concepto;
            return View(pago);
        }

        pago.Concepto = datos.Concepto;

        if (pago.Estado && !datos.Estado) {
            pago.Estado = false;
            pago.Fecha_Anulacion = DateTime.Now;
        }

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Indice));
    }

    // -----------------------------------------------------------------------------------
    // Funciones para Eliminar
    [HttpGet]
    public async Task<IActionResult> Eliminar(int id) {
        var pago = await _context.Pagos.FindAsync(id);
        if (pago == null) return NotFound();
        return View(pago);
    }

    [HttpPost, ValidateAntiForgeryToken, ActionName("Eliminar")]
    public async Task<IActionResult> ConfirmarEliminar(int id) {
        var pago = await _context.Pagos.FindAsync(id);
        if (pago == null) return NotFound();

        _context.Pagos.Remove(pago);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Indice));
    }
}