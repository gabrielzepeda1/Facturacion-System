Imports System.Data
Imports System.Data.OleDb
Imports FACTURACION_CLASS

Partial Class admin_crear_menu
    Inherits Page
    Dim conn As New seguridad
    Dim DataBase As New database

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
            GetTreeview()

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

    Private Sub GetTreeview()
        Try
            trvMenu.Nodes.Clear()

            Dim dtParent = DataBase.GetDataTable($"EXEC sp_crear_menu_web @Tipo = PADRE, @cod_padre = NULL")

            Dim html As New StringBuilder()
            Dim tnode As TreeNode

            For i As Integer = 0 To dtParent.Rows.Count - 1

                Dim cod_padre = dtParent.Rows(i).Item("cod_menu").ToString()
                Dim dtChild As DataTable = DataBase.GetDataTable($"EXEC sp_menu_permisos_rol @CodigoRol = {Session("CodigoRol")}, @cod_menu = {cod_padre}, @Tipo = HIJOS")

                If dtChild.Rows.Count > 0 Then
                    html.AppendLine("<li class=""nav-item"">")
                    html.AppendLine($"<a class=""nav-link"" href=""#"">")
                    html.AppendLine($"<i class=""{dtParent.Rows(i).Item("icono").ToString().Trim()}""></i>")
                    html.AppendLine(dtParent.Rows(i).Item("etiqueta").ToString.Trim())
                    html.AppendLine("</a>")

                    html.AppendLine("<ul class=""nav-treeview"">")
                    html.AppendLine($"<li>{GetTreeViewDetalle(dtChild, cod_padre)}</li>")
                    html.AppendLine("</ul>")
                    html.AppendLine("</li>")

                End If
            Next i

            ltTreeview.Text = html.ToString()

            trvMenu.ExpandAll()

        Catch ex As Exception
            Me.ltMensaje.Text &= conn.PmsgBox("Ocurrio un error al intentar crear el arbol de nodos. " & ex.Message, "error")

        End Try
    End Sub

    Private Function GetTreeViewDetalle(dtChild As DataTable, cod_padre As String) As String

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

                    html.AppendLine($"<a class=""nav-link text-decoration-none"" href=""""><i class=""{clase}""></i> {label}</a>")

                End If
            Next i

            Return html.ToString()

        Catch ex As Exception

        End Try

    End Function

    ''' <summary>
    ''' UTILIZA EL DATASET CARGADO EN EL PROCESO GetTreeview PARA CREAR LOS NODOS HIJOS
    ''' </summary>
    ''' <param name="tnode"></param>
    ''' <param name="ds"></param>
    ''' <remarks></remarks>
    Private Sub AddMenuItem(ByRef tnode As TreeNode, ByVal dt As DataTable)
        'Try
        '' '' ''Recorremos cada elemento del datatable para poder determinar cuales son elementos hijos
        '' '' ''del menuitem dado pasado como parametro ByRef.

        For i As Integer = 0 To dt.Rows.Count - 1
            If dt.Rows(i).Item("cod_Padre").ToString = tnode.Value AndAlso Not dt.Rows(i).Item("cod_menu").ToString = dt.Rows(i).Item("cod_Padre").ToString Then
                Dim newnode As New TreeNode
                newnode.Value = dt.Rows(i).Item("cod_menu").ToString
                newnode.Text = dt.Rows(i).Item("Etiqueta").ToString

                tnode.ChildNodes.Add(newnode)

                'llamada recursiva para ver si el nuevo menú ítem aun tiene elementos hijos.
                AddMenuItem(newnode, dt)
            End If
        Next i
        'Catch ex As Exception
        '    Me.ltMensaje.Text &= conn.pmsgBox("Ocurrio un error al intentar crear los hijos del arbol de nodos. " & ex.Message, "error")

        'End Try
    End Sub
#End Region

#Region "MOSTRAR LOS DATOS DEL NODO SELECCIONADO EN EL FORMULARIO"
    Protected Sub trvMenu_SelectedNodeChanged(sender As Object, e As EventArgs) Handles trvMenu.SelectedNodeChanged
        hdfCodigo.Value = trvMenu.SelectedValue
        GetRegMenu()
    End Sub

    Private Sub GetRegMenu()
        Try
            Me.ltMensaje.Text = String.Empty

            Dim SQL As String = String.Empty
            SQL = "EXEC sp_Menu_web_admin " &
                  "@cod_menu = " & Me.hdfCodigo.Value.Trim & "," &
                  "@cod_Padre = NULL," &
                  "@Posicion = NULL," &
                  "@Etiqueta = NULL," &
                  "@Ruta = NULL," &
                  "@Pagina = NULL," &
                  "@icono = NULL," &
                  "@Activo = NULL," &
                  "@Ocultar_Menu = NULL," &
                  "@Tipo = 'READER'"

            Dim dr As System.Data.OleDb.OleDbDataReader = DataBase.GetDataReader(SQL)
            If dr.Read() Then
                txtCodigo.Text = dr.Item("cod_menu").ToString()
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
        sql = " EXEC sp_Menu_web_admin " &
              "@cod_menu = " & IIf(Me.txtCodigo.Text.Trim = String.Empty, "NULL", Me.txtCodigo.Text.Trim) & "," &
              "@cod_Padre = " & IIf(Me.ddlNodoPadre.Text = String.Empty, "NULL", Me.ddlNodoPadre.SelectedValue) & "," &
              "@Posicion = " & Me.txtPosicion.Text.Trim & "," &
              "@Etiqueta = " & IIf(Me.txtEtiqueta.Text.Trim = String.Empty, "NULL", "'" & Me.txtEtiqueta.Text.Trim & "'") & "," &
              "@Ruta = " & IIf(Me.txtRutaPage.Text.Trim = String.Empty, "NULL", "'" & Me.txtRutaPage.Text.Trim & "'") & "," &
              "@Pagina = " & IIf(Me.txtNombrePagina.Text.Trim = String.Empty, "NULL", "'" & Me.txtNombrePagina.Text.Trim & "'") & "," &
              "@icono = " & IIf(Me.txtIconos.Text.Trim = String.Empty, "NULL", "'" & Me.txtIconos.Text.Trim & "'") & "," &
              "@Activo = 1," &
              "@Ocultar_Menu = " & IIf(Me.chkOcultarMenu.Checked = True, 1, 0) & "," &
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
        Dim dbCon As New System.Data.OleDb.OleDbConnection(conn.conn)
        Try
            If dbCon.State = ConnectionState.Closed Then
                dbCon.Open()
            End If

            Dim cmd As New OleDb.OleDbCommand(sql, dbCon)
            cmd.ExecuteNonQuery()

            Me.ltMensaje.Text = conn.PmsgBox(ExitoMsg, "exito")

            GetTreeview()

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
