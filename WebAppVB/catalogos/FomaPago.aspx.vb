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
Imports FACTURACION_CLASS
Partial Class catalogos_FomaPago
    Inherits System.Web.UI.Page

    Dim conn As New seguridad
    Dim DataBase As New database
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

            Puesto_Name = "Industrial Comercial San Martin"

            Me.txtDescripcion.Attributes.Add("placeholder", "Digite Descripción")
            Me.txtDescripcion.Attributes.Add("requerid", "requerid")
            Me.ddlPais.Attributes.Add("placeholder", "Digite Pais")
            Me.ddlPais.Attributes.Add("requerid", "requerid")

            Load_GridView()

        End If
    End Sub


#Region "PROCESOS Y EVENTOS DEL GRIDVIEW"
    Private Sub Load_GridView()
        Try
            Dim vCPais As String = String.Empty
            vCPais = Context.Request.Cookies("CKSMFACTURA")("codPais")
            Dim SQL As String = String.Empty
            SQL &= "EXEC Cat_FormaPago @opcion=3," &
                  "@codigoPais = " & vCPais & " ," &
                  "@codigoEmpresa =  NULL  ," &
                  "@codigoPuesto =  NULL  ," &
                  "@codigoFormaPago =  NULL  ," &
                  "@descripcion =  NULL ," &
                  "@BUSQUEDAD = '" & BUSQUEDAD & "', " &
                  "@PorDefecto = 1 "

            Dim ds As DataSet
            ds = DataBase.GetDataSet(SQL)

            dtTabla = ds.Tables(0)

            Me.GridViewOne.DataSource = dtTabla.DefaultView
            Me.GridViewOne.DataBind()
            ds.Dispose()

            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "smf_Script", "responsive_grid();", True)

        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.PmsgBox("Ocurrió un error al intentar cargar el listado de rubros en la tabla." & ex.Message, "error")

        End Try
    End Sub

    Protected Sub GridViewOne_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridViewOne.DataBound
        Try
            If GridViewOne.Rows.Count > 0 Then
                Dim pagerRow As GridViewRow = GridViewOne.BottomPagerRow
                Dim pageLabel As Label = CType(pagerRow.Cells(0).FindControl("CurrentPageLabel"), Label)
                If Not pageLabel Is Nothing Then
                    Dim currentPage As Integer = GridViewOne.PageIndex + 1
                    pageLabel.Text = "&nbsp;&nbsp; Pagina " & currentPage.ToString() &
                        " de " & GridViewOne.PageCount.ToString()
                End If
            End If
        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.PmsgBox("Ocurrió un error al disparar el evento DataBound. " & ex.Message, "error")

        End Try
    End Sub

    Protected Sub GridViewOne_PageIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridViewOne.PageIndexChanged
        Try

            Me.GridViewOne.SelectedIndex = -1
            Me.hdfCodigo.Value = String.Empty

            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "smf_Script", "responsive_grid();", True)

        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.PmsgBox("Ocurrió un error al disparar el evento PageIndexChanged." & ex.Message, "error")

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
            Me.ltMensajeGrid.Text &= conn.PmsgBox("Ocurrió un error al disparar el evento PageIndexChanging." & ex.Message, "error")

        End Try

    End Sub

    Protected Sub GridViewOne_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridViewOne.RowDataBound
        Try
            ''carga el campo llave es el 2dopaso DataKeyName
            'If e.Row.RowType = DataControlRowType.DataRow Then
            '    'e.Row.Attributes.Add("OnMouseOver", "On(this);")
            '    'e.Row.Attributes.Add("OnMouseOut", "Off(this);")
            '    e.Row.Attributes.Add("OnClick", "" & Page.ClientScript.GetPostBackClientHyperlink(Me.GridViewOne, "Select$" + e.Row.RowIndex.ToString) & ";")
            'End If

        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.PmsgBox("Ocurrió un error al disparar el evento RowDataBound. " & ex.Message, "error")

        End Try
    End Sub

    Protected Sub GridViewOne_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles GridViewOne.RowDeleting
        Try
            'pasar la información del Gridview hacia otro control, en este caso el control HiddenField,
            Me.hdfCodigo.Value = Me.GridViewOne.DataKeys(e.RowIndex).Value
            'vPais = GridViewOne.DataKeys(e.RowIndex).Item(1).ToString()
            Eliminar()
            Dim MessegeText As String

            MessegeText = "alertify.success(El registro ha sido correctamente eliminado.);"

            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Message", MessegeText, True)

            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "smf_Script", "responsive_grid();", True)

        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.PmsgBox("Ocurrió un error al disparar el evento RowDeleting. " & ex.Message, "error")

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
            Dim Sql As String
            Dim vPais As Integer
            Dim vCodempresa As Integer
            Dim vCodpuesto As Integer
            Dim vPorDefecto As Integer
            ''" SET DATEFORMAT DMY " + 
            Sql = "SELECT  Cod_pais,cod_empresa,cod_puesto,PorDefecto FROM Forma_pago WHERE cod_FormaPago= '" & id & "' "
            Dim fr As System.Data.OleDb.OleDbDataReader = DataBase.GetDataReader(Sql)
            If fr.Read() Then
                vPais = fr.Item("cod_pais").ToString()
                vCodempresa = fr.Item("cod_empresa").ToString()
                vCodpuesto = fr.Item("cod_puesto").ToString()

                vPorDefecto = IIf(fr.Item("PorDefecto").ToString() = "Si", True, False)
            End If
            fr.Close()

            Dim ckeckPorDe As CheckBox = TryCast(GridViewOne.Rows(e.NewEditIndex).FindControl("CheckDefault"), CheckBox)
            If ckeckPorDe IsNot Nothing Then
                ckeckPorDe.Checked = vPorDefecto
            End If


            Dim dataSetCodpuesto As New DataSet
            dataSetCodpuesto = DataBase.GetDataSet("SELECT cod_puesto as codPuesto,descripcion as Puesto FROM puestos  ")
            dtTabla = dataSetCodpuesto.Tables(0)
            Dim comboCodpuesto As DropDownList = TryCast(GridViewOne.Rows(e.NewEditIndex).FindControl("ddlPuesto"), DropDownList)
            If comboCodpuesto IsNot Nothing Then
                comboCodpuesto.DataSource = dataSetCodpuesto
                comboCodpuesto.DataTextField = "Puesto"
                comboCodpuesto.DataValueField = "codPuesto"
                comboCodpuesto.SelectedValue = vCodpuesto
                comboCodpuesto.DataBind()
            End If


            Dim dataSetEmpresa As New DataSet
            dataSetEmpresa = DataBase.GetDataSet("SELECT cod_Empresa as codEmpresa,descripcion as Empresa FROM Empresas  ")
            dtTabla = dataSetEmpresa.Tables(0)
            Dim comboEmpresa As DropDownList = TryCast(GridViewOne.Rows(e.NewEditIndex).FindControl("ddlEmpresa"), DropDownList)
            If comboEmpresa IsNot Nothing Then
                comboEmpresa.DataSource = dataSetEmpresa
                comboEmpresa.DataTextField = "Empresa"
                comboEmpresa.DataValueField = "codEmpresa"
                comboEmpresa.SelectedValue = vCodempresa
                comboEmpresa.DataBind()
            End If


            Dim dataSetPais As New DataSet
            dataSetPais = DataBase.GetDataSet("SELECT cod_pais as codPais,descripcion as DesPais FROM Paises  ")
            dtTabla = dataSetPais.Tables(0)
            Dim comboPais As DropDownList = TryCast(GridViewOne.Rows(e.NewEditIndex).FindControl("ddlPais"), DropDownList)
            If comboPais IsNot Nothing Then
                comboPais.DataSource = dataSetPais
                comboPais.DataTextField = "DesPais"
                comboPais.DataValueField = "codPais"
                comboPais.SelectedValue = vPais
                comboPais.DataBind()
            End If


        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.PmsgBox("Ocurrió un error al disparar el evento SelectedIndexChanged. " & ex.Message, "error")

        End Try
    End Sub

    Protected Sub GridViewOne_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles GridViewOne.RowUpdating
        'ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "smf_Script", " alert(""Esto es un mensaje"");", True)

        Dim codigo As String = e.Keys("codigo").ToString
        Dim Descripcion As String = e.NewValues("Descripcion").ToString
        'Dim CodigoPais As String = e.NewValues("codPais").ToString
        'Dim CodigoEmpresa As String = e.NewValues("codEmpresa").ToString
        'Dim codPuesto As String = e.NewValues("codPuesto").ToString
        'Dim codBanco As String = e.NewValues("codBanco").ToString
        'Dim codMoneda As String = e.NewValues("codMoneda").ToString

        Dim combo As DropDownList
        combo = TryCast(GridViewOne.Rows(e.RowIndex).FindControl("ddlPais"), DropDownList)
        Dim CodigoPais As Integer = Convert.ToInt32(combo.SelectedValue)

        combo = TryCast(GridViewOne.Rows(e.RowIndex).FindControl("ddlEmpresa"), DropDownList)
        Dim CodigoEmpresa As Integer = Convert.ToInt32(combo.SelectedValue)

        combo = TryCast(GridViewOne.Rows(e.RowIndex).FindControl("ddlPuesto"), DropDownList)
        Dim codPuesto As Integer = Convert.ToInt32(combo.SelectedValue)

        Dim pordef As Integer
        Dim cb As CheckBox
        cb = TryCast(GridViewOne.Rows(e.RowIndex).FindControl("CheckDefault"), CheckBox)
        pordef = cb.Checked


        Actualizar(codigo, Descripcion, CodigoPais, CodigoEmpresa, codPuesto, pordef)
        Me.GridViewOne.EditIndex = -1
        Load_GridView()
    End Sub
