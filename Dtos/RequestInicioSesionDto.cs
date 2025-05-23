namespace Backend_PotenciaPC.Dtos
{
    public class RequestInicioSesionDto
    {
        public ResponseUserDto? Usuario { get; set; }

        public String Contrasena { get; set; } = String.Empty;
    }
}
