﻿namespace Backend_PotenciaPC.Dtos
{
    public class ResponseUserDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
        public bool Confirmado { get; set; }
    }
}
