<%@ Page Title="Productos Maestros | Facturación" Language="VB" EnableEventValidation="false" MasterPageFile="~/master/principal.master" AutoEventWireup="false" CodeFile="ProductosMaestro.aspx.vb" Inherits="catalogos_ProductosMaestro" %>

<%@ Register Src="~/usercontrol/menu_catalogos.ascx" TagPrefix="uc1" TagName="menu_catalogos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
  
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cpTitulo" Runat="Server">
     <h1>Catálogo de Maestro de Productos</h1>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cpUbicacion" Runat="Server">
    <a href="../Default.aspx">Escritorio</a>
    <label>&gt;</label>
    <a href="ProductosMaestro.aspx">Catálogo de Maestro de Productos</a>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContentPlaceHolder" Runat="Server">
    <asp:ScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ScriptManager>
    <%--<div class="popup-scroll">--%>
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

                           <div class="clear"  style ="width:850px; overflow:auto;"></div>

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
                                    <asp:BoundField HeaderText="Codigo de Producto" DataField="Codigo" SortExpression="Codigo" ReadOnly="true" />
                                    <asp:BoundField HeaderText="Numero" DataField="num_producto" SortExpression="num_producto"/>
                                    <asp:BoundField HeaderText="Producto" DataField="producto" SortExpression="producto"/>
                                     <%--<asp:BoundField HeaderText="Prod EN SN" DataField="produccion_sm" SortExpression="produccion_sm"/>--%>
                                    
                                    <asp:TemplateField HeaderText="Prod EN SN">
                                           <EditItemTemplate>
                                                 <asp:CheckBox ID="CheckSm" runat="server"  AutoPostBack="True" >
                                                 </asp:CheckBox>
                                           </EditItemTemplate>
                                           <ItemTemplate>
                                                     <asp:Label ID="LabelSN" runat="server" Text='<%# Bind("produccion_sm")%>'></asp:Label>
                                           </ItemTemplate>
                                      </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Sigla">
                                           <EditItemTemplate>
                                                 <%--<label>Sigla</label>--%>
                                                 <asp:DropDownList ID="ddlSigla" runat="server"  AutoPostBack="True" >
                                                 </asp:DropDownList>
                                           </EditItemTemplate>
                                           <ItemTemplate>
                                                     <asp:Label ID="Label3" runat="server" Text='<%# Bind("Sigla")%>'></asp:Label>
                                           </ItemTemplate>
                                      </asp:TemplateField>
                                    
                                    <asp:TemplateField HeaderText="Origen">
                                           <EditItemTemplate>
                                                  <%--<label>Origen</label>--%>
                                                  <asp:DropDownList ID="ddlOrigen" runat="server" >
                                                  </asp:DropDownList>
                                           </EditItemTemplate>
                                           <ItemTemplate>
                                                     <asp:Label ID="Label1" runat="server" Text='<%# Bind("Origen")%>'></asp:Label>
                                           </ItemTemplate>
                                      </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Calidad">
                                           <EditItemTemplate>
                                              
                                                  <asp:DropDownList ID="ddlCalidad" runat="server">
                                                  </asp:DropDownList>
                                           </EditItemTemplate>
                                           <ItemTemplate>
                                                 <asp:Label ID="Label12"  runat="server" Text='<%# Bind("Calidad")%>'></asp:Label>
                                           </ItemTemplate>
                                      </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Presentacion">
                                           <EditItemTemplate>
                                                
                                                  <asp:DropDownList ID="ddlPresentacion" runat="server">
                                                  </asp:DropDownList>
                                           </EditItemTemplate>
                                           <ItemTemplate>
                                                 <asp:Label ID="Label4"  runat="server" Text='<%# Bind("Presentacion")%>'></asp:Label>  
                                           </ItemTemplate>
                                      </asp:TemplateField>
                                   <asp:TemplateField HeaderText="Familia">
                                           <EditItemTemplate>
                                                 
                                                  <asp:DropDownList ID="ddlFamilia" runat="server">
                                                  </asp:DropDownList>
                                           </EditItemTemplate>
                                           <ItemTemplate>
                                                <asp:Label ID="Label5" runat="server" Text='<%# Bind("Familia")%>'></asp:Label>  
                                           </ItemTemplate>
                                      </asp:TemplateField>
                                                                
                                     <asp:CommandField 
                                        HeaderText="Editar"
                                        ButtonType="Button"  
                                        SelectText="Actualizar" 
                                        ShowSelectButton="true"
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
                        <asp:AsyncPostBackTrigger ControlID="GridViewOne" EventName="RowDeleting" />
                        <asp:AsyncPostBackTrigger ControlID="GridViewOne" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>

            </div><!-- #main-form-content-field -->
          <div class="clear"></div>
       </div><!-- =========== #main-form-content ====================================================================== -->
    </div><!-- =========== #main-form ====================================================================== -->      
         <%--</div><!-- ===========<!-- popup-scroll  <div id="popup-form" class="white-popup-block mfp-hide" > ====================================================================== -->--%>      
   
    <div id="popup-form" class="white-popup-block mfp-hide">
     
           <%-- <div class="popup-scroll">--%>
               <div class="bstt-form PantallaPopUp">
                 
                   <%--<i class="fas fa-times-circle Close"></i>--%>
                 
            <h2 class="center-color">Agregar/Editar Productos Maestros</h2>
                   <i class="fas fa-times-circle Close"></i>
          
            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="PantaDosElemIzq">
                     <label>Codigo</label>
                     <asp:TextBox ID="TextCodProducto" runat="server" MaxLength="50"></asp:TextBox>
                </div>
                <div class="PantaDosElemDerec">
                     <label>Descripción</label>
                     <asp:TextBox ID="txtDescripcion" runat="server" MaxLength="100"></asp:TextBox>
                </div>
                <div class="PantaDosElemIzq">
                    <label>Número</label>
                    <asp:TextBox ID="txtNumProd" runat="server" MaxLength="10"></asp:TextBox>
                </div>
                <div class="PantaDosElemDerec">
                    <label>Producido por Sm</label>
                    <asp:CheckBox ID="CheckSm" runat="server" Checked="true"/>
                    
                </div>
                <div class="PantaDosElemDerec"> 
                      <label>Sigla</label>
                      <asp:DropDownList ID="ddlSigla" runat="server" CssClass="marginb11"/>
                      <ajaxToolkit:CascadingDropDown 
                        ID="CCsigla" 
                        runat="server" 
                        Category="CategoryName"
                        TargetControlID="ddlSigla" 
                        ServicePath="../services/WSCatProductos.asmx"
                        ServiceMethod="GetSigla" 
                        PromptText="Seleccione...">
                      </ajaxToolkit:CascadingDropDown>
                 </div>
                
                <div class="PantaDosElemIzq">       
                      <label>Origen</label>
                      <asp:DropDownList ID="ddlOrigen" runat="server" CssClass="marginb11"/>
                      <ajaxToolkit:CascadingDropDown 
                        ID="CCOrigen" 
                        runat="server" 
                        TargetControlID="ddlOrigen" 
                        ServicePath="../services/WSCatProductos.asmx"
                        ServiceMethod="GetOrigen" 
                        Category="CategoryName"
                        PromptText="Seleccione...">
                    </ajaxToolkit:CascadingDropDown>
            </div>
           
            <div class="PantaDosElemDerec">       
             <label>Calidad</label>
             <asp:DropDownList ID="ddlCalidad" runat="server" CssClass="marginb11"/>
             <ajaxToolkit:CascadingDropDown 
                ID="CCCalidad" 
                runat="server" 
                TargetControlID="ddlCalidad" 
                ServicePath="../services/WSCatProductos.asmx"
                ServiceMethod="GetCalidades" 
                Category="CategoryName"
                PromptText="Seleccione...">
             </ajaxToolkit:CascadingDropDown>
           </div>
          
           <div class="PantaDosElemIzq">       
            <label>Presentacion</label>
            <asp:DropDownList ID="ddlPresentacion" runat="server" CssClass="marginb11"/>
            <ajaxToolkit:CascadingDropDown 
                ID="CCPresent" 
                runat="server" 
                TargetControlID="ddlPresentacion" 
                ServicePath="../services/WSCatProductos.asmx"
                ServiceMethod="GetPresentacion" 
                Category="CategoryName"
                PromptText="Seleccione...">
            </ajaxToolkit:CascadingDropDown>
          </div>
          <div class="PantaDosElemDerec">       
             <label>Familia</label>
            <asp:DropDownList ID="ddlFamilia" runat="server" CssClass="marginb11"/>
            <ajaxToolkit:CascadingDropDown 
                ID="CCFamilia" 
                runat="server" 
                TargetControlID="ddlFamilia" 
                ServicePath="../services/WSCatProductos.asmx"
                ServiceMethod="GetFamilia" 
                Category="CategoryName"
                PromptText="Seleccione...">
            </ajaxToolkit:CascadingDropDown>
         </div>
         
         </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="GridViewOne" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="BtnGuardar" EventName="Click" />
            </Triggers>
      </asp:UpdatePanel>
           
            <div class="clear"></div>
     
             <div class="btnGuarda" >
                  <div class="izq">
                      <asp:Button ID="BtnGuardar" runat="server" Text="Guardar"  class="btnEditar" />
                  </div>
                <div class="Derecha">
                      <asp:Button ID="BtnCerrar" runat="server" Text="Cerrar" Class="btnEliminar" />
               </div>
            </div>
   
                </div><!-- .bstt-form -->
          <%--</div><!-- popup-scroll -->--%>
       
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
<asp:Content ID="Content5" ContentPlaceHolderID="cpScripts" Runat="Server">
     <script>
         $(document).ready(function () {
             //$('.datagird').basictable();

             $("#btnNew").click(function () {

                 $("#<%=hdfCodigo.ClientID%>").val("");

                 $("#<%=txtDescripcion.ClientID%>").val("");
                 $("#<%=txtDescripcion.ClientID%>").prev().removeClass('visible');

                 $("#<%=txtNumProd.ClientID%>").val("");
                 $("#<%=txtNumProd.ClientID%>").prev().removeClass('visible');

                 $("#<%=CheckSm.ClientID%>").val("");
                 $("#<%=CheckSm.ClientID%>").prev().removeClass('visible');

                 

                 $("#popuptittle").text('Agregar Producto al maestro');

                 
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

             $("#<%=txtDescripcion.ClientID%>").val("");
             $("#<%=txtNumProd.ClientID%>").val("");
             $("#<%=CheckSm.ClientID%>").val("");
         }
     </script>
</asp:Content>

