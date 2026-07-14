using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace EcoCycleMVC.Models
{
    public class NuevaRecogidaViewModel
    {
        [Required(ErrorMessage = "Seleccione un centro de reciclaje.")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione un centro de reciclaje.")]
        [Display(Name = "Centro de reciclaje")]
        public int CentroId { get; set; }

        [Required(ErrorMessage = "Seleccione una fecha de recogida.")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Fecha de recogida")]
        public DateTime? FechaProgramada { get; set; }

        [Required(ErrorMessage = "Ingrese la dirección.")]
        [Display(Name = "Dirección")]
        public string Direccion { get; set; } = string.Empty;

        public List<SelectListItem> Centros { get; set; } = new();
    }
}