using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace EcoCycleMVC.Models
{
    public class NuevaPublicacionViewModel
    {
        [Required]
        public int MaterialId { get; set; }

        [Required]
        [Display(Name = "Cantidad (Kg)")]
        public decimal CantidadKg { get; set; }

        [Required]
        public string Descripcion { get; set; }

        [Required]
        public string Ubicacion { get; set; }

        public List<SelectListItem> Materiales { get; set; } = new();
    }
}