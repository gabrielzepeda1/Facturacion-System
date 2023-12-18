Imports System.Data.OleDb
Imports System.IO
Imports System.Security.Cryptography
Imports FACTURACION_CLASS
Imports Microsoft.ReportingServices.DataProcessing
Partial Class Login
    Inherits Page
    Dim _conn As New seguridad

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Attributes_Text()
        End If
    End Sub

    Private Sub Attributes_Text()
        txtUsuario.Attributes.Add("placeholder", "Escriba su nombre de usuario")
        'txtUsuario.Attributes.Add("required", "required")
        txtUsuario.Attributes.Add("autocomplete", "off")
        txtPass.Attributes.Add("placeholder", "Introduzca su contraseña")
        'txtPass.Attributes.Add("required", "required")
        txtPass.Attributes.Add("autocomplete", "off")
    End Sub

    ''' <summary>
    ''' Obtiene el IP publico del usuario. Instalar Paquete: Install-Package IpPublicKnowledge
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function ObtenerIp_Publica() As String
        Try
            Dim ipInfo As String = Request.ServerVariables("REMOTE_ADDR").ToString()

            Return ipInfo
        Catch ex As Exception
            Const message = "alertify.error('Ha ocurrido un error al intentar Obtener el IP publico. Si el problema persiste, contacte con el administrador.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", message, True)
            Return String.Empty
        End Try
    End Function

#Region "INICIAR SESIÓN"

    Private Sub btnEnviar_Click(sender As Object, e As EventArgs) Handles btnEnviar.Click
        ValidateUser()

    End Sub
    Protected Sub ValidateUser()

        Dim username As String = txtUsuario.Text.Trim()
        Dim password As String = HttpUtility.UrlEncode(Encrypt(Me.txtPass.Text.Trim()))
        Dim browserData As String = Request.Browser.Browser & " " & Request.Browser.Version & " " & Request.Browser.Platform
        Dim codigoUser As Integer = 0

        Try
            Using dbCon As New OleDbConnection(_conn.conn)
                dbCon.Open()

                Using cmd As New OleDbCommand("sp_sys_login", dbCon)

                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.AddWithValue("@Username", username)
                    cmd.Parameters.AddWithValue("@Password", password)

                    codigoUser = Convert.ToInt32(cmd.ExecuteScalar())
                End Using

                Select Case codigoUser
                    Case -1
                        'MsgBox("Usuario o Contraseña Incorrectos", MsgBoxStyle.Critical, "Sistema de Corrales de Engorde")
                        Exit Select
                    Case -2
                        'MsgBox("Usuario Inactivo", MsgBoxStyle.Critical, "Sistema de Corrales de Engorde")
                        Exit Select
                    Case Else
                        If Not String.IsNullOrEmpty(Request.QueryString("ReturnUrl")) Then
                            FormsAuthentication.SetAuthCookie(username, False)
                            Response.Redirect(Request.QueryString("ReturnUrl"))
                        Else
                            HandleUserSession(username, browserData)
                        End If
                        Exit Select
                End Select
            End Using

        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub
    Protected Sub HandleUserSession(username As String, browserData As String)

        Dim sessionData As Dictionary(Of String, Object) = _conn.ControlarSesion(username, ObtenerIp_Publica(), browserData)

        Try
            If sessionData IsNot Nothing Then

                If sessionData.ContainsKey("Status") AndAlso sessionData.Item("Status") = "SESION INICIADA" Then

                    Dim cookie As New HttpCookie("CKSMFACTURA")

                    cookie("CodigoSesion") = sessionData.Item("CodigoSesion").ToString()
                    cookie("CodigoUser") = sessionData.Item("CodigoUser").ToString()
                    cookie("Username") = sessionData.Item("Username").ToString()
                    cookie("Password") = sessionData.Item("Password").ToString()

                    cookie.Expires = Now.AddDays(1)
                    Response.Cookies.Add(cookie)

                    'FormsAuthentication.RedirectFromLoginPage(username, False)
                    Response.Redirect("~/Utilitarios/PaisEmpresaPuesto.aspx")
                Else
                    Dim userLoggedInAlert As String = $"alertify.alert('{ sessionData.Item("Status").ToString()}')"
                    ScriptManager.RegisterStartupScript(Me, Page.GetType, "userLoggedInAlert", userLoggedInAlert, True)
                End If
            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message)
        End Try
    End Sub

    Private Sub ShowErrorMessage(message As String)
        Dim alertScript As String = "alertify.alert('" & message & "');"
        ScriptManager.RegisterStartupScript(Me, Page.GetType, "errorLogin", alertScript, True)
    End Sub

#End Region

