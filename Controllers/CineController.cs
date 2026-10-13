using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Proyecto_Final.Models;
using Proyecto_Final.Data;

namespace Proyecto_Final.Controllers;

public class CineController : Controller {
    private readonly BD_Context _context;
    public CineController(BD_Context context) { _context = context; }

    // -----------------------------------------------------------------------------------
    // Funciones para Índice
    public async Task<IActionResult> Indice() { return View(await _context.Cines.AsNoTracking().ToListAsync()); }

    // -----------------------------------------------------------------------------------
    // Funciones para Crear
    [HttpGet]
    public IActionResult Crear() { return View(); }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(Cine cine) {
        if (!ModelState.IsValid) return View(cine);

        _context.Cines.Add(cine);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Indice));
    }

    // -----------------------------------------------------------------------------------
    // Funciones para Editar
    [HttpGet]
    public async Task<IActionResult> Editar(int id) {
        var cine = await _context.Cines.FindAsync(id);
        if (cine == null) return NotFound();
        return View(cine);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, Cine cine) {
        if (id != cine.ID) return BadRequest();
        if (!ModelState.IsValid) return View(cine);

        _context.Cines.Update(cine);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Indice));
    }

    // -----------------------------------------------------------------------------------
    // Funciones para Eliminar
    [HttpGet]
    public async Task<IActionResult> Eliminar(int id) {
        var cine = await _context.Cines.FindAsync(id);
        if (cine == null) return NotFound();
        return View(cine);
    }

    [HttpPost, ValidateAntiForgeryToken, ActionName("Eliminar")]
    public async Task<IActionResult> ConfirmarEliminar(int id) {
        var cine = await _context.Cines.FindAsync(id);
        if (cine == null) return NotFound();

        _context.Cines.Remove(cine);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Indice));
    }
}