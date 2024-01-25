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

Imports SM.Entity
Imports SM.Business


Partial Class catalogos_Clientes


    Inherits System.Web.UI.Page
    Dim conn As New FACTURACION_CLASS.seguridad
    Dim DataBase As New FACTURACION_CLASS.database
    Dim BUSQUEDAD As String
    Dim vexterno As String
    Dim vcodEmpresa As String

    Dim clienteBL As ClienteBL = New ClienteBL()

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

            'Form.DefaultButton = Me.btnLoadClientes.UniqueID

            'Puesto_Name = "Industrial Comercial San Martin"
            ''Me.TextCodCliente.Attributes.Add("placeholder", "Digite Codigo cliente")
            'Me.TextCodCliente.Attributes.Add("requerid", "requerid")
            ''Me.TextNombre.Attributes.Add("placeholder", "Digite Nombre del cliente")
            'Me.TextNombre.Attributes.Add("requerid", "requerid")
            ''Me.TextApellido.Attributes.Add("placeholder", "Digite Apellido del cliente")
            'Me.TextApellido.Attributes.Add("requerid", "requerid")
            ''Me.TextNombComer.Attributes.Add("placeholder", "Digite Nombre Comercial del cliente")
            ''Me.TextNombComer.Attributes.Add("requerid", "requerid")
            ''Me.TextRazonSoc.Attributes.Add("placeholder", "Digite Razon Social del cliente")
            'Me.TextRazonSoc.Attributes.Add("requerid", "requerid")
            ''Me.TextDirecc.Attributes.Add("placeholder", "Digite Direccion del cliente")
            'Me.TextDirecc.Attributes.Add("requerid", "requerid")
            ''Me.TextTelf.Attributes.Add("placeholder", "Digite Telefono del cliente")
            'Me.TextTelf.Attributes.Add("requerid", "requerid")
            ''Me.Textemail.Attributes.Add("placeholder", "Digite Correo del cliente")
            'Me.Textemail.Attributes.Add("requerid", "requerid")
            ''Me.TextContacto.Attributes.Add("placeholder", "Digite Contacto del cliente")
            'Me.TextContacto.Attributes.Add("requerid", "requerid")
            ''Me.TextCeduRuc.Attributes.Add("placeholder", "Digite Cedula del cliente")
            'Me.TextCeduRuc.Attributes.Add("requerid", "requerid")
            ''Me.TextCtaCont.Attributes.Add("placeholder", "Digite Cuenta contable del cliente")
            'Me.TextCtaCont.Attributes.Add("requerid", "requerid")
            ''Me.TextDiasCred.Attributes.Add("placeholder", "Digite Días de crédito del cliente")
            'Me.TextDiasCred.Attributes.Add("requerid", "requerid")

            'Me.TextDiasCred.Attributes.Add("min", "0")
            'Me.TextDiasCred.Attributes.Add("max", "1000")

            ''Me.TextLimtCred.Attributes.Add("placeholder", "Digite Limite de crédito del cliente")
            'Me.TextLimtCred.Attributes.Add("requerid", "requerid")
            Load_GridView()
        End If
    End Sub

