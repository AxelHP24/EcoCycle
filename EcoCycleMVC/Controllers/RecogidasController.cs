using EcoCycleMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EcoCycleMVC.Controllers
{
    public class RecogidasController : Controller
    {
        private readonly EcoCycleContext _context;

        public RecogidasController(EcoCycleContext context)
        {
            _context = context;
        }

        //=====================================================
        // CREAR (GET)
        //=====================================================

        [HttpGet]
        public IActionResult Crear()
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            if (usuarioId == null)
                return RedirectToAction("Login", "Usuarios");

            NuevaRecogidaViewModel modelo = new NuevaRecogidaViewModel();

            modelo.Centros = _context.CentrosRecoleccions
                .OrderBy(c => c.Direccion)
                .Select(c => new SelectListItem
                {
                    Value = c.CentroId.ToString(),
                    Text = c.Direccion
                })
                .ToList();

            return View(modelo);
        }

        //=====================================================
        // CREAR (POST)
        //=====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(NuevaRecogidaViewModel modelo)
        {
            modelo.Centros = _context.CentrosRecoleccions
                .OrderBy(c => c.Direccion)
                .Select(c => new SelectListItem
                {
                    Value = c.CentroId.ToString(),
                    Text = c.Direccion
                })
                .ToList();

            if (!ModelState.IsValid)
                return View(modelo);

            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            if (usuarioId == null)
                return RedirectToAction("Login", "Usuarios");

            if (modelo.FechaProgramada == null)
            {
                ModelState.AddModelError("FechaProgramada", "Seleccione una fecha de recogida.");
                return View(modelo);
            }

            RecogidasDomicilio nueva = new RecogidasDomicilio
            {
                UsuarioId = usuarioId.Value,
                CentroId = modelo.CentroId,
                FechaSolicitud = DateTime.Now,
                FechaProgramada = modelo.FechaProgramada.Value,
                Direccion = modelo.Direccion,
                Estado = "Pendiente"
            };

            _context.RecogidasDomicilios.Add(nueva);

            _context.SaveChanges();

            TempData["Mensaje"] = "Recogida solicitada correctamente.";

            return RedirectToAction(nameof(MisRecogidas));
        }

        //=====================================================
        // MIS RECOGIDAS
        //=====================================================

        public IActionResult MisRecogidas()
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            if (usuarioId == null)
                return RedirectToAction("Login", "Usuarios");

            var lista = _context.RecogidasDomicilios
                .Include(r => r.Centro)
                .Where(r => r.UsuarioId == usuarioId.Value)
                .OrderByDescending(r => r.FechaSolicitud)
                .ToList();

            return View(lista);
        }
    }
}