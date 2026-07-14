using EcoCycleMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcoCycleMVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly EcoCycleContext _context;

        public AdminController(EcoCycleContext context)
        {
            _context = context;
        }

        // Dashboard
        public IActionResult Dashboard()
        {
            return View();
        }

        // Usuarios
        public IActionResult Usuarios()
        {
            return View(_context.Usuarios.ToList());
        }

        // Materiales
        public IActionResult Materiales()
        {
            return View(_context.Materiales.ToList());
        }

        // Publicaciones
        public IActionResult Publicaciones()
        {
            var publicaciones = _context.Publicaciones
                .Include(p => p.Usuario)
                .Include(p => p.Material)
                .ToList();

            return View(publicaciones);
        }

        // Recolecciones
        public IActionResult Recolecciones()
        {
            var lista = _context.RecogidasDomicilios
                .Include(r => r.Usuario)
                .Include(r => r.Centro)
                .ToList();

            return View(lista);
        }

        // Centros
        public IActionResult Centros()
        {
            return View(_context.CentrosRecoleccions.ToList());
        }

        // Canjes
        public IActionResult Canjes()
        {
            var lista = _context.Canjes
                .Include(c => c.Usuario)
                .Include(c => c.Recompensa)
                .ToList();

            return View(lista);
        }
    }
}