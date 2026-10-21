using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_Final.Models;

[Table("reservas")]
public class Reserva
{
    [Key]
    [Column("id")]
    public int ID { get; set; }

    [Display(Name = "Fecha de creación")]
    [Column("fecha_creacion")]
    public DateTime Fecha_Creacion { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "El monto es obligatorio")]
    [Column("monto", TypeName = "decimal(10,2)")]
    public decimal Monto { get; set; }

    [Column("estado")]
    public bool Estado { get; set; } = true;

    [Required]
    [Column("usuario_id")]
    public int UsuarioID { get; set; }

    [ForeignKey(nameof(UsuarioID))]
    public virtual Usuario? Usuario { get; set; }

    [Required]
    [Column("funcion_id")]
    public int FuncionID { get; set; }

    [ForeignKey(nameof(FuncionID))]
    public virtual Funcion? Funcion { get; set; }

    public virtual ICollection<DetalleReserva> Detalles { get; set; }
        = new List<DetalleReserva>();

    public virtual Pago? Pago { get; set; }
}