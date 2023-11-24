<%@ Page Title="" Language="VB" MasterPageFile="~/master/prinFactura.master" AutoEventWireup="false" CodeFile="NotaDebito.aspx.vb" Inherits="movimientos_NotaDebito"
    EnableEventValidation="false" %>

<%@ Register Src="~/usercontrol/menu_catalogos.ascx" TagPrefix="uc1" TagName="menu_catalogos" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%--<script src="../js/jquery.min.js"></script>--%>
    <%--<script src="../js/jquery-ui-1.12.1/jquery-ui.min.js"></script>--%>
    <link rel="stylesheet" href="../css/facturacion.css" />
    <link rel="stylesheet" href="../css/select2.css" />
    <link rel="stylesheet" href="../css/recibos.css" />
    <link rel="stylesheet" href="../css/nota-debito-styles.css" />
</asp:Content>

<asp:Content ID="c2" ContentPlaceHolderID="cpTitulo" runat="Server">
    <h4>Nota de Debito</h4>
</asp:Content>

<asp:Content ID="c3" ContentPlaceHolderID="cpUbicacion" runat="Server">
    <a href="../Default.aspx">Menu</a>
    <label>&gt;</label>
    <a href="NotaDebito.aspx">Nota de Debito</a>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CP1" runat="Server">
    <asp:ScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ScriptManager>


    <asp:UpdatePanel ID="upMensaje_popup" runat="server" UpdateMode="Conditional">

        <ContentTemplate>
            <asp:Literal ID="ltMensaje_popup" runat="server"></asp:Literal>
            <asp:Literal ID="ltMensaje" runat="server"></asp:Literal>
        </ContentTemplate>

        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>

    <div class="nd-grid-container">
        <div class="nd-flex-container nd-flex-start">
            <div>
                <label>Fecha</label>
                <asp:TextBox runat="server" ID="txtFecha" CssClass="SelectStyle numero"></asp:TextBox>
            </div>
        </div>




        <div class="nd-flex-container nd-flex-start">
            <div class="widget">
                <fieldset class="fd-container">
                    <legend>Cliente es:</legend>
                    <asp:RadioButton ID="rdbExterno" CssClass="" GroupName="CondicionPago" Text="Externo" runat="server" AutoPostBack="true" />
                    <asp:RadioButton ID="rdbInterno" CssClass="" GroupName="CondicionPago" Text="Interno" runat="server" AutoPostBack="true" />
                </fieldset>
            </div>

            <div>
                <label>Cliente</label>
                <asp:DropDownList runat="server" ID="ddlCliente" CssClass="SelectStyle SelectSearch"></asp:DropDownList>
            </div>
            <div>
                <label>Nota Débito No.</label>
                <asp:TextBox runat="server" ID="txtNotaDebito" CssClass="SelectStyle numero"></asp:TextBox>
            </div>

        </div>

        <div class="nd-flex-container nd-flex-start">
            <fieldset style="border: 1px solid black;">

                <div class="nd-flex-container">
                    <label>DEBITO A CUENTA $:</label>
                    <asp:TextBox runat="server" ID="txtMonto" CssClass="SelectStyle nd-input-numero  "></asp:TextBox>
                </div>


                <div>
                    <label>CONCEPTO:</label>
                    <asp:TextBox runat="server" ID="txtConcepto" CssClass="SelectStyle nd-input-textarea" BackColor="WhiteSmoke" Height="150px" Width="450px"></asp:TextBox>

                </div>


            </fieldset>

        </div>

        <div class="nd-flex-container nd-flex-start" style="width: 40%; justify-content: space-between !important">

            <div style="">
                <asp:Button runat="server" ID="btnGuardar" Text="Guardar" CssClass="btn-submit" />
            </div>

            <div>
                <asp:Button runat="server" ID="btnClear" Text="Eliminar" CssClass="btn-reset" />
            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cpScripts" runat="Server">
    <script type="text/javascript">

        $(document).Ready(function () {
            $('.SelectSearch').select2();
            $('radio').checkboxradio();





            var fecha = new Date();
            var dia = fecha.getDate();
            var mes = fecha.getMonth() + 1;
            var anio = fecha.getFullYear();
            var fechaActual = dia + "/" + mes + "/" + anio;
            $("#txtFecha").val(fechaActual);


        })
    </script>

</asp:Content>

