using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Backend_PotenciaPC.Repositories.Models;

public partial class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Correo { get; set; }
    public string Contraseña { get; set; }
    public int RolId { get; set; }
    public DateTime FechaCreacion { get; set; }
    public bool Activo { get; set; }
    public virtual Role Rol { get; set; } = null!;
    public bool Confirmado { get; set; } = false;

    [Column("TokenConfirmacion")] // 👈 asegúrate que coincida exactamente
    public string? TokenConfirmacion { get; set; }


}
