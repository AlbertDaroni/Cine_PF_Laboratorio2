using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Proyecto_Final.Models;
using Proyecto_Final.Data;

namespace Proyecto_Final.Controllers;

public class GeneroController : Controller {
    private readonly BD_Context _context;
    public GeneroController(BD_Context context) { this._context = context; }

    // -----------------------------------------------------------------------------------
    // Funciones para Índice
    public async Task<IActionResult> Indice() { return View(await _context.Generos.AsNoTracking().ToListAsync()); }

    // -----------------------------------------------------------------------------------
    // Funciones para Crear
    [HttpGet]
    public IActionResult Crear() { return View(); }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(Genero genero) {
        if (!ModelState.IsValid) return View(genero);

        _context.Generos.Add(genero);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Indice));
    }

    // -----------------------------------------------------------------------------------
    // Funciones para Editar
    [HttpGet]
    public async Task<IActionResult> Editar(int id) {
        var genero = await _context.Generos.FindAsync(id);
        if (genero == null) return NotFound();
        return View(genero);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, Genero genero) {
        if (id != genero.ID) return BadRequest();
        if (!ModelState.IsValid) return View(genero);

        _context.Generos.Update(genero);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Indice));
    }

    // -----------------------------------------------------------------------------------
    // Funciones para Eliminar
    [HttpGet]
    public async Task<IActionResult> Eliminar(int id) {
        var genero = await _context.Generos.FindAsync(id);
        if (genero == null) return NotFound();
        return View(genero);
    }

    [HttpPost, ValidateAntiForgeryToken, ActionName("Eliminar")]
    public async Task<IActionResult> ConfirmarEliminar(int id) {
        var genero = await _context.Generos.FindAsync(id);
        if (genero == null) return NotFound();

        _context.Generos.Remove(genero);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Indice));
    }
}