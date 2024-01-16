Imports System.Activities.Expressions
Imports System.Data
Imports System.Data.SqlClient
Imports AlertifyClass

Partial Class Catalogos_Siglas
    Inherits Page
    Dim Seguridad As New FACTURACION_CLASS.seguridad
    Dim Database As New FACTURACION_CLASS.database
    Dim BUSQUEDAD As String

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

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        LoadDataGridView()
        If Not Page.IsPostBack Then
            Puesto_Name = "Industrial Comercial San Martin"
            txtDescripcion.Attributes.Add("placeholder", "Nuevas Siglas..")
            txtDescripcion.Attributes.Add("requerid", "requerid")

        End If
    End Sub

#Region "PROCESOS Y EVENTOS DEL GRIDVIEW"
    Private Sub LoadDataGridView()
        Try
            Dim sql As String = "SELECT cod_sigla, sigla FROM dbo.Siglas"
            If Not String.IsNullOrEmpty(txtSearch.Text.Trim()) Then
                sql &= " WHERE sigla LIKE '%' + @SearchTerm + '%'"
                sql &= " ORDER BY sigla ASC"
            End If

            Using dbCon As New SqlConnection(Seguridad.Sql_conn)
                dbCon.Open()

                Using cmd As New SqlCommand(sql, dbCon)
                    cmd.Parameters.AddWithValue("@SearchTerm", txtSearch.Text.Trim())
                    Using sda As New SqlDataAdapter(cmd)
                        Dim dt As New DataTable()
                        sda.Fill(dt)
                        GridViewOne.DataSource = dt
                        GridViewOne.DataBind()
                    End Using
                End Using
            End Using

        Catch ex As Exception
            AlertifyAlertMessage(Me, ex.Message)
        End Try
    End Sub

    Protected Sub Search(sender As Object, e As EventArgs)
        'En control txtSearch se define el evento OnTextChanged="Search"
        'Cargar GridView con los parametros de Busqueda
        Me.LoadDataGridView()
    End Sub

    Protected Sub OnPaging(sender As Object, e As GridViewPageEventArgs)
        'En control GridView se define el evento OnPageIndexChanging="OnPaging"
        'Cargar GridView con los parametros de Busqueda
        GridViewOne.PageIndex = e.NewPageIndex
        Me.LoadDataGridView()
    End Sub

    Protected Sub BtnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If txtDescripcion.Text = String.Empty Then
            AlertifyAlertMessage(Me, "El campo no puede estar vacío.")
            Return
        End If
        Guardar()
    End Sub
    Private Sub Guardar()
        Dim siglas = txtDescripcion.Text.Trim()
        txtDescripcion.Text = ""

        Try
            Using dbCon As New SqlConnection(Seguridad.Sql_conn)
                Using cmd As New SqlCommand("Cat_Siglas", dbCon)
                    dbCon.Open()

                    cmd.Parameters.AddWithValue("@accion", "INSERT")
                    cmd.Parameters.AddWithValue("@cod_siglas", DBNull.Value)
                    cmd.Parameters.AddWithValue("@siglas", siglas)
                    cmd.Parameters.AddWithValue("@CodigoUser", Session("CodigoUser"))
                    cmd.CommandType = CommandType.StoredProcedure
                    Dim affectedRows = cmd.ExecuteNonQuery()

                    If affectedRows > 0 Then
                        AlertifySuccessMessage(Me, "El registro ha sido guardado correctamente.")
                    End If
                End Using
            End Using

            LoadDataGridView()
        Catch ex As Exception
            AlertifyErrorMessage(Me, "Ha ocurrido un error al intentar guardar los datos. Si el problema persiste, contacte con el administrador.")
        End Try
    End Sub

    Protected Sub GridViewOne_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles GridViewOne.RowEditing
        Try
            GridViewOne.EditIndex = e.NewEditIndex
            LoadDataGridView()

        Catch ex As Exception
            AlertifyErrorMessage(Me, "Ocurrió un error al disparar el evento SelectedIndexChanged")
        End Try
    End Sub
    Protected Sub GridViewOne_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles GridViewOne.RowUpdating

        Dim siglaId As String = GridViewOne.DataKeys(e.RowIndex).Values(0).ToString()
        Dim row As GridViewRow = GridViewOne.Rows(e.RowIndex)
        Dim name As String = DirectCast(row.FindControl("txtSiglas"), TextBox).Text

        Using dbCon As New SqlConnection(Seguridad.Sql_conn)
            Using cmd As New SqlCommand("Cat_Siglas", dbCon)
                dbCon.Open()
                cmd.Parameters.AddWithValue("@accion", "UPDATE")
                cmd.Parameters.AddWithValue("@cod_sigla", siglaId)
                cmd.Parameters.AddWithValue("@siglas", name)
                cmd.Parameters.AddWithValue("@CodigoUser", Session("CodigoUser"))
                cmd.CommandType = CommandType.StoredProcedure

                Dim affectedRows = cmd.ExecuteNonQuery()

                If affectedRows > 0 Then
                    AlertifySuccessMessage(Me, "El registro ha sido guardado correctamente.")
                End If
            End Using
        End Using

        GridViewOne.EditIndex = -1
        LoadDataGridView()
    End Sub

    Protected Sub GridViewOne_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles GridViewOne.RowCancelingEdit
        GridViewOne.EditIndex = -1
        LoadDataGridView()
    End Sub
    Protected Sub GridViewOne_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles GridViewOne.RowDeleting
        Try
            Dim siglaId As String = GridViewOne.DataKeys(e.RowIndex).Values(0).ToString()

            Using dbCon As New SqlConnection(Seguridad.Sql_conn)
                Using cmd As New SqlCommand("Cat_Siglas", dbCon)
                    dbCon.Open()

                    cmd.Parameters.AddWithValue("@accion", "DELETE")
                    cmd.Parameters.AddWithValue("@cod_sigla", siglaId)
                    cmd.Parameters.AddWithValue("@siglas", DBNull.Value)
                    cmd.Parameters.AddWithValue("@codusuario", DBNull.Value)
                    cmd.CommandType = CommandType.StoredProcedure

                    Dim affectedRows = cmd.ExecuteNonQuery()

                    If affectedRows > 0 Then
                        AlertifySuccessMessage(Me, "El registro ha sido guardado correctamente.")
                    End If
                End Using
            End Using

        Catch ex As Exception
            AlertifyErrorMessage(Me, "Ha ocurrido un error al intentar guardar los datos. Si el problema persiste, contacte con el administrador.")
        End Try
    End Sub

    Private Sub GridViewOne_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GridViewOne.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow AndAlso e.Row.RowIndex <> GridViewOne.EditIndex Then
            TryCast(e.Row.Cells(3).Controls(0), LinkButton).Attributes("onclick") = "return confirm('Do you want to delete this row?');"
        End If
    End Sub

