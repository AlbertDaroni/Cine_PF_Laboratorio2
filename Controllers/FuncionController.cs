using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Proyecto_Final.Models;
using Proyecto_Final.Data;

namespace Proyecto_Final.Controllers;

[Authorize(Roles = "Administrador, Empleado")]
public class FuncionController : Controller {
    private readonly BD_Context _context;
    public FuncionController(BD_Context _context) { this._context = _context; }

    // -----------------------------------------------------------------------------------
    // Funciones para Índice
    [HttpGet, AllowAnonymous]
    public async Task<IActionResult> Indice(int PeliculaID, int CineID) {
        List<Funcion> funciones;
        ViewBag.PeliculaID = PeliculaID;
        ViewBag.CineID = CineID;

        funciones = await _context.Funciones
            .Include(f => f.Sala)
                .ThenInclude(s => s!.Cine)
            .Where(f => f.PeliculaID == PeliculaID && f.Sala!.CineID == CineID)
            .AsNoTracking()
            .ToListAsync()
        ;

        if (funciones.Count == 0) {
            funciones = await _context.Funciones
                .Include(f => f.Sala)
                    .ThenInclude(s => s!.Cine)
                .Where(f => f.PeliculaID == PeliculaID)
                .AsNoTracking()
                .ToListAsync()
            ;
        }

        return View(funciones);
    }

    // -----------------------------------------------------------------------------------
    // Funciones para Obtener
    [HttpGet, AllowAnonymous]
    public async Task<IActionResult> Detalles(int id) {
        var funcion = await _context.Funciones.Include(f => f.Pelicula).Include(f => f.Sala).FirstOrDefaultAsync(f => f.ID == id);
        if (funcion == null) return NotFound();
        return View(funcion);
    }

    // -----------------------------------------------------------------------------------
    // Funciones para Crear
    [HttpGet]
    public async Task<IActionResult> Crear(int PeliculaID, int CineID) {
        ViewBag.PeliculaID = PeliculaID;
        ViewBag.CineID = CineID;
        ViewBag.Salas = new SelectList(await _context.Salas.Where(s => s.CineID == CineID).ToListAsync(), "ID", "Numero");
        
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Crear(int PeliculaID, int CineID, Funcion funcion) {
        if (!ModelState.IsValid) {
            ViewBag.PeliculaID = PeliculaID;
            ViewBag.CineID = CineID;
            ViewBag.Salas = new SelectList(await _context.Salas.Where(s => s.CineID == CineID).ToListAsync(), "ID", "Numero");
            return View(funcion);
        }

        _context.Funciones.Add(funcion);
        await _context.SaveChangesAsync();

        return RedirectToAction("Indice", "Pelicula");
    }

    // -----------------------------------------------------------------------------------
    // Funciones para Editar
    [HttpGet]
    public async Task<IActionResult> Editar(int id) {
        var funcion = await _context.Funciones.Include(f => f.Pelicula).Include(f => f.Sala).FirstOrDefaultAsync(f => f.ID == id);
        if (funcion == null) return NotFound();
        return View(funcion);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, Funcion funcion) {
        if (id != funcion.ID) return BadRequest();
        if (!ModelState.IsValid) return View(funcion);

        _context.Funciones.Update(funcion);
        await _context.SaveChangesAsync();

        return RedirectToAction("Indice", "Pelicula");
    }

    // -----------------------------------------------------------------------------------
    // Funciones para Eliminar
    [HttpGet]
    public async Task<IActionResult> Eliminar(int id) {
        var funcion = await _context.Funciones.Include(f => f.Pelicula).Include(f => f.Sala).FirstOrDefaultAsync(f => f.ID == id);
        if (funcion == null) return NotFound();
        return View(funcion);
    }

    [HttpPost, ValidateAntiForgeryToken, ActionName("Eliminar")]
    public async Task<IActionResult> ConfirmarEliminar(int id) {
        var funcion = await _context.Funciones.FindAsync(id);
        if (funcion == null) return NotFound();

        _context.Funciones.Remove(funcion);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Indice));
    }

    [HttpGet]
    public async Task<IActionResult> CrearDesdeCartelera(int PeliculaID, int CineID) {
        ViewBag.PeliculaID = PeliculaID;
        ViewBag.CineID = CineID;
        ViewBag.Salas = new SelectList(await _context.Salas.Where(s => s.CineID == CineID).ToListAsync(), "ID", "Numero");

        return View(nameof(Crear));
    }
}