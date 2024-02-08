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


Partial Class catalogos_ComisionVendedores
    Inherits System.Web.UI.Page

    Dim conn As New seguridad
    Dim DataBase As New database
    Dim BUSQUEDAD As String
    Dim vPais As String
    Dim vEmpresa As Integer
    Dim vPuesto As Integer
    Dim vFamilia As Integer
    Dim vVendedor As String


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
            System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("es-ES")
            Puesto_Name = "Industrial Comercial San Martin"
            Me.TextComision.Attributes.Add("placeholder", "Digite Comision")
            Me.TextComision.Attributes.Add("requerid", "requerid")
            Me.TextComision.Text = Format(CDec(Me.TextComision.Text), "#,##0.00")
            'Me.CheckActivo.Checked = True

            Me.ddlVendedor.Attributes.Add("placeholder", "Digite Vendedor")
            Me.ddlVendedor.Attributes.Add("requerid", "requerid")
            'Me.ddlPais.Attributes.Add("placeholder", "Digite Pais")
            'Me.ddlPais.Attributes.Add("requerid", "requerid")
            'Me.ddlEmpresa.Attributes.Add("placeholder", "Digite Empresa")
            'Me.ddlEmpresa..Attributes.Add("requerid", "requerid")
            'Me.ddlPuesto.Attributes.Add("placeholder", "Digite Puesto")
            'Me.ddlPuesto.Attributes.Add("requerid", "requerid")
            'Me.ddlFamilia.Attributes.Add("placeholder", "Digite Familia")
            'Me.ddlFamilia.Attributes.Add("requerid", "requerid")

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
            SQL &= "EXEC Cat_ComisVendedores @opcion=3," &
                  "@codigoPais =  " & vCPais & " ," &
                  "@codigoEmp =  0  ," &
                  "@codigoPuesto = 0  ," &
                  "@codigoFamilia = 0  ," &
                  "@codigoVendedor = 0  ," &
                  "@comision = 0  ," &
                  "@Activo = 0  ," &
                  "@BUSQUEDAD = '" & BUSQUEDAD & "' "



            Dim ds As DataSet
            ds = DataBase.GetDataSet(SQL)

            dtTabla = ds.Tables(0)

            Me.GridViewOne.DataSource = dtTabla.DefaultView
            Me.GridViewOne.DataBind()
            ds.Dispose()

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

    Protected Sub GridViewOne_RowDeleted(sender As Object, e As GridViewDeletedEventArgs) Handles GridViewOne.RowDeleted

    End Sub

    Protected Sub GridViewOne_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles GridViewOne.RowDeleting
        Try
            'pasar la información del Gridview hacia otro control, en este caso el control HiddenField,
            Me.hdfCodigo.Value = Me.GridViewOne.DataKeys(e.RowIndex).Value
            'vPais = Me.GridViewOne.DataKeys(e.RowIndex).Values(8).ToString()
            vPais = GridViewOne.DataKeys(e.RowIndex).Item(1).ToString()
            vEmpresa = GridViewOne.DataKeys(e.RowIndex).Item(2).ToString()
            vPuesto = GridViewOne.DataKeys(e.RowIndex).Item(3).ToString()
            vFamilia = GridViewOne.DataKeys(e.RowIndex).Item(4).ToString()
            'string OrderID = gvMyGridView.DataKeys[gvr.RowIndex].Values[2].ToString();

            Dim Sql As String
            Sql = "SELECT  comision,CASE WHEN Activo = 1 THEN 'Si' ELSE 'No' END AS Activo,Cod_pais," &
                           "cod_empresa,Cod_puesto,cod_familia,cod_vendedor FROM comis_VendFamilia " &
                           "WHERE cod_vendedor= " & Me.hdfCodigo.Value & " and cod_pais=" & vPais & " and " &
                                 "cod_empresa= " & vEmpresa & " and cod_puesto=" & vPuesto & " and cod_familia= " & vFamilia & ""
            Dim fr As System.Data.OleDb.OleDbDataReader = DataBase.GetDataReader(Sql)
            If fr.Read() Then
                'vVendedor = fr.Item("cod_vendedor").ToString()
                vPais = fr.Item("cod_pais").ToString()
                vEmpresa = fr.Item("cod_empresa").ToString()
                vPuesto = fr.Item("cod_puesto").ToString()
                vFamilia = fr.Item("cod_familia").ToString()
            End If
            fr.Close()


            Eliminar()

            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "smf_Script", "responsive_grid();", True)

        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.PmsgBox("Ocurrió un error al disparar el evento SelectedIndexChanged. " & ex.Message, "error")

        End Try
    End Sub

    Protected Sub GridViewOne_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles GridViewOne.RowCancelingEdit
        Me.GridViewOne.EditIndex = -1
        Load_GridView()
    End Sub


    Protected Sub GridViewOne_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles GridViewOne.RowUpdating
        'ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "smf_Script", " alert(""Esto es un mensaje"");", True)

        Dim Codigo As String = e.Keys("codigo").ToString
        Dim comision As Decimal = e.NewValues("comision").ToString


        Dim activo As Integer

        Dim cb As CheckBox
        cb = TryCast(GridViewOne.Rows(e.RowIndex).FindControl("CheckActivo"), CheckBox)
        activo = cb.Checked

        Dim combo As DropDownList
        combo = TryCast(GridViewOne.Rows(e.RowIndex).FindControl("ddlVendedor"), DropDownList)
        Dim Vendedor As Integer = Convert.ToInt32(combo.SelectedValue)

        'DropDownList dp= (DropDownList )e.Row .FindControl ("DropDownList1");

        combo = TryCast(GridViewOne.Rows(e.RowIndex).FindControl("ddlPais"), DropDownList)
        Dim Pais As Integer = Convert.ToInt32(combo.SelectedValue)

        combo = TryCast(GridViewOne.Rows(e.RowIndex).FindControl("ddlEmpresa"), DropDownList)
        Dim Empresa As String = Convert.ToInt32(combo.SelectedValue)

        combo = TryCast(GridViewOne.Rows(e.RowIndex).FindControl("ddlPuesto"), DropDownList)
        Dim Puesto As Integer = Convert.ToInt32(combo.SelectedValue)

        combo = TryCast(GridViewOne.Rows(e.RowIndex).FindControl("ddlFamilia"), DropDownList)
        Dim Familia As String = Convert.ToInt32(combo.SelectedValue)



        Actualizar(Codigo, comision, activo, Vendedor, Pais, Empresa, Puesto, Familia)
        Me.GridViewOne.EditIndex = -1
        Load_GridView()

    End Sub

    Protected Sub GridViewOne_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles GridViewOne.RowEditing

        Try
            Me.GridViewOne.EditIndex = e.NewEditIndex
            Load_GridView()

            Dim id As String = Me.GridViewOne.DataKeys(e.NewEditIndex).Value
            Dim vactivo As Boolean

            Dim Sql As String

            ''" SET DATEFORMAT DMY " +
            Sql = "SELECT comision,CASE WHEN Activo = 1 THEN 'Si' ELSE 'No' END AS Activo, Cod_pais, cod_empresa, Cod_puesto, cod_familia, cod_vendedor FROM comis_VendFamilia WHERE cod_vendedor= " & id & " AND cod_pais = " & Session("cod_pais") & " "
            Dim fr As System.Data.OleDb.OleDbDataReader = DataBase.GetDataReader(Sql)
            If fr.Read() Then
                vVendedor = fr.Item("cod_vendedor").ToString()
                vPais = fr.Item("cod_pais").ToString()
                vEmpresa = fr.Item("cod_empresa").ToString()
                vPuesto = fr.Item("cod_puesto").ToString()
                vFamilia = fr.Item("cod_familia").ToString()
                vactivo = IIf(fr.Item("Activo").ToString() = "Si", True, False)
                'ckeckAc.Checked = IIf(fr.Item("Activo").ToString() = "Si", True, False)
                'Me.CheckActivo.Checked = IIf(fr.Item("Activo").ToString() = "Si", True, False)
                ' Me.TextComision.Text = fr.Item("comision").ToString()
            End If
            fr.Close()

            Dim ckeckAc As CheckBox = TryCast(GridViewOne.Rows(e.NewEditIndex).FindControl("CheckActivo"), CheckBox)
            If ckeckAc IsNot Nothing Then
                ckeckAc.Checked = vactivo
            End If

            Dim dataSetVendedor As New DataSet
            dataSetVendedor = DataBase.GetDataSet("SELECT cod_vendedor as codVendedor,RTRIM(ltrim(nombres))+RTRIM(ltrim(apellidos)) AS Nombres FROM vendedores  ")
            dtTabla = dataSetVendedor.Tables(0)

            Dim comboVendedor As DropDownList = TryCast(GridViewOne.Rows(e.NewEditIndex).FindControl("ddlVendedor"), DropDownList)
            If comboVendedor IsNot Nothing Then
                comboVendedor.DataSource = dataSetVendedor
                comboVendedor.DataTextField = "Nombres"
                comboVendedor.DataValueField = "codVendedor"
                comboVendedor.SelectedValue = vVendedor
                comboVendedor.DataBind()
            End If

            'Dim vPais As String
            'Sql = " SET DATEFORMAT DMY " + "SELECT cod_pais FROM comis_VendFamilia WHERE cod_vendedor= '" & id & "' "
            'Dim fr As System.Data.OleDb.OleDbDataReader = DataBase.GetDataReader(Sql)
            'If fr.Read() Then
            '    vPais = fr.Item("cod_pais").ToString()
            'End If


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

            'Dim vEmpresa As Integer
            'Sql = " SET DATEFORMAT DMY " + "SELECT cod_empresa FROM comis_VendFamilia WHERE cod_vendedor= '" & id & "' "
            'fr = DataBase.GetDataReader(Sql)
            'If fr.Read() Then
            '    vEmpresa = fr.Item("cod_empresa").ToString()
            'End If

            Dim dataSet As New DataSet
            dataSet = DataBase.GetDataSet("SELECT cod_empresa as codEmpresa,Descripcion AS Empresa FROM Empresas WHERE cod_pais = " & Session("cod_pais") & " ")
            dtTabla = dataSet.Tables(0)
            Dim combo As DropDownList = TryCast(GridViewOne.Rows(e.NewEditIndex).FindControl("ddlEmpresa"), DropDownList)
            If combo IsNot Nothing Then
                combo.DataSource = dataSet
                combo.DataTextField = "Empresa"
                combo.DataValueField = "codEmpresa"
                combo.SelectedValue = vEmpresa
                combo.DataBind()
            End If

            'Dim vPuesto As Integer
            'Sql = " SET DATEFORMAT DMY " + "SELECT cod_puesto FROM comis_VendFamilia WHERE cod_vendedor= '" & id & "' "
            'fr = DataBase.GetDataReader(Sql)
            'If fr.Read() Then
            '    vPuesto = fr.Item("cod_puesto").ToString()
            'End If

            Dim dataSetPuesto As New DataSet
            dataSetPuesto = DataBase.GetDataSet("SELECT cod_Puesto as codPuesto,descripcion as Puesto FROM  Puestos WHERE cod_pais = " & Session("cod_pais") & " ")
            dtTabla = dataSetPuesto.Tables(0)
            Dim comboCalidad As DropDownList = TryCast(GridViewOne.Rows(e.NewEditIndex).FindControl("ddlPuesto"), DropDownList)
            If comboCalidad IsNot Nothing Then
                comboCalidad.DataSource = dataSetPuesto
                comboCalidad.DataTextField = "Puesto"
                comboCalidad.DataValueField = "codPuesto"
                comboCalidad.SelectedValue = vPuesto
                comboCalidad.DataBind()
            End If

            'Dim vFamilia As Integer
            'Sql = " SET DATEFORMAT DMY " + "SELECT cod_familia FROM comis_VendFamilia pm WHERE cod_vendedor= '" & id & "' "
            'fr = DataBase.GetDataReader(Sql)
            'If fr.Read() Then
            '    vFamilia = fr.Item("cod_familia").ToString()
            'End If

            Dim dataSetFamilia As New DataSet
            dataSetFamilia = DataBase.GetDataSet("SELECT cod_familia as CodFamilia,descripcion as familia FROM  Familias_productos  ")
            dtTabla = dataSetFamilia.Tables(0)
            Dim comboFamilia As DropDownList = TryCast(GridViewOne.Rows(e.NewEditIndex).FindControl("ddlFamilia"), DropDownList)
            If comboFamilia IsNot Nothing Then
                comboFamilia.DataSource = dataSetFamilia
                comboFamilia.DataTextField = "familia"
                comboFamilia.DataValueField = "CodFamilia"
                comboFamilia.SelectedValue = vFamilia
                comboFamilia.DataBind()
            End If



        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.PmsgBox("Ocurrió un error al disparar el evento RowEditing SelectedIndexChanged. " & ex.Message, "error")

        End Try
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

    Protected Sub BtnGuardar_Click(sender As Object, e As EventArgs) Handles BtnGuardar.Click
        Dim MessegeText As String = String.Empty

        If Me.TextComision.Text = String.Empty Then
            MessegeText = "alertify.alert('El  proceso no puede continuar. Debe digitar Comision.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)
            Exit Sub
        End If


        If Me.ddlVendedor.SelectedIndex = -1 Then
            MessegeText = "alertify.alert('El  proceso no puede continuar. Debe escojer Vendedor');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)
            Exit Sub
        End If

        If Me.ddlPais.SelectedIndex = -1 Then
            MessegeText = "alertify.alert('El  proceso no puede continuar. Debe escojer Pais');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)
            Exit Sub
        End If

        If Me.ddlEmpresa.SelectedIndex = -1 Then
            MessegeText = "alertify.alert('El  proceso no puede continuar. Debe escojer Empresa');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)
            Exit Sub
        End If

        If Me.ddlPuesto.SelectedIndex = -1 Then
            MessegeText = "alertify.alert('El  proceso no puede continuar. Debe escojer puesto');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)
            Exit Sub
        End If

        If Me.ddlFamilia.SelectedIndex = -1 Then
            MessegeText = "alertify.alert('El  proceso no puede continuar. Debe escojer familia');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)
            Exit Sub
        End If

        Guardar()

        ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Facturacion", "close_popup();", True)
    End Sub

    Private Sub Nuevo()
        Try
            Me.TextComision.Text = String.Empty
            Me.ddlPais.SelectedValue = "Seleccione..."
            Me.ddlEmpresa.SelectedValue = "Seleccione..."
            Me.ddlPuesto.SelectedValue = "Seleccione..."
            Me.ddlFamilia.SelectedValue = "Seleccione..."
            Me.ddlVendedor.SelectedValue = "Seleccione..."
            Me.CheckActivo.Checked = False


        Catch ex As Exception
            Me.ltMensaje.Text = conn.PmsgBox(ex.Message, "error")
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
            sql &= "EXEC Cat_ComisVendedores @opcion=1," &
                   "@codigoPais =  " & Me.ddlPais.SelectedValue & " ," &
                   "@codigoEmp =  " & Me.ddlEmpresa.SelectedValue & " ," &
                   "@codigoPuesto =  '" & Me.ddlPuesto.SelectedValue & "' ," &
                   "@codigoFamilia =  " & Me.ddlFamilia.SelectedValue & " ," &
                   "@codigoVendedor =  " & Me.ddlVendedor.SelectedValue & " ," &
                   "@comision =  " & Me.TextComision.Text.Trim & " ," &
                   "@Activo =  '" & IIf(Me.CheckActivo.Checked = True, 1, 0) & "' ," &
                   "@BUSQUEDAD = '" & BUSQUEDAD & "' "



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
        Dim dbCon As New System.Data.OleDb.OleDbConnection(conn.conn)
        Try
            If dbCon.State = ConnectionState.Closed Then
                dbCon.Open()
            End If

            Dim sql As String = "SET DATEFORMAT DMY " & vbCrLf
            sql &= "EXEC Cat_ComisVendedores @opcion=2," &
                  "@codigoPais =  " & vPais & " ," &
                  "@codigoEmp =  " & vEmpresa & " ," &
                  "@codigoPuesto =  '" & vPuesto & "' ," &
                  "@codigoFamilia =  " & vFamilia & " ," &
                  "@codigoVendedor =  " & Me.hdfCodigo.Value & " ," &
                  "@comision =  0 ," &
                  "@Activo =  '" & IIf(Me.CheckActivo.Checked = True, 1, 0) & "' ," &
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

    Private Sub Actualizar(Codigo As String, comision As Decimal, activo As Integer, Vendedor As Integer, Pais As Integer, Empresa As Integer, Puesto As Integer, Familia As Integer)
        Dim MessegeText As String = String.Empty
        Dim dbCon As New System.Data.OleDb.OleDbConnection(conn.conn)
        Try
            If dbCon.State = ConnectionState.Closed Then
                dbCon.Open()
            End If
            '--- "@codigo =  '" & Codigo & "' ," & _
            Dim sql As String = "SET DATEFORMAT DMY " & vbCrLf
            sql &= "EXEC Cat_ComisVendedores @opcion=1," &
                  "@codigoPais =  " & Pais & " ," &
                  "@codigoEmp =  " & Empresa & " ," &
                  "@codigoPuesto =  " & Puesto & " ," &
                  "@codigoFamilia =  " & Familia & " ," &
                  "@codigoVendedor =  " & Codigo & " ," &
                  "@comision =  " & comision & " ," &
                  "@Activo =  '" & IIf(Me.CheckActivo.Checked = True, 1, 0) & "' ," &
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
#End Region

