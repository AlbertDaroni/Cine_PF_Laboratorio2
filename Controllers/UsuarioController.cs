using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proyecto_Final.Models;
using Proyecto_Final.Data;
using System.Security.Claims;

namespace Proyecto_Final.Controllers;

[Authorize]
public class UsuarioController : Controller
{
    private readonly BD_Context _context;
    private readonly IWebHostEnvironment _environment;
    public UsuarioController(BD_Context context, IWebHostEnvironment _environment) { this._context = context; this._environment = _environment; }

    // -----------------------------------------------------------------------------------
    // Funciones para Índice
    [Authorize(Roles = "Administrador, Empleado")]
    public async Task<IActionResult> Indice() { return View(await _context.Usuarios.Include(u => u.Rol).AsNoTracking().ToListAsync()); }

    // -----------------------------------------------------------------------------------
    // Funciones para Obtener
    [HttpGet]
    public async Task<IActionResult> Detalles(int id)
    {
        if (!int.TryParse(
        User.FindFirstValue(ClaimTypes.NameIdentifier),
        out int usuarioID))
        {
            return Challenge();
        }

        bool esPersonal = User.IsInRole("Administrador")
            || User.IsInRole("Empleado");

        if (!esPersonal && id != usuarioID)
        {
            return Forbid();
        }

        var usuario = await _context.Usuarios
            .Include(u => u.Rol)
            .FirstOrDefaultAsync(u => u.ID == id);

        if (usuario == null)
            return NotFound();

        return View(usuario);
    }

    // -----------------------------------------------------------------------------------
    // Funciones para Crear
    [HttpGet, Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Crear()
    {
        ViewBag.Roles = new SelectList(await _context.Roles.ToListAsync(), "ID", "Nombre");
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Crear(Usuario usuario)
    {
        if (!ModelState.IsValid) return View(usuario);

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Indice));
    }

    // -----------------------------------------------------------------------------------
    // Funciones para Editar
    [HttpGet, Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Editar(int id)
    {
        var usuario = await _context.Usuarios.Include(u => u.Rol).FirstOrDefaultAsync(u => u.ID == id);
        if (usuario == null) return NotFound();

        ViewBag.Roles = new SelectList(await _context.Roles.ToListAsync(), "ID", "Nombre");
        return View(usuario);
    }

    [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Editar(int id, Usuario usuario, IFormFile? foto_perfil)
    {
        if (id != usuario.ID) return BadRequest();
        if (!ModelState.IsValid)
        {
            ViewBag.Roles = new SelectList(await _context.Roles.ToListAsync(), "ID", "Nombre");
            return View(usuario);
        }

        try
        {
            var usuarioOriginal = await _context.Usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.ID == id);
            if (usuarioOriginal == null) return NotFound();

            usuario.Foto_Perfil = usuarioOriginal.Foto_Perfil;

            if (foto_perfil != null && foto_perfil.Length > 0)
            {
                if (!string.IsNullOrEmpty(usuarioOriginal.Foto_Perfil) && usuarioOriginal.Foto_Perfil != "image.png")
                {
                    string rutaFotoVieja = Path.Combine(_environment.WebRootPath, "uploads", "profile_picture", usuarioOriginal.Foto_Perfil);

                    if (System.IO.File.Exists(rutaFotoVieja)) System.IO.File.Delete(rutaFotoVieja);
                }

                string nombre = Guid.NewGuid().ToString() + "_" + Path.GetFileName(foto_perfil.FileName);
                string direccion = Path.Combine(_environment.WebRootPath, "uploads", "profile_picture");
                string direccionCompleta = Path.Combine(direccion, nombre);

                using (var stream = new FileStream(direccionCompleta, FileMode.Create)) { await foto_perfil.CopyToAsync(stream); }
                ;
                usuario.Foto_Perfil = nombre;
            }

            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Usuarios.Any(u => u.ID == usuario.ID)) return NotFound();
            else throw;
        }

        return RedirectToAction(nameof(Indice));
    }

