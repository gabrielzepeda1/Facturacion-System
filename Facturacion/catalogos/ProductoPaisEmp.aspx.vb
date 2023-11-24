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


Partial Class catalogos_ProductoPaisEmp
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
            System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("es-ES")
            Puesto_Name = "Industrial Comercial San Martin"
           

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
           
            SQL &= "EXEC Cat_ProductosXPais @opcion=4," & _
                  "@codPais =  0  ," & _
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

    Protected Sub GridViewOne_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridViewOne.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes.Add("OnClick", "" & Page.ClientScript.GetPostBackClientHyperlink(Me.GridViewOne, "Select$" + e.Row.RowIndex.ToString) & ";")
        End If
    End Sub

    Protected Sub GridViewOne_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridViewOne.SelectedIndexChanged

        Me.hdfCodigo.Value = Me.GridViewOne.SelectedValue.ToString
        Session("Codigo") = Me.GridViewOne.SelectedRow.Cells(0).Text
        Session("Descrip") = Me.GridViewOne.SelectedRow.Cells(1).Text
        'Session("Numero") = Me.GridViewOne.SelectedRow.Cells(2).Text
        'Me.TextCodProducto.Text = "Producto Maestro: " + Session("Codigo") + " " + Session("Descrip")
        'Response.Redirect("ProductoPais.aspx")
        LblProd.Text = Session("Codigo") + "" + Session("Descrip")
        ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "sm_script", "open_popup();", True)


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

    Protected Sub GridViewOne_RowDeleted(sender As Object, e As GridViewDeletedEventArgs) Handles GridViewOne.RowDeleted

    End Sub
#End Region
    Protected Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        Response.Redirect("ProductoPais.aspx")
    End Sub
    'Protected Sub PonerCodig_Click(sender As Object, e As EventArgs) Handles PonerCodig.Click
    '    Me.TextCodProducto.Text = "Producto Maestro: " + Session("Codigo") + " " + Session("Descrip")
    'End Sub
#Region "ENVIAR INFORMACIÓN HACIA LA BASE DE DATOS"
    Protected Sub BtnGuardar_Click(sender As Object, e As EventArgs) Handles BtnGuardar.Click
        Dim MessegeText As String = String.Empty

        If Me.TextDecImpri.Text = String.Empty Then
            MessegeText = "alertify.alert('El  proceso no puede continuar. Debe digitar descripción de impresion');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)
            Exit Sub
        End If

    
        If Me.ddlUnidadMed.SelectedIndex = -1 Then
            MessegeText = "alertify.alert('El  proceso no puede continuar. Debe escojer Unidad de medida.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)
            Exit Sub
        End If

        Guardar()
    End Sub

    Private Sub Guardar()
        Dim MessegeText As String = String.Empty

        Dim dbCon As New System.Data.OleDb.OleDbConnection(conn.conn)
        Try
            If dbCon.State = ConnectionState.Closed Then
                dbCon.Open()
            End If

            Dim sql As String = "SET DATEFORMAT DMY " & vbCrLf
            sql &= "EXEC Cat_ProductosXPais @opcion=1," & _
                   "@codPais =  " & Request.Cookies("CKSMFACTURA")("codPais") & " ," & _
                   "@codigoProducto = '" & Session("Codigo") & "' ," & _
                   "@codUndMedida =  " & Me.ddlUnidadMed.SelectedValue & " ," & _
                   "@descripcion =  '" & Session("Descrip") & "' ," & _
                   "@descripImpr =  '" & Me.TextDecImpri.Text.Trim & "' ," & _
                   "@numProducto =  '" & Session("Numero") & "' ," & _
                   "@activo = '" & IIf(Me.CheckAt.Checked = True, 1, 0) & "' ," & _
                   "@ExcentoImp = '" & IIf(Me.CheckImtos.Checked = True, 1, 0) & "' ," & _
                   "@codEmpresa =  " & Request.Cookies("CKSMFACTURA")("CodEmpresa") & " ," & _
                   "@codusuario =  " & Request.Cookies("CKSMFACTURA")("cod_usuario") & " ," & _
                   "@codusuarioUlt =  " & Request.Cookies("CKSMFACTURA")("cod_usuario") & " ," & _
                   "@BUSQUEDAD = '0'  "


            Dim cmd As New OleDb.OleDbCommand(sql, dbCon)
            cmd.ExecuteNonQuery()
            BUSQUEDAD = "0"
            Load_GridView()

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

End Class
