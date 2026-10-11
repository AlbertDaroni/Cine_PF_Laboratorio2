using Microsoft.AspNetCore.Mvc;
using Proyecto_Final.Data;

public class RolController : Controller {
    private readonly BD_Context _context;
    public RolController(BD_Context context) { _context = context; }

    public IActionResult Indice() {
        var roles = _context.Roles.ToList();
        return View(roles);
    }
}