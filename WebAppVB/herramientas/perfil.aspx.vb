Imports System.Data
Imports System.IO
Imports System.Security.Cryptography
Imports WebAppVB.AlertifyClass
Imports FACTURACION_CLASS

Partial Class herramientas_usuario_editar
    Inherits Page
    Dim conn As New seguridad
    Dim DataBase As New database

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
        txtUsuario.Attributes.Add("autocomplete", "off")

        txtPasswordActual.Attributes.Add("required", "required")
        txtPasswordActual.Attributes.Add("pattern", "^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\s).*$")
        txtPasswordActual.Attributes.Add("autocomplete", "off")

        txtPassword.Attributes.Add("required", "required")
        txtPassword.Attributes.Add("pattern", "^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\s).*$")
        txtPassword.Attributes.Add("autocomplete", "off")

        txtConfirmarPassword.Attributes.Add("required", "required")
        txtConfirmarPassword.Attributes.Add("pattern", "^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\s).*$")
        txtConfirmarPassword.Attributes.Add("autocomplete", "off")

        txtCedula.Attributes.Add("autocomplete", "off")

        txtNombre.Attributes.Add("autocomplete", "off")

        txtApellido.Attributes.Add("autocomplete", "off")

        txtTelefono.Attributes.Add("autocomplete", "off")

        txtCorreo.Attributes.Add("autocomplete", "off")

        txtDireccion.Attributes.Add("autocomplete", "off")
    End Sub

    Private Sub Load_Usuario_Data()
        Try
            Dim sql = " EXEC sp_usuario " &
                  "@consecutivo_usuario = " & Session("CodigoUser") & "," &
                  "@cuenta = null," &
                  "@contrasenia = null," &
                  "@cedula = NULL," &
                  "@nombre = NULL," &
                  "@apellido = NULL," &
                  "@telefono = NULL," &
                  "@correo = NULL," &
                  "@direccion = NULL," &
                  "@cod_rol = NULL," &
                  "@tipo = 'READ' "

            Using dt As DataTable = DataBase.GetDataTable(sql)
                If dt.Rows.Count > 0 Then
                    ' Accessing the first row of the DataTable
                    Dim row As DataRow = dt.Rows(0)

                    ' Accessing data from columns in the DataRow
                    txtUsuario.Text = row("Username").ToString()
                    txtCedula.Text = row("cedula").ToString()
                    txtNombre.Text = row("nombre").ToString()
                    txtApellido.Text = row("apellido").ToString()
                    txtRol.Text = row("Rol").ToString()
                    txtTelefono.Text = row("telefono").ToString()
                    txtCorreo.Text = row("email").ToString()
                    txtDireccion.Text = row("direccion").ToString()
                End If
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

#Region "PROCESO DE ENCRIPTACIÓN"
    Private Function Encrypt(clearText As String) As String
        Dim EncryptionKey As String = conn.Key

        Dim clearBytes As Byte() = Encoding.Unicode.GetBytes(clearText)

        Using encryptor As Aes = Aes.Create()
            Dim pdb As New Rfc2898DeriveBytes(EncryptionKey, New Byte() {&H49, &H76, &H61, &H6E, &H20, &H4D,
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
        Dim EncryptionKey As String = conn.Key

        cipherText = cipherText.Replace(" ", "+")

        Dim cipherBytes As Byte() = Convert.FromBase64String(cipherText)

        Using encryptor As Aes = Aes.Create()
            Dim pdb As New Rfc2898DeriveBytes(EncryptionKey, New Byte() {&H49, &H76, &H61, &H6E, &H20, &H4D,
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

#Region "ENVIAR DATOS A SQL SERVER"
    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click

        If txtCorreo.Text.Trim = String.Empty Then
            AlertifyErrorMessage(Me, "El proceso no puede continuar. La dirección de correo es un campo requerido.")
            Return
        End If

        If txtTelefono.Text.Trim = String.Empty Then
            AlertifyErrorMessage(Me, "El proceso no puede continuar. La dirección de correo es un campo requerido.")
            Return
        End If

        Dim sql = $"EXEC sp_usuario
                @consecutivo_usuario = {Session("CodigoUser")},
                @cuenta = null,
                @contrasenia = null,
                @cedula = {(If(txtCedula.Text = String.Empty, "NULL", $"'{txtCedula.Text.Trim()}'"))},
                @nombre = {(If(txtNombre.Text = String.Empty, "NULL", $"'{txtNombre.Text.Trim()}'"))},
                @apellido = {(If(txtApellido.Text = String.Empty, "NULL", $"'{txtApellido.Text.Trim()}'"))},
                @telefono = {(If(txtTelefono.Text = String.Empty, "NULL", $"'{txtTelefono.Text.Trim()}'"))},
                @correo = {(If(txtCorreo.Text = String.Empty, "NULL", $"'{txtCorreo.Text.Trim()}'"))},
                @direccion = {(If(txtDireccion.Text = String.Empty, "NULL", $"'{txtDireccion.Text.Trim()}'"))},
                @cod_rol = null,
                @tipo = 'ACTUALIZAR'"

        Dim Guardar = DataBase.SaveToDatabase(sql)

        If Guardar Then
            AlertifySuccessMessage(Me, "Cambios Guardados Correctamente")
        Else
            AlertifyErrorMessage(Me, "Ocurrió un error al guardar los cambios. Verifique la información o contacte con un administrador.")
        End If

    End Sub

    Protected Sub btnGuardarPassword_Click(sender As Object, e As EventArgs) Handles btnGuardarPassword.Click
        If txtPasswordActual.Text.Trim = String.Empty Then
            AlertifyErrorMessage(Me, "El proceso no puede continuar.  La contraseña Actual es un campo requerido.")
            Exit Sub
        End If

        If txtPassword.Text.Trim = String.Empty Then
            AlertifyErrorMessage(Me, "El proceso no puede continuar. La contraseña no puede estar vacía.")
            Exit Sub
        End If

        If txtPassword.Text.Trim <> txtConfirmarPassword.Text.Trim Then
            AlertifyErrorMessage(Me, "El proceso no puede continuar. Las contraseñas no coinciden.")
            Exit Sub
        End If

        Dim password_encript As String = HttpUtility.UrlEncode(Encrypt(txtPasswordActual.Text.Trim()))

        If DataBase.GetBoleano("EXEC sp_user_passsword @codusuario = " & Session("CodigoUser") & ", @password = '" & password_encript & "' ", "Usuario") = False Then
            AlertifyErrorMessage(Me, "La contraseña ingresada no coincide con los datos actuales. Por favor Inténtelo nuevamente.")
            Exit Sub
        End If

        password_encript = HttpUtility.UrlEncode(Encrypt(Me.txtPassword.Text.Trim))

        Dim sql = " EXEC sp_usuario " &
              "@consecutivo_usuario = " & Session("CodigoUser") & "," &
              "@cuenta = null," &
              "@contrasenia = '" & password_encript & "'," &
              "@cedula = null," &
              "@nombre = null," &
              "@apellido = null," &
              "@telefono = null," &
              "@correo = null," &
              "@direccion = null," &
              "@cod_rol = null," &
              "@tipo = 'CHANGE PASSWORD' "

        Dim Guardar = DataBase.SaveToDatabase(sql) 'GUARDA LOS DATOS EN LA BASE DE DATOS

        If Guardar Then
            AlertifySuccessMessage(Me, "Cambios Guardados Correctamente")
        Else
            AlertifyErrorMessage(Me, "Ocurrió un error al guardar los cambios. Verifique la información o contacte con un administrador.")
        End If

    End Sub

#End Region
End Class
