using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Final.Data;
using Proyecto_Final.Models;

namespace Proyecto_Final.Controllers;

[ApiController]
[Route("api/generos")]
[Authorize(Roles = "Administrador,Empleado")]
public class GeneroApiController : ControllerBase {
    private readonly BD_Context _context;

    public GeneroApiController(BD_Context context) { _context = context; }

    // GET: api/generos
    [HttpGet]
    public async Task<IActionResult> Listar() {
        var generos = await _context.Generos
            .AsNoTracking()
            .OrderBy(g => g.Nombre)
            .Select(g => new {
                g.ID,
                g.Nombre
            })
            .ToListAsync()
        ;

        return Ok(generos);
    }

    
    // POST: api/generos
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear([FromBody] GeneroRequest request) {
        string nombre = request.Nombre?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(nombre)) return BadRequest(new { mensaje = "El nombre es obligatorio." });
        if (nombre.Length > 50) return BadRequest(new { mensaje = "El nombre no puede superar los 50 caracteres." });

        bool existe = await _context.Generos.AnyAsync(g => g.Nombre.ToLower() == nombre.ToLower());
        if (existe) return Conflict(new { mensaje = "Ya existe un género con ese nombre." });

        var genero = new Genero {
            Nombre = nombre
        };

        _context.Generos.Add(genero);
        await _context.SaveChangesAsync();

        return Ok(new { mensaje = "Género creado correctamente.", genero.ID, genero.Nombre });
    }

    public class GeneroRequest {
        public string? Nombre { get; set; }
    }

    
    // PUT: api/generos/5
    [HttpPut("{id}"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, [FromBody] GeneroRequest request) {
        var genero = await _context.Generos.FindAsync(id);
        if (genero == null) return NotFound(new { mensaje = "El género no existe." });

        string nombre = request.Nombre?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(nombre)) return BadRequest(new { mensaje = "El nombre es obligatorio." });
        if (nombre.Length > 50) return BadRequest(new { mensaje = "El nombre no puede superar los 50 caracteres." });

        bool existe = await _context.Generos.AnyAsync(g => g.ID != id && g.Nombre.ToLower() == nombre.ToLower());
        if (existe) return Conflict(new { mensaje = "Ya existe otro género con ese nombre." });

        genero.Nombre = nombre;

        await _context.SaveChangesAsync();

        return Ok(new { mensaje = "Género actualizado correctamente.", genero.ID, genero.Nombre });
    }

    // DELETE: api/generos/5
    [HttpDelete("{id}"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Eliminar(int id) {
        var genero = await _context.Generos.FindAsync(id);
        if (genero == null) return NotFound(new { mensaje = "El género no existe." });

        // Verificar si tiene películas asociadas
        bool tienePeliculas = await _context.Generos
            .Where(g => g.ID == id)
            .SelectMany(g => g.Peliculas)
            .AnyAsync()
        ;

        if (tienePeliculas) return Conflict(new { mensaje = "No se puede eliminar el género porque tiene películas asociadas." });

        _context.Generos.Remove(genero);
        await _context.SaveChangesAsync();

        return Ok(new { mensaje = "Género eliminado correctamente." });
    }
}