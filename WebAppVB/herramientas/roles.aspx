<%@ Page Title="Roles de Usuario | Facturación" Language="VB" EnableEventValidation="false" MasterPageFile="~/master/principal.master" AutoEventWireup="false" Inherits="WebAppVB.herramientas_roles" Codebehind="roles.aspx.vb" %>

<asp:Content ID="C1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="../css/permisos-menu.css" />
</asp:Content>

<asp:Content ID="C2" ContentPlaceHolderID="cpTitulo" runat="Server">
    <h1>Roles de Usuario</h1>
</asp:Content>

<asp:Content ID="C3" ContentPlaceHolderID="cpUbicacion" runat="Server">
    <a href="../Default.aspx">Escritorio</a>
    <label>&gt;</label>
    <a href="roles.aspx">Roles de Usuario</a>
</asp:Content>

<asp:Content ID="C4" ContentPlaceHolderID="MainContentPlaceHolder" runat="Server">

    <div class="container-fluid">
        <div class="row mt-2">

            <div class="col-lg-6 col-md-8 col-sm-10 mx-auto">
                <div class="card shadow mb-4">
                    <div class="card-header py-3 bg-dark">
                        <h6 class="m-0 font-weight-bold text-white">Permisos del Rol:</h6>
                    </div>
                    <div class="card-body">
                        <div class="row">
                            <div class="col-lg-6 col-md-6 col-sm-12 mb-3 mb-md-0">
                                <button id="btnNuevo" class="btn btn-primary w-100" type="button" data-bs-toggle="modal" data-bs-target="#crearRol">Nuevo</button>
                            </div>
                            <div class="col-lg-6 col-md-6 col-sm-12 mb-3 mb-md-0">
                                <asp:LinkButton Visible="False" CssClass="btn btn-danger w-100" ID="btnDelete" Text="Eliminar" runat="server"></asp:LinkButton>
                            </div>
                        </div>

                        <div class="row mt-3">
                            <div class="col">
                                <asp:DropDownList ID="ddlRol" CssClass="form-select" runat="server" AutoPostBack="true" />
                            </div>
                        </div>

                        <hr />

                        <div class="row">
                            <div class="col">
                                <asp:UpdatePanel ID="upMenuPermisos" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:TreeView ID="trvMenu" runat="server" ImageSet="Arrows" NodeIndent="15" ShowCheckBoxes="Leaf">
                                            <HoverNodeStyle ForeColor="#6666AA" />
                                            <NodeStyle Font-Names="Verdana" Font-Size="12pt" ForeColor="Black" HorizontalPadding="4px" NodeSpacing="0px" VerticalPadding="2px" />
                                            <ParentNodeStyle CssClass="fw-bold" />
                                            <SelectedNodeStyle BackColor="#B5B5B5" Font-Underline="False" HorizontalPadding="0px" VerticalPadding="0px" />
                                        </asp:TreeView>

                                        <asp:HiddenField ID="hdfCodigo" runat="server" />
                                        <asp:HiddenField ID="hdfSelectedNodes" runat="server" />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlRol" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>

                    <div class="card-footer">
                        <div class="row d-grid m-1 col-3">
                            <asp:Button ID="btnGuardarCambios" runat="server" CssClass="btn btn-success" Text="Guardar Cambios" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <%-- Modal --%>
    <div class="modal fade" id="crearRol" tabindex="-1" aria-labelledby="crearRolLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h3 class="modal-title fs-5">Nuevo Rol de Usuario</h3>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <label class="form-label">Nombre: </label>
                    <asp:TextBox ID="txtRol" CssClass="form-control" runat="server" AutoCompleteType="None"></asp:TextBox>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnGuardar" CssClass="btn btn-primary" runat="server" Text="Guardar" />
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>

    <asp:UpdateProgress ID="uprGrid" runat="server">
        <ProgressTemplate>
            <div class="loader spinner">
                <div>
                    <i class="fa-solid fa-rotate spinning"></i>
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>

