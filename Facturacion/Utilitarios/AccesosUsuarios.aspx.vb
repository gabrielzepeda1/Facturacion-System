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
Imports AjaxControlToolkit

Partial Class Utilitarios_AccesosUsuarios
    Inherits System.Web.UI.Page

    Dim conn As New FACTURACION_CLASS.seguridad
    Dim DataBase As New FACTURACION_CLASS.database
    Dim BUSQUEDAD As String

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
            Load_GridView()
        End If
    End Sub

#Region "PROCESOS Y EVENTOS DEL GRIDVIEW"
    Protected Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click

        BUSQUEDAD = txtBuscar.Text.ToString().Trim()
        Load_GridView()

    End Sub

    Private Sub Load_GridView()
        Try
            Dim sql As String = "EXEC AccesosUsuarios @opcion=3," &
                   "@codusuario =  NULL ," &
                   "@codPais =  0  ," &
                   "@codEmpresa =  0  ," &
                   "@codPuesto = 0  ," &
                   "@creaMov = NULL  ," &
                   "@BUSQUEDAD = '" & BUSQUEDAD & "' "

            Dim ds As DataSet
            ds = DataBase.GetDataSet(sql)

            dtTabla = ds.Tables(0)

            GridViewOne.DataSource = dtTabla.DefaultView
            GridViewOne.DataBind()
            ds.Dispose()

        Catch ex As Exception
            ltMensajeGrid.Text &= conn.pmsgBox("Ocurrió un error al intentar cargar el listado de rubros en la tabla." & ex.Message, "error")
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
            ltMensajeGrid.Text &= conn.pmsgBox("Ocurrió un error al disparar el evento DataBound. " & ex.Message, "error")
        End Try
    End Sub

    Protected Sub GridViewOne_PageIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridViewOne.PageIndexChanged
        Try

            GridViewOne.SelectedIndex = -1
            hdfCodigo.Value = String.Empty
            hdfPais.Value = String.Empty
            hdfEmpresa.Value = String.Empty
            hdfPuesto.Value = String.Empty

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

        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.pmsgBox("Ocurrió un error al disparar el evento PageIndexChanging." & ex.Message, "error")

        End Try
    End Sub

    Protected Sub GridViewOne_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridViewOne.RowDataBound

        'If e.Row.RowType = DataControlRowType.DataRow Then
        '    'e.Row.Attributes.Add("onmouseover", "" & Page.ClientScript.GetPostBackClientHyperlink(Me.GridViewOne, "Select$" + e.Row.RowIndex.ToString) & ";")
        '    e.Row.Attributes.Add("OnClick", "" & Page.ClientScript.GetPostBackClientHyperlink(Me.GridViewOne, "Select$" + e.Row.RowIndex.ToString) & ";")

        '    '  e.Row.Attributes("OnClick") = Page.ClientScript.GetPostBackClientHyperlink(Me.GridViewOne, "Select$" + e.Row.RowIndex.ToString)

        '    ' e.Row.Attributes.Add("OnMouseOver", "Resaltar_On(this);")

        '    'e.Row.Attributes.Add("OnMouseOut", "Resaltar_Off(this);")


        'End If
    End Sub

    Protected Sub GridViewOne_SelectedIndexChanged(sender As Object, e As EventArgs) Handles GridViewOne.SelectedIndexChanged

        ' Se obtiene la fila seleccionada del gridview
        'Dim row As GridViewRow = GridViewOne.SelectedRow
        'Dim Vusuario As Integer
        'Dim vPais As Integer
        'Vusuario = Convert.ToString(GridViewOne.DataKeys(row.RowIndex).Values("codigo"))
        'vPais = Convert.ToInt32(GridViewOne.DataKeys(row.RowIndex).Values("codigo_pais"))


    End Sub

    Protected Sub GridViewOne_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles GridViewOne.RowDeleting
        Try

            hdfCodigo.Value = Convert.ToString(GridViewOne.DataKeys(e.RowIndex).Values("codigo"))
            hdfPais.Value = Convert.ToInt32(GridViewOne.DataKeys(e.RowIndex).Values("codigo_pais"))
            hdfEmpresa.Value = Convert.ToInt32(GridViewOne.DataKeys(e.RowIndex).Values("cod_Empresa"))
            hdfPuesto.Value = Convert.ToInt32(GridViewOne.DataKeys(e.RowIndex).Values("cod_puesto"))
            Eliminar()


        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.pmsgBox("Ocurrió un error al disparar el evento RowDeleting. " & ex.Message, "error")

        End Try
    End Sub

    Protected Sub GridViewOne_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles GridViewOne.RowCancelingEdit
        Me.GridViewOne.EditIndex = -1
        Load_GridView()
    End Sub

    'Protected Sub GridViewOne_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles GridViewOne.RowEditing
    '    Try
    '        Me.GridViewOne.EditIndex = e.NewEditIndex

    '        Load_GridView()


    '        Dim id As String = Me.GridViewOne.DataKeys(e.NewEditIndex).Value

    '        'Dim Sql As String
    '        'Dim vsigla As String

    '        'Sql = " SET DATEFORMAT DMY " + "SELECT sigla FROM Productos_Maestro pm WHERE RTRIM(ltrim(pm.cod_producto))= '" & id & "' "
    '        'Dim fr As System.Data.OleDb.OleDbDataReader = DataBase.GetDataReader(Sql)
    '        'If fr.Read() Then
    '        '    vsigla = fr.Item("sigla").ToString()
    '        'End If




    '        Dim dataSetSigla As New DataSet
    '        dataSetSigla = DataBase.GetDataSet("SELECT sigla FROM Siglas  ")
    '        dtTabla = dataSetSigla.Tables(0)
    '        Dim comboSigla As DropDownList = TryCast(GridViewOne.Rows(e.NewEditIndex).FindControl("ddlSigla"), DropDownList)
    '        If comboSigla IsNot Nothing Then
    '            comboSigla.DataSource = dataSetSigla
    '            comboSigla.DataTextField = "sigla"
    '            comboSigla.DataValueField = "sigla"
    '            'comboSigla.SelectedValue = vsigla
    '            comboSigla.DataBind()
    '        End If

    '        Dim vorigen As Integer
    '        Sql = " SET DATEFORMAT DMY " + "SELECT cod_origen FROM Productos_Maestro pm WHERE RTRIM(ltrim(pm.cod_producto))= '" & id & "' "
    '        fr = DataBase.GetDataReader(Sql)
    '        If fr.Read() Then
    '            vorigen = fr.Item("cod_origen").ToString()
    '        End If

    '        Dim dataSet As New DataSet
    '        dataSet = DataBase.GetDataSet("SELECT cod_origen as codOrigen,Descripcion AS DesOrigen FROM  Origenes_productos  ")
    '        dtTabla = dataSet.Tables(0)
    '        Dim combo As DropDownList = TryCast(GridViewOne.Rows(e.NewEditIndex).FindControl("ddlOrigen"), DropDownList)
    '        If combo IsNot Nothing Then
    '            combo.DataSource = dataSet
    '            combo.DataTextField = "DesOrigen"
    '            combo.DataValueField = "codOrigen"
    '            combo.SelectedValue = vorigen
    '            combo.DataBind()
    '        End If

    '        Dim vcalidad As Integer
    '        Sql = " SET DATEFORMAT DMY " + "SELECT cod_calidad FROM Productos_Maestro pm WHERE RTRIM(ltrim(pm.cod_producto))= '" & id & "' "
    '        fr = DataBase.GetDataReader(Sql)
    '        If fr.Read() Then
    '            vcalidad = fr.Item("cod_calidad").ToString()
    '        End If

    '        Dim dataSetCalidad As New DataSet
    '        dataSetCalidad = DataBase.GetDataSet("SELECT cod_calidad as CodCalidad,descripcion as calidad FROM  Calidades_productos  ")
    '        dtTabla = dataSetCalidad.Tables(0)
    '        Dim comboCalidad As DropDownList = TryCast(GridViewOne.Rows(e.NewEditIndex).FindControl("ddlCalidad"), DropDownList)
    '        If comboCalidad IsNot Nothing Then
    '            comboCalidad.DataSource = dataSetCalidad
    '            comboCalidad.DataTextField = "calidad"
    '            comboCalidad.DataValueField = "CodCalidad"
    '            comboCalidad.SelectedValue = vcalidad
    '            comboCalidad.DataBind()
    '        End If

    '        Dim vPresentacion As Integer
    '        Sql = " SET DATEFORMAT DMY " + "SELECT cod_presentacion FROM Productos_Maestro pm WHERE RTRIM(ltrim(pm.cod_producto))= '" & id & "' "
    '        fr = DataBase.GetDataReader(Sql)
    '        If fr.Read() Then
    '            vPresentacion = fr.Item("cod_presentacion").ToString()
    '        End If
    '        Dim dataSetPresentacion As New DataSet
    '        dataSetPresentacion = DataBase.GetDataSet("SELECT cod_presentacion as CodPresentacion,descripcion as presentacion FROM  Presentaciones_productos  ")
    '        dtTabla = dataSetPresentacion.Tables(0)
    '        Dim comboPresent As DropDownList = TryCast(GridViewOne.Rows(e.NewEditIndex).FindControl("ddlPresentacion"), DropDownList)
    '        If comboPresent IsNot Nothing Then
    '            comboPresent.DataSource = dataSetPresentacion
    '            comboPresent.DataTextField = "presentacion"
    '            comboPresent.DataValueField = "CodPresentacion"
    '            comboPresent.SelectedValue = vPresentacion
    '            comboPresent.DataBind()
    '        End If


    '        Dim vFamilia As Integer
    '        Sql = " SET DATEFORMAT DMY " + "SELECT cod_familia FROM Productos_Maestro pm WHERE RTRIM(ltrim(pm.cod_producto))= '" & id & "' "
    '        fr = DataBase.GetDataReader(Sql)
    '        If fr.Read() Then
    '            vFamilia = fr.Item("cod_familia").ToString()
    '        End If

    '        Dim dataSetFamilia As New DataSet
    '        dataSetFamilia = DataBase.GetDataSet("SELECT cod_familia as CodFamilia,descripcion as familia FROM  Familias_productos  ")
    '        dtTabla = dataSetFamilia.Tables(0)
    '        Dim comboFamilia As DropDownList = TryCast(GridViewOne.Rows(e.NewEditIndex).FindControl("ddlFamilia"), DropDownList)
    '        If comboFamilia IsNot Nothing Then
    '            comboFamilia.DataSource = dataSetFamilia
    '            comboFamilia.DataTextField = "familia"
    '            comboFamilia.DataValueField = "CodFamilia"
    '            comboFamilia.SelectedValue = vFamilia
    '            comboFamilia.DataBind()
    '        End If

    '        'Dim comboP As DropDownList = TryCast(GridViewOne.Rows(e.RowIndex).FindControl("ddlOrigen"), DropDownList)
    '        'Dim origen As Integer = Convert.ToInt32(combo.SelectedValue)



    '    Catch ex As Exception
    '        Me.ltMensajeGrid.Text &= conn.pmsgBox("Ocurrió un error al disparar el evento RowEditing SelectedIndexChanged. " & ex.Message, "error")

    '    End Try
    'End Sub

    'Protected Sub GridViewOne_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles GridViewOne.RowUpdating
    '    'ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "smf_Script", " alert(""Esto es un mensaje"");", True)

    '    Dim Codigo As String = e.Keys("codigo").ToString
    '    Dim Descripcion As String = e.NewValues("producto").ToString
    '    Dim Numero As String = e.NewValues("num_producto").ToString
    '    Dim prodSm As Integer
    '    If e.NewValues("produccion_sm").ToString = "SI" Then
    '        prodSm = 1
    '    Else
    '        prodSm = 0
    '    End If

    '    Dim combo As DropDownList = TryCast(GridViewOne.Rows(e.RowIndex).FindControl("ddlOrigen"), DropDownList)
    '    Dim origen As Integer = Convert.ToInt32(combo.SelectedValue)

    '    combo = TryCast(GridViewOne.Rows(e.RowIndex).FindControl("ddlCalidad"), DropDownList)
    '    Dim calidad As String = Convert.ToInt32(combo.SelectedValue)

    '    combo = TryCast(GridViewOne.Rows(e.RowIndex).FindControl("ddlPresentacion"), DropDownList)
    '    Dim presentacion As String = Convert.ToInt32(combo.SelectedValue)

    '    combo = TryCast(GridViewOne.Rows(e.RowIndex).FindControl("ddlFamilia"), DropDownList)
    '    Dim familia As String = Convert.ToInt32(combo.SelectedValue)

    '    combo = TryCast(GridViewOne.Rows(e.RowIndex).FindControl("ddlSigla"), DropDownList)
    '    Dim Sigla As String = combo.SelectedValue



    '    Actualizar(Codigo, Descripcion, Numero, Sigla, origen, calidad, presentacion, familia, prodSm)
    '    Me.GridViewOne.EditIndex = -1
    '    Load_GridView()
    '''''''''''''''''''''''''''''''''''''''''''''''''



    'Dim id As Integer = Convert.ToInt32(GridView1.DataKeys(e.RowIndex).Value)
    'Dim combo As DropDownList = TryCast(GridView1.Rows(e.RowIndex).FindControl("ddlPaises"), DropDownList)
    'Dim pais As Integer = Convert.ToInt32(combo.SelectedValue)
    'Dim text As TextBox = TryCast(GridView1.Rows(e.RowIndex).Cells(1).Controls(0), TextBox)
    'Dim nombre As String = text.Text

    'DataAccess.UpdateUsuario(id, nombre, pais)

    'GridView1.EditIndex = -1
    'BindData()


    'End Sub
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

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click

        Dim messegeText As String

        hdfCodigo.Value = ddlUsuario.SelectedItem.Value
        hdfPais.Value = ddlPais.SelectedItem.Value
        hdfEmpresa.Value = ddlEmpresa.SelectedItem.Value
        hdfPuesto.Value = ddlPuesto.SelectedItem.Value

        If hdfCodigo.Value = 0 Then
            messegeText = "alertify.alert('El proceso no puede continuar. Debe escojer usuario');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", messegeText, True)
            Exit Sub
        End If

        If hdfPais.Value = 0 Then
            messegeText = "alertify.alert('El proceso no puede continuar. Debe escojer Pais');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", messegeText, True)
            Exit Sub
        End If

        If hdfEmpresa.Value = 0 Then
            messegeText = "alertify.alert('El proceso no puede continuar. Debe escojer Empresa');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", messegeText, True)
            Exit Sub
        End If

        If hdfPuesto.Value = 0 Then
            messegeText = "alertify.alert('El proceso no puede continuar. Debe escojer puesto.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", messegeText, True)
            Exit Sub
        End If

        Guardar()

        hdfPais.Value = String.Empty
        hdfEmpresa.Value = String.Empty
        hdfPuesto.Value = String.Empty

        ddlUsuario.SelectedIndex = -1
        ddlPais.SelectedIndex = -1
        ddlEmpresa.SelectedIndex = 0
        ddlPuesto.SelectedIndex = 0

    End Sub

    Private Sub Guardar()
        Dim messegeText As String
        Dim dbCon As New OleDb.OleDbConnection(conn.conn)

        Try
            If dbCon.State = ConnectionState.Closed Then
                dbCon.Open()
            End If

            Dim sql As String = "SET DATEFORMAT DMY " & vbCrLf
            sql &= "EXEC AccesosUsuarios @opcion=1, " &
                   "@codusuario =  " & ddlUsuario.SelectedValue & " ," &
                   "@codPais =  " & ddlPais.SelectedValue & " ," &
                   "@codEmpresa =  '" & ddlEmpresa.SelectedValue & "' ," &
                   "@codPuesto =  " & ddlPuesto.SelectedValue & " ," &
                   "@creaMov = NULL, " &
                   "@BUSQUEDAD = '0'  "


            Dim cmd As New OleDb.OleDbCommand(sql, dbCon)
            cmd.ExecuteNonQuery()

            Load_GridView()


            messegeText = "alertify.success('El registro ha sido guardado de forma correcta.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", messegeText, True)

        Catch ex As Exception
            messegeText = "alertify.error('Ha ocurrido un error al intentar guardar los datos. Si el problema persiste, contacte con el administrador.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", messegeText, True)

        Finally
            If dbCon.State = ConnectionState.Open Then
                dbCon.Close()
            End If

        End Try
    End Sub

    Private Function IfExists() As Boolean


        Dim dbCon As New OleDb.OleDbConnection(conn.conn)
        Dim sql As String = "SET DATEFORMAT DMY " & vbCrLf
        sql &= "EXEC AccesosUsuarios @opcion=4, " &
               "@codusuario =  " & ddlUsuario.SelectedValue & " ," &
               "@codPais =  " & ddlPais.SelectedValue & " ," &
               "@codEmpresa =  '" & ddlEmpresa.SelectedValue & "' ," &
               "@codPuesto =  " & ddlPuesto.SelectedValue & " ," &
               "@creaMov = NULL, " &
               "@BUSQUEDAD = '0'  "

        Dim cmd As New OleDb.OleDbCommand(sql, dbCon)
        Dim dr As OleDb.OleDbDataReader

        Try
            If dbCon.State = ConnectionState.Closed Then
                dbCon.Open()
            End If

            dr = cmd.ExecuteReader()

            If dr.Read() Then
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            Return False

        Finally
            If dbCon.State = ConnectionState.Open Then
                dbCon.Close()
            End If

        End Try



    End Function

    Private Sub Eliminar()
        Dim messegeText As String
        Dim dbCon As New OleDb.OleDbConnection(conn.conn)
        Try
            If dbCon.State = ConnectionState.Closed Then
                dbCon.Open()
            End If

            Dim sql As String = "SET DATEFORMAT DMY " & vbCrLf
            sql &= "EXEC AccesosUsuarios @opcion=2, " &
                    "@codusuario =  " & hdfCodigo.Value & " ," &
                    "@codPais =  " & hdfPais.Value & " ," &
                    "@codEmpresa =  " & hdfEmpresa.Value & " ," &
                    "@codPuesto =  " & hdfPuesto.Value & " ," &
                    "@creaMov =  NULL," &
                    "@BUSQUEDAD = '0' "

            Dim cmd As New OleDb.OleDbCommand(sql, dbCon)
            cmd.ExecuteNonQuery()

            Load_GridView()

            messegeText = "alertify.warning('ELIMINADO');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", messegeText, True)

        Catch ex As InvalidOperationException
            messegeText = "alertify.error('Ha ocurrido un error al intentar eliminar los datos. Si el problema persiste, contacte con el administrador. " & Replace(ex.Message, "'", "") & "');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", messegeText, True)

        Finally
            If dbCon.State = ConnectionState.Open Then
                dbCon.Close()
            End If

        End Try
    End Sub

    Private Sub Actualizar(Codigo As String, Descripcio As String, Numero As Integer, Sigla As String, origen As Integer, calidad As Integer, presentacion As Integer, familia As Integer, prodSm As Integer)
        Dim messegeText As String
        Dim dbCon As New OleDb.OleDbConnection(conn.conn)
        Try
            If dbCon.State = ConnectionState.Closed Then
                dbCon.Open()
            End If

            Dim sql As String = "SET DATEFORMAT DMY " & vbCrLf
            sql &= "EXEC AccesosUsuarios @opcion=1," & _
                    "@codusuario =  " & Me.ddlUsuario.SelectedValue & " ," & _
                    "@codPais =  " & Me.ddlPais.SelectedValue & " ," & _
                    "@codEmpresa =  '" & Me.ddlEmpresa.SelectedValue & "' ," & _
                    "@codPuesto =  " & Me.ddlPuesto.SelectedValue & " ," & _
                    "@BUSQUEDAD = '0'  "

            Dim cmd As New OleDb.OleDbCommand(sql, dbCon)
            cmd.ExecuteNonQuery()

            Load_GridView()
            messegeText = "success_messege('El registro ha sido actualizado de forma correcta.');"

            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", messegeText, True)

        Catch ex As Exception
            messegeText = "alertify.error('Ha ocurrido un error al intentar actualizar los datos. Si el problema persiste, contacte con el administrador. " & Replace(ex.Message, "'", "") & "');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", messegeText, True)

        Finally
            If dbCon.State = ConnectionState.Open Then
                dbCon.Close()
            End If

        End Try
    End Sub
