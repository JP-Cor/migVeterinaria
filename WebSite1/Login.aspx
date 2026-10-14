
<%@ Page Title="Iniciar sesión"
    Language="C#"
    MasterPageFile="~/Site.master"
    AutoEventWireup="true"
    CodeFile="Login.aspx.cs"
    Inherits="Login" %>

<asp:Content ID="Content1"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="container">

        <div class="row justify-content-center">

            <div class="col-md-5">

                <div class="card mt-5">

                    <div class="card-header text-center">
                        <h3>MigVeterinaria</h3>
                        <h5>Inicio de sesión</h5>
                    </div>

                    <div class="card-body">

                        <div class="form-group">
                            <label>Usuario</label>

                            <asp:TextBox
                                ID="txtUsuario"
                                runat="server"
                                CssClass="form-control"
                                MaxLength="15"
                                autocomplete="username" />
                        </div>

                        <br />

                        <div class="form-group">
                            <label>Contraseña</label>

                            <asp:TextBox
                                ID="txtClave"
                                runat="server"
                                TextMode="Password"
                                CssClass="form-control"
                                autocomplete="current-password" />
                        </div>

                        <br />

                        <asp:Button
                            ID="btnIngresar"
                            runat="server"
                            Text="Iniciar sesión"
                            CssClass="btn btn-primary"
                            OnClick="btnIngresar_Click" />

                        <br />
                        <br />

                        <asp:Label
                            ID="lblMensaje"
                            runat="server"
                            EnableViewState="false"
                            ForeColor="Red" />

                    </div>

                </div>

            </div>

        </div>

    </div>

</asp:Content>
