using System;
using System.Collections.Generic;

namespace EcoCycleMVC.Models;

public partial class RecogidasDomicilio
{
    public int RecogidaId { get; set; }

    public int UsuarioId { get; set; }

    public int CentroId { get; set; }

    public DateTime? FechaSolicitud { get; set; }

    public DateTime? FechaProgramada { get; set; }

    public string? Direccion { get; set; }

    public string? Estado { get; set; }

    public virtual CentrosRecoleccion Centro { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
