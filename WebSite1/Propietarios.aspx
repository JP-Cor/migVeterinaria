
<%@ Page Title="Mantenimiento de Propietarios"
    Language="C#"
    MasterPageFile="~/Site.master"
    AutoEventWireup="true"
    CodeFile="Propietarios.aspx.cs"
    Inherits="Propietarios" %>

<asp:Content ID="Content1"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="container">

        <h2>Mantenimiento de Propietarios</h2>
        <hr />

        <asp:Label ID="lblMensaje"
            runat="server"
            EnableViewState="false" />

        <asp:HiddenField ID="hfPropietarioId"
            runat="server" />

        <h3>Datos del Propietario</h3>

        <div class="row">
            <div class="col-md-4">
                <label>Identificación *</label>
                <asp:TextBox ID="txtIdentificacion"
                    runat="server"
                    CssClass="form-control"
                    MaxLength="15" />
            </div>

            <div class="col-md-4">
                <label>Primer nombre *</label>
                <asp:TextBox ID="txtPrimerNombre"
                    runat="server"
                    CssClass="form-control"
                    MaxLength="25" />
            </div>

            <div class="col-md-4">
                <label>Segundo nombre *</label>
                <asp:TextBox ID="txtSegundoNombre"
                    runat="server"
                    CssClass="form-control"
                    MaxLength="25" />
            </div>
        </div>

        <br />

        <div class="row">
            <div class="col-md-4">
                <label>Primer apellido *</label>
                <asp:TextBox ID="txtPrimerApellido"
                    runat="server"
                    CssClass="form-control"
                    MaxLength="25" />
            </div>

            <div class="col-md-4">
                <label>Segundo apellido *</label>
                <asp:TextBox ID="txtSegundoApellido"
                    runat="server"
                    CssClass="form-control"
                    MaxLength="25" />
            </div>

            <div class="col-md-4">
                <label>Teléfono *</label>
                <asp:TextBox ID="txtTelefono"
                    runat="server"
                    CssClass="form-control"
                    MaxLength="8" />
            </div>
        </div>

        <br />

        <div class="row">
            <div class="col-md-6">
                <label>Correo electrónico *</label>
                <asp:TextBox ID="txtCorreo"
                    runat="server"
                    TextMode="Email"
                    CssClass="form-control"
                    MaxLength="150" />
            </div>
        </div>

        <br />

        <!-- BOTONES CRUD -->
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
                OnClientClick="return confirm('¿Está seguro de eliminar este propietario?');" />

            <asp:Button ID="btnLimpiar"
                runat="server"
                Text="Limpiar"
                CssClass="btn btn-secondary"
                OnClick="btnLimpiar_Click" />

        </div>

        <hr />

        <h3>Lista de Propietarios</h3>

        <asp:GridView ID="gvPropietarios"
            runat="server"
            AutoGenerateColumns="False"
            DataKeyNames="Id"
            CssClass="table table-striped table-bordered"
            EmptyDataText="No hay propietarios registrados."
            OnSelectedIndexChanged="gvPropietarios_SelectedIndexChanged">

            <Columns>

                <asp:CommandField
                    ShowSelectButton="True"
                    SelectText="Seleccionar" />

                <asp:BoundField DataField="Id"
                    HeaderText="ID" />

                <asp:BoundField DataField="Identificacion"
                    HeaderText="Identificación" />

                <asp:BoundField DataField="PrimerNombre"
                    HeaderText="Primer Nombre" />

                <asp:BoundField DataField="SegundoNombre"
                    HeaderText="Segundo Nombre" />

                <asp:BoundField DataField="PrimerApellido"
                    HeaderText="Primer Apellido" />

                <asp:BoundField DataField="SegundoApellido"
                    HeaderText="Segundo Apellido" />

                <asp:BoundField DataField="Telefono"
                    HeaderText="Teléfono" />

                <asp:BoundField DataField="Correo"
                    HeaderText="Correo" />

            </Columns>

        </asp:GridView>

    </div>

</asp:Content>
