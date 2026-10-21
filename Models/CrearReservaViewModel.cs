using System.ComponentModel.DataAnnotations;

namespace Proyecto_Final.Models;

public class CrearReservaViewModel
{
    public int FuncionID { get; set; }

    public string TituloPelicula { get; set; } = string.Empty;

    public DateTime FechaHora { get; set; }

    public decimal PrecioEntrada { get; set; }

    public List<ButacaDisponibleViewModel> Butacas { get; set; }
        = new List<ButacaDisponibleViewModel>();

    [MinLength(1, ErrorMessage = "Seleccioná al menos una butaca.")]
    public List<int> ButacasSeleccionadas { get; set; }
        = new List<int>();
}

public class ButacaDisponibleViewModel
{
    public int ID { get; set; }

    public string Numero { get; set; } = string.Empty;

    public bool Disponible { get; set; }
}