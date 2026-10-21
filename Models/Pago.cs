using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_Final.Models;

[Table("pagos")]
public class Pago
{
    [Key]
    [Column("id")]
    public int ID { get; set; }

    [Required(ErrorMessage = "El concepto es obligatorio")]
    [StringLength(50, ErrorMessage = "El concepto no puede superar los 50 caracteres.")]
    [Column("concepto")]
    public string Concepto { get; set; } = string.Empty;

    [Column("fecha_pago")]
    public DateTime Fecha_Pago { get; set; } = DateTime.Now;

    [Column("fecha_anulacion")]
    public DateTime? Fecha_Anulacion { get; set; }

    [Required(ErrorMessage = "El importe es obligatorio")]
    [Column("importe", TypeName = "decimal(10,2)")]
    public decimal Importe { get; set; }

    [Column("estado")]
    public bool Estado { get; set; } = true;

    [Required]
    [Column("reserva_id")]
    public int ReservaID { get; set; }

    [ForeignKey(nameof(ReservaID))]
    public virtual Reserva? Reserva { get; set; }
}