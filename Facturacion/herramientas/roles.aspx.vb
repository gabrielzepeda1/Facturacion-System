Imports System.Data
Imports System.Data.OleDb
Imports AlertifyClass
Imports Microsoft.Ajax.Utilities 'Importar la clase AlertifyClass para poder utilizar los metodos de la clase.

Partial Class herramientas_roles
    Inherits Page
    Dim _conn As New FACTURACION_CLASS.seguridad
    Dim _dataBase As New FACTURACION_CLASS.database
    Public NombreRol As String = String.Empty

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

        'Agregamos el evento onclick al control trvMenu para que se ejecute el metodo checkBoxPostBack() cuando se haga click en el control.
        trvMenu.Attributes.Add("onclick", "checkBoxPostBack()")

        If Not Page.IsPostBack Then
            ddlRol.Focus()
        End If

    End Sub

    Protected Sub ddlRol_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlRol.SelectedIndexChanged

        NombreRol = ddlRol.SelectedItem.Text.ToString()
        hdfCodigo.Value = ddlRol.SelectedValue.ToString()

        trvMenu.Nodes.Clear()

        Dim dt As DataTable = Me.GetData("SELECT cod_menu, etiqueta FROM sys_menu_web_parent")
        LoadTreeView(dt, 0, Nothing)

        trvMenu.CollapseAll()

    End Sub
    Private Sub trvMenu_TreeNodeCheckChanged(sender As Object, e As TreeNodeEventArgs) Handles trvMenu.TreeNodeCheckChanged

        Dim isNodeChecked = trvMenu.CheckedNodes.Contains(e.Node)

        If hdfCodigo.Value.Trim() = String.Empty Then
            AlertifyErrorMessage(Page, "Debe seleccionar un rol.")
            Exit Sub
        End If

        If hdfCodigo.Value.Trim() <> String.Empty Then
            If isNodeChecked Then
                Dim sql = "SET DATEFORMAT DMY " & vbCrLf
                sql &= "EXEC sp_Menu_Permisos_Rol " &
                       "@cod_menu = " & e.Node.Value & "," &
                       "@cod_rol = " & hdfCodigo.Value & "," &
                       "@Tipo = 'INSERTAR'"

                If SaveDelete(sql) = True Then
                    AlertifySuccessMessage(Page, "Permisos actualizados correctamente.")
                End If

            ElseIf isNodeChecked = False Then
                Dim sql = "SET DATEFORMAT DMY " & vbCrLf
                sql &= "EXEC sp_Menu_Permisos_Rol " &
                       "@cod_menu = " & e.Node.Value & "," &
                       "@cod_rol = " & hdfCodigo.Value & "," &
                       "@Tipo = 'ELIMINAR'"

                If SaveDelete(sql) = True Then
                    AlertifySuccessMessage(Page, "Permisos actualizados correctamente.")
                End If
            End If

        End If

    End Sub

