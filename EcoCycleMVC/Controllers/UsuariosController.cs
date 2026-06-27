using EcoCycleMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace EcoCycleMVC.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly EcoCycleContext _context;

        public UsuariosController(EcoCycleContext context)
        {
            _context = context;
        }

        //=========================================
        // REGISTRO
        //=========================================

        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registro(Usuario usuario)
        {
            if (!ModelState.IsValid)
                return View(usuario);

            // Verificar si el correo ya existe
            if (_context.Usuarios.Any(x => x.Correo == usuario.Correo))
            {
                ViewBag.Error = "Ese correo ya está registrado.";
                return View(usuario);
            }

            usuario.FechaRegistro = DateTime.Now;
            usuario.Puntos = 0;
            usuario.TipoUsuario = "Ciudadano";
            usuario.Activo = true;

            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            TempData["Mensaje"] = "Usuario registrado correctamente.";

            return RedirectToAction("Login");
        }

        //=========================================
        // LOGIN
        //=========================================

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string correo, string password)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u =>
                u.Correo == correo &&
                u.PasswordHash == password);

            if (usuario == null)
            {
                ViewBag.Error = "Correo o contraseña incorrectos.";
                return View();
            }

            // Guardar sesión
            HttpContext.Session.SetInt32("UsuarioId", usuario.UsuarioId);
            HttpContext.Session.SetString("Nombre", usuario.Nombre);

            return RedirectToAction("Dashboard");
        }

        //=========================================
        // DASHBOARD
        //=========================================

        public IActionResult Dashboard()
        {
            int? usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            if (usuarioId == null)
            {
                return RedirectToAction("Login");
            }

            var usuario = _context.Usuarios.FirstOrDefault(u => u.UsuarioId == usuarioId);

            if (usuario == null)
            {
                return RedirectToAction("Login");
            }

            return View(usuario);
        }

        //=========================================
        // LOGOUT
        //=========================================

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }
    }
}