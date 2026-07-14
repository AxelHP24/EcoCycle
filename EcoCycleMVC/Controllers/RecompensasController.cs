using EcoCycleMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcoCycleMVC.Controllers
{
    public class RecompensasController : Controller
    {
        private readonly EcoCycleContext _context;

        public RecompensasController(EcoCycleContext context)
        {
            _context = context;
        }

        //=====================================================
        // LISTAR RECOMPENSAS
        //=====================================================

        public IActionResult Index()
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            if (usuarioId == null)
                return RedirectToAction("Login", "Usuarios");

            var usuario = _context.Usuarios
                                  .FirstOrDefault(x => x.UsuarioId == usuarioId);

            ViewBag.Puntos = usuario?.Puntos ?? 0;

            var recompensas = _context.Recompensas
                .Where(r => r.Activa == true)
                .OrderBy(r => r.PuntosNecesarios)
                .ToList();

            return View(recompensas);
        }

        //=====================================================
        // CANJEAR RECOMPENSA
        //=====================================================

        [HttpGet]
        public IActionResult Canjear(int id)
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            if (usuarioId == null)
                return RedirectToAction("Login", "Usuarios");

            var usuario = _context.Usuarios
                .FirstOrDefault(x => x.UsuarioId == usuarioId);

            var recompensa = _context.Recompensas
                .FirstOrDefault(x => x.RecompensaId == id);

            if (usuario == null || recompensa == null)
                return RedirectToAction(nameof(Index));

            if (recompensa.Stock <= 0)
            {
                TempData["Error"] = "La recompensa ya no tiene existencia.";
                return RedirectToAction(nameof(Index));
            }

            if ((usuario.Puntos ?? 0) < recompensa.PuntosNecesarios)
            {
                TempData["Error"] = "No tienes suficientes puntos.";
                return RedirectToAction(nameof(Index));
            }

            //=========================================
            // DESCONTAR PUNTOS
            //=========================================

            usuario.Puntos -= recompensa.PuntosNecesarios;

            //=========================================
            // DESCONTAR STOCK
            //=========================================

            recompensa.Stock--;

            //=========================================
            // REGISTRAR CANJE
            //=========================================

            var canje = new Canje
            {
                UsuarioId = usuario.UsuarioId,
                RecompensaId = recompensa.RecompensaId,
                FechaCanje = DateTime.Now,
                Estado = "Canjeado",
                CodigoCupon = Guid.NewGuid().ToString().Substring(0, 8).ToUpper()
            };

            _context.Canjes.Add(canje);

            //=========================================
            // MOVIMIENTO DE PUNTOS
            //=========================================

            var movimiento = new MovimientosPunto
            {
                UsuarioId = usuario.UsuarioId,
                Puntos = -recompensa.PuntosNecesarios,
                TipoMovimiento = "Canje",
                Descripcion = "Canje de recompensa: " + recompensa.Nombre,
                FechaMovimiento = DateTime.Now
            };

            _context.MovimientosPuntos.Add(movimiento);

            _context.SaveChanges();

            TempData["Mensaje"] =
                "¡Recompensa canjeada correctamente! Código: " +
                canje.CodigoCupon;

            return RedirectToAction(nameof(Index));
        }
    }
}