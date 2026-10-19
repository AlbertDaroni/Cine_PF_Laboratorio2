using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Proyecto_Final.Models;
using Proyecto_Final.Data;

namespace Proyecto_Final.Controllers;

public class PeliculaController : Controller {
    private readonly BD_Context _context;
    private readonly IWebHostEnvironment _environment;
    public PeliculaController(BD_Context context, IWebHostEnvironment _environment) { _context = context; _environment = _environment; }

    // -----------------------------------------------------------------------------------
    // Funciones para Índice
    public async Task<IActionResult> Indice() {
        var peliculas = await _context.Peliculas
            .Include(p => p.Directores)
            .Include(p => p.Generos)
            .AsNoTracking()
            .ToListAsync()
        ;

        return View(peliculas);
    }

    // -----------------------------------------------------------------------------------
    // Funciones para Crear
    [HttpGet]
    public IActionResult Crear() { return View(); }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(Pelicula pelicula, IFormFile portada) {
        if (!ModelState.IsValid || portada == null) return View(pelicula);

        string nombre = Guid.NewGuid().ToString() + "_" + Path.GetFileName(portada.FileName);
        string direccion = Path.Combine(_environment.WebRootPath, "uploads", "portada");
        string direccionCompleta = Path.Combine(direccion, nombre);

        using (var stream = new FileStream(direccionCompleta, FileMode.Create)) { await portada.CopyToAsync(stream); };
        pelicula.Portada = nombre;

        _context.Peliculas.Add(pelicula);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Indice));
    }

    // -----------------------------------------------------------------------------------
    // Funciones para Editar
    [HttpGet]
    public async Task<IActionResult> Editar(int id) {
        var pelicula = await _context.Peliculas.Include(p => p.Directores).Include(p => p.Generos).FirstOrDefaultAsync(p => p.ID == id);
        if (pelicula == null) return NotFound();
        return View(pelicula);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, Pelicula pelicula, IFormFile? nuevaPortada) {
        if (id != pelicula.ID) return BadRequest();
        if (!ModelState.IsValid) return View(pelicula);

        try {
            var peliculaOriginal = await _context.Peliculas.AsNoTracking().FirstOrDefaultAsync(p => p.ID == id);
            if (peliculaOriginal == null) return NotFound();

            pelicula.Portada = peliculaOriginal.Portada;

            if (nuevaPortada != null && nuevaPortada.Length > 0) {
                if (!string.IsNullOrEmpty(peliculaOriginal.Portada) && peliculaOriginal.Portada != "no_image.png") {
                    string rutaPortadaVieja = Path.Combine(_environment.WebRootPath, "uploads", "portada", peliculaOriginal.Portada);

                    if (System.IO.File.Exists(rutaPortadaVieja)) System.IO.File.Delete(rutaPortadaVieja);
                }

                string nombre = Guid.NewGuid().ToString() + "_" + Path.GetFileName(nuevaPortada.FileName);
                string direccion = Path.Combine(_environment.WebRootPath, "uploads", "portada");
                string direccionCompleta = Path.Combine(direccion, nombre);

                using (var stream = new FileStream(direccionCompleta, FileMode.Create)) { await nuevaPortada.CopyToAsync(stream); };
                pelicula.Portada = nombre;
            }

            _context.Peliculas.Update(pelicula);
            await _context.SaveChangesAsync();
        } catch (DbUpdateConcurrencyException) {
            if (!_context.Peliculas.Any(p => p.ID == pelicula.ID)) return NotFound();
            else throw;
        }

        return RedirectToAction(nameof(Indice));
    }

    // -----------------------------------------------------------------------------------
    // Funciones para Eliminar
    [HttpGet]
    public async Task<IActionResult> Eliminar(int id) {
        var pelicula = await _context.Peliculas.Include(p => p.Directores).Include(p => p.Generos).FirstOrDefaultAsync(p => p.ID == id);
        if (pelicula == null) return NotFound();
        return View(pelicula);
    }

    [HttpPost, ValidateAntiForgeryToken, ActionName("Eliminar")]
    public async Task<IActionResult> ConfirmarEliminar(int id) {
        var pelicula = await _context.Peliculas.FindAsync(id);
        if (pelicula == null) return NotFound();

        _context.Peliculas.Remove(pelicula);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Indice));
    }
}