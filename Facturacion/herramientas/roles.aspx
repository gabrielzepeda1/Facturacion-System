<%@ Page Title="Roles de Usuario | Facturación" Language="VB" EnableEventValidation="false" MasterPageFile="~/master/principal.master" AutoEventWireup="false" CodeFile="roles.aspx.vb" Inherits="herramientas_roles" %>

<asp:Content ID="C1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="../css/permisos-menu.css" />
</asp:Content>

<asp:Content ID="C2" ContentPlaceHolderID="cpTitulo" runat="Server">
    <h1>Roles de Usuario</h1>
</asp:Content>

<asp:Content ID="C3" ContentPlaceHolderID="cpUbicacion" runat="Server">
    <a href="../Default.aspx">Escritorio</a>
    <label>&gt;</label>
    <a href="roles.aspx">Roles de Usuario</a>
</asp:Content>

<asp:Content ID="C4" ContentPlaceHolderID="CP1" runat="Server">
    <asp:ScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ScriptManager>

    <div class="form-no-popup">
        <asp:UpdatePanel ID="upMensaje" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Literal ID="ltMensaje" runat="server"></asp:Literal>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="trvMenu" EventName="SelectedNodeChanged" />
                <asp:AsyncPostBackTrigger ControlID="trvPermisos" EventName="SelectedNodeChanged" />
                <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </div>

    <div id="main-form" class="container">

        <div class="row">
            <div class="col-md-2 bg-dark py-3 mx-1 vh-100">
                <div class="" role="group" aria-label="Control Buttons">
                    <button id="btnNuevo" class="btn btn-primary" type="button" data-bs-toggle="modal" data-bs-target="#crearRol">Nuevo</button>
                    <asp:LinkButton CssClass="btn btn-danger" ID="btnDelete" OnClientClick="deleteValidation(event)" Text="Eliminar" runat="server"></asp:LinkButton>
                </div>
                <div class="mt-2 w200px">
                    <asp:DropDownList ID="ddlRol" CssClass="form-select"  runat="server" AutoPostBack="true" />
                    <ajaxToolkit:CascadingDropDown
                        ID="CRol"
                        runat="server"
                        TargetControlID="ddlRol"
                        ServicePath="../services/WSCatProductos.asmx"
                        ServiceMethod="GetRol"
                        Category="CategoryRol"
                        PromptText="Seleccione Rol..."></ajaxToolkit:CascadingDropDown>
                    </div>
            </div>

            <div class="col-md-4 py-3">
                <div class="card">
                    <div class="card-header">
                        <h3 class="card-title fs-5">Opciones Disponibles</h3>
                    </div>
                    <div class="card-body py-0">
                        <asp:TreeView ID="trvMenu" runat="server" ImageSet="Simple" NodeIndent="10">
                            <ParentNodeStyle Font-Bold="True" CssClass="list-group" />
                            <HoverNodeStyle Font-Underline="True" ForeColor="#DD5555" />
                            <SelectedNodeStyle Font-Underline="True" ForeColor="#DD5555" HorizontalPadding="0px" VerticalPadding="0px" />
                            <NodeStyle CssClass="list-group-item" Font-Names="Verdana" Font-Size="7pt" ForeColor="Black" HorizontalPadding="0px" NodeSpacing="2px" VerticalPadding="0px" />
                        </asp:TreeView>
                    </div>
                </div>
            </div>

            <div class="col-md-4 py-3">
                <div class="card">
                    <div class="card-header">
                        <h3 class="card-title fs-5">Opciones Agregadas</h3>
                    </div>
                    <div class="card-body py-0">
                        <asp:UpdatePanel ID="upMenuPermisos" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TreeView ID="trvPermisos" runat="server" ImageSet="Simple" NodeIndent="10">
                                    <ParentNodeStyle Font-Bold="True" CssClass="list-group" />
                                    <HoverNodeStyle Font-Underline="True" ForeColor="#DD5555" />
                                    <SelectedNodeStyle Font-Underline="True" ForeColor="#DD5555" HorizontalPadding="0px" VerticalPadding="0px" />
                                    <NodeStyle CssClass="list-group-item" Font-Names="Verdana" Font-Size="7pt" ForeColor="Black" HorizontalPadding="0px" NodeSpacing="2px" VerticalPadding="0px" />
                                </asp:TreeView>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="trvMenu" EventName="SelectedNodeChanged" />
                                <asp:AsyncPostBackTrigger ControlID="trvPermisos" EventName="SelectedNodeChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlRol" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>



    <asp:UpdatePanel ID="upTable" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <div class="table-content">
                <asp:HiddenField ID="hdfCodigo" runat="server" />
            </div>

        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    <!-- =========== #main-form ====================================================================== -->
    <div class="modal fade" id="crearRol" tabindex="-1" aria-labelledby="crearRolLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h1 id="popuptittle" class="modal-title fs-5">Nuevo Rol de Usuario</h1>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <asp:TextBox ID="txtRol" runat="server" AutoCompleteType="None"></asp:TextBox>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" />
                </div>
            </div>
        </div>
    </div>

    <asp:UpdateProgress ID="uprGrid" runat="server">
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

<asp:Content ID="C5" ContentPlaceHolderID="cpScripts" runat="Server">
    <script>

        $(document).ready(function () {


        });

    </script>
</asp:Content>

