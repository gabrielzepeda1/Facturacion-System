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

Partial Class catalogos_ProductosMaestro
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
            Me.TextCodProducto.Attributes.Add("placeholder", "Digite Codigo")
            Me.TextCodProducto.Attributes.Add("requerid", "requerid")
            Me.txtDescripcion.Attributes.Add("placeholder", "Digite Descripción")
            Me.txtDescripcion.Attributes.Add("requerid", "requerid")
            Me.txtNumProd.Attributes.Add("placeholder", "Digite numero del producto")
            Me.txtNumProd.Attributes.Add("requerid", "requerid")

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
            Dim SQL As String = String.Empty
            SQL &= "EXEC Cat_ProductosMaestro @opcion=3," & _
                  "@codigo =  0  ," & _
                  "@numproducto =  0  ," & _
                  "@descripcion = 0  ," & _
                  "@sigla = 0  ," & _
                  "@codorigen = 0  ," & _
                  "@codcalidad = 0  ," & _
                  "@codpresentacion = 0  ," & _
                  "@codfamilia = 0  ," & _
                  "@codusuario =  NULL ," & _
                  "@codusuarioUlt =  NULL  ," & _
                  "@produccionSm =  0  ," & _
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
            Dim vsigla As String
            Dim fr As System.Data.OleDb.OleDbDataReader

            'Sql = " SET DATEFORMAT DMY " + "SELECT sigla,CASE WHEN produccion_sm = 1 THEN 'Si' ELSE 'No' END AS produccion_sm FROM Productos_Maestro pm WHERE RTRIM(ltrim(pm.cod_producto))= '" & id & "' "
            'Dim fr As System.Data.OleDb.OleDbDataReader = DataBase.GetDataReader(Sql)
            'If fr.Read() Then
            '    vsigla = fr.Item("sigla").ToString()
            '    vactivo = IIf(fr.Item("produccion_sm").ToString() = "Si", True, False)
            'End If




            Dim dataSetSigla As New DataSet
            dataSetSigla = DataBase.GetDataSet("SELECT sigla FROM Siglas  ")
            dtTabla = dataSetSigla.Tables(0)
            Dim comboSigla As DropDownList = TryCast(GridViewOne.Rows(e.NewEditIndex).FindControl("ddlSigla"), DropDownList)
            If comboSigla IsNot Nothing Then
                comboSigla.DataSource = dataSetSigla
                comboSigla.DataTextField = "sigla"
                comboSigla.DataValueField = "sigla"
                comboSigla.SelectedValue = vsigla
                comboSigla.DataBind()
            End If

            Dim vorigen As Integer
            Sql = " SET DATEFORMAT DMY " + "SELECT cod_origen FROM Productos_Maestro pm WHERE RTRIM(ltrim(pm.cod_producto))= '" & id & "' "
            fr = DataBase.GetDataReader(Sql)
            If fr.Read() Then
                vorigen = fr.Item("cod_origen").ToString()
            End If

            Dim dataSet As New DataSet
            dataSet = DataBase.GetDataSet("SELECT cod_origen as codOrigen,Descripcion AS DesOrigen FROM  Origenes_productos  ")
            dtTabla = dataSet.Tables(0)
            Dim combo As DropDownList = TryCast(GridViewOne.Rows(e.NewEditIndex).FindControl("ddlOrigen"), DropDownList)
            If combo IsNot Nothing Then
                combo.DataSource = dataSet
                combo.DataTextField = "DesOrigen"
                combo.DataValueField = "codOrigen"
                combo.SelectedValue = vorigen
                combo.DataBind()
            End If

            Dim vcalidad As Integer
            Sql = " SET DATEFORMAT DMY " + "SELECT cod_calidad FROM Productos_Maestro pm WHERE RTRIM(ltrim(pm.cod_producto))= '" & id & "' "
            fr = DataBase.GetDataReader(Sql)
            If fr.Read() Then
                vcalidad = fr.Item("cod_calidad").ToString()
            End If

            Dim dataSetCalidad As New DataSet
            dataSetCalidad = DataBase.GetDataSet("SELECT cod_calidad as CodCalidad,descripcion as calidad FROM  Calidades_productos  ")
            dtTabla = dataSetCalidad.Tables(0)
            Dim comboCalidad As DropDownList = TryCast(GridViewOne.Rows(e.NewEditIndex).FindControl("ddlCalidad"), DropDownList)
            If comboCalidad IsNot Nothing Then
                comboCalidad.DataSource = dataSetCalidad
                comboCalidad.DataTextField = "calidad"
                comboCalidad.DataValueField = "CodCalidad"
                comboCalidad.SelectedValue = vcalidad
                comboCalidad.DataBind()
            End If

            Dim vPresentacion As Integer
            Sql = " SET DATEFORMAT DMY " + "SELECT cod_presentacion FROM Productos_Maestro pm WHERE RTRIM(ltrim(pm.cod_producto))= '" & id & "' "
            fr = DataBase.GetDataReader(Sql)
            If fr.Read() Then
                vPresentacion = fr.Item("cod_presentacion").ToString()
            End If
            Dim dataSetPresentacion As New DataSet
            dataSetPresentacion = DataBase.GetDataSet("SELECT cod_presentacion as CodPresentacion,descripcion as presentacion FROM  Presentaciones_productos  ")
            dtTabla = dataSetPresentacion.Tables(0)
            Dim comboPresent As DropDownList = TryCast(GridViewOne.Rows(e.NewEditIndex).FindControl("ddlPresentacion"), DropDownList)
            If comboPresent IsNot Nothing Then
                comboPresent.DataSource = dataSetPresentacion
                comboPresent.DataTextField = "presentacion"
                comboPresent.DataValueField = "CodPresentacion"
                comboPresent.SelectedValue = vPresentacion
                comboPresent.DataBind()
            End If


            Dim vFamilia As Integer
            Sql = " SET DATEFORMAT DMY " + "SELECT cod_familia FROM Productos_Maestro pm WHERE RTRIM(ltrim(pm.cod_producto))= '" & id & "' "
            fr = DataBase.GetDataReader(Sql)
            If fr.Read() Then
                vFamilia = fr.Item("cod_familia").ToString()
            End If

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

            Dim vactivo As Boolean
            Dim ckecksanm As CheckBox = TryCast(GridViewOne.Rows(e.NewEditIndex).FindControl("CheckSm"), CheckBox)
            If ckecksanm IsNot Nothing Then
                ckecksanm.Checked = vactivo
            End If



        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.pmsgBox("Ocurrió un error al disparar el evento RowEditing SelectedIndexChanged. " & ex.Message, "error")

        End Try
    End Sub

    Protected Sub GridViewOne_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles GridViewOne.RowUpdating
        'ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "smf_Script", " alert(""Esto es un mensaje"");", True)

        Dim Codigo As String = e.Keys("codigo").ToString
        Dim Descripcion As String = e.NewValues("producto").ToString
        Dim Numero As String = e.NewValues("num_producto").ToString
        Dim prodSm As Boolean
        'If e.NewValues("produccion_sm").ToString = "SI" Then
        '    prodSm = 1
        'Else
        '    prodSm = 0
        'End If




        Dim cb As CheckBox
        cb = TryCast(GridViewOne.Rows(e.RowIndex).FindControl("CheckSm"), CheckBox)
        prodSm = cb.Checked



        Dim combo As DropDownList = TryCast(GridViewOne.Rows(e.RowIndex).FindControl("ddlOrigen"), DropDownList)
        Dim origen As Integer = Convert.ToInt32(combo.SelectedValue)

        combo = TryCast(GridViewOne.Rows(e.RowIndex).FindControl("ddlCalidad"), DropDownList)
        Dim calidad As String = Convert.ToInt32(combo.SelectedValue)

        combo = TryCast(GridViewOne.Rows(e.RowIndex).FindControl("ddlPresentacion"), DropDownList)
        Dim presentacion As String = Convert.ToInt32(combo.SelectedValue)

        combo = TryCast(GridViewOne.Rows(e.RowIndex).FindControl("ddlFamilia"), DropDownList)
        Dim familia As String = Convert.ToInt32(combo.SelectedValue)

        combo = TryCast(GridViewOne.Rows(e.RowIndex).FindControl("ddlSigla"), DropDownList)
        Dim Sigla As String = combo.SelectedValue



        Actualizar(Codigo, Descripcion, Numero, Sigla, origen, calidad, presentacion, familia, prodSm)
        Me.GridViewOne.EditIndex = -1
        Load_GridView()




        'Dim id As Integer = Convert.ToInt32(GridView1.DataKeys(e.RowIndex).Value)
        'Dim combo As DropDownList = TryCast(GridView1.Rows(e.RowIndex).FindControl("ddlPaises"), DropDownList)
        'Dim pais As Integer = Convert.ToInt32(combo.SelectedValue)
        'Dim text As TextBox = TryCast(GridView1.Rows(e.RowIndex).Cells(1).Controls(0), TextBox)
        'Dim nombre As String = text.Text

        'DataAccess.UpdateUsuario(id, nombre, pais)

        'GridView1.EditIndex = -1
        'BindData()


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

        If Me.txtDescripcion.Text = String.Empty Then
            MessegeText = "alertify.alert('El  proceso no puede continuar. Debe digitar descripción.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)
            Exit Sub
        End If

        If Me.txtNumProd.Text = String.Empty Then
            MessegeText = "alertify.alert('El  proceso no puede continuar. Debe digitar numero del producto.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)
            Exit Sub
        End If

        If Me.ddlSigla.SelectedIndex = -1 Then
            MessegeText = "alertify.alert('El  proceso no puede continuar. Debe escojer sigla para el producto.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)
            Exit Sub
        End If

        If Me.ddlOrigen.SelectedIndex = -1 Then
            MessegeText = "alertify.alert('El  proceso no puede continuar. Debe escojer origen para el producto.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)
            Exit Sub
        End If

        If Me.ddlCalidad.SelectedIndex = -1 Then
            MessegeText = "alertify.alert('El  proceso no puede continuar. Debe escojer calidad para el producto.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)
            Exit Sub
        End If

        If Me.ddlPresentacion.SelectedIndex = -1 Then
            MessegeText = "alertify.alert('El  proceso no puede continuar. Debe escojer presentacion para el producto.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)
            Exit Sub
        End If

        If Me.ddlFamilia.SelectedIndex = -1 Then
            MessegeText = "alertify.alert('El  proceso no puede continuar. Debe escojer familia para el producto.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)
            Exit Sub
        End If

        Guardar()

        ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Facturacion", "close_popup();", True)

    End Sub
    Private Sub Nuevo()
        Try
            Me.txtDescripcion.Text = String.Empty
            txtDescripcion.Text = ""
            txtDescripcion.Text.Equals("")
            Me.TextCodProducto.Text = String.Empty
            Me.txtNumProd.Text = String.Empty
            Me.ddlSigla.ClearSelection()
            Me.ddlOrigen.ClearSelection()
            Me.ddlCalidad.ClearSelection()
            Me.ddlPresentacion.ClearSelection()
            Me.ddlFamilia.ClearSelection()
            Me.CheckSm.Checked = False
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
            sql &= "EXEC Cat_ProductosMaestro @opcion=1," & _
                   "@codigo =  " & Me.TextCodProducto.Text.Trim & " ," & _
                   "@numproducto =  " & Me.txtNumProd.Text.Trim & " ," & _
                   "@descripcion =  '" & Me.txtDescripcion.Text.Trim & "' ," & _
                   "@SIGLA =  " & Me.ddlSigla.SelectedValue & " ," & _
                   "@codorigen =  " & Me.ddlOrigen.SelectedValue & " ," & _
                   "@codcalidad =  " & Me.ddlCalidad.SelectedValue & " ," & _
                   "@codpresentacion =  " & Me.ddlPresentacion.SelectedValue & " ," & _
                   "@codfamilia =  " & Me.ddlFamilia.SelectedValue & " ," & _
                   "@codusuario =  " & Request.Cookies("CKSMFACTURA")("cod_usuario") & " ," & _
                   "@codusuarioUlt =  " & Request.Cookies("CKSMFACTURA")("cod_usuario") & " ," & _
                   "@produccionSm =  '" & IIf(Me.CheckSm.Checked = True, 1, 0) & "' ," & _
                   "@BUSQUEDAD = '0'  "

            'Dim Sigla As String = CheckSme.NewValues("sigla").ToString
            'Dim origen As String = e.NewValues("CodOrigen").ToString
            'Dim calidad As String = e.NewValues("CodCalidad").ToString
            'Dim presentacion As String = e.NewValues("CodPresentacion").ToString
            'Dim familia As String = e.NewValues("CodFamilia").ToString
            Me.TextCodProducto.Text = String.Empty

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
            sql &= "EXEC Cat_ProductosMaestro @opcion=2," &
                  "@codigo =  " & Me.hdfCodigo.Value & " ," &
                  "@numproducto =  '" & Me.txtNumProd.Text.Trim & "' ," &
                  "@descripcion =  '" & Me.txtDescripcion.Text.Trim & "' ," &
                  "@SIGLA =  '" & Me.txtDescripcion.Text.Trim & "' ," &
                  "@codorigen =  '" & Me.txtDescripcion.Text.Trim & "' ," &
                  "@codcalidad =  '" & Me.txtDescripcion.Text.Trim & "' ," &
                  "@codpresentacion =  '" & Me.txtDescripcion.Text.Trim & "' ," &
                  "@codfamilia =  '" & Me.txtDescripcion.Text.Trim & "' ," &
                  "@codusuario =   " & Request.Cookies("CKSMFACTURA")("cod_usuario") & " ," &
                  "@codusuarioUlt =  " & Request.Cookies("CKSMFACTURA")("cod_usuario") & " ," &
                  "@produccionSm =  '" & IIf(Me.CheckSm.Checked = True, 1, 0) & "' ," &
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

    Private Sub Actualizar(Codigo As String, Descripcio As String, Numero As Integer, Sigla As String, origen As Integer, calidad As Integer, presentacion As Integer, familia As Integer, prodSm As Boolean)
        Dim MessegeText As String = String.Empty
        Dim dbCon As New System.Data.OleDb.OleDbConnection(conn.conn)
        Try
            If dbCon.State = ConnectionState.Closed Then
                dbCon.Open()
            End If
            '" & IIf(Me.CheckSm.Checked = True, 1, 0) & "' ," & _
            Dim sql As String = "SET DATEFORMAT DMY " & vbCrLf
            sql &= "EXEC Cat_ProductosMaestro @opcion=1," & _
                  "@codigo =  '" & Codigo & "' ," & _
                  "@numproducto =  " & Numero & " ," & _
                  "@descripcion =  '" & Descripcio & "' ," & _
                  "@SIGLA =  '" & Sigla & "' ," & _
                  "@codorigen =  " & origen & " ," & _
                  "@codcalidad =  " & calidad & " ," & _
                  "@codpresentacion =  " & presentacion & " ," & _
                  "@codfamilia =  " & familia & " ," & _
                  "@codusuario =   " & Request.Cookies("CKSMFACTURA")("cod_usuario") & " ," & _
                  "@codusuarioUlt =  " & Request.Cookies("CKSMFACTURA")("cod_usuario") & " ," & _
                  "@produccionSm =  '" & prodSm & "' ," & _
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
            sql &= "EXEC Cat_ProductosMaestro @opcion=4," & _
                   "@codigo =  " & Me.hdfCodigo.Value & "," & _
                   "@numproducto =  0," & _
                   "@descripcion =  '0' ," & _
                   "@SIGLA =  '0'," & _
                   "@codorigen =  0 ," & _
                   "@codcalidad =  0 ," & _
                   "@codpresentacion =  0 ," & _
                   "@codfamilia = 0 ," & _
                   "@codusuario =  " & Request.Cookies("CKSMFACTURA")("cod_usuario") & " ," & _
                   "@codusuarioUlt =  " & Request.Cookies("CKSMFACTURA")("cod_usuario") & " ," & _
                   "@produccionSm =  0 ," & _
                   "@BUSQUEDAD = '0'  "
            Dim dr As System.Data.OleDb.OleDbDataReader = DataBase.GetDataReader(sql)


            If dr.Read() Then
                Me.TextCodProducto.Text = dr.Item("cod_producto").ToString
                Me.txtDescripcion.Text = dr.Item("descripcion").ToString
                Me.txtNumProd.Text = dr.Item("num_producto").ToString
                Me.CheckSm.Checked = IIf(dr.Item("produccion_sm").ToString() = "Si", True, False)
                Me.CCsigla.SelectedValue = dr.Item("sigla").ToString
                Me.CCOrigen.SelectedValue = dr.Item("cod_origen").ToString
                Me.CCCalidad.SelectedValue = dr.Item("cod_calidad").ToString
                Me.CCPresent.SelectedValue = dr.Item("cod_presentacion").ToString
                Me.CCFamilia.SelectedValue = dr.Item("cod_familia").ToString
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


