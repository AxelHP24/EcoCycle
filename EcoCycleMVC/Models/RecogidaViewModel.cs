using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace EcoCycleMVC.Models
{
    public class RecogidaViewModel
    {
        [Required]
        public int CentroId { get; set; }

        [Required]
        public string Direccion { get; set; } // ✅ ESTO FALTABA

        public List<SelectListItem> Centros { get; set; } = new();
    }
}