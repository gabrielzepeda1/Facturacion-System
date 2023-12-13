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
Partial Class catalogos_Puestos
    Inherits System.Web.UI.Page

    Dim conn As New FACTURACION_CLASS.Seguridad
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

            Form.DefaultButton = Me.btnLoad.UniqueID

            Puesto_Name = "Industrial Comercial San Martin"
            Me.TextPuesto.Attributes.Add("placeholder", "Campo Obligatorio")
            Me.TextPuesto.Attributes.Add("requerid", "requerid")
            Me.TextNoDebito.Attributes.Add("placeholder", "Campo Obligatorio")
            Me.TextNoDebito.Attributes.Add("requerid", "requerid")
            Me.TextNoCredito.Attributes.Add("placeholder", "Campo Obligatorio")
            Me.TextNoCredito.Attributes.Add("requerid", "requerid")

            Me.TextNoRecibo.Attributes.Add("placeholder", "Campo Obligatorio")
            Me.TextNoRecibo.Attributes.Add("requerid", "requerid")
            Me.TextNoCredRetenc.Attributes.Add("placeholder", "Campo Obligatorio")
            Me.TextNoCredRetenc.Attributes.Add("requerid", "requerid")
            Me.TextFormatoImp.Attributes.Add("placeholder", "Campo Obligatorio")
            Me.TextFormatoImp.Attributes.Add("requerid", "requerid")
            Me.TextTelefono.Attributes.Add("placeholder", "Campo Obligatorio")
            Me.TextTelefono.Attributes.Add("requerid", "requerid")

            Me.TextDescCorta.Attributes.Add("placeholder", "Campo Obligatorio")
            Me.TextDescCorta.Attributes.Add("requerid", "requerid")

            Me.txtBuscar.Attributes.Add("placeholder", "Escriba para Buscar")

            Me.txtCodigo.Attributes.Add("pattern", "[0-9]")
            Me.TextNoDebito.Attributes.Add("pattern", "[0-9]")
            Me.TextNoCredito.Attributes.Add("pattern", "[0-9]")
            Me.TextNoRecibo.Attributes.Add("pattern", "[0-9]")
            Me.TextNoCredRetenc.Attributes.Add("pattern", "[0-9]")

            
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
            Dim vpAIS As String = String.Empty
            Dim vEMPRESA As String = String.Empty
            vpAIS = Context.Request.Cookies("CKSMFACTURA")("codPais")
            vEMPRESA = Context.Request.Cookies("CKSMFACTURA")("CodEmpresa")


            Dim SQL As String = String.Empty
            SQL &= "EXEC Cat_puesto @opcion=3," &
                  "@codPuesto =  0  ," &
                  "@codEmpresa =  '" & vEMPRESA & "'  ," &
                  "@codPais = '" & vpAIS & "'  ," &
                  "@descripcion =  0  ," &
                  "@noNotaDebito =  0  ," &
                  "@noNotaCredito = 0  ," &
                  "@noRecibo = 0  ," &
                  "@noFactura =  0  ," &
                  "@noCreditoRet =  0  ," &
                  "@formatoImpresion = 0  ," &
                  "@lineasImprimir = 0  ," &
                  "@codusuario =  NULL ," &
                  "@codusuarioUlt =  NULL  ," &
                  "@NoCuotasPlanillas = 0  ," &
                  "@VerifInventario = 0  ," &
                  "@telefono = 0  ," &
                  "@descripCorta = 0  ," &
                  "@BUSQUEDAD = '" & BUSQUEDAD & "' "



            Dim ds As DataSet
            ds = DataBase.GetDataSet(SQL)

            dtTabla = ds.Tables(0)

            Me.GridViewOne.DataSource = dtTabla.DefaultView
            Me.GridViewOne.DataBind()
            ds.Dispose()

        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.PmsgBox("Ocurrió un error al intentar cargar el grid de puestos." & ex.Message, "error")

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
            Dim vpais As String
            Dim vempresa As Integer

            Sql = " SET DATEFORMAT DMY " + "SELECT cod_pais,cod_empresa FROM puestos WHERE cod_puesto= " & id & " "
            Dim fr As System.Data.OleDb.OleDbDataReader = DataBase.GetDataReader(Sql)
            If fr.Read() Then
                vpais = fr.Item("cod_pais").ToString()
                vempresa = fr.Item("cod_empresa").ToString()
            End If

            Dim Vinvent As Boolean
            Dim ckeckInv As CheckBox = TryCast(GridViewOne.Rows(e.NewEditIndex).FindControl("CheckVeInv"), CheckBox)
            If ckeckInv IsNot Nothing Then
                ckeckInv.Checked = Vinvent
            End If

            Dim dataSetPais As New DataSet
            dataSetPais = DataBase.GetDataSet("SELECT cod_pais,descripcion as Pais FROM Paises  ")
            dtTabla = dataSetPais.Tables(0)
            Dim comboPais As DropDownList = TryCast(GridViewOne.Rows(e.NewEditIndex).FindControl("ddlPais"), DropDownList)
            If comboPais IsNot Nothing Then
                comboPais.DataSource = dataSetPais
                comboPais.DataTextField = "Pais"
                comboPais.DataValueField = "Cod_pais"
                comboPais.SelectedValue = vpais
                comboPais.DataBind()
            End If

            
            Dim dataSetEmpresa As New DataSet
            dataSetEmpresa = DataBase.GetDataSet("SELECT cod_empresa,descripcion as Empresa FROM empresas  ")
            dtTabla = dataSetEmpresa.Tables(0)
            Dim comboEmpresa As DropDownList = TryCast(GridViewOne.Rows(e.NewEditIndex).FindControl("ddlEmpresa"), DropDownList)
            If comboEmpresa IsNot Nothing Then
                comboEmpresa.DataSource = dataSetEmpresa
                comboEmpresa.DataTextField = "Empresa"
                comboEmpresa.DataValueField = "cod_empresa"
                comboEmpresa.SelectedValue = vempresa
                comboEmpresa.DataBind()
            End If



        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.PmsgBox("Ocurrió un error al disparar el evento RowEditing . " & ex.Message, "error")

        End Try
    End Sub

    Protected Sub GridViewOne_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles GridViewOne.RowUpdating
        'ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "smf_Script", " alert(""Esto es un mensaje"");", True)

        Dim Codigo As String = e.Keys("codigo").ToString
        Dim DescriPuesto As String = e.NewValues("Puesto").ToString
        Dim debito As String = e.NewValues("NoNotaDebito").ToString
        Dim credito As Double = e.NewValues("NoNotaCredito").ToString
        Dim recibo As String = e.NewValues("NoRecibo").ToString
        Dim factura As String = e.NewValues("NoFactura").ToString
        Dim credRtenc As String = e.NewValues("NoNotaCredRetencion").ToString
        Dim ForIMpre As String = e.NewValues("FormatoImpresion").ToString
        Dim LineIm As String = e.NewValues("LineasImprimir").ToString
        Dim CuotPlan As String = e.NewValues("NoCuotasenPlan").ToString
        Dim telf As String = e.NewValues("telefono").ToString
        'Dim invent As String = e.NewValues("VerifInventario").ToString
        Dim desCo As String = e.NewValues("DesCorta").ToString

        Dim combo As DropDownList = TryCast(GridViewOne.Rows(e.RowIndex).FindControl("ddlPais"), DropDownList)
        Dim Pais As Integer = Convert.ToInt32(combo.SelectedValue)


        Dim comboEmpresa As DropDownList = TryCast(GridViewOne.Rows(e.RowIndex).FindControl("ddlEmpresa"), DropDownList)
        Dim Empresa As Integer = Convert.ToInt32(combo.SelectedValue)

        Dim inv As Integer
        Dim cb As CheckBox
        cb = TryCast(GridViewOne.Rows(e.RowIndex).FindControl("CheckVeInv"), CheckBox)
        inv = cb.Checked


        Actualizar(Empresa, Pais, Codigo, DescriPuesto, debito, credito, recibo, factura, credRtenc, ForIMpre, LineIm, CuotPlan, telf, desCo, inv)
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
            sql &= "EXEC Cat_puesto @opcion=4," & _
                 "@codPuesto =  " & Me.hdfCodigo.Value & "," & _
                 "@codEmpresa =  0  ," & _
                 "@codPais = 0  ," & _
                 "@descripcion =  0  ," & _
                 "@noNotaDebito =  0  ," & _
                 "@noNotaCredito = 0  ," & _
                 "@noRecibo = 0  ," & _
                 "@noFactura =  0  ," & _
                 "@noCreditoRet =  0  ," & _
                 "@formatoImpresion = 0  ," & _
                 "@lineasImprimir = 0  ," & _
                 "@codusuario =  NULL ," & _
                 "@codusuarioUlt =  NULL  ," & _
                 "@NoCuotasPlanillas = 0  ," & _
                 "@VerifInventario = 0  ," & _
                 "@telefono = 0  ," & _
                 "@descripCorta = 0  ," & _
                 "@BUSQUEDAD = '" & BUSQUEDAD & "' "


            Dim dr As System.Data.OleDb.OleDbDataReader = DataBase.GetDataReader(sql)


            If dr.Read() Then
                Me.txtCodigo.Text = dr.Item("cod_puesto").ToString
                Me.TextPuesto.Text = dr.Item("descripcion").ToString
                Me.TextNoDebito.Text = dr.Item("no_nota_debito").ToString
                Me.TextNoCredito.Text = dr.Item("no_nota_credito").ToString
                Me.TextNoRecibo.Text = dr.Item("no_recibo").ToString
                Me.TextNoCredRetenc.Text = dr.Item("no_nota_credito_retencion").ToString
                Me.TextFormatoImp.Text = dr.Item("formato_impresion").ToString
                Me.TextTelefono.Text = dr.Item("telefono").ToString
                Me.TextDescCorta.Text = dr.Item("descripcion_corta").ToString
                Me.CheckVeInv.Checked = IIf(dr.Item("verificar_inventario").ToString() = "Si", True, False)
                Me.CCPAIS.SelectedValue = dr.Item("cod_Pais").ToString()
                Me.CCEMPRESA.SelectedValue = dr.Item("codEmpresa").ToString()
                Me.TextCuotaPla.Text = dr.Item("numero_cuotas").ToString
                Me.TextLineImpri.Text = dr.Item("lineas_imprimir").ToString
                Me.TextFactura.Text = dr.Item("no_factura").ToString
                dr.Close()

            Else
                MsgBox("Error" & vbCrLf & "Proceso se Cancelara", MsgBoxStyle.Critical, "Validación de Procesos")
                Exit Sub
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

        If Me.TextPuesto.Text = String.Empty Then
            MessegeText = "alertify.alert('El  proceso no puede continuar. Debe digitar descripción.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)
            Return
        End If

        If Me.TextNoDebito.Text = String.Empty Then
            MessegeText = "alertify.alert('El  proceso no puede continuar. Debe digitar numero de Nota de Debito.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)
            Return
        End If

        If Me.ddlPais.SelectedIndex = -1 Then
            MessegeText = "alertify.alert('El  proceso no puede continuar. Debe escojer Pais.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)
            Return
        End If

        If Me.ddlEmpresa.SelectedIndex = -1 Then
            MessegeText = "alertify.alert('El  proceso no puede continuar. Debe escojer Empresa.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)
            Return
        End If


        If Me.TextNoCredito.Text = String.Empty Then
            MessegeText = "alertify.alert('El  proceso no puede continuar. Debe digitar numero de Nota de Credito.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)
            Return
        End If


        If Me.TextNoRecibo.Text = String.Empty Then
            MessegeText = "alertify.alert('El  proceso no puede continuar. Debe digitar numero de Recibo.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)
            Return
        End If

        If Me.TextFactura.Text = String.Empty Then
            MessegeText = "alertify.alert('El  proceso no puede continuar. Debe digitar numero de Factura.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)
            Return
        End If

        If Me.TextNoCredRetenc.Text = String.Empty Then
            MessegeText = "alertify.alert('El  proceso no puede continuar. Debe digitar numero de nota de Credito de rentencion.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)
            Return
        End If

        If Me.TextFormatoImp.Text = String.Empty Then
            MessegeText = "alertify.alert('El  proceso no puede continuar. Debe digitar Nombre del formato de impresion.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)
            Return
        End If

        If Me.TextLineImpri.Text = String.Empty Then
            MessegeText = "alertify.alert('El  proceso no puede continuar. Debe digitar el numero de maximo de Items a imprimir .');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)
            Return
        End If

        If Me.TextCuotaPla.Text = String.Empty Then
            MessegeText = "alertify.alert('El  proceso no puede continuar. Debe digitar No de cuotas a deducir en planilla por facturas a pagar.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)
            Return
        End If

        If Me.TextTelefono.Text = String.Empty Then
            MessegeText = "alertify.alert('El  proceso no puede continuar. Debe digitar No de Telefono.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)
            Return
        End If

        If Me.TextDescCorta.Text = String.Empty Then
            MessegeText = "alertify.alert('El  proceso no puede continuar. Debe digitar Descripcion corta.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)
            Return
        End If


        Guardar()

        ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Facturacion", "close_popup();", True)



    End Sub

    Private Sub Nuevo()
        Try


            txtCodigo.Text = String.Empty

            ddlEmpresa.Items.Insert(0, New ListItem("-Seleccione-", 0))
            ddlPais.Items.Insert(0, New ListItem("-Seleccione-", 0))
            ddlEmpresa.SelectedIndex = 0
            ddlPais.SelectedIndex = 0


            TextPuesto.Text = String.Empty
            TextDescCorta.Text = String.Empty
            TextNoDebito.Text = String.Empty
            TextNoCredito.Text = String.Empty
            TextNoRecibo.Text = String.Empty
            TextFactura.Text = String.Empty
            TextNoCredRetenc.Text = String.Empty
            TextFormatoImp.Text = String.Empty
            TextLineImpri.Text = String.Empty
            TextCuotaPla.Text = String.Empty
            CheckVeInv.Checked = False
            TextTelefono.Text = String.Empty

        Catch ex As Exception
            Me.ltMensaje.Text = conn.PmsgBox(ex.Message, "error")
        End Try
    End Sub


    Private Sub Guardar()
        Dim MessegeText As String = String.Empty

        Dim dbCon As New System.Data.OleDb.OleDbConnection(conn.Conn)
        Try
            If dbCon.State = ConnectionState.Closed Then
                dbCon.Open()
            End If

            Dim sql As String = "SET DATEFORMAT DMY " & vbCrLf
            sql &= "EXEC Cat_puesto @opcion=1," & _
                   "@codPuesto =  " & Me.txtCodigo.Text.Trim & " ," & _
                  "@codEmpresa =  " & Me.ddlEmpresa.SelectedValue & " ," & _
                  "@codPais =  " & Me.ddlPais.SelectedValue & " ," & _
                  "@descripcion =  '" & Me.TextPuesto.Text.Trim & "' ," & _
                  "@noNotaDebito =  " & Me.TextNoDebito.Text.Trim & " ," & _
                  "@noNotaCredito =  " & Me.TextNoCredito.Text.Trim & " ," & _
                  "@noRecibo =  " & Me.TextNoRecibo.Text.Trim & " ," & _
                  "@noFactura =  " & Me.TextFactura.Text.Trim & " ," & _
                  "@noCreditoRet =  " & Me.TextNoCredRetenc.Text.Trim & " ," & _
                  "@formatoImpresion =  '" & Me.TextFormatoImp.Text.Trim & "' ," & _
                  "@lineasImprimir =  " & Me.TextLineImpri.Text.Trim & " ," & _
                  "@codusuario =   " & Request.Cookies("CKSMFACTURA")("cod_usuario") & " ," & _
                  "@codusuarioUlt =  " & Request.Cookies("CKSMFACTURA")("cod_usuario") & " ," & _
                  "@NoCuotasPlanillas =  " & Me.TextCuotaPla.Text.Trim & " ," & _
                  "@VerifInventario =  " & IIf(Me.CheckVeInv.Checked = True, 1, 0) & " ," & _
                  "@telefono =  '" & Me.TextTelefono.Text.Trim & "' ," & _
                  "@descripCorta =  '" & Me.TextDescCorta.Text.Trim & "' ," & _
                  "@BUSQUEDAD = '0'  "


            Dim cmd As New OleDb.OleDbCommand(sql, dbCon)
            cmd.ExecuteNonQuery()

            Load_GridView()
            Nuevo()
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
        Dim dbCon As New System.Data.OleDb.OleDbConnection(conn.Conn)
        Try
            If dbCon.State = ConnectionState.Closed Then
                dbCon.Open()
            End If

            Dim sql As String = "SET DATEFORMAT DMY " & vbCrLf
            sql &= "EXEC Cat_puesto @opcion=2," & _
                  "@codPuesto =  " & Me.hdfCodigo.Value & " ," & _
                  "@codEmpresa =  " & Me.ddlEmpresa.SelectedValue & " ," & _
                  "@codPais =  " & Me.ddlPais.SelectedValue & " ," & _
                  "@descripcion =  '" & Me.TextPuesto.Text.Trim & "' ," & _
                  "@noNotaDebito =  0 ," & _
                  "@noNotaCredito =  0 ," & _
                  "@noRecibo =  0 ," & _
                  "@noFactura =  0 ," & _
                  "@noCreditoRet = 0 ," & _
                  "@formatoImpresion =  '" & Me.TextFormatoImp.Text.Trim & "' ," & _
                  "@lineasImprimir =  0 ," & _
                  "@codusuario =   " & Request.Cookies("CKSMFACTURA")("cod_usuario") & " ," & _
                  "@codusuarioUlt =  " & Request.Cookies("CKSMFACTURA")("cod_usuario") & " ," & _
                  "@NoCuotasPlanillas = 0 ," & _
                  "@VerifInventario =  " & IIf(Me.CheckVeInv.Checked = True, 1, 0) & " ," & _
                  "@telefono =  '" & Me.TextTelefono.Text.Trim & " '," & _
                  "@descripCorta =  '" & Me.TextDescCorta.Text.Trim & "' ," & _
                  "@BUSQUEDAD = '0'  "


            Dim cmd As New OleDb.OleDbCommand(sql, dbCon)
            cmd.ExecuteNonQuery()

            Load_GridView()
            Nuevo()

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

    Private Sub Actualizar(Empresa As Integer, Pais As Integer, Codigo As String, DescriPuesto As String, debito As Integer, credito As Integer,
                           recibo As Integer, factura As Integer, credRtenc As Integer, ForIMpre As String, LineIm As Integer,
                           CuotPlan As Integer, telf As String, desCo As String, inv As Integer)
        Dim MessegeText As String = String.Empty
        Dim dbCon As New System.Data.OleDb.OleDbConnection(conn.Conn)
        Try
            If dbCon.State = ConnectionState.Closed Then
                dbCon.Open()
            End If

            Dim sql As String = "SET DATEFORMAT DMY " & vbCrLf
            sql &= "EXEC Cat_puesto @opcion=1," & _
                  "@codPuesto =  " & Codigo & " ," & _
                  "@codEmpresa =  " & Empresa & " ," & _
                  "@codPais =  " & Pais & " ," & _
                  "@descripcion =  '" & DescriPuesto & "' ," & _
                  "@noNotaDebito =  " & debito & " ," & _
                  "@noNotaCredito =  " & credito & " ," & _
                  "@noRecibo =  " & recibo & " ," & _
                  "@noFactura =  " & factura & " ," & _
                  "@noCreditoRet =  " & credRtenc & " ," & _
                  "@formatoImpresion =  '" & ForIMpre & "' ," & _
                  "@lineasImprimir =  " & LineIm & " ," & _
                  "@codusuario =   " & Request.Cookies("CKSMFACTURA")("cod_usuario") & " ," & _
                  "@codusuarioUlt =  " & Request.Cookies("CKSMFACTURA")("cod_usuario") & " ," & _
                  "@NoCuotasPlanillas =  " & CuotPlan & " ," & _
                  "@VerifInventario =  " & inv & " ," & _
                  "@telefono =  '" & telf & "' ," & _
                  "@descripCorta =  '" & desCo & "' ," & _
                  "@BUSQUEDAD = '0'  "



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

    Protected Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click

        Dim Sql As String

        Nuevo()

        Sql = " SET DATEFORMAT DMY " & "SELECT ISNULL(MAX(k.Cod_puesto)+1,1) as consec FROM puestos k "
        Dim fr As System.Data.OleDb.OleDbDataReader = DataBase.GetDataReader(Sql)
        If fr.Read() Then
            Me.txtCodigo.Text = fr.Item("consec").ToString()
        End If


    End Sub
