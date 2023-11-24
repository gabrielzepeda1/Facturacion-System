Imports System.Data
Imports System.Globalization
Imports System.Threading
Imports System.Net
Imports System.IO
Imports System.Text
Imports System.Security.Cryptography
Imports IpPublicKnowledge

Partial Class herramientas_usuario_editar
    Inherits System.Web.UI.Page
    Dim conn As New FACTURACION_CLASS.seguridad
    Dim DataBase As New FACTURACION_CLASS.database

#Region "PROPIEDADES DEL FORMULARIO"
    ''' <summary>
    ''' UTILIZADO PARA LLENAR EL CONTROL DATAGridViewOne
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Property dtTabla() As DataTable
        Get
            Return ViewState("dtTabla")
        End Get
        Set(ByVal value As DataTable)
            ViewState("dtTabla") = value
        End Set
    End Property

    Dim _Name As String = String.Empty
    Private Property Name() As String
        Get
            Dim arrPath() As String = HttpContext.Current.Request.RawUrl.Split("/")

            _Name = arrPath(arrPath.GetUpperBound(0))

            Return _Name
        End Get
        Set(ByVal value As String)
            _Name = value
        End Set
    End Property
#End Region

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            'Form.DefaultButton = Me.btnBuscarGrid.FindControl("btnGuardar").UniqueID

            Attributes_Text()

            Load_Usuario_Data()
        End If
    End Sub

    Private Sub Attributes_Text()
        Me.txtUsuario.Attributes.Add("placeholder", "Nombre de Usuario")
        Me.txtUsuario.Attributes.Add("autocomplete", "off")

        Me.txtPasswordActual.Attributes.Add("placeholder", "Contraseña Actual")
        Me.txtPasswordActual.Attributes.Add("required", "required")
        Me.txtPasswordActual.Attributes.Add("pattern", "^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\s).*$")
        Me.txtPasswordActual.Attributes.Add("autocomplete", "off")

        Me.txtPassword.Attributes.Add("placeholder", "Nueva Contraseña")
        Me.txtPassword.Attributes.Add("required", "required")
        Me.txtPassword.Attributes.Add("pattern", "^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\s).*$")
        Me.txtPassword.Attributes.Add("autocomplete", "off")

        Me.txtConfirmarPassword.Attributes.Add("placeholder", "Confirmar Contraseña")
        Me.txtConfirmarPassword.Attributes.Add("required", "required")
        Me.txtConfirmarPassword.Attributes.Add("pattern", "^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\s).*$")
        Me.txtConfirmarPassword.Attributes.Add("autocomplete", "off")


        Me.txtCedula.Attributes.Add("placeholder", "Escriba el numero de Cedula")
        Me.txtCedula.Attributes.Add("autocomplete", "off")

        Me.txtNombre.Attributes.Add("placeholder", "Nombre de la persona")
        Me.txtNombre.Attributes.Add("autocomplete", "off")

        Me.txtTelefono.Attributes.Add("placeholder", "Teléfono de la persona")
        Me.txtTelefono.Attributes.Add("autocomplete", "off")

        Me.txtCorreo.Attributes.Add("placeholder", "Correo de la persona")
        Me.txtCorreo.Attributes.Add("autocomplete", "off")

        Me.txtDireccion.Attributes.Add("placeholder", "Dirección de la persona")
        Me.txtDireccion.Attributes.Add("autocomplete", "off")
    End Sub

    Private Sub Load_Usuario_Data()
        Try
            Me.ltMensaje.Text = String.Empty

            Dim SQL As String = String.Empty
            SQL = " EXEC sp_usuario " & _
                      "@consecutivo = " & Request.Cookies("CKSMFACTURA")("cod_usuario") & "," & _
                      "@cuenta = null," & _
                      "@contrasenia = null," & _
                      "@cedula = NULL," & _
                      "@nombre = NULL," & _
                      "@apellido = NULL," & _
                      "@telefono = NULL," & _
                      "@correo = NULL," & _
                      "@direccion = NULL," & _
                      "@tipo = 'READ' "

            Dim dr As System.Data.OleDb.OleDbDataReader = DataBase.GetDataReader(SQL)
            If dr.Read() Then
                'dr.Item("").ToString()
                Me.txtUsuario.Text = dr.Item("USUARIO").ToString()
                Me.txtCedula.Text = dr.Item("Cedula").ToString()
                Me.txtNombre.Text = dr.Item("NOMBRE").ToString()
                Me.txtTelefono.Text = dr.Item("TELEFONO").ToString()
                Me.txtCorreo.Text = dr.Item("CORREO").ToString()
                Me.txtDireccion.Text = dr.Item("direccion").ToString()
            End If

            dr.Close()

        Catch ex As Exception
            Me.ltMensaje.Text = conn.pmsgBox(ex.Message, "error")

        End Try
    End Sub


