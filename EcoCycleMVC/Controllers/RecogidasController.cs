using EcoCycleMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace EcoCycleMVC.Controllers
{
    public class RecogidasController : Controller
    {
        private readonly EcoCycleContext _context;

        public RecogidasController(EcoCycleContext context)
        {
            _context = context;
        }

        // GET: Crear recogida
        public IActionResult Crear()
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            if (usuarioId == null)
                return RedirectToAction("Login", "Usuarios");

            var modelo = new RecogidaViewModel
            {
                Centros = _context.CentrosRecoleccions
                    .Select(c => new SelectListItem
                    {
                        Value = c.CentroId.ToString(),
                        Text = c.Direccion // ✅ CORREGIDO
                    })
                    .ToList()
            };

            return View(modelo);
        }

        // POST: Crear recogida
        [HttpPost]
        public IActionResult Crear(RecogidaViewModel modelo)
        {
            modelo.Centros = _context.CentrosRecoleccions
                .Select(c => new SelectListItem
                {
                    Value = c.CentroId.ToString(),
                    Text = c.Direccion // ✅ CORREGIDO
                })
                .ToList();

            if (!ModelState.IsValid)
                return View(modelo);

            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            if (usuarioId == null)
                return RedirectToAction("Login", "Usuarios");

            var recogida = new RecogidasDomicilio
            {
                UsuarioId = usuarioId.Value,
                CentroId = modelo.CentroId,
                Direccion = modelo.Direccion,
                FechaSolicitud = DateTime.Now,
                Estado = "Pendiente"
            };

            _context.RecogidasDomicilios.Add(recogida);
            _context.SaveChanges();

            TempData["Mensaje"] = "Recogida solicitada correctamente";

            return RedirectToAction("Dashboard", "Usuarios");
        }
    }
}