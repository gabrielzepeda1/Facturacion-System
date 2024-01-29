<%@ Page Title="Monedas" Language="VB" EnableEventValidation="false" MasterPageFile="~/master/principal.master" AutoEventWireup="false" CodeFile="Monedas.aspx.vb" Inherits="catalogos_Monedas" %>

<%@ Register Src="~/usercontrol/menu_catalogos.ascx" TagPrefix="uc1" TagName="menu_catalogos" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpTitulo" runat="Server">
    <h1>Catálogo de Monedas</h1>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpUbicacion" runat="Server">
    <a href="../Default.aspx">Escritorio</a>
    <label>&gt;</label>
    <a href="Monedas.aspx">Catálogo de Monedas</a>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MainContentPlaceHolder" runat="Server">
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
                        <%--<asp:AsyncPostBackTrigger ControlID="BtnGuardar" EventName="Click" />--%>
                        <asp:AsyncPostBackTrigger ControlID="btnNew" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>

                <asp:UpdatePanel ID="upGrid" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>

                        <div class="table-content">


                            <div class="clear"></div>

                            <asp:Literal ID="ltMensajeGrid" runat="server"></asp:Literal>

                            <div class="table-responsive">

                                <asp:GridView
                                    ID="GridViewOne"
                                    runat="server"
                                    CssClass="table table-light table-sm table-striped table-hover table-bordered"
                                    CellPadding="0"
                                    GridLines="None"
                                    AllowPaging="True"
                                    AllowSorting="True"
                                    PageSize="5"
                                    DataKeyNames="codigoMoneda,codPais"
                                    AutoGenerateColumns="False">


                                    <HeaderStyle CssClass="table-header table-dark align-middle text-center" />

                                    <Columns>
                                        <asp:BoundField HeaderText="Codigo Moneda" DataField="codigoMoneda" SortExpression="codigoMoneda" ItemStyle-CssClass="align-middle" />
                                        <asp:BoundField HeaderText="Nombre" DataField="Descripcion" SortExpression="Descripcion" ItemStyle-CssClass="align-middle" />
                                        <asp:BoundField HeaderText="Simbolo Moneda" DataField="simbolo_moneda" SortExpression="simbolo_moneda" ItemStyle-CssClass="align-middle" />


                                        <asp:TemplateField HeaderText="Pais" ItemStyle-CssClass="align-middle">
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlPais" runat="server" AutoPostBack="True">
                                                </asp:DropDownList>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Labelp" runat="server" Text='<%# Bind("Pais")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:CommandField
                                            HeaderText="Editar"
                                            ButtonType="Button"
                                            EditText="Actualizar"
                                            ShowEditButton="true"
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
                        <%--<asp:AsyncPostBackTrigger ControlID="btnNew" EventName="Click" />--%>
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
        <%--<div class="popup-scroll">--%>
        <%--   <div class="bstt-form">--%>
        <div class="bstt-form PantallaPopUp">
            <h2 class="center-color">Agregar Monedas</h2>
            <i class="fas fa-times-circle Close"></i>


            <%--Text="Activo" class="vlabel"--%>
            <asp:UpdatePanel ID="upDesc" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="PantaDosElemIzq">
                        <label>Codigo</label>
                        <asp:TextBox ID="TextCodigo" runat="server" MaxLength="20"></asp:TextBox>
                    </div>
                    <div class="PantaDosElemDerec">
                        <label>Descripcion de Moneda</label>
                        <asp:TextBox ID="txtDescripcion" runat="server" MaxLength="100"></asp:TextBox>
                    </div>

                    <div class="PantaDosElemIzq">
                        <label>Simbolo de Moneda</label>
                        <asp:TextBox ID="TextSimbolo" runat="server" MaxLength="100"></asp:TextBox>
                    </div>

                    <div class="PantaDosElemDerec">
                        <label>Pais</label>
                        <asp:DropDownList ID="ddlPais" runat="server" CssClass="marginb22" />
                        <ajaxToolkit:CascadingDropDown
                            ID="CPais"
                            runat="server"
                            TargetControlID="ddlPais"
                            ServicePath="../services/WSCatProductos.asmx"
                            ServiceMethod="GetPaises"
                            Category="CategoryName"
                            PromptText="Seleccione..."></ajaxToolkit:CascadingDropDown>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnNew" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>

            <div class="clear"></div>

            <div class="btnGuarda">
                <div class="izq">
                    <asp:Button ID="BtnGuardar" runat="server" Text="Guardar" class="btnEditar" />
                </div>
                <div class="Derecha">
                    <asp:Button ID="BtnCerrar" runat="server" Text="Cerrar" Class="btnEliminar" />
                </div>
            </div>

            <%-- <div class="form-btn">
               <asp:Button ID="BtnGuardar" runat="server" Text="Guardar"/>
            </div>--%>
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

                var btn = document.getElementById("<%=btnNew.ClientID%>");
                btn.click();


                $("#<%=hdfCodigo.ClientID%>").val("");

                $("#<%=txtDescripcion.ClientID%>").val("");
                $("#<%=txtDescripcion.ClientID%>").prev().removeClass('visible');


                $("#<%=TextSimbolo.ClientID%>").val("");
                $("#<%=TextSimbolo.ClientID%>").prev().removeClass('visible');


                $("#<%=ddlPais.ClientID%>").val("");
                $("#<%=ddlPais.ClientID%>").prev().removeClass('visible');


                $("#popuptittle").text('Agregar Monedas');


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
             //$("#<%=TextCodigo.ClientID%>").val("");
             $("#<%=txtDescripcion.ClientID%>").val("");
             $("#<%=TextSimbolo.ClientID%>").val("");
             $("#<%=ddlPais.ClientID%>").val("");
             var btn = document.getElementById("<%=btnNew.ClientID%>");
            btn.click();

        }
    </script>
</asp:Content>

