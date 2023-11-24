<%@ Page Title="Usuarios Paises/Empresas/Puestos" Language="VB" EnableEventValidation="false" MasterPageFile="~/master/principal.master" AutoEventWireup="false" CodeFile="AccesosUsuarios.aspx.vb" Inherits="Utilitarios_AccesosUsuarios" %>

<%@ Register Src="~/usercontrol/menu_catalogos.ascx" TagPrefix="uc1" TagName="menu_catalogos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cpTitulo" runat="Server">
    <h1>Accesos a Usuarios</h1>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cpUbicacion" runat="Server">
    <a href="../Default.aspx">Escritorio</a>
    <label>&gt;</label>
    <a href="AccesosUsuarios.aspx">Accesos a Usuarios</a>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="CP1" runat="Server">
    <asp:ScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ScriptManager>

    <div id="main-form">
            <div class="container-fluid">
                <div class="row mx-1 py-2 align-items-center justify-content-between">
                    <div class="col-8 d-flex">
                        <%--<a id="btnNuevo" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#crearAcceso"><i class="fas fa-plus-circle"></i>&nbsp Nuevo</a>--%>
                        <asp:LinkButton ID="btnNuevo" CssClass="btn btn-primary" data-bs-toggle="modal" data-bs-target="#crearAcceso" Text="Nuevo" runat="server" />
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

                    <asp:GridView
                        ID="GridViewOne"
                        runat="server"
                        CssClass="table table-light table-sm table-striped table-hover table-bordered"
                        CellPadding="0"
                        GridLines="None"
                        AllowPaging="True"
                        AllowSorting="True"
                        PageSize="10"
                        DataKeyNames="codigo,usuario,codigo_pais,cod_Empresa,cod_puesto"
                        AutoGenerateColumns="False">

                        <HeaderStyle CssClass="table-dark"></HeaderStyle>
                        <SelectedRowStyle CssClass="SelectedRowDelete" />

                        <Columns >
                            <asp:BoundField HeaderText="Codigo" DataField="Codigo" SortExpression="Codigo" ReadOnly="true" />
                            <asp:BoundField HeaderText="usuario" DataField="usuario" SortExpression="usuario" ReadOnly="true" />
                            <asp:BoundField HeaderText="Nombre" DataField="Nombre" SortExpression="Nombre" ReadOnly="true" />
                            <asp:BoundField HeaderText="Pais" DataField="Pais" SortExpression="Pais" ReadOnly="true" />
                            <asp:BoundField HeaderText="Empresa" DataField="Empresa" SortExpression="Empresa" ReadOnly="true" />
                            <asp:BoundField HeaderText="Puesto" DataField="Puesto" SortExpression="Puesto" ReadOnly="true" />

                            <asp:CommandField
                                HeaderText="Acciones"
                                ButtonType="Button"
                                DeleteText="Eliminar"
                                ShowDeleteButton="true"
                                HeaderStyle-Width="120">
                                <ControlStyle CssClass="btn btn-danger" />
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

                <asp:HiddenField ID="hdfCodigo" runat="server" />
                <asp:HiddenField ID="hdfPais" runat="server" />
                <asp:HiddenField ID="hdfEmpresa" runat="server" />
                <asp:HiddenField ID="hdfPuesto" runat="server" />

            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="BtnGuardar" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>

    </div>
    <!-- #main-form-content-field -->
    
    <div class="modal fade" id="crearAcceso" tabindex="-1" aria-labelledby="crearRolLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h1 id="popuptittle" class="modal-title fs-5">Acceso Nuevo</h1>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <label class="vlabel">Usuario</label>
                    <asp:DropDownList ID="ddlUsuario" runat="server" CssClass="form-select" />
                    <ajaxToolkit:CascadingDropDown
                        ID="CUsuario"
                        runat="server"
                        TargetControlID="ddlUsuario"
                        ServicePath="../services/WSCatProductos.asmx"
                        ServiceMethod="GetUsuario"
                        Category="CategoryName"
                        PromptText="Seleccione..."></ajaxToolkit:CascadingDropDown>

                    <label class="vlabel">Pais</label>
                    <asp:DropDownList ID="ddlPais" runat="server" CssClass="form-select" />
                    <ajaxToolkit:CascadingDropDown
                        ID="CPais"
                        runat="server"
                        TargetControlID="ddlPais"
                        ServicePath="../services/WSCatProductos.asmx"
                        ServiceMethod="GetPaises"
                        Category="CategoryPais"
                        PromptText="Seleccione..."></ajaxToolkit:CascadingDropDown>


                    <label class="vlabel">Empresa</label>
                    <asp:DropDownList ID="ddlEmpresa" runat="server" CssClass="form-select" />
                    <ajaxToolkit:CascadingDropDown
                        ID="CEmpresa"
                        runat="server"
                        TargetControlID="ddlEmpresa"
                        ServicePath="~/services/WSCatProductos.asmx"
                        ServiceMethod="GetEmpresaXPais"
                        Category="CategoryEmpresa"
                        ParentControlID="ddlPais"
                        PromptText="Seleccionar..."></ajaxToolkit:CascadingDropDown>

                    <label class="vlabel">Puesto</label>
                    <asp:DropDownList ID="ddlPuesto" runat="server" CssClass="form-select" />
                    <ajaxToolkit:CascadingDropDown
                        ID="CPuesto"
                        runat="server"
                        TargetControlID="ddlPuesto"
                        ServicePath="../services/WSCatProductos.asmx"
                        ServiceMethod="GetPuestoXPaisXEmpresa"
                        Category="CategoryPuesto"
                        ParentControlID="ddlEmpresa"
                        PromptText="Seleccione..."></ajaxToolkit:CascadingDropDown>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnGuardar" CssClass="btn btn-primary" data-bs-dismiss="modal" runat="server" Text="Guardar" />
                    <button type="button" class="btn btn-danger" data-bs-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>

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
        document.addEventListener("DOMContentLoaded",
            () => {

                const btnNuevo = document.getElementById("<%=btnNuevo.ClientID%>");
                const ddlUsuario = document.getElementById("<%=ddlUsuario.ClientID%>");
                const ddlPais = document.getElementById("<%=ddlPais.ClientID%>");
                const ddlEmpresa = document.getElementById("<%=ddlEmpresa.ClientID%>");
                const ddlPuesto = document.getElementById("<%=ddlPuesto.ClientID%>");

                btnNuevo.addEventListener("click",
                    () => {

                        ddlUsuario.value = -1;
                        ddlPais.value = -1;
                        ddlEmpresa.value = -1;
                        ddlPuesto.value = -1;


                    });


            }



        );

    </script>
</asp:Content>

