using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_Final.Models;

[Table("salas")]
public class Sala {
    [Key]
    public int ID { get; set; }

    [Required(ErrorMessage = "Este campo es obligatorio")]
    [Range(1, int.MaxValue, ErrorMessage = "El número de sala debe ser mayor que cero.")]
    public int Numero { get; set; }

    [Required(ErrorMessage = "Este campo es obligatorio"), StringLength(255)]
    public string Tipo { get; set; } = string.Empty;

    [Required(ErrorMessage = "Este campo es obligatorio")]
    [Range(1, int.MaxValue, ErrorMessage = "La capacidad debe ser mayor que cero.")]
    public int Capacidad { get; set; }

    public bool Estado { get; set; } = true;

    [Required, Column("cine_id")]
    [Range(1, int.MaxValue, ErrorMessage = "Seleccioná un cine.")]
    public int CineID { get; set; }
    
    [ForeignKey("CineID")]
    public virtual Cine? Cine { get; set; }

    public virtual ICollection<Butaca> Butacas { get; set; } = new List<Butaca>();
    public virtual ICollection<Funcion> Funciones { get; set; } = new List<Funcion>();
}