<%@ Page Title="Usuarios del Sistema | Facturación" EnableEventValidation="false" Language="VB" MasterPageFile="~/master/principal.master" AutoEventWireup="false" CodeFile="usuarios.aspx.vb" Inherits="herramientas_usuarios" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>


<asp:Content ID="c1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="../css/newStyles.css" />
</asp:Content>

<asp:Content ID="c2" ContentPlaceHolderID="cpTitulo" runat="Server">
    <h1>Usuarios</h1>
</asp:Content>

<asp:Content ID="c3" ContentPlaceHolderID="cpUbicacion" runat="Server">
    <a href="../Default.aspx">Escritorio</a>
    <label>&gt;</label>
    <a href="usuarios.aspx">Usuarios del Sistema</a>
</asp:Content>

<asp:Content ID="c4" ContentPlaceHolderID="MainContentPlaceHolder" runat="Server">

    <asp:ScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ScriptManager>

    <div id="main-form">
        <div id="main-form-content">
            <%--<uc1:menu_tools runat="server" ID="menu_tools" />--%>
            <div id="main-form-content-field">


                <div id="Control">
                    <%--<asp:LinkButton ID="btnNuevo" CssClass="btn btn-primary" Text="Nuevo"  runat="server"></asp:LinkButton>--%>
                    <button id="btnNuevo" class="btn btn-primary" type="button" data-bs-toggle="modal" data-bs-target="#staticBackdrop">Nuevo</button>
                    <button id="btnPassword" class="btn btn-warning" type="button">Cambiar Contraseña</button>
                    <asp:LinkButton CssClass="btn btn-danger" ID="btnDelete" OnClientClick="deleteValidation(event)" Text="Eliminar" runat="server"></asp:LinkButton>
                </div>

                <asp:UpdatePanel ID="upMensaje" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Literal ID="ltMensaje" runat="server"></asp:Literal>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>

                <asp:UpdatePanel ID="upTable" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>

                        <asp:Literal ID="ltMensajeGrid" runat="server"></asp:Literal>

                        <div class="table-content">
                            <asp:GridView
                                ID="GridViewOne"
                                runat="server"
                                CssClass="table table-light table-sm table-striped table-hover table-bordered"
                                CellPadding="0"
                                CellSpacing="0"
                                GridLines="none"
                                AllowPaging="True"
                                AllowSorting="true"
                                PageSize="11"
                                DataKeyNames=""
                                AutoGenerateColumns="False">

                                <HeaderStyle CssClass="table-header table-dark align-middle text-center" />

                                <Columns>
                                    <asp:BoundField HeaderText="CODIGO" DataField="CODIGO" ItemStyle-CssClass="align-middle" Visible="true" />
                                    <asp:BoundField HeaderText="USUARIO" DataField="USUARIO" ItemStyle-CssClass="align-middle" />
                                    <asp:BoundField HeaderText="NOMBRE COMPLETO" DataField="NOMBRE" ItemStyle-CssClass="align-middle" />
                                    <asp:BoundField HeaderText="CORREO" DataField="CORREO" ItemStyle-CssClass="align-middle" />
                                    <asp:BoundField HeaderText="ROL" DataField="Rol" />
                                    <asp:BoundField HeaderText="ACTIVO" DataField="ACTIVO" ItemStyle-CssClass="align-middle" />
                                    <asp:BoundField HeaderText="FECHA REGISTRADO" DataField="FECHA" ItemStyle-CssClass="align-middle" />
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

                            <asp:HiddenField ID="hdfCodigo" runat="server" />
                            <asp:HiddenField ID="hdfUsuario" runat="server" />
                            <asp:HiddenField ID="hdfDelete" runat="server" />
                        </div>

                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="GridViewOne" EventName="SelectedIndexChanged" />

                    </Triggers>
                </asp:UpdatePanel>

                <div class="clear"></div>
            </div>
        </div>
    </div>


    <div class="modal fade popup-container" id="staticBackdrop" data-bs-backdrop="static" data-bs-keyboard="false">
        <div class="modal-dialog modal-dialog-centered modal-xl">
            <div class="modal-content">
                <div class="modal-header">
                    <h2 class="modal-title fs-5 fw-bold text-black" id="staticBackdropLabel">Nuevo Usuario</h2>
                </div>

                <div id="modal-body">
                    <div class="container-fluid my-2 px-3">

                        <div id="modal-row1" class="row mb-1">
                            <div class="col-4">
                                <label>Nombre de Usuario</label>
                                <asp:TextBox CssClass="form-control" ID="txtNombreUsuario" data-bs-toggle="popover" data-bs-trigger="focus" data-bs-placement="top" data-bs-content="El nombre de usuario debe tener un máximo de 30 caracteres. No se permiten espacios ni caracteres especiales." runat="server" AutoCompleteType="None" MaxLength="30" required></asp:TextBox>
                            </div>

                            <div class="col-4 ">
                                <label>Contraseña</label>
                                <asp:TextBox CssClass="form-control" ID="txtPassword" data-bs-toggle="popover" data-bs-trigger="focus" data-bs-placement="top" data-bs-content="La contraseña debe contener de 8 a 30 caracteres, debe llevar mayúsculas, minúsculas, al menos un número y un carácter especial." runat="server" TextMode="Password" AutoCompleteType="None" MaxLength="30" required></asp:TextBox>
                            </div>

                            <div class="col-4">
                                <label>Confirmar Contraseña</label>
                                <asp:TextBox CssClass="form-control" ID="txtConfirmarPassword" runat="server" TextMode="Password" AutoCompleteType="None" MaxLength="30" required></asp:TextBox>
                            </div>
                        </div>

                        <div id="modal-row2" class="row mb-2">
                            <div class="col-4">
                                <label>Nombres</label>
                                <asp:TextBox CssClass="form-control" ID="txtNombre" runat="server" AutoCompleteType="None" MaxLength="50"></asp:TextBox>
                            </div>

                            <div class="col-4">
                                <label>Apellidos</label>
                                <asp:TextBox CssClass="form-control" ID="txtApellido" runat="server" AutoCompleteType="None" MaxLength="50"></asp:TextBox>
                            </div>

                            <div class="col-4">
                                <label>Correo Electrónico</label>
                                <asp:TextBox CssClass="form-control" ID="txtCorreo" runat="server" AutoCompleteType="None" MaxLength="100" TextMode="Email"></asp:TextBox>
                            </div>
                        </div>

                        <div id="modal-row3" class="row mb-2">
                            <div class="col-3">
                                <label>Cédula</label>
                                <asp:TextBox CssClass="form-control" ID="txtCedula" runat="server" AutoCompleteType="None" MaxLength="50"></asp:TextBox>
                            </div>

                            <div class="col-3">
                                <label>Teléfono</label>
                                <asp:TextBox CssClass="form-control" ID="txtTelefono" runat="server" AutoCompleteType="None" MaxLength="50"></asp:TextBox>
                            </div>

                            <div class="col-3">
                                <label>Dirección</label>
                                <asp:TextBox CssClass="form-control" ID="txtDireccion" runat="server" AutoCompleteType="None" MaxLength="100"></asp:TextBox>
                            </div>

                            <div class="col-3">
                                <label>Rol</label>
                                <asp:DropDownList CssClass="form-select" ID="ddlRol" runat="server" AutoCompleteType="None" MaxLength="100"></asp:DropDownList>
                                <ajaxToolkit:CascadingDropDown
                                    ID="CRol"
                                    runat="server"
                                    TargetControlID="ddlRol"
                                    ServicePath="../services/WSCatProductos.asmx"
                                    ServiceMethod="GetRol"
                                    Category="CategoryRol"
                                    PromptText="Seleccione..."></ajaxToolkit:CascadingDropDown>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="modal-footer">
                    <asp:Button CssClass="btn btn-primary" ID="btnGuardar" data-bs-dismiss="modal" runat="server" Text="Guardar" />
                    <asp:LinkButton CssClass="btn btn-danger" ID="btnCerrar" data-bs-dismiss="modal" runat="server" Text="Cerrar" />
                </div>

            </div>
        </div>
    </div>

    <asp:UpdateProgress ID="uprLoad" runat="server">
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

