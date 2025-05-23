using System;
using System.Collections.Generic;

namespace Backend_PotenciaPC.Repositories.Models;

public partial class Audit
{
    public int IdAudit { get; set; }

    public int IdUser { get; set; }

    public string Date { get; set; } = null!;

    public string ViewAction { get; set; } = null!;

    public string Acction { get; set; } = null!;
}
