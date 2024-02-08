Imports System.Data
Imports System.Data.OleDb
Imports System.IO
Imports System.Security.Cryptography

Imports FACTURACION_CLASS

Partial Class herramientas_usuarios
    Inherits Page
    Dim conn As New seguridad
    Dim DataBase As New database

    Private codigoUsuario As String
    Private nombreUsuario As String

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
        If Not IsPostBack Then
            Load_GridView()
            Attributes_Text()
        End If

        'Form.DefaultButton = Me.btnBuscarGrid.FindControl("btnGuardar").UniqueID
        'ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "deleteValidation", "deleteValidation(e)", True)
    End Sub

    Private Sub Attributes_Text()

        Me.txtNombreUsuario.Attributes.Add("pattern", "[a-zA-Z0-9]+")
        Me.txtPassword.Attributes.Add("pattern", "^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\s).{8,30}$")
        Me.txtConfirmarPassword.Attributes.Add("pattern", "^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\s).{8,30}$")

        'Me.btnBuscarGrid.Attributes.Add("formnovalidate", "")
    End Sub

#Region "PROCESOS Y EVENTOS DEL GRIDVIEW"
    ''' <summary>
    ''' CARGA UN LISTADO DE DATOS DENTRO DE UN CONTROL GRIDVIEW
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Load_GridView()
        Try
            ''Thread.CurrentThread.CurrentCulture = New Globalization.CultureInfo("es-NI")

            Using dbCon As New OleDbConnection(conn.conn)
                dbCon.Open()

                Dim sql = $"EXEC sp_usuario 
                            @consecutivo_usuario = null,
                            @cuenta = null,
                            @contrasenia = NULL,
                            @cedula = NULL,
                            @nombre = NULL,
                            @apellido = NULL,
                            @telefono = NULL,
                            @correo = NULL,
                            @direccion = NULL,
                            @cod_rol = NULL,
                            @tipo = 'CONSULTA'"

                Dim ds As DataSet = DataBase.GetDataSet(sql)

                dtTabla = ds.Tables(0)

                GridViewOne.DataSource = dtTabla.DefaultView
                GridViewOne.DataBind()

                ds.Dispose()
            End Using

        Catch ex As Exception
            ltMensajeGrid.Text = conn.PmsgBox("Ocurrio un error al intentar cargar la lista de usuarios. " & ex.Message, "error")
        End Try
    End Sub

    Protected Sub Add(ByVal sender As Object, ByVal e As EventArgs)

        txtNombreUsuario.ReadOnly = False
        txtNombreUsuario.Text = String.Empty
        txtPassword.Text = String.Empty
        txtConfirmarPassword.Text = String.Empty
        txtCedula.Text = String.Empty
        txtNombre.Text = String.Empty
        txtApellido.Text = String.Empty
        txtCorreo.Text = String.Empty
        txtTelefono.Text = String.Empty
        txtDireccion.Text = String.Empty
        ddlRol.SelectedIndex = 0

    End Sub

    Protected Sub Edit(ByVal sender As Object, ByVal e As EventArgs)
        Dim row As GridViewRow = CType(CType(sender, LinkButton).Parent.Parent, GridViewRow)




    End Sub

    Protected Sub GridViewOne_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridViewOne.SelectedIndexChanged

        'Este evento se dispara cuando seleccionamos el boton editar del gridview, antes de editar un usuario para mostrar el popup con la información del usuario.

        Try

            GridViewOne.SelectedRowStyle.CssClass = "table-success"

            hdfCodigo.Value = GridViewOne.SelectedRow.Cells(0).Text.Trim()
            hdfUsuario.Value = GridViewOne.SelectedRow.Cells(1).Text.Trim()

            codigoUsuario = hdfCodigo.Value
            nombreUsuario = hdfUsuario.Value

            'hdfUsuario.Value = HttpUtility.HtmlDecode(Replace(Me.GridViewOne.SelectedRow.Cells(1).Text.Trim, "&nbsp;", String.Empty))

            If hdfCodigo.Value <> Nothing And hdfUsuario.Value <> Nothing Then
                GridViewOne.SelectedRowStyle.CssClass = "table-success"
            End If



        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.PmsgBox("Ocurrió un error al disparar el evento SelectedIndexChanged. " & ex.Message, "error")

        End Try
    End Sub

    Protected Sub GridViewOne_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridViewOne.DataBound
        Try
            If GridViewOne.Rows.Count > 0 Then
                Dim pagerRow As GridViewRow = GridViewOne.BottomPagerRow
                Dim pageLabel As Label = CType(pagerRow.Cells(0).FindControl("CurrentPageLabel"), Label)

                If pageLabel IsNot Nothing Then
                    Dim currentPage As Integer = GridViewOne.PageIndex + 1
                    pageLabel.Text = "&nbsp;&nbsp; Pagina " & currentPage.ToString() &
                        " de " & GridViewOne.PageCount.ToString()
                End If

            End If
        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.PmsgBox("Ocurrió un error al disparar el evento DataBound. " & ex.Message, "error")
        End Try
    End Sub
    Protected Sub GridViewOne_PageIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridViewOne.PageIndexChanged
        Try
            Me.GridViewOne.SelectedIndex = -1
            Me.hdfCodigo.Value = String.Empty

            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "BSTT_SCRIPT", "SetBasicTable();", True)

        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.PmsgBox("Ocurrió un error al disparar el evento PageIndexChanged." & ex.Message, "error")

        End Try
    End Sub
    Protected Sub GridViewOne_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridViewOne.PageIndexChanging
        Try
            If e.NewPageIndex >= 0 Then
                Me.GridViewOne.PageIndex = e.NewPageIndex

                'Para usar la de caché guardada en la variable de sesion
                If (IsPostBack) AndAlso (Not dtTabla Is Nothing) Then
                    If Not dtTabla Is Nothing AndAlso dtTabla.Rows.Count > 0 Then
                        If dtTabla.Rows.Count > 0 Then
                            Me.GridViewOne.DataSource = dtTabla
                            Me.GridViewOne.DataBind()
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.PmsgBox("Ocurrió un error al disparar el evento PageIndexChanging." & ex.Message, "error")

        End Try
    End Sub
    Protected Sub GridViewOne_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridViewOne.RowDataBound
        Try

            If e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Attributes.Add("OnClick", "" & Page.ClientScript.GetPostBackClientHyperlink(Me.GridViewOne, "Select$" + e.Row.RowIndex.ToString) & ";")
            End If

        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.PmsgBox("Ocurrió un error al disparar el evento RowDataBound. " & ex.Message, "error")

        End Try
    End Sub
