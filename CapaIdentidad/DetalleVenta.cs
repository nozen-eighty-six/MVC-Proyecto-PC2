using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaIdentidad
{
//    IdDetalleVenta int identity,
//IdVenta int references Venta(IdVenta),
//IdProducto int references Producto(IdProducto),
//Cantidad int,--Cuantas unidades está llevando del producto
//Total decimal (10,2),--Cantidad x precioProducto
    public class DetalleVenta
    {
        public int IdDetalleVenta { get; set; }
        public int IdVenta { get; set; }
        public Producto oProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal Total { get; set; }
        public string IdTransaccion { get; set; }//Código o token de las T Paypal
        public List<DetalleVenta> oDetalleVenta { get; set; }
    }
}
