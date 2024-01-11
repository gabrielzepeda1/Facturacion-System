Imports System.Data
Imports System.Data.OleDb
Imports System.Web.Configuration
Imports AlertifyClass

Partial Class Mater_principal

    Inherits MasterPage

    Dim _conn As New FACTURACION_CLASS.seguridad
    Dim _database As New FACTURACION_CLASS.database

    Public CompanyName As String = "Facturación Local - Industrial Comercial San Martín"
    Public MyUserName As String = String.Empty
    Public UserPais As String = String.Empty
    Public UserEmpresa As String = String.Empty
    Public UserPuesto As String = String.Empty

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        'If Not Me.IsPostBack Then
        '    Session("Reset") = True
        '    Dim config As Configuration = WebConfigurationManager.OpenWebConfiguration("~/web.Config")
        '    Dim section As SessionStateSection = DirectCast(config.GetSection("system.web/sessionState"), SessionStateSection)
        '    Dim timeout As Integer = CInt(section.Timeout.TotalMinutes) * 1000 * 60
        '    ScriptManager.RegisterStartupScript(Me, [GetType](), "SessionAlert", "SessionExpireAlert(" & timeout & ");", True)
        'End If


        'Pagina se carga por primera vez
        If Not Page.IsPostBack Then
            If Not Page.User.Identity.IsAuthenticated Then
                FormsAuthentication.RedirectToLoginPage() 'Si no esta autenticado, redirecciona al login.aspx
            End If

            Dim pagina As String = GetPageName()

            If pagina <> String.Empty And pagina <> "Default.aspx" Then
                GetPermisos()
            End If

            MyUserName = Session("Username")
            UserPais = String.Empty
            UserEmpresa = String.Empty
            UserPuesto = String.Empty

            GetMenuEncabezado()
            GridViewStyles()
            'Refresh_Session_Time()


        End If

    End Sub

    Private Sub Refresh_Session_Time()
        Try
            Using dbCon As New OleDbConnection(_conn.conn)
                dbCon.Open()

                Dim cmd As New OleDbCommand("sp_sys_sesion_activa", dbCon)
                cmd.Parameters.AddWithValue("@cod_usuario", Session("CodigoUser"))
                cmd.Parameters.AddWithValue("@cod_sesion", Session("CodigoSesion"))
                cmd.Parameters.AddWithValue("@estado", "REFRESH")
                cmd.CommandType = CommandType.StoredProcedure
                cmd.ExecuteNonQuery()

            End Using
        Catch ex As Exception
            Response.Write("Ocurrio un error al intentar actualizar la sesión de Usuario: " & ex.Message)
        End Try
    End Sub

    ‘'' <summary> ‘’’ Verifica los permisos del usuario para entrar a la pagina actual.
    ‘’’ El sistema crea opciones en el menu para cada usuario.
    ''' Sin embargo, si el usuario escribe manualmente la dirección URL este entrara a la pagina.
    ‘’’ Este procedimiento verifica si el usuario está en una pagina a la que tiene permiso en el menú.
    ''' Si no tiene permiso, lo redirecciona a la ‘’’ pagina del perfil. ‘’’ </summary>
    ''' <remarks></remarks>
    Private Sub GetPermisos()
        Try
            Dim pagina As String
            Dim posicion As Integer = InStr(GetPageName(), "?")

            If posicion > 0 Then
                pagina = GetPageName().Remove(posicion - 1)
            Else
                pagina = GetPageName()
            End If

            Dim sql As String
            Dim tienePermiso As Boolean

            If Session("CodigoUser") IsNot Nothing Then

                sql = "EXEC sp_val_PermisoUsuario " &
                          "@cod_usuario = " & Session("CodigoUser") & " ," &
                          "@Pagina = '" & pagina & "' "

                tienePermiso = _database.GetBoleano(sql, "TRUE")

                If tienePermiso = False Then
                    Response.Redirect(ResolveClientUrl("~/Default.aspx"))
                End If

            End If
        Catch ex As Exception
            Response.Write("Ocurrio un error al intentar validar los permisos del usuario en la pagina maestra. " & ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Obtiene el nombre de la pagina web actual, junto con sus variables y codigos en la URL
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetPageName() As String

        Dim arrPath() As String = HttpContext.Current.Request.RawUrl.Split("/")

        Return arrPath(arrPath.GetUpperBound(0))
    End Function

#Region "CREAR EL MENU DE FORMA DINAMICA"

    Private Sub GetMenuEncabezado()
        Try
            'Este procedimiento se utiliza para obtener los encabezados del menu (Catalogos, Movimientos, Utilitarios), por esta razon el @cod_padre es NULL.
            Dim sql = "EXEC sp_menu_accesos " &
                      "@cod_usuario = " & Session("CodigoUser") & " ," &
                      "@cod_padre = NULL"

            Dim ds As DataSet = _database.GetDataSet(sql)
            Dim html As New StringBuilder

            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                Dim codigoPadre = ds.Tables(0).Rows(i).Item("cod_padre").ToString.Trim()
                Dim etiquetaPadre = ds.Tables(0).Rows(i).Item("etiqueta").ToString.Trim()
                Dim codigoMenuPadre = ds.Tables(0).Rows(i).Item("cod_menu").ToString.Trim()

                'Si el codigoPadre es vacio, significa que el elemento es un ENCABEZADO
                'Si el codigoPadre tiene un valor, significa que el elemento es un DETALLE
                'Por lo que, su valor nos indica a que ENCABEZADO pertenece.

                If codigoPadre = String.Empty Then

                    ' Start a dropdown item
                    html.AppendLine("<li class=""nav-item dropdown"">")
                    html.AppendLine("<a class=""btn btn-outline-dark dropdown-toggle"" href=""#"" role=""button"" data-bs-toggle=""dropdown"" aria-expanded=""false"">" & etiquetaPadre & "</a>")
                    html.AppendLine("<ul class=""dropdown-menu animate slideIn"">")

                    ' Append the menu items for this header
                    html.AppendLine(GetMenuDetalle(codigoMenuPadre, ds))

                    ' Close the dropdown menu
                    html.AppendLine("</ul>")
                    html.AppendLine("</li>")

                End If

            Next i

            Me.ltMenu.Text = html.ToString()
            ds.Dispose()
        Catch ex As Exception
            ltMenu.Text = "<ul><li>Error al crear el menú: " & ex.Message & "</li></ul>"
        End Try
    End Sub

    Function GetMenuDetalle(codigoMenuPadre As String, ds As DataSet) As String
        Dim html As New StringBuilder()

        Try
            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1

                Dim codigoPadre = ds.Tables(0).Rows(i).Item("cod_padre").ToString.Trim()

                'Comparamos si el codigoPadre de el DETALLE es igual al codigoMenu del ENCABEZADO (codigoMenuPadre) que se trae desde el procedimiento anterior).

                If codigoPadre = codigoMenuPadre Then
                    'Get menu items
                    Dim ruta = ds.Tables(0).Rows(i).Item("ruta").ToString.Trim()
                    Dim icono = ds.Tables(0).Rows(i).Item("Icono").ToString.Trim()
                    Dim label = ds.Tables(0).Rows(i).Item("etiqueta").ToString.Trim()

                    'Append
                    html.AppendLine("<li class=""mx-2 my-1 ""><a class=""dropdown-item rounded text-decoration-none a-default fs-6"" href=""" & ResolveClientUrl(ruta) & """><i class=""" & icono & """></i> " & label & "</a></li>")

                End If
            Next i
        Catch ex As Exception
            html.AppendLine("<li>Error al crear el menú detalle: " & ex.Message & "</li>")
        End Try

        Return html.ToString()
    End Function

    Private Sub GridViewStyles()
        Dim gridView As GridView = DirectCast(FindControl("GridViewOne"), GridView)

        If gridView IsNot Nothing Then
            gridView.CssClass = "table table-light table-sm table-striped table-hover table-bordered"
        End If
    End Sub

#End Region

#Region "CERRAR SESIÓN"
    Protected Sub btnCerrar_Click(sender As Object, e As EventArgs) Handles btnCerrar.Click
        CerrarSesion(Session("CodigoUser"), Session("CodigoSesion"))
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
                cmd.ExecuteNonQuery()

                'Dim cookie As HttpCookie = Request.Cookies.Get("CKSMFACTURA")
                'cookie.Expires = Now.AddDays(-1)
                'Request.Cookies.Clear()
                Session.Abandon()
                FormsAuthentication.SignOut()
                FormsAuthentication.RedirectToLoginPage()
            End Using

        Catch ex As Exception
            Dim msg = "alertify.error('Ha ocurrido un error al cerrar sesión. Si el problema persiste, contacte con el administrador.');"
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "msg", msg, True)
            AlertifyErrorMessage(Me.Page, "Ha ocurrido un error al cerrar sesión. Si el problema persiste, contacte con el administrador.")
        End Try
    End Sub

#End Region

End Class