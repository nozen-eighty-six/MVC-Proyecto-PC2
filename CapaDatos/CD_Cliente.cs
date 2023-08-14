using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaIdentidad;
using System.Data.Sql;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class CD_Cliente
    {
        //Añadimos la referencia 
        public List<Cliente> Listar()
        {
            List<Cliente> lista = new List<Cliente>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))//Clase.atributo
                {
                    string query = "select IdCliente, Nombres, Apellidos, Correo, Clave, Restablecer  from Cliente order by IdCliente;";

                    //Indicamos que se trata de un comando
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    //De qué tipo es el comando
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();

                    //Poder leer todos los datos del query
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Cada vez que lea se guarde en la Lista de la clase Cliente
                        while (dr.Read())
                        {
                            lista.Add(
                                new Cliente
                                {
                                    IdCliente = Convert.ToInt32(dr["IdCliente"]),
                                    Nombres = dr["Nombres"].ToString(),
                                    Apellidos = dr["Apellidos"].ToString(),
                                    Correo = dr["Correo"].ToString(),
                                    Clave = dr["Clave"].ToString(),
                                    Restablecer = Convert.ToBoolean(dr["Restablecer"])
                                }
                                );
                        }
                    }
                }
            }

            catch
            {
                lista = new List<Cliente>();
            }

            return lista;
        }
        public int registarCliente(Cliente obj, out string mensaje)
        {
            int idAutoGenerado = 0;
            mensaje = String.Empty;//Cadena vacía
            try
            {

                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegisrarClientes", conexion);
                    cmd.Parameters.AddWithValue("Nombres", obj.Nombres);
                    cmd.Parameters.AddWithValue("Apellidos", obj.Apellidos);
                    cmd.Parameters.AddWithValue("Correo", obj.Correo);
                    cmd.Parameters.AddWithValue("Clave", obj.Clave);
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
        public bool CambiarClave(int idCliente, string nuevaclave, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;
            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("update top(1)  Cliente set Clave = @nuevaClave, Restablecer = 0 where IdCliente = @id", conexion);
                    cmd.Parameters.AddWithValue("@id", idCliente);
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
        public bool RestablecerClave(int idCliente, string clave, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;
            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("update top(1)  Cliente set Clave = @Clave, Restablecer = 1 where IdCliente = @id", conexion);
                    cmd.Parameters.AddWithValue("@id", idCliente);
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
