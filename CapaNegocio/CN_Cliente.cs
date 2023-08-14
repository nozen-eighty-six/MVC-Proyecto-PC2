using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaIdentidad;

namespace CapaNegocio
{
    public class CN_Cliente
    {
        private CD_Cliente objCapadatos = new CD_Cliente();
        public List<Cliente> Listar()
        {
            return objCapadatos.Listar();
        }
        public int registar(Cliente obj, out string mensaje)
        {
            mensaje = string.Empty;
            if (string.IsNullOrEmpty(obj.Nombres) || string.IsNullOrWhiteSpace(obj.Nombres))
            {
                mensaje = "El nombre del Cliente no puede estar vacío";
            }
            else if (string.IsNullOrEmpty(obj.Apellidos) || string.IsNullOrWhiteSpace(obj.Apellidos))
            {
                mensaje = "Los apellidos del Cliente no puede estar vacío";
            }
            else if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo))
            {
                mensaje = "El correo no puede estar vacío";
            }

            if (string.IsNullOrEmpty(mensaje))
            {
                //Si el envio es exitoso, que se guarde la clave autogenerada en la BD
                obj.Clave = new CN_Recursos().ConvertirSha256(obj.Clave);
                return objCapadatos.registarCliente(obj, out mensaje);
                
            }
            else
            {
                return 0;
            }

        }
        public bool CambiarClave(int idCliente, string nuevaClave, out string mensaje)
        {

            return objCapadatos.CambiarClave(idCliente, nuevaClave, out mensaje);
        }
        public bool RestablecerClave(int idCliente, string correoCliente, out string mensaje)
        {
            mensaje = string.Empty;
            string nuevaClave = CN_Recursos.GenerarClave();
            bool resultado = objCapadatos.RestablecerClave(idCliente, new CN_Recursos().ConvertirSha256(nuevaClave), out mensaje);
            if (resultado)
            {
                //Creamos el asunto del correo
                string asunto = "Contraseña Restablecida";
                string mensajeCorreo = "<h3>Su cuenta fue restablecida correctamente</h3> <br/><p>Su contraseña para acceder ahora es: " + nuevaClave + "</p>";
                //mensajeCorreo.Replace("!clave!", clave);
                bool respuesta = CN_Recursos.EnviarCorreo(correoCliente, asunto, mensajeCorreo);

                if (respuesta)
                {
                    return true;
                }
                else
                {
                    mensaje = "No se puede enviar el correo ";//Es out del parámetro
                    return false;
                }
            }
            else
            {

                mensaje = "No se pudo restablecer la contraseña";
                return false;
            }



        }
    }
}
