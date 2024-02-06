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
    <a href="AccesosUsuarios.aspx">Accesos de Usuarios</a>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContentPlaceHolder" runat="Server">
    <div id="wrapper">
        <div class="container-fluid">
            <div class="card">
                <div class="card-header py-6 bg-dark">
                    <h6 class="m-0 font-weight-bold text-white">Accesos de Usuarios</h6>
                </div>

                <div class="card-body">

                    <div class="row mx-1 py-2 align-items-center justify-content-between">
                        <div class="col-8 d-flex">
                            <div class="">
                                <asp:LinkButton ID="btnNuevo" CssClass="btn btn-primary" data-bs-toggle="modal" data-bs-target="#staticBackdrop" Text="Nuevo" runat="server" />
                                <asp:LinkButton ID="btnExportar" runat="server" CssClass="btn btn-secondary" ToolTip="Exportar"><i class="fas fa-file-excel"></i> Exportar</asp:LinkButton>
                            </div>
                        </div>

                        <div class="col-4 d-flex">
                            <div class="input-group">
                                <div>
                                    <asp:TextBox ID="txtSearch" CssClass="form-control" runat="server"></asp:TextBox>
                                    <label class="form-label">Buscar</label>
                                </div>
                                <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-primary" ToolTip="Buscar">
                     <i class="fas fa-search"></i>
                                </asp:LinkButton>
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

                                    <Columns>
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
            </div>
        </div>
    </div>


    <%-- MODAL --%>
    <div class="modal fade" id="staticBackdrop" tabindex="-1" aria-labelledby="crearRolLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h1 id="popuptittle" class="modal-title fs-5">Crear Nuevo Acceso</h1>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col">

                            <label class="form-label">Usuario</label>
                            <asp:DropDownList ID="ddlUsuario" runat="server" CssClass="form-select" />
                            <ajaxToolkit:CascadingDropDown
                                ID="CUsuario"
                                runat="server"
                                TargetControlID="ddlUsuario"
                                ServicePath="../services/WSCatProductos.asmx"
                                ServiceMethod="GetUsuario"
                                Category="CategoryName"
                                PromptText="Seleccione..."></ajaxToolkit:CascadingDropDown>

                            <hr />

                            <label class="form-label">Pais</label>
                            <asp:DropDownList ID="ddlPais" runat="server" CssClass="form-select" />
                            <ajaxToolkit:CascadingDropDown
                                ID="CPais"
                                runat="server"
                                TargetControlID="ddlPais"
                                ServicePath="../services/WSCatProductos.asmx"
                                ServiceMethod="GetPaises"
                                Category="CategoryPais"
                                PromptText="Seleccione País"></ajaxToolkit:CascadingDropDown>

                            <label class="form-label">Empresa</label>
                            <asp:DropDownList ID="ddlEmpresa" runat="server" CssClass="form-select" />
                            <ajaxToolkit:CascadingDropDown
                                ID="CEmpresa"
                                runat="server"
                                TargetControlID="ddlEmpresa"
                                ServicePath="../services/WSCatProductos.asmx"
                                ServiceMethod="GetEmpresas"
                                Category="CategoryEmpresa"
                                ParentControlID="ddlPais"
                                PromptText="Seleccione Empresa"></ajaxToolkit:CascadingDropDown>

                            <label class="form-label">Puesto</label>
                            <asp:DropDownList ID="ddlPuesto" runat="server" CssClass="form-select" />
                            <ajaxToolkit:CascadingDropDown
                                ID="CPuesto"
                                runat="server"
                                TargetControlID="ddlPuesto"
                                ServicePath="../services/WSCatProductos.asmx"
                                ServiceMethod="GetPuestos"
                                Category="CategoryPuesto"
                                ParentControlID="ddlEmpresa"
                                PromptText="Seleccione Puesto"></ajaxToolkit:CascadingDropDown>
                        </div>
                    </div>
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

        const GV = document.getElementById("<%=GridViewOne.ClientID%>");


        document.addEventListener("DOMContentLoaded",
            () => {

                //$(GV).basicTable({
                //    breakpoint: 768
                //});

                let table = new DataTable("<%=GridViewOne.ClientID%>");

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

