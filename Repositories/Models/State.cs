using System;
using System.Collections.Generic;

namespace Backend_PotenciaPC.Repositories.Models;

public partial class State
{
    public int IdState { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
