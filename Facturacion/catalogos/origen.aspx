<%@ Page Title="Catálogo de Origen | Facturación" EnableEventValidation="false" Language="VB" MasterPageFile="~/master/principal.master" AutoEventWireup="false" CodeFile="origen.aspx.vb" Inherits="catalogos_origen" %>
<%@ Register Src="~/usercontrol/menu_catalogos.ascx" TagPrefix="uc1" TagName="menu_catalogos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="c2" ContentPlaceHolderID="cpTitulo" Runat="Server">
    <h1>Catálogo de Origen</h1>
</asp:Content>

<asp:Content ID="c3" ContentPlaceHolderID="cpUbicacion" Runat="Server">
    <a href="../Default.aspx">Escritorio</a>
    <label>&gt;</label>
    <a href="origen.aspx">Catálogo de Origen</a>
</asp:Content>

<asp:Content ID="c4" ContentPlaceHolderID="MainContentPlaceHolder" Runat="Server">
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
                                AllowSorting ="True"
                                PageSize="10" 
                                DataKeyNames="codigo" 
                                AutoGenerateColumns="False">

                                <HeaderStyle CssClass="table-header table-dark align-middle text-center" />

                                <Columns>
                                    <asp:BoundField HeaderText="Codigo" DataField="codigo" SortExpression="codigo" ReadOnly="true" ItemStyle-CssClass="align-middle" />
                                    <asp:BoundField HeaderText="Nombre" DataField="Descripcion" SortExpression="Descripcion" ItemStyle-CssClass="align-middle" />
                                    
                                    <asp:CommandField 
                                        HeaderText="Editar"
                                        ButtonType="Button" 
                                        EditText="Actualizar"
                                        ShowEditButton="true" 
                                        HeaderStyle-Width="120" > 
                                        <ControlStyle CssClass="btn btn-success align-middle" />
                                    </asp:CommandField>
                                    <asp:CommandField 
                                        HeaderText="Suprimir"
                                        ButtonType="Button" 
                                        DeleteText="Eliminar" 
                                        ShowDeleteButton="true"
                                        HeaderStyle-Width="120" > 
                                        <ControlStyle CssClass="btn btn-danger align-middle" />
                                    </asp:CommandField>

                                </Columns>


                                <PagerTemplate>
                                    <div class="pagination">
                                        <asp:Button ID="B1" runat="server" CommandName="Page" ToolTip="Prim. Pag"  CommandArgument="First" CssClass="primero" Text="Primera"  formnovalidate/>
                                        <asp:Button ID="B2" runat="server" CommandName="Page" ToolTip="Pág. anterior"  CommandArgument="Prev" CssClass="anterior" Text="&larr;" formnovalidate/>
                                        <asp:Button ID="B3" runat="server" CommandName="Page" ToolTip="Sig. página" CommandArgument="Next" CssClass="siguiente" Text="&rarr;" formnovalidate/>
                                        <asp:Button ID="B4" runat="server" CommandName="Page" ToolTip="Últ. Pag"  CommandArgument="Last" CssClass="ultimo" Text="Ultima" formnovalidate/>
                                        <asp:label id="CurrentPageLabel" runat="server" CssClass = "PagerLabel"/>
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

            </div><!-- #main-form-content-field -->

            <div class="clear"></div>

        </div><!-- =========== #main-form-content ====================================================================== -->
    </div><!-- =========== #main-form ====================================================================== -->      
    
     
    <div id="popup-form" class="white-popup-block mfp-hide">
        <div class="bstt-form PantallaPopUp">
            <i class="fas fa-times-circle Close"></i>

            <h2 class="center-color">Agregar Origen del Producto</h2>
            
            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <label class="vlabel">Descripcion de Origen</label>  
                    <asp:TextBox ID="txtDescripcion" runat="server" MaxLength="100"></asp:TextBox>
                 </ContentTemplate>
                 <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="BtnGuardar" EventName="Click" />
                 </Triggers>
            </asp:UpdatePanel>
           
            <div class="btnGuarda" >
                  <div class="izq">
                      <asp:Button ID="BtnGuardar" runat="server" Text="Guardar"  class="btnEditar" />
                  </div>
                <div class="Derecha">
                      <asp:Button ID="BtnCerrar" runat="server" Text="Cerrar" Class="btnEliminar" />
               </div>
           </div>
        </div><!-- .bstt-form -->
    </div><!-- #popup-form -->


    <asp:UpdateProgress ID="uprData" runat="server">
        <ProgressTemplate>
            <div class="loader">
                <div>
                    <img alt="Cargando" src="../img/load.gif"/>
                    <p>Cargando...</p>
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>


<asp:Content ID="c5" ContentPlaceHolderID="cpScripts" Runat="Server">
     <script>
         $(document).ready(function () {
             //$('.datagird').basictable();

             $("#btnNew").click(function () {

                 $("#<%=hdfCodigo.ClientID%>").val("");

                $("#<%=txtDescripcion.ClientID%>").val("");
                $("#<%=txtDescripcion.ClientID%>").prev().removeClass('visible');

                $("#popuptittle").text('Agregar Origen del Producto');

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

            $("#<%=txtDescripcion.ClientID%>").val("");

        }
     </script>
</asp:Content>

