﻿<%@ Page Title="Catálogo de Siglas | Facturación" Language="VB" EnableEventValidation="false" MasterPageFile="~/master/principal.master" AutoEventWireup="false" Inherits="WebAppVB.Catalogos_Siglas" Codebehind="siglas.aspx.vb" %>

<%@ Register Src="~/usercontrol/menu_catalogos.ascx" TagPrefix="uc1" TagName="menu_catalogos" %>

<asp:Content ID="c1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="c2" ContentPlaceHolderID="cpTitulo" runat="Server">
    <h1>Catálogo de Siglas</h1>
</asp:Content>

<asp:Content ID="c3" ContentPlaceHolderID="cpUbicacion" runat="Server">
    <a href="../Default.aspx">Escritorio</a>
    <label>&gt;</label>
    <a href="siglas.aspx">Catálogo de Siglas</a>
</asp:Content>

<asp:Content ID="c4" ContentPlaceHolderID="MainContentPlaceHolder" runat="Server">
    <asp:HiddenField ID="hdfCodigo" runat="server" />
    <div id="main-form">
        <div class="container-fluid">
            <uc:Buttons ID="ucButtons" runat="server" />  
        </div>

        <div class="container-fluid">
            <div class="row">
                <div class="col-8 offset-2">
                    <div class="table-responsive">
                        <asp:UpdatePanel ID="upGrid" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:GridView ID="GridViewOne" runat="server" AutoGenerateColumns="False"
                                    CssClass="table table-light table-striped table-hover table-bordered align-middle"
                                    CellPadding="0" GridLines="None" AllowPaging="True"
                                    PageSize="10" DataKeyNames="cod_sigla, sigla" AllowSorting="True"
                                    Width="100%"
                                    OnPageIndexChanging="OnPaging" EmptyDataText="No se encontraron registros...">
                                    <HeaderStyle CssClass="table-header table-dark align-middle text-center " />
                                    <Columns>
                                        <asp:BoundField DataField="cod_sigla" HeaderText="Codigo" SortExpression="sigla" Visible="true"></asp:BoundField>
                                        <asp:TemplateField HeaderText="Siglas" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="45%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSiglas" runat="server" Text='<%# Eval("sigla") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <label class="form-label">Editar:</label>
                                                <asp:TextBox ID="txtSiglas" CssClass="form-control-sm text-capitalize" runat="server" Text='<%# Eval("sigla") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>

                                        <asp:CommandField ButtonType="Link" EditText="Editar" ShowEditButton="true" UpdateText="Actualizar" CancelText="Cancelar" HeaderStyle-Width="25%" ItemStyle-HorizontalAlign="Center">
                                            <ControlStyle CssClass="btn btn-primary align-middle" />
                                        </asp:CommandField>

                                        <asp:CommandField ButtonType="Link" DeleteText="Eliminar" ShowDeleteButton="true" HeaderStyle-Width="25%" ItemStyle-HorizontalAlign="Center">
                                            <ControlStyle CssClass="btn btn-danger align-middle" />
                                        </asp:CommandField>
                                    </Columns>

                                    <PagerTemplate>
                                        <div class="pagination">
                                            <asp:Button ID="B1" runat="server" CommandName="Page" ToolTip="1ra pág." CommandArgument="First" CssClass="primero" Text="1" formnovalidate="true" />
                                            <asp:Button ID="B2" runat="server" CommandName="Page" ToolTip="Anterior" CommandArgument="Prev" CssClass="anterior" Text="&larr;" formnovalidate="true" />
                                            <asp:Button ID="B3" runat="server" CommandName="Page" ToolTip="Siguiente" CommandArgument="Next" CssClass="siguiente" Text="&rarr;" formnovalidate="true" />
                                            <asp:Button ID="B4" runat="server" CommandName="Page" ToolTip="Última pág." CommandArgument="Last" CssClass="ultimo" Text="Ult." formnovalidate="true" />
                                            <asp:Label ID="CurrentPageLabel" runat="server" CssClass="PagerLabel" />
                                        </div>
                                    </PagerTemplate>
                                </asp:GridView>
                            </ContentTemplate>

                            <Triggers>
                                <%--<asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />--%>
                                <asp:AsyncPostBackTrigger ControlID="GridViewOne" EventName="RowUpdating" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
        `
    </div>

    <div class="modal fade" id="staticBackdrop" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h3 class="modal-title fs-5">Nuevas Siglas</h3>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <label class="form-label">Descripción: </label>
                    <asp:TextBox ID="txtDescripcion" CssClass="form-control" runat="server" AutoCompleteType="None"></asp:TextBox>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnGuardar" CssClass="btn btn-primary" runat="server" Text="Guardar" />
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="c5" ContentPlaceHolderID="cpScripts" runat="Server">
    <script>
        <%--const txtSearch = document.querySelector('#<%= txtSearch.ClientID %>');

        txtSearch.addEventListener("input", (e) => {
            __doPostBack('<%=btnSearch.UniqueID %>')
        });--%>
    </script>
</asp:Content>

