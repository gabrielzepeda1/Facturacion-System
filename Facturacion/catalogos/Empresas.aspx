<%@ Page Title="Catalogo de Empresas | Facturación" Language="VB" EnableEventValidation="false" MasterPageFile="~/master/principal.master" AutoEventWireup="false" CodeFile="Empresas.aspx.vb" Inherits="catalogos_Empresas" %>

<%@ Register Src="~/usercontrol/menu_catalogos.ascx" TagPrefix="uc1" TagName="menu_catalogos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .DisplayDesc {
            width: 500px;
            word-break: break-all;
        }

        .DisplayDiv {
            width: 200px;
            OVERFLOW: hidden;
            TEXT-OVERFLOW: ellipsis;
        }

        .left {
            float: left;
            margin-left: 5px
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpTitulo" runat="Server">
    <h1>Catálogo de Empresas</h1>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpUbicacion" runat="Server">
    <a href="../Default.aspx">Escritorio</a>
    <label>&gt;</label>
    <a href="Empresas.aspx">Catálogo de Empresas</a>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CP1" runat="Server">
    <asp:ScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ScriptManager>

    <div id="main-form">
        <div id="main-form-content">

            <div id="main-form-content-field">

                <div id="Control">
                    <div class="container-fluid">
                        <div class="row mx-1 py-2 align-items-center justify-content-between">
                            <div class="col-8 d-flex">

                                <a id="btnNew" class="btn btn-primary btn-lg">
                                    <i class="fas fa-plus-circle"></i>&nbsp Nuevo</a>

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
                        <asp:AsyncPostBackTrigger ControlID="BtnGuardar" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>

                <asp:UpdatePanel ID="upGrid" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>

                        <div class="table-content">


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
                                    PageSize="10"
                                    DataKeyNames="codigo"
                                    AutoGenerateColumns="False">

                                    <HeaderStyle CssClass="table-header table-dark align-middle text-center" />

                                    <Columns>
                                        <asp:TemplateField HeaderText="Codigo" ItemStyle-CssClass="align-middle">
                                            <EditItemTemplate>

                                                <asp:TextBox ID="txtCodigo" runat="server" Style="width: 80px;" Text='<%#Bind("Codigo")%>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Labelco" runat="server" Text='<%# Bind("Codigo")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Empresa" ItemStyle-CssClass="align-middle">
                                            <EditItemTemplate>

                                                <asp:TextBox ID="TextEmpresa" runat="server" Text='<%#Bind("Empresa") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="vLabel3" runat="server" Text='<%# Bind("Empresa")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Pais" ItemStyle-CssClass="align-middle">
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlPais" runat="server" AutoPostBack="True">
                                                </asp:DropDownList>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("Pais")%>'></asp:Label>
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

            <h2 class="center-color">Agregar/Editar Empresa</h2>



            <asp:UpdatePanel ID="upDesc" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="PantaDosElemIzq">
                        <label>Codigo</label>
                        <asp:TextBox ID="txtCodigo" runat="server" MaxLength="100"></asp:TextBox>
                    </div>
                    <div class="PantaDosElemDerec">
                        <label>Descripcion de Empresa</label>
                        <asp:TextBox ID="TextEmpresa" runat="server" MaxLength="100"></asp:TextBox>
                    </div>
                    <div class="PantaDosElemIzq">
                        <label>Descripcion de Impuesto</label>
                        <asp:TextBox ID="TextImpuesto" runat="server" MaxLength="100"></asp:TextBox>
                    </div>
                    <div class="PantaDosElemDerec">
                        <label>Porcentaje de Impuesto</label>
                        <asp:TextBox ID="TextPorcImpuesto" runat="server" MaxLength="100"></asp:TextBox>
                    </div>
                    <div class="PantaDosElemIzq">
                        <label>Cedula o Ruc</label>
                        <asp:TextBox ID="TextCedulaRuc" runat="server" MaxLength="100"></asp:TextBox>
                    </div>
                    <div class="PantaDosElemDerec">
                        <label>Descripcion Corta</label>
                        <asp:TextBox ID="TextDescripCorta" runat="server" MaxLength="100"></asp:TextBox>
                    </div>

                    <div class="PantaDosElemIzq">
                        <label>Direccion</label>
                        <asp:TextBox ID="TextDireccion" runat="server" MaxLength="100"></asp:TextBox>
                    </div>
                    <div class="PantaDosElemDerec">
                        <label>Autorizacion del Mifin</label>
                        <asp:TextBox ID="TextAutorizMifin" runat="server" MaxLength="100"></asp:TextBox>
                    </div>
                    <div class="PantaDosElemIzq">
                        <label>Pais</label>
                        <asp:DropDownList ID="ddlPais" runat="server" CssClass="PantaDosElemIzq" />
                        <ajaxToolkit:CascadingDropDown
                            ID="Cpais"
                            runat="server"
                            TargetControlID="ddlPais"
                            ServicePath="../services/WSCatProductos.asmx"
                            ServiceMethod="GetPaises"
                            Category="CategoryName"></ajaxToolkit:CascadingDropDown>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="GridViewOne" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="BtnGuardar" EventName="Click" />
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

                $("#<%=hdfCodigo.ClientID%>").val("");

                  $("#<%=TextEmpresa.ClientID%>").val("");
                  $("#<%=TextEmpresa.ClientID%>").prev().removeClass('visible');

                  $("#<%=TextImpuesto.ClientID%>").val("");
                  $("#<%=TextImpuesto.ClientID%>").prev().removeClass('visible');

                  $("#<%=TextPorcImpuesto.ClientID%>").val("");
                  $("#<%=TextPorcImpuesto.ClientID%>").prev().removeClass('visible');

                  $("#<%=TextDescripCorta.ClientID%>").val("");
                  $("#<%=TextDescripCorta.ClientID%>").prev().removeClass('visible');

                  $("#<%=TextCedulaRuc.ClientID%>").val("");
                  $("#<%=TextCedulaRuc.ClientID%>").prev().removeClass('visible');

                  $("#<%=TextDireccion.ClientID%>").val("");
                  $("#<%=TextDireccion.ClientID%>").prev().removeClass('visible');

                  $("#<%=TextAutorizMifin.ClientID%>").val("");
                  $("#<%=TextAutorizMifin.ClientID%>").prev().removeClass('visible');

                  $("#popuptittle").text('Agregar Empresa');

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
                transitionClose: 'slideBack'
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

            $("#<%=TextEmpresa.ClientID%>").val("");
              $("#<%=TextImpuesto.ClientID%>").val("");
              $("#<%=TextPorcImpuesto.ClientID%>").val("");
              $("#<%=TextDescripCorta.ClientID%>").val("");
              $("#<%=TextCedulaRuc.ClientID%>").val("");
              $("#<%=TextDireccion.ClientID%>").val("");
              $("#<%=TextAutorizMifin.ClientID%>").val("");
        }
    </script>
</asp:Content>

