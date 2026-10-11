using Microsoft.AspNetCore.Mvc;
using Proyecto_Final.Data;

public class GeneroController : Controller {
    private readonly BD_Context _context;
    public GeneroController(BD_Context context) { _context = context; }

    public IActionResult Indice() {
        var generos = _context.Generos.ToList();
        return View(generos);
    }
}