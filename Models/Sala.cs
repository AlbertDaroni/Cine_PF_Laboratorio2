using System.ComponentModel.DataAnnotations;

namespace Proyecto_Final.Models;

public class Sala {
    [Key]
    public int ID { get; set; }

    [Required(ErrorMessage = "Este campo es obligatorio")]
    public int Numero { get; set; }

    [Required(ErrorMessage = "Este campo es obligatorio")]
    public string Tipo { get; set; } = string.Empty;

    [Required(ErrorMessage = "Este campo es obligatorio")]
    public int Capacidad { get; set; }

    public bool Estado { get; set; }
}