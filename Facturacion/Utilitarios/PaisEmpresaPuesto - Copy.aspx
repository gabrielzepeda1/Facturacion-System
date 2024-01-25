<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PaisEmpresaPuesto - Copy.aspx.vb" Inherits="Utilitarios_PaisEmpresaPuesto" %>
<!DOCTYPE html>



<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">


<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>ADMINISTRADOR</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <%--<meta content='width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0' name='viewport' />--%>
    
    <link href="img/favicon.png" rel="shortcut icon" type="image/x-icon" />
    <link href="css/sesion.css" rel="stylesheet" />
    <link href="css/alertify.core.css" rel="stylesheet" />
    <link href="css/alertify.default.css" rel="stylesheet" />

    <script src="js/jquery.min.js" type="text/javascript"></script>
    <script src="js/jquery.placeholder.label.js" type="text/javascript"></script>
    <script src="js/alertify.min.js" type="text/javascript"></script>

    <link href="<%= ResolveClientUrl("../img/favicon.png")%>" rel="shortcut icon" type="image/x-icon" />
    
    <link href="<%= ResolveClientUrl("../js/stylesheets/jquery.sidr.dark.min.css")%>" rel="stylesheet" />
    <link href="https://use.fontawesome.com/releases/v5.5.0/css/all.css" rel="stylesheet" />
    <link href="<%= ResolveClientUrl("../css/basictable.css")%>" rel="stylesheet" />
    <link href="<%= ResolveClientUrl("../css/alertify.core.css")%>" rel="stylesheet" />
    <link href="<%= ResolveClientUrl("../css/alertify.default.css")%>" rel="stylesheet" />
    <link href="<%= ResolveClientUrl("../js/bxslider-4-4.2.12/src/css/jquery.bxslider.css")%>" rel="stylesheet" />
    <link href="<%= ResolveClientUrl("../css/datatable.min.css")%>" rel="stylesheet" />
    <link href="<%= ResolveClientUrl("../js/jquery-ui-1.12.1/jquery-ui.min.css")%>" rel="stylesheet" />

    <script src="<%= ResolveClientUrl("../js/datatable.min.js")%>" type="text/javascript"></script>
    <script src="<%= ResolveClientUrl("../js/jquery.min.js")%>" type="text/javascript"></script>
    <script src="<%= ResolveClientUrl("../js/jquery.sidr.min.js")%>" type="text/javascript"></script>
    <script src="<%= ResolveClientUrl("../js/datatable.jquery.min.js")%>" type="text/javascript"></script>
    <script src="<%= ResolveClientUrl("../js/jquery.basictable.min.js")%>" type="text/javascript"></script>
    <script src="<%= ResolveClientUrl("../js/jquery.bpopup.min.js")%>" type="text/javascript"></script>
    <script src="<%= ResolveClientUrl("../js/alertify.min.js")%>" type="text/javascript"></script>
    <script src="<%= ResolveClientUrl("../js/bxslider-4-4.2.12/src/js/jquery.bxslider.js")%>" type="text/javascript"></script>
    <script src="<%= ResolveClientUrl("../js/jquery-ui-1.12.1/jquery-ui.js")%>" type="text/javascript"></script>
    <script src="<%= ResolveClientUrl("../js/myscript.js")%>" type="text/javascript"></script>


    <link href="<%= ResolveClientUrl("../css/principal.css")%>" rel="stylesheet" />
    <link href="<%= ResolveClientUrl("../css/newStyles.css")%>" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-T3c6CoIi6uLrA9TneNEoa7RxnatzjcDSCmG1MXxSR1GAsXEV/Dwwykc2MPK8M2HN" crossorigin="anonymous" />

