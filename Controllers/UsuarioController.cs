using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proyecto_Final.Models;
using Proyecto_Final.Data;

namespace Proyecto_Final.Controllers;

[Authorize]
public class UsuarioController : Controller {
    private readonly BD_Context _context;
    private readonly IWebHostEnvironment _environment;
    public UsuarioController(BD_Context context, IWebHostEnvironment _environment) { _context = context; this._environment = _environment; }

    // -----------------------------------------------------------------------------------
    // Funciones para Índice
    public async Task<IActionResult> Indice() { return View(await _context.Usuarios.Include(u => u.Rol).AsNoTracking().ToListAsync()); }

    // -----------------------------------------------------------------------------------
    // Funciones para Obtener
    [HttpGet]
    public async Task<IActionResult> Detalles(int id) {
        var usuario = await _context.Usuarios.Include(u => u.Rol).FirstOrDefaultAsync(u => u.ID == id);
        if (usuario == null) return NotFound();
        return View(usuario);
    }

    // -----------------------------------------------------------------------------------
    // Funciones para Crear
    [HttpGet]
    public async Task<IActionResult> Crear() {
        ViewBag.Roles = new SelectList(await _context.Roles.ToListAsync(), "ID", "Nombre");
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(Usuario usuario) {
        if (!ModelState.IsValid) return View(usuario);

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Indice));
    }

    // -----------------------------------------------------------------------------------
    // Funciones para Editar
    [HttpGet]
    public async Task<IActionResult> Editar(int id) {
        var usuario = await _context.Usuarios.Include(u => u.Rol).FirstOrDefaultAsync(u => u.ID == id);
        if (usuario == null) return NotFound();

        ViewBag.Roles = new SelectList(await _context.Roles.ToListAsync(), "ID", "Nombre");
        return View(usuario);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, Usuario usuario, IFormFile? foto_perfil) {
        if (id != usuario.ID) return BadRequest();
        if (!ModelState.IsValid) {
            ViewBag.Roles = new SelectList(await _context.Roles.ToListAsync(), "ID", "Nombre");
            return View(usuario);
        }

        try {
            var usuarioOriginal = await _context.Usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.ID == id);
            if (usuarioOriginal == null) return NotFound();

            usuario.Foto_Perfil = usuarioOriginal.Foto_Perfil;

            if (foto_perfil != null && foto_perfil.Length > 0) {
                if (!string.IsNullOrEmpty(usuarioOriginal.Foto_Perfil) && usuarioOriginal.Foto_Perfil != "image.png") {
                    string rutaFotoVieja = Path.Combine(_environment.WebRootPath, "uploads", "profile_picture", usuarioOriginal.Foto_Perfil);

                    if (System.IO.File.Exists(rutaFotoVieja)) System.IO.File.Delete(rutaFotoVieja);
                }

                string nombre = Guid.NewGuid().ToString() + "_" + Path.GetFileName(foto_perfil.FileName);
                string direccion = Path.Combine(_environment.WebRootPath, "uploads", "profile_picture");
                string direccionCompleta = Path.Combine(direccion, nombre);

                using (var stream = new FileStream(direccionCompleta, FileMode.Create)) { await foto_perfil.CopyToAsync(stream); };
                usuario.Foto_Perfil = nombre;
            }

            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
        } catch (DbUpdateConcurrencyException) {
            if (!_context.Usuarios.Any(u => u.ID == usuario.ID)) return NotFound();
            else throw;
        }

        return RedirectToAction(nameof(Indice));
    }

    // -----------------------------------------------------------------------------------
    // Funciones para Eliminar
    [HttpGet]
    public async Task<IActionResult> Eliminar(int id) {
        var usuario = await _context.Usuarios.Include(u => u.Rol).FirstOrDefaultAsync(u => u.ID == id);
        if (usuario == null) return NotFound();
        return View(usuario);
    }

    [HttpPost, ValidateAntiForgeryToken, ActionName("Eliminar")]
    public async Task<IActionResult> ConfirmarEliminar(int id) {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null) return NotFound();

        _context.Usuarios.Remove(usuario);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Indice));
    }
}