#Region "CARGAR DATOS DEL MENÚ"

    Private Sub LoadTreeView(dtParent As DataTable, parentId As Integer, treeNode As TreeNode)

        For Each row As DataRow In dtParent.Rows
            Dim child As New TreeNode With {
                    .Text = row("etiqueta").ToString(),
                    .Value = row("cod_menu").ToString()
                    }
            If parentId = 0 Then
                trvMenu.Nodes.Add(child)
                Dim dtChild As DataTable = Me.GetData("SELECT cod_menu, cod_padre, etiqueta, posicion FROM sys_menu_web WHERE cod_padre = " + child.Value)
                LoadTreeView(dtChild, Integer.Parse(child.Value), child)
            Else
                treeNode.ChildNodes.Add(child)

                GetCheckedNode(child)

            End If
        Next

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "checkParentCheckBoxOnLoad", "checkParentCheckBoxOnLoad()", True)

    End Sub
    Private Sub GetCheckedNode(child As TreeNode)
        'Este metodo se encarga de activar el checkbox de los nodos hijos si el cod_menu existe en la tabla sys_menu_permisos_roles

        Dim query = "SELECT 1 FROM sys_menu_permisos_roles WHERE cod_rol = " & hdfCodigo.Value & " AND cod_menu = " & child.Value

        Using dbCon As New OleDbConnection(_conn.conn)
            dbCon.Open()

            Using cmd As New OleDbCommand(query, dbCon)
                cmd.CommandType = CommandType.Text
                cmd.CommandText = query
                Dim reader As OleDbDataReader = cmd.ExecuteReader()

                If reader.HasRows Then
                    child.Checked = True
                End If

            End Using
        End Using

    End Sub

    Private Function GetData(query As String) As DataTable
        'Metodo para obtener un datatable de la base de datos. 
        Dim dt As New DataTable()

        Using dbCon As New OleDbConnection(_conn.conn)
            Using cmd As New OleDbCommand(query)
                Using da As New OleDbDataAdapter()
                    cmd.CommandType = CommandType.Text
                    cmd.Connection = dbCon
                    da.SelectCommand = cmd
                    da.Fill(dt)
                End Using
            End Using
            Return dt
        End Using
    End Function

    'Private Sub GetMenu()
    '    Try
    '        trvMenu.Nodes.Clear()

    '        Dim sql As String = "EXEC sp_Menu_web_admin " &
    '              "@cod_menu = NULL," &
    '              "@cod_Padre = NULL," &
    '              "@Posicion = NULL," &
    '              "@Etiqueta = NULL," &
    '              "@Ruta = NULL," &
    '              "@Pagina = NULL," &
    '              "@icono = NULL," &
    '              "@Activo = NULL," &
    '              "@Ocultar_Menu = NULL," &
    '              "@Tipo = 'FULL'"

    '        Dim ds As DataSet = _dataBase.GetDataSet(sql)

    '        Dim tnode As TreeNode

    '        For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
    '            If ds.Tables(0).Rows(i).Item("cod_Padre").ToString = String.Empty Then
    '                'Verifico si el usuario tiene asignado el menu
    '                tnode = New TreeNode
    '                tnode.Text = ds.Tables(0).Rows(i).Item("Etiqueta").ToString
    '                tnode.Value = ds.Tables(0).Rows(i).Item("cod_menu").ToString
    '                trvMenu.Nodes.Add(tnode)
    '                'hacemos un llamado al metodo recursivo encargado de generar el árbol del menú.
    '                AddMenuItem(tnode, ds)
    '            End If
    '        Next i

    '        ds.Dispose()

    '        Me.trvMenu.ExpandAll()

    '    Catch ex As Exception
    '        AlertifyErrorMessage(Page, ex.Message)
    '    End Try
    'End Sub

    ''' <summary>
    ''' Obtiene los permisos del rol seleccionado en el control ddlRol para mostrarlos en el control trvPermisos. 
    ''' </summary>
    ''' <remarks></remarks>
    'Private Sub GetMenuPermisosRol()
    '    Try
    '        trvPermisos.Nodes.Clear()

    '        Dim sql = "EXEC sp_Menu_Permisos_Rol " &
    '                  "@cod_menu = NULL," &
    '                  "@cod_rol = " & hdfCodigo.Value & "," &
    '                  "@Tipo = 'FULL'"

    '        Dim ds As DataSet = _dataBase.GetDataSet(sql)

    '        Dim treeNode As TreeNode

    '        For i = 0 To ds.Tables(0).Rows.Count - 1

    '            'Agregar los nodos que tienen el cod_Padre como String.Empty, los son los nodos principales del trvPermisos. 
    '            If ds.Tables(0).Rows(i).Item("cod_Padre").ToString() = String.Empty Then

    '                treeNode = New TreeNode With {
    '                    .Text = ds.Tables(0).Rows(i).Item("Etiqueta").ToString(),
    '                    .Value = ds.Tables(0).Rows(i).Item("cod_menu").ToString()
    '                }
    '                trvPermisos.Nodes.Add(treeNode)

    '                AddMenuItem(treeNode, ds)

    '            End If
    '        Next i

    '        ds.Dispose()
    '        trvPermisos.ExpandAll()

    '    Catch ex As Exception
    '        ltMensaje.Text = _conn.PmsgBox("Error al desplegar los permisos. " & ex.Message, "error")
    '    End Try
    'End Sub
    ''' <summary>
    '''  Crea los nodos hijos de cada nodo padre del control trvMenu utilizando un metodo recursivo.
    ''' </summary>
    ''' <param name="treeNode"></param>
    ''' <param name="ds"></param>
    ''' <remarks></remarks>
    'Private Sub AddMenuItem(ByRef treeNode As TreeNode, ds As DataSet)
    '    Try
    '        'Recorremos cada registro del DataTable para poder determinar cuales son elementos hijos del parametro treeNode. 

    '        For i = 0 To ds.Tables(0).Rows.Count - 1

    '            'Si el cod_Padre del registro actual es igual al cod_menu del treeNode.value.. 
    '            'Entonces el i registro actual es un elemento hijo del treeNode.. 
    '            If ds.Tables(0).Rows(i).Item("cod_Padre").ToString() = treeNode.Value Then

    '                If ds.Tables(0).Rows(i).Item("cod_menu").ToString() <> ds.Tables(0).Rows(i).Item("cod_Padre").ToString() Then

    '                    Dim newNode As New TreeNode With {
    '                        .Text = ds.Tables(0).Rows(i).Item("Etiqueta").ToString(),
    '                        .Value = ds.Tables(0).Rows(i).Item("cod_menu").ToString()
    '                    }

    '                    treeNode.ChildNodes.Add(newNode)

    '                    'llamada recursiva para ver si el nuevo menú ítem aun tiene elementos hijos.
    '                    AddMenuItem(newNode, ds)
    '                End If
    '            End If

    '        Next i
    '    Catch ex As Exception
    '        AlertifyErrorMessage(Page, ex.Message)
    '    End Try
    'End Sub

