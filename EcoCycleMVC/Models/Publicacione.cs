using System;
using System.Collections.Generic;

namespace EcoCycleMVC.Models;

public partial class Publicacione
{
    public int PublicacionId { get; set; }

    public int UsuarioId { get; set; }

    public int MaterialId { get; set; }

    public string? Descripcion { get; set; }

    public decimal? CantidadKg { get; set; }

    public string? Ubicacion { get; set; }

    public string? Imagen { get; set; }

    public DateTime? FechaPublicacion { get; set; }

    public string? Estado { get; set; }

    public virtual ICollection<Entrega> Entregas { get; set; } = new List<Entrega>();

    public virtual Materiale Material { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
