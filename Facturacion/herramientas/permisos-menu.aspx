<%@ Page Title="Permisos de Menú | Facturación" Language="VB" EnableEventValidation="false" MasterPageFile="~/master/principal.master" AutoEventWireup="false" CodeFile="permisos-menu.aspx.vb" Inherits="admin_permisos_menu" %>

<%@ Register Src="~/usercontrol/menu_tools.ascx" TagPrefix="uc1" TagName="menu_tools" %>

<asp:Content ID="C1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="../css/permisos-menu.css" />
</asp:Content>

<asp:Content ID="C2" ContentPlaceHolderID="cpTitulo" runat="Server">
    <h1>Permisos del Menú</h1>
</asp:Content>

<asp:Content ID="C3" ContentPlaceHolderID="cpUbicacion" runat="Server">
    <a href="../Default.aspx">Escritorio</a>
    <label>&gt;</label>
    <a href="permisos-menu.aspx">Permisos del Menú</a>
</asp:Content>

<asp:Content ID="C4" ContentPlaceHolderID="CP1" runat="Server">
    <asp:ScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ScriptManager>

    <div id="main-form" class="px-2">
        <div class="container">

            <asp:UpdatePanel ID="upMensaje" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Literal ID="ltMensaje" runat="server"></asp:Literal>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ddlUsuario" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="trvMenu" EventName="SelectedNodeChanged" />
                    <asp:AsyncPostBackTrigger ControlID="trvPermisos" EventName="SelectedNodeChanged" />
                    <asp:AsyncPostBackTrigger ControlID="ddlRol" EventName="SelectedIndexChanged" />
                </Triggers>
            </asp:UpdatePanel>

            <div class="row my-3">
                <div class="col-3">
                    <asp:DropDownList ID="ddlUsuario" CssClass="form-select" runat="server" AutoPostBack="true" />
                    <ajaxToolkit:CascadingDropDown
                        ID="CUsuario"
                        runat="server"
                        TargetControlID="ddlUsuario"
                        ServicePath="../services/WSCatProductos.asmx"
                        ServiceMethod="GetUsuario"
                        Category="CategoryName"
                        PromptText="Seleccione Usuario..."></ajaxToolkit:CascadingDropDown>
                </div>

                <div class="col-3">
                    <asp:DropDownList ID="ddlRol" CssClass="form-select" runat="server" AutoPostBack="true" />
                    <ajaxToolkit:CascadingDropDown
                        ID="CRol"
                        runat="server"
                        TargetControlID="ddlRol"
                        ServicePath="../services/WSCatProductos.asmx"
                        ServiceMethod="GetRol"
                        Category="CategoryRol"
                        PromptText="Seleccione..."></ajaxToolkit:CascadingDropDown>
                </div>
            </div>

            <div class="container d-flex justify-content-center align-items-center">


                <div class="row">
                    <div class="col">
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

                    <div class="col">
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
                                        <asp:AsyncPostBackTrigger ControlID="ddlUsuario" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="ddlRol" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </div>
        <!-- =========== #main-form ====================================================================== -->
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
</asp:Content>
