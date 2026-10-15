using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_Final.Models;

public class Funcion {
    [Key]
    public int ID { get; set; }

    [Required(ErrorMessage = "Este campo es obligatorio")]
    public DateTime FechaHora { get; set; }

    [Required(ErrorMessage = "Este campo es obligatorio")]
    public double Precio { get; set; }

    public bool Estado { get; set; } = true;

    [Required]
    public int PeliculaID { get; set; }
    
    [ForeignKey("PeliculaID")]
    public virtual Pelicula? Pelicula { get; set; }

    [Required]
    public int SalaID { get; set; }
    
    [ForeignKey("SalaID")]
    public virtual Sala? Sala { get; set; }

    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}