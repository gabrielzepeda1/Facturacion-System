<%@ Page Title="Catalogo de Puestos | Facturación" Language="VB" EnableEventValidation="false" MasterPageFile="~/master/principal.master" AutoEventWireup="false" CodeFile="Puestos.aspx.vb" Inherits="catalogos_Puestos" %>

<%@ Register Src="~/usercontrol/menu_catalogos.ascx" TagPrefix="uc1" TagName="menu_catalogos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cpTitulo" runat="Server">
    <h1>Catálogo de Puestos</h1>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cpUbicacion" runat="Server">
    <a href="../Default.aspx">Escritorio</a>
    <label>&gt;</label>
    <a href="puestos.aspx">Catálogo de Puestos</a>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContentPlaceHolder" runat="Server">
    <%--<asp:ScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ScriptManager>--%>

    <div id="main-form">
        <div id="main-form-content">


            <div id="main-form-content-field">

                <div id="Control">
                    <div class="container-fluid">
                        <div class="row mx-1 py-2 align-items-center justify-content-between">
                            <div class="col-8 d-flex">

                                <asp:LinkButton ID="btnNew" runat="server" CssClass="btn btn-primary btn-lg" ToolTip="Nuevo" OnClientClick="open_popup()"><i class="fas fa-plus-circle"></i> Nuevo </asp:LinkButton>

                                <asp:LinkButton ID="btnExportar" runat="server" CssClass="btn btn-success btn-lg" ToolTip="Exportar"><i class="fas fa-file-excel"></i> Exportar a Excel </asp:LinkButton>

                            </div>

                            <div class="col-4 d-flex">
                                <asp:Panel ID="PnlSearch" runat="server" CssClass="input-group" DefaultButton="">
                                    <asp:TextBox ID="txtBuscar" CssClass="form-control" ToolTip="Buscar..." runat="server"></asp:TextBox>
                                    <div class="input-group-append">
                                        <asp:LinkButton ID="btnBuscar" CssClass="btn btn-secondary" runat="server">Buscar&nbsp<i class="fas fa-search"></i></asp:LinkButton>

                                    </div>
                                </asp:Panel>

                            </div>
                        </div>
                    </div>
                </div>

                <asp:UpdatePanel ID="upMensaje" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Literal ID="ltMensaje" runat="server"></asp:Literal>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnNew" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>

                <asp:UpdatePanel ID="upGrid" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>

                        <div class="table-content">


                            <div class="clear"></div>

                            <asp:Literal ID="ltMensajeGrid" runat="server"></asp:Literal>
                            <%--<div style ="height:500px; width:1000px; overflow:auto;">--%>
                            <div class="table-responsive" style="overflow: auto;">
                                <asp:GridView
                                    ID="GridViewOne"
                                    runat="server"
                                    CssClass="table table-light table-sm table-striped table-hover table-bordered"
                                    CellPadding="0"
                                    GridLines="None"
                                    AllowPaging="True"
                                    AllowSorting="True"
                                    PageSize="10"
                                    DataKeyNames="codigo"
                                    AutoGenerateColumns="False">

                                    <HeaderStyle CssClass="table-header table-dark align-middle text-center" />

                                    <Columns>
                                        <asp:BoundField HeaderText="Codigo" DataField="codigo" SortExpression="codigo" ReadOnly="true" />
                                        <asp:BoundField HeaderText="Puesto" DataField="Puesto" SortExpression="Puesto" />

                                        <asp:BoundField HeaderText="Descripcion Corta" HeaderStyle-Wrap="true" DataField="DesCorta" SortExpression="DesCorta" />

                                        <%-- <asp:TemplateField HeaderText="Verificar Inventario">
                                           <EditItemTemplate>
                                                 <asp:CheckBox ID="CheckVeInv" runat="server"  AutoPostBack="True" >
                                                 </asp:CheckBox>
                                           </EditItemTemplate>
                                           <ItemTemplate>
                                                     <asp:Label ID="Labelact" runat="server" Text='<%# Bind("VerifInventario")%>'></asp:Label>
                                           </ItemTemplate>
                                      </asp:TemplateField>--%>

                                        <asp:TemplateField HeaderText="Pais">
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlPais" runat="server" AutoPostBack="True">
                                                </asp:DropDownList>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("Pais")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Empresa">
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlEmpresa" runat="server" AutoPostBack="True">
                                                </asp:DropDownList>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="LabelEmpresa" runat="server" Text='<%# Bind("Empresa")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                        <asp:CommandField
                                            HeaderText="Editar"
                                            ButtonType="Button"
                                            SelectText="Actualizar"
                                            ShowSelectButton="true"
                                            HeaderStyle-Width="120">
                                            <ControlStyle CssClass="btn btn-success align-middle" />
                                        </asp:CommandField>

                                        <asp:CommandField
                                            HeaderText="Suprimir"
                                            ButtonType="Button"
                                            DeleteText="Eliminar"
                                            ShowDeleteButton="true"
                                            HeaderStyle-Width="120">
                                            <ControlStyle CssClass="btn btn-danger align-middle" />
                                        </asp:CommandField>
                                    </Columns>


                                    <PagerTemplate>
                                        <div class="pagination">
                                            <asp:Button ID="B1" runat="server" CommandName="Page" ToolTip="Prim. Pag" CommandArgument="First" CssClass="primero" Text="Primera" formnovalidate />
                                            <asp:Button ID="B2" runat="server" CommandName="Page" ToolTip="Pág. anterior" CommandArgument="Prev" CssClass="anterior" Text="&larr;" formnovalidate />
                                            <asp:Button ID="B3" runat="server" CommandName="Page" ToolTip="Sig. página" CommandArgument="Next" CssClass="siguiente" Text="&rarr;" formnovalidate />
                                            <asp:Button ID="B4" runat="server" CommandName="Page" ToolTip="Últ. Pag" CommandArgument="Last" CssClass="ultimo" Text="Ultima" formnovalidate />
                                            <asp:Label ID="CurrentPageLabel" runat="server" CssClass="PagerLabel" />
                                        </div>
                                    </PagerTemplate>
                                </asp:GridView>
                            </div>
                        </div>
                        <asp:HiddenField ID="hdfCodigo" runat="server" />

                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="BtnGuardar" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="GridViewOne" EventName="RowDeleting" />
                        <asp:AsyncPostBackTrigger ControlID="GridViewOne" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>

            </div>
            <!-- #main-form-content-field -->

            <div class="clear"></div>

        </div>
        <!-- =========== #main-form-content ====================================================================== -->
    </div>
    <!-- =========== #main-form ====================================================================== -->

    <div id="popup-form" class="white-popup-block mfp-hide">


        <div class="bstt-form PantallaPopUp">
            <i class="fas fa-times-circle Close"></i>

            <h2 class="center-color">Agregar/Editar Puesto</h2>

            <asp:UpdatePanel ID="upDesc" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Literal ID="ltMensajeLoad" runat="server"></asp:Literal>

                    <div class="fleft w48por">
                        <label>Codigo</label>
                        <asp:Panel ID="pnlCodigoCLiente" runat="server" DefaultButton="btnLoad">
                            <asp:TextBox ID="txtCodigo" runat="server" MaxLength="50" Enabled="False" ForeColor="#FFFF66"></asp:TextBox>
                            <asp:Button ID="btnLoad" runat="server" Text="" class="hide" />
                        </asp:Panel>
                    </div>
                    <div class="fright w48por">
                        <label>Descripcion de Puesto</label>
                        <asp:TextBox ID="TextPuesto" runat="server" MaxLength="100"></asp:TextBox>
                    </div>

                    <div class="fleft w48por">
                        <label>Descripcion Corta</label>
                        <asp:TextBox ID="TextDescCorta" runat="server" MaxLength="100"></asp:TextBox>
                    </div>
                    <div class="fright w48por">
                        <label>No. de Nota Debito</label>
                        <asp:TextBox ID="TextNoDebito" runat="server" MaxLength="50"></asp:TextBox>
                    </div>

                    <div class="fleft w48por">
                        <label>No. de Nota de credito</label>
                        <asp:TextBox ID="TextNoCredito" runat="server" MaxLength="50"></asp:TextBox>
                    </div>
                    <div class="fright w48por">
                        <label>No. de Recibo</label>
                        <asp:TextBox ID="TextNoRecibo" runat="server" MaxLength="50"></asp:TextBox>
                    </div>

                    <div class="fleft w48por">
                        <label>No. de Factura</label>
                        <asp:TextBox ID="TextFactura" runat="server" MaxLength="50"></asp:TextBox>
                    </div>
                    <div class="fright w48por">
                        <label>Nota de credito de retencion</label>
                        <asp:TextBox ID="TextNoCredRetenc" runat="server" MaxLength="50"></asp:TextBox>
                    </div>

                    <div class="fleft w48por">
                        <label>Nombre del Formato de Impresion</label>
                        <asp:TextBox ID="TextFormatoImp" runat="server" MaxLength="100"></asp:TextBox>
                    </div>
                    <div class="fright w48por">
                        <label>Telefono</label>
                        <asp:TextBox ID="TextTelefono" runat="server" MaxLength="50"></asp:TextBox>
                    </div>

                    <div class="fleft w48por">
                        <label>País</label>
                        <asp:DropDownList ID="ddlPais" runat="server" CssClass="SelectStyle" />
                        <ajaxToolkit:CascadingDropDown
                            ID="CCPAIS"
                            runat="server"
                            TargetControlID="ddlPais"
                            ServicePath="../services/WSCatProductos.asmx"
                            ServiceMethod="GetPaises"
                            Category="CategoryPais"></ajaxToolkit:CascadingDropDown>
                    </div>
                    <div class="fright w48por">
                        <label>Empresa</label>
                        <asp:DropDownList ID="ddlEmpresa" runat="server" CssClass="SelectStyle" />
                        <ajaxToolkit:CascadingDropDown
                            ID="CCEMPRESA"
                            runat="server"
                            TargetControlID="ddlEmpresa"
                            ServicePath="../services/WSCatProductos.asmx"
                            ServiceMethod="GetEmpresaXPais"
                            Category="CategoryEmpresa"
                            ParentControlID="ddlPais"></ajaxToolkit:CascadingDropDown>
                    </div>

                    <div class="clear"></div>

                    <div class="fleft w48por">
                        <label>No. de Items a Imprimir</label>
                        <asp:TextBox ID="TextLineImpri" runat="server" MaxLength="50"></asp:TextBox>
                    </div>
                    <div class="fright w48por">
                        <label title="No. de Cuotas a deducir en Planillas por Facturas">Cuotas a deducir en Planillas</label>
                        <asp:TextBox ID="TextCuotaPla" runat="server" MaxLength="50" ToolTip="No. de Cuotas a deducir en Planillas por Facturas"></asp:TextBox>
                    </div>

                    <div class="fleft w48por">
                        <div class="check">
                            <asp:CheckBox ID="CheckVeInv" runat="server" Checked="false" Text="Verificar Inventario"></asp:CheckBox>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="GridViewOne" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="btnNew" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>

            <div class="clear"></div>

            <div class="btnGuarda">
                <asp:Button ID="BtnGuardar" runat="server" Text="Guardar" class="btnEditar" />

                <asp:Button ID="BtnCerrar" runat="server" Text="Cerrar" CssClass="btnEliminar" OnClientClick="close_popup()" />

            </div>
        </div>
        <!-- .bstt-form -->
    </div>
    <!-- #popup-form -->

    <asp:UpdateProgress ID="uprData" runat="server">
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

