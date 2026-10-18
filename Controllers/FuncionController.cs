using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Proyecto_Final.Models;
using Proyecto_Final.Data;

namespace Proyecto_Final.Controllers;

[Authorize]
public class FuncionController : Controller {
    private readonly BD_Context _context;
    public FuncionController(BD_Context _context) { _context = _context; }

    // -----------------------------------------------------------------------------------
    // Funciones para Índice
    [HttpGet]
    public async Task<IActionResult> Indice(int id) {
        var funciones = await _context.Funciones
            .Include(f => f.Pelicula)
            .Include(f => f.Sala)
            .Where(f => f.PeliculaID == id)
            .AsNoTracking().ToListAsync();

        if (funciones == null || !funciones.Any()) return NotFound();
        return View(funciones);
    }

    // -----------------------------------------------------------------------------------
    // Funciones para Obtener
    [HttpGet]
    public async Task<IActionResult> Detalles(int id) {
        var funcion = await _context.Funciones.Include(f => f.Pelicula).Include(f => f.Sala).FirstOrDefaultAsync(f => f.ID == id);
        if (funcion == null) return NotFound();
        return View(funcion);
    }

    // -----------------------------------------------------------------------------------
    // Funciones para Crear
    [HttpGet, Authorize(Roles = "Administrador, Empleado")]
    public IActionResult Crear(int PeliculaID, int SalaID) {
        ViewBag.PeliculaID = PeliculaID;
        ViewBag.SalaID = SalaID;
        
        return View();
    }

    [HttpPost, Authorize(Roles = "Administrador, Empleado")]
    public async Task<IActionResult> Crear(Funcion funcion) {
        if (!ModelState.IsValid) return View(funcion);

        _context.Funciones.Add(funcion);
        await _context.SaveChangesAsync();

        return RedirectToAction("Indice", "Pelicula");
    }

    // -----------------------------------------------------------------------------------
    // Funciones para Editar
    [HttpGet, Authorize(Roles = "Administrador, Empleado")]
    public async Task<IActionResult> Editar(int id) {
        var funcion = await _context.Funciones.Include(f => f.Pelicula).Include(f => f.Sala).FirstOrDefaultAsync(f => f.ID == id);
        if (funcion == null) return NotFound();
        return View(funcion);
    }

    [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Administrador, Empleado")]
    public async Task<IActionResult> Editar(int id, Funcion funcion) {
        if (id != funcion.ID) return BadRequest();
        if (!ModelState.IsValid) return View(funcion);

        _context.Funciones.Update(funcion);
        await _context.SaveChangesAsync();

        return RedirectToAction("Indice", "Pelicula");
    }

    // -----------------------------------------------------------------------------------
    // Funciones para Eliminar
    [HttpGet, Authorize(Roles = "Administrador, Empleado")]
    public async Task<IActionResult> Eliminar(int id) {
        var funcion = await _context.Funciones.Include(f => f.Pelicula).Include(f => f.Sala).FirstOrDefaultAsync(f => f.ID == id);
        if (funcion == null) return NotFound();
        return View(funcion);
    }

    [HttpPost, ValidateAntiForgeryToken, ActionName("Eliminar"), Authorize(Roles = "Administrador, Empleado")]
    public async Task<IActionResult> ConfirmarEliminar(int id) {
        var funcion = await _context.Funciones.FindAsync(id);
        if (funcion == null) return NotFound();

        _context.Funciones.Remove(funcion);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Indice));
    }
}