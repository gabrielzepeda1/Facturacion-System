Imports System.Data
Imports System.Data.OleDb
Imports System.Diagnostics
Imports FACTURACION_CLASS

Partial Class Utilitarios_PaisEmpresaPuesto
    Inherits Page

    Dim _conn As New seguridad
    Dim DataBase As New database

    Public CompanyName As String = "Facturación Local - Industrial Comercial San Martín"
    Public MyUserName As String

#Region "PROPIEDADES DEL FORMULARIO"

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

    Dim _Puesto_Name As String = String.Empty

    Private Property Puesto_Name() As String
        Get

            Return _Puesto_Name
        End Get
        Set(ByVal value As String)
            _Puesto_Name = value
        End Set
    End Property

#End Region

#Region "DESHABILITAR BOTON CERRAR"

    '<System.ComponentModel.DefaultValue(False), System.ComponentModel.Description("Define si se habilita el boton cerrar en el formulario")> _
    'Public Property EnabledCerrar() As Boolean
    '    Get
    '        Return _enabledCerrar
    '    End Get
    '    Set(ByVal Value As Boolean)
    '        If _enabledCerrar <> Value Then
    '            _enabledCerrar = Value
    '        End If
    '    End Set
    'End Property

    'Protected ReadOnly Property CreateParams() As CreateParams
    '    Get
    '        Dim cp As CreateParams = MyBase.CreateParams

    '        If _enabledCerrar = False Then
    '            Const CS_NOCLOSE As Integer = &H200
    '            cp.ClassStyle = cp.ClassStyle Or CS_NOCLOSE
    '        End If
    '        Return cp
    '    End Get
    'End Property

