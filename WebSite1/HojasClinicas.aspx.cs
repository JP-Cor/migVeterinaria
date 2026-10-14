
using System;
using System.Globalization;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class HojasClinicas : Page
{
    private readonly HojaClinicaRepositorio repositorio =
        new HojaClinicaRepositorio();

    private readonly MascotaRepositorio mascotaRepositorio =
        new MascotaRepositorio();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CargarMascotas();
            CargarHojasClinicas();
        }
    }

    // CARGAR MASCOTAS
    private void CargarMascotas()
    {
        try
        {
            var mascotas = mascotaRepositorio.ObtenerMascotas();

            ddlMascotas.DataSource = mascotas;
            ddlMascotas.DataTextField = "Nombre";
            ddlMascotas.DataValueField = "Id";
            ddlMascotas.DataBind();

            ddlMascotas.Items.Insert(
                0, new ListItem("Seleccione una mascota", ""));
        }
        catch (Exception ex)
        {
            RegistrarError(ex);
            lblMensaje.Text =
                "No fue posible cargar las mascotas.";
        }
    }

    // LISTAR HOJAS CLINICAS
    private void CargarHojasClinicas()
    {
        try
        {
            gvHojasClinicas.DataSource =
                repositorio.ObtenerHojasClinicas();

            gvHojasClinicas.DataBind();
        }
        catch (Exception ex)
        {
            RegistrarError(ex);
            lblMensaje.Text =
                "No fue posible cargar las hojas clínicas.";
        }
    }

    // VALIDAR DATOS
    private bool ValidarFormulario()
    {
        DateTime fecha;
        int mascotaId;

        if (string.IsNullOrWhiteSpace(txtFechaAtencion.Text) ||
            string.IsNullOrWhiteSpace(txtSintomas.Text) ||
            string.IsNullOrWhiteSpace(txtDiagnostico.Text) ||
            string.IsNullOrWhiteSpace(txtTratamiento.Text) ||
            string.IsNullOrWhiteSpace(ddlMascotas.SelectedValue))
        {
            lblMensaje.Text =
                "Complete todos los campos obligatorios.";
            return false;
        }

        if (!DateTime.TryParseExact(
            txtFechaAtencion.Text,
            "yyyy-MM-dd",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out fecha) ||
            fecha.Date > DateTime.Today)
        {
            lblMensaje.Text =
                "Ingrese una fecha de atención válida.";
            return false;
        }

        if (txtSintomas.Text.Trim().Length > 150 ||
            txtDiagnostico.Text.Trim().Length > 150 ||
            txtTratamiento.Text.Trim().Length > 150)
        {
            lblMensaje.Text =
                "Los campos clínicos no pueden superar 150 caracteres.";
            return false;
        }

        if (!int.TryParse(
            ddlMascotas.SelectedValue, out mascotaId) ||
            mascotaId <= 0)
        {
            lblMensaje.Text =
                "Seleccione una mascota válida.";
            return false;
        }

        return true;
    }

    // OBTENER DATOS DEL FORMULARIO
    private HojaClinica ObtenerDatosFormulario()
    {
        return new HojaClinica
        {
            FechaAtencion = DateTime.ParseExact(
                txtFechaAtencion.Text,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture),

            Sintomas = txtSintomas.Text.Trim(),
            Diagnostico = txtDiagnostico.Text.Trim(),
            Tratamiento = txtTratamiento.Text.Trim(),

            MascotaId = Convert.ToInt32(
                ddlMascotas.SelectedValue)
        };
    }

    // GUARDAR
    protected void btnGuardar_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(hfHojaId.Value))
        {
            lblMensaje.Text =
                "Presione Limpiar antes de registrar otra hoja.";
            return;
        }

        if (!ValidarFormulario())
            return;

        try
        {
            HojaClinica hoja = ObtenerDatosFormulario();
            hoja.AdicionadoPor = "SISTEMA";

            int id = repositorio.GuardarHojaClinica(hoja);

            LimpiarFormulario();
            CargarHojasClinicas();

            lblMensaje.Text =
                "Hoja clínica registrada correctamente. ID: " + id;
        }
        catch (Exception ex)
        {
            RegistrarError(ex);
            lblMensaje.Text =
                "No fue posible registrar la hoja clínica.";
        }
    }

    // SELECCIONAR HOJA CLINICA
    protected void gvHojasClinicas_SelectedIndexChanged(
        object sender, EventArgs e)
    {
        try
        {
            int id = Convert.ToInt32(
                gvHojasClinicas.SelectedDataKey.Value);

            HojaClinica hoja =
                repositorio.BuscarHojaClinicaPorId(id);

            if (hoja == null)
            {
                lblMensaje.Text =
                    "La hoja clínica no existe.";
                return;
            }

            hfHojaId.Value = hoja.Id.ToString();

            txtFechaAtencion.Text =
                hoja.FechaAtencion.ToString("yyyy-MM-dd");

            txtSintomas.Text = hoja.Sintomas;
            txtDiagnostico.Text = hoja.Diagnostico;
            txtTratamiento.Text = hoja.Tratamiento;

            ddlMascotas.SelectedValue =
                hoja.MascotaId.ToString();

            lblMensaje.Text =
                "Hoja clínica seleccionada. ID: " + hoja.Id;
        }
        catch (Exception ex)
        {
            RegistrarError(ex);
            lblMensaje.Text =
                "No fue posible seleccionar la hoja clínica.";
        }
    }

    // MODIFICAR
    protected void btnModificar_Click(object sender, EventArgs e)
    {
        int id;

        if (!int.TryParse(hfHojaId.Value, out id) || id <= 0)
        {
            lblMensaje.Text =
                "Seleccione primero una hoja clínica.";
            return;
        }

        if (!ValidarFormulario())
            return;

        try
        {
            HojaClinica hoja = ObtenerDatosFormulario();

            hoja.Id = id;
            hoja.ModificadoPor = "SISTEMA";

            repositorio.ActualizarHojaClinica(hoja);

            LimpiarFormulario();
            CargarHojasClinicas();

            lblMensaje.Text =
                "Hoja clínica modificada correctamente.";
        }
        catch (Exception ex)
        {
            RegistrarError(ex);
            lblMensaje.Text =
                "No fue posible modificar la hoja clínica.";
        }
    }

    // ELIMINAR
    protected void btnEliminar_Click(object sender, EventArgs e)
    {
        int id;

        if (!int.TryParse(hfHojaId.Value, out id) || id <= 0)
        {
            lblMensaje.Text =
                "Seleccione primero una hoja clínica.";
            return;
        }

        try
        {
            repositorio.EliminarHojaClinica(id);

            LimpiarFormulario();
            CargarHojasClinicas();

            lblMensaje.Text =
                "Hoja clínica eliminada correctamente.";
        }
        catch (SqlException ex)
        {
            RegistrarError(ex);

            lblMensaje.Text = ex.Number == 50021
                ? "La hoja clínica ya no existe."
                : "No fue posible eliminar la hoja clínica.";
        }
        catch (Exception ex)
        {
            RegistrarError(ex);
            lblMensaje.Text =
                "Ocurrió un error al eliminar la hoja clínica.";
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
        hfHojaId.Value = "";
        ddlMascotas.SelectedIndex = 0;
        txtFechaAtencion.Text = "";
        txtSintomas.Text = "";
        txtDiagnostico.Text = "";
        txtTratamiento.Text = "";
        gvHojasClinicas.SelectedIndex = -1;
    }

    private void RegistrarError(Exception ex)
    {
        System.Diagnostics.Debug.WriteLine(ex.ToString());
    }
}
