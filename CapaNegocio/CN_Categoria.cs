using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaIdentidad;

namespace CapaNegocio
{
    public class CN_Categoria
    {
        private CD_Categoria  objCapadatos = new CD_Categoria();
        public List<Categoria> Listar()
        {
            return objCapadatos.listarCategoria();
        }
        public int registar(Categoria obj, out string mensaje)
        {
            mensaje = string.Empty;
            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                mensaje = "La descripcion de la categoria no puede estar vacío";
            }
            
            if (string.IsNullOrEmpty(mensaje))
            {

                return objCapadatos.registarCategoria(obj, out mensaje);

            }
            else
            {
                return 0;
            }

        }
        public bool editarCategoria(Categoria obj, out string mensaje)
        {
            mensaje = string.Empty;
            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                mensaje = "La descripcion de la categoria no puede estar vacío";
            }

            if (string.IsNullOrEmpty(mensaje))
            {
                return objCapadatos.editarCategoria(obj, out mensaje);
            }
            else
            {
                return false;
            }
        }

        public bool eliminarCategoria(int id, out string mensaje)
        {

            return objCapadatos.eliminarCategoria(id, out mensaje);
        }
    }
}
