using EcoCycleMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
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

            // Hashear contraseña
            var passwordHasher = new PasswordHasher<Usuario>();
            usuario.PasswordHash = passwordHasher.HashPassword(usuario, usuario.PasswordHash);

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
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Correo == correo);

            if (usuario == null)
            {
                ViewBag.Error = "Correo o contraseña incorrectos.";
                return View();
            }

            var passwordHasher = new PasswordHasher<Usuario>();

            var resultado = passwordHasher.VerifyHashedPassword(
                usuario,
                usuario.PasswordHash,
                password);

            if (resultado == PasswordVerificationResult.Failed)
            {
                ViewBag.Error = "Correo o contraseña incorrectos.";
                return View();
            }

            if (usuario.Activo != true)
            {
                ViewBag.Error = "Tu cuenta está desactivada.";
                return View();
            }

            // Guardar sesión
            HttpContext.Session.SetInt32("UsuarioId", usuario.UsuarioId);
            HttpContext.Session.SetString("Nombre", usuario.Nombre);
            HttpContext.Session.SetString("Correo", usuario.Correo);
            HttpContext.Session.SetString("TipoUsuario", usuario.TipoUsuario ?? "Ciudadano");

            // Redireccionar según el rol
            if (usuario.TipoUsuario == "Admin")
            {
                return RedirectToAction("DashboardAdmin");
            }

            return RedirectToAction("Dashboard");
        }

        //=========================================
        // DASHBOARD CIUDADANO
        //=========================================

        public IActionResult Dashboard()
        {
            int? usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            if (usuarioId == null)
                return RedirectToAction("Login");

            if (HttpContext.Session.GetString("TipoUsuario") != "Ciudadano")
                return RedirectToAction("DashboardAdmin");

            var usuario = _context.Usuarios.FirstOrDefault(u => u.UsuarioId == usuarioId);

            if (usuario == null)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Login");
            }

            return View(usuario);
        }

        //=========================================
        // DASHBOARD ADMIN
        //=========================================

        public IActionResult DashboardAdmin()
        {
            int? usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            if (usuarioId == null)
                return RedirectToAction("Login");

            if (HttpContext.Session.GetString("TipoUsuario") != "Admin")
                return RedirectToAction("Dashboard");

            var usuario = _context.Usuarios.FirstOrDefault(u => u.UsuarioId == usuarioId);

            if (usuario == null)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Login");
            }

            return View(usuario);
        }

        //=========================================
        // EDITAR PERFIL (GET)
        //=========================================

        [HttpGet]
        public IActionResult EditarPerfil()
        {
            int? usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            if (usuarioId == null)
                return RedirectToAction("Login");

            var usuario = _context.Usuarios.FirstOrDefault(u => u.UsuarioId == usuarioId);

            if (usuario == null)
                return RedirectToAction("Login");

            return View(usuario);
        }

        //=========================================
        // EDITAR PERFIL (POST)
        //=========================================

        [HttpPost]
        public IActionResult EditarPerfil(Usuario modelo)
        {
            int? usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            if (usuarioId == null)
                return RedirectToAction("Login");

            var usuario = _context.Usuarios.FirstOrDefault(u => u.UsuarioId == usuarioId);

            if (usuario == null)
                return RedirectToAction("Login");

            usuario.Nombre = modelo.Nombre;
            usuario.Telefono = modelo.Telefono;
            usuario.Direccion = modelo.Direccion;

            _context.SaveChanges();

            TempData["Mensaje"] = "Perfil actualizado correctamente.";

            if (usuario.TipoUsuario == "Admin")
                return RedirectToAction("DashboardAdmin");

            return RedirectToAction("Dashboard");
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