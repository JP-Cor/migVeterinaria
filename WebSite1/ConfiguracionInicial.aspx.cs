
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

public partial class ConfiguracionInicial : Page
{
    private readonly UsuarioRepositorio repositorio =
        new UsuarioRepositorio();

    protected void Page_Load(object sender, EventArgs e)
    {
        // Esta herramienta solamente puede ejecutarse
        // desde el equipo local durante la instalación.
        if (!Request.IsLocal)
        {
            Response.StatusCode = 403;
            Response.End();
            return;
        }

        if (ExisteAdministrador())
        {
            btnCrear.Enabled = false;
            lblMensaje.Text =
                "Ya existe un administrador. " +
                "La configuración inicial está deshabilitada.";
        }
    }

    protected void btnCrear_Click(object sender, EventArgs e)
    {
        if (!Request.IsLocal || ExisteAdministrador())
        {
            lblMensaje.Text =
                "Configuración inicial no disponible.";
            return;
        }

        string nombre = txtUsuario.Text.Trim();
        string clave = txtClave.Text;

        if (nombre.Length < 4 || nombre.Length > 15)
        {
            lblMensaje.Text =
                "El usuario debe tener entre 4 y 15 caracteres.";
            return;
        }

        if (clave.Length < 12 ||
            clave.Length > 128 ||
            clave != txtConfirmar.Text)
        {
            lblMensaje.Text =
                "La contraseña debe tener entre 12 y 128 " +
                "caracteres y coincidir con la confirmación.";
            return;
        }

        try
        {
            Usuario usuario = new Usuario
            {
                NombreUsuario = nombre,
                Estado = "A",
                Rol = "ADMIN",
                AdicionadoPor = "INSTALACION"
            };

            int id = repositorio.GuardarUsuario(usuario, clave);

            btnCrear.Enabled = false;

            lblMensaje.Text =
                "Administrador creado correctamente. ID: " + id +
                ". Elimine ahora esta página y su código asociado.";
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.ToString());

            lblMensaje.Text =
                "No fue posible crear el administrador.";
        }
    }

    private bool ExisteAdministrador()
    {
        using (SqlConnection conexion =
            ConexionBD.ObtenerConexion())
        using (SqlCommand comando = new SqlCommand(
            "SELECT COUNT(*) FROM dbo.VT_USUARIOS " +
            "WHERE USU_ROL = 'ADMIN'", conexion))
        {
            conexion.Open();

            return Convert.ToInt32(
                comando.ExecuteScalar()) > 0;
        }
    }
}
