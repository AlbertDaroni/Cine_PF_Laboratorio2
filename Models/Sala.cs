using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_Final.Models;

public class Sala {
    [Key]
    public int ID { get; set; }

    [Required(ErrorMessage = "Este campo es obligatorio")]
    public int Numero { get; set; }

    [Required(ErrorMessage = "Este campo es obligatorio"), StringLength(255)]
    public string Tipo { get; set; } = string.Empty;

    [Required(ErrorMessage = "Este campo es obligatorio")]
    public int Capacidad { get; set; }

    public bool Estado { get; set; } = true;

    [Required]
    public int CineID { get; set; }
    
    [ForeignKey("CineID")]
    public virtual Cine? Cine { get; set; }

    public virtual ICollection<Butaca> Butacas { get; set; } = new List<Butaca>();
    public virtual ICollection<Funcion> Funciones { get; set; } = new List<Funcion>();
}