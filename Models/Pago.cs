using System.ComponentModel.DataAnnotations;

namespace Proyecto_Final.Models;
public class Pago {
    [Key]
    public int ID { get; set; }

    [Required(ErrorMessage = "El concepto es obligatorio")]
    [StringLength(50)]
    public string Concepto { get; set; } = string.Empty;

    [Required(ErrorMessage = "La fecha de pago es obligatoria")]
    public DateTime Fecha_Pago { get; set; }

    public DateTime? Fecha_Anulacion { get; set; }

    [Required(ErrorMessage = "El importe es obligatorio")]
    public decimal Importe { get; set; }

    public string Estado { get; set; }
}