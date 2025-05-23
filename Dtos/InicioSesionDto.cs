using System.ComponentModel.DataAnnotations;
using Backend_PotenciaPC.Repositories.Models;

namespace Backend_PotenciaPC.Dtos
{
    public class InicioSesionDto
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        public string Contrasena { get; set; } = string.Empty;
    }
}
