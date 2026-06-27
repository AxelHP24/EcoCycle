using EcoCycleMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EcoCycleMVC.Controllers
{
    public class PublicacionesController : Controller
    {
        private readonly EcoCycleContext _context;

        public PublicacionesController(EcoCycleContext context)
        {
            _context = context;
        }

        //-------------------------------------------------
        // FORMULARIO
        //-------------------------------------------------

        [HttpGet]
        public IActionResult Crear()
        {
            int? usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            if (usuarioId == null)
                return RedirectToAction("Login", "Usuarios");

            NuevaPublicacionViewModel vm = new NuevaPublicacionViewModel();

            vm.Materiales = _context.Materiales
                .Select(m => new SelectListItem
                {
                    Value = m.MaterialId.ToString(),
                    Text = m.NombreMaterial
                }).ToList();

            return View(vm);
        }

        //-------------------------------------------------
        // GUARDAR
        //-------------------------------------------------

        [HttpPost]
        public IActionResult Crear(NuevaPublicacionViewModel vm)
        {
            int? usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            if (usuarioId == null)
                return RedirectToAction("Login", "Usuarios");

            if (!ModelState.IsValid)
            {
                vm.Materiales = _context.Materiales
                    .Select(m => new SelectListItem
                    {
                        Value = m.MaterialId.ToString(),
                        Text = m.NombreMaterial
                    }).ToList();

                return View(vm);
            }

            Publicacione nueva = new Publicacione();

            nueva.UsuarioId = usuarioId.Value;
            nueva.MaterialId = vm.MaterialId;
            nueva.Descripcion = vm.Descripcion;
            nueva.CantidadKg = vm.CantidadKg;
            nueva.Ubicacion = vm.Ubicacion;
            nueva.Estado = "Disponible";
            nueva.FechaPublicacion = DateTime.Now;

            _context.Publicaciones.Add(nueva);
            _context.SaveChanges();

            TempData["Mensaje"] = "Material publicado correctamente.";

            return RedirectToAction("Dashboard", "Usuarios");
        }
    }
}