<asp:Content ID="C5" ContentPlaceHolderID="cpScripts" runat="Server">
    <script>

        const txtRol = document.getElementById("<%=ddlRol.ClientID%>");
        const btnGuardar = document.getElementById("<%=btnGuardar.ClientID%>");
        const btnDelete = document.getElementById("<%=btnDelete.ClientID%>");
        const hdfCodigo = document.getElementById("<%=hdfCodigo.ClientID%>");
        const ddlRol = document.getElementById("<%=ddlRol.ClientID%>");
        const trvMenu = "<%=trvMenu.ClientID%>";

        const checkBoxPostBack = () => {

            //Se llama esta funcion primero, antes de que se realice un postback con los cambios realizados en los permisos del rol. 
            //Esta funcion recorre los checkboxes hijos de cada TreeNode padre, si encuentra que todos los hijos estan checked, entonces el padre tambien se checkea. 
            //Tambien, si el padre esta checkeado, entonces todos los hijos se checkean.
            checkParentNode();

            //Esta funcion permite que se haga un postback en cada cambio de cualquier checkbox del trvMenu, para guardar los cambios a los permisos del rol seleccionado. 

            const checkBoxes = document.querySelectorAll(`[id*=${trvMenu}] input[type="checkbox"]`);

            checkBoxes.forEach((item) => {

                item.addEventListener('change', (e) => {

                    //Si el usuario hace click en un checkbox, hace un postback.  
                    if (e.target.tagName == "INPUT" && e.target.type == "checkbox") {

                        console.log('Checkbox Postback');
                        __doPostBack(e.target.id)

                    }
                })
            })

            const checkBoxAnchors = document.querySelectorAll(`[id*=${trvMenu}] input[type="checkbox"] + a`);

            checkBoxAnchors.forEach((anchor) => {
                //Para evitar el evento SelectedNodeChanged, se usa la function preventDefault en cada TreeNode, para evitar que se dispare ese evento y no interferir con el TreeNodeCheckChanged.  
                anchor.addEventListener('click', (e) => {
                    console.log('SelectedNodeChanged stopped')
                    e.preventDefault();
                })
            })
        }

        const checkParentNode = () => {

            //This function gets all the checkboxes in a treeview and: 
            //If the changed checkbox is a parent. It selects or deselects all child boxes. 
            //Else if, all child checkboxes are checked on the change event, then the parent checkbox is also checked. 

            const checkBoxes = document.querySelectorAll(`[id*=${trvMenu}] input[type="checkbox"]`);

            checkBoxes.forEach((checkBox) => {

                checkBox.addEventListener('change', () => {

                    const table = checkBox.closest('table');

                    //Check all child checkboxes if parent checkbox isChecked. 
                    if (table.nextElementSibling && table.nextElementSibling.tagName === 'DIV') {

                        const divContainsChildNodes = table.nextElementSibling;
                        const parentCheckboxIsChecked = checkBox.checked;

                        Array.from(divContainsChildNodes.querySelectorAll('input[type=checkbox]'))
                            .forEach((childCheckbox) => childCheckbox.checked = parentCheckboxIsChecked);

                    } else {

                        //Check PARENT checkbox if ALL child checkboxes are checked. 
                        const childCheckboxParentDiv = checkBox.closest('DIV');

                        const parentCheckbox = childCheckboxParentDiv.
                            previousElementSibling
                            .querySelectorAll('input[type=checkbox]')
                            .item(0);

                        const childCheckboxesArr = Array.from(childCheckboxParentDiv.querySelectorAll('input[type=checkbox]'));

                        const checkedChildCountArr = childCheckboxesArr.filter((childCheckbox) => childCheckbox.checked).length;

                        console.log(childCheckboxesArr.length)
                        console.log(checkedChildCountArr)

                        parentCheckbox.checked =
                            (checkedChildCountArr === childCheckboxesArr.length)

                    }
                });
            });

        }

        const checkParentCheckBoxOnLoad = () => {

            //Esta funcion se ubica al final del sub LoadTreeview, verifica si hay checkboxes padre que deberian estar checked o no. en cada ddlRol_SelectedIndexChanged 

            console.log('Checking parent checkboxes..')

            const checkBoxes = document.querySelectorAll(`[id*=${trvMenu}] input[type="checkbox"]`);

            checkBoxes.forEach((checkBox) => {

                const table = checkBox.closest('table');

                if (table.nextElementSibling && table.nextElementSibling.tagName === 'DIV') {

                    const divContainsChildNodes = table.nextElementSibling;

                    const childCheckboxes = Array.from(divContainsChildNodes
                        .querySelectorAll('input[type=checkbox]'));

                    const allChildrenChecked = childCheckboxes.every((childCheckbox) => childCheckbox.checked);

                    checkBox.checked = allChildrenChecked ? true : false
                }
            });

        }

        btnGuardar.addEventListener("click",
            (e) => {
                e.preventDefault();

                if (txtRol.value === "") {
                    alertify.alert('Nuevo Rol', 'Debe ingresar un nombre para el nuevo rol');
                } else {
                    __doPostBack("<%=btnGuardar.UniqueID%>");
                }

            });

      <%--  btnDelete.addEventListener("click",
            (e) => {
                e.preventDefault();

                alertify.confirm(
                    'Eliminar Rol de Usuario',
                    '¿Está seguro de eliminar el rol seleccionado?',
                    () => {
                        __doPostback("<%=btnDelete.UniqueID%>");
                    },
                    () => {

                    }
                );

            });--%>


    </script>
</asp:Content>

