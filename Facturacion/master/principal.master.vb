Imports System.Data
Imports System.Data.OleDb
Imports System.Net
Imports FACTURACION_CLASS

Partial Class Mater_principal
    Inherits MasterPage

    Dim _conn As New seguridad
    Dim _database As New database
    Dim AlertifyClass As New AlertifyClass

    Private WithEvents DropdownsClass As New DropdownsClass()

    Public CompanyName As String = "Facturación Local - Industrial Comercial San Martín"
    Public MyUserName As String

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Dim pageName As String = GetPageName()

        If Request.Cookies("Username") IsNot Nothing Then
            MyUserName = Server.HtmlEncode(Request.Cookies("Username").Value)
        End If

        'Pagina maestra se carga por primera vez.
        If Not Page.IsPostBack Then

            If Not Page.User.Identity.IsAuthenticated Then
                FormsAuthentication.RedirectToLoginPage() 'Si no esta autenticado, redirecciona al login.aspx
            End If

            'Si el usuario accede a una página desde "Default.aspx" se valida si el usuario tiene permiso para acceder a la página
            'de la variable pageName.
            If Not String.IsNullOrWhiteSpace(pageName) AndAlso pageName <> "Default.aspx" Then
                If HasPermission(pageName) = True Then

                    If Request.Cookies("CodigoPais") Is Nothing Or Request.Cookies("CodigoEmpresa") Is Nothing Or Request.Cookies("CodigoPuesto") Is Nothing Then
                        Response.Redirect(ResolveClientUrl("~/Default.aspx"))


                        AlertifyClass.AlertifyAlertMessage(HttpContext.Current.CurrentHandler, "Debe seleccionar un País, Empresa y Puesto.")

                    End If

                ElseIf HasPermission(pageName) = False Then

                    Response.Redirect(ResolveClientUrl("~/Default.aspx"))
                    AlertifyClass.AlertifyAlertMessage(Me.Page, "No tiene permiso para acceder a esta página.")
                    'Agarrar el return url y poner el script dentro de la alerta.

                End If
            End If

            'Cargar menu lateral, estilos del gridview y el ddlPais.
            SidebarEncabezados()
            GridViewStyles()
            LoadDdlPais()

            'En el primer page load, verificar si el usuario trae un pais por defecto seleccionado.
            If Request.Cookies("CodigoPais") IsNot Nothing Then

                'El pais seleccionado por defecto del ddlPais es el que trae el usuario en la session.
                ddlPais.SelectedValue = Request.Cookies("CodigoPais").Value
                ddlPais_SelectedIndexChanged(sender, EventArgs.Empty)

            End If

            If Request.Cookies("CodigoEmpresa") IsNot Nothing Then
                ddlEmpresa.SelectedValue = Request.Cookies("CodigoEmpresa").Value
                ddlEmpresa_SelectedIndexChanged(sender, EventArgs.Empty)
            End If

            If Request.Cookies("CodigoPuesto") IsNot Nothing Then
                ddlPuesto.SelectedValue = Request.Cookies("CodigoPuesto").Value
                ddlPuesto_SelectedIndexChanged(sender, EventArgs.Empty)
            End If

        End If
    End Sub

    Protected Sub ddlPais_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlPais.SelectedIndexChanged
        Try
            'Inhabilitar los ddlEmpresa y ddlPuesto
            ddlEmpresa.Enabled = False
            ddlEmpresa.Items.Clear()
            ddlEmpresa.Items.Insert(0, New ListItem("Seleccione Empresa", 0))

            If Request.Cookies("CodigoEmpresa") IsNot Nothing Then
                ClearCookie("CodigoEmpresa")
            End If

            ddlPuesto.Enabled = False
            ddlPuesto.Items.Clear()
            ddlPuesto.Items.Insert(0, New ListItem("Seleccione Puesto", 0))

            If Request.Cookies("CodigoPuesto") IsNot Nothing Then
                ClearCookie("CodigoPuesto")
            End If

            ''El nuevo valor seleccionado de ddlPais.
            Dim CodigoPais As Integer = Integer.Parse(ddlPais.SelectedItem.Value)

            If CodigoPais > 0 Then

                'Comprobar que la cookie existe y crear una nueva cookie con el valor seleccionado de ddlPais.
                Dim cookie = Request.Cookies("CodigoPais")

                If cookie Is Nothing Then
                    cookie = New HttpCookie("CodigoPais") With {
                    .Value = CodigoPais.ToString()
                    }
                Else
                    cookie.Value = CodigoPais.ToString()
                End If

                cookie.Expires = Now.AddDays(1)
                Response.Cookies.Add(cookie)

                AlertifyClass.AlertifySuccessMessage(Me.Page, "Pais Seleccionado Correctamente.")
                LoadDdlEmpresa()
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub ddlEmpresa_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlEmpresa.SelectedIndexChanged
        Try
            ddlPuesto.Enabled = False
            ddlPuesto.Items.Clear()
            ddlPuesto.Items.Insert(0, New ListItem("Seleccione Puesto", 0))

            If Request.Cookies("CodigoPuesto") IsNot Nothing Then
                ClearCookie("CodigoPuesto")
            End If

            Dim CodigoEmpresa As Integer = Integer.Parse(ddlEmpresa.SelectedItem.Value)

            If CodigoEmpresa > 0 Then

                Dim cookie = Request.Cookies("CodigoEmpresa")

                If cookie Is Nothing Then

                    cookie = New HttpCookie("CodigoEmpresa") With {
                        .Value = CodigoEmpresa.ToString()
                    }
                Else
                    cookie.Value = CodigoEmpresa.ToString()
                End If

                cookie.Expires = Now.AddDays(1)
                Response.Cookies.Add(cookie)

                AlertifyClass.AlertifySuccessMessage(Me.Page, "Empresa Seleccionada Correctamente.")

                'Load ddlPuesto
                LoadDdlPuesto()

            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub ddlPuesto_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlPuesto.SelectedIndexChanged
        Try
            Dim CodigoPuesto As Integer = Integer.Parse(ddlPuesto.SelectedItem.Value)

            If CodigoPuesto > 0 Then

                Dim cookie = Request.Cookies("CodigoPuesto")

                If cookie Is Nothing Then

                    cookie = New HttpCookie("CodigoPuesto") With {
                    .Value = CodigoPuesto.ToString()
                    }

                Else
                    cookie.Value = CodigoPuesto.ToString()
                End If

                cookie.Expires = Now.AddDays(1)
                Response.Cookies.Add(cookie)

                AlertifyClass.AlertifySuccessMessage(Me.Page, "Puesto Seleccionado Correctamente.")
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub LoadDdlPais()
        Dim sql = $"SELECT * FROM GetPaisesAccesoUsuario({Request.Cookies("CodigoUser").Value})"

        DropdownsClass.BindDropDownList(ddlPais, sql, "CodigoPais", "Descripcion", "Seleccione País")
    End Sub

    Protected Sub LoadDdlEmpresa()

        Dim sql = $"SELECT * FROM GetEmpresasAccesoUsuario({Request.Cookies("CodigoUser").Value}, {Request.Cookies("CodigoPais").Value})"

        DropdownsClass.BindDropDownList(ddlEmpresa, sql, "CodigoEmpresa", "Descripcion", "Seleccione Empresa")
        ddlEmpresa.Enabled = True
    End Sub

    Private Sub LoadDdlPuesto()

        Dim sql = $"SELECT * FROM GetPuestosAccesoUsuario({Request.Cookies("CodigoUser").Value}, {Request.Cookies("CodigoEmpresa").Value})"

        DropdownsClass.BindDropDownList(ddlPuesto, sql, "CodigoPuesto", "Descripcion", "Seleccione Puesto")
        ddlPuesto.Enabled = True
    End Sub

    Protected Sub ClearCookie(cookieName As String)

        Dim cookie As New HttpCookie(cookieName) With {
            .Expires = DateTime.Now.AddDays(-1) ' Set expiration date in the past
            }
        Response.Cookies.Add(cookie)
    End Sub




    ‘'' <summary> ‘’’ Verifica los permisos del usuario para entrar a la pagina actual.
    ‘’’ El sistema crea opciones en el menu para cada usuario.
    ''' Sin embargo, si el usuario escribe manualmente la dirección URL este entrara a la pagina.
    ‘’’ Este procedimiento verifica si el usuario está en una pagina a la que tiene permiso en el menú.
    ''' Si no tiene permiso, lo redirecciona a la ‘’’ pagina del perfil. ‘’’ </summary>
    ''' <remarks></remarks>
    Private Function HasPermission(pageName As String) As Boolean
        Try
            If Session("CodigoUser") IsNot Nothing Then

                pageName = pageName.Split("?"c)(0)

                Dim sql = $"EXEC sp_val_PermisoUsuario
                        @CodigoRol = {Session("CodigoRol")},
                        @pageName = '{pageName}'"

                Return _database.GetBoleano(sql, "tienePermiso")
            End If
        Catch ex As Exception
            Response.Write("Ocurrio un error al intentar validar los permisos del usuario en la pagina maestra. " & ex.Message)
        End Try
        Return False
    End Function

    ''' <summary>
    ''' Obtiene el nombre de la pagina web actual, junto con sus variables y codigos en la URL
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetPageName() As String
        Try
            Dim arrPath As String() = HttpContext.Current.Request.RawUrl.Split("/")
            Return arrPath(arrPath.GetUpperBound(0))

        Catch ex As Exception
            Response.Write("Ocurrio un error al obtener el nombre de la página." & ex.Message)
        End Try

        Return String.Empty
    End Function
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
                    html.AppendLine("<a class=""btn btn-outline-dark dropdown-toggle text-white"" href=""#"" role=""button"" data-bs-toggle=""dropdown"" aria-expanded=""false"">" & etiquetaPadre & "</a>")
                    html.AppendLine("<ul class=""dropdown-menu bg-white shadow animate slideIn"">")

                    ' Append the menu items for this header
                    html.AppendLine(GetMenuDetalle(codigoMenuPadre, ds))

                    ' Close the dropdown menu
                    html.AppendLine("</ul>")
                    html.AppendLine("</li>")

                End If

            Next i

            ltMenu.Text = html.ToString()
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

            gridView.UseAccessibleHeader = True
            gridView.HeaderRow.TableSection = TableRowSection.TableHeader
        End If
    End Sub
#End Region

#Region "SIDEBAR"
    Protected Sub SidebarEncabezados()
        Try
            Dim sql = $"EXEC sp_crear_menu_web @Tipo = PADRE, @cod_padre = NULL"

            Dim dtParent As DataTable = _database.GetDataTable(sql)

            Dim html As New StringBuilder()

            For i As Integer = 0 To dtParent.Rows.Count - 1

                Dim cod_padre = dtParent.Rows(i).Item("cod_menu").ToString()
                Dim dtChild As DataTable = _database.GetDataTable($"EXEC sp_menu_permisos_rol @CodigoRol = {Request.Cookies("CodigoRol").Value.ToString()}, @cod_menu = {cod_padre}, @Tipo = HIJOS")

                'Verificar si el encabezado contiene detalles (elementos del menu)
                'Si NO contiene detalles, entonces no hay necesidad de mostrarlo en el menu.
                If dtChild.Rows.Count > 0 Then

                    html.AppendLine("<li class=""nav-item"">")
                    html.AppendLine("<a class=""nav-link link-effect align-middle px-0 text-white"" href=""collapse"" type=""button"" data-bs-toggle=""collapse"" data-bs-target=""#collapse" & i & """ aria-controls=""collapse" & i & """>")
                    html.AppendLine($"<i class=""{dtParent.Rows(i).Item("icono").ToString().Trim()}""></i>")
                    html.AppendLine(dtParent.Rows(i).Item("etiqueta").ToString.Trim())
                    html.AppendLine("</a>")

                    html.AppendLine("<ul id=""collapse" & i & """ class=""collapse nav flex-column bg-white rounded-1 ms-1"" data-bs-parent=""#menu"" >")
                    html.AppendLine("<li class=""w-100 "">") ''nav item
                    html.AppendLine(SidebarDetalles(dtChild, cod_padre))
                    html.AppendLine("</li>") ''nav item
                    html.AppendLine("</ul>") ''collapse
                    html.AppendLine("</li>") ''nav item
                End If
            Next i

            'Asignar el html del menu al ltSidebar al final del loop.
            ltSidebar.Text = html.ToString()

        Catch ex As Exception
            ltSidebar.Text = "<ul><li>" & ex.Message & "</li></ul>"
        End Try
    End Sub

    Protected Function SidebarDetalles(dtChild As DataTable, cod_padre As String) As String
        Try
            Dim html As New StringBuilder()
            Dim ruta As String
            Dim clase As String
            Dim label As String

            For i As Integer = 0 To dtChild.Rows.Count - 1

                If dtChild.Rows(i).Item("cod_padre").ToString().Trim() = cod_padre Then

                    ruta = dtChild.Rows(i).Item("ruta").ToString().Trim()
                    clase = dtChild.Rows(i).Item("icono").ToString().Trim()
                    label = dtChild.Rows(i).Item("etiqueta").ToString().Trim()

                    html.AppendLine($"<a class=""nav-link a-default text-decoration-none text-black"" href=""{ResolveClientUrl(ruta)}""><i class=""{clase}""></i> {label}</a>")
                End If
            Next i
            Return html.ToString()
        Catch ex As Exception
            Return $"<ul><li>Error al crear el detalle: {ex.Message}</li></ul>"
        End Try
    End Function

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


                Dim cookies As HttpCookieCollection = Request.Cookies
                For Each cookieKey As String In cookies.AllKeys
                    Dim cookie As HttpCookie = cookies(cookieKey)
                    cookie.Expires = DateTime.Now.AddDays(-1)
                    Response.Cookies.Add(cookie)
                Next

                Request.Cookies.Clear()
                Session.Abandon()
                FormsAuthentication.SignOut()
                FormsAuthentication.RedirectToLoginPage()
            End Using

        Catch ex As Exception
            AlertifyClass.AlertifyErrorMessage(Me.Page, "Ha ocurrido un error al cerrar sesión. Si el problema persiste, contacte con el administrador.")
        End Try
    End Sub

#End Region


End Class