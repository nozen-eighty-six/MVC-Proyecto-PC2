using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaIdentidad;
using CapaDatos;

namespace CapaNegocio
{
    public class CN_Producto
    {
        private CD_Producto objCapadatos = new CD_Producto();
        public List<Producto> Listar()
        {
            return objCapadatos.Listar();
        }

        public List<Producto> ListarProductoXCategoria()
        {
            return objCapadatos.ListarUnProductoXCategoria();
        }
        public int registar(Producto obj, out string mensaje)
        {
            mensaje = string.Empty;
            if (string.IsNullOrEmpty(obj.Nombre) || string.IsNullOrWhiteSpace(obj.Nombre))
            {
                mensaje = "El nombre del producto no puede estar vacío";
            }
            else if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                mensaje = "La descripcion del producto no puede estar vacío";
            }
            else if (obj.oMarca.IdMarca == 0)
            {
                mensaje = "Debe seleccionar una marca";
            }
            else if (obj.oCategoria.IdCategoria == 0)
            {
                mensaje = "Debe seleccionar una categoria";
            }
            else if (obj.Precio == 0)
            {

                mensaje = "El precio no puede estar vacío";
            }
            else if (obj.Stock == 0)
            {

                mensaje = "El stock no puede estar vacío";
            }

            if (string.IsNullOrEmpty(mensaje))
            {

                return objCapadatos.registarProducto(obj, out mensaje);

            }
            else
            {
                return 0;
            }

        }
        public bool editarProducto(Producto obj, out string mensaje)
        {
            mensaje = string.Empty;
            if (string.IsNullOrEmpty(obj.Nombre) || string.IsNullOrWhiteSpace(obj.Nombre))
            {
                mensaje = "El nombre del producto no puede estar vacío";
            }
            else if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                mensaje = "La descripcion del producto no puede estar vacío";
            }
            else if (obj.oMarca.IdMarca == 0)
            {
                mensaje = "Debe seleccionar una marca";
            }
            else if (obj.oCategoria.IdCategoria == 0)
            {
                mensaje = "Debe seleccionar una categoria";
            }
            else if (obj.Precio == 0)
            {

                mensaje = "El precio no puede estar vacío";
            }
            else if (obj.Stock == 0)
            {

                mensaje = "El stock no puede estar vacío";
            }

            if (string.IsNullOrEmpty(mensaje))
            {
                return objCapadatos.editarProducto(obj, out mensaje);
            }
            else
            {
                return false;
            }
        }
        public bool GuardarDatosImg(Producto obj, out string mensaje)
        {
            return objCapadatos.GuardarDatosImg(obj, out mensaje);
        }

        public bool eliminarProducto(int id, out string mensaje)
        {

            return objCapadatos.eliminarProducto(id, out mensaje);
        }
    }
}
