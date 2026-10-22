using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_Final.Models;

[Table("funciones")]
public class Funcion {
    [Key, Column("id")]
    public int ID { get; set; }

    [Required(ErrorMessage = "La fecha y hora son obligatorias")]
    [Column("fecha_hora")]
    public DateTime FechaHora { get; set; }

    [Required(ErrorMessage = "El precio es obligatorio")]
    [Column("precio", TypeName = "decimal(10,2)")]
    public decimal Precio { get; set; }

    [Column("estado")]
    public bool Estado { get; set; } = true;

    [Required, Column("pelicula_id")]
    public int PeliculaID { get; set; }

    [ForeignKey(nameof(PeliculaID))]
    public virtual Pelicula? Pelicula { get; set; }

    [Required, Column("sala_id")]
    public int SalaID { get; set; }

    [ForeignKey(nameof(SalaID))]
    public virtual Sala? Sala { get; set; }

    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}