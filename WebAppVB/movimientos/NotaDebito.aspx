<%@ Page Title="" Language="VB" MasterPageFile="~/master/prinFactura.master" AutoEventWireup="false" Inherits="WebAppVB.movimientos_NotaDebito"
    EnableEventValidation="false" Codebehind="NotaDebito.aspx.vb" %>

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
        <div class="row d-flex justify-content-center">
            <div class="col-8">
                <div class="card shadow mb-4">
                    <div class="card-header py-6 bg-primary">
                        <h5 class="m-0 font-weight-bold text-white">Nota de Débito</h5>
                    </div>
                    <div class="card-body">
                        <div class="row mb-4">
                            <div class="col">
                                <div class="input-group">
                                    <span class="input-group-text"><i class="far fa-calendar"></i>&nbsp;Fecha</span>
                                    <asp:TextBox runat="server" ID="txtFecha" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col">
                                <div class="input-group">
                                    <span class="input-group-text"><i class="far fa-file-alt"></i>&nbsp;No. Nota Débito</span>
                                    <asp:TextBox runat="server" ID="txtNotaDebito" CssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="row mb-4 align-items-center">
                            <div class="col">
                                <div class="input-group">
                                    <span class="input-group-text">Cliente</span>
                                    <asp:DropDownList runat="server" ID="ddlCliente" CssClass="form-select"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="d-grid gap-2 col-6">
                                <div class="btn-group">
                                    <input type="radio" class="btn-check" name="options" id="rdbExterno" />
                                    <label class="btn btn-outline-secondary" for="rdbExterno">Externo</label>
                                    <input type="radio" class="btn-check" name="options" id="rdbInterno" />
                                    <label class="btn btn-outline-secondary" for="rdbInterno">Interno</label>
                                </div>
                                <%-- <asp:RadioButton ID="rdbExterno" CssClass="" GroupName="CondicionPago" Text="Externo" runat="server" AutoPostBack="true" />
                        <asp:RadioButton ID="rdbInterno" CssClass="" GroupName="CondicionPago" Text="Interno" runat="server" AutoPostBack="true" />--%>
                            </div>

                        </div>

                        <div class="row mb-4">
                            <div class="col-6">
                                <div class="input-group">
                                    <span class="input-group-text">Débito a Cuenta C$</span>
                                    <asp:TextBox runat="server" ID="txtMonto" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="row mb-2">
                            <div class="col">
                                <div class="input-group">
                                    <span class="input-group-text">Concepto</span>
                                    <asp:TextBox runat="server" ID="txtConcepto" CssClass="form-control" Style="height: 100px"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="row d-flex justify-content-end">
                            <div class="col d-grid gap-2 col-3">
                                <asp:Button runat="server" ID="btnGuardar" Text="Guardar" CssClass="btn btn-success" />
                                <asp:Button runat="server" ID="btnClear" Text="Limpiar" CssClass="btn btn-secondary" />
                            </div>
                        </div>

                    </div>
                </div>
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

