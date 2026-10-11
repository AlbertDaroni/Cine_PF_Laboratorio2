using System.ComponentModel.DataAnnotations;

namespace Proyecto_Final.Models;

public class Genero {
    [Key]
    public int ID { get; set; }

    [Required(ErrorMessage = "Este campo es obligatorio")]
    [StringLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres.")]
    public string Nombre { get; set; } = string.Empty;
}