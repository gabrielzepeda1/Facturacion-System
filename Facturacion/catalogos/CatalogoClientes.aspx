<%@ Page Title="" Language="C#" MasterPageFile="~/master/principal.master" AutoEventWireup="true" CodeFile="CatalogoClientes.aspx.cs" Inherits="catalogos_CatalogoClientes" EnableEventValidation="false" %>

<%@ Register Src="~/usercontrol/menu_catalogos.ascx" TagPrefix="uc1" TagName="menu_catalogos" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="../css/clientes.css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cpTitulo" runat="Server">
    <h1 class="text-white fs-5 text-uppercase py-2">Clientes</h1>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cpUbicacion" runat="Server">
    <a class="text-white fs-5 text-decoration-none" href="../Default.aspx">Escritorio</a>
    <label>&gt;</label>
    <a class="text-white fs-5 text-decoration-none" href="CatalogoClientes.aspx">Clientes</a>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContentPlaceHolder" runat="Server">
    <asp:ScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ScriptManager>

    <div id="main-form">
        <div id="main-form-content">
            <div id="main-form-content-field">

                <div id="Control">
                    <div class="container-fluid">
                        <div class="row mx-1 py-2 align-items-center justify-content-between">
                            <div class="col-8 d-flex">

                                <%--'Trigger--%>
                                <asp:LinkButton ID="btnNew" runat="server" CssClass="btn btn-primary btn-lg" ToolTip="Nuevo" OnClientClick="open_popup()"><i class="fas fa-plus-circle"></i> Nuevo </asp:LinkButton>

                                <asp:LinkButton ID="btnExportar" runat="server" CssClass="btn btn-success btn-lg" ToolTip="Exportar"><i class="fas fa-file-excel"></i> Exportar a Excel </asp:LinkButton>

                            </div>

                            <div class="col-4 d-flex">
                                <asp:Panel ID="PnlSearch" runat="server" CssClass="input-group" DefaultButton="btnBuscar">
                                    <asp:TextBox ID="txtBuscar" CssClass="form-control" ToolTip="Buscar..." runat="server"></asp:TextBox>
                                    <div class="input-group-append">
                                        <asp:LinkButton ID="btnBuscar" CssClass="btn btn-secondary" runat="server">Buscar&nbsp<i class="fas fa-search"></i></asp:LinkButton>
                                       
                                    </div>
                                    <%-- Trigger --%>
                                </asp:Panel>

                            </div>

                        </div>
                    </div>
                </div>




                <asp:UpdatePanel ID="UpMensaje" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Literal ID="LtMensaje" runat="server"></asp:Literal>
                    </ContentTemplate>
                    <Triggers>
                        <%--<asp:AsyncPostBackTrigger ControlID="BtnGuardar" EventName="Click" />--%>
                    </Triggers>
                </asp:UpdatePanel>

                <asp:UpdatePanel ID="UpGrid" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="table-content">


                            <div class="clear"></div>

                            <asp:Literal ID="LtMensajeGrid" runat="server"></asp:Literal>

                            <div class="table-responsive">
                            <asp:GridView ID="GVMain"
                                runat="server"
                                CssClass="table table-light table-sm table-striped table-hover table-bordered"
                                CellPadding="0"
                                GridLines="None"
                                AllowPaging="true"
                                AllowSorting="true"
                                PageSize="10"
                                DataKeyNames="CodigoCliente,Externo,CodigoEmpresa"
                                AutoGenerateColumns="false"
                                OnRowCommand="GVMain_RowCommand"
                                OnPageIndexChanging="GVMain_PageIndexChanging">

                                <HeaderStyle CssClass="table-header table-dark align-middle text-center" />
                                <SelectedRowStyle CssClass="table-primary" />
                                <Columns>
                                    <asp:BoundField HeaderStyle-CssClass="col" HeaderText="Codigo" DataField="CodigoCliente" SortExpression="CodigoCliente" Visible="true" />
                                    <asp:BoundField HeaderStyle-CssClass="col-2" HeaderText="Nombre /N.Comercial" DataField="Nombres" SortExpression="Nombres" />
                                    <asp:BoundField HeaderStyle-CssClass="col" HeaderText="Apellido" DataField="Apellidos" SortExpression="Apellidos" />
                                    <asp:BoundField HeaderStyle-CssClass="col" HeaderText="Numero de Identificación" DataField="NumeroIdentificacion" SortExpression="NumeroIdentificacion" />
                                    <asp:BoundField HeaderStyle-CssClass="col" HeaderText="Vendedor" DataField="CodigoVendedor" SortExpression="Vendedor" />
                                    <asp:BoundField HeaderStyle-CssClass="col" HeaderText="Externo" DataField="Externo" SortExpression="Externo" />
                                    <asp:BoundField HeaderStyle-CssClass="col" HeaderText="Excento Imp." DataField="ExcentoImpuestos" SortExpression="ExcentoImpuestos" />
                                    <asp:BoundField HeaderStyle-CssClass="col" HeaderText="Activo" DataField="Activo" SortExpression="Activo" />

                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnEditar" runat="server" CommandArgument='<%#Eval("CodigoCliente") %>' CommandName="MyEdit" CssClass="btn btn-success" ToolTip="Editar" OnClientClick="open_popup()">Editar</asp:LinkButton>

                                            <asp:LinkButton ID="btnEliminar" runat="server" CommandArgument='<%#Eval("CodigoCliente") %>' CommandName="Delete" CssClass="btn btn-danger" OnClick="btnEliminar_Click" OnClientClick="return confirm('Desea eliminar?')" ToolTip="Eliminar">Eliminar</asp:LinkButton>

                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>

                                <PagerTemplate>
                                    <div class="pagination">
                                        <asp:Button ID="B1" runat="server" CommandName="Page" ToolTip="Prim. Pag" CommandArgument="First" CssClass="primero" Text="Primera" formnovalidate />
                                        <asp:Button ID="B2" runat="server" CommandName="Page" ToolTip="Pág. anterior" CommandArgument="Prev" CssClass="anterior" Text="&larr;" formnovalidate />
                                        <asp:Button ID="B3" runat="server" CommandName="Page" ToolTip="Sig. página" CommandArgument="Next" CssClass="siguiente" Text="&rarr;" formnovalidate />
                                        <asp:Button ID="B4" runat="server" CommandName="Page" ToolTip="Últ. Pag" CommandArgument="Last" CssClass="ultimo" Text="Ultima" formnovalidate />
                                        <asp:Label ID="CurrentPageLabel" runat="server" CssClass="PagerLabel" />

                                        <div class="clear"></div>
                                    </div>
                                </PagerTemplate>
                            </asp:GridView>
                            </div>
                        </div>
                        <asp:HiddenField ID="hdfCodigo" runat="server" />

                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="BtnGuardar" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="GVMain" EventName="RowCommand" />
                        <asp:AsyncPostBackTrigger ControlID="GVMain" EventName="RowDeleting" />
                        <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="clear"></div>
        </div>
    </div>

    <div id="popup-form" class="white-popup-block mfp-hide">
        <div class="bstt-form PantallaPopUp">

            <asp:UpdatePanel ID="upDesc" runat="server" UpdateMode="Conditional">
                <ContentTemplate>

                    <i class="fas fa-times-circle Close"></i>

                    <h2 class="center-color">
                        <asp:Label ID="lblTitulo" runat="server">Nuevo Cliente</asp:Label></h2>

                    <div class="cl-grid-container">

                        <div class="form-floating">
                            <asp:TextBox ID="txtCodigoCliente" CssClass="form-control form-control-sm" runat="server" MaxLength="4" AutoPostBack="true"></asp:TextBox>
                            <label>CODIGO CLIENTE</label>
                        </div>

                        <div class="clear"></div>

                        <div class="form-floating" id="nombre">
                            <asp:TextBox ID="txtNombre" CssClass="form-control" runat="server" MaxLength="50" AutoPostBack="true"></asp:TextBox>
                            <label>NOMBRES/NOMBRE COMERCIAL</label>
                        </div>
                        <div class="form-floating">
                            <asp:TextBox ID="txtApellido" CssClass="form-control" runat="server" MaxLength="50" AutoPostBack="true"></asp:TextBox>
                            <asp:Label ID="lblApellido" runat="server" CssClass="">APELLIDOS</asp:Label>
                        </div>

                        <div class="form-floating">
                            <asp:TextBox ID="txtRazonSocial" CssClass="form-control" runat="server" MaxLength="50" AutoPostBack="true"></asp:TextBox>
                            <label>RAZÓN SOCIAL</label>
                        </div>

                        <div class="form-floating">
                            <asp:TextBox ID="txtDireccion" CssClass="form-control" runat="server" MaxLength="50" AutoPostBack="true"></asp:TextBox>
                            <label>DIRECCIÓN</label>
                        </div>

                        <div class="form-floating">
                            <asp:TextBox ID="txtCorreoElectronico" CssClass="form-control" runat="server" MaxLength="50" TextMode="Email" AutoPostBack="true"></asp:TextBox>
                            <label>CORREO ELECTRÓNICO</label>
                        </div>

                        <div class="form-floating">
                            <asp:TextBox ID="txtTelefono" CssClass="form-control" runat="server" MaxLength="15" TextMode="Phone" AutoPostBack="true"></asp:TextBox>
                            <label>TELÉFONO</label>
                        </div>

                        <div class="form-floating">
                            <asp:TextBox ID="txtIdentificacion" CssClass="form-control" runat="server" MaxLength="30" AutoPostBack="true"></asp:TextBox>
                            <label>NÚMERO DE IDENTIFICACIÓN</label>
                        </div>

                        <div class="form-floating">
                            <asp:TextBox ID="txtCuentaContable" CssClass="form-control" runat="server" MaxLength="50" TextMode="Number" AutoPostBack="true"></asp:TextBox>
                            <label>CUENTA CONTABLE</label>
                        </div>

                        <div class="form-floating">
                            <asp:TextBox ID="txtDiasCredito" CssClass="form-control" runat="server" MaxLength="30" TextMode="Number" AutoPostBack="true"></asp:TextBox>
                            <label>DÍAS DE CRÉDITO</label>
                        </div>


                        <div class="form-floating">
                            <asp:TextBox ID="txtLimiteCredito" CssClass="form-control" runat="server" MaxLength="25" Text="0.00" TextMode="Number" onblur="formatNumber(this, 2); control_fill(this);" onkeypress="formatNumber(this, 2)" onfocus="control_clear(this)" AutoPostBack="true"></asp:TextBox>
                            <label>LÍMITE DE CRÉDITO</label>
                        </div>

                        <div class="form-floating">
                            <asp:DropDownList ID="ddlMercado" CssClass="form-select" runat="server" AutoPostBack="true" />
                            <label for="ddlMercado">SECTOR DE MERCADO</label>
                            <asp:HiddenField runat="server" ID="hdfDdlMercado" />
                        </div>

                        <div class="form-floating">
                            <asp:DropDownList ID="ddlVendedor" runat="server" CssClass="form-select" AutoPostBack="true" />
                            <label for="ddlVendedor">VENDEDOR</label>
                            <asp:HiddenField runat="server" ID="hdfDdlVendedor" />

                        </div>

                    </div>

                    <div class="fleft">
                        <div class="">
                            <label>SELECCIONE</label>
                            <div class="cl-checkbox-flex">
                                <asp:CheckBox ID="chkActivo" runat="server" Text="ACTIVO" CssClass="btn-check"></asp:CheckBox>
                                <label class="btn btn-outline-primary" for="chkActivo">Activo</label>

                                <asp:CheckBox ID="chkExcentoImpuestos" runat="server" Text="EXCENTO DE IMPUESTOS" CssClass="btn btn-check"></asp:CheckBox>
                                <label class="btn btn-outline-primary">Excento de Impuestos</label>

                                <asp:CheckBox ID="chkExterno" runat="server" Text="EXTERNO" CssClass="btn btn-check"></asp:CheckBox>
                                <label class="btn btn-outline-primary">Externo</label>

                                <asp:CheckBox ID="chkDistribuidor" runat="server" Text="DISTRIBUIDOR" CssClass="btn btn-check"></asp:CheckBox>
                                <label class="btn btn-outline-primary">Distribuidor</label>

                                <asp:CheckBox ID="chkPersonaJuridica" runat="server" Text="PERSONA JURIDICA" CssClass="btn btn-check" OnCheckedChanged="chkPersonaJuridica_CheckedChanged" AutoPostBack="true"></asp:CheckBox>
                                <label class="btn btn-outline-primary">Persona Jurídica</label>

                            </div>
                        </div>
                    </div>

                    </div>

                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="GVMain" EventName="RowCommand" />
                    <asp:AsyncPostBackTrigger ControlID="btnNew" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="BtnGuardar" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="chkPersonaJuridica" EventName="CheckedChanged" />
                </Triggers>
            </asp:UpdatePanel>



            <div class="clear"></div>

            <div id="btnContainer">
                <asp:Button ID="BtnGuardar" runat="server" Text="Guardar" CssClass="btnEditar" OnClick="BtnGuardar_Click" AutoPostBack="true" />
                <asp:LinkButton ID="BtnCerrar" runat="server" Text="Cerrar" CssClass="btnEliminar" OnClientClick="close_popup()"></asp:LinkButton>
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

    <script type="text/javascript"> 

        $(document).ready(function () {
            console.log("DOM Ready...");

            //$("fieldset").controlgroup();
            //setCheckBox();
            responsive_grid();


        })

        function Clear() {
            console.log("Clear()..")
            $('#txtCodigoCliente').val('');
            $('#txtNombre').val('');
            $('#txtApellido').val('');
            $('#txtRazonSocial').val('');
            $('#txtDireccion').val('');
            $('#txtCorreoElectronico').val('');
            $('#txtTelefono').val('');
            $('#txtIdentificacion').val('');
            $('#txtCuentaContable').val('');
            $('#txtDiasCredito').val('');
            $('#txtLimiteCredito').val('');
            $('#ddlMercado').val('');
            $('#ddlVendedor').val('');
            $('#chkActivo').val('');
            $('#chkExcentoImpuestos').val('');
            $('#chkExterno').val('');
            $('#chkDistribuidor').val('');
            $('#chkPersonaJuridica').val('');
        }

        function hideApellidos() {

            const checkbox = document.getElementById("<%= chkPersonaJuridica.ClientID %>");

            if (checkbox.classList.contains("ui-checkboxradio-checked") = "true") {
                document.getElementById("lblApellido").style.display = "none";
                document.getElementById("<%= txtApellido.ClientID %>").style.display = "none";


            }
            else {
                document.getElementById("lblApellido").style.display = "block";
                document.getElementById("<%= txtApellido.ClientID %>").style.display = "block";

            }
        }

        function updateDdlMercado() {

            console.log("Updating ddlMercado..")
            let ddlMercado = document.getElementById("<%= ddlMercado.ClientID %>");
            let selectedIndex = document.getElementById("<%=hdfDdlMercado.ClientID %>").value;

            console.log(selectedIndex);
            console.log(typeof (selectedIndex));

            if (selectedIndex !== "") {
                ddlMercado.selectedIndex = parseInt(selectedIndex)

                console.log("Updated!")
            }

        }

        function updateDdlVendedor() {

            console.log("Updating ddlVendedor...")
            let ddlVendedor = document.getElementById("<%= ddlVendedor.ClientID %>");
            let selectedIndex = document.getElementById("<%=hdfDdlVendedor.ClientID %>").value;

            console.log(selectedIndex);
            console.log(typeof (selectedIndex))

            if (selectedIndex !== "") {
                ddlVendedor.selectedIndex = parseInt(selectedIndex);

                console.log("Updated!")
            }

        }

        function setCheckBox() {
            $("input[type='checkbox']").checkboxradio();
        }

        function open_popup() {
            console.log("open_popup()..")
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
            console.log("close_popup()..")
            $('#popup-form').bPopup().close();
        }

        function responsive_grid() {

            //$('.datagird').basictable();
        }





    </script>


</asp:Content>

