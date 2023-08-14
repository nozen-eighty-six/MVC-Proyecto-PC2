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
    public class CD_Ubicacion
    {
        public List<Departamento> ObtenerDepartamento()
        {
            List<Departamento> lista = new List<Departamento>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))//Clase.atributo
                {
                    string query = "Select * from DEPARTAMENTO;";

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
                                new Departamento
                                {
                                    IdDepartamento = (dr["IdDepartamento"]).ToString(),
                                    Descripcion = dr["Descripcion"].ToString(),
                                   
                                }
                                );
                        }
                    }
                }
            }

            catch
            {
                lista = new List<Departamento>();
            }

            return lista;
        }
        public List<Provincia> ObtenerProvincia(string iddepartamento)
        {
            List<Provincia> lista = new List<Provincia>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))//Clase.atributo
                {
                    string query = "Select * from PROVINCIA where IdDepartamento = @iddepartamento;";

                    //Indicamos que se trata de un comando
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.Parameters.AddWithValue("@iddepartamento", iddepartamento);
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
                                new Provincia
                                {
                                    IdProvincia = (dr["IdProvincia"]).ToString(),
                                    Descripcion = dr["Descripcion"].ToString(),

                                }
                                );
                        }
                    }
                }
            }

            catch
            {
                lista = new List<Provincia>();
            }

            return lista;
        }

        public List<Distrito> ObtenerDistrito(string iddepartamento, string idprovincia)
        {
            List<Distrito> lista = new List<Distrito>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))//Clase.atributo
                {
                    string query = "Select * from DISTRITO where IdProvincia = @idprovincia and IdDepartamento = @iddepartamento;";

                    //Indicamos que se trata de un comando
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.Parameters.AddWithValue("@iddepartamento", iddepartamento);
                    cmd.Parameters.AddWithValue("@idprovincia", idprovincia);
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
                                new Distrito
                                {
                                    IdDistrito = (dr["IdDistrito"]).ToString(),
                                    Descripcion = dr["Descripcion"].ToString(),

                                }
                                );
                        }
                    }
                }
            }

            catch
            {
                lista = new List<Distrito>();
            }

            return lista;
        }
    }
}
