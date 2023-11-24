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
Partial Class catalogos_CuentaBancos
    Inherits System.Web.UI.Page

    Dim conn As New FACTURACION_CLASS.seguridad
    Dim DataBase As New FACTURACION_CLASS.database
    Dim BUSQUEDAD As String


    Dim codigoCtaBanco As Integer
    Dim Descripcion As String
    Dim CodigoPais As Integer
    Dim CodigoEmpresa As Integer
    Dim CodigoPuesto As Integer
    Dim CodigoBanco As Integer
    Dim CodigoMoneda As Integer

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
            SQL &= "EXEC Cat_BancosCtas @opcion=3," & _
                  "@codigoBancoCta =  NULL  ," & _
                  "@codigoPais =  " & vCPais & " ," & _
                  "@codigoBanco =  NULL  ," & _
                  "@codigoEmpresa =  NULL  ," & _
                  "@codigoPuesto =  NULL ," & _
                  "@codigoMoneda =  NULL ," & _
                  "@descripcion =  NULL ," & _
                  "@BUSQUEDAD = '" & BUSQUEDAD & "' "


            Dim ds As DataSet
            ds = DataBase.GetDataSet(SQL)

            dtTabla = ds.Tables(0)

            Me.GridViewOne.DataSource = dtTabla.DefaultView
            Me.GridViewOne.DataBind()

            ds.Dispose()

            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "smf_Script", "responsive_grid();", True)

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

    Protected Sub GridViewOne_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridViewOne.RowDataBound
        Try
            ''carga el campo llave es el 2dopaso DataKeyName
            'If e.Row.RowType = DataControlRowType.DataRow Then
            '    'e.Row.Attributes.Add("OnMouseOver", "On(this);")
            '    'e.Row.Attributes.Add("OnMouseOut", "Off(this);")
            '    e.Row.Attributes.Add("OnClick", "" & Page.ClientScript.GetPostBackClientHyperlink(Me.GridViewOne, "Select$" + e.Row.RowIndex.ToString) & ";")
            'End If

        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.pmsgBox("Ocurrió un error al disparar el evento RowDataBound. " & ex.Message, "error")

        End Try
    End Sub

    Protected Sub GridViewOne_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles GridViewOne.RowDeleting


        Try
            'pasar la información del Gridview hacia otro control, en este caso el control HiddenField,
            Me.hdfCodigo.Value = Me.GridViewOne.DataKeys(e.RowIndex).Value

            Dim sql As String = "SET DATEFORMAT DMY " & vbCrLf
            sql &= "EXEC Cat_BancosCtas @opcion=4," &
                  "@codigoBancoCta =  " & Me.hdfCodigo.Value & " ," &
                  "@codigoPais =  '0'," &
                  "@codigoBanco = '0'," &
                  "@codigoEmpresa = '0'," &
                  "@codigoPuesto = '0'," &
                  "@codigoMoneda = '0'," &
                  "@descripcion = '0'," &
                  "@BUSQUEDAD = '0'  "


            Dim dr As System.Data.OleDb.OleDbDataReader = DataBase.GetDataReader(sql)


            If dr.Read() Then
                codigoCtaBanco = dr.Item("cod_banco_cta").ToString
                Descripcion = dr.Item("descripcion").ToString
                CodigoPais = dr.Item("cod_puesto").ToString()
                CodigoEmpresa = dr.Item("cod_empresa").ToString()
                CodigoPuesto = dr.Item("cod_Pais").ToString()
                CodigoBanco = dr.Item("cod_Banco").ToString()
                CodigoMoneda = dr.Item("cod_moneda").ToString()
                dr.Close()

                Eliminar(codigoCtaBanco, Descripcion, CodigoPais, CodigoEmpresa, CodigoPuesto, CodigoBanco, CodigoMoneda)

            End If



            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "smf_Script", "responsive_grid();", True)

        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.pmsgBox("Ocurrió un error al disparar el evento RowDeleting. " & ex.Message, "error")

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
            Dim vCodbanco As Integer
            Dim vCodMoneda As Integer
            ''" SET DATEFORMAT DMY " + 
            Sql = "SELECT  Cod_pais,cod_empresa,cod_puesto,cod_banco,cod_moneda FROM Bancos_cuenta WHERE cod_banco_cta= '" & id & "' "
            Dim fr As System.Data.OleDb.OleDbDataReader = DataBase.GetDataReader(Sql)
            If fr.Read() Then
                vPais = fr.Item("cod_pais").ToString()
                vCodempresa = fr.Item("cod_empresa").ToString()
                vCodpuesto = fr.Item("cod_puesto").ToString()
                vCodbanco = fr.Item("cod_banco").ToString()
                vCodMoneda = fr.Item("cod_moneda").ToString()
            End If
            fr.Close()

            'Dim vPais As String
            'vPais = GridViewOne.DataKeys(e.NewEditIndex).Item(1).ToString()
            Dim dataSetMoneda As New DataSet
            dataSetMoneda = DataBase.GetDataSet("SELECT cod_moneda as codMoneda,descripcion as Moneda FROM Monedas  ")
            dtTabla = dataSetMoneda.Tables(0)
            Dim comboMoneda As DropDownList = TryCast(GridViewOne.Rows(e.NewEditIndex).FindControl("ddlMoneda"), DropDownList)
            If comboMoneda IsNot Nothing Then
                comboMoneda.DataSource = dataSetMoneda
                comboMoneda.DataTextField = "Moneda"
                comboMoneda.DataValueField = "codMoneda"
                comboMoneda.SelectedValue = vCodMoneda
                comboMoneda.DataBind()
            End If

            Dim dataSetbanco As New DataSet
            dataSetbanco = DataBase.GetDataSet("SELECT cod_banco as codBanco,descripcion as Banco FROM bancos  ")
            dtTabla = dataSetbanco.Tables(0)
            Dim combobanco As DropDownList = TryCast(GridViewOne.Rows(e.NewEditIndex).FindControl("ddlBanco"), DropDownList)
            If combobanco IsNot Nothing Then
                combobanco.DataSource = dataSetbanco
                combobanco.DataTextField = "Banco"
                combobanco.DataValueField = "codBanco"
                combobanco.SelectedValue = vCodbanco
                combobanco.DataBind()
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
            Me.ltMensajeGrid.Text &= conn.pmsgBox("Ocurrió un error al disparar el evento SelectedIndexChanged. " & ex.Message, "error")

        End Try
    End Sub

    Protected Sub GridViewOne_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles GridViewOne.RowUpdating
        'ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "smf_Script", " alert(""Esto es un mensaje"");", True)

        Dim codigoCtaBanco As String = e.Keys("codigoCtaBanco").ToString
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


        combo = TryCast(GridViewOne.Rows(e.RowIndex).FindControl("ddlBanco"), DropDownList)
        Dim codBanco As Integer = Convert.ToInt32(combo.SelectedValue)


        combo = TryCast(GridViewOne.Rows(e.RowIndex).FindControl("ddlMoneda"), DropDownList)
        Dim codMoneda As Integer = Convert.ToInt32(combo.SelectedValue)


        Actualizar(codigoCtaBanco, Descripcion, CodigoPais, CodigoEmpresa, codPuesto, codBanco, codMoneda)
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
            sql &= "EXEC Cat_BancosCtas @opcion=4," & _
                  "@codigoBancoCta =  " & Me.hdfCodigo.Value & " ," & _
                  "@codigoPais =  '0'," & _
                  "@codigoBanco = '0'," & _
                  "@codigoEmpresa = '0'," & _
                  "@codigoPuesto = '0'," & _
                  "@codigoMoneda = '0'," & _
                  "@descripcion = '0'," & _
                  "@BUSQUEDAD = '0'  "


            Dim dr As System.Data.OleDb.OleDbDataReader = DataBase.GetDataReader(sql)


            If dr.Read() Then
                Me.TextCodigo.Text = dr.Item("cod_banco_cta").ToString
                Me.txtDescripcion.Text = dr.Item("descripcion").ToString
                Me.CPuesto.SelectedValue = dr.Item("cod_puesto").ToString()
                Me.CEmpresa.SelectedValue = dr.Item("cod_empresa").ToString()
                Me.CPais.SelectedValue = dr.Item("cod_Pais").ToString()
                Me.CBanco.SelectedValue = dr.Item("cod_Banco").ToString()
                Me.CMoneda.SelectedValue = dr.Item("cod_moneda").ToString()
                dr.Close()


            Else
                MsgBox("Error" & vbCrLf & "Proceso se Cancelara", MsgBoxStyle.Critical, "Validación de Procesos")
                Return
            End If
        Catch ex As Exception
            Me.ltMensaje.Text = conn.pmsgBox(ex.Message, "error")

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

        If Me.TextCodigo.Text = -1 Then
            MessegeText = "alertify.alert('El  proceso no puede continuar. Debe Digitar Codigo.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)
            Return
        End If



        'Guardar()

        SaveUpdate()

        ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Facturacion", "close_popup();", True)

    End Sub
    Private Sub SaveUpdate()

        Dim codigoCtaBanco As Integer
        Dim Descripcion As String
        Dim CodigoPais As Integer
        Dim CodigoEmpresa As Integer
        Dim CodigoPuesto As Integer
        Dim CodigoBanco As Integer
        Dim CodigoMoneda As Integer

        Try
            If hdfCodigo.Value.ToString() <> String.Empty Then

                Dim sql As String = "SET DATEFORMAT DMY " & vbCrLf
                sql &= "EXEC Cat_BancosCtas @opcion=4," &
                  "@codigoBancoCta =  " & Me.hdfCodigo.Value & " ," &
                  "@codigoPais =  '0'," &
                  "@codigoBanco = '0'," &
                  "@codigoEmpresa = '0'," &
                  "@codigoPuesto = '0'," &
                  "@codigoMoneda = '0'," &
                  "@descripcion = '0'," &
                  "@BUSQUEDAD = '0'  "

                Dim dr As OleDb.OleDbDataReader = DataBase.GetDataReader(sql)

                If dr.Read() Then

                    codigoCtaBanco = Me.TextCodigo.Text
                    Descripcion = Me.txtDescripcion.Text
                    CodigoPuesto = Me.ddlPuesto.SelectedValue
                    CodigoEmpresa = Me.ddlEmpresa.SelectedValue
                    CodigoPais = Me.ddlPais.SelectedValue
                    CodigoBanco = Me.ddlBanco.SelectedValue
                    CodigoMoneda = Me.ddlMoneda.SelectedValue
                    dr.Close()


                    Actualizar(codigoCtaBanco, Descripcion, CodigoPais, CodigoEmpresa, CodigoPuesto, CodigoBanco, CodigoMoneda)

                    dr.Close()

                End If

            ElseIf hdfCodigo.Value.ToString() Is String.Empty OrElse hdfCodigo.Value Is Nothing Then

                Guardar()

            Else
                MsgBox("Error" & vbCrLf & "Proceso se Cancelara", MsgBoxStyle.Critical, "Validación de Procesos")
                Return
            End If
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
            sql &= "EXEC Cat_BancosCtas @opcion=1," & _
                   "@codigoBancoCta =  " & Me.TextCodigo.Text.Trim & " ," & _
                   "@codigoPais =  " & Me.ddlPais.SelectedValue & " ," & _
                   "@codigoBanco =  '" & Me.ddlBanco.SelectedValue & "' ," & _
                   "@codigoEmpresa =  '" & Me.ddlEmpresa.SelectedValue & "' ," & _
                   "@codigoPuesto =  '" & Me.ddlPuesto.SelectedValue & "' ," & _
                   "@codigoMoneda =  '" & Me.ddlMoneda.SelectedValue & "' ," & _
                   "@descripcion =  '" & Me.txtDescripcion.Text.Trim & "' ," & _
                   "@BUSQUEDAD = '0'  "


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

    Private Sub Eliminar(codigoCtaBanco As Integer, Descripcion As String, CodigoPais As Integer, CodigoEmpresa As Integer, CodigoPuesto As Integer, CodigoBanco As Integer, CodigoMoneda As Integer)
        Dim MessegeText As String = String.Empty
        Dim dbCon As New System.Data.OleDb.OleDbConnection(conn.conn)
        Try
            If dbCon.State = ConnectionState.Closed Then
                dbCon.Open()
            End If

            Dim sql As String = "SET DATEFORMAT DMY " & vbCrLf
            sql &= "EXEC Cat_BancosCtas @opcion=2," &
                  "@codigoBancoCta =  " & codigoCtaBanco & " ," &
                  "@codigoPais = " & CodigoPais & " ," &
                  "@codigoBanco = " & CodigoBanco & " ," &
                  "@codigoEmpresa = " & CodigoEmpresa & " ," &
                  "@codigoPuesto = " & CodigoPuesto & " ," &
                  "@codigoMoneda = " & CodigoMoneda & " ," &
                  "@descripcion = " & Descripcion & " ," &
                  "@BUSQUEDAD = '0'  "


            Dim cmd As New OleDb.OleDbCommand(sql, dbCon)
            cmd.ExecuteNonQuery()

            Load_GridView()
            Valorcodig()

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
    Private Sub Actualizar(codigoCtaBanco As Integer, Descripcion As String, CodigoPais As Integer, CodigoEmpresa As Integer, CodigoPuesto As Integer, CodigoBanco As Integer, CodigoMoneda As Integer)

        Dim MessegeText As String = String.Empty
        Dim dbCon As New System.Data.OleDb.OleDbConnection(conn.conn)
        Try
            If dbCon.State = ConnectionState.Closed Then
                dbCon.Open()
            End If
            Dim sql As String = "SET DATEFORMAT DMY " & vbCrLf
            sql &= "EXEC Cat_BancosCtas @opcion=1," &
                  "@codigoBancoCta =  " & codigoCtaBanco & " ," &
                  "@codigoPais =  " & CodigoPais & " ," &
                  "@codigoBanco =  " & CodigoBanco & " ," &
                  "@codigoEmpresa =  " & CodigoEmpresa & " ," &
                  "@codigoPuesto =  " & CodigoPuesto & " ," &
                  "@codigoMoneda =  " & CodigoMoneda & " ," &
                  "@descripcion =  '" & Descripcion & "' ," &
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
    Private Sub Valorcodig()
        Dim Sql As String
        Sql = " SET DATEFORMAT DMY " + "SELECT ISNULL(MAX(k.Cod_banco_cta)+1,1) as consec FROM Bancos_cuenta k "
        Dim fr As System.Data.OleDb.OleDbDataReader = DataBase.GetDataReader(Sql)
        If fr.Read() Then
            Me.TextCodigo.Text = fr.Item("consec").ToString()
        End If
    End Sub

    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNew.Click
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
            MyHTML &= "<h2>Catalogo de Monedas <br/> al dia " & Date.Now.Day & "/" & Date.Now.Month & "/" & Date.Now.Year & "</h2>"

            If Not dtTabla Is Nothing Then
                MyHTML &= "<table cellspacing=""0"" cellpadding=""0"">" & _
                          "<tbody>" & _
                          "<tr>" & _
                          "<th></th>" & _
                          "<th>Codigo Moneda</th>" & _
                          "<th>Descripcion</th>" & _
                          "<th>Codigo Pais</th>" & _
                          "<th>Simbolo Moneda</th>" & _
                          "<th>Pais</th>" & _
                          "</tr>"

                Dim clase As String = String.Empty

                For i As Integer = 0 To dtTabla.Rows.Count - 1
                    MyHTML &= "<tr" & clase & ">" & _
                              "<td class=""first"">" & i + 1 & "</td>" & _
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("codigoMoneda").ToString.Trim & "</td>" & _
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("Descripcion").ToString.Trim & "</td>" & _
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("codPais").ToString.Trim & "</td>" & _
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("simbolo_moneda").ToString.Trim & "</td>" & _
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("DesPais").ToString.Trim & "</td>" & _
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
