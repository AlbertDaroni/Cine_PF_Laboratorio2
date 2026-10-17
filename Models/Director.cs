using System.ComponentModel.DataAnnotations;

namespace Proyecto_Final.Models;

public class Director {
    [Key]
    public int ID { get; set; }

    [Required(ErrorMessage = "Este campo es obligatorio"), StringLength(255)]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "Este campo es obligatorio"), StringLength(255)]
    public string Apellido { get; set; } = string.Empty;

    [StringLength(100)]
    public string? Nacionalidad { get; set; }

    public virtual ICollection<Pelicula> Peliculas { get; set; } = new List<Pelicula>();
}