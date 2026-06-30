using EcoCycleMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcoCycleMVC.Controllers
{
    public class CentrosController : Controller
    {
        private readonly EcoCycleContext _context;

        public CentrosController(EcoCycleContext context)
        {
            _context = context;
        }

        // =========================
        // LISTA DE CENTROS (DINÁMICO)
        // =========================
        public IActionResult Index()
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            if (usuarioId == null)
                return RedirectToAction("Login", "Usuarios");

            var ubicaciones = _context.Publicaciones
                .Where(p => p.Ubicacion != null && p.Ubicacion != "")
                .Select(p => new
                {
                    p.Ubicacion
                })
                .Distinct()
                .ToList();

            return View(ubicaciones);
        }
    }
}