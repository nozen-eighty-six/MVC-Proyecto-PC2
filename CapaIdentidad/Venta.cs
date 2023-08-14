using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaIdentidad
{
//    IdVenta int identity,
//IdCliente int references Cliente(IdCliente),
//TotalProducto int,--Total de productos seleccionados
//MontoTotal decimal (10,2),
//Contacto varchar(50),
//IdDistrito varchar(10),
//Telefono varchar(50),
//Direccion varchar(100),
//IdTransaccion varchar(50),--Integración con PayPal
    public class Venta
    {
        public int IdVenta { get; set; }
        public int IdCliente{ get; set; }//Sin referencia a la tabla Cliente
        public int TotalProducto { get; set; }
        public decimal MontoTotal { get; set; }
        public string Contacto { get; set; }
        public string IdDistrito { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string IdTransaccion { get; set; }
    }
}