#Region "EXPORTAR DATOS A EXCELL"

    Protected Sub btnExportar_Click(sender As Object, e As EventArgs) Handles btnExportar.Click
        Try
            Response.Clear()
            Response.Buffer = True

            Dim name_doc = "catalogo_Puestos" & Date.Now.Year & Date.Now.Month & Date.Now.Day

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
            MyHTML &= "<h2>Catalogo de Puestos <br/> al dia " & Date.Now.Day & "/" & Date.Now.Month & "/" & Date.Now.Year & "</h2>"

            If Not dtTabla Is Nothing Then
                MyHTML &= "<table cellspacing=""0"" cellpadding=""0"">" &
                          "<tbody>" &
                          "<tr>" &
                          "<th></th>" &
                          "<th>Codigo</th>" &
                          "<th>Puesto</th>" &
                          "<th>Pais</th>" &
                          "<th>Empresa</th>" &
                          "<th>NoNotaDebito</th>" &
                          "<th>NoNotaCredito</th>" &
                          "<th>NoRecibo</th>" &
                          "<th>NoFactura/Ruc</th>" &
                          "<th>NoNotaCredRetencion</th>" &
                          "<th>FormatoImpresion</th>" &
                          "<th>LineasImprimir</th>" &
                          "<th>NoCuotasenPlan</th>" &
                          "<th>VerifInventario</th>" &
                          "<th>Telefono</th>" &
                          "<th>DesCorta</th>" &
                          "<th>usuario</th>" &
                          "<th>UltUsuario</th>" &
                          "</tr>"


                Dim clase As String = String.Empty

                For i As Integer = 0 To dtTabla.Rows.Count - 1
                    MyHTML &= "<tr" & clase & ">" & _
                              "<td class=""first"">" & i + 1 & "</td>" & _
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("Codigo").ToString.Trim & "</td>" & _
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("Puesto").ToString.Trim & "</td>" & _
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("pais").ToString.Trim & "</td>" & _
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("Empresa").ToString.Trim & "</td>" & _
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("NoNotaDebito").ToString.Trim & "</td>" & _
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("NoNotaCredito").ToString.Trim & "</td>" & _
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("NoRecibo").ToString.Trim & "</td>" & _
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("NoFactura").ToString.Trim & "</td>" & _
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("NoNotaCredRetencion").ToString.Trim & "</td>" & _
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("FormatoImpresion").ToString.Trim & "</td>" & _
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("LineasImprimir").ToString.Trim & "</td>" & _
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("NoCuotasenPlan").ToString.Trim & "</td>" & _
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("VerifInventario").ToString.Trim & "</td>" & _
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("telefono").ToString.Trim & "</td>" & _
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("DesCorta").ToString.Trim & "</td>" & _
                             "<td class=""ml"">" & dtTabla.Rows(i).Item("usuario").ToString.Trim & "</td>" & _
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("UsuarioUltiMod").ToString.Trim & "</td>" & _
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
            Me.ltMensaje.Text = conn.PmsgBox("Ocurrio un error al intentar exportar la tabla. " & ex.Message, "error")
        End Try
    End Sub

