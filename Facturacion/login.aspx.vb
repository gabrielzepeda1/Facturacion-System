Imports System.Data.OleDb
Imports System.IO
Imports System.Security.Cryptography
Imports FACTURACION_CLASS
Imports Microsoft.ReportingServices.DataProcessing
Imports AlertifyClass
Imports System.Data
Partial Class Login
    Inherits Page
    Dim _conn As New seguridad
    Dim _database As New database

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Attributes_Text()
        End If
    End Sub

    Private Sub Attributes_Text()
        txtUsuario.Attributes.Add("placeholder", "Nombre de Usuario")
        txtUsuario.Attributes.Add("autocomplete", "off")
        txtPass.Attributes.Add("placeholder", "Contraseña")
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
        UserLogin()
    End Sub

    ''''<summary>Ejecuta el stored procedure "sp_sys_login" en SQL SERVER. Si el SWITCH case es ELSE, el usuario es valido.</summary>
    Protected Sub UserLogin()

        Dim Username As String = txtUsuario.Text.ToString()
        Dim CodigoUser As Integer = 0
        Dim CodigoRol As Integer = 0
        Dim browserData As String = Request.Browser.Browser & " " & Request.Browser.Version & " " & Request.Browser.Platform

        Try
            Using dbCon As New OleDbConnection(_conn.conn)
                dbCon.Open()

                Using cmd As New OleDbCommand("sp_sys_login", dbCon)
                    cmd.CommandType = Data.CommandType.StoredProcedure
                    cmd.Parameters.AddWithValue("@Username", Username)
                    cmd.Parameters.AddWithValue("@Password", HttpUtility.UrlEncode(Encrypt(Me.txtPass.Text.Trim())))
                    Dim dr As OleDbDataReader = cmd.ExecuteReader()
                    If dr.Read() Then
                        CodigoUser = Convert.ToInt32(dr("CodigoUser"))
                        If dr("CodigoRol") IsNot DBNull.Value Then
                            CodigoRol = Convert.ToInt32(dr("CodigoRol"))
                        End If
                    End If
                End Using

                Select Case CodigoUser
                    Case -1
                        AlertifyErrorMessage(Me, "Usuario y/o contraseña incorrect@s.")
                    Case -2
                        AlertifyErrorMessage(Me, "Usuario ya tiene una sesión activa.")
                    Case Else
                        'Usuario inicia sesión correctamente
                        Session("CodigoUser") = CodigoUser
                        Session("CodigoRol") = CodigoRol
                        HandleUserSession(Username, browserData)
                End Select
            End Using

        Catch ex As Exception
            AlertifyErrorMessage(Me, ex.Message)
        End Try
    End Sub
    Protected Sub HandleUserSession(username As String, browserData As String)
        'Maneja la sesion del usuario ya autenticado.
        Dim sessionData As Dictionary(Of String, Object) = _conn.ControlarSesion(username, ObtenerIp_Publica(), browserData)

        Try
            If sessionData IsNot Nothing Then
                If sessionData.ContainsKey("Status") AndAlso sessionData.Item("Status") = "SESION INICIADA" Then

                    Session("CodigoSesion") = sessionData.Item("CodigoSesion").ToString()
                    Session("Username") = sessionData.Item("Username").ToString()
                    Session("Password") = sessionData.Item("Password").ToString()
                    Session("CodigoPais") = Convert.ToInt32(sessionData.Item("CodigoPais"))
                    Session("CodigoEmpresa") = Convert.ToInt32(sessionData.Item("CodigoEmpresa"))
                    Session("CodigoPuesto") = Convert.ToInt32(sessionData.Item("CodigoPuesto"))

                    Select Case Session("CodigoRol")
                        'Rol SuperAdmin, AdminPais, AdminEmpresa, AdminPuesto
                        Case 1, 2, 3, 4
                            FormsAuthentication.RedirectFromLoginPage(Session("Username"), False)
                        Case Else
                            Response.Redirect("~/Utilitarios/PaisEmpresaPuesto.aspx")
                    End Select
                Else
                    'Usuario tiene una sesion activa
                    AlertifyAlertMessage(Me, sessionData.Item("Status"))
                End If
            End If
        Catch ex As Exception
            AlertifyErrorMessage(Me, ex.Message)
        End Try
    End Sub

    Private Sub SetSessionVariableArray()

        If Session("CodigoPais") = 0 Then
            Dim dt = _database.GetDataTable($"SELECT * FROM GetPaises({Session("CodigoUser")})")
            Dim ArrCodigoPais As New List(Of Integer)

            For Each row As DataRow In dt.Rows
                ArrCodigoPais.Add(row("CodigoPais"))
            Next

            Session("CodigoPais") = ArrCodigoPais

        End If
        'Asignar al Session("CodigoPais") un array de todos los CodigoPais en la base de datos.
        'Ex: Session("CodigoPais") = [1, 2, 3, 4],  En SQL se utilizaria así: "WHERE IN (1, 2, 3, 4)



    End Sub

    'Private Function GetCodigoPaisUser(CodigoUser As Integer) As Integer
    '    Using dbCon As New OleDbConnection(_conn.conn)
    '        dbCon.Open()

    '        Dim sql = $"SELECT dbo.GetCod_PaisUsuario({CodigoUser})"
    '        Dim CodigoPais = _database.GetScalar(sql)

    '        If CodigoPais IsNot DBNull.Value Then
    '            Return Convert.ToInt32(CodigoPais)
    '        End If
    '    End Using

    '    Return Nothing
    'End Function

    'Private Function GetCodigoEmpresaUser(CodigoUser As Integer) As Integer
    '    Using dbCon As New OleDbConnection(_conn.conn)
    '        dbCon.Open()

    '        Dim sql = $"SELECT dbo.GetCodigoEmpresaUsuario({CodigoUser})"
    '        Dim CodigoEmpresa = _database.GetScalar(sql)

    '        If CodigoEmpresa IsNot DBNull.Value Then
    '            Return Convert.ToInt32(CodigoEmpresa)
    '        End If
    '    End Using

    '    Return Nothing
    'End Function

    'Private Function GetCodigoPuestoUser(CodigoUser As Integer) As Integer
    '    Using dbCon As New OleDbConnection(_conn.conn)
    '        dbCon.Open()

    '        Dim sql = $"SELECT dbo.GetCodigoPuestoUser({CodigoUser})"
    '        Dim CodigoPais = _database.GetScalar(sql)

    '        If CodigoPais IsNot DBNull.Value Then
    '            Return Convert.ToInt32(CodigoPais)
    '        End If
    '    End Using

    '    Return Nothing
    'End Function

#End Region

#Region "PROCESO DE ENCRIPTACIÓN"

    ''' <summary>
    ''' Este proceso sera usado para crear el password el primer administrador del sistema.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Generar_Password_Encriptado()
        Try
            Dim password = "Ladmin19*"
            Dim passwordEncript As String = HttpUtility.UrlEncode(Encrypt(password))

            Response.Write("<p>Desencriptado: " & Decrypt(HttpUtility.UrlDecode(passwordEncript)) & "</p>")
            Response.Write(String.Format("<P>Encriptado: " & passwordEncript & "</P>"))

        Catch ex As Exception
            Dim messegeText = "alertify.error('" & ex.Message & "');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", messegeText, True)
        End Try
    End Sub

    Private Function Encrypt(clearText As String) As String
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