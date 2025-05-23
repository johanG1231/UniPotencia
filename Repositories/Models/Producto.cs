namespace Backend_PotenciaPC.Repositories.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public string Descripcion { get; set; }
        public string ArchivoUrl { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool Activo { get; set; }
    }

}
