<%@ Page Title="Recibos" Language="VB" EnableEventValidation="false"  MasterPageFile="~/master/prinFactura.master" AutoEventWireup="false" CodeFile="Recibos.aspx.vb" Inherits="movimientos_Recibos"  %>
<%@ Register Src="~/usercontrol/menu_catalogos.ascx" TagPrefix="uc1" TagName="menu_catalogos" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="c1" ContentPlaceHolderID="head" Runat="Server">
    <link  rel="stylesheet" href= "../css/facturacion.css" />
    <link  rel="stylesheet" href= "../css/select2.css" />
    <link  rel="stylesheet" href= "../css/recibos.css" />

</asp:Content>

<asp:Content ID="c2" ContentPlaceHolderID="cpTitulo" Runat="Server">
    <h4>Recibos</h4>
 </asp:Content>

<asp:Content ID="c3" ContentPlaceHolderID="cpUbicacion" Runat="Server">
     <a href="../Default.aspx">Menu</a>
    <label>&gt;</label>
    <a href="Recibos.aspx">Recibos</a>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="CP1" Runat="Server">
    <%--<asp:ScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ScriptManager>--%>

    <asp:Panel ID="pnlContent" runat="server" DefaultButton="BtnBuscar">
            <asp:UpdatePanel ID="upMensaje_popup" runat="server" UpdateMode="Conditional">
                <ContentTemplate>

                    <asp:Literal ID="ltMensaje_popup" runat="server"></asp:Literal>
                    <asp:Literal ID="ltMensaje" runat="server"></asp:Literal>
                </ContentTemplate>


                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="BtnBuscar" EventName="Click"/>
                    <asp:AsyncPostBackTrigger ControlID="ddlVendedor" EventName="SelectedIndexChanged" />
                </Triggers>
            </asp:UpdatePanel>

          </asp:Panel>

         <div class="container1">
    <!-- Vendedor Section -->

    <div class="cel fleft" style="margin-right:10px;">
        <label for="ddlVendedor">Vendedor</label>
        <asp:DropDownList ID="ddlVendedor" runat="server" CssClass="SelectStyle SelectSearch" AutoPostBack="true"></asp:DropDownList>
    </div>

    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="cel">
                <label for="txtNoRecibo">No. Recibo</label>
                <asp:TextBox ID="txtNoRecibo" runat="server" CssClass="SelectStyle numero" AutoPostBack="true" ></asp:TextBox>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ddlVendedor" EventName="SelectedIndexChanged" />
        </Triggers>
    </asp:UpdatePanel>



    <!-- Cliente Section -->
    <div class="ui-widget">
        <fieldset>
            <legend>Cliente es:</legend>
            <asp:RadioButton ID="rdbExterno" CssClass="" GroupName="CondicionPago" Text="Externo" runat="server" AutoPostBack="true" />
            <asp:RadioButton ID="rdbInterno" CssClass="" GroupName="CondicionPago" Text="Interno" runat="server" AutoPostBack="true" />
            <asp:RadioButton ID="rdbGanadero" CssClass="" GroupName="CondicionPago" Text="Ganadero" runat="server" AutoPostBack="true" />
        </fieldset>
    </div>

    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="cel">
                <label for="ddlCliente">Nombre del Cliente</label>
                <asp:DropDownList ID="ddlCliente" runat="server" CssClass="SelectStyle SelectSearch" Enabled="false"></asp:DropDownList>
                <%--<ajaxToolkit:CascadingDropDown
                    ID="ccCliente"
                    runat="server"
                    TargetControlID="ddlCliente"
                    ServicePath="../services/WSCatProductos.asmx"
                    ServiceMethod="GetCliente"
                    Category="CategoryCliente"
                    PromptText="Seleccione Cliente...">
                </ajaxToolkit:CascadingDropDown>--%>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="rdbExterno" EventName="CheckedChanged" />
            <asp:AsyncPostBackTrigger ControlID="rdbInterno" EventName="CheckedChanged" />
            <asp:AsyncPostBackTrigger ControlID="rdbGanadero" EventName="CheckedChanged" />
        </Triggers>
    </asp:UpdatePanel>

    <!-- Main Section -->
    <fieldset class="fs-container">
        <div class="flex">
            <div class="fleft">
                <label for="txtValor">Valor</label>
                <asp:TextBox ID="txtValor" runat="server" CssClass="numero" AutoPostBack="true"></asp:TextBox>
            </div>
            <div class="cel">
                <label for="ddlMoneda">Moneda</label>
                <asp:DropDownList ID="ddlMoneda" runat="server" CssClass="SelectStyle" AutoPostBack="true"></asp:DropDownList>
            </div>
            <div class="cel">
                <label id="lblFormaPago" runat="server">Forma de Pago</label>
                <asp:DropDownList ID="ddlFormaPago" runat="server" CssClass="SelectStyle" AutoPostBack="true"></asp:DropDownList>
            </div>
            <div class="fleft">
                <label for="txtCheque" id="lblCheque">No. Cheque</label>
                <asp:TextBox ID="txtCheque" runat="server" CssClass="numero" AutoPostBack="true" Enabled="false"></asp:TextBox>
            </div>
        </div>
    </fieldset>

    <div class="ui-widget">
        <asp:Button ID="BtnBuscar" runat="server" CssClass="btnEditar" Text="Aceptar" />
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

                        <div style ="min-height:200px; overflow:scroll;">

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


            <asp:UpdatePanel ID="upBanco" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="flex small-container">
                        <div class="cel">
                           <label id="lblBanco" runat="server">Banco:</label>
                           <asp:DropDownList ID="ddlBanco" runat="server" CssClass="SelectStyle SelectSearch" AutoPostBack="true"  />
                        </div>
                        <div class="cel">
                           <label id="lblCuenta" runat="server">Cuenta Bancaria:</label>
                           <asp:DropDownList ID="ddlCuenta" runat="server" CssClass="SelectStyle SelectSearch"   />
                        </div>
                    </div>

                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ddlBanco" EventName="SelectedIndexChanged" />
                </Triggers>
            </asp:UpdatePanel>


                    <div class="flex small-container2 ">

                      <div class="cel">
                            <label id="lblPendiente">Pendiente</label>
                            <asp:TextBox ID="txtPendiente" runat="server" ReadOnly="true" CssClass="numero" Enabled="false"></asp:TextBox>
                      </div>

                        <div class="cel">
                            <label id="lblAplicado">Aplicado</label>
                            <asp:TextBox ID="txtAplicado" runat="server" ReadOnly="true" CssClass="numero" Enabled="false"></asp:TextBox>
                        </div>

                        <div class="cel">
                            <label id="lblSaldo">Saldo</label>
                            <asp:TextBox ID="txtSaldo" runat="server" ReadOnly="true" CssClass="numero" Enabled="false"></asp:TextBox>
                        </div>

                    </div>

                <%--Buttons--%>
                <div class="">
                  <asp:Button ID="btnAceptar2" runat="server" CssClass ="btnEditar" Text="Aceptar"/>
                  <asp:Button ID="btnSalir" runat="server" CssClass ="btnEliminar" Text="Salir"/>
                  <asp:Button ID="btnReportes" runat="server" CssClass ="btnEditar" Text="Reportes"/>
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

                if ($("#CP1_ddlFormaPago-menu li:eq(1) ").html() == '<div id="ui-id-5" tabindex="-1" role="option" class="ui-menu-item-wrapper ui-state-active">CHEQUE</div>')
                {
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


