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
    public class CN_Usuarios
    {
        private CD_Usuarios objCapadatos = new CD_Usuarios();
        public List<Usuario> Listar()
        {
            return objCapadatos.Listar();
        }
        public int registar(Usuario obj, out string mensaje)
        {
            mensaje = string.Empty;
            if (string.IsNullOrEmpty(obj.Nombres) || string.IsNullOrWhiteSpace(obj.Nombres))
            {
                mensaje = "El nombre del usuario no puede estar vacío";
            }
            else if (string.IsNullOrEmpty(obj.Apellidos) || string.IsNullOrWhiteSpace(obj.Apellidos))
            {
                mensaje = "Los apellidos del usuario no puede estar vacío";
            }
            else if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo))
            {
                mensaje = "El correo no puede estar vacío";
            }

            if (string.IsNullOrEmpty(mensaje))
            {
                string clave = CN_Recursos.GenerarClave();
                //Creamos el asunto del correo
                string asunto = "Creacion de cuenta";
                string mensajeCorreo = "<h3>Su cuenta fue creada correctamente</h3> <br/><p>Su contraseña para acceder es: "+clave+"</p>";
                //mensajeCorreo.Replace("!clave!", clave);

                //Ejecución del correo
                bool respuesta = CN_Recursos.EnviarCorreo(obj.Correo, asunto, mensajeCorreo);
                if (respuesta)
                {
                    //Si el envio es exitoso, que se guarde la clave autogenerada en la BD
                    obj.Clave = new CN_Recursos().ConvertirSha256(clave);
                    return objCapadatos.registarUsuario(obj, out mensaje);
                }
                else
                {
                    mensaje = "No se puede enviar el correo";//Es out del parámetro
                    return 0;
                }
                
                
            }
            else
            {
                return 0;
            }

        }
        public bool editarUsuario(Usuario obj, out string mensaje)
        {
            mensaje = string.Empty;
            if (string.IsNullOrEmpty(obj.Nombres) || string.IsNullOrWhiteSpace(obj.Nombres))
            {
                mensaje = "El nombre del usuario no puede estar vacío";
            }
            else if (string.IsNullOrEmpty(obj.Apellidos) || string.IsNullOrWhiteSpace(obj.Apellidos))
            {
                mensaje = "Los apellidos del usuario no puede estar vacío";
            }
            else if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo))
            {
                mensaje = "El correo no puede estar vacío";
            }

            if (string.IsNullOrEmpty(mensaje))
            {
                string clave = "test123";
                obj.Clave = new CN_Recursos().ConvertirSha256(clave);
                return objCapadatos.editarUsuario(obj, out mensaje);
            }
            else
            {
                return false;
            }
        }

        public bool eliminarUsuario(int id, out string mensaje)
        {

            return objCapadatos.eliminarUsuario(id, out mensaje) ;
        }

        public bool CambiarClave(int idUsuario,string nuevaClave, out string mensaje)
        {

            return objCapadatos.CambiarClave(idUsuario, nuevaClave,out mensaje);
        }
        public bool RestablecerClave(int idUsuario,string correoUsuario, out string mensaje)
        {
            mensaje = string.Empty;
            string nuevaClave = CN_Recursos.GenerarClave();
            bool resultado = objCapadatos.RestablecerClave(idUsuario, new CN_Recursos().ConvertirSha256(nuevaClave), out mensaje);
            if (resultado)
            {
                //Creamos el asunto del correo
                string asunto = "Contraseña Restablecida";
                string mensajeCorreo = "<h3>Su cuenta fue restablecida correctamente</h3> <br/><p>Su contraseña para acceder ahora es: " + nuevaClave + "</p>";
                //mensajeCorreo.Replace("!clave!", clave);
                bool respuesta = CN_Recursos.EnviarCorreo(correoUsuario, asunto, mensajeCorreo);

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
            else {

                mensaje = "No se pudo restablecer la contraseña";
                return false;
            }

           

        }
    }

}
