<%@ Page Title="Escritorio | FACTURACIÓN" Language="VB" MasterPageFile="~/master/principal.master" AutoEventWireup="false" Inherits="WebAppVB.Default" Codebehind="Default.aspx.vb" %>

<asp:Content ID="c1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="css/newStyles.css" />
</asp:Content>

<asp:Content ID="cTitulo" ContentPlaceHolderID="cpTitulo" runat="Server">
    <h1>Escritorio</h1>
</asp:Content>

<asp:Content ID="cUbicacion" ContentPlaceHolderID="cpUbicacion" runat="Server"></asp:Content>

<asp:Content ID="c2" ContentPlaceHolderID="MainContentPlaceHolder" runat="Server">
    <div class="limited">
        <nav id="navegacion-panel">
            <div class="accordion" id="accordionExample">
                <asp:Literal ID="ltMenu" runat="server"></asp:Literal>
            </div>
        </nav>
    </div>
</asp:Content>
