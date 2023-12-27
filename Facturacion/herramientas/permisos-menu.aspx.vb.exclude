Imports System.Data
Imports System.Globalization
Imports System.Threading

Partial Class admin_permisos_menu
    Inherits System.Web.UI.Page
    Dim conn As New FACTURACION_CLASS.Seguridad
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
            GetMenu()
        End If
    End Sub

#Region "CARGAR DATOS EXISTENTES DEL MENÚ"
    Private Sub GetMenu()
        Try
            trvMenu.Nodes.Clear()

            Dim sql = "EXEC sp_Menu_web_admin " &
                  "@cod_menu = NULL," &
                  "@cod_Padre = NULL," &
                  "@Posicion = NULL," &
                  "@Etiqueta = NULL," &
                  "@Ruta = NULL," &
                  "@Pagina = NULL," &
                  "@icono = NULL," &
                  "@Activo = NULL," &
                  "@Ocultar_Menu = NULL," &
                  "@Tipo = 'FULL'"

            Dim ds As DataSet = DataBase.GetDataSet(sql)
            Dim tnode As TreeNode

            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                'Validar si el usuario tiene asignado el menu
                If ds.Tables(0).Rows(i).Item("cod_Padre").ToString = String.Empty Then
                    tnode = New TreeNode
                    tnode.Text = ds.Tables(0).Rows(i).Item("Etiqueta").ToString()
                    tnode.Value = ds.Tables(0).Rows(i).Item("cod_menu").ToString()
                    trvMenu.Nodes.Add(tnode)

                    AddMenuItem(tnode, ds)
                End If
            Next i

            ds.Dispose()
            trvMenu.ExpandAll()
        Catch ex As Exception
            Me.ltMensaje.Text &= conn.PmsgBox("Ocurrio un error al intentar crear el arbol de nodos. " & ex.Message, "error")
        End Try
    End Sub
#End Region

