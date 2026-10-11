using System.ComponentModel.DataAnnotations;

namespace Proyecto_Final.Models;

public class Butaca {
    [Key]
    public int ID { get; set; }

    [Required(ErrorMessage = "Este campo es obligatorio")]
    public int Numero { get; set; }
    public bool Estado { get; set; }
}