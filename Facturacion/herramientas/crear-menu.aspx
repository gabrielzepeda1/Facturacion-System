<%@ Page Title="Gestionar Opciones del Menú | Facturación" EnableEventValidation="false" Language="VB" MasterPageFile="~/master/principal.master" AutoEventWireup="false" CodeFile="crear-menu.aspx.vb" Inherits="admin_crear_menu" %>

<%@ Register Src="~/usercontrol/menu_tools.ascx" TagPrefix="uc1" TagName="menu_tools" %>

<asp:Content ID="c1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="c2" ContentPlaceHolderID="cpTitulo" Runat="Server">
    <h1>Gestionar Opciones del Menú</h1>
</asp:Content>

<asp:Content ID="c3" ContentPlaceHolderID="cpUbicacion" Runat="Server">
    <a href="../Default.aspx">Escritorio</a>
    <label>&gt;</label>
    <a href="crear-menu.aspx">Gestionar Opciones del Menú</a>
</asp:Content>

<asp:Content ID="c4" ContentPlaceHolderID="MainContentPlaceHolder" Runat="Server">    
    <asp:ScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ScriptManager>

    <div id="main-form">
        <div id="main-form-content">
            
            <uc1:menu_tools runat="server" ID="menu_tools" />

            <div id="main-form-content-field">

                <asp:UpdatePanel ID="upMensaje" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>

                        <asp:Literal ID="ltMensaje" runat="server"></asp:Literal>

                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click"/>
                        <asp:AsyncPostBackTrigger ControlID="btnEliminar" EventName="Click"/>
                        <asp:AsyncPostBackTrigger ControlID="trvMenu" EventName="SelectedNodeChanged"/>
                    </Triggers>
                </asp:UpdatePanel>

                <div class="form-no-popup">
                    <div class="menu-left">
                        <asp:UpdatePanel ID="upMenu" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>

                                <asp:TreeView ID="trvMenu" runat="server" ImageSet="Simple" NodeIndent="10">
                                    <ParentNodeStyle Font-Bold="False" />
                                    <HoverNodeStyle Font-Underline="True" ForeColor="#DD5555" />
                                    <SelectedNodeStyle Font-Underline="True" ForeColor="#DD5555" HorizontalPadding="0px" VerticalPadding="0px" />
                                    <NodeStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" HorizontalPadding="0px" NodeSpacing="0px" VerticalPadding="0px" />
                                </asp:TreeView>
                    
                                <asp:HiddenField ID="hdfCodigo" runat="server" />

                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click"/>
                                <asp:AsyncPostBackTrigger ControlID="btnEliminar" EventName="Click"/>
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>

                    <div class="menu-right">
                        <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtCodigo" runat="server" Enabled="false"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="trvMenu" EventName="SelectedNodeChanged"/>
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click"/>
                                <asp:AsyncPostBackTrigger ControlID="btnEliminar" EventName="Click"/>
                            </Triggers>
                        </asp:UpdatePanel>

                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlNodoPadre" runat="server"></asp:DropDownList>

                                <ajaxToolkit:CascadingDropDown 
                                    ID="ccddNodoPadre" 
                                    runat="server" 
                                    TargetControlID="ddlNodoPadre" 
                                    ServicePath="../services/datalist.asmx"
                                    ServiceMethod="GetMenuLista" 
                                    Category="CategoryName"
                                    PromptText="Seleccione nodo Padre...">
                                </ajaxToolkit:CascadingDropDown>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click"/>
                                <asp:AsyncPostBackTrigger ControlID="btnEliminar" EventName="Click"/>
                                <asp:AsyncPostBackTrigger ControlID="trvMenu" EventName="SelectedNodeChanged"/>
                            </Triggers>
                        </asp:UpdatePanel>
        
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtPosicion" runat="server"></asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender
                                    ID="FTBEPosicion" 
                                    runat="server" 
                                    FilterType="Numbers"
                                    TargetControlID="txtPosicion">
                                </ajaxToolkit:FilteredTextBoxExtender>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click"/>
                                <asp:AsyncPostBackTrigger ControlID="btnEliminar" EventName="Click"/>
                                <asp:AsyncPostBackTrigger ControlID="trvMenu" EventName="SelectedNodeChanged"/>
                            </Triggers>
                        </asp:UpdatePanel>

                        <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtRutaPage" runat="server" CssClass="nomarignb"></asp:TextBox>

                                <label class="nota" style="margin-bottom:11px;"><strong><i class="fas fa-exclamation-triangle"></i> Advertencia:</strong> Por favor crear rutas relativas utilizando el carácter: <span style="font-size:18pt;">~</span></label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click"/>
                                <asp:AsyncPostBackTrigger ControlID="btnEliminar" EventName="Click"/>
                                <asp:AsyncPostBackTrigger ControlID="trvMenu" EventName="SelectedNodeChanged"/>
                            </Triggers>
                        </asp:UpdatePanel>

                        <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtEtiqueta" runat="server"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click"/>
                                <asp:AsyncPostBackTrigger ControlID="btnEliminar" EventName="Click"/>
                                <asp:AsyncPostBackTrigger ControlID="trvMenu" EventName="SelectedNodeChanged"/>
                            </Triggers>
                        </asp:UpdatePanel>

                        <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtNombrePagina" runat="server" Enabled="false"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click"/>
                                <asp:AsyncPostBackTrigger ControlID="btnEliminar" EventName="Click"/>
                                <asp:AsyncPostBackTrigger ControlID="trvMenu" EventName="SelectedNodeChanged"/>
                            </Triggers>
                        </asp:UpdatePanel>

                        <asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="check" style="width:67%; margin-bottom:22px;">
                                    <asp:CheckBox ID="chkOcultarMenu" runat="server" Checked="false" Text="Acceso a la ruta, oculto en el menú"/> 
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click"/>
                                <asp:AsyncPostBackTrigger ControlID="btnEliminar" EventName="Click"/>
                                <asp:AsyncPostBackTrigger ControlID="trvMenu" EventName="SelectedNodeChanged"/>
                            </Triggers>
                        </asp:UpdatePanel>

                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div id="search-articulo" class="search-data">
                                    <asp:TextBox ID="txtIconos" runat="server" CssClass="search nolabel"></asp:TextBox>
                                    <a href="https://fontawesome.com/icons?d=gallery&m=free" target="_blank"><label>Buscar Icono</label> <i class="fas fa-search"></i></a>
                                    <div class="clear"></div>
                                </div>

                                <label class="nota"><strong><i class="fas fa-exclamation-triangle"></i> Advertencia:</strong> Buscar un icono apropiado para el menú, luego copiar y pegar únicamente el nombre de la clase.</label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click"/>
                                <asp:AsyncPostBackTrigger ControlID="btnEliminar" EventName="Click"/>
                                <asp:AsyncPostBackTrigger ControlID="trvMenu" EventName="SelectedNodeChanged"/>
                            </Triggers>
                        </asp:UpdatePanel>

                        <div class="form-btn">
                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClas="btn"/>
                            <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" CssClas="btn"/>
                            <a class="btn" href="crear-menu.aspx">Nuevo</a>
                        </div>
                    </div>
                </div>

                <div class="clear"></div>

                <input id="btnView" type="button" class="hide" />

            </div><!-- #main-form-content-field -->





        </div><!-- =========== #main-form-content ====================================================================== -->
    </div><!-- =========== #main-form ====================================================================== -->

    <asp:UpdateProgress ID="uprGrid" runat="server">
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

<asp:Content ID="c5" ContentPlaceHolderID="cpScripts" Runat="Server">

    <script type="text/javascript">
        function ObtenerPagina() {
            var page = $get('CP1_txtRutaPage').value;
            page = page.substring(page.lastIndexOf('/') + 1);
            $get('CP1_txtNombrePagina').value = page;
        }
    </script>
</asp:Content>