#Region "CARGAR MENU DEL USUARIO SELECCIONADO"
    Protected Sub ddlUsuario_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlUsuario.SelectedIndexChanged
        ltMensaje.Text = String.Empty

        If ddlUsuario.Text = String.Empty Then
            ltMensaje.Text = conn.PmsgBox("El proceso no puede continuar. Debe seleccionar un usuario de la lista desplegable.", "info")
            trvPermisos.Nodes.Clear()
            Exit Sub
        End If

        GetMenuPermisos()
    End Sub

    ''' <summary>
    ''' EL OPERADOR DEBE SELECCIONAR EL NOMBRE DE USUARIO AL CUAL CARGAR LOS PERMISOS DEL MENÚ. UNA VEZ HECHO ESTO SE PROCEDE A DAR CLIC EN EL BOTON ACTUALIZAR
    ''' SE CONSULTA A LA TABLA sys_Menu_Permisos EN LA BASE DE DATOS Y OPTIENE AQUELLOS NODOS ACTIVOS PARA EL USUARIO SELECCIONADO
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetMenuPermisos()
        Try
            trvPermisos.Nodes.Clear()

            Dim sql = "EXEC sp_Menu_Permisos " &
                  "@cod_menu = NULL," &
                  "@cod_usuario = " & ddlUsuario.SelectedValue & "," &
                  "@Tipo = 'FULL'"

            Dim ds As DataSet = DataBase.GetDataSet(sql)
            Dim tnode As TreeNode

            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                'Verificar si el usuario tiene asignado el menu
                If ds.Tables(0).Rows(i).Item("cod_Padre").ToString = String.Empty Then
                    tnode = New TreeNode
                    tnode.Text = ds.Tables(0).Rows(i).Item("Etiqueta").ToString()
                    tnode.Value = ds.Tables(0).Rows(i).Item("cod_menu").ToString()
                    trvPermisos.Nodes.Add(tnode)

                    AddMenuItem(tnode, ds)
                End If
            Next i

            ds.Dispose()
            trvPermisos.ExpandAll()

        Catch ex As Exception
            ltMensaje.Text = conn.PmsgBox("Ocurrio un error al intentar crear el arbol de nodos. " & ex.Message, "error")
        End Try
    End Sub
#End Region
    ''' <summary>
    ''' UTILIZA EL DATASET CARGADO EN EL PROCESO GetMenu PARA CREAR LOS NODOS HIJOS
    ''' </summary>
    ''' <param name="tnode"></param>
    ''' <param name="ds"></param>
    ''' <remarks></remarks>
    Private Sub AddMenuItem(ByRef tnode As TreeNode, ByVal ds As DataSet)
        Try
            'Recorremos cada elemento del datatable para poder determinar cuales son elementos hijos
            'del menu item dado pasado como parametro ByRef.

            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                If ds.Tables(0).Rows(i).Item("cod_Padre").ToString = tnode.Value AndAlso Not ds.Tables(0).Rows(i).Item("cod_menu").ToString = ds.Tables(0).Rows(i).Item("cod_Padre").ToString Then
                    Dim newnode As New TreeNode
                    newnode.Value = ds.Tables(0).Rows(i).Item("cod_menu").ToString()
                    newnode.Text = ds.Tables(0).Rows(i).Item("Etiqueta").ToString()

                    tnode.ChildNodes.Add(newnode)

                    'Llamada recursiva para ver si el nuevo menú ítem aun tiene elementos hijos.
                    AddMenuItem(newnode, ds)
                End If
            Next i
        Catch ex As Exception
            ltMensaje.Text &= conn.PmsgBox("Ocurrio un error al intentar crear los hijos del arbol de nodos. " & ex.Message, "error")
        End Try
    End Sub

#Region "PROCEDIMIENTOS DE LA BASE DE DATOS"
    Protected Sub trvMenu_SelectedNodeChanged(sender As Object, e As System.EventArgs) Handles trvMenu.SelectedNodeChanged
        If ddlUsuario.Text = String.Empty Then
            ltMensaje.Text = conn.PmsgBox("El proceso no puede continuar. Debe seleccionar un usuario de la lista desplegable.", "info")
            trvPermisos.Nodes.Clear()
            Exit Sub
        End If

        ltMensaje.Text = String.Empty

        Dim sql As String = "SET DATEFORMAT DMY " & vbCrLf
        sql = "EXEC sp_Menu_Permisos " & _
              "@cod_menu = " & Me.trvMenu.SelectedValue & "," & _
              "@cod_usuario = " & Me.ddlUsuario.SelectedValue & "," & _
              "@Tipo = 'INSERTAR'"

        Guardar(sql, "sp_Menu_Permisos", String.Empty, "El proceso de guardado no se concreto. Intentelo de nuevo. Si el error continua contacte con el administrador.")
    End Sub

    Protected Sub trvPermisos_SelectedNodeChanged(sender As Object, e As System.EventArgs) Handles trvPermisos.SelectedNodeChanged
        If Me.ddlUsuario.Text = String.Empty Then
            Me.ltMensaje.Text = conn.PmsgBox("El proceso no puede continuar. Debe seleccionar un usuario de la lista desplegable.", "info")
            Me.trvPermisos.Nodes.Clear()
            Exit Sub
        End If

        Me.ltMensaje.Text = String.Empty

        Dim sql As String = "SET DATEFORMAT DMY " & vbCrLf
        sql = "EXEC sp_Menu_Permisos " & _
              "@cod_menu = " & Me.trvPermisos.SelectedValue & "," & _
              "@cod_usuario = " & Me.ddlUsuario.SelectedValue & "," & _
              "@Tipo = 'ELIMINAR'"

        Guardar(sql, "sp_Menu_Permisos", String.Empty, "El proceso de eliminación dio un error. Intentelo de nuevo. Si el error continua contacte con el administrador.")

    End Sub

    Protected Sub ddlRol_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlRol.SelectedIndexChanged
        If Me.ddlUsuario.Text = String.Empty Then
            Me.ltMensaje.Text = conn.PmsgBox("El proceso no puede continuar. Debe seleccionar un usuario de la lista desplegable.", "info")
            Me.trvPermisos.Nodes.Clear()
            Exit Sub
        End If

        If Me.ddlRol.Text = String.Empty Then
            Me.ltMensaje.Text = conn.PmsgBox("El proceso no puede continuar. Debe seleccionar un rol en la lista desplegable.", "info")
            Me.trvPermisos.Nodes.Clear()
            Exit Sub
        End If

        Me.ltMensaje.Text = String.Empty

        Dim sql As String = "SET DATEFORMAT DMY " & vbCrLf
        sql = "EXEC sp_sys_permisos_segun_rol " & _
              "@cod_rol = " & Me.ddlRol.SelectedValue & "," & _
              "@cod_usuario = " & Me.ddlUsuario.SelectedValue & " "

        Guardar(sql, "sp_sys_permisos_segun_rol", String.Empty, "La asignación de permisos mediante roles ha generado un error inesperado.")

    End Sub

    ''' <summary>
    ''' ENVIA UN SCRIPT A SQL SERVER PARA SU EJECUÓN. NO RETORNA NINGUN VALOR
    ''' </summary>
    ''' <param name="sql">SCRIPT SQL QUE SERA ENVIADO AL SERVIDOR</param>
    ''' <param name="Query">NOMBRE DEL PROCEDIMIENTO QUE SERA UTILIZADO</param>
    ''' <param name="ExitoMsg">MENSAJE DE EXITO QUE SERA VISUALIZADO POR EL VISITANTE.</param>
    ''' <param name="ErrorMsg">MENSAJE DE ERROR QUE SERA VISUALIZADO POR EL VISITANTE.</param>
    ''' <remarks></remarks>
    Private Sub Guardar(sql As String, Query As String, ExitoMsg As String, ErrorMsg As String)
        Dim dbCon As New System.Data.OleDb.OleDbConnection(conn.Conn)
        Try
            If dbCon.State = ConnectionState.Closed Then
                dbCon.Open()
            End If

            Dim cmd As New OleDb.OleDbCommand(sql, dbCon)
            cmd.ExecuteNonQuery()

            If Not ExitoMsg = String.Empty Then
                Me.ltMensaje.Text = conn.PmsgBox(ExitoMsg, "exito")
            End If

            GetMenuPermisos()

        Catch ex As Exception
            Me.ltMensaje.Text = conn.PmsgBox(ErrorMsg & " " & ex.Message, "error")

        Finally
            If dbCon.State = ConnectionState.Open Then
                dbCon.Close()
            End If

        End Try
    End Sub
#End Region
End Class
