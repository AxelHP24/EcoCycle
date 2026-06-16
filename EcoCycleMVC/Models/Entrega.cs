using System;
using System.Collections.Generic;

namespace EcoCycleMVC.Models;

public partial class Entrega
{
    public int EntregaId { get; set; }

    public int UsuarioId { get; set; }

    public int CentroId { get; set; }

    public int PublicacionId { get; set; }

    public decimal? PesoValidado { get; set; }

    public DateTime? FechaEntrega { get; set; }

    public string? Estado { get; set; }

    public virtual CentrosRecoleccion Centro { get; set; } = null!;

    public virtual Publicacione Publicacion { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
