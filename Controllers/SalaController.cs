using Microsoft.AspNetCore.Mvc;
using Proyecto_Final.Data;

public class SalaController : Controller {
    private readonly BD_Context _context;
    public SalaController(BD_Context context) { _context = context; }

    public IActionResult Indice() {
        var salas = _context.Salas.ToList();
        return View(salas);
    }
}