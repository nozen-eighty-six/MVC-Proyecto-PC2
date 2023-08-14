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

namespace CapaDatos
{
    public class CD_Reporte
    {

        //Añadimos la referencia 
        public List<Reporte> Ventas(string fechainicio, string fechafin, string idtransaccion)
        {
            List<Reporte> lista = new List<Reporte>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))//Clase.atributo
                {


                    //Indicamos que se trata de un comando
                    SqlCommand cmd = new SqlCommand("sp_ReporteVentas", oconexion);
                    cmd.Parameters.AddWithValue("fechainicio", fechainicio);
                    cmd.Parameters.AddWithValue("fechafin", fechafin);
                    cmd.Parameters.AddWithValue("idtransaccion", idtransaccion);
                    //De qué tipo es el comando
                    cmd.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();

                    //Poder leer todos los datos del query
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Cada vez que lea se guarde en la Lista de la clase Usuario
                        while (dr.Read())
                        {
                            lista.Add(
                                new Reporte
                                {
                                    FechaVenta = dr["FechaVenta"].ToString(),
                                    Cliente = dr["Cliente"].ToString(),
                                    Producto = dr["Producto"].ToString(),
                                    Precio = Convert.ToDecimal(dr["Precio"], new CultureInfo("es-PE")),
                                    Cantidad = Convert.ToInt32(dr["Cantidad"]),
                                    Total = Convert.ToDecimal(dr["Total"], new CultureInfo("es-PE")),
                                    IdTransaccion = dr["IdTransaccion"].ToString()
                                });

                        }


                    }
                }
            }
            catch (Exception ex)
            {
                lista = new List<Reporte>();
                string mensaje = ex.Message;
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
            public DashBoard VerDashBoard()
            {
                DashBoard objeto = new DashBoard();

                try
                {
                    using (SqlConnection oconexion = new SqlConnection(Conexion.cn))//Clase.atributo
                    {


                        //Indicamos que se trata de un comando
                        SqlCommand cmd = new SqlCommand("sp_ReporteDashboard", oconexion);
                        //De qué tipo es el comando
                        cmd.CommandType = CommandType.StoredProcedure;
                        oconexion.Open();

                        //Poder leer todos los datos del query
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            //Cada vez que lea se guarde en la Lista de la clase Usuario
                            while (dr.Read())
                            {
                                objeto = new DashBoard()
                                {
                                    TotalCliente = Convert.ToInt32(dr["TotalCliente"]),
                                    TotalVenta = Convert.ToInt32(dr["TotalVenta"]),
                                    TotalProducto = Convert.ToInt32(dr["TotalProducto"])
                                };
                            }
                        }
                    }
                }

                catch
                {
                    objeto = new DashBoard();
                }

                return objeto;
            }
        }
    }

