Imports System.Data
Imports System.Globalization
Imports System.Threading

Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Data.SqlClient
Imports System.Configuration
Partial Class catalogos_ProductoPais
    Inherits System.Web.UI.Page

    Dim conn As New FACTURACION_CLASS.seguridad
    Dim DataBase As New FACTURACION_CLASS.database
    Dim BUSQUEDAD As String
    Dim vPais As Integer

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

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then

            Puesto_Name = "Industrial Comercial San Martin"
            'Me.TextCodProducto.Text = "LLenar datos para el Producto Maestro: " + Session("Codigo") + " " + Session("Descrip") + " Para el País  " + Request.Cookies("CKSMFACTURA")("desPais")
            'Me.TextCodProducto.Enabled = False
            'Me.TextDecImpri.Attributes.Add("requerid", "requerid")
            'Me.ddlUnidadMed.Attributes.Add("requerid", "requerid")

            Me.txtBuscar.Attributes.Add("placeholder", "Escriba para buscar...")

            Load_GridView()
        End If
    End Sub

#Region "PROCESOS Y EVENTOS DEL GRIDVIEW"
    Protected Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        Load_GridView()

        ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "smf_Script", "responsive_grid();", True)
    End Sub

    Private Sub Load_GridView()
        Try
            Dim vCPais As String = String.Empty
            vCPais = Context.Request.Cookies("CKSMFACTURA")("codPais")
            Dim SQL As String = String.Empty
            SQL &= "EXEC Cat_ProductosXPais @opcion=3," & _
                  "@codPais =  " & vCPais & " ," & _
                  "@codigoProducto =  '0'  ," & _
                  "@codUndMedida = 0  ," & _
                  "@descripcion = '0'  ," & _
                  "@descripImpr = '0'  ," & _
                  "@numProducto = 0  ," & _
                  "@activo = 0  ," & _
                  "@ExcentoImp = 0  ," & _
                  "@codEmpresa =  " & Request.Cookies("CKSMFACTURA")("CodEmpresa") & " ," & _
                  "@codusuario =  NULL ," & _
                  "@codusuarioUlt =  NULL  ," & _
                  "@BUSQUEDAD = '" & BUSQUEDAD & "' "

            Dim ds As DataSet
            ds = DataBase.GetDataSet(SQL)

            dtTabla = ds.Tables(0)

            Me.GridViewOne.DataSource = dtTabla.DefaultView
            Me.GridViewOne.DataBind()
            ds.Dispose()

        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.pmsgBox("Ocurrió un error al intentar cargar el listado de rubros en la tabla." & ex.Message, "error")

        End Try
    End Sub

    Protected Sub GridViewOne_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridViewOne.DataBound
        Try
            If GridViewOne.Rows.Count > 0 Then
                Dim pagerRow As GridViewRow = GridViewOne.BottomPagerRow
                Dim pageLabel As Label = CType(pagerRow.Cells(0).FindControl("CurrentPageLabel"), Label)
                If Not pageLabel Is Nothing Then
                    Dim currentPage As Integer = GridViewOne.PageIndex + 1
                    pageLabel.Text = "&nbsp;&nbsp; Pagina " & currentPage.ToString() & _
                        " de " & GridViewOne.PageCount.ToString()
                End If
            End If
        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.pmsgBox("Ocurrió un error al disparar el evento DataBound. " & ex.Message, "error")

        End Try
    End Sub

    Protected Sub GridViewOne_PageIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridViewOne.PageIndexChanged
        Try

            Me.GridViewOne.SelectedIndex = -1
            Me.hdfCodigo.Value = String.Empty

            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "smf_Script", "responsive_grid();", True)

        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.pmsgBox("Ocurrió un error al disparar el evento PageIndexChanged." & ex.Message, "error")

        End Try
    End Sub

    Protected Sub GridViewOne_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridViewOne.PageIndexChanging
        Try
            Me.GridViewOne.PageIndex = e.NewPageIndex

            'Para usar la de caché guardada en la variable de sesion
            If (IsPostBack) AndAlso (Not dtTabla Is Nothing) Then
                If Not dtTabla Is Nothing AndAlso dtTabla.Rows.Count > 0 Then
                    If dtTabla.Rows.Count > 0 Then
                        Me.GridViewOne.DataSource = dtTabla
                        Me.GridViewOne.DataBind()
                    End If
                End If
            End If

            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "smf_Script", "responsive_grid();", True)

        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.pmsgBox("Ocurrió un error al disparar el evento PageIndexChanging." & ex.Message, "error")

        End Try

    End Sub

    'Protected Sub GridViewOne_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridViewOne.RowDataBound
    '    Try
    '        ''carga el campo llave es el 2dopaso DataKeyName
    '        'If e.Row.RowType = DataControlRowType.DataRow Then
    '        '    'e.Row.Attributes.Add("OnMouseOver", "On(this);")
    '        '    'e.Row.Attributes.Add("OnMouseOut", "Off(this);")
    '        '    e.Row.Attributes.Add("OnClick", "" & Page.ClientScript.GetPostBackClientHyperlink(Me.GridViewOne, "Select$" + e.Row.RowIndex.ToString) & ";")
    '        'End If

    '    Catch ex As Exception
    '        Me.ltMensajeGrid.Text &= conn.pmsgBox("Ocurrió un error al disparar el evento RowDataBound. " & ex.Message, "error")

    '    End Try
    'End Sub

    Protected Sub GridViewOne_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles GridViewOne.RowDeleting
        Try
            'pasar la información del Gridview hacia otro control, en este caso el control HiddenField,
            Me.hdfCodigo.Value = Me.GridViewOne.DataKeys(e.RowIndex).Value
            vPais = GridViewOne.DataKeys(e.RowIndex).Item(1).ToString()

            Eliminar()

            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "smf_Script", "responsive_grid();", True)

        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.pmsgBox("Ocurrió un error al disparar el evento SelectedIndexChanged. " & ex.Message, "error")

        End Try
    End Sub

    Protected Sub GridViewOne_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles GridViewOne.RowCancelingEdit
        Me.GridViewOne.EditIndex = -1
        Load_GridView()
    End Sub


    Protected Sub GridViewOne_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles GridViewOne.RowEditing
        Try
            Me.GridViewOne.EditIndex = e.NewEditIndex
            Load_GridView()



            Dim id As String = Me.GridViewOne.DataKeys(e.NewEditIndex).Value


            'Dim vactivo As Boolean = GridViewOne.Rows.Item(7).ToString
            Dim vUniMed As Integer
            Dim vactivo As Boolean
            Dim vImto As Boolean
            Dim Sql As String
            vPais = Context.Request.Cookies("CKSMFACTURA")("codPais")
            Sql = " SET DATEFORMAT DMY " + "SELECT cod_und_medida,CASE WHEN activo = 1 THEN 'Si' ELSE 'No' END AS activo,CASE WHEN excento_imp = 1 THEN 'Si' ELSE 'No' END AS excento_imp FROM  Productos_Pais pm WHERE RTRIM(ltrim(pm.cod_producto))= '" & id & "' and cod_pais= " & vPais & " "
            Dim fr As System.Data.OleDb.OleDbDataReader = DataBase.GetDataReader(Sql)
            If fr.Read() Then
                vUniMed = fr.Item("cod_und_medida").ToString()
                vactivo = IIf(fr.Item("activo").ToString() = "Si", True, False)
                vImto = IIf(fr.Item("excento_imp").ToString() = "Si", True, False)
            End If


            Dim ckeckAc As CheckBox = TryCast(GridViewOne.Rows(e.NewEditIndex).FindControl("CheckAt"), CheckBox)
            If ckeckAc IsNot Nothing Then
                ckeckAc.Checked = vactivo
            End If


            Dim ckeckImto As CheckBox = TryCast(GridViewOne.Rows(e.NewEditIndex).FindControl("CheckImtos"), CheckBox)
            If ckeckImto IsNot Nothing Then
                ckeckImto.Checked = vImto
            End If

            Dim dataSetUnidadMedi As New DataSet
            dataSetUnidadMedi = DataBase.GetDataSet("SELECT cod_und_medida,RTRIM(ltrim(Descripcion)) AS UnidadMedida FROM Unidades_Medidas  ")
            dtTabla = dataSetUnidadMedi.Tables(0)

            Dim comboUniMedida As DropDownList = TryCast(GridViewOne.Rows(e.NewEditIndex).FindControl("ddlUnidadMed"), DropDownList)
            If comboUniMedida IsNot Nothing Then
                comboUniMedida.DataSource = dataSetUnidadMedi
                comboUniMedida.DataTextField = "UnidadMedida"
                comboUniMedida.DataValueField = "cod_und_medida"
                comboUniMedida.SelectedValue = vUniMed
                comboUniMedida.DataBind()
            End If


            'Dim dataSetPais As New DataSet
            'dataSetPais = DataBase.GetDataSet("SELECT cod_pais as codPais,descripcion as DesPais FROM Paises  ")
            'dtTabla = dataSetPais.Tables(0)
            'Dim comboSigla As DropDownList = TryCast(GridViewOne.Rows(e.NewEditIndex).FindControl("ddlPais"), DropDownList)
            'If comboSigla IsNot Nothing Then
            '    comboSigla.DataSource = dataSetPais
            '    comboSigla.DataTextField = "DesPais"
            '    comboSigla.DataValueField = "codPais"
            '    comboSigla.SelectedValue = vPais
            '    comboSigla.DataBind()
            'End If


        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.pmsgBox("Ocurrió un error al disparar el evento RowEditing SelectedIndexChanged. " & ex.Message, "error")

        End Try

    End Sub

    Protected Sub GridViewOne_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles GridViewOne.RowUpdating
        'ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "smf_Script", " alert(""Esto es un mensaje"");", True)

        Dim Codigo As String = e.Keys("codigo").ToString
        Dim Descripcion As String = e.NewValues("producto").ToString
        Dim deImpre As String = e.NewValues("DescripImpresion").ToString
        Dim Numero As String = 0
        'Dim Numero As String = e.NewValues("num_producto").ToString
        Dim combo As DropDownList
        Dim Impt As Boolean
        Dim Vac As Boolean

        'combo = TryCast(GridViewOne.Rows(e.RowIndex).FindControl("ddlPais"), DropDownList)
        'Dim Pais As Integer = Convert.ToInt32(combo.SelectedValue)


        combo = TryCast(GridViewOne.Rows(e.RowIndex).FindControl("ddlUnidadMed"), DropDownList)
        Dim VUnidad As Integer = Convert.ToInt32(combo.SelectedValue)


        Dim cb As CheckBox
        cb = TryCast(GridViewOne.Rows(e.RowIndex).FindControl("CheckImtos"), CheckBox)
        Impt = cb.Checked


        cb = TryCast(GridViewOne.Rows(e.RowIndex).FindControl("CheckAt"), CheckBox)
        Vac = cb.Checked


        Actualizar(Codigo, Descripcion, deImpre, Numero, VUnidad, Impt, Vac)
        Me.GridViewOne.EditIndex = -1
        Load_GridView()


    End Sub
