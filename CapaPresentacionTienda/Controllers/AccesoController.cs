using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CapaIdentidad;
using CapaNegocio;
namespace CapaPresentacionTienda.Controllers
{
    public class AccesoController : Controller
    {
        //Formato de contra
        string contraFormato = @"^(?=.*[A-Z])(?=.*[\W_]).{8,}$";
        // GET: Acceso
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Registrar()
        {
            return View();
        }
        public ActionResult Restablecer()
        {
            return View();
        }
        public ActionResult CambiarClave()
        {
            return View();
        }
        public ActionResult prueba() {
            return View();
        }
        public ActionResult pruebaRegistrar()
        {
            return View();
        }
        public ActionResult pruebaCambiarClave()
        {
            return View();
        }
        public ActionResult pruebaRestablecer()
        {
            return View();
        }

        //Devuelve un objeto
        [HttpPost]
        public ActionResult Registrar(Cliente objeto)
        {

            //Formato de contraseña
            string correoFormato = @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{3,}$";

       

            int resultado;
            string mensaje = string.Empty;
            //Almacenar temporalmente la infomración
            ViewData["nombre"] = string.IsNullOrEmpty(objeto.Nombres) ? "" : objeto.Nombres;
            ViewData["correo"] = string.IsNullOrEmpty(objeto.Correo) ? "" : objeto.Correo;
            ViewData["apellidos"] = string.IsNullOrEmpty(objeto.Apellidos) ? "" : objeto.Apellidos;
            if (Regex.IsMatch(objeto.Clave, contraFormato))
            {
                if (objeto.Clave != objeto.ConfirmarClave)
                {
                    ViewBag.Error = "Las contraseñas no coinciden.";
                    return View();
                }

            }

            if (!(Regex.IsMatch(objeto.Clave, contraFormato)))
            {
                ViewBag.Error += "Las contraseña debe tener mayúsculas, minúsculas y caracteres especiales."+"\n";
                return View();
            }

            if (!(Regex.IsMatch(objeto.Correo, correoFormato)))
            {
                ViewBag.Error += "El formato del correo es incorrecto."+"\n";
                return View();
            }

            resultado = new CN_Cliente().registar(objeto, out mensaje);

            if(resultado > 0)
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
        [HttpPost]
        public ActionResult Index(string correo, string clave)
        {
            

            Cliente ocliente = null;
            ocliente = new CN_Cliente().Listar().Where(item => item.Correo == correo && item.Clave == new CN_Recursos().ConvertirSha256(clave)).FirstOrDefault();
            if (ocliente == null) {
                ViewBag.Error = "Correo o contraseña no son correctas";
                return View();
            }
            
            else
            {
                if (ocliente.Restablecer)
                {
                    TempData["idcliente"] = ocliente.IdCliente;
                    return RedirectToAction("CambiarClave", "Acceso");
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(ocliente.Correo, false);
                    //Declaramos que podemos usar este cliente desde cualquier controladorr
                    Session["Cliente"] = ocliente;
                    ViewBag.Error = null;

                    return RedirectToAction("Index", "Tienda");
                }
            }
          
        }
        [HttpPost]
        public ActionResult Restablecer(string correo)
        {
            Cliente oCliente = new CN_Cliente().Listar().Where(item => item.Correo == correo).FirstOrDefault();
            if (oCliente == null)
            {

                ViewBag.Error = "No se encontró ningún Cliente relacionado con ese correo";
                return View();
            }

            string mensaje = string.Empty;
            bool respuesta = new CN_Cliente().RestablecerClave(oCliente.IdCliente, correo, out mensaje);
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
        [HttpPost]
        public ActionResult CambiarClave(string idcliente, string claveactual, string clavenueva, string confirmarclave)
        {
            Cliente Cliente = new Cliente();
            Cliente = new CN_Cliente().Listar().Where(u => u.IdCliente == int.Parse(idcliente)).FirstOrDefault();
            if (Cliente.Clave != new CN_Recursos().ConvertirSha256(claveactual))
            {
                //Me quedo con la información del id cuando ocurra un error, ya que si no lo hago, 
                //se perderá el valor del id
                TempData["IdCliente"] = idcliente;
                ViewData["vclave"] = "";
                ViewBag.Error = "La clave actual es incorrecta";
                return View();

            }
            if (Regex.IsMatch(clavenueva, contraFormato))
            {
                if (clavenueva != confirmarclave)
                {
                    TempData["IdCliente"] = idcliente;
                    ViewData["vclave"] = claveactual;
                    ViewBag.Error = "Las contraseñas nuevas no coinciden";
                    return View();
                }
            }

            if (!(Regex.IsMatch(clavenueva, contraFormato)))
            {
                TempData["IdCliente"] = idcliente;
                ViewBag.Error = "Las contraseña debe tener mayúsculas, minúsculas y caracteres especiales.";
                return View();
            }

            //Restablecemos el valor de ViewData
            ViewData["vclave"] = "";

            string mensaje = string.Empty;
            //Convertimos la clave en formato sha256 para guardarlo en la BD
            string nuevaClave = new CN_Recursos().ConvertirSha256(clavenueva);

            bool respuesta = new CN_Cliente().CambiarClave(int.Parse(idcliente), nuevaClave, out mensaje);
            if (respuesta)
            {

                return RedirectToAction("Index");
            }
            else
            {
                TempData["IdCliente"] = idcliente;
                ViewBag.Error = mensaje;
                return View();
            }


        }
        public ActionResult CerrarSesion()
        {
            Session["Cliente"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Acceso");



        }
    }
}