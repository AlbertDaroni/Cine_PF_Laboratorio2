using Microsoft.AspNetCore.Mvc;
using Proyecto_Final.Models;
using Proyecto_Final.Data;

namespace Proyecto_Final.Controllers;

public class ButacaController : Controller {
    private readonly BD_Context _context;
    public ButacaController(BD_Context context) { _context = context; }

    public IActionResult Indice() {
        var butacas = _context.Butacas.ToList();
        return View(butacas);
    }
}