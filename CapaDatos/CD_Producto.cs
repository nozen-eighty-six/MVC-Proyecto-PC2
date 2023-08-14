using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaIdentidad;
//Para poder usar esas referencias
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using System.IO;

namespace CapaDatos
{
    public class CD_Producto
    {
        //Añadimos la referencia 
        public List<Producto> Listar()
        {
            List<Producto> lista = new List<Producto>();
            

            try
            {

                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    StringBuilder sb = new StringBuilder();//Para crear saltos de línea
                    sb.AppendLine("select p.IdProducto, p.Nombre, p.Descripcion, ");
                    sb.AppendLine("m.IdMarca, m.Descripcion[DesMarca],");
                    sb.AppendLine("c.IdCategoria, c.Descripcion[DesCategoria],");
                    sb.AppendLine("p.Precio, p.Stock, p.RutaImagen, p.NombreImagen, p.Activo");
                    sb.AppendLine("from Producto p");
                    sb.AppendLine("Inner join Marca m on m.IdMarca = p.IdMarca");
                    sb.AppendLine("Inner join Categoria c on c.IdCategoria = p.IdCategoria");
                    //Asignamos que se trata de un comando
                    SqlCommand cmd = new SqlCommand(sb.ToString(), conexion);
                    //De qué tipo
                    cmd.CommandType = CommandType.Text;
                    conexion.Open();
                    //Leemos los registros
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Iteramos los datos
                        while (dr.Read())
                        {
                            lista.Add(new Producto
                            {
                                IdProducto = Convert.ToInt32(dr["IdProducto"]),
                                Nombre = dr["Nombre"].ToString(),
                                Descripcion = dr["Descripcion"].ToString(),
                                oMarca = new Marca { IdMarca = Convert.ToInt32(dr["IdMarca"]), Descripcion = dr["DesMarca"].ToString() },
                                oCategoria = new Categoria { IdCategoria = Convert.ToInt32(dr["IdCategoria"]), Descripcion = dr["DesCategoria"].ToString() },
                                Precio =Convert.ToDecimal(dr["Precio"],new CultureInfo("es-PE")),//Según los puntos de decimales en Perú
                                Stock = Convert.ToInt32(dr["Stock"]),
                                RutaImagen = dr["RutaImagen"].ToString(),
                                NombreImagen = dr["NombreImagen"].ToString(),
                                Activo = Convert.ToBoolean(dr["Activo"])
                                
                            });
                        }
                    };
                }
            }

            catch
            {
                lista = new List<Producto>();
            }

