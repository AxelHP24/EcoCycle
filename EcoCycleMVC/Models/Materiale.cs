using System;
using System.Collections.Generic;

namespace EcoCycleMVC.Models;

public partial class Materiale
{
    public int MaterialId { get; set; }

    public string NombreMaterial { get; set; } = null!;

    public decimal PuntosPorKg { get; set; }

    public virtual ICollection<Publicacione> Publicaciones { get; set; } = new List<Publicacione>();
}
