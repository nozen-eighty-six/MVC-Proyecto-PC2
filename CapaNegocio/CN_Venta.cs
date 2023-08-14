using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaIdentidad;
//Para poder usar esas referencias
using System.Data.SqlClient;
using System.Data;

namespace CapaNegocio
{
    public class CN_Venta
    {
        private CD_Venta objVenta = new CD_Venta();
        public bool Registrar(Venta obj, DataTable detalleventa, out string mensaje) {

            return objVenta.Registrar(obj, detalleventa,out mensaje);
        }

        public List<DetalleVenta> ListarCompras(int idcliente)
        {

            return objVenta.ListarCompras(idcliente);
        }
    }
}