#End Region

#Region "PROCESO DE ENCRIPTACIÓN"
    Private Function Encrypt(clearText As String) As String

        Dim encryptionKey As String = conn.Key

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

        Dim messageText As String

        If Me.hdfCodigo.Value = String.Empty And Me.txtNombre.Text.Trim = String.Empty Then
            messageText = "alertify.alert('El proceso no puede continuar. Escriba el nombre del usuario.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", messageText, True)
            Exit Sub
        End If

        If Me.hdfCodigo.Value = String.Empty And Me.txtApellido.Text.Trim = String.Empty Then
            messageText = "alertify.alert('El proceso no puede continuar. Escriba el apellido del usuario.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", messageText, True)
            Exit Sub
        End If

        If Me.txtNombreUsuario.Text.Trim = String.Empty Then
            messageText = "alertify.alert('El nombre de usuario no puede estar vacío.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", messageText, True)
            Exit Sub
        End If

        If Me.txtPassword.Text.Trim = String.Empty Then
            messageText = "alertify.alert('La contraseña no puede estar vacía.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", messageText, True)
            Exit Sub
        End If

        If Not Me.txtPassword.Text.Trim = Me.txtConfirmarPassword.Text.Trim Then
            messageText = "alertify.alert('La contraseña no coincide, verifique la información.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", messageText, True)
            Exit Sub
        End If

        If Me.hdfCodigo.Value = String.Empty And DataBase.GetBoleano("EXEC sp_user_exist @usario = '" & Me.txtNombreUsuario.Text.Trim & "' ", "Usuario") = True Then
            messageText = "alertify.alert('El nombre de usuario que intenta agregar ya existe en la base de datos.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", messageText, True)
            Exit Sub
        End If

        Guardar()
        Load_GridView()
    End Sub

    Protected Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        'ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "invokeDelete", "deleteValidation(e)", True)

        Dim hdfDeleteValue As String = hdfDelete.Value

        If hdfDeleteValue = "true" Then
            Eliminar()
            Load_GridView()
        End If
    End Sub

    Private Sub Guardar()

        Dim messageText As String
        Dim passwordEncrypt As String = HttpUtility.UrlEncode(Encrypt(Me.txtPassword.Text.Trim))

        Try

            Using dbCon As New OleDbConnection(conn.conn)
                dbCon.Open()

                Using cmd As New OleDbCommand("sp_Usuario", dbCon)

                    If hdfCodigo.Value = 0.ToString() And hdfUsuario.Value = String.Empty Then

                        cmd.Parameters.AddWithValue("@consecutivo_usuario", DBNull.Value)
                        cmd.Parameters.AddWithValue("@cuenta", txtNombreUsuario.Text.Trim())
                        cmd.Parameters.AddWithValue("@contrasenia", passwordEncrypt)
                        cmd.Parameters.AddWithValue("@nombre", txtNombre.Text.Trim())
                        cmd.Parameters.AddWithValue("@apellido", txtApellido.Text.Trim())
                        cmd.Parameters.AddWithValue("@cedula", txtCedula.Text.Trim())
                        cmd.Parameters.AddWithValue("@correo", IIf(txtCorreo.Text = String.Empty, DBNull.Value, txtCorreo.Text.Trim()))
                        cmd.Parameters.AddWithValue("@telefono", IIf(txtTelefono.Text = String.Empty, DBNull.Value, txtTelefono.Text.Trim()))
                        cmd.Parameters.AddWithValue("@direccion", IIf(txtDireccion.Text = String.Empty, DBNull.Value, txtDireccion.Text.Trim()))
                        cmd.Parameters.AddWithValue("@cod_rol", ddlRol.SelectedValue())
                        cmd.Parameters.AddWithValue("@tipo", "NUEVO")

                    ElseIf hdfCodigo.Value = GridViewOne.SelectedRow.Cells(0).Text.Trim() And hdfUsuario.Value = GridViewOne.SelectedRow.Cells(1).Text.Trim() Then

                        cmd.Parameters.AddWithValue("@consecutivo_usuario", hdfCodigo.Value.Trim())
                        cmd.Parameters.AddWithValue("@cuenta", DBNull.Value)
                        cmd.Parameters.AddWithValue("@contrasenia", passwordEncrypt)
                        cmd.Parameters.AddWithValue("@nombre", DBNull.Value)
                        cmd.Parameters.AddWithValue("@apellido", DBNull.Value)
                        cmd.Parameters.AddWithValue("@cedula", DBNull.Value)
                        cmd.Parameters.AddWithValue("@correo", DBNull.Value)
                        cmd.Parameters.AddWithValue("@telefono", DBNull.Value)
                        cmd.Parameters.AddWithValue("@direccion", DBNull.Value)
                        cmd.Parameters.AddWithValue("@cod_rol", DBNull.Value)
                        cmd.Parameters.AddWithValue("@tipo", "CHANGE PASSWORD")

                    End If

                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.ExecuteNonQuery()

                    messageText = "alertify.success('El registro ha sido guardado de forma correcta.');"
                    ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "message", messageText, True)

                End Using

            End Using

        Catch ex As Exception
            messageText = "alertify.error('Ha ocurrido un error al intentar guardar los datos. Si el problema persiste, contacte con el administrador.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "message", messageText, True)

        End Try
    End Sub

    Private Sub Eliminar()
        Dim messageText As String
        Try
            Using dbCon As New OleDbConnection(conn.conn)
                dbCon.Open()
                Using cmd As New OleDbCommand("sp_Usuario", dbCon)
                    cmd.Parameters.AddWithValue("@consecutivo_usuario", Me.hdfCodigo.Value.Trim())
                    cmd.Parameters.AddWithValue("@cuenta", DBNull.Value)
                    cmd.Parameters.AddWithValue("@contrasenia", DBNull.Value)
                    cmd.Parameters.AddWithValue("@cedula", DBNull.Value)
                    cmd.Parameters.AddWithValue("@nombre", DBNull.Value)
                    cmd.Parameters.AddWithValue("@apellido", DBNull.Value)
                    cmd.Parameters.AddWithValue("@telefono", DBNull.Value)
                    cmd.Parameters.AddWithValue("@correo", DBNull.Value)
                    cmd.Parameters.AddWithValue("@direccion", DBNull.Value)
                    cmd.Parameters.AddWithValue("@cod_rol", DBNull.Value)
                    cmd.Parameters.AddWithValue("@tipo", "BAJA")
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.ExecuteNonQuery()
                End Using
            End Using

            messageText = "alertify.success('El usuario ha sido eliminado de forma correcta.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "message", messageText, True)

        Catch ex As Exception
            messageText = "alertify.error(`Ha ocurrido un error al intentar guardar los datos. Si el problema persiste, contacte con el administrador.');"
            messageText += ex.Message
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "message", messageText, True)
        End Try

    End Sub

    'Protected Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click

    '    Me.hdfCodigo.Value = String.Empty
    '    Me.txtNombreUsuario.Text = String.Empty
    '    Me.txtPassword.Text = String.Empty
    '    Me.txtConfirmarPassword.Text = String.Empty
    '    Me.txtCedula.Text = String.Empty
    '    Me.txtNombre.Text = String.Empty
    '    Me.txtApellido.Text = String.Empty
    '    Me.txtTelefono.Text = String.Empty
    '    Me.txtCorreo.Text = String.Empty
    '    Me.txtDireccion.Text = String.Empty

    '    Me.GridViewOne.SelectedIndex = -1
    '    Me.hdfCodigo.Value = String.Empty
    '    Me.hdfUsuario.Value = String.Empty

    '    'btnNuevo.Attributes.Add("data-bs-toggle", "modal")
    '    'btnNuevo.Attributes.Add("data-bs-target", "#staticBackdrop")
    '    'staticBackdropLabel.InnerText = "Nuevo Usuario"

    '    'ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "revertPopup", "revertPopup();", True)

    'End Sub
#End Region

End Class
