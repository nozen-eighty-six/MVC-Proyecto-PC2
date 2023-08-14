using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using CapaIdentidad;
using System.Globalization;

namespace CapaDatos
{
    public class CD_Carrito
    {
        public bool ExisteCarrito(int idcliente, int idproducto)
        {
            bool resultado = true;
            
            try
            {

                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_ExisteCarrito", conexion);
                    cmd.Parameters.AddWithValue("IdCliente", idcliente);
                    cmd.Parameters.AddWithValue("Idproducto",idproducto);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    //Que tipo de comando es
                    cmd.CommandType = CommandType.StoredProcedure;
                    conexion.Open();
                    //EJecutamos
                    cmd.ExecuteNonQuery();

                    //Obetenr los valores de los parámetros OUT
                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                   

                }
            }
            catch (Exception ex)
            {
                resultado =false;
               

            }
            return resultado;
        }

        public bool OperacionCarrito(int idcliente, int idproducto, bool sumar, out string mensaje)
        {
            bool resultado=true;
            mensaje = String.Empty;//Cadena vacía
            try
            {

                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_OperacionCarrito", conexion);
                    cmd.Parameters.AddWithValue("IdCliente", idcliente);
                    cmd.Parameters.AddWithValue("IdProducto", idproducto);
                    cmd.Parameters.AddWithValue("Sumar", sumar);
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    //Que tipo de comando es
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

        public int CantidadEnCarrito(int idcliente)
        {
            int resultado =0;
            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("Select count(*) from Carrito where IdCliente = @idcliente", conexion);
                    cmd.Parameters.AddWithValue("@idcliente", idcliente);
                    cmd.CommandType = CommandType.Text;
                    conexion.Open();
                    //Si el número de filas afectadas es mayor a "0", porque lo estamos haciendo mendiante query
                    resultado =Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                resultado = 0;

            }
            return resultado;
        }
        public List<Carrito> ListarProducto(int idcliente)
        {
            List<Carrito> lista = new List<Carrito>();

            try
            {

                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    string query = "select * from fn_obtenerCarritoCliente(@idcliente);";

                    //Asignamos que se trata de un comando
                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@idcliente", idcliente);
                    //De qué tipo
                    cmd.CommandType = CommandType.Text;
                    conexion.Open();
                    //Leemos los registros
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Iteramos los datos
                        while (dr.Read())
                        {
                            lista.Add(new Carrito()
                            {
                                oProducto = new Producto() {

                                    IdProducto = Convert.ToInt32(dr["IdProducto"]),
                                    Nombre = dr["Nombre"].ToString(),
                                    Precio = Convert.ToDecimal(dr["Precio"], new CultureInfo("es-PE")),//Según los puntos de decimales en Perú
                                    RutaImagen = dr["RutaImagen"].ToString(),
                                    NombreImagen = dr["NombreImagen"].ToString(),
                                    oMarca = new Marca() {Descripcion = dr["DesMarca"].ToString() }
                                },
                                Cantidad = Convert.ToInt32(dr["Cantidad"])
                                
                            });
                        }
                    };
                }
            }

            catch
            {
                lista = new List<Carrito>();
            }

            return lista;
        }

        public bool EliminarCarrito(int idcliente, int idproducto)
        {
            bool resultado = true;

            try
            {

                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EliminarCarrito", conexion);
                    cmd.Parameters.AddWithValue("IdCliente", idcliente);
                    cmd.Parameters.AddWithValue("Idproducto", idproducto);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    //Que tipo de comando es
                    cmd.CommandType = CommandType.StoredProcedure;
                    conexion.Open();
                    //EJecutamos
                    cmd.ExecuteNonQuery();

                    //Obetenr los valores de los parámetros OUT
                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);


                }
            }
            catch (Exception ex)
            {
                resultado = false;


            }
            return resultado;
        }
    }
}
