
<%@ Page Title="Mantenimiento de Mascotas"
    Language="C#"
    MasterPageFile="~/Site.master"
    AutoEventWireup="true"
    CodeFile="Mascotas.aspx.cs"
    Inherits="Mascotas" %>

<asp:Content ID="Content1"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="container">

        <h2>Mantenimiento de Mascotas</h2>
        <hr />

        <asp:Label ID="lblMensaje"
            runat="server"
            EnableViewState="false" />

        <asp:HiddenField ID="hfMascotaId"
            runat="server" />

        <h3>Datos de la Mascota</h3>

        <div class="row">

            <div class="col-md-4">
                <label>Nombre *</label>
                <asp:TextBox ID="txtNombre"
                    runat="server"
                    CssClass="form-control"
                    MaxLength="15" />
            </div>

            <div class="col-md-4">
                <label>Fecha de nacimiento *</label>
                <asp:TextBox ID="txtFechaNacimiento"
                    runat="server"
                    TextMode="Date"
                    CssClass="form-control" />
            </div>

            <div class="col-md-4">
                <label>Sexo *</label>
                <asp:DropDownList ID="ddlSexo"
                    runat="server"
                    CssClass="form-control">
                    <asp:ListItem Text="Seleccione" Value="" />
                    <asp:ListItem Text="Macho" Value="M" />
                    <asp:ListItem Text="Hembra" Value="H" />
                </asp:DropDownList>
            </div>

        </div>

        <br />

        <div class="row">

            <div class="col-md-4">
                <label>Peso (kg) *</label>
                <asp:TextBox ID="txtPeso"
                    runat="server"
                    CssClass="form-control"
                    TextMode="Number"
                    step="0.01" />
            </div>

            <div class="col-md-8">
                <label>Alergias</label>
                <asp:TextBox ID="txtAlergias"
                    runat="server"
                    CssClass="form-control"
                    MaxLength="150" />
            </div>

        </div>

        <br />

        <div class="row">

            <div class="col-md-6">
                <label>Propietario *</label>
                <asp:DropDownList ID="ddlPropietarios"
                    runat="server"
                    CssClass="form-control" />
            </div>

        </div>

        <br />

        <div class="mb-3">

            <asp:Button ID="btnGuardar"
                runat="server"
                Text="Guardar"
                CssClass="btn btn-primary"
                OnClick="btnGuardar_Click" />

            <asp:Button ID="btnModificar"
                runat="server"
                Text="Modificar"
                CssClass="btn btn-warning"
                OnClick="btnModificar_Click" />

            <asp:Button ID="btnEliminar"
                runat="server"
                Text="Eliminar"
                CssClass="btn btn-danger"
                OnClick="btnEliminar_Click"
                OnClientClick="return confirm('¿Está seguro de eliminar esta mascota?');" />

            <asp:Button ID="btnLimpiar"
                runat="server"
                Text="Limpiar"
                CssClass="btn btn-secondary"
                OnClick="btnLimpiar_Click" />

        </div>

        <hr />

        <h3>Lista de Mascotas</h3>

        <asp:GridView ID="gvMascotas"
            runat="server"
            AutoGenerateColumns="False"
            DataKeyNames="Id"
            CssClass="table table-striped table-bordered"
            EmptyDataText="No hay mascotas registradas."
            OnSelectedIndexChanged="gvMascotas_SelectedIndexChanged">

            <Columns>

                <asp:CommandField
                    ShowSelectButton="True"
                    SelectText="Seleccionar" />

                <asp:BoundField DataField="Id"
                    HeaderText="ID" />

                <asp:BoundField DataField="Nombre"
                    HeaderText="Nombre" />

                <asp:BoundField DataField="FechaNacimiento"
                    HeaderText="Fecha de Nacimiento"
                    DataFormatString="{0:dd/MM/yyyy}"
                    HtmlEncode="False" />

                <asp:BoundField DataField="Sexo"
                    HeaderText="Sexo" />

                <asp:BoundField DataField="Peso"
                    HeaderText="Peso (kg)" />

                <asp:BoundField DataField="Alergias"
                    HeaderText="Alergias" />

                <asp:BoundField DataField="NombrePropietario"
                    HeaderText="Nombre del Propietario" />

                <asp:BoundField DataField="ApellidoPropietario"
                    HeaderText="Apellido del Propietario" />

            </Columns>

        </asp:GridView>

    </div>

</asp:Content>