#Region "PROCESO DE ENCRIPTACIÓN"

    ''' <summary>
    ''' Este proceso sera usado para crear el password el primer administrador del sistema.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Generar_Password_Encriptado()
        Dim messegeText As String = String.Empty
        Try

            Dim password As String = "Ladmin19*"

            Dim passwordEncript As String = HttpUtility.UrlEncode(Encrypt(password))

            Response.Write("<p>Desencriptado: " & Decrypt(HttpUtility.UrlDecode(passwordEncript)) & "</p>")

            Response.Write(String.Format("<P>Encriptado: " & passwordEncript & "</P>"))
        Catch ex As Exception
            messegeText = "alertify.error('" & ex.Message & "');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", messegeText, True)

        End Try
    End Sub

    Private Function Encrypt(clearText As String) As String
        'Dim EncryptionKey As String = conn.Key
        Dim encryptionKey As String = _conn.Key

        Dim clearBytes As Byte() = Encoding.Unicode.GetBytes(clearText)

        Using encryptor As Aes = Aes.Create()
            Dim pdb As New Rfc2898DeriveBytes(encryptionKey, New Byte() {&H49, &H76, &H61, &H6E, &H20, &H4D,
             &H65, &H64, &H76, &H65, &H64, &H65,
             &H76})
            encryptor.Key = pdb.GetBytes(32)
            encryptor.IV = pdb.GetBytes(16)

            Using ms As New MemoryStream()
                Using cs As New CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write)
                    cs.Write(clearBytes, 0, clearBytes.Length)
                    cs.Close()
                End Using
                clearText = Convert.ToBase64String(ms.ToArray())
            End Using

        End Using

        Return clearText

    End Function

    Private Function Decrypt(cipherText As String) As String
        Dim encryptionKey As String = _conn.Key

        cipherText = cipherText.Replace(" ", "+")

        Dim cipherBytes As Byte() = Convert.FromBase64String(cipherText)

        Using encryptor As Aes = Aes.Create()
            Dim pdb As New Rfc2898DeriveBytes(encryptionKey, New Byte() {&H49, &H76, &H61, &H6E, &H20, &H4D,
             &H65, &H64, &H76, &H65, &H64, &H65,
             &H76})

            encryptor.Key = pdb.GetBytes(32)

            encryptor.IV = pdb.GetBytes(16)

            Using ms As New MemoryStream()

                Using cs As New CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write)
                    cs.Write(cipherBytes, 0, cipherBytes.Length)
                    cs.Close()
                End Using

                cipherText = Encoding.Unicode.GetString(ms.ToArray())
            End Using

        End Using

        Return cipherText
    End Function

#End Region

#Region "CONOCER LA INFORMACIÓN DEL NAVEGADOR"

    ''' <summary>
    ''' OBTIENE LA INFORMACIÓN DEL NAVEGADOR
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Browser_data()
        Dim s As String = ""

        With Request.Browser
            s &= "<p>Browser Capabilities" & "</p>" & vbCrLf
            s &= "<p>Type = " & .Type & "</p>" & vbCrLf
            s &= "<p>Name = " & .Browser & "</p>" & vbCrLf
            s &= "<p>Version = " & .Version & "</p>" & vbCrLf
            s &= "<p>Major Version = " & .MajorVersion & "</p>" & vbCrLf
            s &= "<p>Minor Version = " & .MinorVersion & "</p>" & vbCrLf
            s &= "<p>Platform = " & .Platform & "</p>" & vbCrLf
            s &= "<p>Is Beta = " & .Beta & "</p>" & vbCrLf
            s &= "<p>Is Crawler = " & .Crawler & "</p>" & vbCrLf
            s &= "<p>Is AOL = " & .AOL & "</p>" & vbCrLf
            s &= "<p>Is Win16 = " & .Win16 & "</p>" & vbCrLf
            s &= "<p>Is Win32 = " & .Win32 & "</p>" & vbCrLf
            s &= "<p>Supports Frames = " & .Frames & "</p>" & vbCrLf
            s &= "<p>Supports Tables = " & .Tables & "</p>" & vbCrLf
            s &= "<p>Supports Cookies = " & .Cookies & "</p>" & vbCrLf
            s &= "<p>Supports VBScript = " & .VBScript & "</p>" & vbCrLf
            s &= "<p>Supports JavaScript = " & .EcmaScriptVersion.ToString() & "</p>" & vbCrLf
            s &= "<p>Supports Java Applets = " & .JavaApplets & "</p>" & vbCrLf
            s &= "<p>Supports ActiveX Controls = " & .ActiveXControls & "</p>"
        End With

        Response.Write(s)

    End Sub

#End Region

End Class