using System;
using System.Collections.Generic;

namespace EcoCycleMVC.Models;

public partial class CentrosRecoleccion
{
    public int CentroId { get; set; }

    public string NombreCentro { get; set; } = null!;

    public string? Direccion { get; set; }

    public decimal? Latitud { get; set; }

    public decimal? Longitud { get; set; }

    public decimal? CapacidadActual { get; set; }

    public decimal? CapacidadMaxima { get; set; }

    public string? Correo { get; set; }

    public string? Telefono { get; set; }

public string Ubicacion { get; set; }

    public virtual ICollection<Entrega> Entregas { get; set; } = new List<Entrega>();

    public virtual ICollection<RecogidasDomicilio> RecogidasDomicilios { get; set; } = new List<RecogidasDomicilio>();
}
