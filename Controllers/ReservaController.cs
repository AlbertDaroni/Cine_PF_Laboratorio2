using Microsoft.AspNetCore.Mvc;
using Proyecto_Final.Models;
using Proyecto_Final.Data;

namespace Proyecto_Final.Controllers;

public class ReservaController : Controller {
    private readonly BD_Context _context;
    public ReservaController(BD_Context context) { _context = context; }

    public IActionResult Indice() {
        var reservas = _context.Reservas.ToList();
        return View(reservas);
    }
}