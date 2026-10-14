
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

public class MascotaRepositorio
{
    // CONSULTAR TODAS LAS MASCOTAS
    public List<Mascota> ObtenerMascotas()
    {
        List<Mascota> mascotas = new List<Mascota>();

        using (SqlConnection conexion = ConexionBD.ObtenerConexion())
        using (SqlCommand comando = new SqlCommand(
            "dbo.sp_ListarMascotas", conexion))
        {
            comando.CommandType = CommandType.StoredProcedure;

            conexion.Open();

            using (SqlDataReader lector = comando.ExecuteReader())
            {
                while (lector.Read())
                {
                    mascotas.Add(new Mascota
                    {
                        Id = Convert.ToInt32(lector["MAS_ID"]),
                        Nombre = lector["MAS_NOMBRE"].ToString(),
                        FechaNacimiento = Convert.ToDateTime(
                            lector["MAS_FECHA_NACIMIENTO"]),
                        Sexo = lector["MAS_SEXO"].ToString(),
                        Peso = Convert.ToDecimal(lector["MAS_PESO"]),
                        Alergias = lector["MAS_ALERGIAS"] == DBNull.Value
                            ? ""
                            : lector["MAS_ALERGIAS"].ToString(),
                        PropietarioId = Convert.ToInt32(
                            lector["MAS_PRO_ID"]),
                        IdentificacionPropietario =
                            lector["PRO_IDENTIFICACION"].ToString(),
                        NombrePropietario =
                            lector["PRO_PRIMER_NOMBRE"].ToString(),
                        ApellidoPropietario =
                            lector["PRO_PRIMER_APELLIDO"].ToString()
                    });
                }
            }
        }

        return mascotas;
    }

    // BUSCAR MASCOTA POR ID
    public Mascota BuscarMascotaPorId(int id)
    {
        using (SqlConnection conexion = ConexionBD.ObtenerConexion())
        using (SqlCommand comando = new SqlCommand(
            "dbo.sp_BuscarMascotaPorId", conexion))
        {
            comando.CommandType = CommandType.StoredProcedure;
            comando.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            conexion.Open();

            using (SqlDataReader lector = comando.ExecuteReader())
            {
                if (!lector.Read())
                    return null;

                return new Mascota
                {
                    Id = Convert.ToInt32(lector["MAS_ID"]),
                    Nombre = lector["MAS_NOMBRE"].ToString(),
                    FechaNacimiento = Convert.ToDateTime(
                        lector["MAS_FECHA_NACIMIENTO"]),
                    Sexo = lector["MAS_SEXO"].ToString(),
                    Peso = Convert.ToDecimal(lector["MAS_PESO"]),
                    Alergias = lector["MAS_ALERGIAS"] == DBNull.Value
                        ? ""
                        : lector["MAS_ALERGIAS"].ToString(),
                    PropietarioId = Convert.ToInt32(
                        lector["MAS_PRO_ID"])
                };
            }
        }
    }

    // REGISTRAR MASCOTA
    public int GuardarMascota(Mascota mascota)
    {
        using (SqlConnection conexion = ConexionBD.ObtenerConexion())
        using (SqlCommand comando = new SqlCommand(
            "dbo.sp_CrearMascota", conexion))
        {
            comando.CommandType = CommandType.StoredProcedure;

            AgregarParametrosMascota(comando, mascota);

            conexion.Open();

            return Convert.ToInt32(comando.ExecuteScalar());
        }
    }

    // MODIFICAR MASCOTA
    public void ActualizarMascota(Mascota mascota)
    {
        using (SqlConnection conexion = ConexionBD.ObtenerConexion())
        using (SqlCommand comando = new SqlCommand(
            "dbo.sp_ActualizarMascota", conexion))
        {
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.Add("@Id", SqlDbType.Int)
                .Value = mascota.Id;

            AgregarParametrosMascota(comando, mascota, true);

            conexion.Open();
            comando.ExecuteNonQuery();
        }
    }

    // ELIMINAR MASCOTA
    public void EliminarMascota(int id)
    {
        using (SqlConnection conexion = ConexionBD.ObtenerConexion())
        using (SqlCommand comando = new SqlCommand(
            "dbo.sp_EliminarMascota", conexion))
        {
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            conexion.Open();
            comando.ExecuteNonQuery();
        }
    }

    // PARAMETROS COMUNES PARA GUARDAR Y MODIFICAR
    private void AgregarParametrosMascota(
        SqlCommand comando,
        Mascota mascota,
        bool esModificacion = false)
    {
        comando.Parameters.Add("@Nombre", SqlDbType.VarChar, 15)
            .Value = mascota.Nombre;

        comando.Parameters.Add("@FechaNacimiento", SqlDbType.Date)
            .Value = mascota.FechaNacimiento;

        comando.Parameters.Add("@Sexo", SqlDbType.VarChar, 1)
            .Value = mascota.Sexo;

        SqlParameter parametroPeso =
            comando.Parameters.Add("@Peso", SqlDbType.Decimal);

        parametroPeso.Precision = 5;
        parametroPeso.Scale = 2;
        parametroPeso.Value = mascota.Peso;

        comando.Parameters.Add("@Alergias", SqlDbType.VarChar, 150)
            .Value = string.IsNullOrWhiteSpace(mascota.Alergias)
                ? (object)DBNull.Value
                : mascota.Alergias;

        comando.Parameters.Add("@PropietarioId", SqlDbType.Int)
            .Value = mascota.PropietarioId;

        comando.Parameters.Add("@Usuario", SqlDbType.VarChar, 15)
            .Value = esModificacion
                ? mascota.ModificadoPor
                : mascota.AdicionadoPor;
    }
}
