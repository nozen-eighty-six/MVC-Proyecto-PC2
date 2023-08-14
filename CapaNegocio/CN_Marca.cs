using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaIdentidad;
using CapaDatos;
namespace CapaNegocio
{
    public class CN_Marca
    {
        private CD_Marca objCapadatos = new CD_Marca();
        public List<Marca> Listar()
        {
            return objCapadatos.listarMarca();
        }
        public int registar(Marca obj, out string mensaje)
        {
            mensaje = string.Empty;
            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                mensaje = "La descripcion de la categoria no puede estar vacío";
            }

            if (string.IsNullOrEmpty(mensaje))
            {

                return objCapadatos.registarMarca(obj, out mensaje);

            }
            else
            {
                return 0;
            }

        }
        public bool editarMarca(Marca obj, out string mensaje)
        {
            mensaje = string.Empty;
            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                mensaje = "La descripcion de la categoria no puede estar vacío";
            }

            if (string.IsNullOrEmpty(mensaje))
            {
                return objCapadatos.editarMarca(obj, out mensaje);
            }
            else
            {
                return false;
            }
        }

        public bool eliminarMarca(int id, out string mensaje)
        {

            return objCapadatos.eliminarMarca(id, out mensaje);
        }
        public List<Marca> listarMarcaPorCategoria(int idcategoria) {

            return new CD_Marca().listarMarcaPorCategoria(idcategoria);
        }
    }
}
