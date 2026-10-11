using System.ComponentModel.DataAnnotations;

namespace Proyecto_Final.Models;

public class Rol {
    [Key]
    public int ID { get; set; }

    [Required(ErrorMessage = "Este campo es obligatorio")]
    public string Nombre { get; set; } = string.Empty;
}