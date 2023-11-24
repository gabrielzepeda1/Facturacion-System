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
Partial Class catalogos_Vendedores
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

            Puesto_Name = "Industrial Comercial San Martin"
            Me.TextCodigo.Attributes.Add("placeholder", "Digite Codigo")
            Me.TextCodigo.Attributes.Add("requerid", "requerid")
            Me.txtNombre.Attributes.Add("placeholder", "Digite Nombre")
            Me.txtNombre.Attributes.Add("requerid", "requerid")
            Me.txtApellidos.Attributes.Add("placeholder", "Digite Apellido")
            Me.txtApellidos.Attributes.Add("requerid", "requerid")

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
            SQL &= "EXEC Cat_Vendedores @opcion=3," & _
                  "@codigoPais =   " & vCPais & " ," & _
                  "@codigoEmp =  0  ," & _
                  "@codigoPuesto = 0  ," & _
                  "@nombre = 0  ," & _
                  "@Apellido = 0  ," & _
                  "@codusuario =  NULL ," & _
                  "@codusuarioUlt =  NULL  ," & _
                  "@codigoVendedor =  0  ," & _
                  "@ctaContable =  0  ," & _
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

            Dim Sql As String
            Dim vPais As Integer

            Sql = " SET DATEFORMAT DMY " + "SELECT cod_pais FROM Vendedores WHERE cod_vendedor= '" & id & "' "
            Dim fr As System.Data.OleDb.OleDbDataReader = DataBase.GetDataReader(Sql)
            If fr.Read() Then
                vPais = fr.Item("cod_pais")
            End If


            Dim dataSetPais As New DataSet
            dataSetPais = DataBase.GetDataSet("SELECT cod_pais as codPais,descripcion as DesPais FROM Paises  ")
            dtTabla = dataSetPais.Tables(0)
            Dim comboSigla As DropDownList = TryCast(GridViewOne.Rows(e.NewEditIndex).FindControl("ddlPais"), DropDownList)
            If comboSigla IsNot Nothing Then
                comboSigla.DataSource = dataSetPais
                comboSigla.DataTextField = "DesPais"
                comboSigla.DataValueField = "codPais"
                comboSigla.SelectedValue = vPais
                comboSigla.DataBind()
            End If

            Dim vEmpresa As Integer
            Sql = " SET DATEFORMAT DMY " + "SELECT cod_empresa FROM Vendedores WHERE cod_vendedor= '" & id & "' "
            fr = DataBase.GetDataReader(Sql)
            If fr.Read() Then
                vEmpresa = fr.Item("cod_empresa").ToString()
            End If

            Dim dataSet As New DataSet
            dataSet = DataBase.GetDataSet("SELECT cod_empresa as codEmpresa,Descripcion AS Empresa FROM  Empresas  ")
            dtTabla = dataSet.Tables(0)
            Dim combo As DropDownList = TryCast(GridViewOne.Rows(e.NewEditIndex).FindControl("ddlEmpresa"), DropDownList)
            If combo IsNot Nothing Then
                combo.DataSource = dataSet
                combo.DataTextField = "Empresa"
                combo.DataValueField = "codEmpresa"
                combo.SelectedValue = vEmpresa
                combo.DataBind()
            End If

            Dim vPuesto As Integer
            Sql = " SET DATEFORMAT DMY " + "SELECT cod_puesto FROM Vendedores WHERE cod_vendedor= '" & id & "' "
            fr = DataBase.GetDataReader(Sql)
            If fr.Read() Then
                vPuesto = fr.Item("cod_puesto").ToString()
            End If

            Dim dataSetPuesto As New DataSet
            dataSetPuesto = DataBase.GetDataSet("SELECT cod_Puesto as codPuesto,descripcion as Puesto FROM  Puestos  ")
            dtTabla = dataSetPuesto.Tables(0)
            Dim comboCalidad As DropDownList = TryCast(GridViewOne.Rows(e.NewEditIndex).FindControl("ddlPuesto"), DropDownList)
            If comboCalidad IsNot Nothing Then
                comboCalidad.DataSource = dataSetPuesto
                comboCalidad.DataTextField = "Puesto"
                comboCalidad.DataValueField = "codPuesto"
                comboCalidad.SelectedValue = vPuesto
                comboCalidad.DataBind()
            End If



        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.pmsgBox("Ocurrió un error al disparar el evento RowEditing SelectedIndexChanged. " & ex.Message, "error")

        End Try
    End Sub

    Protected Sub GridViewOne_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles GridViewOne.RowUpdating
        'ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "smf_Script", " alert(""Esto es un mensaje"");", True)

        Dim Codigo As String = e.Keys("codigo").ToString
        Dim nombres As String = e.NewValues("nombres").ToString
        Dim apellidos As String = e.NewValues("Apellidos").ToString
        Dim cta_contable As String = e.NewValues("Cta_Contable").ToString

        Dim combo As DropDownList = TryCast(GridViewOne.Rows(e.RowIndex).FindControl("ddlPais"), DropDownList)
        Dim Pais As Integer = Convert.ToInt32(combo.SelectedValue)

        combo = TryCast(GridViewOne.Rows(e.RowIndex).FindControl("ddlEmpresa"), DropDownList)
        Dim Empresa As String = Convert.ToInt32(combo.SelectedValue)

        combo = TryCast(GridViewOne.Rows(e.RowIndex).FindControl("ddlPuesto"), DropDownList)
        Dim Puesto As String = Convert.ToInt32(combo.SelectedValue)


        Actualizar(Codigo, nombres, apellidos, cta_contable, Pais, Empresa, Puesto)
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
            sql &= "EXEC Cat_Vendedores @opcion=4," & _
                   "@codigoPais =  0 ," & _
                   "@codigoEmp =  0 ," & _
                   "@codigoPuesto = 0 ," & _
                   "@nombre =  '0' ," & _
                   "@Apellido =  '0' ," & _
                   "@codusuario =  " & Request.Cookies("CKSMFACTURA")("cod_usuario") & " ," & _
                   "@codusuarioUlt =  " & Request.Cookies("CKSMFACTURA")("cod_usuario") & " ," & _
                   "@codigoVendedor =  " & Me.hdfCodigo.Value & "," & _
                   "@ctaContable =  '0' ," & _
                   "@BUSQUEDAD = '0'  "


            Dim dr As System.Data.OleDb.OleDbDataReader = DataBase.GetDataReader(sql)


            If dr.Read() Then
                Me.TextCodigo.Text = dr.Item("cod_vendedor").ToString
                Me.Textctacontable.Text = dr.Item("cta_contable").ToString
                Me.txtApellidos.Text = dr.Item("apellidos").ToString
                Me.txtNombre.Text = dr.Item("nombres").ToString
                Me.CPuesto.SelectedValue = dr.Item("cod_puesto").ToString()
                Me.CPais.SelectedValue = dr.Item("cod_Pais").ToString()
                Me.CEmpresa.SelectedValue = dr.Item("cod_empresa").ToString()
                dr.Close()

            Else
                MsgBox("Error" & vbCrLf & "Proceso se Cancelara", MsgBoxStyle.Critical, "Validación de Procesos")
                Exit Sub
            End If
        Catch ex As Exception
            Me.ltMensaje.Text = conn.pmsgBox(ex.Message, "error")

        End Try
    End Sub
