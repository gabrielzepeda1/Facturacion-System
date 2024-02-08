<%@ Page Title="Facturacion" Language="VB" EnableEventValidation="false" MasterPageFile="~/master/prinFactura.master" AutoEventWireup="false" Inherits="WebAppVB.movimientos_Factura" Codebehind="Factura.aspx.vb" %>

<%@ Register Src="~/usercontrol/menu_catalogos.ascx" TagPrefix="uc1" TagName="menu_catalogos" %>



<asp:Content ID="c1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="./css/Facturacion.css" />
</asp:Content>

<asp:Content ID="c2" ContentPlaceHolderID="cpTitulo" runat="Server">
    <h4>Facturación</h4>
</asp:Content>

<asp:Content ID="c3" ContentPlaceHolderID="cpUbicacion" runat="Server">
    <a href="../Default.aspx">Menu</a>
    <label>&gt;</label>
    <a href="Factura.aspx">Facturación</a>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContentPlaceHolder" runat="Server">


    <asp:Panel ID="pnlContent" runat="server" DefaultButton="btnGuardar">


        <asp:UpdatePanel ID="upMensaje_popup" runat="server" UpdateMode="Conditional">

            <ContentTemplate>
                <asp:Literal ID="ltMensaje_popup" runat="server"></asp:Literal>
                <asp:Literal ID="ltMensaje" runat="server"></asp:Literal>
            </ContentTemplate>

            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="BtnAdicionar" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </asp:Panel>

    <div id="main-form">
        <div id="main-form-content">

            <div class="fact-1">

                <div class="table">

                    <div class="row">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>

                                <div class="cel">
                                    <label>Factura</label>
                                    <asp:TextBox ID="txtNoFac" runat="server" ReadOnly="true" CssClass="numero"></asp:TextBox>
                                </div>

                                <div class="cel">
                                    <asp:RadioButton ID="rdbCredito" CssClass="grupos" GroupName="CondicionPago" Text="Crèdito" runat="server" AutoPostBack="true" />
                                    <asp:RadioButton ID="rdbContado" CssClass="grupos" GroupName="CondicionPago" Text="Contado" runat="server" AutoPostBack="true" Checked="true" />
                                </div>

                                <div class="cel">
                                    <asp:RadioButton ID="rdbExterno" CssClass="grupos" GroupName="TipoCliente" runat="server" Text="Externo" AutoPostBack="true" Checked="true" />
                                    <asp:RadioButton ID="rdbInterno" CssClass="grupos" GroupName="TipoCliente" runat="server" Text="Interno" AutoPostBack="true" />

                                </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <%-- <div class="fleft" style="padding-top: 22px;">
                                         <asp:Button ID="BtnBorrarTodos" runat="server" Text="Eliminar. Productos" Class="btnEditar" />
                                     </div>--%>
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <div class="cel">
                                    <label>Nombre del Cliente</label>
                                    <asp:DropDownList ID="ddlCliente" runat="server" CssClass="SelectStyle SelectSearch" />
                                </div>
                                <div class="cel">
                                    <label>Cliente Eventual</label>
                                    <asp:TextBox ID="TextNomClien" runat="server" MaxLength="50" onchange="ClienEventLosFocu(this);"></asp:TextBox>
                                </div>
                                <div class="PantaDosElemDerec">
                                </div>
                                <div class="cel">
                                    <label>Vendedor</label>
                                    <asp:DropDownList ID="ddVendedor" runat="server" CssClass="SelectStyle SelectSearch" />
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <!-- ROW -->

                    <div class="row">
                        <div class="cel-top">
                            <label>ID</label>
                            <asp:TextBox ID="TextId" runat="server" MaxLength="100"></asp:TextBox>
                        </div>
                        <div class="cel">
                        </div>
                    </div>
                    <!-- ROW -->

                </div>
                <!-- TABLE -->

                <hr />

                <div class="table">

                    <div class="row">
                        <div class="cel w40por">
                            <label>Nombre del Producto</label>
                            <asp:DropDownList ID="ddlProducto" runat="server" CssClass="SelectStyle SelectSearch" AutoPostBack="true" />
                        </div>

                        <div class="cel">
                            <asp:UpdatePanel ID="upDatosFactura" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <%--  <asp:Literal ID="LMensajeInv" runat="server"></asp:Literal>--%>
                                    <div class="fleft">
                                        <label>Cantidad</label>
                                        <asp:Panel ID="PanelCantidad" runat="server" DefaultButton="btnVeriInvent">
                                            <%-- <asp:TextBox ID="TextCantidad" runat="server" CssClass="w80px" Text="0.00"  onblur="formatNumber(this, 2); control_fill(this);" onkeypress="formatNumber(this, 2)" onfocus="control_clear(this)"></asp:TextBox>--%>
                                            <asp:TextBox ID="TextCantidad" runat="server" CssClass="w80px" Text="0.00" onkeypress="formatNumber(this, 2)"></asp:TextBox>
                                            <asp:Button ID="btnVeriInvent" runat="server" Text="" class="hide" />
                                        </asp:Panel>
                                    </div>

                                    <div class="fleft">
                                        <label>Bultos</label>
                                        <asp:TextBox ID="TextBultos" runat="server" CssClass="w80px" Text="0.00" onkeypress="formatNumber(this, 2)"></asp:TextBox>
                                    </div>

                                    <div class="fleft">
                                        <label>Precio</label>
                                        <asp:TextBox ID="TextPrecio" runat="server" CssClass="w80px" AutoPostBack="true"></asp:TextBox>
                                    </div>

                                    <div class="fleft">
                                        <label>Total</label>
                                        <asp:TextBox ID="TextTotal" runat="server" CssClass="w80px" AutoPostBack="true"></asp:TextBox>
                                    </div>

                                    <div class="fleft" style="padding-top: 22px;">
                                        <asp:Button ID="BtnAdicionar" runat="server" Text="Adicionar Producto" class="btnEditar" />
                                    </div>

                                    <%-- <div class="fleft" style="padding-top: 22px;">
                                         <asp:Button ID="BtnBorrarTodos" runat="server" Text="Eliminar. Productos" Class="btnEditar" />
                                     </div>--%>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="TextCantidad" EventName="TextChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="BtnAdicionar" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="ddlProducto" EventName="SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <!-- ROW -->

                </div>
                <!-- TABLE -->

                <asp:UpdatePanel ID="upGrid" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Literal ID="ltMensajeGrid" runat="server"></asp:Literal>

                        <div style="min-height: 200px; overflow: scroll;">
                            <%--<asp:GridView ShowHeaderWhenEmpty="True"--%>
                            <asp:GridView
                                ID="GridViewOne"
                                runat="server"
                                CssClass="table table-light table-sm table-striped table-hover table-bordered"
                                CellPadding="0"
                                GridLines="none"
                                AllowPaging="True"
                                AllowSorting="True"
                                PageSize="5"
                                AutoGenerateColumns="False"
                                DataKeyNames="cod_pais,cod_empresa,cod_puesto,no_factura,fecha,cod_producto"
                                BorderColor="#993300">

                                <SelectedRowStyle CssClass="SelectedRowDelete" />

                                <Columns>
                                    <asp:BoundField HeaderText="cod_producto" DataField="cod_producto" SortExpression="cod_producto" Visible="False" />
                                    <asp:BoundField DataField="Producto" HeaderText="Producto" SortExpression="Producto" />
                                    <asp:BoundField DataField="cantidad" HeaderText="Cantidad" SortExpression="cantidad" />
                                    <asp:BoundField DataField="bultos" HeaderText="Bultos" SortExpression="bultos" />
                                    <asp:BoundField DataField="precio" HeaderText="Precio" SortExpression="precio" />
                                    <asp:BoundField DataField="subtotal" HeaderText="Subtotal" SortExpression="subtotal" />
                                    <asp:BoundField DataField="Descuento" HeaderText="Descuento" SortExpression="Descuento" />
                                    <asp:BoundField DataField="IVA" HeaderText="IVA" SortExpression="IVA" />

                                    <asp:CommandField
                                        HeaderText="Suprimir"
                                        ButtonType="Button"
                                        SelectText="Eliminar"
                                        ShowDeleteButton="false"
                                        ShowSelectButton="true"
                                        HeaderStyle-Width="120">
                                        <ControlStyle CssClass="btnEliminar" />
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
                            <asp:HiddenField ID="hdfCodigo" runat="server" />
                            <%--<asp:SqlDataSource ID="FacturaTmp" runat="server" ConnectionString="<%$ ConnectionStrings:FacturacionConnectionString %>" SelectCommand="SELECT [no_factura] as NoFactura, [cod_producto] as Codigo, [desc_imprimir] as Producto, [cantidad] as Cantidad, [bultos] as Bultos, [precio_unidad] as Precio, [sub_total] as SubTotal, [valor_descuento] as Descuento, [valor_iva] as Iva FROM [TmpDetFact]"></asp:SqlDataSource>--%>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <%--<asp:AsyncPostBackTrigger ControlID="BtnGuardar" EventName="Click" />--%>
                        <asp:AsyncPostBackTrigger ControlID="BtnAdicionar" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="GridViewOne" EventName="RowDeleting" />
                        <asp:AsyncPostBackTrigger ControlID="GridViewOne" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>


                <asp:UpdatePanel ID="upPiePagina" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>

                        <div class="factura-form-totales w130px">
                            <label>Total Libras</label>
                            <asp:TextBox ID="TextLibras" runat="server" Text="0.00" onkeypress="formatNumber(this, 2)" Enabled="False"></asp:TextBox>
                        </div>

                        <div class="factura-form-totales w130px">
                            <label>Subtotal</label>
                            <asp:TextBox ID="TextSubtotal" runat="server" Text="0.00" onkeypress="formatNumber(this, 2)" Enabled="False"></asp:TextBox>
                        </div>

                        <div class="factura-form-totales w130px">
                            <label>Porcentaje Desto.</label>
                            <asp:TextBox ID="TextPorDesc" runat="server"></asp:TextBox>
                        </div>

                        <div class="factura-form-totales w130px">
                            <label>Valor Desto.</label>
                            <asp:TextBox ID="TextValorDescu" runat="server" Enabled="false" Text="0.00" onkeypress="formatNumber(this, 2)"></asp:TextBox>
                        </div>

                        <div class="factura-form-totales w130px">
                            <label>.IVA</label>
                            <asp:TextBox ID="TextIVA" runat="server" Text="0.00" onkeypress="formatNumber(this, 2)" Enabled="False"></asp:TextBox>
                        </div>

                        <div class="factura-form-totales w130px">
                            <label>Neto C$</label>
                            <asp:TextBox ID="TextNeto" runat="server" Text="0.00" onkeypress="formatNumber(this, 2)" Enabled="False"></asp:TextBox>
                            <br />
                            <label>
                                Neto&nbsp;&nbsp;&nbsp; $</label>
                            <asp:TextBox ID="TextNetoDol" runat="server" Enabled="False" onkeypress="formatNumber(this, 2)" Text="0.00"></asp:TextBox>
                        </div>

                        <div class="factura-form-btn w160px">
                            <asp:Button ID="BtnGuardar" runat="server" Text="Imprimir/Guardar" Class="btnEditar" />
                        </div>

                        <div class="clear"></div>
                        <hr />




                        <h3>Dinero Entregado</h3>

                        <div class="factura-dinero w130px">
                            <label>Valor</label>
                            <asp:TextBox ID="TextDinero" runat="server" Text="0.00" onkeypress="formatNumber(this, 2)"></asp:TextBox>
                        </div>

                        <div class="factura-dinero w130px">
                            <label>Vuelto</label>
                            <asp:TextBox ID="TextVuelto" runat="server" Enabled="false" Text="0.00" onkeypress="formatNumber(this, 2)"></asp:TextBox>
                        </div>

                        <div class="factura-dinero w130px">
                            <label>Dolares</label>
                            <asp:TextBox ID="TextDolares" runat="server" Text="0.00" onkeypress="formatNumber(this, 2)"></asp:TextBox>
                        </div>

                        <div class="factura-dinero w130px">
                            <label>Paridad</label>
                            <asp:TextBox ID="TextTipoCambio" runat="server" Enabled="false"></asp:TextBox>
                        </div>

                        <div class="factura-dinero w130px">
                            <label>Cordobas</label>
                            <asp:TextBox ID="TextCordoba" runat="server" Enabled="false" Text="0.00" onkeypress="formatNumber(this, 2)"></asp:TextBox>
                        </div>

                        <div class="clear"></div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="BtnAdicionar" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="GridViewOne" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <!-- END:factura-form -->

        </div>
        <!-- END: #main-form-content -->
    </div>
    <!-- END: #main-form -->

    <%--<ajaxToolkit:CascadingDropDown 
                                ID="CMoneda" 
                                runat="server" 
                                TargetControlID  ="ddMoneda" 
                                ServicePath ="../services/WSCatProductos.asmx"
                                ServiceMethod="GetMoneda" 
                                Category="CategoryName">
                                </ajaxToolkit:CascadingDropDown>--%>
    <div id="popup-form" class="white-popup-block mfp-hide popupFactura">

        <div class="PantallaPopUp">
            <h2 class="center-color">Datos complementarios de facturas</h2>

            <i class="fas fa-times-circle Close"></i>

            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="center-color">
                        <h2>
                            <asp:Label ID="LblResp" runat="server" AutoPostBack="true"></asp:Label></h2>
                    </div>

                    <div class="table">
                        <div class="row">
                            <div class="cel">
                                <label>Saldo Factura</label>
                                <asp:TextBox ID="TextSaldoFact" runat="server" Enabled="false" Text="0.00" onkeypress="formatNumber(this, 2)"></asp:TextBox>
                            </div>

                            <div class="cel">
                                <label id="lblFomarPa" runat="server">Forma de Pago</label>
                                <asp:DropDownList ID="ddformaPago" runat="server" CssClass="SelectStyle SelectSearch" AutoPostBack="true" />
                                <%-- <ajaxToolkit:CascadingDropDown 
                                ID="CFormaPago" 
                                runat="server" 
                                TargetControlID  ="ddformaPago" 
                                ServicePath ="../services/WSCatProductos.asmx"
                                ServiceMethod="GetFormaPago" 
                                Category="CategoryName">
                                </ajaxToolkit:CascadingDropDown>--%>
                            </div>
                            <div class="cel">
                                <label>Datos de Forma de pago</label>
                                <asp:TextBox ID="txtDatos" runat="server" MaxLength="100"></asp:TextBox>
                            </div>

                            <div class="cel">
                                <label id="lblBanco" runat="server">Banco</label>
                                <asp:DropDownList ID="ddbanco" runat="server" CssClass="SelectStyle SelectSearch" />
                                <ajaxToolkit:CascadingDropDown
                                    ID="Cbanco"
                                    runat="server"
                                    TargetControlID="ddbanco"
                                    ServicePath="../services/WSCatProductos.asmx"
                                    ServiceMethod="GetBanco"
                                    Category="CategoryName"></ajaxToolkit:CascadingDropDown>
                            </div>
                        </div>
                        <!-- END ROW -->

                        <div class="row">
                            <div class="cel">
                                <label id="lblTarjeta" runat="server">Tarjeta</label>
                                <asp:DropDownList ID="ddTarjeta" runat="server" CssClass="SelectStyle SelectSearch" />
                                <ajaxToolkit:CascadingDropDown
                                    ID="Ctarjeta"
                                    runat="server"
                                    TargetControlID="ddTarjeta"
                                    ServicePath="../services/WSCatProductos.asmx"
                                    ServiceMethod="GetTarjeta"
                                    Category="CategoryName"></ajaxToolkit:CascadingDropDown>
                            </div>

                            <div class="cel">
                                <label>Moneda</label>
                                <asp:DropDownList ID="ddMoneda" runat="server" CssClass="SelectStyle SelectSearch" AutoPostBack="true" />
                                <%--<ajaxToolkit:CascadingDropDown 
                                ID="CMoneda" 
                                runat="server" 
                                TargetControlID  ="ddMoneda" 
                                ServicePath ="../services/WSCatProductos.asmx"
                                ServiceMethod="GetMoneda" 
                                Category="CategoryName">
                                </ajaxToolkit:CascadingDropDown>--%>
                            </div>

                            <div class="cel">
                                <label>Valor Recibido C$</label>
                                <asp:TextBox ID="TextValCor" runat="server" MaxLength="100" Text="0.00" onkeypress="formatNumber(this, 2)"></asp:TextBox>
                                <label id="LblDolar" runat="server">
                                    <br />
                                    Valor Recibido $</label>
                                <asp:TextBox ID="TextValDol" runat="server" Enabled="False" MaxLength="100" onkeypress="formatNumber(this, 2)" Text="0.00" Visible="False" AutoPostBack="true"></asp:TextBox>
                            </div>
                        </div>
                        <!-- END ROW -->

                        <div class="row">
                            <div class="cel">
                                <label id="lblConverDolACor" runat="server">Conversión de $ a C$</label>
                                <asp:TextBox ID="TextCambioCor" runat="server" MaxLength="100" Text="0.00" onkeypress="formatNumber(this, 2)" Visible="False"></asp:TextBox>
                            </div>

                            <%-- <div class="cel">
                                <label>Vendedor</label>
                                <asp:DropDownList ID="ddVendedor" runat="server" CssClass="SelectStyle SelectSearch"   />
                                <ajaxToolkit:CascadingDropDown 
                                ID="Cvendedor" 
                                runat="server" 
                                TargetControlID  ="ddVendedor" 
                                ServicePath ="../services/WSCatProductos.asmx"
                                ServiceMethod="GetVendedor" 
                                Category="CategoryName">
                                </ajaxToolkit:CascadingDropDown>
                            </div>--%>

                            <div class="cel">
                            </div>

                            <div class="cel">
                            </div>
                        </div>
                        <!-- END ROW -->
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="BtnGuardar" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="ddformaPago" EventName="SelectedIndexChanged" />
                    <%--<asp:AsyncPostBackTrigger ControlID="BtnAdiciFormaPago" EventName="Click" />--%>
                    <asp:AsyncPostBackTrigger ControlID="ddMoneda" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="TextValDol" EventName="TextChanged" />
                    <asp:AsyncPostBackTrigger ControlID="TextSaldoFact" EventName="TextChanged" />
                    <asp:AsyncPostBackTrigger ControlID="GridViewpop" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="BtnAdiciFormaPago" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>

            <div class="btnGuarda">
                <asp:Button ID="BtnAdiciFormaPago" runat="server" Text="Aceptar" class="btnEditar" />
                <%--<!-- popup-scroll -->--%>
            </div>

            <%--'''''''''FIN DE PANTALLA DOS '''''''''''''''''''''''''''''''''''''''''''--%>
            <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Literal ID="LiteralGridpop" runat="server"></asp:Literal>

                    <div style="min-height: 200px; overflow: scroll;">
                        <asp:GridView ShowHeaderWhenEmpty="True"
                            ID="GridViewpop"
                            runat="server"
                            CssClass="table table-light table-sm table-striped table-hover table-bordered"
                            CellPadding="0"
                            GridLines="None"
                            AllowPaging="True"
                            AllowSorting="True"
                            PageSize="5"
                            AutoGenerateColumns="false"
                            DataKeyNames="cod_FormaPago,No_Factura"
                            BorderColor="#993300">


                            <SelectedRowStyle CssClass="SelectedRowDelete" />

                            <Columns>
                                <asp:BoundField DataField="FormaPago" HeaderText="FormaPago" SortExpression="FormaPago" />
                                <asp:BoundField DataField="Moneda" HeaderText="Moneda" SortExpression="Moneda" />
                                <asp:BoundField DataField="ValorRecibidoCor" HeaderText="Cordobas" SortExpression="ValorRecibidoCor" />
                                <asp:BoundField DataField="ValorRecibidoDol" HeaderText="Dolares" SortExpression="ValorRecibidoDol" />

                                <asp:CommandField
                                    HeaderText="Suprimir"
                                    ButtonType="Button"
                                    SelectText="Eliminar"
                                    ShowDeleteButton="false"
                                    ShowSelectButton="true"
                                    HeaderStyle-Width="120">
                                    <ControlStyle CssClass="btnEliminar" />
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
                        <asp:HiddenField ID="HiddenField2" runat="server" />
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="GridViewpop" EventName="RowDeleting" />
                    <asp:AsyncPostBackTrigger ControlID="GridViewpop" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="BtnAdiciFormaPago" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="BtnGuardar" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="ddMoneda" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="TextValDol" EventName="TextChanged" />
                    <asp:AsyncPostBackTrigger ControlID="TextSaldoFact" EventName="TextChanged" />
                </Triggers>
            </asp:UpdatePanel>
            <div class="btnGuarda">
                <asp:Button ID="BtnPopupImpGuar" runat="server" Text="Imprimir/Guardar" class="btnEditar" Width="189px" />
                <asp:Button ID="BtnRegresa" runat="server" Text="Regresar" Class="btnEliminar" />
            </div>

            <%--'''''''''FIN DE PANTALLA DOS '''''''''''''''''''''''''''''''''''''''''''--%>
        </div>
        <%--'''''''''FIN DE PANTALLA DOS '''''''''''''''''''''''''''''''''''''''''''--%>
    </div>
    <!-- #popup-form -->

    <%--'''''''''FIN DE PANTALLA DOS '''''''''''''''''''''''''''''''''''''''''''--%>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="cpScripts" runat="Server">
    <script>

        $(document).ready(function () {
            //$('.datagird').basictable();

            $("#BtnGuardar").click(function () {




                //if ($("input[rdbContado']").is(':checked')) {



                $("#<%=ddformaPago.ClientID%>").val("E");
                  $("#<%=ddformaPago.ClientID%>").prev().removeClass('visible');


                  $("#<%=ddMoneda.ClientID%>").val("1");
                  $("#<%=ddMoneda.ClientID%>").prev().removeClass('visible');

                  $("#<%=ddVendedor.ClientID%>").val("4");
                  $("#<%=ddVendedor.ClientID%>").prev().removeClass('visible');

                  $("#<%=txtDatos.ClientID%>").val("");
                  $("#<%=txtDatos.ClientID%>").prev().removeClass('visible');

                  $("#<%=ddbanco.ClientID%>").val("");
                  $("#<%=ddbanco.ClientID%>").prev().removeClass('visible');

                  $("#<%=ddTarjeta.ClientID%>").val("");
                  $("#<%=ddTarjeta.ClientID%>").prev().removeClass('visible');

                  $("#<%=GridViewpop.ClientID%>").val("");
                  $("#<%=GridViewpop.ClientID%>").prev().removeClass('visible');


                  //}

                  $("#popuptittle").text('Distribucion de pago');
                  open_popup();

              });

             $(".Close").click(function () {
                 $('#popup-form').bPopup().close();
             });
         });


        $("#<%=ddformaPago.ClientID%>").change(function () {
            if ($(this).val() == "E") {
                $("#ddbanco").prop("disabled", false);
                $("#ddTarjeta").prop("disabled", false);
            } else {
                $("#ddbanco").prop("disabled", true);
                $("ddTarjeta").prop("disabled", true);
            }
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

        function responsive_grid() {

            //$('.datagird').basictable();
        }

        function success_messege(msg) {

            alertify.success(msg);
            $("#<%=txtDatos.ClientID%>").val("");

        }



    </script>

</asp:Content>
