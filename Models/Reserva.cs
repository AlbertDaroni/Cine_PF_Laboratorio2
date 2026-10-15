using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_Final.Models;

public class Reserva {
    [Key]
    public int ID { get; set; }

    [Display(Name = "Fecha de creación")]
    public DateTime Fecha_Creacion { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "Este campo es obligatorio")]
    public double Monto { get; set; }

    public bool Estado { get; set; } = true;

    [Required, Column("usuario_id")]
    public int UsuarioID { get; set; }
    
    [ForeignKey("UsuarioID")]
    public virtual Usuario? Usuario { get; set; }

    [Required, Column("funcion_id")]
    public int FuncionID { get; set; }
    
    [ForeignKey("FuncionID")]
    public virtual Funcion? Funcion { get; set; }

    public virtual ICollection<DetalleReserva> Detalles { get; set; } = new List<DetalleReserva>();
    
    public virtual Pago? Pago { get; set; }
}