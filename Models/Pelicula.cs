using System.ComponentModel.DataAnnotations;

namespace Proyecto_Final.Models;

public class Pelicula {
    [Key]
    public int ID { get; set; }

    [Required(ErrorMessage = "Este campo es obligatorio")]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "Este campo es obligatorio")]
    public string Director { get; set; } = string.Empty;

    [Required(ErrorMessage = "Este campo es obligatorio")]
    [StringLength(3, ErrorMessage = "La clasificación no puede superar los 3 caracteres.")]
    public string Clasificacion { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Este campo es obligatorio")]
    public string Idioma { get; set; } = string.Empty;

    [Required(ErrorMessage = "Este campo es obligatorio")]
    public int Horario { get; set; }

    [Required(ErrorMessage = "Este campo es obligatorio")]
    public int Duracion { get; set; }

    [Required(ErrorMessage = "Este campo es obligatorio")]
    public double Precio { get; set; }

    public bool Estado { get; set; }
}