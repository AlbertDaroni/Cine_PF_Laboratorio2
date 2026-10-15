using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_Final.Models;

public class DetalleReserva {
    [Key]
    public int ID { get; set; }

    [Required]
    public double Precio { get; set; }

    [Required]
    public int ReservaID { get; set; }
    
    [ForeignKey("ReservaID")]
    public virtual Reserva? Reserva { get; set; }

    [Required]
    public int ButacaID { get; set; }
    
    [ForeignKey("ButacaID")]
    public virtual Butaca? Butaca { get; set; }
}