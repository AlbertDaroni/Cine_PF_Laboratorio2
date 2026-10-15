using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_Final.Models;

public class Pelicula {
    [Key]
    public int ID { get; set; }

    [Required(ErrorMessage = "Este campo es obligatorio"), StringLength(255)]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "Este campo es obligatorio")]
    [StringLength(10, ErrorMessage = "La clasificación no puede superar los 3 caracteres.")]
    public string Clasificacion { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Este campo es obligatorio"), StringLength(255)]
    public string Idioma { get; set; } = string.Empty;

    [Required(ErrorMessage = "Este campo es obligatorio")]
    public int Duracion { get; set; }

    public bool Estado { get; set; } = true;

    public virtual ICollection<Director> Directores { get; set; } = new List<Director>();
    public virtual ICollection<Genero> Generos { get; set; } = new List<Genero>();
    public virtual ICollection<Funcion> Funciones { get; set; } = new List<Funcion>();
}