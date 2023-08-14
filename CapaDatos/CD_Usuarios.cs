using CapaIdentidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Para poder usar esas referencias
using System.Data.SqlClient;
using System.Data;

namespace CapaDatos
{
    public class CD_Usuarios
    {
        //Añadimos la referencia 
        public List<Usuario> Listar()
        {
            List<Usuario> lista = new List<Usuario>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))//Clase.atributo
                {
                    string query = "select IdUsuario, Nombres, Apellidos, Correo, Clave, Restablecer, Activo  from USUARIO order by IdUsuario;";

                    //Indicamos que se trata de un comando
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    //De qué tipo es el comando
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();

                    //Poder leer todos los datos del query
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Cada vez que lea se guarde en la Lista de la clase Usuario
                        while (dr.Read())
                        {
                            lista.Add(
                                new Usuario
                                {
                                    IdUsuario = Convert.ToInt32(dr["IdUsuario"]), Nombres= dr["Nombres"].ToString(), Apellidos = dr["Apellidos"].ToString(), Correo = dr["Correo"].ToString(), Clave= dr["Clave"].ToString(), Restablecer = Convert.ToBoolean(dr["Restablecer"]), Activo = Convert.ToBoolean(dr["Activo"])
                                }
                                );
                        }
                    }
                }
            }

            catch
            {
                lista = new List<Usuario>();
            }

            return lista;
        }

        public int registarUsuario(Usuario obj, out string mensaje)
        {
            int idAutoGenerado = 0;
            mensaje = String.Empty;//Cadena vacía
            try
            {
                
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarUsuario", conexion);
                    cmd.Parameters.AddWithValue("Nombres", obj.Nombres);
                    cmd.Parameters.AddWithValue("Apellidos", obj.Apellidos);
                    cmd.Parameters.AddWithValue("Correo", obj.Correo);
                    cmd.Parameters.AddWithValue("Clave", obj.Clave);
                    cmd.Parameters.AddWithValue("Activo", obj.Activo);
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    //Que tipo de comando es
                    cmd.CommandType = CommandType.StoredProcedure;
                    conexion.Open();
                    //EJecutamos
                    cmd.ExecuteNonQuery();

                    //Obetenr los valores de los parámetros OUT
                    idAutoGenerado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                    mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                idAutoGenerado = 0;
                mensaje = ex.Message;

            }
            return idAutoGenerado;
        }

        public bool editarUsuario(Usuario obj, out string mensaje)
        {
            bool resultado = false;
            mensaje = String.Empty;//Cadena vacía
            try
            {

                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditarUsuario", conexion);
                    cmd.Parameters.AddWithValue("IdUsuario", obj.IdUsuario);
                    cmd.Parameters.AddWithValue("Nombres", obj.Nombres);
                    cmd.Parameters.AddWithValue("Apellidos", obj.Apellidos);
                    cmd.Parameters.AddWithValue("Correo", obj.Correo);
                    cmd.Parameters.AddWithValue("Activo", obj.Activo);
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;
                    conexion.Open();
                    //EJecutamos
                    cmd.ExecuteNonQuery();

                    //Obetenr los valores de los parámetros OUT
                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;

            }
            return resultado;
        }

        public bool eliminarUsuario(int id, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;
            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("delete top(1) from Usuario where IdUsuario = @id", conexion);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.CommandType = CommandType.Text;
                    conexion.Open();
                    //Si el número de filas afectadas es mayor a "0", porque lo estamos haciendo mendiante query
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;
            }
            return resultado;
        }

        public bool CambiarClave(int idUsuario,string nuevaclave, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;
            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("update top(1)  Usuario set Clave = @nuevaClave, Restablecer = 0 where IdUsuario = @id", conexion);
                    cmd.Parameters.AddWithValue("@id", idUsuario);
                    cmd.Parameters.AddWithValue("@nuevaClave", nuevaclave);
                    cmd.CommandType = CommandType.Text;
                    conexion.Open();
                    //Si el número de filas afectadas es mayor a "0", porque lo estamos haciendo mendiante query
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;
            }
            return resultado;
        }

        public bool RestablecerClave(int idUsuario, string clave, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;
            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("update top(1)  Usuario set Clave = @Clave, Restablecer = 1 where IdUsuario = @id", conexion);
                    cmd.Parameters.AddWithValue("@id", idUsuario);
                    cmd.Parameters.AddWithValue("@Clave", clave);
                    cmd.CommandType = CommandType.Text;
                    conexion.Open();
                    //Si el número de filas afectadas es mayor a "0", porque lo estamos haciendo mendiante query
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;
            }
            return resultado;
        }
    }
}
