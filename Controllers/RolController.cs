using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Proyecto_Final.Models;
using Proyecto_Final.Data;

namespace Proyecto_Final.Controllers;

public class RolController : Controller {
    private readonly BD_Context _context;
    public RolController(BD_Context context) { _context = context; }

    // -----------------------------------------------------------------------------------
    // Funciones para Índice
    public async Task<IActionResult> Indice() { return View(await _context.Roles.AsNoTracking().ToListAsync()); }

    // -----------------------------------------------------------------------------------
    // Funciones para Crear
    [HttpGet]
    public IActionResult Crear() { return View(); }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(Rol rol) {
        if (!ModelState.IsValid) return View(rol);

        _context.Roles.Add(rol);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Indice));
    }

    // -----------------------------------------------------------------------------------
    // Funciones para Editar
    [HttpGet]
    public async Task<IActionResult> Editar(int id) {
        var rol = await _context.Roles.FindAsync(id);
        if (rol == null) return NotFound();
        return View(rol);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, Rol rol) {
        if (id != rol.ID) return BadRequest();
        if (!ModelState.IsValid) return View(rol);

        _context.Roles.Update(rol);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Indice));
    }

    // -----------------------------------------------------------------------------------
    // Funciones para Eliminar
    [HttpGet]
    public async Task<IActionResult> Eliminar(int id) {
        var rol = await _context.Roles.FindAsync(id);
        if (rol == null) return NotFound();
        return View(rol);
    }

    [HttpPost, ValidateAntiForgeryToken, ActionName("Eliminar")]
    public async Task<IActionResult> ConfirmarEliminar(int id) {
        var rol = await _context.Roles.FindAsync(id);
        if (rol == null) return NotFound();

        _context.Roles.Remove(rol);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Indice));
    }
}