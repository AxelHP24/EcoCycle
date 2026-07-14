using Microsoft.AspNetCore.Mvc.Rendering;

namespace EcoCycleMVC.Models
{
    public class RecogidaViewModel
    {
        public int CentroId { get; set; }
        public string? Direccion { get; set; }

        public List<SelectListItem> Centros { get; set; } = new();
        public List<SelectListItem> UbicacionesPublicaciones { get; set; } = new();
    }
}