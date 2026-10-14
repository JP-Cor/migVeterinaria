
using System;
using System.Web.UI;

public partial class SiteMaster : MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Mostrar el menú de módulos solamente
        // cuando el usuario haya iniciado sesión.
        phMenuPrivado.Visible =
            Context.User != null &&
            Context.User.Identity != null &&
            Context.User.Identity.IsAuthenticated;
    }
}
