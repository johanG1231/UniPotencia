namespace Backend_PotenciaPC.Repositories.Models
{
    public class Descarga
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int ProductoId { get; set; }
        public DateTime FechaDescarga { get; set; }
    }

}
