﻿<%@ Page Title="Clientes" Language="VB" EnableEventValidation="false" MasterPageFile="~/master/principal.master" AutoEventWireup="false" CodeFile="Clientes.aspx.vb" Inherits="catalogos_Clientes" %>

<%@ Register Src="~/usercontrol/menu_catalogos.ascx" TagPrefix="uc1" TagName="menu_catalogos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <%--    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" />
<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.16.0/umd/popper.min.js"></script>
<script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>--%>
    <link rel="stylesheet" href="../css/clientes.css" />


</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cpTitulo" runat="Server">
    <h1>Clientes</h1>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cpUbicacion" runat="Server">
    <a href="../Default.aspx">Escritorio</a>
    <label>&gt;</label>
    <a href="Clientes.aspx">Catálogo de Clientes</a>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContentPlaceHolder" runat="Server">
    <asp:ScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ScriptManager>

    <div id="main-form">
        <div id="main-form-content">
             

            <div id="main-form-content-field">

                <div id="Control">
                    <a id="btnNew" class="new" onclick="Nuevo_Click"><i class="fas fa-plus-circle"></i>
                        <label>Nuevo</label></a>

                    <asp:LinkButton ID="btnExportar" runat="server" CssClass="print" ToolTip="Exportar"><i class="fas fa-file-excel"></i><label>Exportar</label></asp:LinkButton>
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
                            <asp:Panel ID="Panel1" runat="server" CssClass="table-search" DefaultButton="btnBuscar">
                                <asp:TextBox ID="txtBuscar" runat="server" CssClass="search nolabel"></asp:TextBox>
                                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
                                <div class="clear"></div>
                            </asp:Panel>

                            <div class="clear"></div>

                            <asp:Literal ID="ltMensajeGrid" runat="server"></asp:Literal>

                            <asp:GridView ID="GridViewOne" runat="server" CssClass="table table-light table-sm table-striped table-hover table-bordered" CellPadding="0" GridLines="None" AllowPaging="True" AllowSorting="True" PageSize="10" DataKeyNames="CodigoCliente,Externo,CodigoEmpresa" AutoGenerateColumns="False">
                                <SelectedRowStyle CssClass="SelectedRowDelete" />

                                <Columns>
                                    <asp:BoundField HeaderText="Nombre/NombreComercial" DataField="Nombres" SortExpression="nombres" />
                                    <asp:BoundField HeaderText="Apellido" DataField="Apellidos" SortExpression="apellidos" />
                                    <asp:BoundField HeaderText="Numero de Identificación" DataField="NumeroIdentificacion" SortExpression="NumeroIdentificacion" />
                                    <asp:BoundField HeaderText="Vendedor" DataField="Vendedor" SortExpression="Vendedor" />
                                    <asp:BoundField HeaderText="Externo" DataField="Externo" SortExpression="Externo" />
                                    <asp:BoundField HeaderText="Excento Impuestos" DataField="ExcentoImpuestos" SortExpression="ExcentoImpuestos" />
                                    <asp:BoundField HeaderText="Activo" DataField="Activo" SortExpression="Activo" />


                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnEditar" runat="server" CommandArgument='<%#Eval("CodigoCliente") %>' CommandName="Edit" CssClass="btnEditar" ToolTip="Editar" >
                                                <i class="fas fa-edit"></i></asp:LinkButton>
                                            <asp:LinkButton ID="btnEliminar" runat="server" CommandArgument='<%#Eval("CodigoCliente") %>' CommandName="Delete" CssClass="btnEliminar" OnClientClick="return confirm('Desea eliminar?')" ToolTip="Eliminar" >
                                                <i class="fas fa-trash-alt"></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>




                                      <asp:TemplateField HeaderText="Vendedor">
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlVendedor" runat="server" AutoPostBack="True">
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Labelv" runat="server" Text='<%# Bind("Vendedor")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Excento Impuesto">
                                        <EditItemTemplate>
                                            <asp:CheckBox ID="CheckExCenImp" runat="server" AutoPostBack="True"></asp:CheckBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="LabelexceImp" runat="server" Text='<%# Bind("ExcentoImpuestos")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Externo">
                                        <EditItemTemplate>
                                            <asp:CheckBox ID="CheckExterno" runat="server" AutoPostBack="True"></asp:CheckBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Labelext" runat="server" Text='<%# Bind("Externo")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Activo">
                                        <EditItemTemplate>
                                            <asp:CheckBox ID="CheckActivo" runat="server" AutoPostBack="True"></asp:CheckBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Labelact" runat="server" Text='<%# Bind("Activo")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>



                                    <asp:CommandField
                                        HeaderText="Suprimir"
                                        ButtonType="Button"
                                        DeleteText="Eliminar"
                                        ShowDeleteButton="true"
                                        HeaderStyle-Width="120">
                                        <ControlStyle CssClass="btnEliminar" />
                                    </asp:CommandField>

                                    <asp:CommandField
                                        HeaderText="Editar"
                                        ButtonType="Button"
                                        SelectText="Actualizar"
                                        ShowSelectButton="true"
                                        HeaderStyle-Width="120">
                                        <ControlStyle CssClass="btnEditar" />
                                    </asp:CommandField>
                                </Columns>


                                    <pagertemplate>
                                        <div class="pagination">
                                                 <asp:Button ID="B1" runat="server" CommandName="Page" ToolTip="Prim. Pag" CommandArgument="First" CssClass="primero" Text="Primera" formnovalidate />
                                            <asp:Button ID="B2" runat="server" CommandName="Page" ToolTip="Pág. anterior" CommandArgument="Prev" CssClass="anterior" Text="&larr;" formnovalidate />
                                            <asp:Button ID="B3" runat="server" CommandName="Page" ToolTip="Sig. página" CommandArgument="Next" CssClass="siguiente" Text="&rarr;" formnovalidate />
                                            <asp:Button ID="B4" runat="server" CommandName="Page" ToolTip="Últ. Pag" CommandArgument="Last" CssClass="ultimo" Text="Ultima" formnovalidate />
                                            <asp:Label ID="CurrentPageLabel" runat="server" CssClass="PagerLabel" />
                                        </div>
                                    </pagertemplate>
                            </asp:GridView>


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

            <h2 class="center-color">Agregar/Editar Cliente</h2>

            <asp:UpdatePanel ID="upDesc" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Literal ID="ltMensajeBuscarCliente" runat="server"></asp:Literal>

                    <div class="cl-grid-container">

                        <div class="  ">
                            <label>CODIGO CLIENTE</label>
                            <asp:Panel ID="pnlCodigoCLiente" runat="server" DefaultButton="btnLoadClientes">
                                <asp:TextBox ID="TextCodCliente" runat="server" MaxLength="4"></asp:TextBox>
                                <asp:Button ID="btnLoadClientes" runat="server" Text="" class="hide" />
                            </asp:Panel>
                        </div>

                        <div class="  ">
                            <label>SECTOR DE MERCADO</label>
                            <asp:DropDownList ID="ddlMercado" runat="server" CssClass="SelectStyle" AutoPostBack="true" />
                            <ajaxToolkit:CascadingDropDown
                                ID="Cmercado"
                                runat="server"
                                TargetControlID="ddlMercado"
                                ServicePath="../services/WSCatProductos.asmx"
                                ServiceMethod="GetMercado"
                                Category="CategoryName"
                                PromptText="--SELECCIONE--"></ajaxToolkit:CascadingDropDown>
                        </div>

                        <div class=" " id="nombre">
                            <label>NOMBRE COMERCIAL</label>
                            <asp:TextBox ID="TextNombre" runat="server" MaxLength="50"></asp:TextBox>
                        </div>
                        <div class=" ">
                            <label id="lblApellido">APELLIDO</label>
                            <asp:TextBox ID="TextApellido" runat="server" MaxLength="30"></asp:TextBox>
                        </div>

                        <div class=" ">
                            <label>RAZÓN SOCIAL</label>
                            <asp:TextBox ID="TextRazonSoc" runat="server" MaxLength="50"></asp:TextBox>
                        </div>

                        <div class=" ">
                            <label>DIRECCIÓN</label>
                            <asp:TextBox ID="TextDirecc" runat="server" MaxLength="50"></asp:TextBox>
                        </div>

                        <div class=" ">
                            <label>TELÉFONO</label>
                            <asp:TextBox ID="TextTelf" runat="server" MaxLength="15"></asp:TextBox>
                        </div>
                        <div class=" ">
                            <label>CORREO ELECTRÓNICO</label>
                            <asp:TextBox ID="Textemail" runat="server" MaxLength="30"></asp:TextBox>
                        </div>

                        <div class="  ">
                            <label>Contacto</label>
                            <asp:TextBox ID="TextContacto" runat="server" MaxLength="30"></asp:TextBox>
                        </div>
                        <div class="  ">
                            <label>NÚMERO DE IDENTIFICACIÓN</label>
                            <asp:TextBox ID="TextCeduRuc" runat="server" MaxLength="30"></asp:TextBox>
                        </div>

                        <div class=" ">
                            <label>CUENTA CONTABLE</label>
                            <asp:TextBox ID="TextCtaCont" runat="server" MaxLength="25"></asp:TextBox>
                        </div>

                        <div class=" ">
                            <label>DÍAS DE CRÉDITO</label>
                            <asp:TextBox ID="TextDiasCred" runat="server" MaxLength="30" TextMode="Number"></asp:TextBox>
                        </div>


                        <div class=" ">
                            <label>LÍMITE DE CRÉDITO</label>
                            <asp:TextBox ID="TextLimtCred" runat="server" MaxLength="25" Text="0.00" onblur="formatNumber(this, 2); control_fill(this);" onkeypress="formatNumber(this, 2)" onfocus="control_clear(this)"></asp:TextBox>
                        </div>

                        <div class=" ">
                            <label>VENDEDOR</label>
                            <asp:DropDownList ID="ddlVendedor" runat="server" class="PantaDosElemIzq" CssClass="SelectStyle" />
                            <ajaxToolkit:CascadingDropDown
                                ID="CVendedor"
                                runat="server"
                                TargetControlID="ddlVendedor"
                                ServicePath="../services/WSCatProductos.asmx"
                                ServiceMethod="GetVendedor"
                                Category="CategoryName"
                                PromptText="Seleccione..."></ajaxToolkit:CascadingDropDown>
                        </div>

                    </div>

                    <div class="cl-flex">
                        <fieldset>

                            <div class="ui-controlgroup">
                                <asp:CheckBox ID="CheckPersonaJuridica" runat="server" Checked="true" Text="PERSONA JURIDICA" CssClass="cl-checkbox"></asp:CheckBox>
                                <asp:CheckBox ID="CheckActivo" runat="server" Checked="true" Text="ACTIVO" CssClass="cl-checkbox"></asp:CheckBox>
                                <asp:CheckBox ID="CheckExterno" runat="server" Checked="true" Text="EXTERNO" CssClass="cl-checkbox"></asp:CheckBox>
                                <asp:CheckBox ID="CheckExCenImp" runat="server" Checked="true" Text="EXCENTO DE IMPUESTOS" CssClass="cl-checkbox"></asp:CheckBox>
                                <asp:CheckBox ID="CheckDist" runat="server" Checked="true" Text="DISTRIBUIDOR" CssClass="cl-checkbox"></asp:CheckBox>
                            </div>
                        </fieldset>
                    </div>


                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="GridViewOne" EventName="SelectedIndexChanged" />
                </Triggers>
            </asp:UpdatePanel>



            <div class="btnGuarda" style="margin-top: 5px;">
                <asp:Button ID="BtnGuardar" runat="server" Text="Guardar" class="btnEditar" />

                <a class="btnEliminar" onclick="close_popup()">Cerrar</a>
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
            $("input[type='checkbox']").checkboxradio();
            $("fieldset").controlgroup();
            console.log("ready")
            //$('.datagird').basictable();
            console.log("HERE2")


            $("#btnNew").on("click", function () {
                console.log("HERE")


                $("#<%=hdfCodigo.ClientID%>").val("");

                <%--$("#<%=TextNombre.ClientID%>").val("");
                $("#<%=TextNombre.ClientID%>").prev().removeClass('visible');

                $("#<%=TextApellido.ClientID%>").val("");
                $("#<%=TextApellido.ClientID%>").prev().removeClass('visible');

                $("#<%=TextNombComer.ClientID%>").val("");
                 $("#<%=TextNombComer.ClientID%>").prev().removeClass('visible');

                $("#<%=TextRazonSoc.ClientID%>").val("");
                $("#<%=TextRazonSoc.ClientID%>").prev().removeClass('visible');

                $("#<%=TextDirecc.ClientID%>").val("");
                $("#<%=TextDirecc.ClientID%>").prev().removeClass('visible');

                $("#<%=TextTelf.ClientID%>").val("");
                $("#<%=TextTelf.ClientID%>").prev().removeClass('visible');

                $("#<%=Textemail.ClientID%>").val("");
                $("#<%=Textemail.ClientID%>").prev().removeClass('visible');

                $("#<%=TextContacto.ClientID%>").val("");
                $("#<%=TextContacto.ClientID%>").prev().removeClass('visible');

                $("#<%=TextCeduRuc.ClientID%>").val("");
                $("#<%=TextCeduRuc.ClientID%>").prev().removeClass('visible');

                $("#<%=TextCtaCont.ClientID%>").val("");
                $("#<%=TextCtaCont.ClientID%>").prev().removeClass('visible');

                $("#<%=CheckExterno.ClientID%>").val("");
                $("#<%=CheckExterno.ClientID%>").prev().removeClass('visible');

                $("#<%=CheckExCenImp.ClientID%>").val("");
                $("#<%=CheckExCenImp.ClientID%>").prev().removeClass('visible');

                $("#<%=CheckActivo.ClientID%>").val("");
                $("#<%=CheckActivo.ClientID%>").prev().removeClass('visible');

                $("#<%=CheckDist.ClientID%>").val("");
                $("#<%=CheckDist.ClientID%>").prev().removeClass('visible');

                $("#<%=ddlMercado.ClientID%>").val("");
                $("#<%=ddlMercado.ClientID%>").prev().removeClass('visible');

                $("#<%=TextDiasCred.ClientID%>").val("");
                $("#<%=TextDiasCred.ClientID%>").prev().removeClass('visible');

                $("#<%=TextLimtCred.ClientID%>").val("");
                $("#<%=TextLimtCred.ClientID%>").prev().removeClass('visible');

                $("#<%=ddlVendedor.ClientID%>").val("");
                $("#<%=ddlVendedor.ClientID%>").prev().removeClass('visible');

                $("#popuptittle").text('Clientes');--%>


                open_popup();
            });

            $(".Close").click(function () {
                $('#popup-form').bPopup().close();
            });


            $("checkbox").checkboxradio();
            $("fieldset").controlgroup();
            console.log("ready")

        });

        function open_popup() {
            console.log("HERE3")
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

        function select_mercado() {
            console.log("Script is run")
            $("#<%=ddlMercado.ClientID %>").change(function () {
                var selectedValue = $(this).val();

                console.log(selectedValue)

                if (selectedValue == "DISTRIBUIDOR" || selectedValue == "MAYORISTAS" || selectedValue == "DETALLE" || selectedValue == "MERCADO POPULAR" || selectedValue == "NANDAIME") {

                    $("#<%=TextApellido.ClientID%>").show()
                    console.log("Show")

                } else {

                    $("#<%=TextApellido.ClientID%>").hide()
                    console.log("Hide")

                }


            })
        }

        function responsive_grid() {
            console.log("HERE5")

            //$('.datagird').basictable();
        }

        function success_messege(msg) {
            console.log("HERE4")

            alertify.success(msg);

            $("#<%=TextNombre.ClientID%>").val("");
            $("#<%=TextApellido.ClientID%>").val("");
             <%--$("#<%=TextNombComer.ClientID%>").val("");--%>
            $("#<%=TextRazonSoc.ClientID%>").val("");
            $("#<%=TextDirecc.ClientID%>").val("");
            $("#<%=TextTelf.ClientID%>").val("");
            $("#<%=Textemail.ClientID%>").val("");
            $("#<%=TextContacto.ClientID%>").val("");
            $("#<%=TextCeduRuc.ClientID%>").val("");
            $("#<%=TextCtaCont.ClientID%>").val("");

            $("#<%=CheckActivo.ClientID%>").val("");
            $("#<%=CheckExterno.ClientID%>").val("");
            $("#<%=CheckExCenImp.ClientID%>").val("");
            $("#<%=CheckDist.ClientID%>").val("");
            $("#<%=ddlVendedor.ClientID%>").val("");
            $("#<%=ddlMercado.ClientID%>").val("");
        }
    </script>
</asp:Content>

