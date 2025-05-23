namespace Backend_PotenciaPC.Repositories.Models;

public partial class Role
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
