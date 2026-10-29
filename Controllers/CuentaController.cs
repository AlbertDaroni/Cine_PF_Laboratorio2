using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Proyecto_Final.Models;
using Proyecto_Final.Data;
using Microsoft.AspNetCore.Identity;

namespace Proyecto_Final.Controllers;

public class CuentaController : Controller
{
    private readonly BD_Context _context;
    public CuentaController(BD_Context context) { this._context = context; }

    // -----------------------------------------------------------------------------------
    // Funciones para Inicio de sesión
    [HttpGet]
    public IActionResult SignIn() { return View(); }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> SignIn(SignInViewModel signin)
    {
        if (!ModelState.IsValid)
            return View(signin);

        var usuario = await _context.Usuarios
            .Include(u => u.Rol)
            .FirstOrDefaultAsync(u => u.Correo == signin.Correo);

        if (usuario == null
            || !usuario.Estado
            || usuario.DNI != signin.DNI
            || !usuario.PasswordHasheada)
        {
            ModelState.AddModelError(
                "", "Los datos de acceso no son válidos.");

            return View(signin);
        }

        var hasher = new PasswordHasher<Usuario>();

        PasswordVerificationResult resultado;

        try
        {
            resultado = hasher.VerifyHashedPassword(
                usuario,
                usuario.Contraseña,
                signin.Contraseña);
        }
        catch (FormatException)
        {
            resultado = PasswordVerificationResult.Failed;
        }

        if (resultado == PasswordVerificationResult.Failed)
        {
            ModelState.AddModelError(
                "", "Los datos de acceso no son válidos.");

            return View(signin);
        }

        if (resultado == PasswordVerificationResult.SuccessRehashNeeded)
        {
            usuario.Contraseña = hasher.HashPassword(
                usuario, signin.Contraseña);

            await _context.SaveChangesAsync();
        }

        await AutenticarUsuario(usuario);

        return RedirectToAction("Index", "Home");
    }

    // -----------------------------------------------------------------------------------
    // Funciones para Salir de sesión
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    // -----------------------------------------------------------------------------------
    // Funciones para Registrarse
    [HttpGet]
    public IActionResult SignUp() { return View(); }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> SignUp(SignUpViewModel signup)
    {
        if (!ModelState.IsValid) return View(signup);

        var correoExistente = await _context.Usuarios.AnyAsync(u => u.Correo == signup.Correo);
        if (correoExistente)
        {
            ModelState.AddModelError("", "Ese correo ya existe. Intentá con otro.");
            return View(signup);
        }

        var dniExistente = await _context.Usuarios
            .AnyAsync(u => u.DNI == signup.DNI);

        if (dniExistente)
        {
            ModelState.AddModelError(
                "", "Ese DNI ya está registrado.");

            return View(signup);
        }

        var rol = await _context.Roles.FirstOrDefaultAsync(r => r.Nombre == "Usuario");

        if (rol == null)
        {
            ModelState.AddModelError(
                "",
                "No se pudo completar el registro: falta configurar el rol Usuario.");

            return View(signup);
        }

        Usuario usuario = new Usuario
        {
            Nombre = signup.Nombre,
            Apellido = signup.Apellido,
            Correo = signup.Correo,
            DNI = signup.DNI,
            Contraseña = signup.Contraseña,
            RolID = rol.ID
        };

        var hasher = new PasswordHasher<Usuario>();

        usuario.Contraseña = hasher.HashPassword(
            usuario, signup.Contraseña);

        usuario.PasswordHasheada = true;

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
    private async Task AutenticarUsuario(Usuario usuario)
    {
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