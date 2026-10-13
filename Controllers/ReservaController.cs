using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Proyecto_Final.Models;
using Proyecto_Final.Data;

namespace Proyecto_Final.Controllers;

public class ReservaController : Controller {
    private readonly BD_Context _context;
    public ReservaController(BD_Context context) { _context = context; }

    // -----------------------------------------------------------------------------------
    // Funciones para Índice
    public async Task<IActionResult> Indice() { return View(await _context.Reservas.AsNoTracking().ToListAsync()); }

    // -----------------------------------------------------------------------------------
    // Funciones para Crear
    [HttpGet]
    public IActionResult Crear() { return View(); }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(Reserva reserva) {
        if (!ModelState.IsValid) return View(reserva);

        _context.Reservas.Add(reserva);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Indice));
    }

    // -----------------------------------------------------------------------------------
    // Funciones para Editar
    [HttpGet]
    public async Task<IActionResult> Editar(int id) {
        var reserva = await _context.Reservas.FindAsync(id);
        if (reserva == null) return NotFound();
        return View(reserva);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, Reserva reserva) {
        if (id != reserva.ID) return BadRequest();
        if (!ModelState.IsValid) return View(reserva);

        _context.Reservas.Update(reserva);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Indice));
    }

    // -----------------------------------------------------------------------------------
    // Funciones para Eliminar
    [HttpGet]
    public async Task<IActionResult> Eliminar(int id) {
        var reserva = await _context.Reservas.FindAsync(id);
        if (reserva == null) return NotFound();
        return View(reserva);
    }

    [HttpPost, ValidateAntiForgeryToken, ActionName("Eliminar")]
    public async Task<IActionResult> ConfirmarEliminar(int id) {
        var reserva = await _context.Reservas.FindAsync(id);
        if (reserva == null) return NotFound();

        _context.Reservas.Remove(reserva);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Indice));
    }
}