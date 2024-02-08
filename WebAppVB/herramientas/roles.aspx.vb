Imports System.Data
Imports System.Data.OleDb
Imports WebAppVB.AlertifyClass
Imports FACTURACION_CLASS
Imports WebAppVB.FACTURACION_CLASS

Partial Class herramientas_roles
    Inherits Page

    Dim _conn As New seguridad
    Dim _dataBase As New database
    Private WithEvents DropdownsClass As New DropdownsClass()

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
            LoadDdlRol()
        End If

        'Agregamos el evento onclick al control trvMenu para que se ejecute el metodo checkBoxPostBack() cuando se haga click en el control.
        'trvMenu.Attributes.Add("onclick", "checkBoxPostBack()")

    End Sub

    Private Sub trvMenu_TreeNodeCheckChanged(sender As Object, e As TreeNodeEventArgs) Handles trvMenu.TreeNodeCheckChanged

        If ddlRol.SelectedValue = String.Empty Then
            AlertifyErrorMessage(Page, "Debe seleccionar un rol.")
            Exit Sub
        End If

        Dim isNodeChecked As Boolean = trvMenu.CheckedNodes.Contains(e.Node)

        'Si el nodo tiene un checkbox checked, se actualiza sys_menu_permisos_roles con el valor INSERTAR
        If isNodeChecked Then
            UpdateMenuPermissions(e.Node.Value, "INSERTAR")
        Else
            UpdateMenuPermissions(e.Node.Value, "ELIMINAR")
        End If
    End Sub

    Private Sub UpdateMenuPermissions(ByVal cod_menu As String, ByVal tipo As String)

        Dim sql As String = $"SET DATEFORMAT DMY
                         EXEC sp_Menu_Permisos_Rol
                         @cod_menu = {cod_menu},
                         @CodigoRol = {hdfCodigo.Value},
                         @Tipo = '{tipo}'"

        If _dataBase.SaveToDatabase(sql) Then
            AlertifySuccessMessage(Page, "Permisos actualizados correctamente.")
        End If
    End Sub

    Private Sub btnGuardarCambios_Click(sender As Object, e As EventArgs) Handles btnGuardarCambios.Click

    End Sub

    Protected Sub ddlRol_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlRol.SelectedIndexChanged

        hdfCodigo.Value = ddlRol.SelectedItem.Value

        trvMenu.Nodes.Clear()

        'Obtener un DataTable con los nodos padre desde la tabla sys_menu_web_parent. 
        'y Llenar los nodos padres con los elementos del menu en el siguiente procedimiento

        LoadTreeView()

        'For i As Integer = 0 To dt.Rows.Count - 1
        'Convert.ToInt32(dt.Rows(i).Item("cod_menu"))
        'Next
        trvMenu.CollapseAll()
    End Sub
    Private Sub LoadDdlRol()
        DropdownsClass.BindDropDownList(ddlRol, $"EXEC CombosProductos @opcion = 24, @codigo = {Session("CodigoRol")}", "CodigoRol", "Descripcion", "Seleccione Rol")
    End Sub

#Region "LOAD TREEVIEW"
    Private Sub LoadTreeView()

        Try
            Dim dtParent = _dataBase.GetDataTable($"EXEC sp_crear_menu_web @Tipo = PADRE, @cod_padre = NULL")

            'Recorrer la tabla de nodos padre 
            For Each row As DataRow In dtParent.Rows

                Dim node As New TreeNode With {
                        .Text = row("etiqueta").ToString(),
                        .Value = row("cod_menu").ToString()
                        }

                trvMenu.Nodes.Add(node)

                'dtChild ya contiene los elementos del menu que son hijos del nodo padre actual.
                Dim dtChild = _dataBase.GetDataTable($"EXEC sp_crear_menu_web @Tipo = HIJOS, @cod_padre = {node.Value}")

                If dtChild.Rows.Count > 0 Then
                    GetChildNodes(dtChild, Integer.Parse(node.Value), node)
                End If

            Next

            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "checkParentCheckBoxOnLoad", "checkParentCheckBoxOnLoad()", True)

        Catch ex As Exception
            AlertifyAlertMessage(Me.Page, "Ocurrió un error al cargar los nodos padre del menú.")
        End Try
    End Sub

    Private Sub GetChildNodes(dtChild As DataTable, parentId As Integer, parentNode As TreeNode)

        Try

            For Each row As DataRow In dtChild.Rows

                Dim node As New TreeNode With {
                        .Text = row("etiqueta").ToString(),
                        .Value = row("cod_menu").ToString()
                        }

                parentNode.ChildNodes.Add(node)
                GetCheckedNode(node)
            Next

        Catch ex As Exception
            AlertifyAlertMessage(Me.Page, "Ocurrió un error al cargar los nodos hijos del menú.")
        End Try

    End Sub

    Private Sub GetCheckedNode(child As TreeNode)

        Try
            'Este metodo se encarga de activar el checkbox de los nodos hijos
            'si el nodo hijo existe en la tabla sys_menu_permisos_roles

            Dim sql = $"SELECT 1 FROM sys_menu_permisos_roles WHERE cod_rol = {hdfCodigo.Value} AND cod_menu = {child.Value}"

            Dim dt As DataTable = _dataBase.GetDataTable(sql)

            If dt.Rows.Count > 0 Then
                child.Checked = True
            End If
        Catch ex As Exception
            AlertifyAlertMessage(Me.Page, "Ocurrió un error al cargar los permisos del rol.")
        End Try


    End Sub



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
            AlertifyErrorMessage(Me, ex.Message)
            Return False
        End Try
    End Function

    Private Sub trvMenu_SelectedNodeChanged(sender As Object, e As EventArgs) Handles trvMenu.SelectedNodeChanged

    End Sub

#End Region
End Class