#End Region
#Region "Editar en formulario popup"

    Protected Sub GridViewOne_SelectedIndexChanged(sender As Object, e As EventArgs) Handles GridViewOne.SelectedIndexChanged
        Me.hdfCodigo.Value = Me.GridViewOne.SelectedValue.ToString
        GetReg()
        Me.ltMensaje.Text = String.Empty
        ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Facturacion", "open_popup();", True)


    End Sub

    ''' <summary>
    ''' ONTIENE EL NOMBRE DE LA PAGINA WEB ACTUAL JUNTO CON SUS VARIABLES Y CODIGOS EN LA URL
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetPageName() As String
        Dim arrPath() As String = HttpContext.Current.Request.RawUrl.Split("/")
        'use ésta... si desea que el resultado sea en minúsculas
        'Return arrPath(arrPath.GetUpperBound(0)).ToLower

        Return arrPath(arrPath.GetUpperBound(0))
    End Function

    '"CARGAR DATOS PARA EDITAR"
    ''' <summary>
    ''' OBTIENE LOS DATOS DEL REGISTRO SELECCIONADO Y LOS CARGA EN EL FORMULARIO.
    ''' </summary>
    ''' <remarks></remarks>

    Private Sub GetReg()

        Try

            Dim sql As String = "SET DATEFORMAT DMY " & vbCrLf
            sql &= "EXEC Cat_FormaPago @opcion=4," &
                  "@codigoPais =  '0'," &
                  "@codigoEmpresa = '0'," &
                  "@codigoPuesto = '0'," &
                  "@codigoFormaPago = '" & Me.hdfCodigo.Value & "' ," &
                  "@descripcion = '0'," &
                  "@BUSQUEDAD = '0', " &
                  "@PorDefecto = 0 "
            Dim dr As System.Data.OleDb.OleDbDataReader = DataBase.GetDataReader(sql)


            If dr.Read() Then
                Me.TextCodigo.Text = dr.Item("cod_FormaPago").ToString
                Me.txtDescripcion.Text = dr.Item("descripcion").ToString
                Me.CEmpresa.SelectedValue = dr.Item("cod_empresa").ToString()
                Me.CPais.SelectedValue = dr.Item("cod_Pais").ToString()
                Me.CPuesto.SelectedValue = dr.Item("cod_puesto").ToString()
                CheckDefault.Checked = dr.Item("PorDefecto").ToString()
                dr.Close()

            Else
                MsgBox("Error" & vbCrLf & "Proceso se Cancelara", MsgBoxStyle.Critical, "Validación de Procesos")
                Return
            End If
        Catch ex As Exception
            Me.ltMensaje.Text = conn.PmsgBox(ex.Message, "error")

        End Try
    End Sub

    Private Sub SaveUpdate()


        Dim CodigoForma As String
        Dim Descripcion As String
        Dim CodigoPais As Integer
        Dim CodigoEmpresa As Integer
        Dim CodigoPuesto As Integer
        Dim PorDefecto As Boolean

        Try

            Dim sql As String = "SET DATEFORMAT DMY " & vbCrLf
            sql &= "EXEC Cat_FormaPago @opcion=4," &
                  "@codigoPais =  '0'," &
                  "@codigoEmpresa = '0'," &
                  "@codigoPuesto = '0'," &
                  "@codigoFormaPago = '" & Me.hdfCodigo.Value & "' ," &
                  "@descripcion = '0'," &
                  "@BUSQUEDAD = '0', " &
                  "@PorDefecto = 0 "
            Dim dr As System.Data.OleDb.OleDbDataReader = DataBase.GetDataReader(sql)


            If dr.Read() Then

                CodigoForma = TextCodigo.Text.ToString()
                Descripcion = txtDescripcion.Text.ToString()
                CodigoPais = ddlPais.SelectedValue
                CodigoEmpresa = ddlEmpresa.SelectedValue
                CodigoPuesto = ddlPuesto.SelectedValue
                PorDefecto = IIf(Me.CheckDefault.Checked = True, 1, 0)

                Actualizar(CodigoForma, Descripcion, CodigoPais, CodigoEmpresa, CodigoPuesto, PorDefecto)

                dr.Close()

            ElseIf Not dr.HasRows Then

                Guardar()

            Else
                MsgBox("Error" & vbCrLf & "Proceso se Cancelara", MsgBoxStyle.Critical, "Validación de Procesos")
                Return
            End If
        Catch ex As Exception
            Me.ltMensaje.Text = conn.PmsgBox(ex.Message, "error")

        End Try


    End Sub
