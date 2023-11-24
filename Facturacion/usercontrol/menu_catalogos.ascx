<%@ Control Language="VB" AutoEventWireup="false" CodeFile="menu_catalogos.ascx.vb" Inherits="usercontrol_menu_catalogos" %>

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