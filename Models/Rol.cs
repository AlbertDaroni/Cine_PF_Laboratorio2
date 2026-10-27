using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_Final.Models;

[Table("roles")]
public class Rol {
    [Key]
    public int ID { get; set; }

    [Required(ErrorMessage = "Este campo es obligatorio"), StringLength(255)]
    public string Nombre { get; set; } = string.Empty;

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}