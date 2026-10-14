
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

public class PropietarioRepositorio
{   



    public List<Propietario> ObtenerPropietarios()
    {
        var lista = new List<Propietario>();

        using (SqlConnection conexion =
               ConexionBD.ObtenerConexion())
        {
            using (SqlCommand comando = new SqlCommand(
                "dbo.sp_ListarPropietarios", conexion))
            {
                comando.CommandType =
                    CommandType.StoredProcedure;

                conexion.Open();

                using (SqlDataReader lector =
                       comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        lista.Add(new Propietario
                        {
                            Id = Convert.ToInt32(
                                lector["PRO_ID"]),

                            Identificacion =
                                lector["PRO_IDENTIFICACION"].ToString(),

                            PrimerNombre =
                                lector["PRO_PRIMER_NOMBRE"].ToString(),

                            SegundoNombre =
                                lector["PRO_SEGUNDO_NOMBRE"].ToString(),

                            PrimerApellido =
                                lector["PRO_PRIMER_APELLIDO"].ToString(),

                            SegundoApellido =
                                lector["PRO_SEGUNDO_APELLIDO"].ToString(),

                            Telefono =
                                lector["PRO_TELEFONO"].ToString(),

                            Correo =
                                lector["PRO_CORREO"].ToString()
                        });
                    }
                }
            }
        }

        return lista;
    }


    public int GuardarPropietario(Propietario propietario)
    {
        using (SqlConnection conexion =
               ConexionBD.ObtenerConexion())
        using (SqlCommand comando = new SqlCommand(
               "dbo.sp_CrearPropietario", conexion))
        {
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.Add("@Identificacion",
                SqlDbType.VarChar, 15).Value = propietario.Identificacion;

            comando.Parameters.Add("@PrimerNombre",
                SqlDbType.VarChar, 25).Value = propietario.PrimerNombre;

            comando.Parameters.Add("@SegundoNombre",
                SqlDbType.VarChar, 25).Value = propietario.SegundoNombre;

            comando.Parameters.Add("@PrimerApellido",
                SqlDbType.VarChar, 25).Value = propietario.PrimerApellido;

            comando.Parameters.Add("@SegundoApellido",
                SqlDbType.VarChar, 25).Value = propietario.SegundoApellido;

            comando.Parameters.Add("@Telefono",
                SqlDbType.VarChar, 8).Value = propietario.Telefono;

            comando.Parameters.Add("@Correo",
                SqlDbType.VarChar, 150).Value = propietario.Correo;

            comando.Parameters.Add("@Usuario",
                SqlDbType.VarChar, 15).Value = propietario.AdicionadoPor;

            conexion.Open();

            return Convert.ToInt32(comando.ExecuteScalar());
        }
    }


    public Propietario BuscarPropietarioPorId(int id)
    {
        using (SqlConnection conexion = ConexionBD.ObtenerConexion())
        using (SqlCommand comando = new SqlCommand(
            "dbo.sp_BuscarPropietarioPorId", conexion))
        {
            comando.CommandType = CommandType.StoredProcedure;
            comando.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            conexion.Open();

            using (SqlDataReader lector = comando.ExecuteReader())
            {
                if (!lector.Read())
                    return null;

                return new Propietario
                {
                    Id = Convert.ToInt32(lector["PRO_ID"]),
                    Identificacion = lector["PRO_IDENTIFICACION"].ToString(),
                    PrimerNombre = lector["PRO_PRIMER_NOMBRE"].ToString(),
                    SegundoNombre = lector["PRO_SEGUNDO_NOMBRE"].ToString(),
                    PrimerApellido = lector["PRO_PRIMER_APELLIDO"].ToString(),
                    SegundoApellido = lector["PRO_SEGUNDO_APELLIDO"].ToString(),
                    Telefono = lector["PRO_TELEFONO"].ToString(),
                    Correo = lector["PRO_CORREO"].ToString()
                };
            }
        }
    }

    public void ActualizarPropietario(Propietario propietario)
    {
        using (SqlConnection conexion = ConexionBD.ObtenerConexion())
        using (SqlCommand comando = new SqlCommand(
            "dbo.sp_ActualizarPropietario", conexion))
        {
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.Add("@Id", SqlDbType.Int)
                .Value = propietario.Id;

            comando.Parameters.Add("@Identificacion", SqlDbType.VarChar, 15)
                .Value = propietario.Identificacion;

            comando.Parameters.Add("@PrimerNombre", SqlDbType.VarChar, 25)
                .Value = propietario.PrimerNombre;

            comando.Parameters.Add("@SegundoNombre", SqlDbType.VarChar, 25)
                .Value = propietario.SegundoNombre;

            comando.Parameters.Add("@PrimerApellido", SqlDbType.VarChar, 25)
                .Value = propietario.PrimerApellido;

            comando.Parameters.Add("@SegundoApellido", SqlDbType.VarChar, 25)
                .Value = propietario.SegundoApellido;

            comando.Parameters.Add("@Telefono", SqlDbType.VarChar, 8)
                .Value = propietario.Telefono;

            comando.Parameters.Add("@Correo", SqlDbType.VarChar, 150)
                .Value = propietario.Correo;

            comando.Parameters.Add("@Usuario", SqlDbType.VarChar, 15)
                .Value = propietario.ModificadoPor;

            conexion.Open();
            comando.ExecuteNonQuery();
        }
    }

    public void EliminarPropietario(int id)
    {
        using (SqlConnection conexion = ConexionBD.ObtenerConexion())
        using (SqlCommand comando = new SqlCommand(
            "dbo.sp_EliminarPropietario", conexion))
        {
            comando.CommandType = CommandType.StoredProcedure;
            comando.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            conexion.Open();
            comando.ExecuteNonQuery();
        }
    }






}
