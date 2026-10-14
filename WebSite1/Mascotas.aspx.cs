
using System;
using System.Globalization;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mascotas : Page
{
    private readonly MascotaRepositorio repositorio =
        new MascotaRepositorio();

    private readonly PropietarioRepositorio propietarioRepositorio =
        new PropietarioRepositorio();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CargarPropietarios();
            CargarMascotas();
        }
    }

    // CARGAR PROPIETARIOS
    private void CargarPropietarios()
    {
        try
        {
            ddlPropietarios.DataSource =
                propietarioRepositorio.ObtenerPropietarios();

            ddlPropietarios.DataTextField = "Identificacion";
            ddlPropietarios.DataValueField = "Id";
            ddlPropietarios.DataBind();

            ddlPropietarios.Items.Insert(
                0, new ListItem("Seleccione propietario", ""));
        }
        catch (Exception ex)
        {
            RegistrarError(ex);
            lblMensaje.Text =
                "No fue posible cargar los propietarios.";
        }
    }

    // CARGAR MASCOTAS
    private void CargarMascotas()
    {
        try
        {
            gvMascotas.DataSource =
                repositorio.ObtenerMascotas();

            gvMascotas.DataBind();
        }
        catch (Exception ex)
        {
            RegistrarError(ex);
            lblMensaje.Text =
                "No fue posible cargar las mascotas.";
        }
    }

    // VALIDAR FORMULARIO
    private bool ValidarFormulario()
    {
        DateTime fecha;
        decimal peso;

        if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
            string.IsNullOrWhiteSpace(txtFechaNacimiento.Text) ||
            string.IsNullOrWhiteSpace(ddlSexo.SelectedValue) ||
            string.IsNullOrWhiteSpace(txtPeso.Text) ||
            string.IsNullOrWhiteSpace(ddlPropietarios.SelectedValue))
        {
            lblMensaje.Text =
                "Complete todos los campos obligatorios.";
            return false;
        }

        if (txtNombre.Text.Trim().Length > 15 ||
            txtAlergias.Text.Trim().Length > 150)
        {
            lblMensaje.Text =
                "Uno de los campos supera la longitud permitida.";
            return false;
        }

        if (!DateTime.TryParseExact(
            txtFechaNacimiento.Text,
            "yyyy-MM-dd",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out fecha) ||
            fecha.Date > DateTime.Today)
        {
            lblMensaje.Text =
                "Ingrese una fecha de nacimiento válida.";
            return false;
        }

        if (!decimal.TryParse(
            txtPeso.Text,
            NumberStyles.AllowDecimalPoint,
            CultureInfo.InvariantCulture,
            out peso) ||
            peso <= 0 ||
            peso > 999.99m ||
            decimal.Round(peso, 2) != peso)
        {
            lblMensaje.Text =
                "Ingrese un peso válido, mayor que 0 y máximo 999.99 kg, con dos decimales.";
            return false;
        }

        if (ddlSexo.SelectedValue != "M" &&
            ddlSexo.SelectedValue != "H")
        {
            lblMensaje.Text = "Seleccione un sexo válido.";
            return false;
        }

        int propietarioId;

        if (!int.TryParse(
            ddlPropietarios.SelectedValue,
            out propietarioId) ||
            propietarioId <= 0)
        {
            lblMensaje.Text =
                "Seleccione un propietario válido.";
            return false;
        }

        return true;
    }

    // OBTENER DATOS DEL FORMULARIO
    private Mascota ObtenerDatosFormulario()
    {
        return new Mascota
        {
            Nombre = txtNombre.Text.Trim(),
            FechaNacimiento = DateTime.ParseExact(
                txtFechaNacimiento.Text,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture),
            Sexo = ddlSexo.SelectedValue,
            Peso = decimal.Parse(
                txtPeso.Text,
                CultureInfo.InvariantCulture),
            Alergias = txtAlergias.Text.Trim(),
            PropietarioId =
                Convert.ToInt32(ddlPropietarios.SelectedValue)
        };
    }

    // GUARDAR
    protected void btnGuardar_Click(object sender, EventArgs e)
    {
        if (!ValidarFormulario())
            return;

        if (!string.IsNullOrEmpty(hfMascotaId.Value))
        {
            lblMensaje.Text =
                "Presione Limpiar antes de registrar otra mascota.";
            return;
        }

        try
        {
            Mascota mascota = ObtenerDatosFormulario();
            mascota.AdicionadoPor = "SISTEMA";

            int id = repositorio.GuardarMascota(mascota);

            LimpiarFormulario();
            CargarMascotas();

            lblMensaje.Text =
                "Mascota registrada correctamente. ID: " + id;
        }
        catch (Exception ex)
        {
            RegistrarError(ex);
            lblMensaje.Text =
                "No fue posible registrar la mascota.";
        }
    }

    // SELECCIONAR MASCOTA
    protected void gvMascotas_SelectedIndexChanged(
        object sender, EventArgs e)
    {
        try
        {
            int id = Convert.ToInt32(
                gvMascotas.SelectedDataKey.Value);

            Mascota mascota =
                repositorio.BuscarMascotaPorId(id);

            if (mascota == null)
            {
                lblMensaje.Text = "Mascota no encontrada.";
                return;
            }

            hfMascotaId.Value = mascota.Id.ToString();
            txtNombre.Text = mascota.Nombre;
            txtFechaNacimiento.Text =
                mascota.FechaNacimiento.ToString("yyyy-MM-dd");

            ddlSexo.SelectedValue = mascota.Sexo;
            txtPeso.Text = mascota.Peso.ToString(
                "0.##", CultureInfo.InvariantCulture);

            txtAlergias.Text = mascota.Alergias;

            ddlPropietarios.SelectedValue =
                mascota.PropietarioId.ToString();

            lblMensaje.Text =
                "Mascota seleccionada. ID: " + mascota.Id;
        }
        catch (Exception ex)
        {
            RegistrarError(ex);
            lblMensaje.Text =
                "No fue posible seleccionar la mascota.";
        }
    }

    // MODIFICAR
    protected void btnModificar_Click(object sender, EventArgs e)
    {
        int id;

        if (!int.TryParse(hfMascotaId.Value, out id) || id <= 0)
        {
            lblMensaje.Text =
                "Seleccione primero una mascota.";
            return;
        }

        if (!ValidarFormulario())
            return;

        try
        {
            Mascota mascota = ObtenerDatosFormulario();
            mascota.Id = id;
            mascota.ModificadoPor = "SISTEMA";

            repositorio.ActualizarMascota(mascota);

            LimpiarFormulario();
            CargarMascotas();

            lblMensaje.Text =
                "Mascota modificada correctamente.";
        }
        catch (Exception ex)
        {
            RegistrarError(ex);
            lblMensaje.Text =
                "No fue posible modificar la mascota.";
        }
    }

    // ELIMINAR
    protected void btnEliminar_Click(object sender, EventArgs e)
    {
        int id;

        if (!int.TryParse(hfMascotaId.Value, out id) || id <= 0)
        {
            lblMensaje.Text =
                "Seleccione primero una mascota.";
            return;
        }

        try
        {
            repositorio.EliminarMascota(id);

            LimpiarFormulario();
            CargarMascotas();

            lblMensaje.Text =
                "Mascota eliminada correctamente.";
        }
        catch (SqlException ex)
        {
            RegistrarError(ex);

            lblMensaje.Text = ex.Number == 50012
                ? "No se puede eliminar una mascota con hojas clínicas."
                : "No fue posible eliminar la mascota.";
        }
        catch (Exception ex)
        {
            RegistrarError(ex);
            lblMensaje.Text =
                "Ocurrió un error al eliminar la mascota.";
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
        hfMascotaId.Value = "";
        txtNombre.Text = "";
        txtFechaNacimiento.Text = "";
        ddlSexo.SelectedIndex = 0;
        txtPeso.Text = "";
        txtAlergias.Text = "";
        ddlPropietarios.SelectedIndex = 0;
        gvMascotas.SelectedIndex = -1;
    }

    private void RegistrarError(Exception ex)
    {
        System.Diagnostics.Debug.WriteLine(ex.ToString());
    }
}