    // -----------------------------------------------------------------------------------
    // Funciones para Eliminar
    [HttpGet, Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var usuario = await _context.Usuarios.Include(u => u.Rol).FirstOrDefaultAsync(u => u.ID == id);
        if (usuario == null) return NotFound();
        return View(usuario);
    }

    [HttpPost, ValidateAntiForgeryToken, ActionName("Eliminar"), Authorize(Roles = "Administrador")]
    public async Task<IActionResult> ConfirmarEliminar(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null) return NotFound();

        _context.Usuarios.Remove(usuario);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Indice));
    }


    [HttpGet]
    public IActionResult CambiarAvatar()
    {
        return View(new CambiarAvatarViewModel());
    }

    [HttpPost, ValidateAntiForgeryToken]
    [RequestSizeLimit(6 * 1024 * 1024)]
    public async Task<IActionResult> CambiarAvatar(
        CambiarAvatarViewModel modelo)
    {
        if (!int.TryParse(
            User.FindFirstValue(ClaimTypes.NameIdentifier),
            out int usuarioID))
        {
            return Challenge();
        }

        if (!ModelState.IsValid)
            return View(modelo);

        var foto = modelo.Foto!;

        // Permitimos imágenes de hasta 2 MB.
        if (foto.Length == 0 || foto.Length > 5 * 1024 * 1024)
        {
            ModelState.AddModelError(
                "Foto", "La imagen debe pesar entre 1 byte y 5 MB.");

            return View(modelo);
        }

        // Comprobamos el contenido, además de la extensión.
        byte[] contenido;

        using (var memoria = new MemoryStream())
        {
            await foto.CopyToAsync(memoria);
            contenido = memoria.ToArray();
        }

        var extension = Path.GetExtension(foto.FileName).ToLowerInvariant();

        bool esPng = contenido.Length >= 8
            && contenido.Take(8).SequenceEqual(
                new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 });

        bool esJpeg = contenido.Length >= 3
            && contenido[0] == 255
            && contenido[1] == 216
            && contenido[2] == 255;

        bool formatoValido =
            (extension == ".png" && esPng)
            || ((extension == ".jpg" || extension == ".jpeg") && esJpeg);

        if (!formatoValido)
        {
            ModelState.AddModelError(
                "Foto", "Seleccioná una imagen PNG o JPG válida.");

            return View(modelo);
        }

        var usuario = await _context.Usuarios.FindAsync(usuarioID);

        if (usuario == null)
            return NotFound();

        var carpeta = Path.Combine(
            _environment.WebRootPath, "uploads", "profile_picture");

        Directory.CreateDirectory(carpeta);

        var nombreArchivo = Guid.NewGuid().ToString("N")
            + (esPng ? ".png" : ".jpg");

        var rutaArchivo = Path.Combine(carpeta, nombreArchivo);

        await System.IO.File.WriteAllBytesAsync(rutaArchivo, contenido);

        usuario.Foto_Perfil = nombreArchivo;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch
        {
            System.IO.File.Delete(rutaArchivo);
            throw;
        }


        // -----------------------------------------------------------------------------------
        // Actualizamos la foto de la sesion para el menu superior.
        var identidad = new ClaimsIdentity((ClaimsIdentity)User.Identity!);

        foreach (var claim in identidad.FindAll(ClaimTypes.Uri).ToList())
        {
            identidad.RemoveClaim(claim);
        }

        identidad.AddClaim(new Claim(ClaimTypes.Uri, nombreArchivo));

        await HttpContext.SignInAsync(
            Microsoft.AspNetCore.Authentication.Cookies
                .CookieAuthenticationDefaults.AuthenticationScheme ,
            new ClaimsPrincipal(identidad));

        return RedirectToAction(nameof(Detalles),
            new { id = usuarioID });
    }

}