#End Region

    'Protected Sub GridViewOne_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridViewOne.DataBound
    '    Try
    '        If GridViewOne.Rows.Count > 0 Then
    '            Dim pagerRow As GridViewRow = GridViewOne.BottomPagerRow
    '            Dim pageLabel As Label = CType(pagerRow.Cells(0).FindControl("CurrentPageLabel"), Label)
    '            If Not pageLabel Is Nothing Then
    '                Dim currentPage As Integer = GridViewOne.PageIndex + 1
    '                pageLabel.Text = "&nbsp;&nbsp; Pagina " & currentPage.ToString() &
    '                    " de " & GridViewOne.PageCount.ToString()
    '            End If
    '        End If
    '    Catch ex As Exception
    '        Me.ltMensajeGrid.Text &= Seguridad.PmsgBox("Ocurrió un error al disparar el evento DataBound. " & ex.Message, "error")

    '    End Try
    'End Sub
    'Protected Sub GridViewOne_PageIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridViewOne.PageIndexChanged
    '    Try

    '        Me.GridViewOne.SelectedIndex = -1
    '        Me.hdfCodigo.Value = String.Empty

    '        ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "smf_Script", "responsive_grid();", True)

    '    Catch ex As Exception
    '        Me.ltMensajeGrid.Text &= Seguridad.PmsgBox("Ocurrió un error al disparar el evento PageIndexChanged." & ex.Message, "error")

    '    End Try
    'End Sub

    'Protected Sub GridViewOne_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridViewOne.PageIndexChanging
    '    Try
    '        Me.GridViewOne.PageIndex = e.NewPageIndex

    '        'Para usar la de caché guardada en la variable de sesion
    '        If (IsPostBack) AndAlso (Not dtTabla Is Nothing) Then
    '            If Not dtTabla Is Nothing AndAlso dtTabla.Rows.Count > 0 Then
    '                If dtTabla.Rows.Count > 0 Then
    '                    Me.GridViewOne.DataSource = dtTabla
    '                    Me.GridViewOne.DataBind()
    '                End If
    '            End If
    '        End If

    '        ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "smf_Script", "responsive_grid();", True)

    '    Catch ex As Exception
    '        Me.ltMensajeGrid.Text &= Seguridad.PmsgBox("Ocurrió un error al disparar el evento PageIndexChanging." & ex.Message, "error")

    '    End Try
    'End Sub

    'Protected Sub GridViewOne_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridViewOne.RowDataBound
    '    Try

    '        If e.Row.RowType = DataControlRowType.DataRow Then
    '            e.Row.Attributes.Add("OnMouseOver", "On(this);")
    '            e.Row.Attributes.Add("OnMouseOut", "Off(this);")
    '            'e.Row.Attributes.Add("OnClick", "" & Page.ClientScript.GetPostBackClientHyperlink(Me.GridViewOne, "Select$" + e.Row.RowIndex.ToString) & ";")
    '        End If

    '    Catch ex As Exception
    '        Me.ltMensajeGrid.Text &= Seguridad.PmsgBox("Ocurrió un error al disparar el evento RowDataBound. " & ex.Message, "error")

    '    End Try
    'End Sub

    'Private Sub txtBuscar_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBuscar.TextChanged
    '    'BUSQUEDAD = "%" & UCase(Trim(txtBuscar.Text)) & "%"
    '    'If txtBuscar.Text <> "" Then
    '    '    BUSQUEDAD = IIf(Me.txtBuscar.Text.Trim = String.Empty, "0", "" & Me.txtBuscar.Text.Trim & "")
    '    '    Load_GridView()
    '    'Else
    '    '    BUSQUEDAD = "0"
    '    '    Load_GridView()
    '    'End If
    'End Sub

