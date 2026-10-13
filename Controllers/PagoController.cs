using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Proyecto_Final.Models;
using Proyecto_Final.Data;

namespace Proyecto_Final.Controllers;

public class PagoController : Controller {
    private readonly BD_Context _context;
    public PagoController(BD_Context context) { _context = context; }

    // -----------------------------------------------------------------------------------
    // Funciones para Índice
    public async Task<IActionResult> Indice() { return View(await _context.Pagos.AsNoTracking().ToListAsync()); }

    // -----------------------------------------------------------------------------------
    // Funciones para Crear
    [HttpGet]
    public IActionResult Crear() { return View(); }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(Pago pago) {
        if (!ModelState.IsValid) return View(pago);

        _context.Pagos.Add(pago);
        await _context.SaveChanges();

        return RedirectToAction(nameof(Indice));
    }

    // -----------------------------------------------------------------------------------
    // Funciones para Editar
    [HttpGet]
    public async Task<IActionResult> Editar(int id) {
        var pago = await _context.Pagos.Find(id);
        if (pago == null) return NotFound();
        return View(pago);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, Pago pago) {
        if (id != pago.ID) return BadRequest();
        if (!ModelState.IsValid) return View(pago);

        _context.Pagos.Update(pago);
        await _context.SaveChanges();

        return RedirectToAction(nameof(Indice));
    }

    // -----------------------------------------------------------------------------------
    // Funciones para Eliminar
    [HttpGet]
    public async Task<IActionResult> Eliminar(int id) {
        var pago = await _context.Pagos.Find(id);
        if (pago == null) return NotFound();
        return View(pago);
    }

    [HttpPost, ValidateAntiForgeryToken, ActionName("Eliminar")]
    public async Task<IActionResult> ConfirmarEliminar(int id) {
        var pago = _context.Pagos.Find(id);
        if (pago == null) return NotFound();

        _context.Pagos.Remove(pago);
        await _context.SaveChanges();

        return RedirectToAction(nameof(Indice));
    }
}