
using Owin;

namespace WebSite1
{
    public partial class Startup
    {
        // MigVeterinaria utiliza Forms Authentication
        // configurado en Web.config.
        //
        // Se deshabilita la autenticacion por cookies
        // de OWIN para evitar utilizar dos mecanismos
        // diferentes de inicio de sesion.

        public void ConfigureAuth(IAppBuilder app)
        {
            // No registrar middleware de autenticacion OWIN.
        }
    }
}
