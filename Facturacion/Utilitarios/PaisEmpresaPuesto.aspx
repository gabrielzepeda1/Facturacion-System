<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PaisEmpresaPuesto.aspx.vb" Inherits="Utilitarios_PaisEmpresaPuesto" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%=MyUserName%></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <link href="<%= ResolveClientUrl("../img/favicon.png")%>" rel="shortcut icon" type="image/x-icon" />
    <link href="img/favicon.png" rel="shortcut icon" type="image/x-icon" />
    <link href="https://use.fontawesome.com/releases/v5.5.0/css/all.css" rel="stylesheet" />
    <%--<link href="~/css/sesion.css" rel="stylesheet" />--%>
    <link href="<%= ResolveClientUrl("../css/principal.css")%>" rel="stylesheet" />
    <link href="<%= ResolveClientUrl("../css/newStyles.css")%>" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-T3c6CoIi6uLrA9TneNEoa7RxnatzjcDSCmG1MXxSR1GAsXEV/Dwwykc2MPK8M2HN" crossorigin="anonymous" />
    <link rel="stylesheet" href="//cdn.jsdelivr.net/npm/alertifyjs@1.13.1/build/css/alertify.min.css" />
    <link rel="stylesheet" href="//cdn.jsdelivr.net/npm/alertifyjs@1.13.1/build/css/themes/bootstrap.min.css" />
