using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_Final.Models;

public class Butaca {
    [Key]
    public int ID { get; set; }

    [Required(ErrorMessage = "Este campo es obligatorio")]
    [StringLength(3, ErrorMessage = "El número no puede superar los 3 caracteres.")]
    public string Numero { get; set; }

    public bool Ocupado { get; set; }

    [Required]
    public int SalaID { get; set; }
    
    [ForeignKey("SalaID")]
    public virtual Sala? Sala { get; set; }
}