#End Region

#Region "ENVIAR INFORMACIÓN HACIA LA BASE DE DATOS"

    Private Sub txtBuscar_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBuscar.TextChanged
        BUSQUEDAD = "%" & UCase(Trim(txtBuscar.Text)) & "%"
        If txtBuscar.Text <> "" Then
            BUSQUEDAD = IIf(Me.txtBuscar.Text.Trim = String.Empty, "0", "" & Me.txtBuscar.Text.Trim & "")
            Load_GridView()
        Else
            BUSQUEDAD = "0"
            Load_GridView()
        End If
    End Sub

    'Protected Sub BtnGuardar_Click(sender As Object, e As EventArgs) Handles BtnGuardar.Click
    '    Dim MessegeText As String = String.Empty

    '    If Me.TextDecImpri.Text = String.Empty Then
    '        MessegeText = "alertify.alert('El  proceso no puede continuar. Debe digitar descripción de impresion');"
    '        ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)
    '        Exit Sub
    '    End If



    '    If Me.ddlUnidadMed.SelectedIndex = -1 Then
    '        MessegeText = "alertify.alert('El  proceso no puede continuar. Debe escojer Unidad de medida.');"
    '        ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)
    '        Exit Sub
    '    End If

    '    Guardar()
    'End Sub
    Protected Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        Response.Redirect("ProductoPaisEmp.aspx")
    End Sub
    'Private Sub Guardar()
    '    Dim MessegeText As String = String.Empty

    '    Dim dbCon As New System.Data.OleDb.OleDbConnection(conn.conn)
    '    Try
    '        If dbCon.State = ConnectionState.Closed Then
    '            dbCon.Open()
    '        End If

    '        Dim sql As String = "SET DATEFORMAT DMY " & vbCrLf
    '        sql &= "EXEC Cat_ProductosXPais @opcion=1," & _
    '               "@codPais =  " & Request.Cookies("CKSMFACTURA")("codPais") & " ," & _
    '               "@codigoProducto = '" & Session("Codigo") & "' ," & _
    '               "@codUndMedida =  " & Me.ddlUnidadMed.SelectedValue & " ," & _
    '               "@descripcion =  '" & Session("Descrip") & "' ," & _
    '               "@descripImpr =  '" & Me.TextDecImpri.Text.Trim & "' ," & _
    '               "@numProducto =  '" & Session("Numero") & "' ," & _
    '               "@activo = '" & IIf(Me.CheckAt.Checked = True, 1, 0) & "' ," & _
    '               "@ExcentoImp = '" & IIf(Me.CheckImtos.Checked = True, 1, 0) & "' ," & _
    '               "@codEmpresa =  " & Request.Cookies("CKSMFACTURA")("CodEmpresa") & " ," & _
    '               "@codusuario =  " & Request.Cookies("CKSMFACTURA")("cod_usuario") & " ," & _
    '               "@codusuarioUlt =  " & Request.Cookies("CKSMFACTURA")("cod_usuario") & " ," & _
    '               "@BUSQUEDAD = '0'  "


    '        Dim cmd As New OleDb.OleDbCommand(sql, dbCon)
    '        cmd.ExecuteNonQuery()

    '        Load_GridView()

    '        MessegeText = "success_messege('El registro ha sido guardado de forma correcta.');"

    '        ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)

    '    Catch ex As Exception
    '        MessegeText = "alertify.error('Ha ocurrido un error al intentar guardar los datos. Si el problema persiste, contacte con el administrador.');"
    '        ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)

    '    Finally
    '        If dbCon.State = ConnectionState.Open Then
    '            dbCon.Close()
    '        End If

    '    End Try
    'End Sub

    Private Sub Eliminar()
        Dim MessegeText As String = String.Empty
        Dim dbCon As New System.Data.OleDb.OleDbConnection(conn.conn)
        Try
            If dbCon.State = ConnectionState.Closed Then
                dbCon.Open()
            End If

            Dim sql As String = "SET DATEFORMAT DMY " & vbCrLf
            sql &= "EXEC Cat_ProductosXPais @opcion=2," & _
                  "@codPais =  " & vPais & " ," & _
                  "@codigoProducto =  " & Me.hdfCodigo.Value & " ," & _
                  "@codUndMedida =  0 ," & _
                  "@descripcion =  '" & Session("Descrip") & "' ," & _
                  "@descripImpr =  '' ," & _
                  "@numProducto =  '" & Session("numero") & "' ," & _
                  "@activo = 0 ," & _
                  "@ExcentoImp = 0 ," & _
                  "@codEmpresa =  " & Request.Cookies("CKSMFACTURA")("CodEmpresa") & " ," & _
                  "@codusuario =   " & Request.Cookies("CKSMFACTURA")("cod_usuario") & " ," & _
                  "@codusuarioUlt =  " & Request.Cookies("CKSMFACTURA")("cod_usuario") & " ," & _
                  "@BUSQUEDAD = '0'  "


          
            Dim cmd As New OleDb.OleDbCommand(sql, dbCon)
            cmd.ExecuteNonQuery()

            Load_GridView()

            MessegeText = "success_messege(El registro ha sido correctamente eliminado.);"

            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)

        Catch ex As Exception
            MessegeText = "alertify.error('Ha ocurrido un error al intentar eliminar los datos. Si el problema persiste, contacte con el administrador. " & Replace(ex.Message, "'", "") & "');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)

        Finally
            If dbCon.State = ConnectionState.Open Then
                dbCon.Close()
            End If

        End Try
    End Sub

    Private Sub Actualizar(Codigo As String, Descripcion As String, deImpre As String, Numero As Integer, VUnidad As Integer, Impt As Boolean, Vac As Boolean)
        Dim MessegeText As String = String.Empty
        Dim dbCon As New System.Data.OleDb.OleDbConnection(conn.conn)
        Try
            If dbCon.State = ConnectionState.Closed Then
                dbCon.Open()
            End If

            Dim sql As String = "SET DATEFORMAT DMY " & vbCrLf
            sql &= "EXEC Cat_ProductosXPais @opcion=1," & _
                   "@codPais =  " & Request.Cookies("CKSMFACTURA")("codPais") & " ," & _
                   "@codigoProducto =  '" & Codigo & "' ," & _
                   "@codUndMedida =  " & VUnidad & " ," & _
                   "@descripcion =  '" & Descripcion & "' ," & _
                   "@descripImpr =  '" & deImpre & "' ," & _
                   "@numProducto =  " & Numero & " ," & _
                   "@activo = '" & Vac & "' ," & _
                   "@ExcentoImp = '" & Impt & "' ," & _
                   "@codEmpresa =  " & Request.Cookies("CKSMFACTURA")("CodEmpresa") & " ," & _
                   "@codusuario =   " & Request.Cookies("CKSMFACTURA")("cod_usuario") & " ," & _
                   "@codusuarioUlt =  " & Request.Cookies("CKSMFACTURA")("cod_usuario") & " ," & _
                   "@BUSQUEDAD = '0'  "

            Dim cmd As New OleDb.OleDbCommand(sql, dbCon)
            cmd.ExecuteNonQuery()

            MessegeText = "success_messege('El registro ha sido actualizado de forma correcta.');"

            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)

        Catch ex As Exception
            MessegeText = "alertify.error('Ha ocurrido un error al intentar actualizar los datos. Si el problema persiste, contacte con el administrador. " & Replace(ex.Message, "'", "") & "');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)

        Finally
            If dbCon.State = ConnectionState.Open Then
                dbCon.Close()
            End If

        End Try
    End Sub
