<%@ Page Language="VB" AutoEventWireup="false" CodeFile="login.aspx.vb" Inherits="Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta content='width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0' name='viewport' />

    <title>Inicio de Sesión | Facturación</title>

    <link href="img/favicon.png" rel="shortcut icon" type="image/x-icon" />
    <link href="css/sesion.css" rel="stylesheet" />
    <link href="css/alertify.core.css" rel="stylesheet" />
    <link href="css/alertify.default.css" rel="stylesheet" />
    <script src="js/jquery.min.js" type="text/javascript"></script>
    <script src="js/jquery.placeholder.label.js" type="text/javascript"></script>
    <script src="js/alertify.min.js" type="text/javascript"></script>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-T3c6CoIi6uLrA9TneNEoa7RxnatzjcDSCmG1MXxSR1GAsXEV/Dwwykc2MPK8M2HN" crossorigin="anonymous" />
</head>

<body>
    <form id="sesion" runat="server">
        <asp:ScriptManager ID="ScriptManager" runat="server"></asp:ScriptManager>
        <asp:Panel ID="pnlPrincipal" runat="server" CssClass="table">
            <div class="row">
                <div class="cell">
                    <article class="sesion">
                        <header>
                            <img src="img/logo.png" alt="logo" />
                        </header>
                        <div>
                            <asp:TextBox ID="txtUsuario" runat="server" AutoCompleteType="None"></asp:TextBox>
                        </div>
                        <div>
                            <asp:TextBox ID="txtPass" runat="server" set TextMode="Password" AutoCompleteType="None"></asp:TextBox>
                        </div>
                        <asp:Literal ID="ltMensaje" runat="server"></asp:Literal>
                        <asp:Button ID="btnEnviar" runat="server" Text="Iniciar Sesión" OnClientClick="validacionCustom()" />
                        <asp:Label ID="lblPublicoIp" runat="server" Text="" Style="display: none;"></asp:Label>
                    </article>
                </div>
            </div>
        </asp:Panel>
    </form>

    <script type="text/javascript">
        $(document).ready(function () {
            $('input[placeholder]').placeholderLabel();

        })

        function validacionCustom() {
            const txtUsuario = document.getElementById(<%=txtUsuario.ClientID%>);
            const txtPassword = document.getElementById(<%=txtPass.ClientID%>);

            txtUsuario.setCustomValidity("Ingrese el nombre de usuario.");
            txtPassword.setCustomValidity("Ingrese la contraseña.");

            txtUsuario.reportValidity();
            txtPassword.reportValidity();
        }
    </script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-C6RzsynM9kWDrMNeT87bh95OGNyZPhcTNXj1NW7RuBCsyN/o0jlpcV8Qyq46cDfL" crossorigin="anonymous"></script>
</body>
</html>