#End Region

#Region "PROCEDIMIENTOS DE LA BASE DE DATOS"
    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim sql = "EXEC sp_sys_roles_usuarios " &
              "@cod_rol = null, " &
              "@nombre_rol = '" & Me.txtRol.Text.Trim & "', " &
              "@cod_usuario = " & Request.Cookies("CKSMFACTURA")("cod_usuario") & ", " &
              "@modo = 'INSERTAR'"

        If SaveDelete(sql) = True Then
            AlertifySuccessMessage(Me, "Nuevo rol guardado correctamente.")
        End If
    End Sub

    Protected Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Dim sql = "EXEC sp_sys_roles_usuarios " &
                      "@cod_rol = " & hdfCodigo.Value & ", " &
                      "@nombre_rol = null, " &
                      "@cod_usuario = " & Request.Cookies("CKSMFACTURA")("cod_usuario") & ", " &
                      "@modo = 'ELIMINAR'"


        If SaveDelete(sql) = True Then
            AlertifySuccessMessage(Me, "Rol eliminado correctamente.")
        End If
    End Sub
    ''' <summary>
    ''' Envia una cadena sql a la base de datos para ser ejecutada.
    ''' </summary>
    ''' <param name="sql"></param>
    ''' <remarks></remarks>
    Private Function SaveDelete(sql As String) As Boolean
        Try
            Using dbCon As New OleDbConnection(_conn.conn)
                dbCon.Open()

                Dim cmd As New OleDbCommand(sql, dbCon)
                cmd.CommandType = CommandType.Text
                cmd.ExecuteNonQuery()

                Return True
            End Using

        Catch ex As Exception
            AlertifyErrorMessage(Page, ex.Message)
            Return False
        End Try
    End Function

    Private Sub trvMenu_SelectedNodeChanged(sender As Object, e As EventArgs) Handles trvMenu.SelectedNodeChanged

    End Sub

#End Region

End Class
