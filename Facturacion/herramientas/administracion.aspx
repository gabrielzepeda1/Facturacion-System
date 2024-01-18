<%@ Page Title="Administración | Facturación"  Language="VB" MasterPageFile="~/master/principal.master" AutoEventWireup="false" CodeFile="administracion.aspx.vb" Inherits="admin_administracion" %>

<asp:Content ID="c1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="c2" ContentPlaceHolderID="cpTitulo" Runat="Server">
    <h1>Administración</h1>
</asp:Content>

<asp:Content ID="c3" ContentPlaceHolderID="cpUbicacion" Runat="Server">
    <a href="../Default.aspx">Escritorio</a>
    <label>&gt;</label>
    <a href="administracion.aspx">Administración</a>
</asp:Content>

<asp:Content ID="c4" ContentPlaceHolderID="MainContentPlaceHolder" Runat="Server">
    <div class="limited">
        <nav id="navegacion-panel">
            <asp:Literal ID="ltMenu" runat="server"></asp:Literal>
        </nav>
    </div>
</asp:Content>

<asp:Content ID="c5" ContentPlaceHolderID="cpScripts" Runat="Server">
</asp:Content>