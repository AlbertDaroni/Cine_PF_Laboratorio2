using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Proyecto_Final.Models;
using Proyecto_Final.Data;
using System.Security.Claims;

namespace Proyecto_Final.Controllers;

[Authorize]
public class ReservaController : Controller
{
    private readonly BD_Context _context;
    public ReservaController(BD_Context context) { _context = context; }

    // -----------------------------------------------------------------------------------
    // Funciones para Índice
    [HttpGet]
    public async Task<IActionResult> Indice(int pagina = 1)
    {
        if (!int.TryParse(
            User.FindFirstValue(ClaimTypes.NameIdentifier),
            out int usuarioID))
        {
            return Challenge();
        }

        const int tamanioPagina = 10;

        var consulta = _context.Reservas
            .AsNoTracking()
            .Where(r => r.UsuarioID == usuarioID);

        var total = await consulta.CountAsync();

        var totalPaginas = Math.Max(
            1,
            (int)Math.Ceiling(total / (double)tamanioPagina));

        pagina = Math.Clamp(pagina, 1, totalPaginas);

        var reservas = await consulta
            .Include(r => r.Funcion)
                .ThenInclude(f => f!.Pelicula)
            .Include(r => r.Detalles)
                .ThenInclude(d => d.Butaca)
            .AsSplitQuery()
            .OrderByDescending(r => r.Fecha_Creacion)
            .ThenByDescending(r => r.ID)
            .Skip((pagina - 1) * tamanioPagina)
            .Take(tamanioPagina)
            .ToListAsync();

        ViewBag.Pagina = pagina;
        ViewBag.TotalPaginas = totalPaginas;

        return View(reservas);
    }

    // -----------------------------------------------------------------------------------
    // Funciones para Crear
    [HttpGet]
    public async Task<IActionResult> Crear(int? FuncionID)
    {
        if (FuncionID == null)
            return BadRequest("Debés indicar una función.");

        var funcion = await _context.Funciones
            .Include(f => f.Pelicula)
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.ID == FuncionID.Value);

        if (funcion == null)
            return NotFound();

        if (!funcion.Estado || funcion.FechaHora <= DateTime.Now)
            return BadRequest("Esta función no está disponible para reservar.");

        var butacasReservadas = _context.DetallesReservas
            .Where(d => d.Reserva != null
                && d.Reserva.FuncionID == funcion.ID
                && d.Reserva.Estado)
            .Select(d => d.ButacaID);

        var modelo = new CrearReservaViewModel
        {
            FuncionID = funcion.ID,
            TituloPelicula = funcion.Pelicula?.Titulo ?? "Sin título",
            FechaHora = funcion.FechaHora,
            PrecioEntrada = funcion.Precio,

            Butacas = await _context.Butacas
                .Where(b => b.SalaID == funcion.SalaID)
                .OrderBy(b => b.Numero)
                .Select(b => new ButacaDisponibleViewModel
                {
                    ID = b.ID,
                    Numero = b.Numero,
                    Disponible = !butacasReservadas.Contains(b.ID)
                })
                .ToListAsync()
        };