#End Region

    Protected Sub btnLoad_Click(sender As Object, e As EventArgs) Handles btnLoad.Click
        Try
            If Me.txtCodigo.Text.Trim = String.Empty Then
                Exit Sub
            End If

            If Not IsNumeric(Me.txtCodigo.Text.Trim) Then
                Exit Sub
            End If

            If Not CDec(Me.txtCodigo.Text.Trim) > 0 Then
                Exit Sub
            End If

            Dim sql As String = String.Empty
            sql = "EXEC sp_show_puesto @cod_puesto = " & Me.txtCodigo.Text.Trim & " "

            Dim dr As System.Data.OleDb.OleDbDataReader
            dr = DataBase.GetDataReader(sql)

            If dr.Read() Then
                TextPuesto.Text = dr.Item("descripcion").ToString()
                TextDescCorta.Text = dr.Item("descripcion_corta").ToString()
                TextNoDebito.Text = dr.Item("no_nota_debito").ToString()
                TextNoCredito.Text = dr.Item("no_nota_credito").ToString()
                TextNoRecibo.Text = dr.Item("no_recibo").ToString()
                TextFactura.Text = dr.Item("no_factura").ToString()
                TextNoCredRetenc.Text = dr.Item("no_nota_credito_retencion").ToString()
                TextFormatoImp.Text = dr.Item("formato_impresion").ToString()
                TextTelefono.Text = dr.Item("telefono").ToString()
                TextLineImpri.Text = dr.Item("lineas_imprimir").ToString()
                TextCuotaPla.Text = dr.Item("numero_cuotas").ToString()

                If dr.Item("verificar_inventario").ToString() = "si" Then
                    CheckVeInv.Checked = True
                Else
                    CheckVeInv.Checked = False
                End If

                CCPAIS.SelectedValue = dr.Item("cod_pais").ToString()
                CCEMPRESA.SelectedValue = dr.Item("cod_empresa").ToString()
            Else
                txtCodigo.Text = String.Empty
                TextPuesto.Text = String.Empty
                TextDescCorta.Text = String.Empty
                TextNoDebito.Text = String.Empty
                TextNoCredito.Text = String.Empty
                TextNoRecibo.Text = String.Empty
                TextFactura.Text = String.Empty
                TextNoCredRetenc.Text = String.Empty
                TextFormatoImp.Text = String.Empty
                TextTelefono.Text = String.Empty
                CCPAIS.SelectedValue = String.Empty
                CCEMPRESA.SelectedValue = String.Empty
                TextLineImpri.Text = String.Empty
                TextCuotaPla.Text = String.Empty
                CheckVeInv.Checked = False
            End If

            txtCodigo.Focus()

            dr.Close()

        Catch ex As Exception
            Me.ltMensajeLoad.Text = conn.PmsgBox("Ocurrió un error al intentar recuperar los datos del cliente." & ex.Message, "error")
        End Try
    End Sub

    Private Sub BtnCerrar_Click(sender As Object, e As EventArgs) Handles BtnCerrar.Click

        ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Facturacion", "close_popup();", True)

        Nuevo()
    End Sub
End Class
