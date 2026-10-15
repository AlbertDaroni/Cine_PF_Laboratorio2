using System.ComponentModel.DataAnnotations;

namespace Proyecto_Final.Models {
    public class SignInViewModel {
        [Required, EmailAddress]
        public string Correo { get; set; } = string.Empty;

        [Required]
        public int DNI { get; set; }

        [Required, DataType(DataType.Password)]
        public string Contraseña { get; set; } = string.Empty;
    }
}