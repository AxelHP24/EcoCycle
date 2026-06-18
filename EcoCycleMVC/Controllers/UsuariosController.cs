using EcoCycleMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace EcoCycleMVC.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly EcoCycleContext _context;

        public UsuariosController(EcoCycleContext context)
        {
            _context = context;
        }

        //LOGIN
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string correo,string password)
        {

            var usuario=_context.Usuarios
            .FirstOrDefault(x=>x.Correo==correo && x.PasswordHash==password);

            if(usuario==null)
            {
                ViewBag.Error="Correo o contraseña incorrectos";
                return View();
            }

            return RedirectToAction("Dashboard");
        }

        //REGISTRO

        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registro(Usuario usuario)
        {

            if(ModelState.IsValid)
            {

                usuario.PasswordHash=usuario.PasswordHash;
                usuario.Puntos=0;
                usuario.Activo=true;

                _context.Usuarios.Add(usuario);
                _context.SaveChanges();

                return RedirectToAction("Login");
            }

            return View(usuario);

        }

        public IActionResult Dashboard()
        {
            return View();
        }

    }
}