<%@ Page Title="Recibos" Language="VB" EnableEventValidation="false" MasterPageFile="~/master/prinFactura.master" AutoEventWireup="false" CodeFile="Recibos.aspx.vb" Inherits="movimientos_Recibos" %>

<%@ Register Src="~/usercontrol/menu_catalogos.ascx" TagPrefix="uc1" TagName="menu_catalogos" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="c1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="../css/select2.css" />
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
    <div class="container-fluid">

        <div class="row">
            <!-- Detalles del Vendedor y Cliente-->
            <div class="col-6">
                <div class="card shadow mb-4">
                    <div class="card-header py-6 bg-primary">
                        <h6 class="m-0 font-weight-bold text-white">Datos del Vendedor</h6>
                    </div>
                    <div class="card-body">
                        <div class="row mb-3">
                            <div class="col-6">
                                <div class="input-group">
                                    <span class="input-group-text">Vendedor</span>
                                    <asp:DropDownList ID="ddlVendedor" runat="server" CssClass="form-select form-select-sm" AutoPostBack="true"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-6">
                                <div class="input-group">
                                    <span class="input-group-text">No. Recibo</span>
                                    <asp:TextBox ID="txtNoRecibo" runat="server" CssClass="form-control form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="row mb-3">
                            <div class="col-6">
                                <div class="input-group">
                                    <span class="input-group-text">Cliente</span>
                                    <asp:DropDownList ID="ddlCliente" runat="server" CssClass="form-select form-select-sm"></asp:DropDownList>
                                </div>
                            </div>

                            <div class="col-6">
                                <div class="input-group">
                                    <span class="input-group-text">Tipo Cliente</span>
                                    <asp:DropDownList ID="ddlTipoCliente" runat="server" CssClass="form-select form-select-sm"></asp:DropDownList>
                                </div>
                            </div>

                        </div>

                        <div class="row">
                            <div class="d-grid gap-2 col-5 mx-auto">
                                <asp:Button ID="BtnBuscar" runat="server" CssClass="btn btn-secondary btn-sm" Text="Buscar Recibos" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <!-- END Detalle Vendedor y Cliente -->

            <%-- Datos del Recibo --%>
            <div class="col-4">
                <div class="card shadow mb-4">
                    <div class="card-header py-6 bg-primary">
                        <h6 class="m-0 font-weight-bold text-white">Datos del Recibo</h6>
                    </div>
                    <div class="card-body">
                        <div class="row d-flex flex-column">
                            <div class="col mb-3">
                                <div class="input-group">
                                    <span class="input-group-text">Pendiente</span>
                                    <asp:TextBox ID="txtPendiente" runat="server" ReadOnly="true" CssClass="form-control form-control-sm" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col mb-3">
                                <div class="input-group">
                                    <span class="input-group-text">Aplicado</span>
                                    <asp:TextBox ID="txtAplicado" runat="server" ReadOnly="true" CssClass="form-control form-control-sm" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col">
                                <div class="input-group">
                                    <span class="input-group-text">Saldo</span>
                                    <asp:TextBox ID="txtSaldo" runat="server" ReadOnly="true" CssClass="form-control form-control-sm" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <%-- End Datos del Recibo --%>
        </div>
        <%-- END Row --%>


        <%-- GridView Recibos --%>
        <div class="row mb-4 shadow">
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
                            DataSourceID="SqlDataSource1"
                            EmptyDataText="No se encontraron registros...">

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

        </div>
        <%-- End GridView Recibos--%>


        <%-- Row --%>
        <div class="row">
            <%-- Datos del Pago --%>
            <div class="col-8">
                <div class="card shadow mb-4">
                    <div class="card-header py-6 bg-primary">
                        <h6 class="m-0 font-weight-bold text-white">Datos del Pago</h6>
                    </div>
                    <div class="card-body">
                        <div class="row mb-3">
                            <div class="col">
                                <div class="input-group">
                                    <span class="input-group-text">Forma de Pago</span>
                                    <asp:DropDownList ID="ddlFormaPago" runat="server" CssClass="form-select form-select-sm" AutoPostBack="true"></asp:DropDownList>
                                </div>
                            </div>

                            <div class="col">
                                <div class="input-group">
                                    <span class="input-group-text">Moneda</span>
                                    <asp:DropDownList ID="ddlMoneda" runat="server" CssClass="form-select form-select-sm" AutoPostBack="true"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="col">
                                <div class="input-group">
                                    <span class="input-group-text">Valor $</span>
                                    <asp:TextBox ID="txtValor" runat="server" CssClass="form-control form-control-sm" AutoPostBack="true"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <asp:UpdatePanel ID="upBanco" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col">
                                        <div class="input-group">
                                            <span class="input-group-text">Banco</span>
                                            <asp:DropDownList ID="ddlBanco" runat="server" CssClass="form-select form-select-sm" AutoPostBack="true" />
                                        </div>
                                    </div>
                                    <div class="col">
                                        <div class="input-group">
                                            <span class="input-group-text">Cuenta</span>
                                            <asp:DropDownList ID="ddlCuenta" runat="server" CssClass="form-select form-select-sm" />
                                        </div>
                                    </div>
                                    <div class="col">
                                        <div class="input-group">
                                            <span class="input-group-text">No. Cheque</span>
                                            <asp:TextBox ID="txtCheque" runat="server" CssClass="form-control form-control-sm" AutoPostBack="true" Enabled="false"></asp:TextBox>
                                        </div>
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
            <%-- END Datos del Pago --%>

            <%-- Btn Group --%>
            <div class="col-3">
                <div class="card shadow mb-4">
                    <div class="card-header py-6 bg-secondary">
                        <h6 class="m-0 font-weight-bold text-white">Opciones</h6>
                    </div>
                    <div class="card-body">
                        <div class="row d-grid gap-1 flex-column">
                            <asp:Button ID="btnAceptar2" runat="server" CssClass="btn btn-primary   " Text="Aceptar" />
                            <asp:Button ID="btnSalir" runat="server" CssClass="btn btn-danger " Text="Salir" />
                            <asp:Button ID="btnReportes" runat="server" CssClass="btn btn-secondary " Text="Reportes" />
                        </div>
                    </div>
                </div>
            </div>
            <%-- End Btn Group --%>
        </div>
        <%-- End Row --%>
    </div>
    <%-- End Container-fluid --%>

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

    <%--  </ContentTemplate>
                       <Triggers>

                         <asp:AsyncPostBackTrigger ControlID="BtnBuscar" EventName="Click" />
                         <asp:AsyncPostBackTrigger ControlID="txtValor" EventName="TextChanged" />
                       </Triggers>
          </asp:UpdatePanel>--%>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="cpScripts" runat="server">
    <script type="text/javascript">
        document.addEventListener("DOMContentLoaded", () => {
            console.log("DOM is ready")

           <%-- $("#<%=ddlVendedor.ClientID%>").select2({
                placeholder: "Seleccione un vendedor",
                allowClear: true,
                width: '100%'
            });

            $("#<%=ddlCliente.ClientID%>").select2({
                placeholder: "Seleccione un cliente",
                allowClear: true,
                width: '100%'
            });--%>

         <%--   $("#<%=ddlMoneda.ClientID%>").select2({
                placeholder: "Seleccione una moneda",
                allowClear: true,
                width: '100%'
            });

            $("#<%=ddlFormaPago.ClientID%>").select2({
                placeholder: "Seleccione una forma de pago",
                allowClear: true,
                width: '100%'
            });

            $("#<%=ddlBanco.ClientID%>").select2({
                placeholder: "Seleccione un banco",
                allowClear: true,
                width: '100%'
            });

            $("#<%=ddlCuenta.ClientID%>").select2({
                placeholder: "Seleccione una cuenta",
                allowClear: true,
                width: '100%'
            });--%>

        <%--    $("#<%=ddlVendedor.ClientID%>").on("select2:select", function (e) {
                console.log("Vendedor is selected")
                var data = e.params.data;
                console.log(data)
                var vendedor = data.id;
                console.log(vendedor)
                $("#<%=ddlCliente.ClientID%>").empty();
                $("#<%=ddlCliente.ClientID%>").append("<option value=''>Seleccione un cliente</option>");
                $("#<%=ddlCliente.ClientID%>").select2("val", "");
                $("#<%=ddlCliente.ClientID%>").select2("data", null);
                $("#<%=ddlCliente.ClientID%>").select2("destroy");
                $("#<%=ddlCliente.ClientID%>").select2({
                    placeholder: "Seleccione un cliente",
                    allowClear: true,
                    width: '100%'
                });
                $("#<%=ddlCliente.ClientID%>").select2("val", "");
                $("#<%=ddlCliente.ClientID%>").select2("data", null);
                $("#<%=ddlCliente.ClientID%>").select2("destroy");
                $("#<%=ddlCliente.ClientID%>").select2({
                    placeholder: "Seleccione un cliente",
                    allowClear: true,
                    width: '100%'
                });
                $("#<%=ddlCliente.ClientID%>").select2()
            })--%>
        });


<%--            $(document).ready(function () {
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

        });--%>


    </script>
</asp:Content>


