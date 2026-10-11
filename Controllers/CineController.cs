using Microsoft.AspNetCore.Mvc;
using Proyecto_Final.Data;

public class CineController : Controller {
    private readonly BD_Context _context;
    public CineController(BD_Context context) { _context = context; }

    public IActionResult Indice() {
        var cines = _context.Cines.ToList();
        return View(cines);
    }
}