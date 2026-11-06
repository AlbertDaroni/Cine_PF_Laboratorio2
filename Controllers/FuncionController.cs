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

    [HttpGet]
    public async Task<IActionResult> BuscarPeliculas(string? termino) {
        termino = termino?.Trim();

        if (string.IsNullOrEmpty(termino) || termino.Length < 2) return Json(Array.Empty<object>());
        if (termino.Length > 100) return BadRequest("La búsqueda es demasiado larga.");

        var peliculas = await _context.Peliculas
            .AsNoTracking()
            .Where(p => p.Titulo.Contains(termino))
            .OrderBy(p => p.Titulo)
            .ThenBy(p => p.ID)
            .Select(p => new {
                id = p.ID,
                nombre = p.Titulo
            })
            .Take(10)
            .ToListAsync()
        ;

        return Json(peliculas);
    }

    [HttpGet]
    public async Task<IActionResult> BuscarSalas(string? termino) {
        termino = termino?.Trim();

        if (string.IsNullOrEmpty(termino) || termino.Length < 2) return Json(Array.Empty<object>());
        if (termino.Length > 100) return BadRequest("La búsqueda es demasiado larga.");

        var salas = await _context.Salas
            .AsNoTracking()
            .Where(s => s.Estado
                && s.Cine != null
                && (
                    s.Cine.Nombre.Contains(termino)
                    || s.Tipo.Contains(termino)
                    || s.Numero.ToString() == termino
                ))
            .OrderBy(s => s.Cine!.Nombre)
            .ThenBy(s => s.Numero)
            .ThenBy(s => s.ID)
            .Select(s => new {
                id = s.ID,
                cine = s.Cine!.Nombre,
                numero = s.Numero,
                tipo = s.Tipo,
                capacidad = s.Capacidad
            })
            .Take(10)
            .ToListAsync()
        ;

        return Json(salas);
    }

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
    [HttpGet]
    public IActionResult Crear() {
        return View(new Funcion {
            FechaHora = DateTime.Now.AddDays(1),
            Estado = true
        });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear([Bind("FechaHora,Precio,Estado,PeliculaID,SalaID")] Funcion funcion) {
        var pelicula = await _context.Peliculas
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.ID == funcion.PeliculaID)
        ;

        var sala = await _context.Salas
            .AsNoTracking()
            .Include(s => s.Cine)
            .FirstOrDefaultAsync(s => s.ID == funcion.SalaID)
        ;

        if (pelicula == null) ModelState.AddModelError("PeliculaID", "Seleccioná una película válida.");
        if (sala == null || !sala.Estado) ModelState.AddModelError("SalaID", "Seleccioná una sala activa.");
        if (funcion.FechaHora <= DateTime.Now) ModelState.AddModelError("FechaHora", "La función debe tener una fecha futura.");
        if (funcion.Precio <= 0) ModelState.AddModelError("Precio", "El precio debe ser mayor que cero.");

        ViewBag.PeliculaSeleccionada = pelicula?.Titulo;
        ViewBag.SalaSeleccionada = sala == null ? "" : $"{sala.Cine?.Nombre} — Sala {sala.Numero} ({sala.Tipo})";

        if (!ModelState.IsValid) return View(funcion);

        _context.Funciones.Add(funcion);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Indice), new { PeliculaID = funcion.PeliculaID, CineID = sala!.CineID });
    }

    // -----------------------------------------------------------------------------------
    // Funciones para Editar
    [HttpGet]
    public async Task<IActionResult> Editar(int id) {
        var funcion = await _context.Funciones
            .AsNoTracking()
            .Include(f => f.Pelicula)
            .Include(f => f.Sala)
                .ThenInclude(s => s!.Cine)
            .FirstOrDefaultAsync(f => f.ID == id)
        ;

        if (funcion == null) return NotFound();

        ViewBag.PeliculaSeleccionada = funcion.Pelicula?.Titulo;
        ViewBag.SalaSeleccionada = funcion.Sala == null ? "" : $"{funcion.Sala.Cine?.Nombre} — Sala {funcion.Sala.Numero} ({funcion.Sala.Tipo})";

        return View(funcion);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, [Bind("ID,FechaHora,Precio,Estado,PeliculaID,SalaID")] Funcion funcion) {
        if (id != funcion.ID) return BadRequest();

        var original = await _context.Funciones.FirstOrDefaultAsync(f => f.ID == id);
        if (original == null) return NotFound();

        var pelicula = await _context.Peliculas
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.ID == funcion.PeliculaID)
        ;

        var sala = await _context.Salas
            .AsNoTracking()
            .Include(s => s.Cine)
            .FirstOrDefaultAsync(s => s.ID == funcion.SalaID)
        ;

        if (pelicula == null) ModelState.AddModelError("PeliculaID", "Seleccioná una película válida.");
        if (sala == null || (!sala.Estado && sala.ID != original.SalaID)) ModelState.AddModelError("SalaID", "Seleccioná una sala activa.");
        if (funcion.Precio <= 0) ModelState.AddModelError("Precio", "El precio debe ser mayor que cero.");
        if (funcion.FechaHora != original.FechaHora && funcion.FechaHora <= DateTime.Now) ModelState.AddModelError("FechaHora", "La nueva fecha debe ser futura.");

        bool tieneReservas = await _context.Reservas.AnyAsync(r => r.FuncionID == id);
        if (tieneReservas && (funcion.PeliculaID != original.PeliculaID || funcion.SalaID != original.SalaID || funcion.FechaHora != original.FechaHora)) {
            ModelState.AddModelError("", "Esta función tiene reservas. No se puede cambiar su película, sala ni fecha y hora.");
        }

        ViewBag.PeliculaSeleccionada = pelicula?.Titulo;
        ViewBag.SalaSeleccionada = sala == null ? "" : $"{sala.Cine?.Nombre} — Sala {sala.Numero} ({sala.Tipo})";

        if (!ModelState.IsValid) return View(funcion);

        original.PeliculaID = funcion.PeliculaID;
        original.SalaID = funcion.SalaID;
        original.FechaHora = funcion.FechaHora;
        original.Precio = funcion.Precio;
        original.Estado = funcion.Estado;

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Indice), new { PeliculaID = original.PeliculaID, CineID = sala!.CineID });
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
}