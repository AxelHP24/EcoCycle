using System;
using System.Collections.Generic;

namespace EcoCycleMVC.Models;

public partial class Canje
{
    public int CanjeId { get; set; }

    public int UsuarioId { get; set; }

    public int RecompensaId { get; set; }

    public DateTime? FechaCanje { get; set; }

    public string? CodigoCupon { get; set; }

    public string? Estado { get; set; }

    public virtual Recompensa Recompensa { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