#End Region

#Region "EXPORTAR DATOS A EXCEL"

    Protected Sub btnExportar_Click(sender As Object, e As EventArgs) Handles btnExportar.Click
        Try
            Response.Clear()
            Response.Buffer = True

            Dim name_doc = "catalogo_Usuarios/Pais/Empresa/Puestos" & Date.Now.Year & Date.Now.Month & Date.Now.Day

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
            MyHTML &= "<h2>Catalogo_Usuarios/Pais/Empresa/Puestos <br/> al dia " & Date.Now.Day & "/" & Date.Now.Month & "/" & Date.Now.Year & "</h2>"

            If Not dtTabla Is Nothing Then
                MyHTML &= "<table cellspacing=""0"" cellpadding=""0"">" & _
                          "<tbody>" & _
                          "<tr>" & _
                          "<th></th>" & _
                          "<th>Usuario</th>" & _
                          "<th>Pais</th>" & _
                          "<th>Empresa</th>" & _
                          "<th>Puesto</th>" & _
                          "</tr>"

                Dim clase As String = String.Empty

                For i As Integer = 0 To dtTabla.Rows.Count - 1
                    MyHTML &= "<tr" & clase & ">" & _
                              "<td class=""first"">" & i + 1 & "</td>" & _
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("Nombre").ToString.Trim & "</td>" & _
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("Pais").ToString.Trim & "</td>" & _
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("Empresa").ToString.Trim & "</td>" & _
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("Puesto").ToString.Trim & "</td>" & _
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
            Me.ltMensaje.Text = conn.pmsgBox("Ocurrio un error al intentar exportar la tabla. " & ex.Message, "error")
        End Try
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        GridViewOne.SelectedIndex = -1
        hdfCodigo.Value = String.Empty
        hdfPais.Value = String.Empty
        hdfEmpresa.Value = String.Empty
        hdfPuesto.Value = String.Empty

        ddlUsuario.SelectedIndex = 0
        ddlPais.SelectedIndex = 0
        ddlEmpresa.SelectedIndex = 0
        ddlPuesto.SelectedIndex = 0

    End Sub

#End Region

End Class
