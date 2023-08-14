using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaIdentidad;
namespace CapaNegocio
{
    public class CN_Carrito
    {
        private CD_Carrito objCarrito = new CD_Carrito();
        public bool ExisteCarrito(int idcliente, int idproducto)
        {
            return objCarrito.ExisteCarrito(idcliente, idproducto);
        }
        public bool OperacionCarrito(int idcliente, int idproducto, bool sumar, out string mensaje)
        {
            return objCarrito.OperacionCarrito(idcliente, idproducto, sumar, out mensaje);
        }

        public int CantidadEnCarrito(int idcliente)
        {
            return objCarrito.CantidadEnCarrito(idcliente);
        }
        public List<Carrito> ListarProducto(int idcliente)
        {
            return objCarrito.ListarProducto(idcliente);
        }
        public bool EliminarCarrito(int idcliente, int idproducto)
        {
            return objCarrito.EliminarCarrito(idcliente, idproducto);

        }

    }
}
