using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;


namespace Proyecto_Final.Models;


public class CambiarAvatarViewModel
{
    [Required(ErrorMessage = "Selecciona una imagen.")]
    public IFormFile? Foto { get; set; }
}