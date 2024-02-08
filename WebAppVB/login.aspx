<%@ Page Language="VB" AutoEventWireup="false" Inherits="WebAppVB.Login" Codebehind="login.aspx.vb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name='viewport' content='width=device-width, initial-scale=1.0' />
    <title>Inicio de Sesión | Facturación</title>
    <link href="img/favicon.png" rel="shortcut icon" type="image/x-icon" />
    <link href="https://use.fontawesome.com/releases/v5.5.0/css/all.css" rel="stylesheet" />
    <link href="<%= ResolveClientUrl("../css/newStyles.css")%>" rel="stylesheet" />
    <link href="<%= ResolveClientUrl("../css/loader.css")%>" rel="stylesheet" />
    <link href="css/sesion.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-T3c6CoIi6uLrA9TneNEoa7RxnatzjcDSCmG1MXxSR1GAsXEV/Dwwykc2MPK8M2HN" crossorigin="anonymous" />
    <link rel="stylesheet" href="//cdn.jsdelivr.net/npm/alertifyjs@1.13.1/build/css/alertify.min.css" />
    <link rel="stylesheet" href="//cdn.jsdelivr.net/npm/alertifyjs@1.13.1/build/css/themes/bootstrap.min.css" />
    <script src="//cdn.jsdelivr.net/npm/alertifyjs@1.13.1/build/alertify.min.js"></script>

</head>

<body class="bg-dark">
    <form runat="server">
        <asp:ScriptManager ID="ScriptManager" runat="server"></asp:ScriptManager>

        <div class="container d-flex flex-column justify-content-center align-items-center" style="height: 100vh;">
            <div class="p-2">
                <img src="img/logo.png" class="img-fluid img-logo" style="cursor: default;" width="115" alt="logo" />
            </div>
            <div class="my-1">
                <asp:TextBox ID="txtUsuario" CssClass="form-control" runat="server" AutoCompleteType="None"></asp:TextBox>
                <label>Usuario: </label>
            </div>
            <div class="my-1">
                <asp:TextBox ID="txtPass" CssClass="form-control" runat="server" TextMode="Password" AutoCompleteType="None"></asp:TextBox>
                <label>Contraseña: </label>
            </div>
            <div class="my-1 p-2">
                <asp:LinkButton ID="btnEnviar" runat="server" Text='<i class="fas fa-sign-in-alt"></i> Iniciar Sesión' CssClass="btn btn-primary" />
            </div>
        </div>

        <asp:Literal ID="ltMensaje" runat="server"></asp:Literal>
        <asp:Label ID="lblPublicoIp" runat="server" Text="" Style="display: none;"></asp:Label>


        <div>
            <h3>Session Idle:&nbsp;<span id="secondsIdle"></span>&nbsp;seconds.</h3>
            <asp:LinkButton ID="lnkFake" runat="server" />
        </div>

    </form>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-C6RzsynM9kWDrMNeT87bh95OGNyZPhcTNXj1NW7RuBCsyN/o0jlpcV8Qyq46cDfL" crossorigin="anonymous"></script>
    <script type="text/javascript">
        const btnEnviar = document.getElementById("<%=btnEnviar.ClientID%>");

        document.addEventListener("DOMContentLoaded", () => {
            setFloatingLabel();

        })

        document.querySelector("form").addEventListener("submit", (e) => {
            createLoader();
        });

        const createLoader = () => {
            const loader = document.createElement("div");
            loader.classList.add("loader");
            document.body.appendChild(loader);

            loader.classList.add("loader-hidden");
            setTimeout(() => {
                loader.addEventListener("transitionend", () => {
                    document.body.removeChild(loader);
                }, { once: true });
            }, 0);
        };

        const setFloatingLabel = () => {
            document.querySelectorAll("form div").forEach((div) => {
                if (div.children && div.children[0].tagName == "INPUT" && div.children[1].tagName == "LABEL") {
                    div.classList.add("form-floating")
                }
            })
        }

        btnEnviar.addEventListener("click",
            (e) => {
                e.preventDefault();

                const txtUsuario = document.getElementById("<%=txtUsuario.ClientID%>");
                const txtPassword = document.getElementById("<%=txtPass.ClientID%>");

                if (txtUsuario.value === "") {
                    alertify.error("El nombre de usuario no puede estar vacío");
                } else if (txtPassword.value === "") {
                    alertify.error("La contraseña no puede estar vacía.");
                } else {
                    __doPostBack("<%=btnEnviar.UniqueID%>", "");
                }
            });

    </script>
</body>
</html>
