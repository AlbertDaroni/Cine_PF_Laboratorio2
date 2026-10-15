using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_Final.Models;
public class Pago {
    [Key]
    public int ID { get; set; }

    [Required(ErrorMessage = "El concepto es obligatorio")]
    [StringLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres.")]
    public string Concepto { get; set; } = string.Empty;

    public DateTime Fecha_Pago { get; set; } = DateTime.Now;

    public DateTime? Fecha_Anulacion { get; set; }

    [Required(ErrorMessage = "El importe es obligatorio")]
    public double Importe { get; set; }

    public bool Estado { get; set; } = true;

    [Required]
    public int ReservaID { get; set; }
    
    [ForeignKey("ReservaID")]
    public virtual Reserva? Reserva { get; set; }
}