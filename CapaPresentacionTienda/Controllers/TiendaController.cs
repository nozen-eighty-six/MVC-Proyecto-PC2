using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CapaIdentidad;
using CapaNegocio;
using CapaIdentidad.Paypal;
using CapaPresentacionTienda.Filter;

namespace CapaPresentacionTienda.Controllers
{
    public class TiendaController : Controller
    {
        // GET: Tienda
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DetalleProducto(int idproducto = 0)
        {
            Producto oproducto = new Producto();
            bool conversion;
            oproducto = new CN_Producto().Listar().Where(p => p.IdProducto == idproducto).FirstOrDefault();
            if(oproducto != null)
            {
                oproducto.Base64 = CN_Recursos.convertirBase64(Path.Combine(oproducto.RutaImagen, oproducto.NombreImagen), out conversion);
                oproducto.Extension = Path.GetExtension(oproducto.NombreImagen);
            }
            return Json(new { data = oproducto }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ListarCategorias()
        {
            List<Categoria> olista = new List<Categoria>();
            olista = new CN_Categoria().Listar();

            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ListarMarcaPorCategoria(int idcategoria)
        {
            List<Marca> olista = new List<Marca>();
            olista = new CN_Marca().listarMarcaPorCategoria(idcategoria);

            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarProducto(int idcategoria, int idmarca)
        {
            List<Producto> lista = new List<Producto>();
            bool conversion;

            lista = new CN_Producto().Listar().Select(p => new Producto()
            {
                IdProducto = p.IdProducto,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                oMarca = p.oMarca,
                oCategoria = p.oCategoria,
                Precio = p.Precio,
                Stock = p.Stock,
                RutaImagen = p.RutaImagen,
                Base64 = CN_Recursos.convertirBase64(Path.Combine(p.RutaImagen, p.NombreImagen), out conversion),
                Extension = Path.GetExtension(p.NombreImagen),
                Activo = p.Activo


            }).Where(p => p.oCategoria.IdCategoria == (idcategoria == 0 ? p.oCategoria.IdCategoria : idcategoria) &&
            p.oMarca.IdMarca == (idmarca == 0 ? p.oMarca.IdMarca : idmarca) && p.Stock > 0 && p.Activo == true
            ).ToList();

            var jsonresult = Json(new { data = lista }, JsonRequestBehavior.AllowGet);
            //El json tenga límite en el contenido
            jsonresult.MaxJsonLength = int.MaxValue;
            return jsonresult;

        }

        [HttpGet]
        public JsonResult ListarProductoCategoria()
        {
            List<Producto> olista = new List<Producto>();
            olista = new CN_Producto().ListarProductoXCategoria();
            

            var jsonresult= Json(new { data = olista }, JsonRequestBehavior.AllowGet);
            //El json tenga límite en el contenido
            jsonresult.MaxJsonLength = int.MaxValue;
            return jsonresult;
        }
        [HttpPost]
        public JsonResult AgregarCarrito(int idproducto)
        {
            int idcliente =((Cliente)Session["Cliente"]).IdCliente;
            bool existe = new CN_Carrito().ExisteCarrito(idcliente, idproducto);

            bool respuesta = false;
            string mensaje = string.Empty;

            if (existe)
            {
                mensaje = "El producto ya existe en el carrito";
            }
            else
            {
                respuesta = new CN_Carrito().OperacionCarrito(idcliente, idproducto, true, out mensaje);
            }

            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult CantidadEnCarrito()
        {
            //Saber quien ha accedido
            int idcliente = ((Cliente)Session["Cliente"]).IdCliente;

            int cantidad = new CN_Carrito().CantidadEnCarrito(idcliente);
            return Json(new { cantidad = cantidad }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]

        public JsonResult listarProductoCarrito()
        {
            int idcliente = ((Cliente)Session["Cliente"]).IdCliente;
            List<Carrito> olista = new List<Carrito>();
            bool conversion;


            olista = new CN_Carrito().ListarProducto(idcliente).Select(oc => new Carrito()
            {
                oProducto = new Producto()
                {
                    IdProducto = oc.oProducto.IdProducto,
                    Nombre = oc.oProducto.Nombre,
                    oMarca = oc.oProducto.oMarca,
                    Precio = oc.oProducto.Precio,
                    RutaImagen = oc.oProducto.RutaImagen,
                    Base64 = CN_Recursos.convertirBase64(Path.Combine(oc.oProducto.RutaImagen, oc.oProducto.NombreImagen), out conversion),
                    Extension = Path.GetExtension(oc.oProducto.NombreImagen)


                },
                Cantidad = oc.Cantidad
            }).ToList();

            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult OperacionCarrito(int idproducto, bool sumar)
        {
            int idcliente = ((Cliente)Session["Cliente"]).IdCliente;

            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Carrito().OperacionCarrito(idcliente, idproducto, sumar, out mensaje);

            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarCarrito(int idproducto)
        {
            int idcliente = ((Cliente)Session["Cliente"]).IdCliente;
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Carrito().EliminarCarrito(idcliente, idproducto);
            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult obtenerDepartamento()
        {
            List<Departamento> olista = new List<Departamento>();
            olista = new CN_Ubicacion().ObtenerDepartamento();
            return Json(new { lista = olista }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult obtenerProvincia(string idDepartamento)
        {
            List<Provincia> olista = new List<Provincia>();
            olista = new CN_Ubicacion().ObtenerProvincia(idDepartamento);
            return Json(new { lista = olista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult obtenerDistrito(string idDepartamento, string idProvincia)
        {
            List<Distrito> olista = new List<Distrito>();
            olista = new CN_Ubicacion().ObtenerDistrito(idDepartamento, idProvincia);
            return Json(new { lista = olista }, JsonRequestBehavior.AllowGet);
        }


        //Vista
        [ValidarSesion]
        [Authorize]
        public ActionResult Carrito()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> ProcesarPago(List<Carrito> olistaCarrito, Venta oVenta)
        {
            decimal total = 0;

            DataTable detalle_venta = new DataTable();
            detalle_venta.Locale = new CultureInfo("es-PE");//La cultura
            detalle_venta.Columns.Add("IdProducto", typeof(string));
            detalle_venta.Columns.Add("Cantidad", typeof(int));
            detalle_venta.Columns.Add("Total", typeof(decimal));

            List<Item> oListaItem = new List<Item>();


            //Iterar la información de la lista Carrito

            foreach (Carrito ocarrito in olistaCarrito)
            {
                decimal subTotal = Convert.ToDecimal(ocarrito.Cantidad.ToString()) * ocarrito.oProducto.Precio;

                total += subTotal;//Sumamos cada pago que se va a realizar


                oListaItem.Add(new Item()
                {
                    name = ocarrito.oProducto.Nombre,
                    quantity = ocarrito.Cantidad.ToString(),
                    unit_amount = new UnitAmount()
                    {
                        currency_code = "USD",
                        value = ocarrito.oProducto.Precio.ToString("G", new CultureInfo("es-PE"))
                    }
                });

                //Filas 
                detalle_venta.Rows.Add(new Object[] {
                    ocarrito.oProducto.IdProducto,
                    ocarrito.Cantidad,
                    subTotal
                });
            }
            PurchaseUnit purchaseUnit = new PurchaseUnit()
            {
                //Pasar el monto total de los precios
                amount = new Amount()
                {
                    currency_code = "USD",
                    value = total.ToString("G", new CultureInfo("es-PE")),
                    //Repetimos en breakdown
                    breakdown = new Breakdown()
                    {
                        item_total = new ItemTotal
                        {
                            currency_code = "USD",
                            value = total.ToString("G", new CultureInfo("es-PE"))
                        }
                    }
                },
                description = "Compra de articulo de mi tienda",
                //Pasamos la lista de productos
                items = oListaItem
            };

            //Pasamos todas las configuraciones ya creadas
            Checkout_Order checkout_Order = new Checkout_Order()
            {
                //Porque solo es una solicitud
                intent = "CAPTURE",
                purchase_units = new List<PurchaseUnit>()
                {
                    purchaseUnit
                },
                application_context = new ApplicationContext()
                {
                    //Nombre del sitio web
                    brand_name = "MiTienda.com",
                    landing_page = "NO_PREFERENCE",
                    user_action = "PAY_NOW",
                    //Url en caso nuestra solicitud ha sido aprobadad 
                    //Además en la url nos devolverá el token
                    return_url = "http://localhost:55629/Tienda/PagoEfectuado",
                    cancel_url = "http://localhost:55629/Tienda/Carrito"
                }
            };

            oVenta.MontoTotal = total;
            oVenta.IdCliente =((Cliente)Session["Cliente"]).IdCliente;
            //Almacenar información que puede ser compartida para otros métodos del mismo controlador
            TempData["Venta"] = oVenta;
            TempData["DetalleVenta"] = detalle_venta;

            //Ejecutar los servicios Paypal
            CN_Paypal opaypal = new CN_Paypal();
            Response_Paypal<Response_Checkout> response_Paypal = new Response_Paypal<Response_Checkout>();
            //Recibir la respuesta de la solicitud ejecutada por el método crearSolicitud
            response_Paypal = await opaypal.CrearSolicitud(checkout_Order);

            return Json(response_Paypal, JsonRequestBehavior.AllowGet);
        }

        [ValidarSesion]
        [Authorize]
        public async Task<ActionResult> PagoEfectuado()
        {
            //Obtener los datos de la URL
            string token = Request.QueryString["token"];

            CN_Paypal opaypal = new CN_Paypal();
            Response_Paypal<Response_Capture> response_Paypal = new Response_Paypal<Response_Capture>();
            //Recibir la respuesta del método aprobar pago
            response_Paypal = await opaypal.AprobarPago(token);
            //Guardamos información para usarlo en la vista actual
            ViewData["Status"] = response_Paypal.Status;

            if (response_Paypal.Status)
            {
                Venta oVenta = (Venta)TempData["Venta"];
                DataTable detalle_venta = (DataTable)TempData["DetalleVenta"];
                //Asignamos el idTransaccion del servicio de Payapl
                oVenta.IdTransaccion = response_Paypal.Response.purchase_units[0].payments.captures[0].id;

                string mensaje = string.Empty;
                //Registramos
                bool respuesta = new CN_Venta().Registrar(oVenta, detalle_venta, out mensaje);

                ViewData["IdTransaccion"] = oVenta.IdTransaccion;
            }

            return View();
        }

        [ValidarSesion]
        [Authorize]
        public ActionResult MisCompras()
        {
            int idcliente = ((Cliente)Session["Cliente"]).IdCliente;
            List<DetalleVenta> olista = new List<DetalleVenta>();
            bool conversion;


            olista = new CN_Venta().ListarCompras(idcliente).Select(oc => new DetalleVenta()
            {
                oProducto = new Producto()
                {
                   
                    Nombre = oc.oProducto.Nombre,
                    Precio = oc.oProducto.Precio,
                    Base64 = CN_Recursos.convertirBase64(Path.Combine(oc.oProducto.RutaImagen, oc.oProducto.NombreImagen), out conversion),
                    Extension = Path.GetExtension(oc.oProducto.NombreImagen)


                },
                Cantidad = oc.Cantidad,
                Total = oc.Total,
                IdTransaccion = oc.IdTransaccion
            }).ToList();

            return View(olista);
        }



    }

    
}