#Region "PROCESOS Y EVENTOS DEL GRIDVIEW"
    Protected Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        Load_GridView()

        ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "smf_Script", "responsive_grid();", True)
    End Sub

    Private Sub MostrarClientes()

        Dim lista As List(Of Cliente) = clienteBL.Lista()

        GridViewOne.DataSource = lista
        GridViewOne.DataBind()
    End Sub



    Private Sub Load_GridView()
        Try

            Dim SQL As String = String.Empty
            SQL &= "EXEC Cat_Clientes @opcion=3," &
                  "@codigoPais =  " & Session("cod_pais") & "," &
                  "@razonSocial =  0  ," &
                  "@direccion = 0  ," &
                  "@telefono = 0  ," &
                  "@Email =  0  ," &
                  "@Contacto = 0  ," &
                  "@CedulaRuc = 0  ," &
                  "@NombreComercial =  0  ," &
                  "@codCliente = 0  ," &
                  "@Nombres = 0  ," &
                  "@Apellidos = 0  ," &
                  "@codusuario = 0  ," &
                  "@codusuarioUlt = 0  ," &
                  "@DiasCredito = 0  ," &
                  "@LimiteCredito = 0  ," &
                  "@CodSectorMercado = 0  ," &
                  "@CodVendedor = 0  ," &
                  "@Activo = 0  ," &
                  "@CtaContable = 0  ," &
                  "@externo = 0  ," &
                  "@excentoImp = 0  ," &
                  "@esDistribuidora = 0  ," &
                  "@codempresa = " & Session("cod_empresa") & "," &
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
                    pageLabel.Text = "&nbsp;&nbsp; Pagina " & currentPage.ToString() &
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

    Protected Sub GridViewOne_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles GridViewOne.RowDeleting
        Try
            'pasar la información del Gridview hacia otro control, en este caso el control HiddenField,
            Me.hdfCodigo.Value = Me.GridViewOne.DataKeys(e.RowIndex).Value

            vexterno = GridViewOne.DataKeys(e.RowIndex).Item(1).ToString()
            vcodEmpresa = GridViewOne.DataKeys(e.RowIndex).Item(2).ToString()


            'Dim Sql As String
            'Sql = "SELECT  comision,CASE WHEN Activo = 1 THEN 'Si' ELSE 'No' END AS Activo,Cod_pais," & _
            '               "cod_empresa,Cod_puesto,cod_familia,cod_vendedor FROM comis_VendFamilia " & _
            '               "WHERE cod_vendedor= " & Me.hdfCodigo.Value & " and cod_pais=" & vPais & " and " & _
            '                     "cod_empresa= " & vEmpresa & " and cod_puesto=" & vPuesto & " and cod_familia= " & vFamilia & ""
            'Dim fr As System.Data.OleDb.OleDbDataReader = DataBase.GetDataReader(Sql)
            'If fr.Read() Then
            '    'vVendedor = fr.Item("cod_vendedor").ToString()
            '    vPais = fr.Item("cod_pais").ToString()
            '    vEmpresa = fr.Item("cod_empresa").ToString()
            '    vPuesto = fr.Item("cod_puesto").ToString()
            '    vFamilia = fr.Item("cod_familia").ToString()
            'End If
            'fr.Close()


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


    Protected Sub GridViewOne_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles GridViewOne.RowUpdating
        'ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "smf_Script", " alert(""Esto es un mensaje"");", True)
        'SELECT s.cod_pais,Pais,Vendedor,Activo,cod_empresa,Empresa
        's.cod_sector_mercado,m.descripcion AS Mercado,s.cod_vendedor,
        Dim Codigo As String = e.Keys("codigo").ToString
        Dim Nombre As String = e.NewValues("nombres").ToString
        Dim apell As String = e.NewValues("apellidos").ToString
        Dim razRuc As String = e.NewValues("razon_social").ToString
        Dim direc As String = e.NewValues("direccion").ToString
        Dim tele As String = e.NewValues("telefono").ToString
        Dim Email As String = e.NewValues("email").ToString
        Dim contacto As String = e.NewValues("contacto").ToString
        Dim Ced As String = e.NewValues("cedula_ruc").ToString
        Dim NomCome As String = e.NewValues("nombre_comercial").ToString
        Dim DiasCre As String = e.NewValues("dias_credito").ToString
        Dim LimtCre As Decimal = e.NewValues("limite_credito").ToString
        Dim ctacontable As String = e.NewValues("cta_contable").ToString
        'Dim excenIm As Integer = e.NewValues("excento_imp").ToString
        'Dim esDist As Integer = e.NewValues("es_distribuidora").ToString
        'Dim Exter As Integer = e.NewValues("externo").ToString
        'Dim activo As String = e.NewValues("Activo").ToString
        Dim activo As String
        Dim excenIm As Integer
        Dim esDist As Integer
        Dim Exter As Integer



        'Dim comision As Decimal = e.NewValues("comision").ToString

        Dim cb As CheckBox
        cb = TryCast(GridViewOne.Rows(e.RowIndex).FindControl("CheckActivo"), CheckBox)
        activo = cb.Checked

        cb = TryCast(GridViewOne.Rows(e.RowIndex).FindControl("CheckExterno"), CheckBox)
        Exter = cb.Checked

        cb = TryCast(GridViewOne.Rows(e.RowIndex).FindControl("CheckExCenImp"), CheckBox)
        excenIm = cb.Checked

        cb = TryCast(GridViewOne.Rows(e.RowIndex).FindControl("CheckDist"), CheckBox)
        esDist = cb.Checked


        Dim combo As DropDownList
        combo = TryCast(GridViewOne.Rows(e.RowIndex).FindControl("ddlMercado"), DropDownList)
        Dim merca As Integer = Convert.ToInt32(combo.SelectedValue)

        combo = TryCast(GridViewOne.Rows(e.RowIndex).FindControl("ddlVendedor"), DropDownList)
        Dim vend As String = Convert.ToInt32(combo.SelectedValue)


        'combo = TryCast(GridViewOne.Rows(e.RowIndex).FindControl("ddlPais"), DropDownList)
        'Dim Pais As Integer = Convert.ToInt32(combo.SelectedValue)

        'combo = TryCast(GridViewOne.Rows(e.RowIndex).FindControl("ddlEmpresa"), DropDownList)
        'Dim Empresa As String = Convert.ToInt32(combo.SelectedValue)



        Actualizar(Codigo, Nombre, apell, razRuc, direc, tele, Email, contacto, Ced, NomCome, DiasCre, LimtCre, activo, ctacontable, excenIm, esDist, Exter, merca, vend)
        Me.GridViewOne.EditIndex = -1
        Load_GridView()

    End Sub

    Protected Sub GridViewOne_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles GridViewOne.RowEditing

        Try
            Me.GridViewOne.EditIndex = e.NewEditIndex
            Load_GridView()

            Dim vEmpresa As Integer
            Dim vert As Integer
            Dim id As String
            id = Me.GridViewOne.DataKeys(e.NewEditIndex).Value
            vert = IIf(GridViewOne.DataKeys(e.NewEditIndex).Item(1).ToString() = "Si", 1, 0)
            vEmpresa = GridViewOne.DataKeys(e.NewEditIndex).Item(2).ToString()

            Dim vPais As String
            Dim vSectMerca As Integer
            Dim vFamilia As Integer
            Dim vVendedor As String
            Dim vactivo As Boolean
            Dim vExcImtos As Boolean
            Dim vDist As Boolean

            Dim Sql As String

            ''" SET DATEFORMAT DMY " + 
            'Sql = "SELECT  comision,CASE WHEN Activo = 1 THEN 'Si' ELSE 'No' END AS Activo,Cod_pais,cod_empresa,Cod_puesto,cod_familia,cod_vendedor FROM comis_VendFamilia WHERE cod_vendedor= '" & id & "' "
            Sql = "SELECT cod_pais,cod_sector_mercado,cod_vendedor,cod_empresa," &
                           "activo,excento_imp,es_distribuidora FROM clientes " &
                   "WHERE RTRIM(ltrim(cod_cliente))= '" & id & "' AND cod_empresa='" & vEmpresa & "' AND " &
                   " externo= " & vert & " "

            Dim fr As System.Data.OleDb.OleDbDataReader = DataBase.GetDataReader(Sql)
            If fr.Read() Then
                vVendedor = fr.Item("cod_vendedor").ToString()
                vPais = fr.Item("cod_pais").ToString()
                vEmpresa = fr.Item("cod_empresa").ToString()
                vSectMerca = fr.Item("cod_sector_mercado").ToString()
                vExcImtos = IIf(fr.Item("excento_imp").ToString() = "Si", True, False)
                vactivo = IIf(fr.Item("Activo").ToString() = "Si", True, False)
                vDist = IIf(fr.Item("es_distribuidora").ToString() = "Si", True, False)
            End If
            fr.Close()

            Dim ckeckAc As CheckBox = TryCast(GridViewOne.Rows(e.NewEditIndex).FindControl("CheckActivo"), CheckBox)
            If ckeckAc IsNot Nothing Then
                ckeckAc.Checked = vactivo
            End If

            Dim ckeckExter As CheckBox = TryCast(GridViewOne.Rows(e.NewEditIndex).FindControl("CheckExterno"), CheckBox)
            If ckeckAc IsNot Nothing Then
                ckeckExter.Checked = vert
            End If

            Dim ckeckExcImtos As CheckBox = TryCast(GridViewOne.Rows(e.NewEditIndex).FindControl("CheckExCenImp"), CheckBox)
            If ckeckExcImtos IsNot Nothing Then
                ckeckExcImtos.Checked = vExcImtos
            End If

            Dim ckeckDist As CheckBox = TryCast(GridViewOne.Rows(e.NewEditIndex).FindControl("CheckDist"), CheckBox)
            If ckeckDist IsNot Nothing Then
                ckeckDist.Checked = vDist
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



            'Dim dataSet As New DataSet
            'dataSet = DataBase.GetDataSet("SELECT cod_empresa as codEmpresa,Descripcion AS Empresa FROM  Empresas  ")
            'dtTabla = dataSet.Tables(0)
            'Dim combo As DropDownList = TryCast(GridViewOne.Rows(e.NewEditIndex).FindControl("ddlEmpresa"), DropDownList)
            'If combo IsNot Nothing Then
            '    combo.DataSource = dataSet
            '    combo.DataTextField = "Empresa"
            '    combo.DataValueField = "codEmpresa"
            '    combo.SelectedValue = vEmpresa
            '    combo.DataBind()
            'End If



            Dim dataSetMercado As New DataSet
            dataSetMercado = DataBase.GetDataSet("SELECT cod_sector_mercado as codigoSecMerc,descripcion as Mercado FROM  Sector_Mercados  ")
            dtTabla = dataSetMercado.Tables(0)
            Dim comboFamilia As DropDownList = TryCast(GridViewOne.Rows(e.NewEditIndex).FindControl("ddlMercado"), DropDownList)
            If comboFamilia IsNot Nothing Then
                comboFamilia.DataSource = dataSetMercado
                comboFamilia.DataTextField = "Mercado"
                comboFamilia.DataValueField = "codigoSecMerc"
                comboFamilia.SelectedValue = vSectMerca
                comboFamilia.DataBind()
            End If



        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.pmsgBox("Ocurrió un error al disparar el evento RowEditing SelectedIndexChanged. " & ex.Message, "error")

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

        If Me.TextNombre.Text = String.Empty Then
            MessegeText = "alertify.alert('El  proceso no puede continuar. Debe digitar Nombre.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)
            Exit Sub
        End If

        If Me.TextApellido.Text = String.Empty Then
            MessegeText = "alertify.alert('El  proceso no puede continuar. Debe digitar Apellido.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)
            Exit Sub
        End If

        If Me.TextNombre.Text = String.Empty Then
            MessegeText = "alertify.alert('El  proceso no puede continuar. Debe digitar Nombre Comercial');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)
            Exit Sub
        End If


        If Me.TextRazonSoc.Text = String.Empty Then
            MessegeText = "alertify.alert('El  proceso no puede continuar. Debe digitar Razon Social');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)
            Exit Sub
        End If

        If Me.TextDirecc.Text = String.Empty Then
            MessegeText = "alertify.alert('El  proceso no puede continuar. Debe digitar Dirección');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)
            Exit Sub
        End If

        If Me.TextTelf.Text = String.Empty Then
            MessegeText = "alertify.alert('El  proceso no puede continuar. Debe digitar Telefono');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)
            Exit Sub
        End If

        If Me.Textemail.Text = String.Empty Then
            MessegeText = "alertify.alert('El  proceso no puede continuar. Debe digitar Email');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)
            Exit Sub
        End If

        If Me.TextContacto.Text = String.Empty Then
            MessegeText = "alertify.alert('El  proceso no puede continuar. Debe digitar Contacto');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)
            Exit Sub
        End If

        If Me.TextCeduRuc.Text = String.Empty Then
            MessegeText = "alertify.alert('El  proceso no puede continuar. Debe digitar Cedula Ruc');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)
            Exit Sub
        End If

        If Me.TextCtaCont.Text = String.Empty Then
            MessegeText = "alertify.alert('El  proceso no puede continuar. Debe digitar Cuenta Contable');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)
            Exit Sub
        End If

        If Me.TextDiasCred.Text = String.Empty Then
            MessegeText = "alertify.alert('El  proceso no puede continuar. Debe digitar Días de crédito');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)
            Exit Sub
        End If

        If Me.TextLimtCred.Text = String.Empty Then
            MessegeText = "alertify.alert('El  proceso no puede continuar. Debe digitar Limite de crédito');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)
            Exit Sub
        End If




        'If Me.ddlPais.SelectedIndex = -1 Then
        '    MessegeText = "alertify.alert('El  proceso no puede continuar. Debe escojer Pais');"
        '    ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)
        '    Exit Sub
        'End If

        'If Me.ddlEmpresa.SelectedIndex = -1 Then
        '    MessegeText = "alertify.alert('El  proceso no puede continuar. Debe escojer Empresa');"
        '    ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)
        '    Exit Sub
        'End If

        If Me.ddlVendedor.SelectedIndex = -1 Then
            MessegeText = "alertify.alert('El  proceso no puede continuar. Debe escojer vendedor');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessegeText, True)
            Exit Sub
        End If

        If Me.ddlMercado.SelectedIndex = -1 Then
            MessegeText = "alertify.alert('El  proceso no puede continuar. Debe escojer Tipo de precio');"
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
            sql &= "EXEC Cat_Clientes @opcion=1," &
                   "@codigoPais =  " & Request.Cookies("CKSMFACTURA")("codPais") & " ," &
                   "@razonSocial = '" & Me.TextRazonSoc.Text.Trim & "' ," &
                   "@direccion = '" & Me.TextDirecc.Text.Trim & "' ," &
                   "@telefono = '" & Me.TextTelf.Text.Trim & "' ," &
                   "@Email =  '" & Me.Textemail.Text.Trim & "' ," &
                   "@Contacto = '" & Me.TextContacto.Text.Trim & "' ," &
                   "@CedulaRuc = '" & Me.TextCeduRuc.Text.Trim & " '," &
                   "@NombreComercial =  '" & Me.TextNombre.Text.Trim & "' ," &
                   "@codCliente =  '" & Me.TextCodCliente.Text.Trim & "' ," &
                   "@Nombres = '" & Me.TextNombre.Text.Trim & " '," &
                   "@Apellidos = '" & Me.TextApellido.Text.Trim & " '," &
                   "@codusuario =  " & Request.Cookies("CKSMFACTURA")("cod_usuario") & " ," &
                   "@codusuarioUlt =  " & Request.Cookies("CKSMFACTURA")("cod_usuario") & " ," &
                   "@DiasCredito =  " & Me.TextDiasCred.Text.Trim & " ," &
                   "@LimiteCredito =  " & Replace(Me.TextLimtCred.Text.Trim, ",", "") & " ," &
                   "@CodSectorMercado =  " & Me.ddlMercado.SelectedValue & " ," &
                   "@CodVendedor =  " & Me.ddlVendedor.SelectedValue & " ," &
                   "@Activo =  " & IIf(Me.CheckActivo.Checked = True, 1, 0) & " ," &
                   "@CtaContable = '" & Me.TextCtaCont.Text.Trim & " '," &
                   "@externo = " & IIf(Me.CheckExterno.Checked = True, 1, 0) & " ," &
                   "@excentoImp = " & IIf(Me.CheckExCenImp.Checked = True, 1, 0) & " ," &
                   "@esDistribuidora = " & IIf(Me.CheckDist.Checked = True, 1, 0) & " ," &
                   "@codempresa = " & Request.Cookies("CKSMFACTURA")("CodEmpresa") & "  ," &
                   "@BUSQUEDAD = '" & BUSQUEDAD & "' "



            Dim cmd As New OleDb.OleDbCommand(sql, dbCon)
            cmd.ExecuteNonQuery()

            Load_GridView()

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
            sql &= "EXEC Cat_Clientes @opcion=2," &
                  "@codigoPais =  " & Request.Cookies("CKSMFACTURA")("codPais") & " ," &
                  "@razonSocial =  0  ," &
                  "@direccion = 0  ," &
                  "@telefono = 0  ," &
                  "@Email =  0  ," &
                  "@Contacto = 0  ," &
                  "@CedulaRuc = 0  ," &
                  "@NombreComercial =  0  ," &
                  "@codCliente =  " & Me.hdfCodigo.Value & " ," &
                  "@Nombres = 0  ," &
                  "@Apellidos = 0  ," &
                  "@codusuario = 0  ," &
                  "@codusuarioUlt = 0  ," &
                  "@DiasCredito = 0  ," &
                  "@LimiteCredito = 0  ," &
                  "@CodSectorMercado = 0  ," &
                  "@CodVendedor = 0  ," &
                  "@Activo = 0  ," &
                  "@CtaContable = 0  ," &
                  "@externo = " & vexterno & "  ," &
                  "@excentoImp = 0  ," &
                  "@esDistribuidora = 0  ," &
                  "@codempresa = " & Request.Cookies("CKSMFACTURA")("CodEmpresa") & "  ," &
                  "@BUSQUEDAD = '" & BUSQUEDAD & "' "


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

    Private Sub Actualizar(Codigo As String, Nombre As String, apell As String, razRuc As String, direc As String, tele As String,
                           Email As String, contacto As String, Ced As String, NomCome As String, DiasCre As Integer,
                           LimtCre As Decimal, activo As String, ctacontable As String, excenIm As String,
                           esDist As String, Exter As String, merca As Decimal, vend As Decimal)
        Dim MessegeText As String = String.Empty
        Dim dbCon As New System.Data.OleDb.OleDbConnection(conn.conn)
        Try
            If dbCon.State = ConnectionState.Closed Then
                dbCon.Open()
            End If
            '--- "@codigo =  '" & Codigo & "' ," & _
            Dim sql As String = "SET DATEFORMAT DMY " & vbCrLf
            sql &= "EXEC Cat_Clientes @opcion=1," &
                "@codigoPais =  " & Request.Cookies("CKSMFACTURA")("codPais") & " ," &
                "@razonSocial = '" & razRuc & "' ," &
                "@direccion = '" & direc & "' ," &
                "@telefono = '" & tele & "' ," &
                "@Email =  '" & Email & "' ," &
                "@Contacto = '" & contacto & "' ," &
                "@CedulaRuc = '" & Ced & "' ," &
                "@NombreComercial =  '" & NomCome & "' ," &
                "@codCliente =  '" & Codigo & "' ," &
                "@Nombres = '" & Nombre & "' ," &
                "@Apellidos = '" & apell & "' ," &
                "@codusuario =  " & Request.Cookies("CKSMFACTURA")("cod_usuario") & " ," &
                "@codusuarioUlt =  " & Request.Cookies("CKSMFACTURA")("cod_usuario") & " ," &
                "@DiasCredito =" & DiasCre & "  ," &
                "@LimiteCredito =" & LimtCre & "  ," &
                "@CodSectorMercado = " & merca & "  ," &
                "@CodVendedor = " & vend & "  ," &
                "@Activo = " & IIf(Me.CheckActivo.Checked = True, 1, 0) & " ," &
                "@CtaContable =  '" & ctacontable & "' ," &
                "@externo = " & IIf(Me.CheckExterno.Checked = True, 1, 0) & " ," &
                "@excentoImp = " & IIf(Me.CheckExCenImp.Checked = True, 1, 0) & " ," &
                "@esDistribuidora = " & IIf(Me.CheckDist.Checked = True, 1, 0) & " ," &
                "@codempresa = " & Request.Cookies("CKSMFACTURA")("CodEmpresa") & "  ," &
                "@BUSQUEDAD = '" & BUSQUEDAD & "' "

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
        Dim exter As String

        Me.hdfCodigo.Value = Me.GridViewOne.SelectedValue.ToString
        exter = Me.GridViewOne.SelectedRow.Cells(2).Text()


        ' Me.GridView1.SelectedRow.Cells(3).Text
        If exter = "No" Then
            Session("Cexterno") = 0
        Else
            Session("Cexterno") = 1
        End If

        Session("CEmpresa") = Me.GridViewOne.SelectedRow.Cells(3).Text

        'Me.hdfCodigo.Value = Me.GridViewOne.DataKeys(e.ToString).Value

        GetReg()
        Me.ltMensaje.Text = String.Empty
        ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Facturacion", "open_popup();", True)
        ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Facturacion", "select_mercado();", True)
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
            sql &= "EXEC Cat_Clientes @opcion=4," &
                  "@codigoPais =  " & Request.Cookies("CKSMFACTURA")("codPais") & " ," &
                  "@razonSocial =  0  ," &
                  "@direccion = 0  ," &
                  "@telefono = 0  ," &
                  "@Email =  0  ," &
                  "@Contacto = 0  ," &
                  "@CedulaRuc = 0  ," &
                  "@NombreComercial =  0  ," &
                  "@codCliente =  " & Me.hdfCodigo.Value & " ," &
                  "@Nombres = 0  ," &
                  "@Apellidos = 0  ," &
                  "@codusuario = 0  ," &
                  "@codusuarioUlt = 0  ," &
                  "@DiasCredito = 0  ," &
                  "@LimiteCredito = 0  ," &
                  "@CodSectorMercado = 0  ," &
                  "@CodVendedor = 0  ," &
                  "@Activo = 0  ," &
                  "@CtaContable = 0  ," &
                  "@externo = " & Session("Cexterno") & "," &
                  "@excentoImp = 0  ," &
                  "@esDistribuidora = 0  ," &
                  "@codempresa =  " & Request.Cookies("CKSMFACTURA")("CodEmpresa") & " ," &
                  "@BUSQUEDAD = '" & BUSQUEDAD & "' "

            Dim dr As System.Data.OleDb.OleDbDataReader = DataBase.GetDataReader(sql)
            'Request.Cookies("CKSMFACTURA")("CodEmpresa")..cod_pais(, c.cod_empresa)

            If dr.Read() Then
                Me.TextCodCliente.Text = dr.Item("cod_cliente").ToString
                Me.TextNombre.Text = dr.Item("nombres").ToString
                Me.TextApellido.Text = dr.Item("apellidos").ToString
                'Me.TextNombComer.Text = dr.Item("nombre_comercial").ToString
                Me.TextRazonSoc.Text = dr.Item("razon_social").ToString
                Me.TextDirecc.Text = dr.Item("direccion").ToString
                Me.TextTelf.Text = dr.Item("telefono").ToString
                Me.Textemail.Text = dr.Item("email").ToString
                Me.TextContacto.Text = dr.Item("contacto").ToString
                Me.TextCeduRuc.Text = dr.Item("cedula_ruc").ToString
                Me.TextCtaCont.Text = dr.Item("cta_contable").ToString
                Me.TextDiasCred.Text = dr.Item("dias_credito").ToString
                Me.TextLimtCred.Text = dr.Item("limite_credito").ToString
                Me.CheckActivo.Checked = IIf(dr.Item("activo").ToString() = "Si", True, False)
                Me.CheckExterno.Checked = IIf(dr.Item("externo").ToString() = "Si", True, False)
                Me.CheckExCenImp.Checked = IIf(dr.Item("excento_imp").ToString() = "Si", True, False)
                Me.CheckDist.Checked = IIf(dr.Item("es_distribuidora").ToString() = "Si", True, False)
                Me.Cmercado.SelectedValue = dr.Item("cod_sector_mercado").ToString
                Me.CVendedor.SelectedValue = dr.Item("cod_vendedor").ToString



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

            Dim name_doc = "catalogo_Clientes" & Date.Now.Year & Date.Now.Month & Date.Now.Day

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
            MyHTML &= "<h2>Catalogo de clientes <br/> al dia " & Date.Now.Day & "/" & Date.Now.Month & "/" & Date.Now.Year & "</h2>"

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
                          "<th>RazonSocial</th>" &
                          "<th>Direccion</th>" &
                          "<th>Telefono</th>" &
                          "<th>Email</th>" &
                          "<th>Contacto</th>" &
                          "<th>Cedula/Ruc</th>" &
                          "<th>Nombre Comercial</th>" &
                          "<th>Dias Credito</th>" &
                          "<th>Limite Credito</th>" &
                          "<th>Mercado</th>" &
                          "<th>Vendedor</th>" &
                          "<th>Activo</th>" &
                          "<th>CtaContable</th>" &
                          "<th>Cliente Externo</th>" &
                          "<th>Excento Impuestos</th>" &
                          "<th>EsDeDistribuidora</th>" &
                          "<th>Empresa</th>" &
                          "</tr>"


                Dim clase As String = String.Empty

                For i As Integer = 0 To dtTabla.Rows.Count - 1
                    MyHTML &= "<tr" & clase & ">" &
                              "<td class=""first"">" & i + 1 & "</td>" &
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("Codigo").ToString.Trim & "</td>" &
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("nombres").ToString.Trim & "</td>" &
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("apellidos").ToString.Trim & "</td>" &
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("Pais").ToString.Trim & "</td>" &
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("Empresa").ToString.Trim & "</td>" &
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("razon_social").ToString.Trim & "</td>" &
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("direccion").ToString.Trim & "</td>" &
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("telefono").ToString.Trim & "</td>" &
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("email").ToString.Trim & "</td>" &
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("contacto").ToString.Trim & "</td>" &
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("cedula_ruc").ToString.Trim & "</td>" &
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("nombre_comercial").ToString.Trim & "</td>" &
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("dias_credito").ToString.Trim & "</td>" &
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("limite_credito").ToString.Trim & "</td>" &
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("Mercado").ToString.Trim & "</td>" &
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("Vendedor").ToString.Trim & "</td>" &
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("activo").ToString.Trim & "</td>" &
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("cta_contable").ToString.Trim & "</td>" &
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("externo").ToString.Trim & "</td>" &
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("excento_imp").ToString.Trim & "</td>" &
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("es_distribuidora").ToString.Trim & "</td>" &
                              "<td class=""ml"">" & dtTabla.Rows(i).Item("Empresa").ToString.Trim & "</td>" &
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
            Me.ltMensaje.Text = conn.pmsgBox("Ocurrio un error al intentar exportar la tabla. " & ex.Message, "error")
        End Try
    End Sub
