using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaIdentidad;
using System.Data.SqlClient;
using System.Data;

namespace CapaDatos
{
     public class CD_Marca
    {
        public List<Marca> listarMarca()
        {
            List<Marca> listar = new List<Marca>();

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    string query = "select IdMarca, Descripcion, Activo  from MARCA order by IdMarca;";
                    //Asignamos que se trata de un comando
                    SqlCommand cmd = new SqlCommand(query, conexion);
                    //De qué tipo
                    cmd.CommandType = CommandType.Text;
                    conexion.Open();
                    //Leemos los registros
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Iteramos los datos
                        while (dr.Read())
                        {
                            listar.Add(new Marca
                            {
                                IdMarca = Convert.ToInt32(dr["IdMarca"]),
                                Descripcion = dr["Descripcion"].ToString(),
                                Activo = Convert.ToBoolean(dr["Activo"])
                            });
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                listar = new List<Marca>();
            }

            return listar;
        }

        public int registarMarca(Marca obj, out string mensaje)
        {
            int idGenerado = 0;
            try
            {

                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarMarca", conexion);
                    cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
                    cmd.Parameters.AddWithValue("Activo", obj.Activo);
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    //De qué tipo es el comando
                    cmd.CommandType = CommandType.StoredProcedure;
                    conexion.Open();
                    cmd.ExecuteNonQuery();

                    //Asignamos los datos
                    idGenerado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                    mensaje = cmd.Parameters["mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                idGenerado = 0;
                mensaje = ex.Message;
            }
            return idGenerado;
        }

        public bool editarMarca(Marca obj, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;
            try
            {

                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditarMarca", conexion);
                    cmd.Parameters.AddWithValue("IdMarca", obj.IdMarca);
                    cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
                    cmd.Parameters.AddWithValue("Activo", obj.Activo);
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

        public bool eliminarMarca(int id, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;
            try
            {

                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("delete top(1) from MARCA where IdMarca = @id", conexion);
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
        public List<Marca> listarMarcaPorCategoria(int idcategoria)
        {
            List<Marca> listar = new List<Marca>();

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("Select distinct m.IdMarca, m.Descripcion from PRODUCTO p ");
                    sb.AppendLine("inner join CATEGORIA c on c.IdCategoria = p.IdCategoria");
                    sb.AppendLine("inner join Marca m on m.IdMarca = p.IdMarca and m.Activo = 1 ");
                    sb.AppendLine("where c.IdCategoria = iif(@idcategoria = 0, c.IdCategoria, @idcategoria);");
                    //Asignamos que se trata de un comando
                    SqlCommand cmd = new SqlCommand(sb.ToString(), conexion);
                    cmd.Parameters.AddWithValue("@idcategoria",idcategoria);
                    //De qué tipo
                    cmd.CommandType = CommandType.Text;
                    conexion.Open();
                    //Leemos los registros
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Iteramos los datos
                        while (dr.Read())
                        {
                            listar.Add(new Marca
                            {
                                IdMarca = Convert.ToInt32(dr["IdMarca"]),
                                Descripcion = dr["Descripcion"].ToString()
                            });
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                listar = new List<Marca>();
            }

            return listar;
        }
    }
}
