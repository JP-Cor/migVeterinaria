
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

public class HojaClinicaRepositorio
{
    // CONSULTAR TODAS LAS HOJAS CLINICAS
    public List<HojaClinica> ObtenerHojasClinicas()
    {
        List<HojaClinica> hojas = new List<HojaClinica>();

        using (SqlConnection conexion = ConexionBD.ObtenerConexion())
        using (SqlCommand comando = new SqlCommand(
            "dbo.sp_ListarHojasClinicas", conexion))
        {
            comando.CommandType = CommandType.StoredProcedure;

            conexion.Open();

            using (SqlDataReader lector = comando.ExecuteReader())
            {
                while (lector.Read())
                {
                    hojas.Add(new HojaClinica
                    {
                        Id = Convert.ToInt32(lector["HOJ_ID"]),

                        FechaAtencion = Convert.ToDateTime(
                            lector["HOJ_FECHA_ATENCION"]),

                        Sintomas = lector["HOJ_SINTOMAS"].ToString(),

                        Diagnostico = lector["HOJ_DIAGNOSTICO"].ToString(),

                        Tratamiento = lector["HOJ_TRATAMIENTO"].ToString(),

                        MascotaId = Convert.ToInt32(
                            lector["HOJ_MAS_ID"]),

                        NombreMascota =
                            lector["MAS_NOMBRE"].ToString(),

                        NombrePropietario =
                            lector["PRO_PRIMER_NOMBRE"].ToString(),

                        ApellidoPropietario =
                            lector["PRO_PRIMER_APELLIDO"].ToString()
                    });
                }
            }
        }

        return hojas;
    }

    // BUSCAR HOJA CLINICA POR ID
    public HojaClinica BuscarHojaClinicaPorId(int id)
    {
        using (SqlConnection conexion = ConexionBD.ObtenerConexion())
        using (SqlCommand comando = new SqlCommand(
            "dbo.sp_BuscarHojaClinicaPorId", conexion))
        {
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.Add("@Id", SqlDbType.Int)
                .Value = id;

            conexion.Open();

            using (SqlDataReader lector = comando.ExecuteReader())
            {
                if (!lector.Read())
                    return null;

                return new HojaClinica
                {
                    Id = Convert.ToInt32(lector["HOJ_ID"]),

                    FechaAtencion = Convert.ToDateTime(
                        lector["HOJ_FECHA_ATENCION"]),

                    Sintomas = lector["HOJ_SINTOMAS"].ToString(),

                    Diagnostico = lector["HOJ_DIAGNOSTICO"].ToString(),

                    Tratamiento = lector["HOJ_TRATAMIENTO"].ToString(),

                    MascotaId = Convert.ToInt32(
                        lector["HOJ_MAS_ID"])
                };
            }
        }
    }

    // REGISTRAR HOJA CLINICA
    public int GuardarHojaClinica(HojaClinica hoja)
    {
        using (SqlConnection conexion = ConexionBD.ObtenerConexion())
        using (SqlCommand comando = new SqlCommand(
            "dbo.sp_CrearHojaClinica", conexion))
        {
            comando.CommandType = CommandType.StoredProcedure;

            AgregarParametrosHoja(comando, hoja);

            conexion.Open();

            return Convert.ToInt32(comando.ExecuteScalar());
        }
    }

    // MODIFICAR HOJA CLINICA
    public void ActualizarHojaClinica(HojaClinica hoja)
    {
        using (SqlConnection conexion = ConexionBD.ObtenerConexion())
        using (SqlCommand comando = new SqlCommand(
            "dbo.sp_ActualizarHojaClinica", conexion))
        {
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.Add("@Id", SqlDbType.Int)
                .Value = hoja.Id;

            AgregarParametrosHoja(comando, hoja, true);

            conexion.Open();

            comando.ExecuteNonQuery();
        }
    }

    // ELIMINAR HOJA CLINICA
    public void EliminarHojaClinica(int id)
    {
        using (SqlConnection conexion = ConexionBD.ObtenerConexion())
        using (SqlCommand comando = new SqlCommand(
            "dbo.sp_EliminarHojaClinica", conexion))
        {
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.Add("@Id", SqlDbType.Int)
                .Value = id;

            conexion.Open();

            comando.ExecuteNonQuery();
        }
    }

    // PARAMETROS PARA GUARDAR Y MODIFICAR
    private void AgregarParametrosHoja(
        SqlCommand comando,
        HojaClinica hoja,
        bool esModificacion = false)
    {
        comando.Parameters.Add("@FechaAtencion", SqlDbType.Date)
            .Value = hoja.FechaAtencion;

        comando.Parameters.Add("@Sintomas", SqlDbType.VarChar, 150)
            .Value = hoja.Sintomas;

        comando.Parameters.Add("@Diagnostico", SqlDbType.VarChar, 150)
            .Value = hoja.Diagnostico;

        comando.Parameters.Add("@Tratamiento", SqlDbType.VarChar, 150)
            .Value = hoja.Tratamiento;

        comando.Parameters.Add("@MascotaId", SqlDbType.Int)
            .Value = hoja.MascotaId;

        comando.Parameters.Add("@Usuario", SqlDbType.VarChar, 15)
            .Value = esModificacion
                ? hoja.ModificadoPor
                : hoja.AdicionadoPor;
    }
}
