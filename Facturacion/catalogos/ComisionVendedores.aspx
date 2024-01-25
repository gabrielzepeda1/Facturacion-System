<%@ Page Title="Comisión Vendedores | Facturación" Language="VB" EnableEventValidation="false" MasterPageFile="~/master/principal.master" AutoEventWireup="false" CodeFile="ComisionVendedores.aspx.vb" Inherits="catalogos_ComisionVendedores" %>

<%@ Register Src="~/usercontrol/menu_catalogos.ascx" TagPrefix="uc1" TagName="menu_catalogos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cpTitulo" runat="Server">
    <h1>Catálogo de Comisión de Vendedores</h1>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cpUbicacion" runat="Server">
    <a href="../Default.aspx">Escritorio</a>
    <label>&gt;</label>
    <a href="ComisionVendedores.aspx">Catálogo de Comisión de Vendedores</a>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContentPlaceHolder" runat="Server">
    <%--<asp:ScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ScriptManager>--%>
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
                                    DataKeyNames="codigo,cod_pais,cod_empresa,cod_puesto,cod_familia"
                                    AutoGenerateColumns="False">


                                    <HeaderStyle CssClass="table-header table-dark align-middle text-center" />

                                    <Columns>
                                        <asp:BoundField HeaderText="Comisión" DataField="comision" DataFormatString='{0:F2}' SortExpression="comision" ItemStyle-CssClass="align-middle" />


                                        <asp:TemplateField HeaderText="Activo">
                                            <EditItemTemplate>
                                                <asp:CheckBox ID="CheckActivo" runat="server" AutoPostBack="True"></asp:CheckBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Labelact" runat="server" Text='<%# Bind("Activo")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="Vendedor">
                                            <EditItemTemplate>
                                                <%--<label>Sigla</label>--%>
                                                <asp:DropDownList ID="ddlVendedor" runat="server" AutoPostBack="True">
                                                </asp:DropDownList>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Labelv" runat="server" Text='<%# Bind("Vendedor")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Pais">
                                            <EditItemTemplate>
                                                <%--<label>Sigla</label>--%>
                                                <asp:DropDownList ID="ddlPais" runat="server" AutoPostBack="True">
                                                </asp:DropDownList>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Labelp" runat="server" Text='<%# Bind("Pais")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Empresa">
                                            <EditItemTemplate>
                                                <%--<label>Origen</label>--%>
                                                <asp:DropDownList ID="ddlEmpresa" runat="server">
                                                </asp:DropDownList>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Labele" runat="server" Text='<%# Bind("Empresa")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Puesto">
                                            <EditItemTemplate>
                                                <%--<label>Calidad</label>--%>
                                                <asp:DropDownList ID="ddlPuesto" runat="server">
                                                </asp:DropDownList>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Labelpu" runat="server" Text='<%# Bind("Puesto")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Familia">
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlFamilia" runat="server">
                                                </asp:DropDownList>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Labelf" runat="server" Text='<%# Bind("Familia")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:CommandField
                                            HeaderText="Suprimir"
                                            ButtonType="Button"
                                            DeleteText="Eliminar"
                                            ShowDeleteButton="true"
                                            HeaderStyle-Width="120">
                                            <ControlStyle CssClass="btn btn-success align-middle" />
                                        </asp:CommandField>



                                        <asp:CommandField
                                            HeaderText="Editar"
                                            ButtonType="Button"
                                            EditText="Actualizar"
                                            ShowEditButton="true"
                                            HeaderStyle-Width="120">
                                            <ControlStyle CssClass="btn btn-success align-middle" />
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
        <%--<div class="popup-scroll">--%>
        <div class="bstt-form">

            <h2 class="center-color">Agregar Comision a vendedor</h2>
            <i class="fas fa-times-circle Close"></i>

            <asp:UpdatePanel ID="upDesc" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <label class="vlabel">Vendedor</label>
                    <asp:DropDownList ID="ddlVendedor" runat="server" CssClass="marginb22" />
                    <ajaxToolkit:CascadingDropDown
                        ID="CVendedor"
                        runat="server"
                        TargetControlID="ddlVendedor"
                        ServicePath="../services/WSCatProductos.asmx"
                        ServiceMethod="GetVendedor"
                        Category="CategoryName"
                        PromptText="Seleccione..."></ajaxToolkit:CascadingDropDown>

                    <%--Text="Activo" class="vlabel"--%>
                    <label class="vlabel">Comision</label>
                    <asp:TextBox ID="TextComision" runat="server" Text='0.00'></asp:TextBox>
                    <%--onblur="formatNumber(this, 2); control_fill(this);" onkeypress="formatNumber(this, 2)" onfocus="control_clear(this)"></asp:TextBox>--%>
                    <ajaxToolkit:FilteredTextBoxExtender
                        ID="FTEGComision"
                        runat="server"
                        FilterType="Numbers, Custom"
                        ValidChars=".,"
                        TargetControlID="TextComision"></ajaxToolkit:FilteredTextBoxExtender>


                    <label class="vlabel">Activo</label>
                    <asp:CheckBox ID="CheckActivo" runat="server" Checked="true"></asp:CheckBox>


                    <label class="vlabel">Pais</label>
                    <asp:DropDownList ID="ddlPais" runat="server" CssClass="marginb22" />
                    <ajaxToolkit:CascadingDropDown
                        ID="CPais"
                        runat="server"
                        TargetControlID="ddlPais"
                        ServicePath="../services/WSCatProductos.asmx"
                        ServiceMethod="GetPaises"
                        Category="CategoryPais"
                        PromptText="Seleccione..."></ajaxToolkit:CascadingDropDown>

                    <label class="vlabel">Empresa</label>
                    <asp:DropDownList ID="ddlEmpresa" runat="server" CssClass="marginb22" />
                    <ajaxToolkit:CascadingDropDown
                        ID="CEmpresa"
                        runat="server"
                        TargetControlID="ddlEmpresa"
                        ServicePath="../services/WSCatProductos.asmx"
                        ServiceMethod="GetEmpresaXPais"
                        Category="CategoryEmpresa"
                        ParentControlID="ddlPais"
                        PromptText="Seleccione..."></ajaxToolkit:CascadingDropDown>

                    <label class="vlabel">Puesto</label>
                    <asp:DropDownList ID="ddlPuesto" runat="server" CssClass="marginb22" />
                    <ajaxToolkit:CascadingDropDown
                        ID="CPuesto"
                        runat="server"
                        TargetControlID="ddlPuesto"
                        ServicePath="../services/WSCatProductos.asmx"
                        ServiceMethod="GetPuestoXPaisXEmpresa"
                        Category="CategoryPuesto"
                        ParentControlID="ddlEmpresa"
                        PromptText="Seleccione..."></ajaxToolkit:CascadingDropDown>

                    <label class="vlabel">Familia</label>
                    <asp:DropDownList ID="ddlFamilia" runat="server" CssClass="marginb22" />
                    <ajaxToolkit:CascadingDropDown
                        ID="CFamilia"
                        runat="server"
                        TargetControlID="ddlFamilia"
                        ServicePath="../services/WSCatProductos.asmx"
                        ServiceMethod="GetFamilia"
                        Category="CategoryName"
                        PromptText="Seleccione..."></ajaxToolkit:CascadingDropDown>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="BtnGuardar" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>


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

                 $("#<%=TextComision.ClientID%>").val("");
                 $("#<%=TextComision.ClientID%>").prev().removeClass('visible');


                 $("#<%=CheckActivo.ClientID%>").val("");
                 $("#<%=CheckActivo.ClientID%>").prev().removeClass('visible');

                 $("#<%=ddlVendedor.ClientID%>").val("");
                 $("#<%=ddlVendedor.ClientID%>").prev().removeClass('visible');

                 $("#<%=ddlPais.ClientID%>").val("");
                 $("#<%=ddlPais.ClientID%>").prev().removeClass('visible');

                 $("#<%=ddlEmpresa.ClientID%>").val("");
                 $("#<%=ddlEmpresa.ClientID%>").prev().removeClass('visible');

                 $("#<%=ddlPuesto.ClientID%>").val("");
                 $("#<%=ddlPuesto.ClientID%>").prev().removeClass('visible');

                 $("#<%=ddlFamilia.ClientID%>").val("");
                 $("#<%=ddlFamilia.ClientID%>").prev().removeClass('visible');

                 $("#popuptittle").text('Agregar comision a los vendedores');


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

            $("#<%=TextComision.ClientID%>").val("");
             $("#<%=CheckActivo.ClientID%>").val("");
             $("#<%=ddlVendedor.ClientID%>").val("");
             $("#<%=ddlPais.ClientID%>").val("");
             $("#<%=ddlEmpresa.ClientID%>").val("");
             $("#<%=ddlPuesto.ClientID%>").val("");
             $("#<%=ddlFamilia.ClientID%>").val("");

        }
    </script>
</asp:Content>