#Region "PROCESO DE ENCRIPTACIÓN"
    Private Function Encrypt(clearText As String) As String
        Dim EncryptionKey As String = conn.Key

        Dim clearBytes As Byte() = Encoding.Unicode.GetBytes(clearText)

        Using encryptor As Aes = Aes.Create()
            Dim pdb As New Rfc2898DeriveBytes(EncryptionKey, New Byte() {&H49, &H76, &H61, &H6E, &H20, &H4D, _
             &H65, &H64, &H76, &H65, &H64, &H65, _
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
        Dim EncryptionKey As String = conn.Key

        cipherText = cipherText.Replace(" ", "+")

        Dim cipherBytes As Byte() = Convert.FromBase64String(cipherText)

        Using encryptor As Aes = Aes.Create()
            Dim pdb As New Rfc2898DeriveBytes(EncryptionKey, New Byte() {&H49, &H76, &H61, &H6E, &H20, &H4D, _
             &H65, &H64, &H76, &H65, &H64, &H65, _
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


#Region "ENVIAR DATOS A SQL SERVER"
    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim MessegeText As String = String.Empty

        If Me.txtCorreo.Text.Trim = String.Empty Then
            MessegeText = "alertify.alert('El proceso no puede continuar. La dirección de correo es un campo requerido.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)
            Exit Sub
        End If

        If Me.txtTelefono.Text.Trim = String.Empty Then
            MessegeText = "alertify.alert('El proceso no puede continuar. Debe ingresar un numero de contacto.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)
            Exit Sub
        End If

        Dim sql As String = String.Empty
        sql = " EXEC sp_usuario " & _
              "@consecutivo = " & Request.Cookies("CKSMFACTURA")("cod_usuario") & "," & _
              "@cuenta = null," & _
              "@contrasenia = null," & _
              "@cedula = null," & _
              "@nombre = null," & _
              "@apellido = null," & _
              "@telefono = " & IIf(Me.txtTelefono.Text = String.Empty, "NULL", "'" & Me.txtTelefono.Text.Trim & "'") & "," & _
              "@correo = " & IIf(Me.txtCorreo.Text = String.Empty, "NULL", "'" & Me.txtCorreo.Text.Trim & "'") & "," & _
              "@direccion = " & IIf(Me.txtDireccion.Text = String.Empty, "NULL", "'" & Me.txtDireccion.Text.Trim & "'") & "," & _
              "@tipo = 'ACTUALIZAR' "

        Guardar(sql)

    End Sub

    Protected Sub btnGuardarPassword_Click(sender As Object, e As EventArgs) Handles btnGuardarPassword.Click
        Dim MessegeText As String = String.Empty

        If Me.txtPasswordActual.Text.Trim = String.Empty Then
            MessegeText = "alertify.alert('El proceso no puede continuar. La contraseña Actual es un campo requerido.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)
            Exit Sub
        End If

        If Me.txtPassword.Text.Trim = String.Empty Then
            MessegeText = "alertify.alert('La contraseña no puede estar vacía.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)
            Exit Sub
        End If

        If Not Me.txtPassword.Text.Trim = Me.txtConfirmarPassword.Text.Trim Then
            MessegeText = "alertify.alert('La contraseña no coincide,  verifique la información.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)
            Exit Sub
        End If

        Dim password_encript As String = HttpUtility.UrlEncode(Encrypt(Me.txtPasswordActual.Text.Trim))

        If DataBase.GetBoleano("EXEC sp_user_passsword @codusuario = " & Request.Cookies("CKGEIN")("cod_usuario") & ", @password = '" & password_encript & "' ", "Usuario") = False Then
            MessegeText = "alertify.alert('La contraseña ingresada no coincide con los datos actuales. Por favor Inténtelo nuevamente.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)
            Exit Sub
        End If

        Dim sql As String = String.Empty

        password_encript = HttpUtility.UrlEncode(Encrypt(Me.txtPassword.Text.Trim))

        sql = " EXEC sp_usuario " & _
              "@consecutivo = " & Request.Cookies("CKSMFACTURA")("cod_usuario") & "," & _
              "@cuenta = null," & _
              "@contrasenia = '" & password_encript & "'," & _
              "@cedula = null," & _
              "@nombre = null," & _
              "@apellido = null," & _
              "@telefono = null," & _
              "@correo = null," & _
              "@direccion = null," & _
              "@tipo = 'CHANGE PASSWORD' "

        Guardar(sql)

    End Sub

    Private Sub Guardar(query As String)
        Dim MessegeText As String = String.Empty

        Dim dbCon As New System.Data.OleDb.OleDbConnection(conn.conn)
        Try
            If dbCon.State = ConnectionState.Closed Then
                dbCon.Open()
            End If

            Dim cmd As New OleDb.OleDbCommand(query, dbCon)
            cmd.ExecuteNonQuery()

            MessegeText = "alertify.success('El registro ha sido guardado de forma correcta.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)

        Catch ex As Exception
            MessegeText = "alertify.error('Ha ocurrido un error al intentar guardar los datos. Si el problema persiste, contacte con el administrador. " & ex.Message & "');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)

        Finally
            If dbCon.State = ConnectionState.Open Then
                dbCon.Close()
            End If

        End Try
    End Sub
#End Region
End Class