        return View(modelo);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(
    int FuncionID,
    List<int>? ButacasSeleccionadas)
    {
        if (!int.TryParse(
            User.FindFirstValue(ClaimTypes.NameIdentifier),
            out int usuarioID))
        {
            return Challenge();
        }

        var seleccionadas = ButacasSeleccionadas?
            .Distinct()
            .ToList() ?? new List<int>();

        if (!ModelState.IsValid || seleccionadas.Count == 0)
        {
            ModelState.AddModelError(
                "", "Seleccioná al menos una butaca válida.");

            return await Crear((int?)FuncionID);
        }

        // La transacción permite guardar la reserva y sus detalles juntos.
        await using var transaccion =
            await _context.Database.BeginTransactionAsync();

        // Bloqueamos esta función mientras comprobamos y guardamos.
        // Este mismo bloqueo debe usarse en futuros caminos de reserva.
        var funciones = await _context.Funciones
            .FromSqlInterpolated(
                $"SELECT * FROM funciones WHERE id = {FuncionID} FOR UPDATE")
            .ToListAsync();

        var funcion = funciones.FirstOrDefault();

        if (funcion == null)
            return NotFound();

        if (!funcion.Estado || funcion.FechaHora <= DateTime.Now)
            return BadRequest("La función ya no está disponible.");

        var cantidadValida = await _context.Butacas
            .CountAsync(b =>
                b.SalaID == funcion.SalaID
                && seleccionadas.Contains(b.ID));

        if (cantidadValida != seleccionadas.Count)
            return BadRequest("Hay butacas que no pertenecen a esta sala.");

        var hayReservadas = await _context.DetallesReservas
            .AnyAsync(d =>
                seleccionadas.Contains(d.ButacaID)
                && d.Reserva != null
                && d.Reserva.FuncionID == FuncionID
                && d.Reserva.Estado);

        if (hayReservadas)
        {
            await transaccion.RollbackAsync();

            ModelState.AddModelError(
                "", "Otra persona reservó alguna de esas butacas. Elegí nuevamente.");

            return await Crear((int?)FuncionID);
        }

        var reserva = new Reserva
        {
            UsuarioID = usuarioID,
            FuncionID = funcion.ID,
            Fecha_Creacion = DateTime.Now,
            Estado = true,
            Monto = funcion.Precio * seleccionadas.Count,

            Detalles = seleccionadas.Select(id => new DetalleReserva
            {
                ButacaID = id,
                Precio = funcion.Precio
            }).ToList()
        };

        _context.Reservas.Add(reserva);
        await _context.SaveChangesAsync();
        await transaccion.CommitAsync();

        TempData["Mensaje"] =
            $"Reserva N.º {reserva.ID} creada correctamente.";

        return RedirectToAction(nameof(Crear),
            new { FuncionID = funcion.ID });
    }

    // -----------------------------------------------------------------------------------
    // Funciones para Editar
    [HttpGet, Authorize(Roles = "Administrador, Empleado")]
    public async Task<IActionResult> Editar(int id)
    {
        var reserva = await _context.Reservas.FindAsync(id);
        if (reserva == null) return NotFound();
        return View(reserva);
    }

    [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Administrador, Empleado")]
    public async Task<IActionResult> Editar(int id, Reserva reserva)
    {
        if (id != reserva.ID) return BadRequest();
        if (!ModelState.IsValid) return View(reserva);

        _context.Reservas.Update(reserva);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Indice));
    }

    // -----------------------------------------------------------------------------------
    // Funciones para Eliminar
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancelar(int id)
    {
        if (!int.TryParse(
            User.FindFirstValue(ClaimTypes.NameIdentifier),
            out int usuarioID))
        {
            return Challenge();
        }

        var reserva = await _context.Reservas
            .Include(r => r.Funcion)
            .Include(r => r.Pago)
            .FirstOrDefaultAsync(r =>
                r.ID == id && r.UsuarioID == usuarioID);

        if (reserva == null)
            return NotFound();

        if (!reserva.Estado)
        {
            TempData["Mensaje"] = "La reserva ya estaba cancelada.";
            return RedirectToAction(nameof(Indice));
        }

        if (reserva.Funcion == null
            || reserva.Funcion.FechaHora <= DateTime.Now)
        {
            TempData["Error"] =
                "No se puede cancelar una función que ya comenzó.";

            return RedirectToAction(nameof(Indice));
        }

        if (reserva.Pago != null && reserva.Pago.Estado)
        {
            TempData["Error"] =
                "Esta reserva tiene un pago activo. Debe gestionarse su anulación.";

            return RedirectToAction(nameof(Indice));
        }

        reserva.Estado = false;
        await _context.SaveChangesAsync();

        TempData["Mensaje"] =
            $"Reserva N.º {reserva.ID} cancelada correctamente.";

        return RedirectToAction(nameof(Indice));
    }
}