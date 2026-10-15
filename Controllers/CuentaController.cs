using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Proyecto_Final.Models;
using Proyecto_Final.Data;

namespace Proyecto_Final.Controllers;

public class CuentaController : Controller {
    private readonly BD_Context _context;
    public CuentaController(BD_Context context) { _context = context; }

    // -----------------------------------------------------------------------------------
    // Funciones para Inicio de sesión
    [HttpGet]
    public IActionResult SignIn() { return View(); }

    [HttpPost]
    public async Task<IActionResult> SignIn(SignInViewModel signin){
        if (!ModelState.IsValid) return View(signin);

        var usuario = await _context.Usuarios.Include(u => u.Rol).FirstOrDefaultAsync(u => u.Correo == signin.Correo);
        if (usuario == null) {
            ModelState.AddModelError("", "El correo es incorrecto.");
            return View(signin);
        }
        if (!usuario.Estado) {
            ModelState.AddModelError("", "El usuario se encuentra inactivo.");
            return View(signin);
        }
        if (usuario.Contraseña != signin.Contraseña) {
            ModelState.AddModelError("", "La contraseña es incorrecta.");
            return View(signin);
        }

        await AutenticarUsuario(usuario);

        return RedirectToAction("Index", "Home");
    }

    // -----------------------------------------------------------------------------------
    // Funciones para Salir de sesión
    [HttpPost]
    public async Task<IActionResult> Logout() {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("SignIn");
    }

    // -----------------------------------------------------------------------------------
    // Funciones para Registrarse
    [HttpGet]
    public IActionResult SignUp () { return View(); }

    [HttpPost]
    public async Task<IActionResult> SignUp (SignUpViewModel signup) {
        if (!ModelState.IsValid) return View(signup);

        var correoExistente = await _context.Usuarios.AnyAsync(u => u.Correo == signup.Correo);
        if (correoExistente) {
            ModelState.AddModelError("", "Ese correo ya existe. Intentá con otro.");
            return View(signup);
        }

        var rol = await _context.Roles.FirstOrDefaultAsync(r => r.Nombre == "Usuario");

        Usuario usuario = new Usuario {
            Nombre = signup.Nombre,
            Correo = signup.Correo,
            DNI = signup.DNI,
            Contraseña = signup.Contraseña,
            RolID = rol.ID
        };

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
        usuario.Rol = rol;

        await AutenticarUsuario(usuario);

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult AccesoDenegado() { return View(); }

    // -----------------------------------------------------------------------------------
    // Funciones para Autenticación (Cookies)
    private async Task AutenticarUsuario (Usuario usuario) {
        var claims = new List<Claim> {
            new Claim(ClaimTypes.NameIdentifier, usuario.ID.ToString()),
            new Claim(ClaimTypes.Name, usuario.Nombre),
            new Claim(ClaimTypes.Email, usuario.Correo),
            new Claim(ClaimTypes.Uri, usuario.Foto_Perfil ?? string.Empty),
            new Claim(ClaimTypes.Role, usuario.Rol?.Nombre ?? "Usuario")
        };

        var identidad = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identidad);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
    }
}