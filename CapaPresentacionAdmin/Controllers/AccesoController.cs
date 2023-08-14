using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaIdentidad;
using CapaNegocio;
using System.Web.Security;
using System.Text.RegularExpressions;

namespace CapaPresentacionAdmin.Controllers
{
    public class AccesoController : Controller
    {

        //Formato de correo
        string contraFormato = @"^(?=.*[A-Z])(?=.*[\W_]).{8,}$";
        // GET: Acceso
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CambiarClave()
        {
            return View();
        }
        public ActionResult Restablecer()
        {
            return View();
        }
        //Se ejecuta cuando se da al button submit
        [HttpPost]
        public ActionResult Index(string correo, string clave)
        {
            Usuario usuario = new Usuario();
            usuario = new CN_Usuarios().Listar().Where(u => u.Correo == correo && u.Clave == new CN_Recursos().ConvertirSha256(clave)).FirstOrDefault();

            if (usuario == null)
            {
                //Compartir información hacia nuestra vista
                ViewBag.Error = "Correo o contraseña no correcta";
                return View();
            }
            else
            {
                if (usuario.Restablecer)
                {
                    TempData["IdUsuario"] = usuario.IdUsuario;
                    return RedirectToAction("CambiarClave");
                }
                //Tomo la autenticación por correo
                FormsAuthentication.SetAuthCookie(usuario.Correo, false);

                ViewBag.Error = null;
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public ActionResult CambiarClave(string idusuario, string claveactual, string clavenueva, string confirmarclave)
        {
                Usuario usuario = new Usuario();
            usuario = new CN_Usuarios().Listar().Where(u => u.IdUsuario == int.Parse(idusuario)).FirstOrDefault();
            if (usuario.Clave != new CN_Recursos().ConvertirSha256(claveactual)) {
                //Me quedo con la información del id cuando ocurra un error, ya que si no lo hago, 
                //se perderá el valor del id
                TempData["IdUsuario"] = idusuario;
                ViewData["vclave"] = "";
                ViewBag.Error = "La clave actual es incorrecta";
                return View();

            }

            if (Regex.IsMatch(clavenueva, contraFormato))
            {
                if (clavenueva != confirmarclave)
                {
                    TempData["IdUsuario"] = idusuario;
                    ViewData["vclave"] = claveactual;
                    ViewBag.Error = "Las contraseñas nuevas no coinciden";
                    return View();
                }
            }

            if (!(Regex.IsMatch(clavenueva, contraFormato)))
            {
                ViewBag.Error = "Las contraseña debe tener mayúsculas, minúsculas y caracteres especiales.";
                return View();
            }
            //Restablecemos el valor de ViewData
            ViewData["vclave"] = "";

            string mensaje = string.Empty;
            //Convertimos la clave en formato sha256 para guardarlo en la BD
            string nuevaClave = new CN_Recursos().ConvertirSha256(clavenueva);

            bool respuesta = new CN_Usuarios().CambiarClave(int.Parse(idusuario), nuevaClave, out mensaje);
            if (respuesta)
            {

                return RedirectToAction("Index");
            }
            else
            {
                TempData["IdUsuario"] = idusuario;
                ViewBag.Error = mensaje;
                return View();
            }

        }
        [HttpPost]
        public ActionResult RestablecerClave(string correo)
        {
            Usuario ousuario = new CN_Usuarios().Listar().Where(item => item.Correo == correo).FirstOrDefault();
            if(ousuario == null)
            {

                ViewBag.Error = "No se encontró ningún usuario relacionado con ese correo";
                return View();
            }

            string mensaje = string.Empty;
            bool respuesta = new CN_Usuarios().RestablecerClave(ousuario.IdUsuario, correo, out mensaje);
            if (respuesta)
            {
                ViewBag.Error = null;
                return RedirectToAction("Index", "Acceso");
            }
            else
            {
                ViewBag.Error = mensaje;
                return View();
            }

        }
        public ActionResult CerrarSesion()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Acceso");

           
            
        }


    }

}

