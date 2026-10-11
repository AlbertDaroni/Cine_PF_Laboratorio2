using System.ComponentModel.DataAnnotations;

namespace Proyecto_Final.Models;

public class Cine {
    [Key]
    public int ID { get; set; }

    [Required(ErrorMessage = "Este campo es obligatorio")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "Este campo es obligatorio")]
    public string Direccion { get; set; } = string.Empty;

    public bool Abierto { get; set; }
}