#End Region
    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNew.Click

        Dim Sql As String

        Sql = " SET DATEFORMAT DMY " + "SELECT ISNULL(MAX(k.Cod_vendedor)+1,1) as consec FROM Vendedores k "
        Dim fr As System.Data.OleDb.OleDbDataReader = DataBase.GetDataReader(Sql)
        If fr.Read() Then
            Me.TextCodigo.Text = fr.Item("consec").ToString()
        End If
    End Sub

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

    Protected Sub BtnGuardar_Click(sender As Object, e As EventArgs) Handles BtnGuardar.Click
        Dim MessegeText As String = String.Empty

        If Me.txtNombre.Text = String.Empty Then
            MessegeText = "alertify.alert('El  proceso no puede continuar. Debe digitar Nombre de Vendedor.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)
            Exit Sub
        End If

        If Me.txtApellidos.Text = String.Empty Then
            MessegeText = "alertify.alert('El  proceso no puede continuar. Debe digitar Apellidos.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)
            Exit Sub
        End If

        If Me.Textctacontable.Text = String.Empty Then
            MessegeText = "alertify.alert('El  proceso no puede continuar. Debe digitar Cuenta Contable.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)
            Exit Sub
        End If

        If Me.ddlPais.SelectedIndex = -1 Then
            MessegeText = "alertify.alert('El  proceso no puede continuar. Debe escojer Pais del vendedor.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)
            Exit Sub
        End If

        If Me.ddlEmpresa.SelectedIndex = -1 Then
            MessegeText = "alertify.alert('El  proceso no puede continuar. Debe escojer Empresa del vendedor.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)
            Exit Sub
        End If

        If Me.ddlPuesto.SelectedIndex = -1 Then
            MessegeText = "alertify.alert('El  proceso no puede continuar. Debe escojer Puesto del vendedor.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)
            Exit Sub
        End If


        Guardar()
    End Sub

    Private Sub Nuevo()
        Try

            'Me.ddlPais.SelectedValue = "Seleccione..."
            'Me.ddlEmpresa.SelectedValue = "Seleccione..."
            'Me.ddlPuesto.SelectedValue = "Seleccione..."
            ' COMENTADO PARA ELIMINAR ERROR DE VALOR EN DDLPAIS
            Me.txtNombre.Text = String.Empty
            Me.txtApellidos.Text = String.Empty
            Me.TextCodigo.Text = String.Empty
            Me.Textctacontable.Text = String.Empty


        Catch ex As Exception
            Me.ltMensaje.Text = conn.pmsgBox(ex.Message, "error")
        End Try
    End Sub

    Private Sub Guardar()
        Dim MessegeText As String = String.Empty

        Dim dbCon As New System.Data.OleDb.OleDbConnection(conn.conn)
        Try
            If dbCon.State = ConnectionState.Closed Then
                dbCon.Open()
            End If

            Dim sql As String = "SET DATEFORMAT DMY " & vbCrLf
            sql &= "EXEC Cat_Vendedores @opcion=1," & _
                   "@codigoPais =  " & Me.ddlPais.SelectedValue & " ," & _
                   "@codigoEmp =  " & Me.ddlEmpresa.SelectedValue & " ," & _
                   "@codigoPuesto =  " & Me.ddlPuesto.SelectedValue & " ," & _
                   "@nombre =  '" & Me.txtNombre.Text.Trim & "' ," & _
                   "@Apellido =  '" & Me.txtApellidos.Text.Trim & "' ," & _
                   "@codusuario =  " & Request.Cookies("CKSMFACTURA")("cod_usuario") & " ," & _
                   "@codusuarioUlt =  " & Request.Cookies("CKSMFACTURA")("cod_usuario") & " ," & _
                   "@codigoVendedor =  " & Me.TextCodigo.Text.Trim & " ," & _
                   "@ctaContable =  '" & Me.Textctacontable.Text.Trim & "' ," & _
                   "@BUSQUEDAD = '0'  "

            Dim cmd As New OleDb.OleDbCommand(sql, dbCon)
            cmd.ExecuteNonQuery()

            Load_GridView()
            Nuevo()
            MessegeText = "success_messege('El registro ha sido guardado de forma correcta.');"

            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Facturacion", "close_popup();", True)

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
            sql &= "EXEC Cat_Vendedores @opcion=2," & _
                   "@codigoPais =  0 ," & _
                   "@codigoEmp =  0 ," & _
                   "@codigoPuesto =  0 ," & _
                   "@nombre =  '0' ," & _
                   "@Apellido =  '0' ," & _
                   "@codusuario =  " & Request.Cookies("CKSMFACTURA")("cod_usuario") & " ," & _
                   "@codusuarioUlt =  " & Request.Cookies("CKSMFACTURA")("cod_usuario") & " ," & _
                   "@codigoVendedor =  " & Me.hdfCodigo.Value & " ," & _
                   "@ctaContable =  '0' ," & _
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

    Private Sub Actualizar(Codigo As String, nombres As String, apellidos As String, cta_contable As String, Pais As Integer, Empresa As Integer, Puesto As Integer)
        Dim MessegeText As String = String.Empty
        Dim dbCon As New System.Data.OleDb.OleDbConnection(conn.conn)
        Try
            If dbCon.State = ConnectionState.Closed Then
                dbCon.Open()
            End If

            Dim sql As String = "SET DATEFORMAT DMY " & vbCrLf
            sql &= "EXEC Cat_Vendedores @opcion=1," & _
                  "@codigoPais =  " & Pais & " ," & _
                   "@codigoEmp =  " & Empresa & " ," & _
                   "@codigoPuesto =  " & Puesto & " ," & _
                   "@nombre =  '" & nombres & "' ," & _
                   "@Apellido =  '" & apellidos & "' ," & _
                   "@codusuario =  " & Request.Cookies("CKSMFACTURA")("cod_usuario") & " ," & _
                   "@codusuarioUlt =  " & Request.Cookies("CKSMFACTURA")("cod_usuario") & " ," & _
                   "@codigoVendedor =  " & Codigo & " ," & _
                   "@ctaContable =  '" & cta_contable & "' ," & _
                   "@BUSQUEDAD = '0'  "            'Actualizar(Codigo, nombres, apellidos, cta_contable, Pais, Empresa, Puesto)


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
#End Region

