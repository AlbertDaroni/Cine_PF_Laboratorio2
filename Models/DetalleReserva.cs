using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_Final.Models;

[Table("detalle_reserva")]
public class DetalleReserva {
    [Key, Column("id")]
    public int ID { get; set; }

    [Required, Column("precio", TypeName = "decimal(10,2)")]
    public decimal Precio { get; set; }

    [Required, Column("reserva_id")]
    public int ReservaID { get; set; }

    [ForeignKey(nameof(ReservaID))]
    public virtual Reserva? Reserva { get; set; }

    [Required, Column("butaca_id")]
    public int ButacaID { get; set; }

    [ForeignKey(nameof(ButacaID))]
    public virtual Butaca? Butaca { get; set; }
}