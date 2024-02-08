<%@ Page Title="" Language="VB" MasterPageFile="~/master/prinFactura.master" AutoEventWireup="false" Inherits="WebAppVB.movimientos_Factura"
    EnableEventValidation="false" Codebehind="Facturacion.aspx.vb" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Src="~/usercontrol/menu_catalogos.ascx" TagPrefix="uc1" TagName="menu_catalogos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cpTitulo" runat="Server">
    <h4>&nbsp;Facturacion</h4>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cpUbicacion" runat="Server">
    <a href="../Default.aspx">Menu</a>
    <label>&gt;</label>
    <a href="Facturacion.aspx">Facturación</a>

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContentPlaceHolder" runat="Server">
    <asp:UpdatePanel ID="upMensaje_popup" runat="server" UpdateMode="Conditional">

        <ContentTemplate>
            <asp:Literal ID="ltMensaje_popup" runat="server"></asp:Literal>
            <asp:Literal ID="ltMensaje" runat="server"></asp:Literal>
        </ContentTemplate>

        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="BtnAdicionar" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>

    <div class="container-fluid">

        <div class="row">

            <div class="card shadow mb-4">
                <div class="card-header py-6 bg-dark">
                    <h5 class="m-0 font-weight-bold text-white">Facturación</h5>
                </div>

                <div class="card-body">
                    <div class="row mb-3">

                        <div class="col">

                            <div>
                                <label>Factura:</label>
                            </div>
                            <div class="radio-container">
                                <asp:RadioButton ID="rdbCredito" CssClass="radio" GroupName="CondicionPago" Text="Crédito" runat="server" AutoPostBack="true" />
                                <asp:RadioButton ID="rdbContado" CssClass="radio" GroupName="CondicionPago" Text="De Contado" runat="server" AutoPostBack="true" />
                            </div>

                            <div>
                                <label>Cliente es:</label>
                            </div>
                            <div class="radio-container">
                                <asp:RadioButton ID="rdbExterno" CssClass="radio" GroupName="TipoDeCliente" runat="server" Text="Externo" AutoPostBack="true" />
                                <asp:RadioButton ID="rdbInterno" CssClass="radio" GroupName="TipoDeCliente" runat="server" Text="Interno" AutoPostBack="true" />
                            </div>
                        </div>

                        <div class="col">
                            <div>
                                <label for="nombre-cliente">Nombre del Cliente</label>
                                <asp:DropDownList ID="ddlCliente" runat="server" CssClass="form-select" AutoPostBack="true"></asp:DropDownList>
                            </div>

                            <div>
                                <label for="cliente-eventual">Cliente Eventual</label>
                                <asp:TextBox ID="TextNombreCliente" runat="server" CssClass="form-control" MaxLength="50" onchange="ClienEventLosFocu(this)" AutoPostBack="true"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col">
                            <div>
                                <label for="txtNoFactura">Número de Factura:</label>
                                <asp:TextBox ID="txtNoFactura" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                            </div>
                            <div>
                                <label for="ID">Cédula del Cliente</label>
                                <asp:TextBox ID="TextId" runat="server" CssClass="form-control" Text="" AutoPostBack="true"></asp:TextBox>
                            </div>
                        </div>

                    </div>

                    <div class="row mb-3">
                        <asp:UpdatePanel ID="upDatosFactura" runat="server" UpdateMode="Conditional" class="grid-container-2">
                            <ContentTemplate>

                                <div class="row">

                                    <div class="col">
                                        <label for="vendedor">Vendedor:</label>
                                        <asp:DropDownList ID="ddlVendedor" runat="server" CssClass="form-select"></asp:DropDownList>
                                    </div>

                                    <div class="col">
                                        <label for="producto">Producto:</label>
                                        <asp:DropDownList ID="ddlProducto" runat="server" CssClass="form-select" AutoPostBack="true"></asp:DropDownList>
                                    </div>

                                    <div class="col">
                                        <label for="cantidad">Cantidad:</label>
                                        <asp:TextBox ID="TextCantidad" runat="server" CssClass="form-control" Text="0" onkeypress="formatNumber(this, 2)"> </asp:TextBox>
                                    </div>

                                    <div class="col">
                                        <label for="bultos">Bultos:</label>
                                        <asp:TextBox ID="TextBultos" runat="server" CssClass="form-control" Text="0" onkeypress="formatNumber(this, 2)"> </asp:TextBox>
                                    </div>

                                    <div class="col">
                                        <label for="% Desc">% Desc.</label>
                                        <asp:TextBox ID="TextPorDesc" runat="server" CssClass="form-control" Text="0" AutoPostBack="true"></asp:TextBox>
                                    </div>


                                    <div class="col">
                                        <label for="precio">Precio:</label>
                                        <asp:TextBox ID="TextPrecio" runat="server" CssClass="form-control" Text="0.00" AutoPostBack="true" ReadOnly="true"></asp:TextBox>
                                    </div>

                                    <div class="col">
                                        <label for="total">Total:</label>
                                        <asp:TextBox ID="TextTotal" runat="server" CssClass="form-control" Text="0.00" AutoPostBack="true" ReadOnly="true"></asp:TextBox>
                                    </div>

                                </div>

                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="TextCantidad" EventName="TextChanged" />
                                <asp:AsyncPostBackTrigger ControlID="BtnAdicionar" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="ddlProducto" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>


                    </div>

                    <div class="row mb-3">
                        <div class="col">

                            <asp:Button ID="BtnAdicionar" runat="server" Text="Adicionar" CssClass="btn btn-success" class="btnEditar" Enabled="true" />
                            <%--<asp:Button ID="BtnGuardar" runat="server" Text="Eliminar" CssClass="btn-reset" class="btn-reset"  />--%>
                        </div>
                    </div>

                    <div class="row mb-3">


                        <asp:UpdatePanel ID="upGrid" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Literal ID="ltMensajeGrid" runat="server"></asp:Literal>


                                <div class="col">



                                    <div style="min-height: 200px; overflow-y: scroll;">
                                        <asp:GridView
                                            ShowHeaderWhenEmpty="True"
                                            ID="GridViewOne"
                                            runat="server"
                                            CssClass="table table-light table-sm table-striped table-hover table-bordered"
                                            CellPadding="0"
                                            GridLines="None"
                                            AllowPaging="True"
                                            AllowSorting="True"
                                            AutoGenerateColumns="false"
                                            ViewStateMode="Enabled"
                                            DataKeyNames="cod_pais,cod_empresa,cod_puesto,no_factura,fecha,cod_producto">

                                            <SelectedRowStyle CssClass="SelectedRowDelete" />

                                            <Columns>
                                                <asp:BoundField HeaderText="Codigo Producto" DataField="cod_producto" SortExpression="cod_producto" Visible="true" />
                                                <asp:BoundField DataField="Producto" HeaderText="Producto" SortExpression="Producto" Visible="false" />
                                                <asp:BoundField DataField="cantidad" HeaderText="Cantidad" SortExpression="cantidad" />
                                                <asp:BoundField DataField="bultos" HeaderText="Bultos" SortExpression="bultos" />
                                                <asp:BoundField DataField="precio" HeaderText="Precio" SortExpression="precio" />
                                                <asp:BoundField DataField="subtotal" HeaderText="Subtotal" SortExpression="subtotal" />
                                                <asp:BoundField DataField="Descuento" HeaderText="Descuento" SortExpression="Descuento" />
                                                <asp:BoundField DataField="IVA" HeaderText="IVA" SortExpression="IVA" />
                                                <%--<asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" />--%>


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
                                                    <asp:Button ID="B1" runat="server" CommandName="Page" ToolTip="Inicio" CommandArgument="First" CssClass="primero" Text="Primera" formnovalidate="true" />
                                                    <asp:Button ID="B2" runat="server" CommandName="Page" ToolTip="Anterior" CommandArgument="Prev" CssClass="anterior" Text="&larr;" formnovalidate="true" />
                                                    <asp:Button ID="B3" runat="server" CommandName="Page" ToolTip="Siguiente" CommandArgument="Next" CssClass="siguiente" Text="&rarr;" formnovalidate="true" />
                                                    <asp:Button ID="B4" runat="server" CommandName="Page" ToolTip="Último" CommandArgument="Last" CssClass="ultimo" Text="Ultima" formnovalidate="true" />
                                                    <asp:Label ID="CurrentPageLabel" runat="server" CssClass="PagerLabel" />
                                                </div>
                                            </PagerTemplate>

                                        </asp:GridView>

                                    </div>
                                    <asp:HiddenField ID="hdfCodigo" runat="server" />
                                </div>

                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="BtnAdicionar" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="GridViewOne" EventName="RowDeleting" />
                                <asp:AsyncPostBackTrigger ControlID="GridViewOne" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>

                    </div>

                    <div class="row mb-3">
                        <asp:UpdatePanel ID="upPiePagina" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>

                                <div class="row">

                                    <div class="col">
                                        <label>Total Libras</label>
                                        <asp:TextBox ID="TextTotalLibras" runat="server" CssClass="form-control" Text="0.00"
                                            onkeypress="formatNumber(this,2)" Enabled="false" AutoPostBack="true"></asp:TextBox>
                                    </div>

                                    <div class="col">
                                        <label>Subtotal</label>
                                        <asp:TextBox ID="TextSubtotal" runat="server" CssClass="form-control" Text="0.00" onkeypress="formatNumber(this,2)" Enabled="false"></asp:TextBox>
                                    </div>


                                    <div class="col">
                                        <label>Valor Desc.</label>
                                        <asp:TextBox ID="TextValorDesc" CssClass="form-control" runat="server" Enabled="false" Text="0.00" onkeypress="formatNumber(this,2)" AutoPostBack="true"></asp:TextBox>
                                    </div>

                                    <div class="col">
                                        <label>IVA</label>
                                        <asp:TextBox ID="TextIVA" CssClass="form-control" runat="server" Text="0.00" onkeypress="formatNumber(this, 2)" Enabled="False"></asp:TextBox>
                                    </div>

                                    <div class="col">
                                        <label>Neto C$</label>
                                        <asp:TextBox ID="TextNeto" CssClass="form-control" runat="server" Enabled="false"></asp:TextBox>
                                    </div>

                                    <div class="col">
                                        <label>Neto $</label>
                                        <asp:TextBox ID="TextNetoDolar" CssClass="form-control" runat="server" Enabled="false"></asp:TextBox>
                                    </div>

                                </div>

                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="BtnAdicionar" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="GridViewOne" EventName="RowDeleting" />
                                <asp:AsyncPostBackTrigger ControlID="GridViewOne" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>

                    </div>

                    <div class="row">
                        <div class="col">
                            <asp:Button ID="btnDatosComp" runat="server" Text="Guardar Factura" CssClass="btn btn-primary" />
                        </div>
                    </div>

                </div>
            </div>
        </div>


        <div id="form-container-6 " class="">
            <!--- DATOS COMPLEMENTARIOS DE FACTURAS -->

            <div id="popup-form" class="white-popup-block mfp-hide popupFactura">

                <div class="PantallaPopUp">

                    <div class="popup-header">
                        <div class="popup-title">
                            <h4 class="center-color">Datos Complementarios de Facturas</h4>
                        </div>
                        <i class="fas fa-times-circle Close"></i>
                    </div>

                    <asp:UpdatePanel runat="server" ID="upDatosComp" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="center-color">
                                <h2>
                                    <asp:Label ID="LblResp" runat="server" AutoPostBack="true"></asp:Label></h2>
                            </div>
                            <div class="dc-grid-container">

                                <div>
                                    <label id="lblFormaPago" runat="server">Forma de Pago</label>
                                    <asp:DropDownList ID="ddlFormaPago" runat="server" CssClass="" AutoPostBack="true" />
                                </div>


                                <div>
                                    <label>Saldo Factura C$</label>
                                    <asp:TextBox ID="TextSaldoFact" runat="server" Enabled="false" Text="0.00" onkeypress="formatNumber(this, 2)" AutoPostBack="true"></asp:TextBox>
                                </div>

                                <%--  <div>
                                    <label>Saldo Factura US$</label>
                                    <asp:TextBox ID="TextSaldoFactDol" runat="server" Enabled="false" Text="0.00"  onkeypress="formatNumber(this, 2)" AutoPostBack="true" ></asp:TextBox>
                                </div>--%>



                                <div>
                                    <label>Paridad</label>
                                    <asp:TextBox ID="TextTipoCambio" runat="server" Enabled="false"></asp:TextBox>
                                </div>




                                <%--'////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////--%>
                                <div>
                                    <label>Moneda</label>
                                    <asp:DropDownList ID="ddlMoneda" runat="server" CssClass="" AutoPostBack="true" />
                                    <ajaxToolkit:CascadingDropDown
                                        ID="CMoneda"
                                        runat="server"
                                        TargetControlID="ddlMoneda"
                                        ServicePath="../services/WSCatProductos.asmx"
                                        ServiceMethod="GetMoneda"
                                        Category="CategoryName"></ajaxToolkit:CascadingDropDown>
                                </div>

                                <div>
                                    <label id="lblTarjeta" runat="server">Tarjeta</label>
                                    <asp:DropDownList ID="ddlTarjeta" runat="server" CssClass="" AutoPostBack="true" />
                                    <ajaxToolkit:CascadingDropDown
                                        ID="Ctarjeta"
                                        runat="server"
                                        TargetControlID="ddlTarjeta"
                                        ServicePath="../services/WSCatProductos.asmx"
                                        ServiceMethod="GetTarjeta"
                                        Category="CategoryName" />
                                    <%--<ajaxToolkit:CascadingDropDown
                                    ID="Ctarjeta"
                                    runat="server"
                                    TargetControlID="ddlTarjeta"
                                    ServicePath="../services/WSCatProductos.asmx"
                                    ServiceMethod="GetTarjeta"
                                    Category="CategoryName"></
                                    ajaxToolkit:CascadingDropDown>--%>
                                </div>
                                <div class="cel">
                                    <label id="lblBanco" runat="server">Banco</label>
                                    <asp:DropDownList ID="ddlBanco" runat="server" CssClass="" />
                                    <ajaxToolkit:CascadingDropDown
                                        ID="Cbanco"
                                        runat="server"
                                        TargetControlID="ddlBanco"
                                        ServicePath="../services/WSCatProductos.asmx"
                                        ServiceMethod="GetBanco"
                                        Category="CategoryName"></ajaxToolkit:CascadingDropDown>
                                </div>



                                <%--//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////--%>

                                <div>
                                    <label id="lblCordobasRec" runat="server">Valor Recibido C$/US$</label>
                                    <asp:TextBox ID="TextCordobasRec" runat="server" MaxLength="100" Text="0.00" onkeypress="formatNumber(this, 2)" AutoPostBack="true"></asp:TextBox>
                                </div>

                                <%--                                <div>
                                    <label id="lblDolar" runat="server" visible="false">Valor Recibido $</label>
                                    <asp:TextBox ID="TextDolar" runat="server" Enabled="True" MaxLength="100" onkeypress="formatNumber(this, 2)" Text="0.00" Visible="false"  AutoPostBack="true"></asp:TextBox>
                                </div>--%>

                                <%-- <div>
                                    <label id="lblConverDolACor" runat="server" visible="true">Conversión de $ a C$</label>
                                    <asp:TextBox ID="TextConverDolACor" runat="server" MaxLength="100" Text="0.00" onkeypress="formatNumber(this, 2)" Visible="true" Enabled="false"></asp:TextBox>
                                </div>--%>

                                <div>
                                    <label>Cambio C$</label>
                                    <asp:TextBox ID="TextVuelto" runat="server" Text="0.00" onkeypress="formatNumber(this, 2)" Enabled="false" AutoPostBack="true"></asp:TextBox>
                                </div>



                                <%--      <div>
                                    <label>Vendedor</label>
                            <asp:DropDownList ID="ddVendedor" runat="server" CssClass=" "/>
                                    <ajaxToolkit:CascadingDropDown
                                    ID="Cvendedor"
                                    runat="server"
                                    TargetControlID= "ddVendedor"
                                    ServicePath ="../services/WSCatProductos.asmx"
                                    ServiceMethod="GetVendedor"
                                    Category="CategoryName">
                                    </ajaxToolkit:CascadingDropDown>
                            </div>--%>
                            </div>
                            <!-- GRID CONTAINER DC -->

                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnDatosComp" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="BtnAdiciFormaPago" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="ddlFormaPago" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="ddlMoneda" EventName="SelectedIndexChanged" />
                            <%--<asp:AsyncPostBackTrigger ControlID="TextDolar" EventName="TextChanged"/>--%>
                            <asp:AsyncPostBackTrigger ControlID="TextSaldoFact" EventName="TextChanged" />
                            <asp:AsyncPostBackTrigger ControlID="GridViewpop" EventName="RowDeleting" />
                            <asp:AsyncPostBackTrigger ControlID="BtnPopupImpGuar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="TextNeto" EventName="TextChanged" />
                            <asp:AsyncPostBackTrigger ControlID="TextCordobasRec" EventName="TextChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                    <div class="btnGuarda">
                        <asp:Button ID="BtnAdiciFormaPago" runat="server" Text="Aceptar Forma de Pago" class="BtnAdiciFormaPago" />
                    </div>

                    <!-- GRIDVIEW DC -->

                    <asp:UpdatePanel ID="upGridPop" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Literal ID="LiteralGridpop" runat="server"></asp:Literal>

                            <div style="min-height: 200px; overflow: scroll;">
                                <asp:GridView ShowHeaderWhenEmpty="true"
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
                                    BorderColor="#993300"
                                    OnRowDeleting="GridViewpop_RowDeleting">

                                    <SelectedRowStyle CssClass="SelectedRowDelete" />

                                    <Columns>
                                        <asp:BoundField DataField="No_Factura" HeaderText="No_Factura" SortExpression="No_Factura" Visible="false" />
                                        <asp:BoundField DataField="Cod_FormaPago" HeaderText="Cod_FormaPago" SortExpression="Cod_FormaPago" Visible="false" />
                                        <asp:BoundField DataField="FormaPago" HeaderText="Forma de Pago" SortExpression="FormaPago" Visible="true" />
                                        <asp:BoundField DataField="Moneda" HeaderText="Moneda" SortExpression="Moneda" />
                                        <asp:BoundField DataField="ValorFacturaCor" HeaderText="Valor Factura C$" SortExpression="ValorFacturaCor" />
                                        <asp:BoundField DataField="ValorFacturaDol" HeaderText="Valor Factura US$" SortExpression="ValorFacturaDol" />
                                        <asp:BoundField DataField="ValorRecibidoCor" HeaderText="Cordobas Recibidos" SortExpression="ValorRecibidoCor" />
                                        <asp:BoundField DataField="ValorRecibidoDol" HeaderText="Dolares Recibidos" SortExpression="ValorRecibidoDol" />
                                        <asp:BoundField DataField="ConversionUS$aC$" HeaderText="Conversión" SortExpression="ConversionUS$aC$" />
                                        <asp:BoundField DataField="SaldoFacturaCor" HeaderText="Saldo Factura C$" SortExpression="SaldoFacturaCor" />
                                        <asp:BoundField DataField="SaldoFacturaDol" HeaderText="Saldo Factura US$" SortExpression="SaldoFacturaDol" />

                                        <asp:CommandField
                                            HeaderText="Suprimir"
                                            ButtonType="Button"
                                            SelectText="Eliminar"
                                            ShowDeleteButton="true"
                                            ShowSelectButton="false"
                                            HeaderStyle-Width="120">
                                            <ControlStyle CssClass="btnEliminar" />
                                        </asp:CommandField>



                                    </Columns>
                                    <PagerTemplate>
                                        <div class="pagination">
                                            <asp:Button ID="B1" runat="server" CommandName="Page" ToolTip="Prim. Pag" CommandArgument="First" CssClass="primero" Text="Primera" formnovalidate="true" />
                                            <asp:Button ID="B2" runat="server" CommandName="Page" ToolTip="Pág. anterior" CommandArgument="Prev" CssClass="anterior" Text="&larr;" formnovalidate="true" />
                                            <asp:Button ID="B3" runat="server" CommandName="Page" ToolTip="Sig. página" CommandArgument="Next" CssClass="siguiente" Text="&rarr;" formnovalidate="true" />
                                            <asp:Button ID="B4" runat="server" CommandName="Page" ToolTip="Últ. Pag" CommandArgument="Last" CssClass="ultimo" Text="Ultima" formnovalidate="true" />
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
                            <asp:AsyncPostBackTrigger ControlID="btnDatosComp" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="ddlMoneda" EventName="SelectedIndexChanged" />
                            <%--<asp:AsyncPostBackTrigger ControlID="TextDolar" EventName="TextChanged"/>--%>
                            <asp:AsyncPostBackTrigger ControlID="TextSaldoFact" EventName="TextChanged" />

                        </Triggers>
                    </asp:UpdatePanel>

                    <div id="btnContainer">
                        <asp:Button ID="BtnPopupImpGuar" runat="server" Text="Imprimir/Guardar" class="btnEditar" Width="189px" />
                        <asp:Button ID="BtnRegresa" runat="server" Text="Regresar" Class="btnEliminar" />
                    </div>

                    <!-- GRIDVIEW DC -->



                </div>
                <!-- PantallaPopUp  -->
            </div>
            <!-- #popup-form  -->


        </div>
        <!--- END DATOS COMPLEMENTARIOS DE FACTURAS -->

    </div>
    <!-- MAIN-CONTAINER -->
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="cpScripts" runat="Server">

    <script type="text/javascript">

        window.onload = function () {
            const textBoxes = document.querySelectorAll("input[type='text'].number-format");
            for (var i = 0; i < textBoxes.length; i++) {
                textBoxes[i].addEventListener("input", formatNum);
            }
        };

        function formatNum(event) {
            let txtNumber = event.target;
            let number = txtNumber.value;
            txtNumber.value = numberWithCommas(number);
        }


        function numberWithCommas(x) {
            return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        }

        $(document).ready(function () {
            $('.select2').select2();
        });

        $(document).ready(function () {
            //$('.datagird').basictable();

            $("#btnDatosComp").click(function () {




                //if ($("input[rdbContado']").is(':checked')) {



                $("#<%=ddlFormaPago.ClientID%>").val("E");
                $("#<%=ddlFormaPago.ClientID%>").prev().removeClass('visible');


                $("#<%=ddlMoneda.ClientID%>").val("1");
                $("#<%=ddlMoneda.ClientID%>").prev().removeClass('visible');

                $("#<%=ddlVendedor.ClientID%>").val("4");
                $("#<%=ddlVendedor.ClientID%>").prev().removeClass('visible');

                 <%-- $("#<%=TextDatos.ClientID%>").val("");
                  $("#<%=TextDatos.ClientID%>").prev().removeClass('visible');--%>

                $("#<%=ddlBanco.ClientID%>").val("");
                $("#<%=ddlBanco.ClientID%>").prev().removeClass('visible');

                $("#<%=ddlTarjeta.ClientID%>").val("");
                $("#<%=ddlTarjeta.ClientID%>").prev().removeClass('visible');

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


        $("#<%=ddlFormaPago.ClientID%>").change(function () {
            if ($(this).val() == "E") {
                $("#ddlbanco").prop("disabled", false);
                $("#ddlTarjeta").prop("disabled", false);
            } else {
                $("#ddlbanco").prop("disabled", true);
                $("ddlTarjeta").prop("disabled", true);
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



    </script>
</asp:Content>

