<%@ Page Title="Actualizar Mi Información | Facturación" EnableEventValidation="false" Language="VB" MasterPageFile="~/master/principal.master" AutoEventWireup="false" Inherits="WebAppVB.herramientas_usuario_editar" Codebehind="perfil.aspx.vb" %>

<%@ Register Src="~/usercontrol/menu_tools.ascx" TagPrefix="uc1" TagName="menu_tools" %>

<asp:Content ID="c1" ContentPlaceHolderID="head" runat="Server"></asp:Content>

<asp:Content ID="c3" ContentPlaceHolderID="cpTitulo" runat="Server">
    <h1>Actualizar Mi Información</h1>
</asp:Content>

<asp:Content ID="c4" ContentPlaceHolderID="cpUbicacion" runat="Server">
    <a href="../Default.aspx">Escritorio</a>
    <label>&gt;</label>
    <a href="perfil.aspx">Actualizar Mi Información</a>
</asp:Content>

<asp:Content ID="c5" ContentPlaceHolderID="MainContentPlaceHolder" runat="Server">
    <div id="container-fluid">
        <div class="row p-2">

            <%--<uc1:menu_tools runat="server" ID="menu_tools" />--%>
            <asp:UpdatePanel ID="upMensaje" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Literal ID="ltMensaje" runat="server"></asp:Literal>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnGuardarPassword" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>

            <div class="col-6">
                <div class="card shadow mb-4">
                    <div class="card-header py-6 bg-dark">
                        <h6 class="m-0 font-weight-bold text-white">Mis Datos</h6>
                    </div>
                    <div class="card-body">
                        <div class="row mb-2">
                            <div class="col">
                                <label class="form-label">Usuario</label>
                                <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control form-control-sm" AutoCompleteType="None" Enabled="false"></asp:TextBox>
                            </div>
                            <div class="col mb-2">
                                <label class="form-label">Correo</label>
                                <asp:TextBox ID="txtCorreo" runat="server" CssClass="form-control form-control-sm" AutoCompleteType="None" MaxLength="100" TextMode="Email"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col mb-2">
                                <label class="form-label">Nombre</label>
                                <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control form-control-sm" AutoCompleteType="None" MaxLength="50"></asp:TextBox>
                            </div>
                            <div class="col mb-2">
                                <label class="form-label">Apellido</label>
                                <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control form-control-sm" AutoCompleteType="None" MaxLength="50"></asp:TextBox>

                            </div>
                        </div>
                        <div class="row">
                            <div class="col mb-2">
                                <label class="form-label">Rol</label>
                                <asp:TextBox ID="txtRol" runat="server" CssClass="form-control form-control-sm" Enabled="false" />
                            </div>

                            <div class="col mb-2">
                                <label class="form-label">Cedula</label>
                                <asp:TextBox ID="txtCedula" runat="server" CssClass="form-control form-control-sm" AutoCompleteType="None" MaxLength="20"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col mb-2">
                                <label class="form-label">Dirección</label>
                                <asp:TextBox ID="txtDireccion" runat="server" CssClass="form-control form-control-sm" AutoCompleteType="None" MaxLength="500" TextMode="MultiLine"></asp:TextBox>
                            </div>
                            <div class="col mb-2">
                                <label class="form-label">Teléfono</label>
                                <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control form-control-sm" AutoCompleteType="None" MaxLength="20" TextMode="Phone"></asp:TextBox>
                            </div>
                        </div>
                        <hr />
                        <div class="row">
                            <div class="col-6 d-grid mx-auto">
                                <asp:LinkButton ID="btnGuardar" runat="server" CssClass="btn btn-success btn-sm" Text="Guardar"></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-3">
                <div class="card shadow mb-4">
                    <div class="card-header py-6 bg-dark">
                        <h6 class="m-0 font-weight-bold text-white">Cambiar Contraseña</h6>
                    </div>
                    <div class="card-body">
                        <div class="row d-flex flex-column">
                            <div class="col mb-2">
                                <label class="form-label">Contraseña Actual</label>
                                <asp:TextBox ID="txtPasswordActual" runat="server" CssClass="form-control form-control-sm" TextMode="Password" AutoCompleteType="None" MaxLength="30"></asp:TextBox>
                            </div>
                            <div class="col mb-2">
                                <label class="form-label">Nueva Contraseña</label>
                                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control form-control-sm" TextMode="Password" AutoCompleteType="None" MaxLength="30"></asp:TextBox>
                            </div>
                            <div class="col mb-4">
                                <label class="form-label">Confirmar Contraseña</label>
                                <asp:TextBox ID="txtConfirmarPassword" runat="server" CssClass="form-control form-control-sm" TextMode="Password" AutoCompleteType="None" MaxLength="30"></asp:TextBox>
                            </div>
                            <hr />
                            <div class="col d-grid mx-auto">
                                <asp:LinkButton ID="btnGuardarPassword" runat="server" CssClass="btn btn-success btn-sm">Guardar Cambios</asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </div>
    </div>

    <asp:UpdateProgress ID="uprLoad" runat="server">
        <ProgressTemplate>
            <div class="loader">
                <div>
                    <img alt="Cargando" src="../img/load.gif" />
                    <p>Cargando...</p>
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>

<asp:Content ID="c6" ContentPlaceHolderID="cpScripts" runat="server"></asp:Content>

