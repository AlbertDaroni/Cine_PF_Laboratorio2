using System.ComponentModel.DataAnnotations;

namespace Proyecto_Final.Models;

public class Reserva {
    [Key]
    public int ID { get; set; }

    [Display(Name = "Fecha de creación")]
    public DateTime Fecha_Creacion { get; set; }

    [Required(ErrorMessage = "Este campo es obligatorio")]
    public double Monto { get; set; }

    public bool Estado { get; set; }
}