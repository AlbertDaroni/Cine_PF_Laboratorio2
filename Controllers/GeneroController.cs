
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Proyecto_Final.Models;
using Proyecto_Final.Data;

namespace Proyecto_Final.Controllers;

[Authorize(Roles = "Administrador, Empleado")]
public class GeneroController : Controller
{
    private readonly BD_Context _context;

    public GeneroController(BD_Context context)
    {
        _context = context;
    }

    // Listado
    [HttpGet]
    public async Task<IActionResult> Indice()
    {
        var generos = await _context.Generos
            .AsNoTracking()
            .OrderBy(g => g.Nombre)
            .ToListAsync();

        return View(generos);
    }

    // Crear - GET
    [HttpGet]
    public IActionResult Crear()
    {
        return View(new Genero());
    }

    // Crear - POST
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(
        [Bind("Nombre")] Genero genero)
    {
        genero.Nombre = genero.Nombre?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(genero.Nombre))
        {
            ModelState.AddModelError(
                nameof(Genero.Nombre),
                "El nombre del género es obligatorio.");
        }

        bool existe = await _context.Generos
            .AnyAsync(g => g.Nombre.ToLower() ==
                           genero.Nombre.ToLower());

        if (existe)
        {
            ModelState.AddModelError(
                nameof(Genero.Nombre),
                "Ya existe un género con ese nombre.");
        }

        if (!ModelState.IsValid)
            return View(genero);

        _context.Generos.Add(genero);
        await _context.SaveChangesAsync();

        TempData["Mensaje"] = "Género creado correctamente.";

        return RedirectToAction(nameof(Indice));
    }

    // Editar - GET
    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        var genero = await _context.Generos.FindAsync(id);

        if (genero == null)
            return NotFound();

        return View(genero);
    }

    // Editar - POST
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(
        int id, [Bind("ID,Nombre")] Genero datos)
    {
        if (id != datos.ID)
            return BadRequest();

        datos.Nombre = datos.Nombre?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(datos.Nombre))
        {
            ModelState.AddModelError(
                nameof(Genero.Nombre),
                "El nombre del género es obligatorio.");
        }

        bool existe = await _context.Generos
            .AnyAsync(g => g.ID != id &&
                g.Nombre.ToLower() == datos.Nombre.ToLower());

        if (existe)
        {
            ModelState.AddModelError(
                nameof(Genero.Nombre),
                "Ya existe otro género con ese nombre.");
        }

        if (!ModelState.IsValid)
            return View(datos);

        var genero = await _context.Generos.FindAsync(id);

        if (genero == null)
            return NotFound();

        genero.Nombre = datos.Nombre;

        await _context.SaveChangesAsync();

        TempData["Mensaje"] = "Género actualizado correctamente.";

        return RedirectToAction(nameof(Indice));
    }

    // Eliminar - GET
    [HttpGet]
    public async Task<IActionResult> Eliminar(int id)
    {
        var genero = await _context.Generos
            .AsNoTracking()
            .FirstOrDefaultAsync(g => g.ID == id);

        if (genero == null)
            return NotFound();

        ViewBag.TienePeliculas = await _context.Generos
            .Where(g => g.ID == id)
            .SelectMany(g => g.Peliculas)
            .AnyAsync();

        return View(genero);
    }

    // Eliminar - POST
    [HttpPost, ValidateAntiForgeryToken, ActionName("Eliminar")]
    public async Task<IActionResult> ConfirmarEliminar(int id)
    {
        var genero = await _context.Generos.FindAsync(id);

        if (genero == null)
            return NotFound();

        bool tienePeliculas = await _context.Generos
            .Where(g => g.ID == id)
            .SelectMany(g => g.Peliculas)
            .AnyAsync();

        if (tienePeliculas)
        {
            ViewBag.TienePeliculas = true;

            ModelState.AddModelError("",
                "No se puede eliminar este género porque está asociado a películas.");

            return View("Eliminar", genero);
        }

        _context.Generos.Remove(genero);
        await _context.SaveChangesAsync();

        TempData["Mensaje"] = "Género eliminado correctamente.";

        return RedirectToAction(nameof(Indice));
    }

    
    [HttpGet]
    public IActionResult Vue()
    {
        return View();
    }


}
