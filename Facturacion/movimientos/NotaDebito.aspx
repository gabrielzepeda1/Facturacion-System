<%@ Page Title="" Language="VB" MasterPageFile="~/master/prinFactura.master" AutoEventWireup="false" CodeFile="NotaDebito.aspx.vb" Inherits="movimientos_NotaDebito"
    EnableEventValidation="false" %>

<%@ Register Src="~/usercontrol/menu_catalogos.ascx" TagPrefix="uc1" TagName="menu_catalogos" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="c2" ContentPlaceHolderID="cpTitulo" runat="Server">
    <h4>Nota de Debito</h4>
</asp:Content>

<asp:Content ID="c3" ContentPlaceHolderID="cpUbicacion" runat="Server">
    <a href="../Default.aspx">Menu</a>
    <label>&gt;</label>
    <a href="NotaDebito.aspx">Nota de Debito</a>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MainContentPlaceHolder" runat="Server">
    <asp:UpdatePanel ID="upMensaje_popup" runat="server" UpdateMode="Conditional">

        <ContentTemplate>
            <asp:Literal ID="ltMensaje_popup" runat="server"></asp:Literal>
            <asp:Literal ID="ltMensaje" runat="server"></asp:Literal>
        </ContentTemplate>

        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>

    <div class="container-fluid">
        <div class="row">
            <div class="col-4">
                <div class="card shadow mb-4">
                    <div class="card-header"></div>
                    <div class="card-body">
                        <div class="row">
                            <div class="col">
                                <label class="form-label">Fecha</label>
                                <asp:TextBox runat="server" ID="txtFecha" CssClass="form-select"></asp:TextBox>
                            </div>
                            <div class="col">
                                <label>Nota Débito No.</label>
                                <asp:TextBox runat="server" ID="txtNotaDebito" CssClass="SelectStyle numero"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                </div>
            </div>

            <div class="col-4">
                <div class="card shadow mb-4">
                    <div class="card-header"></div>
                    <div class="card-body">
                        <div class="row">
                            <div class="btn-group">
                                <div class="form-check">
                                    <asp:RadioButton ID="rdbExterno" CssClass="" GroupName="CondicionPago" Text="Externo" runat="server" AutoPostBack="true" />
                                    <asp:RadioButton ID="rdbInterno" CssClass="" GroupName="CondicionPago" Text="Interno" runat="server" AutoPostBack="true" />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col">
                                <label>Cliente</label>
                                <asp:DropDownList runat="server" ID="ddlCliente" CssClass="SelectStyle SelectSearch"></asp:DropDownList>
                            </div>

                        </div>
                    </div>
                </div>
            </div>

        </div>

        <div class="row">
            <div class="col-6">
                <div class="card shadow mb-4">
                    <div class="card-header"></div>
                    <div class="card-body">
                        <div class="row">
                            <div class="col">

                                <label class="form-label">DEBITO A CUENTA $:</label>
                                <asp:TextBox runat="server" ID="txtMonto" CssClass="form-control"></asp:TextBox>
                            </div>

                            <div class="col">
                                <label class="form-label">CONCEPTO:</label>
                                <asp:TextBox runat="server" ID="txtConcepto" CssClass="form-control" BackColor="WhiteSmoke"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="col">
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

