namespace EcoCycleMVC.Models
{
    public class PublicacionViewModel
    {
        public int PublicacionId { get; set; }
        public string Material { get; set; }
        public decimal? CantidadKg { get; set; }
        public string Ubicacion { get; set; }
        public string Estado { get; set; }
        public DateTime? Fecha { get; set; }
    }
}