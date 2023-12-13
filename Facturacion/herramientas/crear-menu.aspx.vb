Imports System.Data
Imports System.Globalization
Imports System.Threading

Partial Class admin_crear_menu
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
            Form.DefaultButton = Me.btnGuardar.UniqueID
            GetMenu()

            Attributes_Text()

        End If
    End Sub

    Private Sub Attributes_Text()
        Me.txtCodigo.Attributes.Add("placeholder", "Código")
        Me.txtCodigo.Attributes.Add("autocomplete", "off")

        Me.txtPosicion.Attributes.Add("placeholder", "Posición")
        Me.txtPosicion.Attributes.Add("autocomplete", "off")

        Me.txtRutaPage.Attributes.Add("placeholder", "Ruta (Usar ~):")
        Me.txtRutaPage.Attributes.Add("autocomplete", "off")
        Me.txtRutaPage.Attributes.Add("onblur", "ObtenerPagina();")

        Me.txtEtiqueta.Attributes.Add("placeholder", "Etiqueta")
        Me.txtEtiqueta.Attributes.Add("autocomplete", "off")

        Me.txtNombrePagina.Attributes.Add("placeholder", "Página")
        Me.txtNombrePagina.Attributes.Add("autocomplete", "off")

        Me.txtIconos.Attributes.Add("placeholder", "Icono del Menú")
        Me.txtIconos.Attributes.Add("autocomplete", "off")


    End Sub

#Region "CARGAR DATOS EXISTENTES DEL MENÚ"
    Private Sub GetMenu()
        'Try
        trvMenu.Nodes.Clear()

        Dim sql As String = String.Empty
        sql = "EXEC sp_Menu_web_admin " & _
              "@cod_menu = NULL," & _
              "@cod_Padre = NULL," & _
              "@Posicion = NULL," & _
              "@Etiqueta = NULL," & _
              "@Ruta = NULL," & _
              "@Pagina = NULL," & _
              "@icono = NULL," & _
              "@Activo = NULL," & _
              "@Ocultar_Menu = NULL," & _
              "@Tipo = 'FULL'"

        Dim ds As DataSet = DataBase.GetDataSet(sql)

        Dim tnode As TreeNode

        For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
            If ds.Tables(0).Rows(i).Item("cod_Padre").ToString = String.Empty Then
                'Verifico si el usuario tiene asignado el menu
                tnode = New TreeNode
                tnode.Text = ds.Tables(0).Rows(i).Item("Etiqueta").ToString
                tnode.Value = ds.Tables(0).Rows(i).Item("cod_menu").ToString
                trvMenu.Nodes.Add(tnode)
                'hacemos un llamado al metodo recursivo encargado de generar el árbol del menú.
                AddMenuItem(tnode, ds)
            End If
        Next i

        ds.Dispose()

        Me.trvMenu.ExpandAll()

        'Catch ex As Exception
        '    Me.ltMensaje.Text &= conn.pmsgBox("Ocurrio un error al intentar crear el arbol de nodos. " & ex.Message, "error")

        'End Try
    End Sub

    ''' <summary>
    ''' UTILIZA EL DATASET CARGADO EN EL PROCESO GetMenu PARA CREAR LOS NODOS HIJOS
    ''' </summary>
    ''' <param name="tnode"></param>
    ''' <param name="ds"></param>
    ''' <remarks></remarks>
    Private Sub AddMenuItem(ByRef tnode As TreeNode, ByVal ds As DataSet)
        'Try
        '' '' ''Recorremos cada elemento del datatable para poder determinar cuales son elementos hijos
        '' '' ''del menuitem dado pasado como parametro ByRef.

        For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
            If ds.Tables(0).Rows(i).Item("cod_Padre").ToString = tnode.Value AndAlso Not ds.Tables(0).Rows(i).Item("cod_menu").ToString = ds.Tables(0).Rows(i).Item("cod_Padre").ToString Then
                Dim newnode As New TreeNode
                newnode.Value = ds.Tables(0).Rows(i).Item("cod_menu").ToString
                newnode.Text = ds.Tables(0).Rows(i).Item("Etiqueta").ToString

                tnode.ChildNodes.Add(newnode)

                'llamada recursiva para ver si el nuevo menú ítem aun tiene elementos hijos.
                AddMenuItem(newnode, ds)
            End If
        Next i
        'Catch ex As Exception
        '    Me.ltMensaje.Text &= conn.pmsgBox("Ocurrio un error al intentar crear los hijos del arbol de nodos. " & ex.Message, "error")

        'End Try
    End Sub
#End Region

