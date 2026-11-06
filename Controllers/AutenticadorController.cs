using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Proyecto_Final.Data;
using Proyecto_Final.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Proyecto_Final.Controllers.Api;

[Route("api/[controller]"), ApiController]
public class AutenticadorController : ControllerBase
{
    private readonly BD_Context _context;
    private readonly IConfiguration _configuration;

    public class LoginRequest
    {
        public string Correo { get; set; } = null!;
        public string Contraseña { get; set; } = null!;
    }

    public AutenticadorController(BD_Context context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    // ENDPOINT 1: Generación de Token (Público)
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var usuario = await _context.Usuarios.Include(u => u.Rol).FirstOrDefaultAsync(u => u.Correo == request.Correo);
        if (usuario == null || !usuario.Estado || !usuario.PasswordHasheada)
        {
            return Unauthorized(new { mensaje = "Credenciales inválidas." });
        }

        // Verificación con el Hasher
        var hasher = new PasswordHasher<Usuario>();
        var resultado = hasher.VerifyHashedPassword(usuario, usuario.Contraseña, request.Contraseña);
        if (resultado == PasswordVerificationResult.Failed) return Unauthorized(new { mensaje = "Credenciales inválidas." });

        // Generamos el Token JWT
        var token = GenerarJwtToken(usuario);

        return Ok(new
        {
            token = token,
            usuario = usuario.Nombre,
            rol = usuario.Rol?.Nombre
        });
    }

    // ENDPOINT 2: Endpoint Protegido de Prueba (Requiere Token)
    [HttpGet("mis-datos"), Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult ObtenerDatosProtegidos()
    {
        // Al estar autenticado, podemos leer los Claims del token enviado por el cliente
        var UsuarioID = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var Correo = User.FindFirstValue(ClaimTypes.Email);
        var Rol = User.FindFirstValue(ClaimTypes.Role);

        return Ok(new
        {
            mensaje = "Acceso concedido",
            UsuarioID,
            Correo,
            Rol
        });
    }

    // Método auxiliar para construir el JWT
    private string GenerarJwtToken(Usuario usuario)
    {
        var claims = new[] {
            new Claim(ClaimTypes.NameIdentifier, usuario.ID.ToString()),
            new Claim(ClaimTypes.Email, usuario.Correo),
            new Claim(ClaimTypes.Role, usuario.Rol?.Nombre ?? "Usuario")
        };

        var clave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var credenciales = new SigningCredentials(clave, SecurityAlgorithms.HmacSha256);
        var tiempoExpiracion = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:DurationInMinutes"]));

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: tiempoExpiracion,
            signingCredentials: credenciales
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}