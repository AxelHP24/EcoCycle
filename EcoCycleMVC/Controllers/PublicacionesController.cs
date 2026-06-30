using EcoCycleMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EcoCycleMVC.Controllers
{
    public class PublicacionesController : Controller
    {
        private readonly EcoCycleContext _context;

        public PublicacionesController(EcoCycleContext context)
        {
            _context = context;
        }

        //=====================================
        // CREAR PUBLICACIÓN (GET)
        //=====================================

        [HttpGet]
        public IActionResult Crear()
        {
            var modelo = new NuevaPublicacionViewModel();

            modelo.Materiales = _context.Materiales
                .Select(m => new SelectListItem
                {
                    Value = m.MaterialId.ToString(),
                    Text = m.NombreMaterial
                })
                .ToList();

            return View(modelo);
        }

        //=====================================
        // CREAR PUBLICACIÓN (POST)
        //=====================================

        [HttpPost]
        public IActionResult Crear(NuevaPublicacionViewModel modelo)
        {
            modelo.Materiales = _context.Materiales
                .Select(m => new SelectListItem
                {
                    Value = m.MaterialId.ToString(),
                    Text = m.NombreMaterial
                })
                .ToList();

            if (!ModelState.IsValid)
                return View(modelo);

            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            if (usuarioId == null)
                return RedirectToAction("Login", "Usuarios");

            // Buscar el material seleccionado
            var material = _context.Materiales
                .FirstOrDefault(m => m.MaterialId == modelo.MaterialId);

            if (material == null)
                return View(modelo);

            // Crear publicación
            Publicacione nueva = new Publicacione
            {
                UsuarioId = usuarioId.Value,
                MaterialId = modelo.MaterialId,
                Descripcion = modelo.Descripcion,
                CantidadKg = modelo.CantidadKg,
                Ubicacion = modelo.Ubicacion,
                FechaPublicacion = DateTime.Now,
                Estado = "Disponible"
            };

            _context.Publicaciones.Add(nueva);

            //=========================================
            // CALCULAR PUNTOS
            //=========================================

            int puntosGanados = (int)(modelo.CantidadKg * material.PuntosPorKg);

            var usuario = _context.Usuarios
                .FirstOrDefault(u => u.UsuarioId == usuarioId.Value);

            if (usuario != null)
            {
                usuario.Puntos = (usuario.Puntos ?? 0) + puntosGanados;
            }

            //=========================================
            // REGISTRAR MOVIMIENTO
            //=========================================

            MovimientosPunto movimiento = new MovimientosPunto
            {
                UsuarioId = usuarioId.Value,
                Puntos = puntosGanados,
                TipoMovimiento = "Ganados",
                Descripcion = "Publicación de material reciclable",
                FechaMovimiento = DateTime.Now
            };

            _context.MovimientosPuntos.Add(movimiento);

            _context.SaveChanges();

            TempData["Mensaje"] =
                $"¡Publicación realizada! Has ganado {puntosGanados} puntos.";

            return RedirectToAction(nameof(MisPublicaciones));
        }

        //=====================================
        // MIS PUBLICACIONES
        //=====================================

        public IActionResult MisPublicaciones()
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            if (usuarioId == null)
                return RedirectToAction("Login", "Usuarios");

            var publicaciones = _context.Publicaciones
                .Include(p => p.Material)
                .Where(p => p.UsuarioId == usuarioId.Value)
                .OrderByDescending(p => p.FechaPublicacion)
                .ToList();

            return View(publicaciones);
        }

        //=====================================
        // EDITAR (GET)
        //=====================================

        [HttpGet]
        public IActionResult Editar(int id)
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            if (usuarioId == null)
                return RedirectToAction("Login", "Usuarios");

            var publicacion = _context.Publicaciones
                .FirstOrDefault(x => x.PublicacionId == id &&
                                     x.UsuarioId == usuarioId.Value);

            if (publicacion == null)
                return NotFound();

            var modelo = new NuevaPublicacionViewModel
            {
                MaterialId = publicacion.MaterialId,
                CantidadKg = publicacion.CantidadKg ?? 0,
                Descripcion = publicacion.Descripcion ?? "",
                Ubicacion = publicacion.Ubicacion ?? ""
            };

            modelo.Materiales = _context.Materiales
                .Select(m => new SelectListItem
                {
                    Value = m.MaterialId.ToString(),
                    Text = m.NombreMaterial
                })
                .ToList();

            ViewBag.PublicacionId = id;

            return View(modelo);
        }

        //=====================================
        // EDITAR (POST)
        //=====================================

        [HttpPost]
        public IActionResult Editar(int id, NuevaPublicacionViewModel modelo)
        {
            modelo.Materiales = _context.Materiales
                .Select(m => new SelectListItem
                {
                    Value = m.MaterialId.ToString(),
                    Text = m.NombreMaterial
                })
                .ToList();

            if (!ModelState.IsValid)
            {
                ViewBag.PublicacionId = id;
                return View(modelo);
            }

            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            if (usuarioId == null)
                return RedirectToAction("Login", "Usuarios");

            var publicacion = _context.Publicaciones
                .FirstOrDefault(x => x.PublicacionId == id &&
                                     x.UsuarioId == usuarioId.Value);

            if (publicacion == null)
                return NotFound();

            publicacion.MaterialId = modelo.MaterialId;
            publicacion.CantidadKg = modelo.CantidadKg;
            publicacion.Descripcion = modelo.Descripcion;
            publicacion.Ubicacion = modelo.Ubicacion;

            _context.SaveChanges();

            TempData["Mensaje"] = "Publicación actualizada correctamente.";

            return RedirectToAction(nameof(MisPublicaciones));
        }

        //=====================================
        // ELIMINAR
        //=====================================

        [HttpGet]
        public IActionResult Eliminar(int id)
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            if (usuarioId == null)
                return RedirectToAction("Login", "Usuarios");

            var publicacion = _context.Publicaciones
                .FirstOrDefault(x => x.PublicacionId == id &&
                                     x.UsuarioId == usuarioId.Value);

            if (publicacion == null)
                return NotFound();

            _context.Publicaciones.Remove(publicacion);

            _context.SaveChanges();

            TempData["Mensaje"] = "Publicación eliminada correctamente.";

            return RedirectToAction(nameof(MisPublicaciones));
        }
    }
}