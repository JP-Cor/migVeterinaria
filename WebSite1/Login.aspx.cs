
using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;

public partial class Login : Page
{
    private readonly UsuarioRepositorio repositorio =
        new UsuarioRepositorio();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack && User.Identity.IsAuthenticated)
        {
            Response.Redirect("~/Default.aspx");
        }
    }

    protected void btnIngresar_Click(object sender, EventArgs e)
    {
        string nombreUsuario = txtUsuario.Text.Trim();
        string clave = txtClave.Text;

        if (string.IsNullOrWhiteSpace(nombreUsuario) ||
            string.IsNullOrEmpty(clave))
        {
            lblMensaje.Text =
                "Ingrese su usuario y contraseña.";
            return;
        }

        try
        {
            Usuario usuario =
                repositorio.ValidarCredenciales(
                    nombreUsuario, clave);

            if (usuario == null)
            {
                lblMensaje.Text =
                    "Usuario o contraseña incorrectos.";
                return;
            }

            // Crear cookie de autenticación.
            FormsAuthentication.SetAuthCookie(
                usuario.NombreUsuario, false);

            // Redirigir a la página principal.
            Response.Redirect("~/Default.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.ToString());

            lblMensaje.Text =
                "No fue posible iniciar sesión. Intente nuevamente.";
        }
    }
}