#Region "EXPORTAR DATOS A EXCELL"

    Protected Sub btnExportar_Click(sender As Object, e As EventArgs) Handles btnExportar.Click
        Try
            Response.Clear()
            Response.Buffer = True

            Dim name_doc = "catalogo_ComisionVendedores" & Date.Now.Year & Date.Now.Month & Date.Now.Day

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
            MyHTML &= "<h2>Catalogo de comision de vendedores <br/> al dia " & Date.Now.Day & "/" & Date.Now.Month & "/" & Date.Now.Year & "</h2>"

            If Not dtTabla Is Nothing Then
                MyHTML &= "<table cellspacing=""0"" cellpadding=""0"">" &
                          "<tbody>" &
                          "<tr>" &
                          "<th></th>" &
                          "<th>codigo</th>" &
                          "<th>Nombres</th>" &
                          "<th>Apellidos</th>" &
                          "<th>Pais</th>" &
                          "<th>Empresa</th>" &
                          "<th>Puesto</th>" &
                          "<th>Familia</th>" &
                          "<th>comision</th>" &
                          "<th>activo</th>" &
                          "</tr>"

                Dim clase As String = String.Empty

                For i As Integer = 0 To dtTabla.Rows.Count - 1
                    MyHTML &= "<tr" & clase & ">" &
                              "<td class=""first"">" & i + 1 & "</td>" &
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("Codigo").ToString.Trim & "</td>" &
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("Nombres").ToString.Trim & "</td>" &
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("Apellidos").ToString.Trim & "</td>" &
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("Pais").ToString.Trim & "</td>" &
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("Empresa").ToString.Trim & "</td>" &
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("Puesto").ToString.Trim & "</td>" &
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("Familia").ToString.Trim & "</td>" &
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("Comision").ToString.Trim & "</td>" &
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("activo").ToString.Trim & "</td>" &
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

    Private Sub GridViewOne_SelectedIndexChanged(sender As Object, e As EventArgs) Handles GridViewOne.SelectedIndexChanged

    End Sub

#End Region


End Class