#Region "MOSTRAR LOS DATOS DEL NODO SELECCIONADO EN EL FORMULARIO"
    Protected Sub trvMenu_SelectedNodeChanged(sender As Object, e As EventArgs) Handles trvMenu.SelectedNodeChanged
        Me.hdfCodigo.Value = Me.trvMenu.SelectedValue
        GetRegMenu()
    End Sub

    Private Sub GetRegMenu()
        Try
            Me.ltMensaje.Text = String.Empty

            Dim SQL As String = String.Empty
            SQL = "EXEC sp_Menu_web_admin " & _
                  "@cod_menu = " & Me.hdfCodigo.Value.Trim & "," & _
                  "@cod_Padre = NULL," & _
                  "@Posicion = NULL," & _
                  "@Etiqueta = NULL," & _
                  "@Ruta = NULL," & _
                  "@Pagina = NULL," & _
                  "@icono = NULL," & _
                  "@Activo = NULL," & _
                  "@Ocultar_Menu = NULL," & _
                  "@Tipo = 'READER'"

            Dim dr As System.Data.OleDb.OleDbDataReader = DataBase.GetDataReader(SQL)
            If dr.Read() Then
                Me.txtCodigo.Text = dr.Item("cod_menu").ToString()
                Me.ccddNodoPadre.SelectedValue = dr.Item("cod_Padre").ToString()
                Me.txtPosicion.Text = dr.Item("Posicion").ToString()
                Me.txtEtiqueta.Text = dr.Item("Etiqueta").ToString()
                Me.txtRutaPage.Text = dr.Item("Ruta").ToString()
                Me.txtIconos.Text = dr.Item("Icono").ToString()
                Me.txtNombrePagina.Text = dr.Item("Pagina").ToString()
                Me.chkOcultarMenu.Checked = IIf(dr.Item("Ocultar_Menu").ToString() = "Si", True, False)
            End If

            dr.Close()

        Catch ex As Exception
            Me.ltMensaje.Text = conn.PmsgBox(ex.Message, "error")

        End Try
    End Sub
#End Region

#Region "PROCEDIMIENTOS EJECUTADOS EN LA BASE DE DATOS"
    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If Me.txtPosicion.Text.Trim = String.Empty Then
            Me.ltMensaje.Text = conn.PmsgBox("El proceso no puede continuar. Debe escribir la posicion que tendra el apartado en el menú..", "alerta")
            Exit Sub
        End If

        If Me.txtEtiqueta.Text.Trim = String.Empty Then
            Me.ltMensaje.Text = conn.PmsgBox("El proceso no puede continuar. Debe escribir una etiqueta que identifique el apartado en el menú.", "alerta")
            Exit Sub
        End If

        Me.ltMensaje.Text = String.Empty

        Dim sql As String = String.Empty
        sql = " EXEC sp_Menu_web_admin " & _
              "@cod_menu = " & IIf(Me.txtCodigo.Text.Trim = String.Empty, "NULL", Me.txtCodigo.Text.Trim) & "," & _
              "@cod_Padre = " & IIf(Me.ddlNodoPadre.Text = String.Empty, "NULL", Me.ddlNodoPadre.SelectedValue) & "," & _
              "@Posicion = " & Me.txtPosicion.Text.Trim & "," & _
              "@Etiqueta = " & IIf(Me.txtEtiqueta.Text.Trim = String.Empty, "NULL", "'" & Me.txtEtiqueta.Text.Trim & "'") & "," & _
              "@Ruta = " & IIf(Me.txtRutaPage.Text.Trim = String.Empty, "NULL", "'" & Me.txtRutaPage.Text.Trim & "'") & "," & _
              "@Pagina = " & IIf(Me.txtNombrePagina.Text.Trim = String.Empty, "NULL", "'" & Me.txtNombrePagina.Text.Trim & "'") & "," & _
              "@icono = " & IIf(Me.txtIconos.Text.Trim = String.Empty, "NULL", "'" & Me.txtIconos.Text.Trim & "'") & "," & _
              "@Activo = 1," & _
              "@Ocultar_Menu = " & IIf(Me.chkOcultarMenu.Checked = True, 1, 0) & "," & _
              "@Tipo = 'INSERTAR'"

        If Me.txtCodigo.Text.Trim = String.Empty Then
            Me.txtPosicion.Text = CInt(Me.txtPosicion.Text) + 1
            Me.txtRutaPage.Text = "~/"
            Me.txtEtiqueta.Text = String.Empty
            Me.txtNombrePagina.Text = String.Empty
            Me.txtIconos.Text = String.Empty
        End If

        Guardar(sql, "", "El proceso ha finalizado de forma correcta.", "Ha ocurrido un error al intentar guardar el registro.")
    End Sub

    Protected Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        If Me.txtCodigo.Text.Trim = String.Empty Then
            Me.ltMensaje.Text = conn.PmsgBox("El proceso no puede continuar. Debe seleccionar un menú.", "alerta")
            Exit Sub
        End If

        Me.ltMensaje.Text = String.Empty

        Dim sql As String = String.Empty
        sql = " EXEC sp_Menu_web_admin " &
              "@cod_menu = " & Me.txtCodigo.Text.Trim & "," &
              "@cod_Padre = NULL," &
              "@Posicion = NULL," &
              "@Etiqueta = NULL," &
              "@Ruta = NULL," &
              "@Pagina = NULL," &
              "@icono = NULL," &
              "@Activo = 0," &
              "@Ocultar_Menu = NULL," &
              "@Tipo = 'ELIMINAR'"

        Guardar(sql, "", "El menú ha sido eliminado de forma correcta.", "Ha ocurrido un error al intentar eliminar el registro.")
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

            Me.ltMensaje.Text = conn.PmsgBox(ExitoMsg, "exito")

            GetMenu()

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
