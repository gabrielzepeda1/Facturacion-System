Imports System.Data
Imports System.Data.OleDb
Imports System.Diagnostics
Imports FACTURACION_CLASS

Partial Class Utilitarios_PaisEmpresaPuesto
    Inherits Page

    Dim _conn As New seguridad
    Dim _database As New database

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
            If Session("CodigoSesion") Is Nothing Then
                FormsAuthentication.SignOut()
                Response.Redirect(ResolveClientUrl("~/Login.aspx"))


            Else
                MyUserName = Session("Username")
                Dim codigoRol As Integer = Convert.ToInt32(Session("CodigoRol"))

                Select Case codigoRol

                    Case 1 'Super Administrador
                    Case 2 'Administrador Pais 


                    Case 3 'AdministradorEmpresa 

                    Case 4 'Administrador Puesto

                End Select
            End If
        End If
    End Sub

    Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click

        Dim codigoUser As Integer = Convert.ToInt32(Session("CodigoUser"))
        Dim codigoRol As Integer = Convert.ToInt32(Session("CodigoRol"))

        SetUserVariables(codigoUser, codigoRol)
    End Sub

    Protected Sub SetUserVariables(CodigoUser As Integer, CodigoRol As Integer)

        Select Case CodigoRol
            Case 1 'SuperAdmin
            Case 2 'AdminPais
            Case 3 'AdminEmpresa
            Case 4 'AdminPuesto
                GetSessionPaisEmpresaPuesto()
        End Select

        'Redireccionar al usuario a la pagina principal "Default.aspx"
        'FormsAuthentication.SetAuthCookie(userCookie, False)
        FormsAuthentication.RedirectFromLoginPage(CodigoUser, False)
    End Sub

    Private Sub GetSessionPaisEmpresaPuesto()

        'Estas functiones retornan un array de Pais, Empresa, Puestos a los cuales el usuario tiene acceso y los guarda en sus variables de sesion correspondientes. 
        Session("cod_pais") = GetPaisesUsuario()
        Session("cod_empresa") = GetEmpresasUsuario()
        Session("cod_puesto") = GetPuestosUsuario()

    End Sub

    Private Function GetPaisesUsuario() As List(Of Integer)

        Dim sql = $"SELECT * FROM GetDistinctPaisesUsuario({Session("CodigoUser")})"
        Dim reader = _database.GetDataReader(sql)
        Dim ArrCodigoPais As New List(Of Integer)

        Do While reader.Read()
            ArrCodigoPais.Add(reader("cod_pais"))
        Loop

        Debug.WriteLine(ArrCodigoPais)

        Return ArrCodigoPais

    End Function

    Private Function GetEmpresasUsuario() As List(Of Integer)

        Dim sql = $"SELECT * FROM GetDistinctEmpresasUsuario({Session("CodigoUser")})"
        Dim reader = _database.GetDataReader(sql)
        Dim ArrCodigoEmpresa As New List(Of Integer)

        Do While reader.Read()
            ArrCodigoEmpresa.Add(reader("cod_empresa"))
        Loop

        Debug.WriteLine(ArrCodigoEmpresa)
        Return ArrCodigoEmpresa
    End Function

    Private Function GetPuestosUsuario() As List(Of Integer)

        Dim sql = $"SELECT * FROM GetDistinctPuestosUsuario({Session("CodigoUser")})"
        Dim reader = _database.GetDataReader(sql)
        Dim ArrCodigoPuesto As New List(Of Integer)

        Do While reader.Read()
            ArrCodigoPuesto.Add(reader("cod_empresa"))
        Loop

        Debug.WriteLine(ArrCodigoPuesto)
        Return ArrCodigoPuesto
    End Function

    Private Sub btnCerrar_Click(sender As Object, e As EventArgs) Handles btnCerrar.Click

        Dim codigoUser As Integer = Convert.ToInt32(Session("CodigoUser"))
        Dim codigoSesion As Integer = Convert.ToInt32(Session("CodigoSesion"))

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
End Class