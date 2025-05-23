namespace Backend_PotenciaPC.Dtos
{
    public class ResponseInicionSesionDto
    {
        public int Respuesta { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public DateTime TiempoExpiracion { get; set; }
        public ResponseUserDto Usuario { get; set; } = new ResponseUserDto();
    }

}
