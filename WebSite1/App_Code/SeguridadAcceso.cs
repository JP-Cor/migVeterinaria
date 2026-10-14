
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;

public static class SeguridadAcceso
{
    // Verificar que el usuario esté activo
    // y consultar su rol directamente en SQL Server.
    public static bool UsuarioActivo(
        string nombreUsuario,
        out string rol)
    {
        rol = null;

        if (string.IsNullOrWhiteSpace(nombreUsuario))
            return false;

        using (SqlConnection conexion =
            ConexionBD.ObtenerConexion())
        using (SqlCommand comando = new SqlCommand(
            @"SELECT USU_ESTADO, USU_ROL
              FROM dbo.VT_USUARIOS
              WHERE USU_USUARIO = @Usuario",
            conexion))
        {
            comando.Parameters.Add(
                "@Usuario", SqlDbType.VarChar, 15)
                .Value = nombreUsuario;

            conexion.Open();

            using (SqlDataReader lector =
                comando.ExecuteReader())
            {
                if (!lector.Read())
                    return false;

                string estado =
                    lector["USU_ESTADO"].ToString();

                rol = lector["USU_ROL"].ToString();

                return estado == "A";
            }
        }
    }

    // Verificar si el usuario actual es ADMIN.
    public static bool EsAdministrador()
    {
        HttpContext contexto = HttpContext.Current;

        if (contexto == null ||
            contexto.User == null ||
            contexto.User.Identity == null ||
            !contexto.User.Identity.IsAuthenticated)
        {
            return false;
        }

        string rol;

        return UsuarioActivo(
            contexto.User.Identity.Name,
            out rol) && rol == "ADMIN";
    }

    // Proteger páginas exclusivas para ADMIN.
    public static void RequerirAdministrador()
    {
        HttpContext contexto = HttpContext.Current;

        if (contexto == null)
            throw new InvalidOperationException(
                "No existe un contexto HTTP.");

        if (contexto.User == null ||
            contexto.User.Identity == null ||
            !contexto.User.Identity.IsAuthenticated)
        {
            contexto.Response.Redirect(
                "~/Login.aspx", false);

            contexto.ApplicationInstance.CompleteRequest();
            return;
        }

        if (!EsAdministrador())
        {
            contexto.Response.StatusCode = 403;
            contexto.Response.TrySkipIisCustomErrors = true;

            throw new HttpException(
                403,
                "Acceso denegado. Se requiere el rol ADMIN.");
        }
    }
}
