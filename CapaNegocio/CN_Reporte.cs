using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaIdentidad;
using CapaDatos;
//Para poder usar esas referencias
using System.Data.SqlClient;
using System.Data;
namespace CapaNegocio
{
    public class CN_Reporte
    {
        private CD_Reporte objCapadatos = new CD_Reporte();
        public DashBoard VerDashBoard()
        {
            return objCapadatos.VerDashBoard();
        }
        public List<Reporte> Ventas(string fechainicio, string fechafin, string idtransaccion)
        {
            return objCapadatos.Ventas(fechainicio, fechafin, idtransaccion);
        }
    }
}
