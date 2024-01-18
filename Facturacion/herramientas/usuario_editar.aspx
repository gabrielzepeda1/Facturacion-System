<%@ Page Title="Actualizar Mi Información | Facturación" EnableEventValidation="false" Language="VB" MasterPageFile="~/master/principal.master" AutoEventWireup="false" CodeFile="usuario_editar.aspx.vb" Inherits="herramientas_usuario_editar" %>

<%@ Register Src="~/usercontrol/menu_tools.ascx" TagPrefix="uc1" TagName="menu_tools" %>

<asp:Content ID="c1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="c3" ContentPlaceHolderID="cpTitulo" Runat="Server">
    <h1>Actualizar Mi Información</h1>
</asp:Content>

<asp:Content ID="c4" ContentPlaceHolderID="cpUbicacion" Runat="Server">
    <a href="../Default.aspx">Escritorio</a>
    <label>&gt;</label>
    <a href="usuario_editar.aspx">Actualizar Mi Información</a>
</asp:Content>

<asp:Content ID="c5" ContentPlaceHolderID="MainContentPlaceHolder" Runat="Server">
    <asp:ScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ScriptManager>
    
    <div id="main-form">
        <div id="main-form-content">
            
            <uc1:menu_tools runat="server" ID="menu_tools" />

            <div id="main-form-content-field" style="padding-top:22px;">

                <asp:UpdatePanel ID="upMensaje" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Literal ID="ltMensaje" runat="server"></asp:Literal>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnGuardarPassword" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>

                <div class="form-no-popup">
                    <h3 style="margin-bottom:22px;">Información General</h3>

                    <asp:TextBox ID="txtUsuario" runat="server" AutoCompleteType="None" Enabled="false"></asp:TextBox>

                    <div id="colaborador">
                        <asp:TextBox ID="txtCedula" runat="server" AutoCompleteType="None" MaxLength="20" Enabled="false"></asp:TextBox> 

                        <asp:TextBox ID="txtNombre" runat="server" AutoCompleteType="None" MaxLength="50" Enabled="false"></asp:TextBox>

                        <asp:TextBox ID="txtTelefono" runat="server" AutoCompleteType="None" MaxLength="20" TextMode="Phone"></asp:TextBox>

                        <asp:TextBox ID="txtCorreo" runat="server" AutoCompleteType="None" MaxLength="100" TextMode="Email"></asp:TextBox>

                        <asp:TextBox ID="txtDireccion" runat="server" AutoCompleteType="None" MaxLength="500" TextMode="MultiLine"></asp:TextBox>
                    </div>

                    <h3>Actualizar Contraseña</h3>

                    <asp:TextBox ID="txtPasswordActual" runat="server" TextMode="Password" AutoCompleteType="None" MaxLength="30" ></asp:TextBox>

                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" AutoCompleteType="None" MaxLength="30"  CssClass="nomarignb"></asp:TextBox>
                    <label class="nota marginb11"><strong><i class="fas fa-exclamation-triangle"></i> Advertencia:</strong> La contraseña debe tener un máximo de 30 caracteres. La contraseña debe llevar mayúscula, minúscula, número y al menos un carácter especial.</label>

                    <asp:TextBox ID="txtConfirmarPassword" runat="server" TextMode="Password" AutoCompleteType="None" MaxLength="30"></asp:TextBox>

                </div>

                <div id="Control">
                    <asp:LinkButton ID="btnGuardar" runat="server" CssClass="new">
                        <i class="fas fa-plus-circle"></i><label>Guardar</label>
                    </asp:LinkButton>

                    <asp:LinkButton ID="btnGuardarPassword" runat="server" CssClass="edit">
                        <i class="fas fa-sync-alt"></i><label>Actualizar Contraseña</label>
                    </asp:LinkButton>
                </div>



            </div><!-- #main-form-content-field -->

            <div class="clear"></div>



        </div><!-- =========== #main-form-content ====================================================================== -->
    </div><!-- =========== #main-form ====================================================================== -->

    <asp:UpdateProgress ID="uprLoad" runat="server">
        <ProgressTemplate>
            <div class="loader">
                <div>
                    <img alt="Cargando" src="../img/load.gif"/>
                    <p>Cargando...</p>
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>

<asp:Content ID="c6" ContentPlaceHolderID="cpScripts" Runat="Server">
</asp:Content>