#End Region
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


    Protected Sub BtnGuardar_Click(sender As Object, e As EventArgs) Handles BtnGuardar.Click
        Dim MessegeText As String = String.Empty

        If Me.txtDescripcion.Text = String.Empty Then
            MessegeText = "alertify.alert('El  proceso no puede continuar. Debe digitar descripción.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)
            Return
        End If


        If Me.ddlPais.SelectedIndex = -1 Then
            MessegeText = "alertify.alert('El  proceso no puede continuar. Debe escojer Pais.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)
            Return
        End If

        If Me.TextCodigo.Text = String.Empty Then
            MessegeText = "alertify.alert('El  proceso no puede continuar. Debe Digitar Codigo.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)
            Return
        End If


        SaveUpdate()

        ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Facturacion", "close_popup();", True)



    End Sub

    Private Sub Guardar()
        Dim MessegeText As String = String.Empty

        Dim dbCon As New System.Data.OleDb.OleDbConnection(conn.conn)
        Try
            If dbCon.State = ConnectionState.Closed Then
                dbCon.Open()
            End If

            Dim sql As String = "SET DATEFORMAT DMY " & vbCrLf
            sql &= "EXEC Cat_FormaPago @opcion=1," &
                   "@codigoPais =  " & Me.ddlPais.SelectedValue & " ," &
                   "@codigoEmpresa =  " & Me.ddlEmpresa.SelectedValue & " ," &
                   "@codigoPuesto =  " & Me.ddlPuesto.SelectedValue & " ," &
                   "@codigoFormaPago =  '" & Me.TextCodigo.Text.Trim & "' ," &
                   "@descripcion =  '" & Me.txtDescripcion.Text.Trim & "' ," &
                   "@BUSQUEDAD = '0'  ," &
                   "@PorDefecto = " & IIf(Me.CheckDefault.Checked = True, 1, 0) & " "

            Dim cmd As New OleDb.OleDbCommand(sql, dbCon)
            cmd.ExecuteNonQuery()

            Load_GridView()
            Valorcodig()
            MessegeText = "success_messege('El registro ha sido guardado de forma correcta.');"

            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)

        Catch ex As Exception
            MessegeText = "alertify.error('Ha ocurrido un error al intentar guardar los datos. Si el problema persiste, contacte con el administrador.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)

        Finally
            If dbCon.State = ConnectionState.Open Then
                dbCon.Close()
            End If

        End Try
    End Sub

    Private Sub Eliminar()
        Dim MessegeText As String = String.Empty
        Dim dbCon As New System.Data.OleDb.OleDbConnection(conn.conn)
        Try
            If dbCon.State = ConnectionState.Closed Then
                dbCon.Open()
            End If

            Dim sql As String = "SET DATEFORMAT DMY " & vbCrLf
            sql &= "EXEC Cat_FormaPago @opcion=2," &
                  "@codigoPais =  '0'," &
                  "@codigoEmpresa = '0'," &
                  "@codigoPuesto = '0'," &
                  "@codigoFormaPago =  '" & Me.hdfCodigo.Value & "' ," &
                  "@descripcion = '0'," &
                  "@BUSQUEDAD = '0'  ," &
                  "@PorDefecto = 0 "


            Dim cmd As New OleDb.OleDbCommand(sql, dbCon)
            cmd.ExecuteNonQuery()

            Load_GridView()
            Valorcodig()

            MessegeText = "alertify.success(El registro ha sido correctamente eliminado.);"

            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Message", MessegeText, True)



        Catch ex As Exception
            MessegeText = "alertify.error('Ha ocurrido un error al intentar eliminar los datos. Si el problema persiste, contacte con el administrador. " & Replace(ex.Message, "'", "") & "');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)

        Finally
            If dbCon.State = ConnectionState.Open Then
                dbCon.Close()
            End If

        End Try
    End Sub

    Private Sub Actualizar(CodigoForma As String, Descripcion As String, CodigoPais As Integer, CodigoEmpresa As Integer, CodigoPuesto As Integer, PorDefecto As Boolean)

        Dim MessegeText As String = String.Empty
        Dim dbCon As New System.Data.OleDb.OleDbConnection(conn.conn)
        Try
            If dbCon.State = ConnectionState.Closed Then
                dbCon.Open()
            End If
            Dim sql As String = "SET DATEFORMAT DMY " & vbCrLf
            sql &= "EXEC Cat_FormaPago @opcion=1," &
                  "@codigoPais =  " & CodigoPais & " ," &
                  "@codigoEmpresa =  " & CodigoEmpresa & " ," &
                  "@codigoPuesto =  " & CodigoPuesto & " ," &
                  "@codigoFormaPago =  '" & CodigoForma & "' ," &
                  "@descripcion =  " & Descripcion & " ," &
                  "@BUSQUEDAD = '0'  ," &
                  "@PorDefecto = " & IIf(PorDefecto = True, 1, 0) & " "

            Dim cmd As New OleDb.OleDbCommand(sql, dbCon)
            cmd.ExecuteNonQuery()

            Load_GridView()
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
    Private Sub Valorcodig()

        'Dim Sql As String
        'Sql = "SET DATEFORMAT DMY;" & "SELECT ISNULL(COUNT(k.Cod_FormaPago)+1,1) as consec FROM Forma_pago k "
        'Dim fr As System.Data.OleDb.OleDbDataReader = DataBase.GetDataReader(Sql)
        'If fr.Read() Then
        '    Me.TextCodigo.Text = fr.Item("consec").ToString()
        'End If
    End Sub

    Protected Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        Valorcodig()
        'Dim Sql As String
        'Sql = " SET DATEFORMAT DMY " + "SELECT ISNULL(MAX(k.Cod_moneda)+1,1) as consec FROM monedas k "
        'Dim fr As System.Data.OleDb.OleDbDataReader = DataBase.GetDataReader(Sql)
        'If fr.Read() Then
        '    Me.TextCodigo.Text = fr.Item("consec").ToString()
        'End If
    End Sub
