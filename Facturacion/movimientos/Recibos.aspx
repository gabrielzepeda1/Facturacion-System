<%@ Page Title="Recibos" Language="VB" EnableEventValidation="false" MasterPageFile="~/master/prinFactura.master" AutoEventWireup="false" CodeFile="Recibos.aspx.vb" Inherits="movimientos_Recibos" %>

<%@ Register Src="~/usercontrol/menu_catalogos.ascx" TagPrefix="uc1" TagName="menu_catalogos" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="c1" ContentPlaceHolderID="head" runat="Server">
    <%--<link rel="stylesheet" href="../css/facturacion.css" />--%>
    <link rel="stylesheet" href="../css/select2.css" />
    <%--<link rel="stylesheet" href="../css/recibos.css" />--%>
</asp:Content>

<asp:Content ID="c2" ContentPlaceHolderID="cpTitulo" runat="Server">
    <h4>Recibos</h4>
</asp:Content>

<asp:Content ID="c3" ContentPlaceHolderID="cpUbicacion" runat="Server">
    <a href="../Default.aspx">Menu</a>
    <label>&gt;</label>
    <a href="Recibos.aspx">Recibos</a>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContentPlaceHolder" runat="Server">
    <%--<asp:ScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ScriptManager>--%>

    <asp:Panel ID="pnlContent" runat="server" DefaultButton="BtnBuscar">
    </asp:Panel>

    <div class="container-fluid">
        <div class="row">
            <div class="col-4">
                <!-- Vendedor Section -->
                <div class="card shadow mb-4">
                    <div class="card-header py-6 bg-primary">
                        <h6 class="m-0 font-weight-bold text-white">Datos del Vendedor</h6>
                    </div>
                    <div class="card-body">
                        <div class="row">
                            <div class="">
                                <asp:DropDownList ID="ddlVendedor" runat="server" CssClass="form-select" AutoPostBack="true"></asp:DropDownList>
                                <label class="form-label" for="ddlVendedor">Vendedor</label>
                            </div>
                            <div class="col">
                                <label for="txtNoRecibo">No. Recibo</label>
                                <asp:TextBox ID="txtNoRecibo" runat="server" CssClass="form-control" AutoPostBack="true"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Cliente Section -->
                <div class="card shadow mb-4">
                    <div class="card-header py-6 bg-primary">
                        <h6 class="m-0 font-weight-bold text-white">Datos del Cliente</h6>
                    </div>
                    <div class="card-body">
                        <div class="row">

                            <div class="btn-group">
                                <div class="form-check">
                                    <input type="radio" value="" class="form-check-input" />
                                    <label class="btn btn-outline-primary" for="btncheck1">Externo</label>
                                    <input type="radio" value="" class="form-check-input" />
                                    <label class="btn btn-outline-primary" for="btncheck1">Interno</label>
                                    <input type="radio" value="" class="form-check-input" />
                                    <label class="btn btn-outline-primary" for="btncheck1">Ganadero</label>
                                    <%--<asp:RadioButton ID="rdbExterno" CssClass="form-check-input" GroupName="CondicionPago" Text="Externo" runat="server" AutoPostBack="true" />
                        <asp:RadioButton ID="rdbInterno" CssClass="form-check-input" GroupName="CondicionPago" Text="Interno" runat="server" AutoPostBack="true" />
                        <asp:RadioButton ID="rdbGanadero" CssClass="form-check-input" GroupName="CondicionPago" Text="Ganadero" runat="server" AutoPostBack="true" />--%>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <label class="form-label" for="ddlCliente">Nombre del Cliente</label>
                            <asp:DropDownList ID="ddlCliente" runat="server" CssClass="form-select SelectStyle SelectSearch" Enabled="false"></asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-5">
                <div class="card shadow mb-4">
                    <div class="card-header py-6 bg-primary">
                        <h6 class="m-0 font-weight-bold text-white">Datos del Recibo</h6>
                    </div>
                    <div class="card-body">
                        <div class="row">
                            <div class="col-6">
                                <label for="txtValor">Valor</label>
                                <asp:TextBox ID="txtValor" runat="server" CssClass="form-control" AutoPostBack="true"></asp:TextBox>
                            </div>
                            <div class="col-6">
                                <label for="ddlMoneda">Moneda</label>
                                <asp:DropDownList ID="ddlMoneda" runat="server" CssClass="form-select" AutoPostBack="true"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-6">
                                <label id="lblFormaPago" runat="server">Forma de Pago</label>
                                <asp:DropDownList ID="ddlFormaPago" runat="server" CssClass="form-select" AutoPostBack="true"></asp:DropDownList>
                            </div>
                            <div class="col-6">
                                <label for="txtCheque" id="lblCheque">No. Cheque</label>
                                <asp:TextBox ID="txtCheque" runat="server" CssClass="form-control" AutoPostBack="true" Enabled="false"></asp:TextBox>
                            </div>
                        </div>

                        <asp:UpdatePanel ID="upBanco" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col">
                                        <label id="lblBanco" class="form-label" runat="server">Banco:</label>
                                        <asp:DropDownList ID="ddlBanco" runat="server" CssClass="form-select" AutoPostBack="true" />
                                    </div>
                                    <div class="col">
                                        <label id="lblCuenta" class="form-label" runat="server">Cuenta Bancaria:</label>
                                        <asp:DropDownList ID="ddlCuenta" runat="server" CssClass="form-select" />
                                    </div>
                                </div>

                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlBanco" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>

            </div>

            <div class="col-3">
                <div class="card shadow mb-4">
                    <div class="card-header py-6 bg-primary">
                        <h6 class="m-0 font-weight-bold text-white">Datos del Recibo</h6>
                    </div>
                    <div class="card-body">
                        <div class="row d-flex flex-column">
                            <div class="col">
                                <label id="lblPendiente" class="form-label">Pendiente</label>
                                <asp:TextBox ID="txtPendiente" runat="server" ReadOnly="true" CssClass="form-control" Enabled="false"></asp:TextBox>
                            </div>
                            <div class="col">
                                <label id="lblAplicado" class="form-label">Aplicado</label>
                                <asp:TextBox ID="txtAplicado" runat="server" ReadOnly="true" CssClass="form-control" Enabled="false"></asp:TextBox>
                            </div>
                            <div class="col">
                                <label id="lblSaldo" class="form-label">Saldo</label>
                                <asp:TextBox ID="txtSaldo" runat="server" ReadOnly="true" CssClass="form-control" Enabled="false"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>


        <asp:UpdatePanel runat="server">
            <ContentTemplate>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlVendedor" EventName="SelectedIndexChanged" />
            </Triggers>
        </asp:UpdatePanel>

        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
            <ContentTemplate>
            </ContentTemplate>
            <Triggers>
                <%-- <asp:AsyncPostBackTrigger ControlID="rdbExterno" EventName="CheckedChanged" />
         <asp:AsyncPostBackTrigger ControlID="rdbInterno" EventName="CheckedChanged" />
         <asp:AsyncPostBackTrigger ControlID="rdbGanadero" EventName="CheckedChanged" />--%>
            </Triggers>
        </asp:UpdatePanel>

        <div class="container-fluid d-flex justify-content-start">
            <asp:Button ID="BtnBuscar" runat="server" CssClass="btn btn-success" Text="Aceptar" />
        </div>
    </div>

    <%--  </ContentTemplate>
                       <Triggers>

                         <asp:AsyncPostBackTrigger ControlID="BtnBuscar" EventName="Click" />
                         <asp:AsyncPostBackTrigger ControlID="txtValor" EventName="TextChanged" />
                       </Triggers>
          </asp:UpdatePanel>--%>

    <asp:UpdatePanel ID="upGrid" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Literal ID="ltMensajeGrid" runat="server"></asp:Literal>

            <div style="min-height: 200px; overflow: scroll;">

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
                    ShowHeaderWhenEmpty="true"
                    DataKeyNames=""
                    BorderColor="#993300"
                    DataSourceID="SqlDataSource1">

                    <SelectedRowStyle CssClass="SelectedRowDelete" />

                    <Columns>
                        <asp:BoundField HeaderText="Cliente" DataField="Nombre_cliente" SortExpression="Nombre_cliente" Visible="true" />
                        <asp:BoundField DataField="Tipo_Docum" HeaderText="Pago" SortExpression="Pago" />
                        <asp:BoundField DataField="No_Documento" HeaderText="No_Documento" SortExpression="Num_documento" />
                        <asp:BoundField DataField="Fecha_Doc" HeaderText="Fecha" SortExpression="Fecha" />
                        <asp:BoundField DataField="Valor_pend" HeaderText="Pendiente" SortExpression="Pendiente" />
                        <asp:BoundField DataField="Valor_aplicado" HeaderText="Aplicado" SortExpression="Aplicado" />
                        <asp:BoundField DataField="Saldo" HeaderText="Saldo" SortExpression="Saldo" />

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
                            <asp:Button ID="B1" runat="server" CommandName="Page" ToolTip="Prim. Pag" CommandArgument="First" CssClass="primero" Text="Primera" formnovalidate="true" />
                            <asp:Button ID="B2" runat="server" CommandName="Page" ToolTip="Pág. anterior" CommandArgument="Prev" CssClass="anterior" Text="&larr;" formnovalidate="true" />
                            <asp:Button ID="B3" runat="server" CommandName="Page" ToolTip="Sig. página" CommandArgument="Next" CssClass="siguiente" Text="&rarr;" formnovalidate="true" />
                            <asp:Button ID="B4" runat="server" CommandName="Page" ToolTip="Últ. Pag" CommandArgument="Last" CssClass="ultimo" Text="Ultima" formnovalidate="true" />
                            <asp:Label ID="CurrentPageLabel" runat="server" CssClass="PagerLabel" />
                        </div>
                    </PagerTemplate>
                </asp:GridView>
                <asp:SqlDataSource runat="server" ID="SqlDataSource1"></asp:SqlDataSource>
                <asp:HiddenField ID="hdfCodigo" runat="server" />
                <%--<asp:SqlDataSource ID="FacturaTmp" runat="server" ConnectionString="<%$ ConnectionStrings:FacturacionConnectionString %>" SelectCommand="SELECT [no_factura] as NoFactura, [cod_producto] as Codigo, [desc_imprimir] as Producto, [cantidad] as Cantidad, [bultos] as Bultos, [precio_unidad] as Precio, [sub_total] as SubTotal, [valor_descuento] as Descuento, [valor_iva] as Iva FROM [TmpDetFact]"></asp:SqlDataSource>--%>
            </div>
        </ContentTemplate>
        <Triggers>
            <%--<asp:AsyncPostBackTrigger ControlID="BtnGuardar" EventName="Click" />--%>
            <asp:AsyncPostBackTrigger ControlID="BtnBuscar" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="GridViewOne" EventName="RowDeleting" />
            <asp:AsyncPostBackTrigger ControlID="GridViewOne" EventName="SelectedIndexChanged" />
        </Triggers>
    </asp:UpdatePanel>

    <div class="container">
    </div>

    <%--Buttons--%>
    <div class="btn-group">
        <asp:Button ID="btnAceptar2" runat="server" CssClass="btn btn-primary" Text="Aceptar" />
        <asp:Button ID="btnSalir" runat="server" CssClass="btn btn-danger" Text="Salir" />
        <asp:Button ID="btnReportes" runat="server" CssClass="btn btn-secondary" Text="Reportes" />
    </div>

</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="cpScripts" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {
            $('.SelectSearch').select2();
            $("radio").checkboxradio();
            $("fieldset").controlgroup();
            $(".ui-button").button();
            $("#<%=txtCheque.ClientID%>").hide();
            $("#lblCheque").hide();
            console.log("Ready")


            $("#CP1_ddlFormaPago-menu").on("click", "li", function () {
                console.log("Function is running...")
                var output = $("#CP1_ddlFormaPago-menu li:eq(1) ").html()
                var type = typeof output
                console.log(output)
                console.log(type)

                if ($("#CP1_ddlFormaPago-menu li:eq(1) ").html() == '<div id="ui-id-5" tabindex="-1" role="option" class="ui-menu-item-wrapper ui-state-active">CHEQUE</div>') {
                    console.log("Show");
                    $("#<%=txtCheque.ClientID%>").show();
                    $("#lblCheque").show();
                } else {
                    console.log("Hide")
                    $("#<%=txtCheque.ClientID%>").hide();
                    $("#lblCheque").hide();
                }



            });

        });


    </script>
</asp:Content>


