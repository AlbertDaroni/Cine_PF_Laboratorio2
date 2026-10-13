using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Proyecto_Final.Models;
using Proyecto_Final.Data;

namespace Proyecto_Final.Controllers;

public class SalaController : Controller {
    private readonly BD_Context _context;
    public SalaController(BD_Context context) { _context = context; }
    
    // -----------------------------------------------------------------------------------
    // Funciones para Índice
    public async Task<IActionResult> Indice() { return View(await _context.Salas.AsNoTracking().ToListAsync()); }

    // -----------------------------------------------------------------------------------
    // Funciones para Crear
    [HttpGet]
    public IActionResult Crear() { return View(); }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(Sala sala) {
        if (!ModelState.IsValid) return View(sala);

        _context.Salas.Add(sala);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Indice));
    }

    // -----------------------------------------------------------------------------------
    // Funciones para Editar
    [HttpGet]
    public async Task<IActionResult> Editar(int id) {
        var sala = await _context.Salas.FindAsync(id);
        if (sala == null) return NotFound();
        return View(sala);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, Sala sala) {
        if (id != sala.ID) return BadRequest();
        if (!ModelState.IsValid) return View(sala);

        _context.Salas.Update(sala);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Indice));
    }

    // -----------------------------------------------------------------------------------
    // Funciones para Eliminar
    [HttpGet]
    public async Task<IActionResult> Eliminar(int id) {
        var sala = await _context.Salas.FindAsync(id);
        if (sala == null) return NotFound();
        return View(sala);
    }

    [HttpPost, ValidateAntiForgeryToken, ActionName("Eliminar")]
    public async Task<IActionResult> ConfirmarEliminar(int id) {
        var sala = await _context.Salas.FindAsync(id);
        if (sala == null) return NotFound();

        _context.Salas.Remove(sala);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Indice));
    }
}