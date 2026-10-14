
using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web.UI;

public partial class Propietarios : Page
{
    private readonly PropietarioRepositorio repositorio =
        new PropietarioRepositorio();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CargarPropietarios();
        }
    }

    // CONSULTAR PROPIETARIOS
    private void CargarPropietarios()
    {
        try
        {
            gvPropietarios.DataSource =
                repositorio.ObtenerPropietarios();

            gvPropietarios.DataBind();
        }
        catch (Exception ex)
        {
            RegistrarError(ex);
            lblMensaje.Text =
                "No fue posible consultar los propietarios.";
        }
    }

    // VALIDAR DATOS DEL FORMULARIO
    private bool ValidarFormulario()
    {
        if (string.IsNullOrWhiteSpace(txtIdentificacion.Text) ||
            string.IsNullOrWhiteSpace(txtPrimerNombre.Text) ||
            string.IsNullOrWhiteSpace(txtSegundoNombre.Text) ||
            string.IsNullOrWhiteSpace(txtPrimerApellido.Text) ||
            string.IsNullOrWhiteSpace(txtSegundoApellido.Text) ||
            string.IsNullOrWhiteSpace(txtTelefono.Text) ||
            string.IsNullOrWhiteSpace(txtCorreo.Text))
        {
            lblMensaje.Text =
                "Todos los campos son obligatorios.";
            return false;
        }

        if (!Regex.IsMatch(txtTelefono.Text.Trim(), @"^\d{8}$"))
        {
            lblMensaje.Text =
                "El teléfono debe contener 8 dígitos.";
            return false;
        }

        if (!Regex.IsMatch(txtCorreo.Text.Trim(),
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            lblMensaje.Text =
                "Ingrese un correo electrónico válido.";
            return false;
        }

        return true;
    }

    // OBTENER DATOS DEL FORMULARIO
    private Propietario ObtenerDatosFormulario()
    {
        return new Propietario
        {
            Identificacion = txtIdentificacion.Text.Trim(),
            PrimerNombre = txtPrimerNombre.Text.Trim(),
            SegundoNombre = txtSegundoNombre.Text.Trim(),
            PrimerApellido = txtPrimerApellido.Text.Trim(),
            SegundoApellido = txtSegundoApellido.Text.Trim(),
            Telefono = txtTelefono.Text.Trim(),
            Correo = txtCorreo.Text.Trim()
        };
    }

    // GUARDAR
    protected void btnGuardar_Click(object sender, EventArgs e)
    {
        if (!ValidarFormulario())
            return;

        if (!string.IsNullOrEmpty(hfPropietarioId.Value))
        {
            lblMensaje.Text =
                "Presione Limpiar antes de registrar uno nuevo.";
            return;
        }

        try
        {
            Propietario propietario = ObtenerDatosFormulario();
            propietario.AdicionadoPor = "SISTEMA";

            int id = repositorio.GuardarPropietario(propietario);

            LimpiarFormulario();
            CargarPropietarios();

            lblMensaje.Text =
                "Propietario registrado correctamente. ID: " + id;
        }
        catch (SqlException ex)
        {
            RegistrarError(ex);

            lblMensaje.Text = ex.Number == 50001
                ? "La identificación ya está registrada."
                : "No fue posible guardar el propietario.";
        }
        catch (Exception ex)
        {
            RegistrarError(ex);
            lblMensaje.Text =
                "Ocurrió un error al registrar el propietario.";
        }
    }

    // SELECCIONAR PROPIETARIO
    protected void gvPropietarios_SelectedIndexChanged(
        object sender, EventArgs e)
    {
        try
        {
            int id = Convert.ToInt32(
                gvPropietarios.SelectedDataKey.Value);

            Propietario propietario =
                repositorio.BuscarPropietarioPorId(id);

            if (propietario == null)
            {
                lblMensaje.Text = "Propietario no encontrado.";
                return;
            }

            hfPropietarioId.Value = propietario.Id.ToString();
            txtIdentificacion.Text = propietario.Identificacion;
            txtPrimerNombre.Text = propietario.PrimerNombre;
            txtSegundoNombre.Text = propietario.SegundoNombre;
            txtPrimerApellido.Text = propietario.PrimerApellido;
            txtSegundoApellido.Text = propietario.SegundoApellido;
            txtTelefono.Text = propietario.Telefono;
            txtCorreo.Text = propietario.Correo;

            lblMensaje.Text =
                "Propietario seleccionado. ID: " + propietario.Id;
        }
        catch (Exception ex)
        {
            RegistrarError(ex);
            lblMensaje.Text =
                "No fue posible seleccionar el propietario.";
        }
    }

    // MODIFICAR
    protected void btnModificar_Click(object sender, EventArgs e)
    {
        int id;

        if (!int.TryParse(hfPropietarioId.Value, out id) ||
            id <= 0)
        {
            lblMensaje.Text =
                "Seleccione primero un propietario.";
            return;
        }

        if (!ValidarFormulario())
            return;

        try
        {
            Propietario propietario = ObtenerDatosFormulario();
            propietario.Id = id;
            propietario.ModificadoPor = "SISTEMA";

            repositorio.ActualizarPropietario(propietario);

            LimpiarFormulario();
            CargarPropietarios();

            lblMensaje.Text =
                "Propietario modificado correctamente.";
        }
        catch (SqlException ex)
        {
            RegistrarError(ex);

            lblMensaje.Text = ex.Number == 50002
                ? "La identificación pertenece a otro propietario."
                : "No fue posible modificar el propietario.";
        }
        catch (Exception ex)
        {
            RegistrarError(ex);
            lblMensaje.Text =
                "Ocurrió un error al modificar el propietario.";
        }
    }

    // ELIMINAR
    protected void btnEliminar_Click(object sender, EventArgs e)
    {
        int id;

        if (!int.TryParse(hfPropietarioId.Value, out id) ||
            id <= 0)
        {
            lblMensaje.Text =
                "Seleccione primero un propietario.";
            return;
        }

        try
        {
            repositorio.EliminarPropietario(id);

            LimpiarFormulario();
            CargarPropietarios();

            lblMensaje.Text =
                "Propietario eliminado correctamente.";
        }
        catch (SqlException ex)
        {
            RegistrarError(ex);

            lblMensaje.Text = ex.Number == 50004
                ? "No se puede eliminar: tiene mascotas registradas."
                : "No fue posible eliminar el propietario.";
        }
        catch (Exception ex)
        {
            RegistrarError(ex);
            lblMensaje.Text =
                "Ocurrió un error al eliminar el propietario.";
        }
    }

    // LIMPIAR
    protected void btnLimpiar_Click(object sender, EventArgs e)
    {
        LimpiarFormulario();
        lblMensaje.Text = "Formulario limpio.";
    }

    private void LimpiarFormulario()
    {
        hfPropietarioId.Value = "";
        txtIdentificacion.Text = "";
        txtPrimerNombre.Text = "";
        txtSegundoNombre.Text = "";
        txtPrimerApellido.Text = "";
        txtSegundoApellido.Text = "";
        txtTelefono.Text = "";
        txtCorreo.Text = "";
        gvPropietarios.SelectedIndex = -1;
    }

    private void RegistrarError(Exception ex)
    {
        System.Diagnostics.Debug.WriteLine(ex.ToString());
    }
}