#Region "EXPORTAR DATOS A EXCELL"

    Protected Sub btnExportar_Click(sender As Object, e As EventArgs) Handles btnExportar.Click
        Try
            Response.Clear()
            Response.Buffer = True

            Dim name_doc = "catalogo_productosMaestros" & Date.Now.Year & Date.Now.Month & Date.Now.Day

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
            MyHTML &= "<h2>Catalogo de productos maestros <br/> al dia " & Date.Now.Day & "/" & Date.Now.Month & "/" & Date.Now.Year & "</h2>"

            If Not dtTabla Is Nothing Then
                MyHTML &= "<table cellspacing=""0"" cellpadding=""0"">" & _
                          "<tbody>" & _
                          "<tr>" & _
                          "<th></th>" & _
                          "<th>codigoProducto</th>" & _
                          "<th>numero</th>" & _
                          "<th>DescripcionProducto</th>" & _
                          "<th>sigla</th>" & _
                          "<th>origen</th>" & _
                          "<th>calidad</th>" & _
                          "<th>presentacion</th>" & _
                          "<th>familia</th>" & _
                          "</tr>"



                Dim clase As String = String.Empty

                For i As Integer = 0 To dtTabla.Rows.Count - 1
                    MyHTML &= "<tr" & clase & ">" & _
                              "<td class=""first"">" & i + 1 & "</td>" & _
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("Codigo").ToString.Trim & "</td>" & _
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("num_producto").ToString.Trim & "</td>" & _
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("producto").ToString.Trim & "</td>" & _
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("sigla").ToString.Trim & "</td>" & _
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("origen").ToString.Trim & "</td>" & _
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("calidad").ToString.Trim & "</td>" & _
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("presentacion").ToString.Trim & "</td>" & _
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("familia").ToString.Trim & "</td>" & _
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("produccion_sm").ToString.Trim & "</td>" & _
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

    Private Sub DropDownList(p1 As Object)
        Throw New NotImplementedException
    End Sub


End Class