<asp:Content ID="c5" ContentPlaceHolderID="cpScripts" runat="Server">
    <script src="../js/usuarios.js"></script>
    <script>

        document.addEventListener("DOMContentLoaded", () => {
            console.log("DOMContentLoaded");

            const txtNombreUsuario = document.getElementById("<%=txtNombreUsuario.ClientID%>");
            const txtPassword = document.getElementById("<%=txtPassword.ClientID%>");
            const txtConfirmarPassword = document.getElementById("<%=txtConfirmarPassword.ClientID%>");
            const txtNombre = document.getElementById("<%=txtNombre.ClientID%>");
            const txtApellido = document.getElementById("<%=txtApellido.ClientID%>");
            const txtCorreo = document.getElementById("<%=txtCorreo.ClientID%>");
            const txtCedula = document.getElementById("<%=txtCedula.ClientID%>");
            const txtTelefono = document.getElementById("<%=txtTelefono.ClientID%>");
            const txtDireccion = document.getElementById("<%=txtDireccion.ClientID%>");
            const ddlRol = document.getElementById("<%=ddlRol.ClientID%>");
            const btnNuevo = document.getElementById("btnNuevo");
            const btnPassword = document.getElementById("btnPassword");

            btnNuevo.addEventListener("click", () => {
                const hdfCodigo = document.getElementById("<%=hdfCodigo.ClientID%>");
                const hdfUsuario = document.getElementById("<%=hdfUsuario.ClientID%>");

                const txtNombreUsuario = document.getElementById("<%=txtNombreUsuario.ClientID%>");

                hdfCodigo.value = 0;
                hdfUsuario.value = "";

                console.log("btnNuevo Click");
                console.log("hdfUsuario.value:", hdfUsuario.value);

                document.getElementById("staticBackdropLabel").textContent = "Nuevo Usuario";
                document.getElementById("modal-row2").style.display = 'flex';
                document.getElementById("modal-row3").style.display = 'flex';

                /* btnNuevo.click();*/

                if (hdfUsuario.value === "" && hdfCodigo.value === "0") {

                    txtNombreUsuario.value = hdfUsuario.value;
                    txtNombreUsuario.disabled = false;

                    txtPassword.value = "";
                    txtPassword.disabled = false;

                    txtConfirmarPassword.value = "";
                    txtConfirmarPassword.disabled = false;

                    txtNombre.value = "";
                    txtNombre.disabled = false;

                    txtApellido.value = "";
                    txtApellido.disabled = false;

                    txtCorreo.value = "";
                    txtCorreo.disabled = false;

                    txtCedula.value = "";
                    txtCedula.disabled = false;

                    txtTelefono.value = "";
                    txtTelefono.disabled = false;

                    txtDireccion.value = "";
                    txtDireccion.disabled = false;

                    ddlRol.disabled = false;
                    ddlRol.SelectedIndex = 0;

                    if (document.querySelector(".table-success")) {
                        document.querySelector(".table-success").className = "";
                    }
                }
                return true;
            });

            btnPassword.addEventListener("click", () => {
                const hdfCodigo = document.getElementById("<%=hdfCodigo.ClientID%>");
                const hdfUsuario = document.getElementById("<%=hdfUsuario.ClientID%>");

                const txtNombreUsuario = document.getElementById("<%=txtNombreUsuario.ClientID%>");

                console.log("btnPassword Click");
                console.log("hdfUsuario.value:", hdfUsuario.value);
                console.log("hdfCodigo.value:", hdfCodigo.value);

                if (hdfUsuario.value === "" && hdfCodigo.value === "0") {
                    alertify.alert('Seleccione un usuario haciendo click en la tabla.');
                    return false;
                } else {
                    //Hacer que el btnPassword pueda abrir el mismo modal que el btnNuevo
                    btnPassword.setAttribute("data-bs-toggle", "modal");
                    btnPassword.setAttribute("data-bs-target", "#staticBackdrop");
                    document.getElementById("staticBackdropLabel").textContent = "Cambiar Contraseña";
                    document.getElementById("modal-row2").style.display = 'none';
                    document.getElementById("modal-row3").style.display = 'none';

                    txtNombreUsuario.value = hdfUsuario.value;
                    txtNombreUsuario.disabled = true;

                    txtPassword.value = "";
                    txtPassword.disabled = false;

                    txtConfirmarPassword.value = "";
                    txtConfirmarPassword.disabled = false;

                    //Abrir popup 
                    btnPassword.click();

                    btnPassword.removeAttribute("data-bs-toggle");
                    btnPassword.removeAttribute("data-bs-target");

                    return true;
                };
            });

            const popoverTriggerList = document.querySelectorAll('[data-bs-toggle="popover"]');
            const popoverList = [...popoverTriggerList].map(popoverTriggerEl => new bootstrap.Popover(popoverTriggerEl));
        });
    </script>

    <script>    
        function deleteValidation(e) {
            const hdfCodigo = document.getElementById("<%=hdfCodigo.ClientID%>");
            const hdfUsuario = document.getElementById("<%=hdfUsuario.ClientID%>");
            const hdfDelete = document.getElementById("<%=hdfDelete.ClientID%>"); //Variable para validar si se puede eliminar el usuario

            e.preventDefault();

            if (hdfCodigo.value !== "" && hdfUsuario.value !== "") {
                alertify.confirm(`Eliminar usuario: ${hdfUsuario.value}`, `¿Desea dar de baja al usuario: ${hdfUsuario.value}?`,
                    () => {
                        //User clicked OK, allow postback 
                        hdfDelete.value = true;
                        __doPostBack("<%=btnDelete.UniqueID%>", "");
                        return true;
                    },
                    () => {
                        alertify.confirm().close();
                        hdfDelete.value = false;
                        e.preventDefault();
                        console.log("Cancelado");
                        return false;
                    }
                );

            } else if (hdfCodigo.value === "" || hdfUsuario.value === "") {
                e.preventDefault();
                alertify.alert('Usuario no seleccionado.', 'Seleccione un usuario de la tabla.');
                hdfDelete.value = false;
                return false;

            } else {
                hdfDelete.value = false; //No se puede eliminar el usuario
                e.preventDefault();
                console.log("Cancelado2");
                return false;
            }
        };
    </script>


</asp:Content>

