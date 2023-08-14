using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaIdentidad;
using CapaNegocio;
using ClosedXML.Excel;

namespace CapaPresentacionAdmin.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }

        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}
        //public ActionResult PruebaTest()
        //{


        //    return View();
        //}

        public ActionResult Usuarios()
        {
            return View();
        }
        [HttpGet]
        public JsonResult ListarUsuarios()
        {
            List<Usuario> olista = new List<Usuario>();
            olista = new CN_Usuarios().Listar();

            return Json(new { data = olista}, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        //Se usa un objeto Usuario, ya que en el HTML le vamos a mandar un Usuario
        //Vacío, como se sabe el Modal al querer registrar un nuevo usuario, el campo
        //txtid tiene valor cero, y ese es asignado al objeto, con ello se puedo hacer 
        //la validación si es para registrar o editar
        public JsonResult GuardarUsuario(Usuario objeto)//Guardar y editar
        {
            object resultado; //Nos permite almacendar cualquier tipo de dato
            string mensaje = string.Empty;
            if(objeto.IdUsuario == 0)
            {
                //Almacenamos el id del usuario
                resultado = new CN_Usuarios().registar(objeto,out mensaje);
            }
            else
            {
                resultado = new CN_Usuarios().editarUsuario(objeto, out mensaje);//Se devuelve un mensaje cuando se ejecuta el procedimiento
            }
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
            //Los elementos resultado y  mensaje, serán recibidos por un solo parámetro
        }
        [HttpPost]
        public JsonResult EliminarUsuario(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Usuarios().eliminarUsuario(id,out mensaje);

            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarReportes()
        {
            DashBoard objeto= new CN_Reporte().VerDashBoard();
            

            return Json(new { resultado = objeto }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarReporte(string fechainicio,string fechafin, string idtransaccion)
        {
            List<Reporte> olista = new CN_Reporte().Ventas(fechainicio, fechafin, idtransaccion);


            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public FileResult ExportarVenta(string fechainicio, string fechafin, string idtransaccion)
        {
            List<Reporte> olista = new List<Reporte>();
            olista = new CN_Reporte().Ventas(fechainicio, fechafin, idtransaccion);
            DataTable dt = new DataTable();
            //Configuración de la región
            dt.Locale = new System.Globalization.CultureInfo("es-PE");
            //Agrego columnas
            dt.Columns.Add("Fecha venta", typeof(string));
            dt.Columns.Add("Cliente", typeof(string));
            dt.Columns.Add("Producto", typeof(string));
            dt.Columns.Add("Precio", typeof(decimal));
            dt.Columns.Add("Cantidad", typeof(int));
            dt.Columns.Add("Total", typeof(decimal));
            dt.Columns.Add("IdTransaccion", typeof(string));

            //Agregamos los datos en la tabla
            foreach (Reporte rp in olista) {
                dt.Rows.Add(new object[]{
                    rp.FechaVenta,
                    rp.Cliente,
                    rp.Producto,
                    rp.Precio,
                    rp.Cantidad,
                    rp.Total,
                    rp.IdTransaccion

                });
            }
            //Nombre la tabla
            dt.TableName = "Datos";

            //Conf. la exportación
            using (XLWorkbook wb = new XLWorkbook()) {
                //Agregamos nuestra tabla
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream()) {
                    //Guardamos este documento en este steam
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReporteVenta" + DateTime.Now.ToString() + ".xlsx");

                }

            }

               
        }


    }
}