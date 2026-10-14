
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Usuarios : Page
{
    private readonly UsuarioRepositorio repositorio =
        new UsuarioRepositorio();

    protected void Page_Init(object sender, EventArgs e)
    {
        // Verificar permisos antes de procesar
        // eventos o modificaciones.
        SeguridadAcceso.RequerirAdministrador();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LimpiarFormulario();
            CargarUsuarios();
        }
    }

    private void CargarUsuarios()
    {
        gvUsuarios.DataSource =
            repositorio.ObtenerUsuarios();

        gvUsuarios.DataBind();
    }

    protected void btnGuardar_Click(
        object sender, EventArgs e)
    {
        if (!SeguridadAcceso.EsAdministrador())
        {
            throw new HttpException(
                403, "Acceso denegado.");
        }

        string nombre = txtUsuario.Text.Trim();
        string clave = txtClave.Text;

        if (nombre.Length < 4 || nombre.Length > 15)
        {
            lblMensaje.Text =
                "El usuario debe tener entre 4 y 15 caracteres.";
            return;
        }

        if (ddlEstado.SelectedValue != "A" &&
            ddlEstado.SelectedValue != "I")
        {
            lblMensaje.Text = "Estado no válido.";
            return;
        }

        try
        {
            int id;

            if (!int.TryParse(hfUsuarioId.Value, out id))
                id = 0;

            if (id == 0)
            {
                // CREAR USUARIO
                if (clave.Length < 12 ||
                    clave.Length > 128)
                {
                    lblMensaje.Text =
                        "La contraseña debe tener entre " +
                        "12 y 128 caracteres.";
                    return;
                }

                if (ddlRol.SelectedValue != "ADMIN" &&
                    ddlRol.SelectedValue != "USUARIO")
                {
                    lblMensaje.Text = "Rol no válido.";
                    return;
                }

                Usuario nuevo = new Usuario
                {
                    NombreUsuario = nombre,
                    Estado = ddlEstado.SelectedValue,
                    Rol = ddlRol.SelectedValue,
                    AdicionadoPor = User.Identity.Name
                };

                repositorio.GuardarUsuario(nuevo, clave);

                lblMensaje.Text =
                    "Usuario registrado correctamente.";
            }
            else
            {
                // MODIFICAR USUARIO
                Usuario existente =
                    repositorio.BuscarUsuarioPorId(id);

                if (existente == null)
                {
                    lblMensaje.Text =
                        "El usuario no existe.";
                    return;
                }

                // Evitar desactivar la propia cuenta
                // desde el formulario de edición.
                if (existente.NombreUsuario.Equals(
                    User.Identity.Name,
                    StringComparison.OrdinalIgnoreCase)
                    && ddlEstado.SelectedValue == "I")
                {
                    lblMensaje.Text =
                        "No puede desactivar su propia cuenta.";
                    return;
                }

                existente.NombreUsuario = nombre;
                existente.Estado = ddlEstado.SelectedValue;
                existente.ModificadoPor = User.Identity.Name;

                repositorio.ActualizarUsuario(existente);

                lblMensaje.Text =
                    "Usuario actualizado correctamente.";
            }

            LimpiarFormulario();
            CargarUsuarios();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.ToString());

            lblMensaje.Text =
                "No fue posible guardar el usuario. " +
                "Verifique los datos ingresados.";
        }
    }

    protected void btnLimpiar_Click(
        object sender, EventArgs e)
    {
        SeguridadAcceso.RequerirAdministrador();

        LimpiarFormulario();
        lblMensaje.Text = "";
    }

    protected void gvUsuarios_RowCommand(
        object sender, GridViewCommandEventArgs e)
    {
        if (!SeguridadAcceso.EsAdministrador())
        {
            throw new HttpException(
                403, "Acceso denegado.");
        }

        if (e.CommandName != "EditarUsuario" &&
            e.CommandName != "DesactivarUsuario")
        {
            return;
        }

        int id;

        if (!int.TryParse(
            Convert.ToString(e.CommandArgument), out id)
            || id <= 0)
        {
            lblMensaje.Text = "Identificador no válido.";
            return;
        }

        try
        {
            Usuario usuario =
                repositorio.BuscarUsuarioPorId(id);

            if (usuario == null)
            {
                lblMensaje.Text =
                    "El usuario no existe.";
                return;
            }

            if (e.CommandName == "EditarUsuario")
            {
                hfUsuarioId.Value = usuario.Id.ToString();
                txtUsuario.Text = usuario.NombreUsuario;
                txtClave.Text = "";

                ddlRol.SelectedValue = usuario.Rol;
                ddlRol.Enabled = false;

                ddlEstado.SelectedValue = usuario.Estado;

                lblMensaje.Text =
                    "Modifique los datos y presione Guardar.";
            }
            else if (e.CommandName == "DesactivarUsuario")
            {
                // Impedir desactivar la cuenta propia.
                if (usuario.NombreUsuario.Equals(
                    User.Identity.Name,
                    StringComparison.OrdinalIgnoreCase))
                {
                    lblMensaje.Text =
                        "No puede desactivar su propia cuenta.";
                    return;
                }

                repositorio.DesactivarUsuario(
                    id, User.Identity.Name);

                LimpiarFormulario();
                CargarUsuarios();

                lblMensaje.Text =
                    "Usuario desactivado correctamente.";
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.ToString());

            lblMensaje.Text =
                "No fue posible completar la operación.";
        }
    }

    private void LimpiarFormulario()
    {
        hfUsuarioId.Value = "0";
        txtUsuario.Text = "";
        txtClave.Text = "";

        ddlRol.Enabled = true;
        ddlRol.SelectedValue = "USUARIO";

        ddlEstado.SelectedValue = "A";
    }
}