</head>
 
     
<body class="valingc">
    
    <form id="form1" runat="server" class="valingc">
         <asp:ScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ScriptManager>
         <div class="limited">
           <article class="sesion">
             <header id="main-header">
               <div class="table">
                <div class="row">
                
                <div id="logo" class="cel">
                    <img alt="<%= CompanyName%>" title="<%= CompanyName%>" class="logo" src="<%= ResolveClientUrl("../img/logo.png")%>" />
                </div>

                  <div id="options" class="cel tright">
                    <nav class="utilities">
                        <ul>
                            <li>
                                <a class="user" href="<%= ResolveClientUrl("../herramientas/usuario_editar.aspx")%>"><i class="fas fa-user"></i> <%= MyUserName%></a>
                            </li>
                          
                            <li>
                                <a onclick="OnCerrar();" title="Cerrar Sesión"><i class="fas fa-sign-out-alt"></i></a>
                            </li>
                        </ul>
                    </nav>
                </div>
            </div>
        </div>
      </header>
               
                           
            <asp:Literal ID="ltMensajeGrid" runat="server"></asp:Literal>
               <h2>Seleccione: Pais - Empresa - Puesto</h2>
            <asp:GridView 
                ID="GridViewOne" 
                runat="server" 
                CssClass="table table-light table-sm table-striped table-hover table-bordered" 
                CellPadding="0"
                GridLines="None" 
                AllowPaging="True"
                AllowSorting ="True"
                PageSize="10" 
                DataKeyNames="cod_pais,cod_empresa,cod_puesto,lista_predeterminada" 
                AutoGenerateColumns="False">
                                                                
                <HeaderStyle CssClass="table-header table-dark align-middle text-center" />

                <Columns>
                    <asp:BoundField HeaderText="Pais" DataField="Pais" SortExpression="Pais" ReadOnly="true" />
                    <asp:BoundField HeaderText="Empresa" DataField="Empresa" SortExpression="Empresa"/>
                    <asp:BoundField HeaderText="Puesto" headerstyle-wrap="true" DataField="Puesto" SortExpression="DesCorta"/>
                    <asp:BoundField HeaderText="Codigo Pais" DataField="cod_Pais" SortExpression="cod_Pais" ReadOnly="true" Visible="false"   />
                    <asp:BoundField HeaderText="Codigo Empresa" DataField="cod_empresa" SortExpression="cod_Empresa"  visible="false"/>
                    <asp:BoundField HeaderText="Codigo Puesto" headerstyle-wrap="true" DataField="cod_Puesto" SortExpression="cod_Puesto" Visible="false" />
                    <asp:BoundField HeaderText="Modifica Precios" headerstyle-wrap="true" DataField="cambiar_precio" SortExpression="cambiar_precio"/>
                    <asp:BoundField HeaderText="Sector de Mercado" headerstyle-wrap="true" DataField="lista_predeterminada" SortExpression="lista_predeterminada"  />
                    <asp:BoundField HeaderText="Verifica Inventario" headerstyle-wrap="true" DataField="verificar_inventario" SortExpression="verificar_inventario"  />
                    <asp:BoundField HeaderText="Porcentaje(%) de Impuestos" headerstyle-wrap="true" DataField="PorcImtos" SortExpression="PorcImtos"  />
                    <asp:BoundField HeaderText="Identificación" headerstyle-wrap="true" DataField="cedula_ruc" SortExpression="cedula_ruc"  Visible="false" />
                    <asp:BoundField HeaderText="Autorización MIFIN" headerstyle-wrap="true" DataField="autorizacion_mifin" SortExpression="autorizacion_mifin"  Visible="false" />
                    <asp:BoundField HeaderText="Dirección" headerstyle-wrap="true" DataField="direccion" SortExpression="direccion"  Visible="false" />
                    <asp:BoundField HeaderText="Teléfono" headerstyle-wrap="true" DataField="telefono" SortExpression="telefono"  Visible="false" />
                    <asp:CommandField 
                        ButtonType="Button" 
                        EditText="Seleccionar"
                        ShowSelectButton="true" 
                        HeaderStyle-Width="120" > 
                        <ControlStyle CssClass="btn btn-success align-middle" />
                    </asp:CommandField>
                                    
                </Columns>
            </asp:GridView>
          </article>
        </div>
       </form>
</body>
</html>
