
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

public class UsuarioRepositorio
{
    // LISTAR USUARIOS
    public List<Usuario> ObtenerUsuarios()
    {
        List<Usuario> usuarios = new List<Usuario>();

        using (SqlConnection conexion = ConexionBD.ObtenerConexion())
        using (SqlCommand comando = new SqlCommand(
            "dbo.sp_ListarUsuarios", conexion))
        {
            comando.CommandType = CommandType.StoredProcedure;

            conexion.Open();

            using (SqlDataReader lector = comando.ExecuteReader())
            {
                while (lector.Read())
                {
                    usuarios.Add(MapearUsuario(lector));
                }
            }
        }

        return usuarios;
    }

    // BUSCAR USUARIO POR ID
    public Usuario BuscarUsuarioPorId(int id)
    {
        using (SqlConnection conexion = ConexionBD.ObtenerConexion())
        using (SqlCommand comando = new SqlCommand(
            "dbo.sp_BuscarUsuarioPorId", conexion))
        {
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.Add("@Id", SqlDbType.Int)
                .Value = id;

            conexion.Open();

            using (SqlDataReader lector = comando.ExecuteReader())
            {
                if (lector.Read())
                    return MapearUsuario(lector);
            }
        }

        return null;
    }

    // CREAR USUARIO CON CONTRASEÑA SEGURA
    public int GuardarUsuario(Usuario usuario, string clave)
    {
        if (usuario == null)
            throw new ArgumentNullException("usuario");

        if (usuario.Rol != "ADMIN" &&
            usuario.Rol != "USUARIO")
        {
            throw new ArgumentException("Rol no válido.");
        }

        string hash = SeguridadClave.GenerarHash(clave);

        using (SqlConnection conexion = ConexionBD.ObtenerConexion())
        using (SqlCommand comando = new SqlCommand(
            "dbo.sp_CrearUsuario", conexion))
        {
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.Add("@Usuario", SqlDbType.VarChar, 15)
                .Value = usuario.NombreUsuario;

            comando.Parameters.Add("@ClaveHash", SqlDbType.VarChar, 255)
                .Value = hash;

            comando.Parameters.Add("@Estado", SqlDbType.VarChar, 1)
                .Value = usuario.Estado;

            comando.Parameters.Add("@AdicionadoPor", SqlDbType.VarChar, 15)
                .Value = usuario.AdicionadoPor;

            comando.Parameters.Add("@Rol", SqlDbType.VarChar, 15)
                .Value = usuario.Rol;

            conexion.Open();

            return Convert.ToInt32(comando.ExecuteScalar());
        }
    }

    // ACTUALIZAR USUARIO
    public void ActualizarUsuario(Usuario usuario)
    {
        if (usuario == null)
            throw new ArgumentNullException("usuario");

        using (SqlConnection conexion = ConexionBD.ObtenerConexion())
        using (SqlCommand comando = new SqlCommand(
            "dbo.sp_ActualizarUsuario", conexion))
        {
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.Add("@Id", SqlDbType.Int)
                .Value = usuario.Id;

            comando.Parameters.Add("@Usuario", SqlDbType.VarChar, 15)
                .Value = usuario.NombreUsuario;

            comando.Parameters.Add("@Estado", SqlDbType.VarChar, 1)
                .Value = usuario.Estado;

            comando.Parameters.Add("@ModificadoPor", SqlDbType.VarChar, 15)
                .Value = usuario.ModificadoPor;

            conexion.Open();
            comando.ExecuteNonQuery();
        }
    }

    // DESACTIVAR USUARIO
    public void DesactivarUsuario(int id, string modificadoPor)
    {
        using (SqlConnection conexion = ConexionBD.ObtenerConexion())
        using (SqlCommand comando = new SqlCommand(
            "dbo.sp_DesactivarUsuario", conexion))
        {
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.Add("@Id", SqlDbType.Int)
                .Value = id;

            comando.Parameters.Add("@ModificadoPor", SqlDbType.VarChar, 15)
                .Value = modificadoPor;

            conexion.Open();
            comando.ExecuteNonQuery();
        }
    }

    // VALIDAR CREDENCIALES PARA LOGIN
    public Usuario ValidarCredenciales(
        string nombreUsuario, string clave)
    {
        if (string.IsNullOrWhiteSpace(nombreUsuario) ||
            string.IsNullOrEmpty(clave))
            return null;

        using (SqlConnection conexion = ConexionBD.ObtenerConexion())
        using (SqlCommand comando = new SqlCommand(
            "dbo.sp_ObtenerUsuarioParaLogin", conexion))
        {
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.Add("@Usuario", SqlDbType.VarChar, 15)
                .Value = nombreUsuario;

            conexion.Open();

            using (SqlDataReader lector = comando.ExecuteReader())
            {
                if (!lector.Read())
                    return null;

                string estado =
                    lector["USU_ESTADO"].ToString();

                if (estado != "A" ||
                    lector["USU_CLAVE_HASH"] == DBNull.Value)
                    return null;

                string hash =
                    lector["USU_CLAVE_HASH"].ToString();

                if (!SeguridadClave.VerificarClave(clave, hash))
                    return null;

                return new Usuario
                {
                    Id = Convert.ToInt32(lector["USU_ID"]),

                    NombreUsuario =
                        lector["USU_USUARIO"].ToString(),

                    Estado = estado,

                    Rol = lector["USU_ROL"].ToString()
                };
            }
        }
    }

    // MAPEAR DATOS DE SQL SERVER
    private Usuario MapearUsuario(SqlDataReader lector)
    {
        return new Usuario
        {
            Id = Convert.ToInt32(lector["USU_ID"]),

            NombreUsuario =
                lector["USU_USUARIO"].ToString(),

            Estado =
                lector["USU_ESTADO"].ToString(),

            Rol =
                lector["USU_ROL"].ToString(),

            AdicionadoPor =
                lector["USU_ADICIONADO_POR"].ToString(),

            FechaAdicion = Convert.ToDateTime(
                lector["USU_FECHA_ADICION"]),

            ModificadoPor =
                lector["USU_MODIFICADO_POR"] == DBNull.Value
                    ? null
                    : lector["USU_MODIFICADO_POR"].ToString(),

            FechaModificacion =
                lector["USU_FECHA_MODIFICACION"] == DBNull.Value
                    ? (DateTime?)null
                    : Convert.ToDateTime(
                        lector["USU_FECHA_MODIFICACION"])
        };
    }
}