#End Region

    Protected Sub btnLoadClientes_Click(sender As Object, e As EventArgs) Handles btnLoadClientes.Click
        Try
            Dim sql As String = String.Empty
            sql = "EXEC sp_show_cliente @cod_cliente = '" & Me.TextCodCliente.Text.Trim & "' "

            Dim dr As System.Data.OleDb.OleDbDataReader
            dr = DataBase.GetDataReader(sql)

            If dr.Read() Then
                TextNombre.Text = dr.Item("nombres").ToString()
                TextApellido.Text = dr.Item("apellidos").ToString()
                'TextNombComer.Text = dr.Item("nombre_comercial").ToString()
                TextRazonSoc.Text = dr.Item("razon_social").ToString()
                TextDirecc.Text = dr.Item("direccion").ToString()
                TextTelf.Text = dr.Item("telefono").ToString()
                Textemail.Text = dr.Item("email").ToString()
                TextContacto.Text = dr.Item("contacto").ToString()
                TextCeduRuc.Text = dr.Item("cedula_ruc").ToString()
                TextCtaCont.Text = dr.Item("cta_contable").ToString()
                TextDiasCred.Text = dr.Item("dias_credito").ToString()
                TextLimtCred.Text = dr.Item("limite_credito").ToString()

                If dr.Item("activo").ToString() = "si" Then
                    CheckActivo.Checked = True
                Else
                    CheckActivo.Checked = False
                End If

                If dr.Item("externo").ToString() = "si" Then
                    CheckExterno.Checked = True
                Else
                    CheckExterno.Checked = False
                End If

                If dr.Item("excento_imp").ToString() = "si" Then
                    CheckExCenImp.Checked = True
                Else
                    CheckExCenImp.Checked = False
                End If

                If dr.Item("es_distribuidora").ToString() = "si" Then
                    CheckDist.Checked = True
                Else
                    CheckDist.Checked = False
                End If

                Cmercado.SelectedValue = dr.Item("cod_sector_mercado").ToString()
                CVendedor.SelectedValue = dr.Item("cod_vendedor").ToString()
            Else
                TextNombre.Text = String.Empty
                TextApellido.Text = String.Empty
                'TextNombre.Text = String.Empty
                TextRazonSoc.Text = String.Empty
                TextDirecc.Text = String.Empty
                TextTelf.Text = String.Empty
                Textemail.Text = String.Empty
                TextContacto.Text = String.Empty
                TextCeduRuc.Text = String.Empty
                TextCtaCont.Text = String.Empty
                TextDiasCred.Text = String.Empty
                TextLimtCred.Text = String.Empty
                CheckActivo.Checked = False
                CheckExterno.Checked = False
                CheckExCenImp.Checked = False
                CheckDist.Checked = False
                Cmercado.SelectedValue = String.Empty
                CVendedor.SelectedValue = String.Empty
            End If

            TextCodCliente.Focus()

            dr.Close()

        Catch ex As Exception
            Me.ltMensajeBuscarCliente.Text = conn.pmsgBox("Ocurrió un error al intentar recuperar los datos del cliente." & ex.Message, "error")
        End Try
    End Sub



    Private Sub ddlMercado_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlMercado.SelectedIndexChanged

        '    If ddlMercado.SelectedItem.Text = "DISTRIBUIDOR" OrElse ddlMercado.SelectedItem.Text = "MAYORISTAS" OrElse ddlMercado.SelectedItem.Text = "DETALLE" OrElse ddlMercado.SelectedItem.Text = "MERCADO POPULAR" OrElse ddlMercado.SelectedItem.Text = "NANDAIME" Then

        '        TextApellido.Style("Visibility") = "Visible"
        '        lblApellido.Style("Visibility") = "Visible"

        '    Else
        '        TextApellido.Style("Visibility") = "Hidden"
        '        lblApellido.Style("Visibility") = "Hidden"

        '    End If

    End Sub



End Class