#End Region
#Region "EXPORTAR DATOS A EXCELL"

    Protected Sub btnExportar_Click(sender As Object, e As EventArgs) Handles btnExportar.Click
        Try
            Response.Clear()
            Response.Buffer = True

            Dim name_doc = "Catalogo_ProductosPais" & Date.Now.Year & Date.Now.Month & Date.Now.Day

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
            MyHTML &= "<h2>Catalogo de Producto por pais <br/> al dia " & Date.Now.Day & "/" & Date.Now.Month & "/" & Date.Now.Year & "</h2>"

            If Not dtTabla Is Nothing Then
                MyHTML &= "<table cellspacing=""0"" cellpadding=""0"">" & _
                          "<tbody>" & _
                          "<tr>" & _
                          "<th></th>" & _
                          "<th>Codigo</th>" & _
                          "<th>Descripcion Maestro</th>" & _
                          "<th>Descripcion Imprimir</th>" & _
                          "<th>Pais</th>" & _
                          "<th>UnidadMedida</th>" & _
                          "<th>excento_imp</th>" & _
                          "<th>activo</th>" & _
                          "</tr>"
                Dim clase As String = String.Empty

                For i As Integer = 0 To dtTabla.Rows.Count - 1
                    MyHTML &= "<tr" & clase & ">" & _
                              "<td class=""first"">" & i + 1 & "</td>" & _
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("codigo").ToString.Trim & "</td>" & _
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("producto").ToString.Trim & "</td>" & _
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("DescripImpresion").ToString.Trim & "</td>" & _
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("Pais").ToString.Trim & "</td>" & _
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("UnidadMedida").ToString.Trim & "</td>" & _
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("excento_imp").ToString.Trim & "</td>" & _
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("activo").ToString.Trim & "</td>" & _
                              "<td></td>" & _
                              "</tr>"

                    If clase = String.Empty Then
                        clase = " class=""alt"""
                    Else
                        clase = String.Empty
                    End If

                Next i

                MyHTML &= "</tbody>" & _
                          "<table>"
            End If

            MyHTML &= "</div>"
            MyHTML &= "</body>"
            MyHTML &= "</html>"
            Response.Write(MyHTML)

            Response.End()


        Catch ex As Exception
            'Me.ltMensaje.Text = conn.pmsgBox("Ocurrio un error al intentar exportar la tabla. " & ex.Message, "error")
        End Try
    End Sub

#End Region


End Class
