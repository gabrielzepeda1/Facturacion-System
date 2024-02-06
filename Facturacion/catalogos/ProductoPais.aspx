<%@ Page Title="Producto por País" Language="VB" EnableEventValidation="false" MasterPageFile="~/master/principal.master" AutoEventWireup="false" CodeFile="ProductoPais.aspx.vb" Inherits="catalogos_ProductoPais" %>

<%@ Register Src="~/usercontrol/menu_catalogos.ascx" TagPrefix="uc1" TagName="menu_catalogos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpTitulo" runat="Server">
    <h1>Catálogo de Productos por país</h1>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpUbicacion" runat="Server">
    <a href="../Default.aspx">Escritorio</a>
    <label>&gt;</label>
    <a href="ProductoPais.aspx">Catálogo de Productos por país</a>
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
                                    PageSize="10"
                                    DataKeyNames="codigo,cod_pais"
                                    AutoGenerateColumns="False">

                                    <HeaderStyle CssClass="table-header table-dark align-middle text-center" />

                                    <Columns>

                                        <asp:BoundField HeaderText="Codigo" DataField="Codigo" SortExpression="Codigo" ReadOnly="true" />
                                        <asp:BoundField HeaderText="Descripcion Maestro" DataField="producto" SortExpression="producto" />
                                        <asp:BoundField HeaderText="Descripcion al Imprimir" DataField="DescripImpresion" SortExpression="DescripImpresion" />
                                        <%--<asp:BoundField HeaderText="Numero" DataField="num_producto" SortExpression="num_producto"/>--%>

                                        <%--  <asp:TemplateField HeaderText="Pais">
                                           <EditItemTemplate>
                                              
                                                 <asp:DropDownList ID="ddlPais" runat="server"  AutoPostBack="True" >
                                                 </asp:DropDownList>
                                           </EditItemTemplate>
                                           <ItemTemplate>
                                                     <asp:Label ID="Labelp" runat="server" Text='<%# Bind("Pais")%>'></asp:Label>
                                           </ItemTemplate>
                                      </asp:TemplateField>--%>

                                        <asp:TemplateField HeaderText="Unidad Medida">
                                            <EditItemTemplate>
                                                <%--<label>Sigla</label>--%>
                                                <asp:DropDownList ID="ddlUnidadMed" runat="server" AutoPostBack="True">
                                                </asp:DropDownList>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="LabelUniM" runat="server" Text='<%# Bind("UnidadMedida")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="ExcentoImp">
                                            <EditItemTemplate>
                                                <asp:CheckBox ID="CheckImtos" runat="server" AutoPostBack="True"></asp:CheckBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="LabelImp" runat="server" Text='<%# Bind("excento_imp")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Activo">
                                            <EditItemTemplate>
                                                <asp:CheckBox ID="CheckAt" runat="server" AutoPostBack="True"></asp:CheckBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="LabelAc" runat="server" Text='<%# Bind("Activo")%>'></asp:Label>
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
                        <%--<asp:AsyncPostBackTrigger ControlID="BtnGuardar" EventName="Click" />--%>
                    </Triggers>
                </asp:UpdatePanel>


                <%--                <fieldset class="w700px">
                    <legend >
                        <asp:TextBox ID="TextCodProducto" runat="server" MaxLength="20" Height="25px" Width="1000px" BackColor="WhiteSmoke"></asp:TextBox> 
                   </legend>

                    <div>
                        <div class="izq">  
                            <label>Descripción Impresión</label>
                            <asp:TextBox ID="TextDecImpri" runat="server" MaxLength="100"></asp:TextBox>
                        </div>

                         <div class="Derecha">
                            <label>Unidad de Medida</label>
                            <asp:DropDownList ID="ddlUnidadMed" runat="server" CssClass="SelectStyle"/>
                            <ajaxToolkit:CascadingDropDown 
                                ID="CascadingDropDown2" 
                                runat="server" 
                                TargetControlID="ddlUnidadMed" 
                                ServicePath="../services/WSCatProductos.asmx"
                                ServiceMethod="GetUnMedida" 
                                Category="CategoryName"
                                PromptText="Seleccione...">
                            </ajaxToolkit:CascadingDropDown>
                        </div>

                        <div class="clear"></div>

                         <div class="izq">
                             <div class="check marginb22">
                                 <asp:CheckBox ID="CheckImtos" runat="server" Text="Excento de Impuesto"></asp:CheckBox>
                              </div>
                         </div>

                        <div class="Derecha">
                            <div class="check marginb22">
                                <asp:CheckBox ID="CheckAt" runat="server" Text="Activo"></asp:CheckBox>
                            </div>
                        </div>

                        <div class="clear"></div>
               
             <div class="btnGuarda" >
                  <div class="izq">
                      <asp:Button ID="BtnGuardar" runat="server" Text="Guardar"  class="btnEditar" />
                  </div>
               <%-- <div class="Derecha">
                      <asp:Button ID="BtnCerrar" runat="server" Text="Cerrar" Class="btnEliminar" />
               </div>
            </div> 
                   </div>
             </fieldset>   --%>



                <%-- inicio --%>
                <%-- <asp:UpdatePanel ID="upMensaje" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Literal ID="ltMensaje" runat="server"></asp:Literal>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="BtnGuardar" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>--%>
            </div>
            <!-- #main-form-content-field -->
            --%>
            <div class="clear"></div>

        </div>
        <!-- =========== #main-form-content ====================================================================== -->
    </div>
    <!-- =========== #main-form ====================================================================== -->

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
</asp:Content>

