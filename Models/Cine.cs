using System.ComponentModel.DataAnnotations;

namespace Proyecto_Final.Models;

public class Cine {
    [Key]
    public int ID { get; set; }

    [Required(ErrorMessage = "Este campo es obligatorio"), StringLength(255)]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "Este campo es obligatorio"), StringLength(255)]
    public string Direccion { get; set; } = string.Empty;

    public bool Abierto { get; set; }

    public virtual ICollection<Sala> Salas { get; set; } = new List<Sala>();
}