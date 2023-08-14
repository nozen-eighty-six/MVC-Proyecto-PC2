 using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaIdentidad;
using CapaNegocio;
using Newtonsoft.Json;


namespace CapaPresentacionAdmin.Controllers
{
    [Authorize]
    public class MantenedorController : Controller
    {
        // GET: Mantenedor
        public ActionResult Categoria()//Nos muestre el formulario de categoría

        {
            return View();
        }
        public ActionResult Marca()//Nos muestre el formulario de categoría

        {
            return View();
        }
        public ActionResult Producto()//Nos muestre el formulario de categoría

        {
            return View();
        }
        //*************************CATEGORIA*************************
        #region CATEGORIA
        [HttpGet]
        public JsonResult ListarCategoria()
        {
            List<Categoria> olista = new List<Categoria>();
            olista = new CN_Categoria().Listar();

            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        //Se usa un objeto Categoria, ya que en el HTML le vamos a mandar una Categoria
        //Vacío, como se sabe el Modal al querer registrar un nuevo usuario, el campo
        //txtid tiene valor cero, y ese es asignado al objeto, con ello se puedo hacer 
        //la validación si es para registrar o editar, ya que vamos a invocar al método desde la vista y
        // en la vista se analiza si cumple o no
        public JsonResult GuardarCategoria(Categoria objeto)//Guardar y editar
        {
            object resultado; //Nos permite almacendar cualquier tipo de dato
            string mensaje = string.Empty;
            if (objeto.IdCategoria == 0)
            {
                //Almacenamos el id de la categoria
                resultado = new CN_Categoria().registar(objeto, out mensaje);
            }
            else
            {
                resultado = new CN_Categoria().editarCategoria(objeto, out mensaje);//Se devuelve un mensaje cuando se ejecuta el procedimiento
            }
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
            //Los elementos resultado y  mensaje, serán recibidos por un solo parámetro
        }
        [HttpPost]
        public JsonResult EliminarCategoria(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Categoria().eliminarCategoria(id, out mensaje);

            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        //************************* MARCA *************************
        #region MARCA
        [HttpGet]
        public JsonResult ListarMarca()
        {
            List<Marca> olista = new List<Marca>();
            olista = new CN_Marca().Listar();

            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        //Se usa un objeto Categoria, ya que en el HTML le vamos a mandar una Categoria
        //Vacío, como se sabe el Modal al querer registrar un nuevo usuario, el campo
        //txtid tiene valor cero, y ese es asignado al objeto, con ello se puedo hacer 
        //la validación si es para registrar o editar, ya que vamos a invocar al método desde la vista y
        // en la vista se analiza si cumple o no
        public JsonResult GuardarMarca(Marca objeto)//Guardar y editar
        {
            object resultado; //Nos permite almacendar cualquier tipo de dato
            string mensaje = string.Empty;
            if (objeto.IdMarca == 0)
            {
                //Almacenamos el id de la categoria
                resultado = new CN_Marca().registar(objeto, out mensaje);
            }
            else
            {
                resultado = new CN_Marca().editarMarca(objeto, out mensaje);//Se devuelve un mensaje cuando se ejecuta el procedimiento
            }
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
            //Los elementos resultado y  mensaje, serán recibidos por un solo parámetro
        }
        [HttpPost]
        public JsonResult EliminarMarca(int id)
        {

            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Marca().eliminarMarca(id, out mensaje);

            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ListarMarcaPorCategoria(int idcategoria)
        {
            List<Marca> olista = new List<Marca>();
            olista = new CN_Marca().listarMarcaPorCategoria(idcategoria);

            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
        }
        #endregion


        //************************* PRODUCTO *************************
        #region PRODUCTO
        [HttpGet]
        public JsonResult ListarProducto()
        {
            List<Producto> olista = new List<Producto>();
            olista = new CN_Producto().Listar();

            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        //Se usa un objeto Categoria, ya que en el HTML le vamos a mandar una Categoria
        //Vacío, como se sabe el Modal al querer registrar un nuevo usuario, el campo
        //txtid tiene valor cero, y ese es asignado al objeto, con ello se puedo hacer 
        //la validación si es para registrar o editar, ya que vamos a invocar al método desde la vista y
        // en la vista se analiza si cumple o no
        public JsonResult GuardarProducto(string objeto, HttpPostedFileBase archivoImagen)//Guardar y editar
        {
            object resultado; //Nos permite almacendar cualquier tipo de dato
            string mensaje = string.Empty;
            bool operacion_exitosa = true;
            bool guardar_imagen_exito = true;

            //Convertimos a un objeto de tipo Producto a la cadena objeto
            Producto oproducto = new Producto();
            oproducto = JsonConvert.DeserializeObject<Producto>(objeto);

            //Validamos el precio a la moneda local
            decimal precio;
            if (decimal.TryParse(oproducto.PrecioTexto, System.Globalization.NumberStyles.AllowDecimalPoint, new CultureInfo("es-PE"), out precio))
            {//Que se pueda convertir a decimales en  punto según la región
                oproducto.Precio = precio;
            }
            else {
                return Json(new { operacionExitosa = false, mensaje = "El formato del precio debe ser ##-##" }, JsonRequestBehavior.AllowGet);

            }
            if (oproducto.IdProducto == 0)
            {
                //Almacenamos el id de la categoria
                int idProductoGenerado = new CN_Producto().registar(oproducto, out mensaje);
                if(idProductoGenerado != 0)
                {
                    oproducto.IdProducto = idProductoGenerado;
                }
                else
                {
                    operacion_exitosa = false;
                }
            }
            else
            {
                operacion_exitosa = new CN_Producto().editarProducto(oproducto, out mensaje);//Se devuelve un mensaje cuando se ejecuta el procedimiento
            }
            //Si se cumple guardar o editar producto, pasamos a agregar la img
            if (operacion_exitosa)
            {
                if (archivoImagen != null) {

                    string ruta_guardar = ConfigurationManager.AppSettings["ServidorFotos"];
                    string extension = Path.GetExtension(archivoImagen.FileName);//Obtenemos la extesnión
                    //Creamos el nombre de la img
                    string nombre_imagen = string.Concat(oproducto.IdProducto, extension);//El id con la extensión de la imagen, la variable superior
                    try
                    {
                        archivoImagen.SaveAs(Path.Combine(ruta_guardar, nombre_imagen));//Combinar la ruta, como si  se guardará una img en una carpeta
                    } 
                    catch (Exception ex)
                    {
                        string msg = ex.Message;
                        guardar_imagen_exito = false;
                    }
                    if (guardar_imagen_exito)
                    {
                        oproducto.RutaImagen = ruta_guardar;
                        oproducto.NombreImagen = nombre_imagen;
                        bool rpta = new CN_Producto().GuardarDatosImg(oproducto, out mensaje);
                    }
                    else
                    {
                        mensaje = "Se guardo el producto, pero hubo problemas con la imagen";
                    }

                }
            }
            return Json(new { operacionExitosa = operacion_exitosa, idGnerado = oproducto.IdProducto, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
            //Los elementos resultado y  mensaje, serán recibidos por un solo parámetro
        }
        [HttpPost]
        public JsonResult EliminarProducto(int id)
        {

            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Producto().eliminarProducto(id, out mensaje);

            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult imagenProducto(int id)
        {
            bool conversion;
            string extension;
            Producto oproducto = new CN_Producto().Listar().Where(p => p.IdProducto == id).FirstOrDefault();

            string textoBase64 = CN_Recursos.convertirBase64(Path.Combine(oproducto.RutaImagen, oproducto.NombreImagen), out conversion);
            extension = Path.GetExtension(oproducto.NombreImagen);
            return Json(new { conversion = conversion, textoBase64 = textoBase64, extension = extension }, JsonRequestBehavior.AllowGet);
            //conversion devuelve boll, textoBase64 devuelve la cadena y la extensión de la imagen
        }

        //    [HttpPost]
        //    public JsonResult GuardarImagen(Producto obj)
        //    {
        //        bool respueta;

        //       bool respuesta = new CN_Producto()-GuardarImagen(obj,)
        //        return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        //    }
        //}
        #endregion

    }
}