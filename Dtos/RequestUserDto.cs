using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Backend_PotenciaPC.Dtos
{
    public class RequestUserDto
    {
        [JsonPropertyName("nombre")]
        public string Name { get; set; }

        [JsonPropertyName("correo")]
        public string Username { get; set; }

        [JsonPropertyName("Contrasena")]
        public string Password { get; set; }
    }
}