
<%@ Page Title="Mantenimiento de Hojas Clínicas"
    Language="C#"
    MasterPageFile="~/Site.master"
    AutoEventWireup="true"
    CodeFile="HojasClinicas.aspx.cs"
    Inherits="HojasClinicas" %>

<asp:Content ID="Content1"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="container">

        <h2>Mantenimiento de Hojas Clínicas</h2>
        <hr />

        <!-- MENSAJES -->
        <asp:Label ID="lblMensaje"
            runat="server"
            EnableViewState="false" />

        <!-- ID DEL REGISTRO SELECCIONADO -->
        <asp:HiddenField ID="hfHojaId"
            runat="server" />

        <h3>Datos de la Atención Clínica</h3>

        <div class="row">

            <div class="col-md-6">
                <label>Mascota *</label>
                <asp:DropDownList ID="ddlMascotas"
                    runat="server"
                    CssClass="form-control" />
            </div>

            <div class="col-md-6">
                <label>Fecha de atención *</label>
                <asp:TextBox ID="txtFechaAtencion"
                    runat="server"
                    TextMode="Date"
                    CssClass="form-control" />
            </div>

        </div>

        <br />

        <div class="row">

            <div class="col-md-12">
                <label>Síntomas *</label>
                <asp:TextBox ID="txtSintomas"
                    runat="server"
                    TextMode="MultiLine"
                    Rows="3"
                    MaxLength="150"
                    CssClass="form-control" />
            </div>

        </div>

        <br />

        <div class="row">

            <div class="col-md-12">
                <label>Diagnóstico *</label>
                <asp:TextBox ID="txtDiagnostico"
                    runat="server"
                    TextMode="MultiLine"
                    Rows="3"
                    MaxLength="150"
                    CssClass="form-control" />
            </div>

        </div>

        <br />

        <div class="row">

            <div class="col-md-12">
                <label>Tratamiento *</label>
                <asp:TextBox ID="txtTratamiento"
                    runat="server"
                    TextMode="MultiLine"
                    Rows="3"
                    MaxLength="150"
                    CssClass="form-control" />
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
                OnClientClick="return confirm('¿Está seguro de eliminar esta hoja clínica?');" />

            <asp:Button ID="btnLimpiar"
                runat="server"
                Text="Limpiar"
                CssClass="btn btn-secondary"
                OnClick="btnLimpiar_Click" />

        </div>

        <hr />

        <h3>Lista de Hojas Clínicas</h3>

        <asp:GridView ID="gvHojasClinicas"
            runat="server"
            AutoGenerateColumns="False"
            DataKeyNames="Id"
            CssClass="table table-striped table-bordered"
            EmptyDataText="No hay hojas clínicas registradas."
            OnSelectedIndexChanged="gvHojasClinicas_SelectedIndexChanged">

            <Columns>

                <asp:CommandField
                    ShowSelectButton="True"
                    SelectText="Seleccionar" />

                <asp:BoundField DataField="Id"
                    HeaderText="ID" />

                <asp:BoundField DataField="FechaAtencion"
                    HeaderText="Fecha de Atención"
                    DataFormatString="{0:dd/MM/yyyy}" />

                <asp:BoundField DataField="NombreMascota"
                    HeaderText="Mascota" />

                <asp:BoundField DataField="NombrePropietario"
                    HeaderText="Nombre del Propietario" />

                <asp:BoundField DataField="ApellidoPropietario"
                    HeaderText="Apellido del Propietario" />

                <asp:BoundField DataField="Sintomas"
                    HeaderText="Síntomas" />

                <asp:BoundField DataField="Diagnostico"
                    HeaderText="Diagnóstico" />

                <asp:BoundField DataField="Tratamiento"
                    HeaderText="Tratamiento" />

            </Columns>

        </asp:GridView>

    </div>

</asp:Content>