            return lista;
        }
        public static string convertirBase64(string ruta, out bool conversion)
        {
            string textoBase64 = string.Empty;
            conversion = true;
            try
            {
                byte[] bytes = File.ReadAllBytes(ruta);
                textoBase64 = Convert.ToBase64String(bytes);
            }
            catch (Exception ex)
            {
                conversion = false;
            }
            return textoBase64;
        }
        public List<Producto> ListarUnProductoXCategoria()
        {
            List<Producto> listaProducto = new List<Producto>();

            string mensaje = string.Empty;
            bool conversion;
            try
            {

                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    StringBuilder sb = new StringBuilder();//Para crear saltos de línea
                    sb.AppendLine("select p.IdProducto, p.Nombre, p.Descripcion, ");
                    sb.AppendLine("m.IdMarca, m.Descripcion[DesMarca],");
                    sb.AppendLine("c.IdCategoria, c.Descripcion[DesCategoria],");
                    sb.AppendLine("p.Precio, p.Stock, p.RutaImagen, p.NombreImagen, p.Activo");
                    sb.AppendLine("from Producto p");
                    sb.AppendLine("Inner join Marca m on m.IdMarca = p.IdMarca");
                    sb.AppendLine("Inner join Categoria c on c.IdCategoria = p.IdCategoria");
                    sb.AppendLine("Where p.IdProducto IN(Select Top(1) IdProducto from Producto p2 where p2.IdCategoria = c.IdCategoria)");
                    //Asignamos que se trata de un comando
                    SqlCommand cmd = new SqlCommand(sb.ToString(), conexion);
                    //De qué tipo
                    cmd.CommandType = CommandType.Text;
                    conexion.Open();
                    //Leemos los registros
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Iteramos los datos
                        while (dr.Read())
                        {
                            listaProducto.Add(new Producto
                            {
                                IdProducto = Convert.ToInt32(dr["IdProducto"]),
                                Nombre = dr["Nombre"].ToString(),
                                Descripcion = dr["Descripcion"].ToString(),
                                oMarca = new Marca { IdMarca = Convert.ToInt32(dr["IdMarca"]), Descripcion = dr["DesMarca"].ToString() },
                                oCategoria = new Categoria { IdCategoria = Convert.ToInt32(dr["IdCategoria"]), Descripcion = dr["DesCategoria"].ToString() },
                                Precio = Convert.ToDecimal(dr["Precio"], new CultureInfo("es-PE")),//Según los puntos de decimales en Perú
                                Stock = Convert.ToInt32(dr["Stock"]),
                                RutaImagen = dr["RutaImagen"].ToString(),
                                NombreImagen = dr["NombreImagen"].ToString(),
                                Activo = Convert.ToBoolean(dr["Activo"]),
                                Base64 = convertirBase64(Path.Combine(dr["RutaImagen"].ToString(), dr["NombreImagen"].ToString()), out conversion),
                                Extension = Path.GetExtension(dr["NombreImagen"].ToString())
                            });
                        }
                    };
                }
            }

            catch(Exception ex)
            {
                mensaje = ex.Message;
                listaProducto = new List<Producto>();
            }

            return listaProducto;
        }

        public int registarProducto(Producto obj, out string mensaje)
        {
            int idAutoGenerado = 0;
            mensaje = String.Empty;//Cadena vacía
            try
            {

                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarProducto", conexion);
                    cmd.Parameters.AddWithValue("Nombre", obj.Nombre);
                    cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
                    cmd.Parameters.AddWithValue("IdMarca", obj.oMarca.IdMarca);
                    cmd.Parameters.AddWithValue("IdCategoria", obj.oCategoria.IdCategoria);
                    cmd.Parameters.AddWithValue("Precio", obj.Precio);
                    cmd.Parameters.AddWithValue("Stock", obj.Stock);
                    cmd.Parameters.AddWithValue("Activo", obj.Activo);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    
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

        public bool editarProducto(Producto obj, out string mensaje)
        {
            bool resultado = false;
            mensaje = String.Empty;//Cadena vacía
            try
            {

                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditarProducto", conexion);
                    cmd.Parameters.AddWithValue("IdProducto", obj.IdProducto);
                    cmd.Parameters.AddWithValue("Nombre", obj.Nombre);
                    cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
                    cmd.Parameters.AddWithValue("IdMarca", obj.oMarca.IdMarca);
                    cmd.Parameters.AddWithValue("IdCategoria", obj.oCategoria.IdCategoria);
                    cmd.Parameters.AddWithValue("Precio", obj.Precio);
                    cmd.Parameters.AddWithValue("Stock", obj.Stock);
                    cmd.Parameters.AddWithValue("Activo", obj.Activo);
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

        public bool GuardarDatosImg(Producto obj, out string mensaje)
        {
            //Guarde el resultado final
            bool resultado = false;
            mensaje = string.Empty;
            try
            {
                string query = "Update Producto set  RutaImagen = @rutaimagen, NombreImagen = @nombreimagen where IdProducto = @idproducto";
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@rutaimagen", obj.RutaImagen);
                    cmd.Parameters.AddWithValue("@nombreimagen", obj.NombreImagen);
                    cmd.Parameters.AddWithValue("@idproducto", obj.IdProducto);
                    cmd.Parameters.AddWithValue("Activo", obj.Activo);
                    
                    //Que tipo de comando es
                    cmd.CommandType = CommandType.Text;
                    conexion.Open();
                    //EJecutamos, pero como no se trata de un procedimiento, tenemos que verificar que si
                    //se afectaron filas, por eso condicionamos
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        resultado = true;
                    }
                    else
                    {
                        mensaje = "No se puede actualizar la imagen";
                    }

                  
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;

            }
            return resultado;
        }

        public bool eliminarProducto(int id, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;
            try
            {

                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EliminarProducto", conexion);
                    cmd.Parameters.AddWithValue("IdProducto", id);
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    //De qué tipo es el comando
                    cmd.CommandType = CommandType.StoredProcedure;
                    conexion.Open();
                    cmd.ExecuteNonQuery();

                    //Asignamos los datos
                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    mensaje = cmd.Parameters["mensaje"].Value.ToString();
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