#Region "EXPORTAR DATOS A EXCELL"
    Protected Sub btnExportar_Click(sender As Object, e As EventArgs) Handles btnExportar.Click
        Try
            Response.Clear()
            Response.Buffer = True

            Dim name_doc = "catalogo_siglas" & Date.Now.Year & Date.Now.Month & Date.Now.Day

            'Response.ContentType = "application/ms-word"
            'Response.AddHeader("Content-Disposition", "attachment;filename=" & name_doc & ".doc")

            Response.ContentType = "application/vnd.ms-excel"
            Response.AddHeader("Content-Disposition", "attachment;filename=" & name_doc & ".xls")

            Response.Charset = "UTF-8"
            Response.ContentEncoding = Encoding.Default
            Response.BinaryWrite(System.Text.Encoding.UTF8.GetPreamble())

            Dim MyHTML As String = String.Empty
            MyHTML &= "<html>"
            MyHTML &= "<head>"
            MyHTML &= "<meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8""/>"
            MyHTML &= "<meta name=ProgId content=Word.Document>"
            MyHTML &= "<meta name=Generator content=""Microsoft Word 9"">"
            MyHTML &= "<meta name=Originator content=""Microsoft Word 9"">"
            MyHTML &= "<style>"
            MyHTML &= "@page Section1 {size:595.45pt 841.7pt; margin:1.0in 1.25in 1.0in 1.25in;mso-header-margin:.5in;mso-footer-margin:.5in;mso-paper-source:0;}"
            MyHTML &= "div.Section1 {page:Section1;}"
            MyHTML &= "@page Section2 {size:841.7pt 595.45pt;mso-page-orientation: landscape;margin:1.25in 1.0in 1.25in 1.0in;mso-header-margin:.5in; mso-footer-margin:.5in;mso-paper-source:0;}"
            MyHTML &= "div.Section2 {page:Section2;}"
            MyHTML &= "h1,h2{font-family:Calibri;text-align:center}h1{font-size:14pt}h2{font-size:12pt}"
            MyHTML &= "table{margin:0 auto;width:100%;background:#FFF;color:#333}table tbody tr{border:none}table tbody tr.alt{background-color:#f0f4f5}table tbody tr td,table tbody tr th{text-align:left;font-family:Calibri;font-size:11pt;padding:7px 0}table tbody tr th{font-style:italic;font-weight:700;border-bottom:solid 1px #333}table tbody tr td.first{border-right:solid 1px #333;text-align:right;font-style:italic;background-color:#FFF;padding-right:7px}table tbody tr td.ml{margin-left:7px}"
            MyHTML &= "</style>"
            MyHTML &= "</head>"

            MyHTML &= "<body>"
            MyHTML &= "<div class=""Section2"">"
            MyHTML &= "<h1>" & Puesto_Name & "</h1>"
            MyHTML &= "<h2>Catalogo de Siglas <br/> al dia " & Date.Now.Day & "/" & Date.Now.Month & "/" & Date.Now.Year & "</h2>"

            If Not dtTabla Is Nothing Then
                MyHTML &= "<table cellspacing=""0"" cellpadding=""0"">" &
                          "<tbody>" &
                          "<tr>" &
                          "<th></th>" &
                          "<th>SIGLA</th>" &
                          "</tr>"

                Dim clase As String = String.Empty

                For i As Integer = 0 To dtTabla.Rows.Count - 1
                    MyHTML &= "<tr" & clase & ">" &
                              "<td class=""first"">" & i + 1 & "</td>" &
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("sigla").ToString.Trim & "</td>" &
                              "<td></td>" &
                              "</tr>"

                    If clase = String.Empty Then
                        clase = " class=""alt"""
                    Else
                        clase = String.Empty
                    End If

                Next i

                MyHTML &= "</tbody>" &
                          "<table>"
            End If

            MyHTML &= "</div>"
            MyHTML &= "</body>"
            MyHTML &= "</html>"
            Response.Write(MyHTML)

            Response.End()


        Catch ex As Exception
            'Me.ltMensaje.Text = Seguridad.PmsgBox("Ocurrio un error al intentar exportar la tabla. " & ex.Message, "error")
        End Try
    End Sub

    Private Sub GridViewOne_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles GridViewOne.PageIndexChanging
        GridViewOne.PageIndex = e.NewPageIndex
        LoadDataGridView()
    End Sub


#End Region

End Class