</head>
<body class="bg-black">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ScriptManager>

        <div class="d-flex justify-content-center align-items-center flex-column" style="min-height: 100vh;">
            <div class="container-md bg-dark rounded p-3" style="max-width: 500px">
                <div class="row mb-3">
                    <div class="d-flex col-2 justify-content-start align-items-center">
                        <a id="homeButton" class="navbar-brand">
                            <img alt="<%= CompanyName%>" title="<%= CompanyName%>" class="img-fluid img-logo" width="100" src="<%= ResolveClientUrl("../img/logo.png")%>" /></a>
                    </div>

                    <div class="d-flex col-6 align-items-center text-white">
                        <a class="btn btn-warning mb-0"><%=MyUserName%></a>
                    </div>
                    <div class="d-flex col-4 align-items-center justify-content-end ">
                        <%--<a class="btn btn-success text-center align-middle" href="<%= ResolveClientUrl("../herramientas/usuario_editar.aspx")%>" role="button"><i class="fas fa-user"></i>&nbsp <%= MyUserName%></a>--%>
                        <asp:LinkButton ID="btnCerrar" CssClass="btn btn-danger" Text='<span class="fas fa-sign-out-alt"></span>' runat="server"></asp:LinkButton>
                    </div>
                </div>

                <div class="row mb-3 px-1 align-items-center">
                    <div class="col">
                        <label for="ddlPais" class="text-white">País</label>
                        <asp:DropDownList ID="ddlPais" runat="server" CssClass="form-select" />
                        <ajaxToolkit:CascadingDropDown
                            ID="CPais"
                            TargetControlID="ddlPais"
                            ServicePath="../services/WSCatProductos.asmx"
                            ServiceMethod="GetPaises"
                            Category="CategoryPais"
                            PromptText="Seleccione..."
                            runat="server"></ajaxToolkit:CascadingDropDown>
                    </div>
                </div>

                <div class="row mb-3 px-1 align-items-center">
                    <div class="col">
                        <label for="ddlPais" class="text-white">Empresa</label>
                        <asp:DropDownList ID="ddlEmpresa" runat="server" CssClass="form-select" />
                        <ajaxToolkit:CascadingDropDown
                            ID="CEmpresa"
                            runat="server"
                            TargetControlID="ddlEmpresa"
                            ServicePath="../services/WSCatProductos.asmx"
                            ServiceMethod="GetEmpresa"
                            ParentControlID="ddlPais"
                            Category="CategoryEmpresa"
                            PromptText="Seleccione..."></ajaxToolkit:CascadingDropDown>
                    </div>
                </div>

                <div class="row mb-3 px-1 align-items-center">
                    <div class="col">
                        <label for="ddlPuesto" class="text-white">Puesto</label>
                        <asp:DropDownList ID="ddlPuesto" runat="server" CssClass="form-select" />
                        <ajaxToolkit:CascadingDropDown
                            ID="CPuesto"
                            runat="server"
                            TargetControlID="ddlPuesto"
                            ServicePath="../services/WSCatProductos.asmx"
                            ServiceMethod="GetPuesto"
                            ParentControlID="ddlEmpresa"
                            Category="CategoryPuesto"
                            PromptText="Seleccione..."></ajaxToolkit:CascadingDropDown>
                    </div>
                </div>

                <div class="row mb-3 px-1 align-items-center">
                    <div class="col-12 d-flex justify-content-center">
                        <asp:LinkButton ID="btnSubmit" runat="server" Text='<i class="fas fa-sign-in-alt"></i> Iniciar Sesión' CssClass="btn btn-primary" />
                    </div>
                </div>
            </div>
        </div>

        <asp:Literal ID="ltMensajeGrid" runat="server"></asp:Literal>
        <%--<asp:GridView
            ID="GridViewOne"
            runat="server"
            CssClass="table table-light table-sm table-striped table-hover table-bordered"
            CellPadding="0"
            GridLines="None"
            AllowPaging="True"
            AllowSorting="True"
            PageSize="10"
            DataKeyNames="cod_pais,cod_empresa,cod_puesto,lista_predeterminada"
            AutoGenerateColumns="False"
            Visible="false">

            <HeaderStyle CssClass="table-header table-dark align-middle text-center" />
            <Columns>
                <asp:BoundField HeaderText="Pais" DataField="Pais" SortExpression="Pais" ReadOnly="true" />
                <asp:BoundField HeaderText="Empresa" DataField="Empresa" SortExpression="Empresa" />
                <asp:BoundField HeaderText="Puesto" HeaderStyle-Wrap="true" DataField="Puesto" SortExpression="DesCorta" />
                <asp:BoundField HeaderText="Codigo Pais" DataField="cod_Pais" SortExpression="cod_Pais" ReadOnly="true" Visible="false" />
                <asp:BoundField HeaderText="Codigo Empresa" DataField="cod_empresa" SortExpression="cod_Empresa" Visible="false" />
                <asp:BoundField HeaderText="Codigo Puesto" HeaderStyle-Wrap="true" DataField="cod_Puesto" SortExpression="cod_Puesto" Visible="false" />
                <asp:BoundField HeaderText="Modifica Precios" HeaderStyle-Wrap="true" DataField="cambiar_precio" SortExpression="cambiar_precio" />
                <asp:BoundField HeaderText="Sector de Mercado" HeaderStyle-Wrap="true" DataField="lista_predeterminada" SortExpression="lista_predeterminada" />
                <asp:BoundField HeaderText="Verifica Inventario" HeaderStyle-Wrap="true" DataField="verificar_inventario" SortExpression="verificar_inventario" />
                <asp:BoundField HeaderText="Porcentaje(%) de Impuestos" HeaderStyle-Wrap="true" DataField="PorcImtos" SortExpression="PorcImtos" />
                <asp:BoundField HeaderText="Identificación" HeaderStyle-Wrap="true" DataField="cedula_ruc" SortExpression="cedula_ruc" Visible="false" />
                <asp:BoundField HeaderText="Autorización MIFIN" HeaderStyle-Wrap="true" DataField="autorizacion_mifin" SortExpression="autorizacion_mifin" Visible="false" />
                <asp:BoundField HeaderText="Dirección" HeaderStyle-Wrap="true" DataField="direccion" SortExpression="direccion" Visible="false" />
                <asp:BoundField HeaderText="Teléfono" HeaderStyle-Wrap="true" DataField="telefono" SortExpression="telefono" Visible="false" />
                <asp:CommandField
                    ButtonType="Button"
                    EditText="Seleccionar"
                    ShowSelectButton="true"
                    HeaderStyle-Width="120">
                    <ControlStyle CssClass="btn btn-success align-middle" />
                </asp:CommandField>

            </Columns>
        </asp:GridView>--%>
    </form>

    <script src="<%= ResolveClientUrl("../js/jquery.min.js")%>" type="text/javascript"></script>
    <script src="~/js/jquery.placeholder.label.js" type="text/javascript"></script>
    <script src="<%= ResolveClientUrl("../js/jquery-ui-1.12.1/jquery-ui.js")%>" type="text/javascript"></script>
    <script src="<%= ResolveClientUrl("../js/myscript.js")%>" type="text/javascript"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-C6RzsynM9kWDrMNeT87bh95OGNyZPhcTNXj1NW7RuBCsyN/o0jlpcV8Qyq46cDfL" crossorigin="anonymous"></script>
    <script src="//cdn.jsdelivr.net/npm/alertifyjs@1.13.1/build/alertify.min.js"></script>

    <script src="<%= ResolveClientUrl("~/js/settings/alertifySettings.js") %>"></script>

    <script> 

        const btnSubmit = document.getElementById("<%= btnSubmit.ClientID %>");
        const btnCerrar = document.getElementById("<%= btnCerrar.ClientID %>");

        btnSubmit.addEventListener("click", function (event) {
            event.preventDefault();

            const ddlPais = document.getElementById("<%= ddlPais.ClientID %>");
            const ddlEmpresa = document.getElementById("<%= ddlEmpresa.ClientID %>");
            const ddlPuesto = document.getElementById("<%= ddlPuesto.ClientID %>");

            if (ddlPais.value === "") {
                alertify.alert('Debe seleccionar un país.');
            } else if (ddlEmpresa.value === "") {
                alertify.alert('Debe seleccionar una empresa.');
            } else if (ddlPuesto.value === "") {
                alertify.alert('Debe seleccionar un puesto.');
            } else {
                alertify.confirm(
                    '',
                    'Iniciar Sesión?',
                    function () {
                        __doPostBack("<%=btnSubmit.UniqueID%>", '');
                        alertify.success('Sesión Iniciada');
                    },
                    function () {
                        alertify.error('Cancelado');
                    });
            }

        });

        btnCerrar.addEventListener("click", (event) => {
            event.preventDefault();
            alertify.confirm(
                '',
                'Está seguro que desea cerrar sesión?',
                function () {
                    __doPostBack("<%=btnCerrar.UniqueID%>", '');
                    
                },
                function () {
                    alertify.error('Cancelado');
                });
        })
    </script>

</body>
</html>
