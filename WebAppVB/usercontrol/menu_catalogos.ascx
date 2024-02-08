<%@ Control Language="VB" AutoEventWireup="false" Inherits="WebAppVB.usercontrol_menu_catalogos" Codebehind="menu_catalogos.ascx.vb" %>

<%--<nav id="navegacion-widget">
    <asp:Literal ID="ltMenu" runat="server"></asp:Literal>
</nav>--%>

<script>
    $('#navegacion-widget ul > li').click(function (e) {

        if ($(this).children('ul').css('display') == 'none')
        {
            $(this).children("ul").slideDown();
        }
        else
        {
            $(this).children("ul").slideUp();
        }

    });
</script>