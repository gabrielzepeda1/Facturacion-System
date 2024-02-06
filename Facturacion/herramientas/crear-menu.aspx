<%@ Page Title="Gestionar Opciones del Menú | Facturación" EnableEventValidation="false" Language="VB" MasterPageFile="~/master/principal.master" AutoEventWireup="false" CodeFile="crear-menu.aspx.vb" Inherits="admin_crear_menu" %>

<%@ Register Src="~/usercontrol/menu_tools.ascx" TagPrefix="uc1" TagName="menu_tools" %>

<asp:Content ID="c1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="c2" ContentPlaceHolderID="cpTitulo" runat="Server">
    <h1>Gestionar Opciones del Menú</h1>
</asp:Content>

<asp:Content ID="c3" ContentPlaceHolderID="cpUbicacion" runat="Server">
    <a href="../Default.aspx">Escritorio</a>
    <label>&gt;</label>
    <a href="crear-menu.aspx">Gestionar Opciones del Menú</a>
</asp:Content>

<asp:Content ID="c4" ContentPlaceHolderID="MainContentPlaceHolder" runat="Server">

    <div id="wrapper">
        <div class="container-fluid d-flex flex-column bg-light">

            <%--<uc1:menu_tools runat="server" ID="menu_tools" />--%>

            <asp:UpdatePanel ID="upMensaje" runat="server" UpdateMode="Conditional">
                <ContentTemplate>

                    <asp:Literal ID="ltMensaje" runat="server"></asp:Literal>

                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnEliminar" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="trvMenu" EventName="SelectedNodeChanged" />
                </Triggers>
            </asp:UpdatePanel>

            <div class="row flex-nowrap">
                <div class="col-auto">

                    <asp:UpdatePanel ID="upMenu" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:HiddenField ID="hdfCodigo" runat="server" />

                            <asp:TreeView ID="trvMenu" runat="server" ImageSet="Simple" NodeIndent="10">
                                <ParentNodeStyle Font-Bold="False" />
                                <HoverNodeStyle Font-Underline="True" ForeColor="#DD5555" />
                                <SelectedNodeStyle Font-Underline="True" ForeColor="#DD5555" HorizontalPadding="0px" VerticalPadding="0px" />
                                <NodeStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" HorizontalPadding="0px" NodeSpacing="0px" VerticalPadding="0px" />
                            </asp:TreeView>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnEliminar" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>

                <div class="col">
                    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>

                            <div class="card shadow mb-4">
                                <div class="card-header py-6 bg-dark">
                                    <h5 class="m-0 font-weight-bold text-white">Menú</h5>
                                </div>

                                <div class="card-body">
                                    <div class="row">

                                        <div class="col-md-3" style="height: 600px; overflow-y: scroll;">
                                            <ul data-widget="treeview">
                                                <asp:Literal ID="ltTreeview" runat="server"></asp:Literal>
                                            </ul>
                                        </div>

                                        <div class="col-6 mx-2 p-2">

                                            <div class="row mb-3">

                                                <div class="col">
                                                    <label class="form-label">Código del Menu</label>
                                                    <asp:TextBox ID="txtCodigo" runat="server" CssClass="form-control" Enabled="false" ReadOnly="true"></asp:TextBox>
                                                </div>

                                                <div class="col">
                                                    <label class="form-label">Nodo Padre</label>
                                                    <asp:DropDownList ID="ddlNodoPadre" runat="server" CssClass="form-select"></asp:DropDownList>
                                                    <ajaxToolkit:CascadingDropDown
                                                        ID="ccddNodoPadre"
                                                        runat="server"
                                                        TargetControlID="ddlNodoPadre"
                                                        ServicePath="../services/datalist.asmx"
                                                        ServiceMethod="GetMenuLista"
                                                        Category="CategoryName"
                                                        PromptText="Seleccione nodo Padre..."></ajaxToolkit:CascadingDropDown>
                                                </div>

                                                <div class="col">
                                                    <label class="form-label">Posición</label>
                                                    <asp:TextBox ID="txtPosicion" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <ajaxToolkit:FilteredTextBoxExtender
                                                        ID="FTBEPosicion"
                                                        runat="server"
                                                        FilterType="Numbers"
                                                        TargetControlID="txtPosicion"></ajaxToolkit:FilteredTextBoxExtender>
                                                </div>
                                            </div>

                                            <div class="row mb-3">
                                                <div class="col">
                                                    <label class="form-label">Ruta</label>
                                                    <asp:TextBox ID="txtRutaPage" runat="server" CssClass="form-control"></asp:TextBox>
                                                </div>

                                                <div class="col">
                                                    <label class="form-label">Etiqueta</label>
                                                    <asp:TextBox ID="txtEtiqueta" runat="server" CssClass="form-control"></asp:TextBox>

                                                </div>

                                                <div class="col">
                                                    <label class="form-label">Nombre de la Página</label>
                                                    <asp:TextBox ID="txtNombrePagina" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>

                                                </div>
                                            </div>

                                            <div class="row">

                                                <div class="col">
                                                    <div class="form-group">
                                                        <div class="custom-control custom-switch">
                                                            <asp:CheckBox ID="chkOcultarMenu" runat="server" CssClass="custom-control-input" />
                                                            <label class="custom-control-label" for="chkOcultarMenu">Ocultar en el menú</label>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="col">
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtIconos" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <a href="https://fontawesome.com/icons?d=gallery&m=free" target="_blank">
                                                            <span class="input-group-text">Buscar &nbsp;<i class="fas fa-search"></i></span></a>

                                                        <%--     <label class="nota"><strong><i class="fas fa-exclamation-triangle"></i>Advertencia:</strong> Buscar un icono apropiado para el menú, luego copiar y pegar únicamente el nombre de la clase.</label>--%>
                                                    </div>
                                                </div>
                                            </div>

                                            <%--  <label class="nota" style="margin-bottom: 11px;"><strong><i class="fas fa-exclamation-triangle"></i>Advertencia:</strong> Por favor crear rutas relativas utilizando el carácter: <span style="font-size: 18pt;">~</span></label>--%>
                                        </div>
                                    </div>

                                </div>
                            </div>




                            </div>
                            </div>

                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnEliminar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="trvMenu" EventName="SelectedNodeChanged" />
                        </Triggers>
                    </asp:UpdatePanel>

                    <div class="form-btn">
                        <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClas="btn" />
                        <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" CssClas="btn" />
                        <a class="btn" href="crear-menu.aspx">Nuevo</a>
                    </div>
                </div>
            </div>

            <input id="btnView" type="button" class="hide" />

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

<asp:Content ID="c5" ContentPlaceHolderID="cpScripts" runat="Server">

    <script type="text/javascript">
        function ObtenerPagina() {
            var page = $get('CP1_txtRutaPage').value;
            page = page.substring(page.lastIndexOf('/') + 1);
            $get('CP1_txtNombrePagina').value = page;
        }
    </script>
</asp:Content>