#Region "EXPORTAR DATOS A EXCELL"

    Protected Sub btnExportar_Click(sender As Object, e As EventArgs) Handles btnExportar.Click
        Try
            Response.Clear()
            Response.Buffer = True

            Dim name_doc = "catalogo_Vendedores" & Date.Now.Year & Date.Now.Month & Date.Now.Day

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
            MyHTML &= "<h2>Catalogo de Vendedores <br/> al dia " & Date.Now.Day & "/" & Date.Now.Month & "/" & Date.Now.Year & "</h2>"

            If Not dtTabla Is Nothing Then
                MyHTML &= "<table cellspacing=""0"" cellpadding=""0"">" & _
                          "<tbody>" & _
                          "<tr>" & _
                          "<th></th>" & _
                          "<th>Codigo</th>" & _
                          "<th>Nombres</th>" & _
                          "<th>Apellidos</th>" & _
                          "<th>Pais</th>" & _
                          "<th>Empresa</th>" & _
                          "<th>Puesto</th>" & _
                          "<th>Cta_Contable</th>" & _
                          "</tr>"
               
                Dim clase As String = String.Empty

                For i As Integer = 0 To dtTabla.Rows.Count - 1
                    MyHTML &= "<tr" & clase & ">" & _
                              "<td class=""first"">" & i + 1 & "</td>" & _
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("Codigo").ToString.Trim & "</td>" & _
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("Nombres").ToString.Trim & "</td>" & _
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("Apellidos").ToString.Trim & "</td>" & _
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("Pais").ToString.Trim & "</td>" & _
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("Empresa").ToString.Trim & "</td>" & _
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("Puesto").ToString.Trim & "</td>" & _
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("Cta_Contable").ToString.Trim & "</td>" & _
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

#End Region


End Class
