using System;
using System.Collections.Generic;

namespace EcoCycleMVC.Models;

public partial class MovimientosPunto
{
    public int MovimientoId { get; set; }

    public int UsuarioId { get; set; }

    public int Puntos { get; set; }

    public string? TipoMovimiento { get; set; }

    public string? Descripcion { get; set; }

    public DateTime? FechaMovimiento { get; set; }

    public virtual Usuario Usuario { get; set; } = null!;
}