#End Region

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            If Request.Cookies.Get("CKSMFACTURA") Is Nothing Then
                Response.Redirect(ResolveClientUrl("~/Login.aspx"))
            End If

            Dim cookie As HttpCookie = Request.Cookies("CKSMFACTURA")

            If cookie IsNot Nothing Then
                MyUserName = "USUARIO: " & Context.Request.Cookies("CKSMFACTURA")("Username")
            End If

            If WatchSession() = False Then
                Debug.WriteLine("Sesión cerrada")
            End If
        End If
    End Sub

    'REMEMBER SUB PROCEDURES HAVE THEIR OWN SCOPE (VARIABLES, OBJECTS, CONSTANTS)
    'Private Sub Load_GridView()
    '    Try

    '        Dim vUss As String = String.Empty
    '        vUss = Context.Request.Cookies("CKSMFACTURA")("cod_usuario") 'Variable initialized using the 'Request.Cookies' method with a value of CKSMFACTURA.
    '        Dim SQL As String = String.Empty
    '        SQL &= "EXEC CombosProductos " &
    '              " @opcion = 20," &
    '              " @codigo = " & vUss & " "

    '        Dim ds As DataSet 'ds varaible is declared as a DATASET'
    '        ds = DataBase.GetDataSet(SQL) 'ds is assigned the value from the DataBase CLASS, using the GetDataSet method, which receives the SQL string.

    '        dtTabla = ds.Tables(0)

    '        '' VERIFICA QUE EL DATATABLE TENGA DATOS
    '        If dtTabla.Rows.Count > 0 Then

    '            If dtTabla.Rows.Count > 1 Then
    '                '' SI ES MAYOR A 1, ENTONCES PERMITE QUE EL USUARIO ELIJA
    '                Me.GridViewOne.DataSource = dtTabla.DefaultView
    '                Me.GridViewOne.DataBind()
    '            Else

    '                '' SI ES IGUAL A 1, ENTONCES CARGA LOS DATOS POR DEFECTO
    '                If Session("Pais") = String.Empty Or Session("Empresa") = String.Empty Or Session("Puesto") = String.Empty Or
    '                Session("cod_pais") = String.Empty Or Session("cod_empresa") = String.Empty Or Session("cod_puesto") = String.Empty Then

    '                    Session("Pais") = dtTabla.Rows(0).Item("Pais").ToString.Trim
    '                    Session("Empresa") = dtTabla.Rows(0).Item("Empresa").ToString.Trim
    '                    Session("Puesto") = dtTabla.Rows(0).Item("Puesto").ToString.Trim
    '                    Session("cod_pais") = dtTabla.Rows(0).Item("cod_pais").ToString.Trim
    '                    Session("cod_empresa") = dtTabla.Rows(0).Item("cod_empresa").ToString.Trim
    '                    Session("cod_puesto") = dtTabla.Rows(0).Item("cod_puesto").ToString.Trim
    '                    Session("VerifInven") = dtTabla.Rows(0).Item("verificar_inventario").ToString.Trim
    '                    Session("Ruc") = dtTabla.Rows(0).Item("cedula_ruc").ToString.Trim
    '                    Session("mifin") = dtTabla.Rows(0).Item("autorizacion_mifin").ToString.Trim
    '                    Session("direccion") = dtTabla.Rows(0).Item("direccion").ToString.Trim
    '                    Session("telefono") = dtTabla.Rows(0).Item("telefono").ToString.Trim
    '                End If
    '                '' =================================================================================================================

    '                Response.Redirect(ResolveClientUrl("../Default.aspx"))

    '            End If

    '        End If

    '        ds.Dispose()

    '        If Not Me.GridViewOne.Rows.Count > 0 Then
    '            MsgBox("No se encontró ningun pais o puesto asociado con el usuario, favor pónganse en contacto con el administrador del sistema.", MsgBoxStyle.Information, "Sistema de Facturacion")
    '        End If
    '    Catch ex As Exception
    '        Me.ltMensajeGrid.Text &= _conn.pmsgBox("Ocurrió un error al intentar cargar el grid de pais,empresa,puestos." & ex.Message, "error")

    '    End Try
    'End Sub

    'Protected Sub GridViewOne_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridViewOne.RowDataBound

    'End Sub

    'Protected Sub GridViewOne_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridViewOne.SelectedIndexChanged
    '    Dim vcod_sesion As String = Context.Request.Cookies("CKSMFACTURA")("cod_sesion")
    '    Dim vcod_usuario As String = Context.Request.Cookies("CKSMFACTURA")("cod_usuario")
    '    Dim vNombre_Usuario As String = Context.Request.Cookies("CKSMFACTURA")("Nombre_Usuario")
    '    Dim vcontrasenia As String = Context.Request.Cookies("CKSMFACTURA")("contrasenia")

    '    'Me.hdfCodigo.Value = Me.GridViewOne.SelectedValue.ToString
    '    Session("Pais") = Me.GridViewOne.SelectedRow.Cells(0).Text
    '    Session("Empresa") = Me.GridViewOne.SelectedRow.Cells(1).Text
    '    Session("Puesto") = Me.GridViewOne.SelectedRow.Cells(2).Text

    '    Session("cod_pais") = Me.GridViewOne.SelectedDataKey.Item(0).ToString()
    '    Session("cod_empresa") = Me.GridViewOne.SelectedDataKey.Item(1).ToString()
    '    Session("cod_puesto") = Me.GridViewOne.SelectedDataKey.Item(2).ToString()
    '    'grdcollcetion.SelectedRow.Cells(0).Text.ToString()

    '    'Session("cod_pais") = Me.GridViewOne.SelectedRow.Cells(3).Text
    '    'Session("cod_empresa") = Me.GridViewOne.SelectedRow.Cells(4).Text
    '    'Session("cod_puesto") = Me.GridViewOne.SelectedRow.Cells(5).Text

    '    Session("lista") = Me.GridViewOne.SelectedRow.Cells(7).Text
    '    Session("CambiaPrecio") = Me.GridViewOne.SelectedRow.Cells(6).Text
    '    Session("VerifInven") = Me.GridViewOne.SelectedRow.Cells(8).Text
    '    Session("porcImp") = Me.GridViewOne.SelectedRow.Cells(9).Text

    '    Session("Ruc") = Me.GridViewOne.SelectedRow.Cells(10).Text
    '    Session("mifin") = Me.GridViewOne.SelectedRow.Cells(11).Text
    '    Session("direccion") = Me.GridViewOne.SelectedRow.Cells(12).Text
    '    Session("telefono") = Me.GridViewOne.SelectedRow.Cells(13).Text

    '    Dim CKSMFACTURA As HttpCookie = Request.Cookies.Get("CKSMFACTURA")
    '    'Dim CKSMFACTURA As HttpCookie = New HttpCookie("CKSMFACTURA")

    '    CKSMFACTURA("desEmpre") = Me.GridViewOne.SelectedRow.Cells(1).Text
    '    CKSMFACTURA("desPais") = Me.GridViewOne.SelectedRow.Cells(0).Text
    '    CKSMFACTURA("desPuesto") = Me.GridViewOne.SelectedRow.Cells(2).Text

    '    'CKSMFACTURA("CodPuesto") = Me.GridViewOne.SelectedRow.Cells(5).Text
    '    'CKSMFACTURA("codPais") = Me.GridViewOne.SelectedRow.Cells(3).Text
    '    'CKSMFACTURA("CodEmpresa") = Me.GridViewOne.SelectedRow.Cells(4).Text

    '    CKSMFACTURA("codPais") = Me.GridViewOne.SelectedDataKey.Item(0).ToString()
    '    CKSMFACTURA("CodEmpresa") = Me.GridViewOne.SelectedDataKey.Item(1).ToString()
    '    CKSMFACTURA("CodPuesto") = Me.GridViewOne.SelectedDataKey.Item(2).ToString()

    '    'CKSMFACTURA("codPais") = Me.GridViewOne.SelectedDataKey.Item(3).ToString()
    '    'CKSMFACTURA("CodEmpresa") = Me.GridViewOne.SelectedDataKey.Item(2).ToString()
    '    'CKSMFACTURA("CodPuesto") = Me.GridViewOne.SelectedDataKey.Item(3).ToString()

    '    CKSMFACTURA("cod_sesion") = vcod_sesion
    '    CKSMFACTURA("cod_usuario") = vcod_usuario
    '    CKSMFACTURA("Nombre_Usuario") = vNombre_Usuario
    '    CKSMFACTURA("contrasenia") = vcontrasenia

    '    CKSMFACTURA.Expires = Now.AddDays(1)
    '    Response.Cookies.Add(CKSMFACTURA)

    '    Response.Redirect(ResolveClientUrl("../Default.aspx"))

    'End Sub

    Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        SetCookies()

    End Sub

    Protected Sub SetCookies()
        'Asignar en las cookies los datos seleccionados (pais, empresa, puesto).
        Dim cookie As HttpCookie = Request.Cookies.Get("CKSMFACTURA")
        Dim userCookie = cookie("Username")

        cookie("CodigoPais") = ddlPais.SelectedValue()
        cookie("CodigoEmpresa") = ddlEmpresa.SelectedValue()
        cookie("CodigoPuesto") = ddlPuesto.SelectedValue()
        cookie("Pais") = ddlPais.SelectedItem.Text()
        cookie("Empresa") = ddlEmpresa.SelectedItem.Text()
        cookie("Puesto") = ddlPuesto.SelectedItem.Text()
        cookie.Expires = Now.AddDays(1)
        Response.Cookies.Add(cookie)

        'Asignamos las variables de sesion desde los controles en la pantalla PaisEmpresaPuesto.aspx
        Session("cod_pais") = cookie("CodigoPais")
        Session("cod_empresa") = cookie("CodigoEmpresa")
        Session("cod_puesto") = cookie("CodigoPuesto")
        Session("Pais") = cookie("Pais")
        Session("Empresa") = cookie("Empresa")
        Session("Puesto") = cookie("Puesto")

        'Redireccionar al usuario a la pagina principal "Default.aspx"
        'FormsAuthentication.SetAuthCookie(userCookie, False)
        FormsAuthentication.RedirectFromLoginPage(userCookie, False)
    End Sub

    Private Sub btnCerrar_Click(sender As Object, e As EventArgs) Handles btnCerrar.Click

        Dim codigoUser As String = Context.Request.Cookies("CKSMFACTURA")("CodigoUser").ToString()
        Dim codigoSesion As String = Context.Request.Cookies("CKSMFACTURA")("CodigoSesion").ToString()

        CerrarSesion(codigoUser, codigoSesion)
    End Sub

    Private Sub CerrarSesion(codigoUser As String, codigoSesion As String)

        Try

            Using dbCon As New OleDbConnection(_conn.conn)
                dbCon.Open()

                Dim cmd As New OleDbCommand("sp_sys_sesion_activa", dbCon)
                cmd.CommandType = CommandType.StoredProcedure

                cmd.Parameters.AddWithValue("@cod_usuario", codigoUser)
                cmd.Parameters.AddWithValue("@cod_sesion", codigoSesion)
                cmd.Parameters.AddWithValue("@estado", "CERRAR")
                Dim rowsAffected = cmd.ExecuteNonQuery()

                If rowsAffected > 0 Then

                    Dim cookie As HttpCookie = Request.Cookies.Get("CKSMFACTURA")
                    cookie.Expires = Now.AddDays(-1)
                    Request.Cookies.Clear()
                    Session.Abandon()
                    FormsAuthentication.SignOut()
                    FormsAuthentication.RedirectToLoginPage()

                End If

            End Using

        Catch ex As Exception
            Dim msg = "alertify.error('Ha ocurrido un error al cerrar sesión. Si el problema persiste, contacte con el administrador.');"
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "msg", msg, True)
        End Try
    End Sub

    Private Function WatchSession() As Boolean

        Using dbCon As New OleDbConnection(_conn.conn)

            dbCon.Open()

            Dim cmd As New OleDbCommand("sp_sys_sesion_activa", dbCon)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@cod_usuario", Context.Request.Cookies("CKSMFACTURA")("CodigoUser"))
            cmd.Parameters.AddWithValue("@cod_sesion", Context.Request.Cookies("CKSMFACTURA")("CodigoSesion"))
            cmd.Parameters.AddWithValue("@estado", "CONSULTAR")

            Dim reader As OleDbDataReader = cmd.ExecuteReader()

            If reader.HasRows Then
                Return True
            End If

            Return False

        End Using

    End Function

    Private Sub CloseSessionSuccessAlert()
        Dim msg = "alertify.success('Sesión cerrada correctamente.');"
        ScriptManager.RegisterStartupScript(Me, Page.GetType, "msg", msg, True)
    End Sub

End Class