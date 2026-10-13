using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Proyecto_Final.Models;
using Proyecto_Final.Data;

namespace Proyecto_Final.Controllers;

public class PeliculaController : Controller {
    private readonly BD_Context _context;
    public PeliculaController(BD_Context context) { _context = context; }

    // -----------------------------------------------------------------------------------
    // Funciones para Índice
    public async Task<IActionResult> Indice() { return View(await _context.Peliculas.AsNoTracking().ToListAsync()); }

    // -----------------------------------------------------------------------------------
    // Funciones para Crear
    [HttpGet]
    public IActionResult Crear() { return View(); }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(Pelicula pelicula) {
        if (!ModelState.IsValid) return View(pelicula);

        _context.Peliculas.Add(pelicula);
        await _context.SaveChanges();

        return RedirectToAction(nameof(Indice));
    }

    // -----------------------------------------------------------------------------------
    // Funciones para Editar
    [HttpGet]
    public async Task<IActionResult> Editar(int id) {
        var pelicula = await _context.Peliculas.Find(id);
        if (pelicula == null) return NotFound();
        return View(pelicula);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, Pelicula pelicula) {
        if (id != pelicula.ID) return BadRequest();
        if (!ModelState.IsValid) return View(pelicula);

        _context.Peliculas.Update(pelicula);
        await _context.SaveChanges();

        return RedirectToAction(nameof(Indice));
    }

    // -----------------------------------------------------------------------------------
    // Funciones para Eliminar
    [HttpGet]
    public async Task<IActionResult> Eliminar(int id) {
        var pelicula = await _context.Peliculas.Find(id);
        if (pelicula == null) return NotFound();
        return View(pelicula);
    }

    [HttpPost, ValidateAntiForgeryToken, ActionName("Eliminar")]
    public async Task<IActionResult> ConfirmarEliminar(int id) {
        var pelicula = _context.Peliculas.Find(id);
        if (pelicula == null) return NotFound();

        _context.Peliculas.Remove(pelicula);
        await _context.SaveChanges();

        return RedirectToAction(nameof(Indice));
    }
}