#Region "EXPORTAR DATOS A EXCELL"

    Protected Sub btnExportar_Click(sender As Object, e As EventArgs) Handles btnExportar.Click
        Try
            Response.Clear()
            Response.Buffer = True

            Dim name_doc = "catalogo_FormasDeOago" & Date.Now.Year & Date.Now.Month & Date.Now.Day

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
            MyHTML &= "<h2>Catalogo de Formas de Pago <br/> al dia " & Date.Now.Day & "/" & Date.Now.Month & "/" & Date.Now.Year & "</h2>"

            If Not dtTabla Is Nothing Then
                MyHTML &= "<table cellspacing=""0"" cellpadding=""0"">" &
                          "<tbody>" &
                          "<tr>" &
                          "<th></th>" &
                          "<th>Codigo</th>" &
                          "<th>Descripcion</th>" &
                          "<th>Pais</th>" &
                          "<th>Puesto</th>" &
                          "<th>Empresa</th>" &
                          "<th>PorDefecto</th>" &
                          "</tr>"

                Dim clase As String = String.Empty

                For i As Integer = 0 To dtTabla.Rows.Count - 1
                    MyHTML &= "<tr" & clase & ">" &
                              "<td class=""first"">" & i + 1 & "</td>" &
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("codigo").ToString.Trim & "</td>" &
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("Descripcion").ToString.Trim & "</td>" &
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("Pais").ToString.Trim & "</td>" &
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("puesto").ToString.Trim & "</td>" &
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("empresa").ToString.Trim & "</td>" &
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("PorDefecto").ToString.Trim & "</td>" &
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
            Me.ltMensaje.Text = conn.PmsgBox("Ocurrio un error al intentar exportar la tabla. " & ex.Message, "error")
        End Try
    End Sub

    Private Sub BtnCerrar_Click(sender As Object, e As EventArgs) Handles BtnCerrar.Click
        ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Facturacion", "close_popup();", True)
    End Sub

#End Region
End Class