<asp:Content ID="Content5" ContentPlaceHolderID="cpScripts" runat="Server">
    <script>
        $(document).ready(function () {
            //$('.datagird').basictable();

            $("#btnNew").click(function () {

                var btn = document.getElementById("btnNew");
                btn.click();

                $("#<%=hdfCodigo.ClientID%>").val("");

                $("#<%=TextPuesto.ClientID%>").val("");
                $("#<%=TextPuesto.ClientID%>").prev().removeClass('visible');

                $("#<%=TextNoDebito.ClientID%>").val("");
                $("#<%=TextNoDebito.ClientID%>").prev().removeClass('visible');

                $("#<%=TextNoCredito.ClientID%>").val("");
                $("#<%=TextNoCredito.ClientID%>").prev().removeClass('visible');

                $("#<%=TextNoRecibo.ClientID%>").val("");
                $("#<%=TextNoRecibo.ClientID%>").prev().removeClass('visible');

                $("#<%=TextFactura.ClientID%>").val("");
                $("#<%=TextFactura.ClientID%>").prev().removeClass('visible');

                $("#<%=TextNoCredRetenc.ClientID%>").val("");
                $("#<%=TextNoCredRetenc.ClientID%>").prev().removeClass('visible');

                $("#<%=TextFormatoImp.ClientID%>").val("");
                $("#<%=TextFormatoImp.ClientID%>").prev().removeClass('visible');

                $("#<%=TextLineImpri.ClientID%>").val("");
                $("#<%=TextLineImpri.ClientID%>").prev().removeClass('visible');

                $("#<%=TextCuotaPla.ClientID%>").val("");
                $("#<%=TextCuotaPla.ClientID%>").prev().removeClass('visible');

                $("#<%=CheckVeInv.ClientID%>").val("");
                $("#<%=CheckVeInv.ClientID%>").prev().removeClass('visible');


                $("#<%=TextTelefono.ClientID%>").val("");
                $("#<%=TextTelefono.ClientID%>").prev().removeClass('visible');

                $("#<%=TextDescCorta.ClientID%>").val("");
                $("#<%=TextDescCorta.ClientID%>").prev().removeClass('visible');

                $("#popuptittle").text('Agregar Puesto');

                open_popup();

            });

            $(".Close").click(function () {
                $('#popup-form').bPopup().close();
            });


        });

        function open_popup() {
            $('#popup-form').bPopup({
                appendTo: 'form',
                speed: 650,
                transition: 'slideIn',
                transitionClose: 'slideBack',
                height: '200',
                width: '150',
            });
        }

        function close_popup() {
            $('#popup-form').bPopup().close();
        }

        function responsive_grid() {

            //$('.datagird').basictable();
        }

        function success_messege(msg) {

            alertify.success(msg);

            $("#<%=TextPuesto.ClientID%>").val("");
              $("#<%=TextNoDebito.ClientID%>").val("");
              $("#<%=TextNoCredito.ClientID%>").val("");
              $("#<%=TextNoRecibo.ClientID%>").val("");
              $("#<%=TextNoCredRetenc.ClientID%>").val("");
              $("#<%=TextFormatoImp.ClientID%>").val("");
              $("#<%=TextLineImpri.ClientID%>").val("");
              $("#<%=TextCuotaPla.ClientID%>").val("");
              $("#<%=CheckVeInv.ClientID%>").val("");
              $("#<%=TextTelefono.ClientID%>").val("");
              $("#<%=TextDescCorta.ClientID%>").val("");


        }
    </script>
</asp:Content>

