<%@ Page Title="Paridad | Facturación" Language="VB" EnableEventValidation="false" MasterPageFile="~/master/principal.master" AutoEventWireup="false" CodeFile="paridad.aspx.vb" Inherits="catalogos_paridad" %>

<%@ Register Src="~/usercontrol/menu_catalogos.ascx" TagPrefix="uc1" TagName="menu_catalogos" %>

<asp:Content ID="c1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="c2" ContentPlaceHolderID="cpTitulo" Runat="Server">
    <h1>Paridad</h1>
</asp:Content>

<asp:Content ID="c3" ContentPlaceHolderID="cpUbicacion" Runat="Server">
    <a href="../Default.aspx">Escritorio</a>
    <label>&gt;</label>
    <a href="paridad.aspx">Paridad</a>
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
                            <asp:Literal ID="ltMensajeGrid" runat="server"></asp:Literal>


                            <div class="table-responsive />
                            <asp:GridView 
                                ID="GridViewOne" 
                                runat="server" 
                                CssClass="table table-light table-sm table-striped table-hover table-bordered" 
                                CellPadding="0"
                                GridLines="None" 
                                AllowPaging="True"
                                AllowSorting ="True"
                                PageSize="16" 
                                DataKeyNames="Fecha" 
                                AutoGenerateColumns="False">

                                <Columns>
                                    <asp:BoundField HeaderText="FECHA" DataField="FECHA" ItemStyle-CssClass="align-middle" />
                                    <asp:BoundField HeaderText="VALOR" DataField="VALOR" ItemStyle-CssClass="align-middle" />
                                    <asp:BoundField HeaderText="AÑO" DataField="ANO"/>
                                    <asp:BoundField HeaderText="MES" DataField="MES" Visible="false" ItemStyle-CssClass="align-middle" />
                                    <asp:BoundField HeaderText="DIA" DataField="DIA" Visible="false" ItemStyle-CssClass="align-middle" />
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
        <div class="bstt-form">
            <i class="fas fa-times-circle Close"></i>

            <h2 class="center-color">Tasa de Cambio</h2>

            <asp:TextBox ID="txtFecha" runat="server" AutoCompleteType="None" ></asp:TextBox>

            <asp:TextBox ID="txtDescripcion" runat="server" MaxLength="100"></asp:TextBox>

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

            $("#<%=txtFecha.ClientID%>").datepicker({
                dateFormat: "dd/mm/yy",
                showOtherMonths: true,
                selectOtherMonths: true
            });

            $("#btnNew").click(function () {

                $("#<%=hdfCodigo.ClientID%>").val("");

                $("#<%=txtDescripcion.ClientID%>").val("");
                $("#<%=txtDescripcion.ClientID%>").prev().removeClass('visible');

                $("#popuptittle").text('Agregue Valor de Paridad');

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

        function responsive_grid() {
            //$('.datagird').basictable();
        }
    </script>
</asp:Content>