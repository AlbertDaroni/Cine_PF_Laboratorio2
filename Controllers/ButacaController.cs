using Microsoft.AspNetCore.Mvc;
using Proyecto_Final.Models;
using Proyecto_Final.Data;

namespace Proyecto_Final.Controllers;

public class ButacaController : Controller {
    private readonly BD_Context _context;
    public ButacaController(BD_Context context) { this._context = context; }

    // -----------------------------------------------------------------------------------
    // Funciones para Índice
    [HttpGet]
    public async Task<IActionResult> Indice() { return View(await _context.Butacas.ToListAsync()); }

    // -----------------------------------------------------------------------------------
    // Funciones para Editar
    [HttpGet]
    public async Task<IActionResult> Editar(int id) {
        var butaca = await _context.Butacas.FindAsync(id);
        if (butaca == null) return NotFound();
        return View(butaca);
    }

    [HttpPost]
    public async Task<IActionResult> Editar(int id, Butaca butaca) {
        if (id != butaca.ID) return BadRequest();
        if (!ModelState.IsValid) return View(butaca);

        _context.Butacas.Update(butaca);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Indice));
    }
}