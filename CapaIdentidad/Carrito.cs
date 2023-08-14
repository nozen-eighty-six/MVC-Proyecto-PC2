using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaIdentidad
{
    public class Carrito
    {
        public int IdCarrito { get; set; }
        public Producto oProducto { get; set; }
        public Cliente ocliente { get; set; }
        public int Cantidad { get; set; }
    }
}
