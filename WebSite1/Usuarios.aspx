
<%@ Page Title="Administración de Usuarios"
    Language="C#"
    MasterPageFile="~/Site.master"
    AutoEventWireup="true"
    CodeFile="Usuarios.aspx.cs"
    Inherits="Usuarios" %>

<asp:Content ID="Content1"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <h2>Administración de Usuarios</h2>

    <p>
        Registro y mantenimiento de usuarios
        de MigVeterinaria.
    </p>

    <asp:Label ID="lblMensaje"
        runat="server"
        EnableViewState="false"
        ForeColor="Red" />

    <hr />

    <h3>Datos del usuario</h3>

    <asp:HiddenField ID="hfUsuarioId"
        runat="server" />

    <div class="form-group">
        <label>Nombre de usuario</label>

        <asp:TextBox ID="txtUsuario"
            runat="server"
            CssClass="form-control"
            MaxLength="15" />
    </div>

    <div class="form-group">
        <label>Contraseña (solo usuarios nuevos)</label>

        <asp:TextBox ID="txtClave"
            runat="server"
            TextMode="Password"
            CssClass="form-control" />
    </div>

    <div class="form-group">
        <label>Rol</label>

        <asp:DropDownList ID="ddlRol"
            runat="server"
            CssClass="form-control">

            <asp:ListItem Text="Usuario"
                Value="USUARIO" />

            <asp:ListItem Text="Administrador"
                Value="ADMIN" />

        </asp:DropDownList>
    </div>

    <div class="form-group">
        <label>Estado</label>

        <asp:DropDownList ID="ddlEstado"
            runat="server"
            CssClass="form-control">

            <asp:ListItem Text="Activo"
                Value="A" />

            <asp:ListItem Text="Inactivo"
                Value="I" />

        </asp:DropDownList>
    </div>

    <br />

    <asp:Button ID="btnGuardar"
        runat="server"
        Text="Guardar"
        CssClass="btn btn-primary"
        OnClick="btnGuardar_Click" />

    <asp:Button ID="btnLimpiar"
        runat="server"
        Text="Limpiar"
        CssClass="btn btn-default"
        CausesValidation="false"
        OnClick="btnLimpiar_Click" />

    <hr />

    <h3>Usuarios registrados</h3>

    <asp:GridView ID="gvUsuarios"
        runat="server"
        AutoGenerateColumns="False"
        CssClass="table table-striped table-bordered"
        DataKeyNames="Id"
        OnRowCommand="gvUsuarios_RowCommand"
        EmptyDataText="No hay usuarios registrados.">

        <Columns>

            <asp:BoundField DataField="Id"
                HeaderText="ID" />

            <asp:BoundField DataField="NombreUsuario"
                HeaderText="Usuario" />

            <asp:BoundField DataField="Rol"
                HeaderText="Rol" />

            <asp:BoundField DataField="Estado"
                HeaderText="Estado" />

            <asp:TemplateField HeaderText="Acciones">
                <ItemTemplate>

                    <asp:LinkButton
                        ID="btnEditar"
                        runat="server"
                        Text="Editar"
                        CssClass="btn btn-sm btn-info"
                        CommandName="EditarUsuario"
                        CommandArgument='<%# Eval("Id") %>'
                        CausesValidation="false" />

                    <asp:LinkButton
                        ID="btnDesactivar"
                        runat="server"
                        Text="Desactivar"
                        CssClass="btn btn-sm btn-danger"
                        CommandName="DesactivarUsuario"
                        CommandArgument='<%# Eval("Id") %>'
                        CausesValidation="false"
                        OnClientClick="return confirm('¿Desea desactivar este usuario?');" />

                </ItemTemplate>
            </asp:TemplateField>

        </Columns>

    </asp:GridView>

</asp:Content>
