
<%@ Page Language="C#"
    AutoEventWireup="true"
    CodeFile="ConfiguracionInicial.aspx.cs"
    Inherits="ConfiguracionInicial" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Configuración inicial - MigVeterinaria</title>
</head>
<body>
    <form id="form1" runat="server">
        <div style="max-width:420px;margin:60px auto;
                    font-family:Arial;">

            <h2>MigVeterinaria</h2>
            <h3>Crear administrador inicial</h3>

            <p>Usuario administrador</p>
            <asp:TextBox ID="txtUsuario"
                runat="server"
                MaxLength="15" />

            <p>Contraseña</p>
            <asp:TextBox ID="txtClave"
                runat="server"
                TextMode="Password" />

            <p>Confirmar contraseña</p>
            <asp:TextBox ID="txtConfirmar"
                runat="server"
                TextMode="Password" />

            <p>
                <asp:Button ID="btnCrear"
                    runat="server"
                    Text="Crear administrador"
                    OnClick="btnCrear_Click" />
            </p>

            <asp:Label ID="lblMensaje"
                runat="server" />

        </div>
    </form>
</body>
</html>
