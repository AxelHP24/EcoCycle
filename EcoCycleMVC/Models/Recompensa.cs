using System;
using System.Collections.Generic;

namespace EcoCycleMVC.Models;

public partial class Recompensa
{
    public int RecompensaId { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public int PuntosNecesarios { get; set; }

    public int? Stock { get; set; }

    public bool? Activa { get; set; }

    public virtual ICollection<Canje> Canjes { get; set; } = new List<Canje>();
}
