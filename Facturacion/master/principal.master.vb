Imports System.Data
Imports System.Data.OleDb

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

        'Pagina se carga por primera vez
        If Not Page.IsPostBack Then

            'Validar que las variables de sesión contienen un valor correcto
            If Session("Pais") = String.Empty OrElse Session("Empresa") = String.Empty OrElse Session("Puesto") = String.Empty OrElse
                   Session("cod_pais") = String.Empty OrElse Session("cod_empresa") = String.Empty OrElse Session("cod_puesto") = String.Empty Then

                Response.Redirect(ResolveClientUrl("~/Utilitarios/PaisEmpresaPuesto.aspx"))

            End If

            Dim pagina As String = GetPageName()

            If Not pagina = String.Empty Then
                If Not pagina = "Default.aspx" Then
                    GetPermisos()
                End If
            End If

            MyUserName = Request.Cookies("CKSMFACTURA")("Username")
            UserPais = Request.Cookies("CKSMFACTURA")("Pais")
            UserEmpresa = Request.Cookies("CKSMFACTURA")("Empresa")
            UserPuesto = Request.Cookies("CKSMFACTURA")("Puesto")

            GetMenuEncabezado()
            GridViewStyles()
            'Refresh_Session_Time()
        End If

    End Sub

    Private Sub Refresh_Session_Time()

        Dim codigoUser = Convert.ToInt32(Request.Cookies("CKSMFACTURA")("CodigoUser"))
        Dim codigoSesion = Convert.ToInt32(Request.Cookies("CKSMFACTURA")("CodigoSesion"))

        Try
            Using dbCon As New OleDbConnection(_conn.conn)
                dbCon.Open()

                Dim cmd As New OleDbCommand("sp_sys_sesion_activa", dbCon)
                cmd.Parameters.AddWithValue("@cod_usuario", codigoUser)
                cmd.Parameters.AddWithValue("@cod_sesion", codigoSesion)
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

            If Request.Cookies("CKSMFACTURA") IsNot Nothing Then

                sql = "EXEC sp_val_PermisoUsuario " &
                          "@cod_usuario = " & Request.Cookies("CKSMFACTURA")("CodigoUser") & " ," &
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
                      "@cod_usuario = " & Request.Cookies("CKSMFACTURA")("CodigoUser") & " ," &
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

    Protected Sub btnCerrar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCerrar.Click

        Dim codigoUser As String = Context.Request.Cookies("CKSMFACTURA")("CodigoUser").ToString()
        Dim codigoSesion As String = Context.Request.Cookies("CKSMFACTURA")("CodigoSesion").ToString()

        Try
            If _conn.CerrarSesion(codigoUser, codigoSesion) Then

                Dim cookie As HttpCookie = Request.Cookies.Get("CKSMFACTURA")
                cookie.Expires = Now.AddDays(-1)
                Request.Cookies.Clear()
                Session.Abandon()
                FormsAuthentication.SignOut()
                FormsAuthentication.RedirectToLoginPage()
            Else

                Request.Cookies.Clear()
                Session.Abandon()
                FormsAuthentication.SignOut()
                FormsAuthentication.RedirectToLoginPage()

                Dim msg = "alertify.error('Ha ocurrido un error al cerrar sesión. Si el problema persiste, contacte con el administrador.');"
                ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "msg", msg, True)
                Exit Sub
            End If
        Catch ex As Exception
            Dim msg = "alertify.error('Ha ocurrido un error al cerrar sesión. Si el problema persiste, contacte con el administrador.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "msg", msg, True)
        End Try

    End Sub

    'Private Sub OnCloseSession()
    '    Dim dbCon As New System.Data.OleDb.OleDbConnection(_conn.conn)

    '    Try

    '        If dbCon.State = ConnectionState.Closed Then
    '            dbCon.Open()
    '        End If

    '        Dim sql As String = String.Empty
    '        sql = "EXEC sp_sys_sesion_activa " &
    '                  "@cod_usuario = " & Request.Cookies("CKSMFACTURA")("CodigoUser") & "," &
    '                  "@cod_sesion = null," &
    '                  "@estado = 'INACTIVAR'"

    '        Dim cmd As New OleDb.OleDbCommand(sql, dbCon)
    '        cmd.ExecuteNonQuery()

    '        Dim cksmfactura As HttpCookie = New HttpCookie("CKSMFACTURA")
    '        cksmfactura("CodigoSesion") = String.Empty
    '        cksmfactura("CodigoUser") = String.Empty
    '        cksmfactura("Username") = String.Empty
    '        cksmfactura("Password") = String.Empty
    '        cksmfactura.Expires = Now.AddDays(-1)
    '        Response.Cookies.Add(cksmfactura)
    '        Me.Session.Abandon()

    '        FormsAuthentication.SignOut()
    '        FormsAuthentication.RedirectToLoginPage()

    '    Catch ex As Exception
    '        Response.Write("Ocurrio un error al intentar cerrar la sesión de Usuario: " & ex.Message)

    '    Finally
    '        If dbCon.State = ConnectionState.Open Then
    '            dbCon.Close()
    '        End If

    '    End Try
    'End Sub
    'Private Sub CerrarSesion(codigoSesion As String, codigoUser As String)

    '    Try

    '        Using dbCon As New OleDbConnection(_conn.conn)
    '            dbCon.Open()

    '            Dim cmd As New OleDbCommand("sp_sys_sesion_activa", dbCon)
    '            cmd.Parameters.AddWithValue("@cod_usuario", codigoSesion)
    '            cmd.Parameters.AddWithValue("@cod_sesion", codigoUser)
    '            cmd.Parameters.AddWithValue("@estado", "INACTIVAR")
    '            cmd.CommandType = CommandType.StoredProcedure
    '            cmd.ExecuteNonQuery()

    '            Dim cookie As HttpCookie = Request.Cookies.Get("CKSMFACTURA")
    '            cookie.Expires = Now.AddDays(-1)
    '            Request.Cookies.Clear()
    '            Session.Abandon()
    '            FormsAuthentication.SignOut()
    '            FormsAuthentication.RedirectToLoginPage()
    '        End Using

    '    Catch ex As Exception
    '        Response.Write("Ocurrio un error al intentar cerrar la sesión de Usuario: " & ex.Message)

    '    End Try
    'End Sub

#End Region

End Class