using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_Final.Models;

public class Usuario {
    [Key]
    public int ID { get; set; }

    [Required(ErrorMessage = "Este campo es obligatorio")]
    [StringLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "Este campo es obligatorio")]
    [EmailAddress(ErrorMessage = "El formato del correo no es válido")]
    public string Correo { get; set; } = string.Empty;

    [Required(ErrorMessage = "Este campo es obligatorio")]
    public int DNI { get; set; }
    
    public string? Foto_Perfil { get; set; }

    [Required(ErrorMessage = "Este campo es obligatorio")]
    public string Contraseña { get; set; } = string.Empty;
    
    public bool Estado { get; set; } = true;

    [Required]
    public int RolID { get; set; }
    
    [ForeignKey("RolID")]
    public virtual Rol? Rol { get; set; }

    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}