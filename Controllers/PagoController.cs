using Microsoft.AspNetCore.Mvc;
using Proyecto_Final.Data;

public class PagoController : Controller {
    private readonly BD_Context _context;
    public PagoController(BD_Context context) { _context = context; }

    public IActionResult Indice() {
        var pagos = _context.Pagos.ToList();
        return View(pagos);
    }
}