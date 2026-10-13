using Microsoft.AspNetCore.Mvc;
using Proyecto_Final.Models;
using Proyecto_Final.Data;

namespace Proyecto_Final.Controllers;

public class PeliculaController : Controller {
    private readonly BD_Context _context;
    public PeliculaController(BD_Context context) { _context = context; }

    public IActionResult Indice() {
        var peliculas = _context.Peliculas.ToList();
        return View(peliculas);
    }
}