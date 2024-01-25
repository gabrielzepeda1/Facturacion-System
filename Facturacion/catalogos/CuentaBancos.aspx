<%@ Page Title="Cuenta de Bancos" Language="VB" EnableEventValidation="false" MasterPageFile="~/master/principal.master" AutoEventWireup="false" CodeFile="CuentaBancos.aspx.vb" Inherits="catalogos_CuentaBancos" %>
<%@ Register Src="~/usercontrol/menu_catalogos.ascx" TagPrefix="uc1" TagName="menu_catalogos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpTitulo" Runat="Server">
      <h1>Catálogo de Cuenta de Bancos</h1>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpUbicacion" Runat="Server">
     <a href="../Default.aspx">Escritorio</a>
     <label>&gt;</label>
     <a href="CuentaBancos.aspx">Catálogo de Cuentas de Bancos</a>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MainContentPlaceHolder" Runat="Server">
     <%--<asp:ScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ScriptManager>--%>
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

                <asp:UpdatePanel ID="upMensaje" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Literal ID="ltMensaje" runat="server"></asp:Literal>
                    </ContentTemplate>
                    <Triggers>
                        <%--<asp:AsyncPostBackTrigger ControlID="BtnGuardar" EventName="Click" />--%>
                         <asp:AsyncPostBackTrigger ControlID="btnNew" EventName="Click" />
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
                                DataKeyNames="codigoCtaBanco"
                                AutoPostBack="true"
                                AutoGenerateColumns="False"
                                EnableViewState ="true">

                                                                
                                <HeaderStyle CssClass="table-header table-dark align-middle text-center" />

                                <Columns>
                                    <asp:BoundField HeaderText="Codigo" DataField="codigoCtaBanco" SortExpression="codigoCtaBanco" ItemStyle-CssClass="align-middle" />
                                    <asp:BoundField HeaderText="Descripcion" DataField="descripcion" SortExpression="descripcion" ItemStyle-CssClass="align-middle" />
                                    
                                     <asp:TemplateField HeaderText="Banco">
                                           <EditItemTemplate>
                                                 <%--<label>Sigla</label>--%>
                                                 <asp:DropDownList ID="ddlBanco" runat="server"  AutoPostBack="True" >
                                                 </asp:DropDownList>
                                           </EditItemTemplate>
                                           <ItemTemplate>
                                                     <asp:Label ID="lblBanco" runat="server" Text='<%# Bind("Banco")%>'></asp:Label>
                                           </ItemTemplate>
                                      </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Moneda">
                                           <EditItemTemplate>
                                                 <%--<label>Sigla</label>--%>
                                                 <asp:DropDownList ID="ddlMoneda" runat="server"  AutoPostBack="True" >
                                                 </asp:DropDownList>
                                           </EditItemTemplate>
                                           <ItemTemplate>
                                                     <asp:Label ID="lblMoneda" runat="server" Text='<%# Bind("Moneda")%>'></asp:Label>
                                           </ItemTemplate>
                                      </asp:TemplateField>
                                    
                                      <asp:TemplateField HeaderText="Pais">
                                           <EditItemTemplate>
                                                 <%--<label>Sigla</label>--%>
                                                 <asp:DropDownList ID="ddlPais" runat="server"  AutoPostBack="True" >
                                                 </asp:DropDownList>
                                           </EditItemTemplate>
                                           <ItemTemplate>
                                                     <asp:Label ID="lblPais" runat="server" Text='<%# Bind("Pais")%>'></asp:Label>
                                           </ItemTemplate>
                                      </asp:TemplateField>
                                    
                                  <%--  <asp:TemplateField HeaderText="Empresa">
                                           <EditItemTemplate>
                                                   <asp:DropDownList ID="ddlEmpresa" runat="server" >
                                                  </asp:DropDownList>
                                           </EditItemTemplate>
                                           <ItemTemplate>
                                                     <asp:Label ID="lblEmpresa" runat="server" Text='<%# Bind("Empresa")%>'></asp:Label>
                                           </ItemTemplate>
                                      </asp:TemplateField>--%>

                                    <asp:TemplateField HeaderText="Puesto">
                                           <EditItemTemplate>
                                                  <%--<label>Calidad</label>--%>
                                                  <asp:DropDownList ID="ddlPuesto" runat="server">
                                                  </asp:DropDownList>
                                           </EditItemTemplate>
                                           <ItemTemplate>
                                                 <asp:Label ID="lblPuesto"  runat="server" Text='<%# Bind("Puesto")%>'></asp:Label>
                                           </ItemTemplate>
                                      </asp:TemplateField>

                                     
                                    <asp:CommandField 
                                        HeaderText="Suprimir"
                                        ButtonType="Button" 
                                        DeleteText="Eliminar" 
                                        ShowDeleteButton="true"
                                        HeaderStyle-Width="120" > 
                                        <ControlStyle CssClass="btn btn-success align-middle" />
                                    </asp:CommandField>
                                    
                                     <asp:CommandField 
                                        HeaderText="Editar"
                                        ButtonType="Button"  
                                        SelectText="Actualizar" 
                                        ShowSelectButton="true"
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
    
     <div id="popup-form" class="white-popup-block mfp-hide">
        <%--<div class="popup-scroll">--%>
         <div class="bstt-form PantallaPopUp">
            
            <h2 class="center-color">Agregar/Editar Cuentas de Bancos</h2>
            <i class="fas fa-times-circle Close"></i>   
            
            <asp:UpdatePanel ID="upDesc" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
             <div class="PantaDosElemIzq">
                <label >Codigo</label>
                 <asp:TextBox ID="TextCodigo" runat="server" MaxLength="20"></asp:TextBox>  
             </div>
             <div class="PantaDosElemDerec">
                 <label>Descripcion de Cuenta de Banco</label>  
                 <asp:TextBox ID="txtDescripcion" runat="server" MaxLength="100"></asp:TextBox>
            </div>
           <div class="PantaDosElemIzq">
                <label>Moneda</label>
                <asp:DropDownList ID="ddlMoneda" runat="server" CssClass="marginb22"/>
                <ajaxToolkit:CascadingDropDown 
                    ID="CMoneda" 
                    runat="server" 
                    TargetControlID="ddlMoneda" 
                    ServicePath="../services/WSCatProductos.asmx"
                    ServiceMethod="GetMoneda" 
                    Category="CategoryName"
                    PromptText="Seleccione...">
                </ajaxToolkit:CascadingDropDown>
          </div>
          <div class="PantaDosElemDerec">
             <label>Banco</label>
                <asp:DropDownList ID="ddlBanco" runat="server" CssClass="marginb22"/>
                <ajaxToolkit:CascadingDropDown 
                    ID="CBanco" 
                    runat="server" 
                    TargetControlID="ddlBanco" 
                    ServicePath="../services/WSCatProductos.asmx"
                    ServiceMethod="GetBanco" 
                    Category="CategoryName"
                    PromptText="Seleccione...">
                </ajaxToolkit:CascadingDropDown>
          </div>   
          <div class="PantaDosElemIzq">                              
                <label>Pais</label>
                <asp:DropDownList ID="ddlPais" runat="server" CssClass="marginb22"/>
                <ajaxToolkit:CascadingDropDown 
                    ID="CPais" 
                    runat="server" 
                    TargetControlID="ddlPais" 
                    ServicePath="../services/WSCatProductos.asmx"
                    ServiceMethod="GetPaises" 
                    Category="CategoryPais"
                    PromptText="Seleccione...">
                </ajaxToolkit:CascadingDropDown>
         </div>
         <div class="PantaDosElemDerec">                              
             <label>Empresa</label>
                <asp:DropDownList ID="ddlEmpresa" runat="server" CssClass="marginb22"/>
                <ajaxToolkit:CascadingDropDown 
                    ID="CEmpresa" 
                    runat="server" 
                    TargetControlID="ddlEmpresa" 
                    ServicePath="../services/WSCatProductos.asmx"
                    ServiceMethod="GetEmpresaXPais" 
                    Category="CategoryEmpresa" 
                    ParentControlID ="ddlPais"
                    PromptText="Seleccione...">
                </ajaxToolkit:CascadingDropDown>
         </div>
         <div class="PantaDosElemIzq">
             <label>Puesto</label>
                <asp:DropDownList ID="ddlPuesto" runat="server" CssClass="marginb22"/>
                <ajaxToolkit:CascadingDropDown 
                    ID="CPuesto" 
                    runat="server" 
                    TargetControlID="ddlPuesto" 
                    ServicePath="../services/WSCatProductos.asmx"
                    ServiceMethod="GetPuestoXPaisXEmpresa" 
                    Category="CategoryPuesto"
                    ParentControlID="ddlEmpresa"
                    PromptText="Seleccione...">
                </ajaxToolkit:CascadingDropDown>
          </div>
          
              </ContentTemplate>  
               <Triggers>
                  <asp:AsyncPostBackTrigger ControlID="btnNew" EventName="Click" />
                  <asp:AsyncPostBackTrigger ControlID="GridViewOne" EventName="SelectedIndexChanged" />

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
     
             <%-- <div class="form-btn">
               <asp:Button ID="BtnGuardar" runat="server" Text="Guardar"/>
            </div>--%>
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
<asp:Content ID="Content5" ContentPlaceHolderID="cpScripts" Runat="Server">
     <script>
         $(document).ready(function () {
             //$('.datagird').basictable();

             $("#btnNew").click(function () {

                 var btn = document.getElementById("btnNew");
                btn.click();


                $("#<%=hdfCodigo.ClientID%>").val("");

                $("#<%=ddlPais.ClientID%>").val("");
                $("#<%=ddlPais.ClientID%>").prev().removeClass('visible');

                $("#<%=txtDescripcion.ClientID%>").val("");
                $("#<%=txtDescripcion.ClientID%>").prev().removeClass('visible');

                $("#<%=TextCodigo.ClientID%>").val("");
                $("#<%=TextCodigo.ClientID%>").prev().removeClass('visible');

                $("#<%=ddlMoneda.ClientID%>").val("");
                $("#<%=ddlMoneda.ClientID%>").prev().removeClass('visible');

                $("#<%=ddlBanco.ClientID%>").val("");
                $("#<%=ddlBanco.ClientID%>").prev().removeClass('visible');

                $("#<%=ddlEmpresa.ClientID%>").val("");
                $("#<%=ddlEmpresa.ClientID%>").prev().removeClass('visible');

                $("#<%=ddlPuesto.ClientID%>").val("");
                $("#<%=ddlPuesto.ClientID%>").prev().removeClass('visible');

                $("#popuptittle").text('Agregar Banco');

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
            $("#<%=ddlPais.ClientID%>").val("");
            $("#<%=ddlPuesto.ClientID%>").val("");
            $("#<%=ddlEmpresa.ClientID%>").val("");
            $("#<%=ddlBanco.ClientID%>").val("");
            $("#<%=ddlMoneda.ClientID%>").val("");

            var btn = document.getElementById("btnNew");
             btn.click();

         }
     </script>
</asp:Content>

