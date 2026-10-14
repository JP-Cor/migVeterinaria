
using System;
using System.Web.UI;

public partial class _Default : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                using (var conexion = ConexionBD.ObtenerConexion())
                {
                    conexion.Open();

                    System.Diagnostics.Debug.WriteLine(
                        "CONEXION EXITOSA A VETERINARIA_WEB_DB");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    "ERROR SQL: " + ex.Message);
            }
        }
    }
}
