using Microsoft.AspNetCore.Mvc;
using Proyecto_Final.Data;

public class UsuarioController : Controller {
    private readonly BD_Context _context;
    public UsuarioController(BD_Context context) { _context = context; }

    public IActionResult Indice() {
        var usuarios = _context.Usuarios.ToList();
        return View(usuarios);
    }
}