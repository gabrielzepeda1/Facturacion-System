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
Imports System.Activities
Imports System.Activities.Statements

Partial Class movimientos_Factura
    Inherits System.Web.UI.Page

    'Public DatosFact As String = String.Empty
    Dim conn As New FACTURACION_CLASS.Seguridad
    Dim DataBase As New FACTURACION_CLASS.database
    Dim BUSQUEDAD As String

    Dim ProdusctoCodpais As Integer
    Dim ProductoEmpre As Integer
    Dim ProductoPuesto As Integer
    Dim ProductoNoFac As Integer
    Dim ProductoFecha As Date
    Dim ProductoCodProd As String
    Dim EliminarFormaPago As String

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

    'Protected Sub btnCboCLi_Click(sender As Object, e As EventArgs) Handles btnCboCLi.Click

    'End Sub


    'Protected Sub btnNomCli_Click(sender As Object, e As EventArgs) Handles btnNomCli.Click
    '    Try

    '        'If Me.ddlCliente.Text = String.Empty And Me.TextNomClien.Text = String.Empty Then
    '        'If rdbContado.Checked = True And Me.ddlCliente.Text = String.Empty Then

    '        '    'Me.ddlCliente.Items.Insert(0, New ListItem("Cliente Eventual...", String.Empty))
    '        '    ddlCliente.SelectedValue = "0"
    '        '    'Me.ddlCliente.SelectedValue = ""
    '        '    TextId.Enabled = True
    '        'End If

    '        'If rdbCredito.Checked = True Then
    '        '    TextNomClien.Text = String.Empty
    '        '    TextId.Enabled = False

    '        '    ddlCliente.Focus()



    '        'End If

    '    Catch ex As Exception
    '        Me.ltMensajeGrid.Text = conn.pmsgBox("Ocurrió un error en signacion de cliente eventual, combo  del cliente." & ex.Message, "error")
    '    End Try
    'End 
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not IsPostBack Then
            paridad()
            cboFormaPago()
            'cboMoneda()
            cboproductos()
            CboCliente()
            cboVendedor()
            NoFactura()
            System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("es-ES")
            Form.DefaultButton = Me.btnVeriInvent.UniqueID
            'ddlCliente.Attributes.Add("onblur", Me.Page.ClientScript.GetPostBackEventReference(Me.btnNomCli, ""))
            'TextNomClien.Attributes.Add("onblur", Me.Page.ClientScript.GetPostBackEventReference(Me.btnNomCli, ""))

            ProductosPrecio()
            TextTotal.Enabled = False
            'TextTotal.BackColor = Drawing.Color.LightBlue
            TextCambioCor.BackColor = Drawing.Color.AntiqueWhite
            TextSaldoFact.BackColor = Drawing.Color.AntiqueWhite
            TextTotal.BackColor = Drawing.Color.AntiqueWhite
            lblConverDolACor.Visible = False
            LblDolar.Visible = False
            Session("Mercado") = Session("lista")
            If Session("CambiaPrecio") = "Si" Then
                TextPrecio.Enabled = True
            Else
                TextPrecio.Enabled = False
                TextPrecio.BackColor = Drawing.Color.AntiqueWhite
            End If

            ProducExistEnInv()

        End If
        If Session("VerifInven") = "Si" Then
            TextCantidad.Attributes.Add("onblur", Me.Page.ClientScript.GetPostBackEventReference(Me.TextCantidad, ""))
        End If
    End Sub

    Private Sub NoFactura()
        Dim vCcod_usuario As String = Context.Request.Cookies("CKSMFACTURA")("cod_usuario")

        'The first line of the sub-routine declares a variable called "vCcod_usuario" And assigns it the value of a cookie called "CKSMFACTURA" that contains a "cod_usuario" field. This line of code assumes that the cookie has already been set elsewhere in the application.

        Dim sql As String
        sql = " SET DATEFORMAT DMY " + "SELECT no_factura+1 as NoFactura,verificar_inventario AS INV FROM puestos WHERE cod_pais= " & Session("cod_pais") & "  " &
              " and cod_empresa= " & Session("cod_empresa") & " " &
             " and cod_puesto= " & Session("cod_puesto") & " "

        'The next line declares a SQL query that will be used to retrieve the next invoice number for a particular user, company, and location. The query selects the "no_factura" field from a database table called "puestos" and adds 1 to it to generate the next invoice number. It also selects a field called "verificar_inventario" from the same table and assigns it to a variable called "INV". The SQL query uses the values of three Session variables called "cod_pais", "cod_empresa", and "cod_puesto" to filter the results.


        '" and consecutivo_usuario= " & vCcod_usuario & " "
        Dim fr As System.Data.OleDb.OleDbDataReader = DataBase.GetDataReader(sql)

        'The next line of code executes the SQL query and retrieves a data reader object called "fr".

        If fr.Read() Then
            txtNoFac.Text = fr.Item("NoFactura").ToString()
            rdbContado.Checked = True
            rdbCredito.Checked = False
            Session("INV") = fr.Item("INV").ToString()
            ddlCliente.Focus()
        End If

        'The next line of code uses the data reader object to read the results of the query. If the query returned any rows, the code sets the value of the "txtNoFac" textbox to the next invoice number, sets the "rdbContado" radio button to "checked", sets the "rdbCredito" radio button to "unchecked", assigns the value of "INV" to a Session variable called "INV", and sets the focus to a dropdown list called "ddlCliente".
    End Sub
    Private Sub paridad()
        Dim Sql As String
        'Sql = "SELECT Paridad FROM Paridad  WHERE cod_Pais= '" & Session("cod_pais") & "'"
        Sql = "EXEC Cat_FormaPago @opcion=5," &
                  "@codigoPais =  " & Session("cod_pais") & " ," &
                  "@codigoEmpresa =  " & Session("cod_empresa") & " ," &
                  "@codigoPuesto =  " & Session("cod_puesto") & " ," &
                  "@codigoFormaPago =  '0' ," &
                  "@descripcion =  '0' ," &
                  "@BUSQUEDAD = '0'  ," &
                  "@PorDefecto = 0 "
        Dim MessageText As String
        Dim fr As System.Data.OleDb.OleDbDataReader = DataBase.GetDataReader(Sql)
        If fr.Read() Then
            If fr("codformapago").ToString().Trim = "99" Then
                MessageText = "alertify.alert('Codigo de forma de pago, no esta predeterminado, favor grabarla en catalogo de forma de pago', function () { window.location.href = '../Default.aspx'; });"
                ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessageText, True)
                Exit Sub
            End If
            If fr("Paridad").ToString().Trim = 0 Then
                MessageText = "alertify.alert('Paridad del dia no ha sido ingresado, favor grabarla en catalogo de paridad', function () { window.location.href = '../Default.aspx'; });"
                ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessageText, True)
                Exit Sub
            End If
            If fr("moneda").ToString().Trim = 99 Then
                MessageText = "alertify.alert('Codigo de Moneda, no esta predeterminado, favor grabarla en catalogo de moneda', function () { window.location.href = '../Default.aspx'; });"
                ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessageText, True)
                Exit Sub
            End If
            Session("Paridad") = fr("Paridad").ToString().Trim
            Session("Cod_FormaPago") = fr("codformapago").ToString().Trim
            Session("Cod_Moneda") = fr("moneda").ToString().Trim
            'If fr.Read() Then
            '    If fr("Paridad").ToString().Trim <> 0 Then
            '        Session("Paridad") = fr("Paridad").ToString().Trim
            '        Session("Cod_FormaPago") = fr("Cod_FormaPago").ToString().Trim
            '    Else
            '        Session("Paridad") = 0
            '        Dim MessageText As String
            '        MessageText = "alertify.alert('Paridad del dia no ha sido ingresado, favor grabarla en catalogo de paridad', function () { window.location.href = '../Default.aspx'; });"
            '        ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessageText, True)
            '        Exit Sub
            '    End If

            'Else
            '    Session("Paridad") = 0
            '    Dim MessageText As String
            '    MessageText = "alertify.alert('Paridad del dia no ha sido ingresado, favor grabarla en catalogo de paridad', function () { window.location.href = '../Default.aspx'; });"
            '    ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessageText, True)
            '    Exit Sub
        End If
        fr.Close()

    End Sub
    Private Sub cboVendedor()
        Try

            Dim dataSet As New DataSet
            dataSet = DataBase.GetDataSet(" EXEC CombosProductos " &
                  "@opcion = 10," &
                  "@codigo = " & Session("cod_pais") & " ")


            If dataSet.Tables(0).Rows.Count > 0 Then
                ddVendedor.DataSource = dataSet.Tables(0)
                ddVendedor.DataTextField = "Vendedor"
                ddVendedor.DataValueField = "codVendedor"
                ddVendedor.DataBind()
            End If
            'If Me.rdbExterno.Checked = False Then
            '    ddlCliente.Items.Insert(0, New ListItem("-Seleccione-", "0"))
            'End If
            dataSet.Dispose()

        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.PmsgBox("Ocurrió un error al intentar cargar el listado de rubros en la tabla. de vendedor" & ex.Message, "error")

        End Try
    End Sub


    Private Sub cboFormaPago()
        Try

            Dim dataSet As New DataSet
            dataSet = DataBase.GetDataSet(" EXEC CombosProductos " &
                  "@opcion = 21," &
                  "@codigo = " & Session("cod_pais") & " ")


            If dataSet.Tables(0).Rows.Count > 0 Then
                ddformaPago.DataSource = dataSet.Tables(0)
                ddformaPago.DataTextField = "FormaPago"
                ddformaPago.DataValueField = "codFormaPago"
                ddformaPago.DataBind()
            End If
            'If Me.rdbExterno.Checked = False Then
            '    ddlCliente.Items.Insert(0, New ListItem("-Seleccione-", "0"))
            'End If
            dataSet.Dispose()

        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.PmsgBox("Ocurrió un error al intentar cargar el listado de rubros en la tabla. de forma de pago" & ex.Message, "error")

        End Try
    End Sub
    Private Sub cboproductos()
        Try
            'Dim sql As String = String.Empty
            'sql = " EXEC CombosProductos " & _
            '      "@opcion = 17," & _
            '      "@codigo = " & Session("cod_pais") & " "


            'If Session("Paridad") = 0 Then
            '    Response.Redirect(ResolveClientUrl("../Default.aspx"))
            'End If

            Dim dataSet As New DataSet
            dataSet = DataBase.GetDataSet(" EXEC CombosProductos " &
                  "@opcion = 17," &
                  "@codigo = " & Session("cod_pais") & " ")


            If dataSet.Tables(0).Rows.Count > 0 Then
                ddlProducto.DataSource = dataSet.Tables(0)
                ddlProducto.DataTextField = "Producto"
                ddlProducto.DataValueField = "codProduto"
                ddlProducto.DataBind()
            End If
            'If Me.rdbExterno.Checked = False Then
            '    ddlCliente.Items.Insert(0, New ListItem("-Seleccione-", "0"))
            'End If
            dataSet.Dispose()

        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.PmsgBox("Ocurrió un error al intentar cargar el listado de rubros en la tabla." & ex.Message, "error")

        End Try
    End Sub
    Private Sub CboCliente()
        Try

            Dim vext As Integer
            If Me.rdbExterno.Checked = False Then
                vext = 0
            Else
                vext = 1
            End If

            Dim dataSet As New DataSet
            'dataSet = DataBase.GetDataSet(" EXEC CombosProductos " & _
            '                              "@opcion = 16," & _
            '                              "@codigo = " & Request.Cookies("CKSMFACTURA")("codPais") & " ")
            ' " and externo= " & vext & " " & _
            'dataSet = DataBase.GetDataSet("SELECT ltrim(rtrim(cod_cliente)) as codcliente," & _
            '                             "ltrim(rtrim(nombre_comercial)) AS cliente" & _
            '                            " FROM Clientes WHERE cod_pais= " & Session("cod_pais") & " " & _
            '                           " AND cod_pais= " & Session("cod_pais") & " " & _
            '                          " AND cod_empresa= " & Session("cod_empresa") & " " & _
            '                         " order by ltrim(rtrim(nombre_comercial)) ")
            '
            '" and  (externo=0 or cod_cliente='0')  " & _
            'Dim dv As DataView = ds.Tables("a").DefaultView
            'dv.RowFilter = "FamCodigo='" & codigo & "'"


            Dim Sql As String
            Sql = "SELECT ltrim(rtrim(cod_cliente)) as codcliente," &
                                      "ltrim(rtrim(nombre_comercial)) AS cliente,externo" &
                                      " FROM Clientes WHERE cod_pais= " & Session("cod_pais") & " " &
                                      " AND cod_pais= " & Session("cod_pais") & " " &
                                      " AND cod_empresa= " & Session("cod_empresa") & " " &
                                      " order by ltrim(rtrim(nombre_comercial)) "

            Dim dt As New DataTable
            dt = DataBase.GetDateTable(Sql)

            If dt.Rows.Count Then
                Dim DR As DataRow
                DR = dt.Rows(0)
                Session("TableCliente") = dt
            End If

            '            Dim DT_array As DataTable()
            '            Dim DS As DataSet
            '            DS.Tables.AddRange(DT_array)

            dataSet.Tables.Add(dt)
            If dataSet.Tables(0).Rows.Count > 0 Then
                ddlCliente.DataSource = dataSet.Tables(0)
                ddlCliente.DataTextField = "cliente"
                ddlCliente.DataValueField = "codcliente"
                ddlCliente.DataBind()
            End If
            'If Me.rdbExterno.Checked = False Then
            '    ddlCliente.Items.Insert(0, New ListItem("-Seleccione-", "0"))
            'End If
            dataSet.Dispose()

        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.PmsgBox("Ocurrió un error al intentar cargar el listado de rubros en la tabla." & ex.Message, "error")

        End Try
    End Sub

    Protected Sub rdbCredito_CheckedChanged(sender As Object, e As EventArgs) Handles rdbCredito.CheckedChanged
        Me.TextNomClien.Text = " "
        Me.TextNomClien.Enabled = False
        Me.TextNomClien.BackColor = Drawing.Color.Beige
        Me.ddlCliente.Focus()
    End Sub

    Protected Sub rdbContado_CheckedChanged(sender As Object, e As EventArgs) Handles rdbContado.CheckedChanged
        Me.TextNomClien.BackColor = Drawing.Color.WhiteSmoke
        Me.TextNomClien.Enabled = True
        Me.TextNomClien.Focus()
    End Sub

    Private Sub InterExter()
        Try
            Dim TipoClie As Integer
            If Me.rdbExterno.Checked = False Then
                TipoClie = 0
            Else
                TipoClie = 1
            End If


            Dim dtDataTable As New DataTable
            dtDataTable = Session("TableCliente")

            Dim dv As DataView = New DataView(dtDataTable)
            Dim dtNew As DataTable = dtDataTable.Clone


            dv.Sort = "externo,cliente"
            dv.RowFilter = ("externo =" & TipoClie & " ")

            dtNew = dv.ToTable("", True, "codcliente", "cliente")

            If Me.rdbExterno.Checked = False Then
                dtNew.Rows.Add("0", "CLIENTE EVENTUAL")
                'dtNew.NewRow.AcceptChanges()
            End If

            dv.RowFilter = ""



            Dim dataSet As New DataSet

            dataSet.Tables.Add(dtNew)
            If dataSet.Tables(0).Rows.Count > 0 Then
                ddlCliente.DataSource = dataSet.Tables(0)
                ddlCliente.DataTextField = "cliente"
                ddlCliente.DataValueField = "codcliente"
                ddlCliente.DataBind()
            End If

        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.PmsgBox("Ocurrió un error al intentar cargar el listado de rubros en la tabla." & ex.Message, "error")

        End Try
    End Sub

    Protected Sub rdbInterno_CheckedChanged(sender As Object, e As EventArgs) Handles rdbInterno.CheckedChanged
        'CboCliente()
        InterExter()
    End Sub

    Protected Sub rdbExterno_CheckedChanged(sender As Object, e As EventArgs) Handles rdbExterno.CheckedChanged
        InterExter()
    End Sub

    Private Sub nuevo()
        TextCantidad.Text = 0
        TextBultos.Text = 0
        TextPrecio.Text = 0
        TextTotal.Text = 0
        Session("Imtos") = 0
        Session("desimprimir") = ""
        Session("Unidad") = ""
        'ddlProducto.Focus()
    End Sub

#Region "Proceso verificar inventario"
    Private Sub Inventario()
        'Private Sub btnVeriInvent_Click(sender As Object, e As EventArgs) Handles btnVeriInvent.Click
        'Session("VerifInven")


        Try
            Dim MessageText As String = String.Empty
            If Session("INV") = True Then
                'Dim SQL As String = String.Empty
                'SQL = "EXEC VerifiInventario @opcion=1," & _
                '      "@codigoPais =  " & Session("cod_pais") & "  ," & _
                '      "@codigoEmpresa =  " & Session("cod_empresa") & "  ," & _
                '      "@codigoPuesto =  " & Session("cod_puesto") & " "

                'Dim dt As DataTable
                'dt = DataBase.GetDataSet(SQL).Tables(0)

                Dim dt As New DataTable
                dt = Session("TableInven")
                Dim v_producto As String = Me.ddlProducto.SelectedValue
                Dim busca_Producto() As DataRow
                Dim vcantida As String
                If dt.Rows.Count >= 1 Then
                    busca_Producto = dt.Select("cod_producto='" & v_producto & "'")
                    If busca_Producto.Length > 0 Then
                        vcantida = (CStr(busca_Producto(0).Item(1)))
                        Session("Imtos") = busca_Producto(0).Item(2)
                        Session("desimprimir") = busca_Producto(0).Item(3)
                        Session("Unidad") = busca_Producto(0).Item(4)
                        If CDec(vcantida.Trim) < CDec(TextCantidad.Text.Trim) Then
                            TextCantidad.Text = vcantida
                            'Me.ltMensajeGrid.Text = conn.pmsgBox("CANTIDAD A VENDER MAYOR QUE EXISTENCIA. EXISTENCIA=" & vcantida, "Sin Inventario")
                            MessageText = "alertify.alert('Cantidad a vender es mayor que la existencia=" + vcantida + "');"
                            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessageText, True)
                            Exit Sub
                        End If
                        precio()

                    Else
                        nuevo()
                        'Me.ltMensajeGrid.Text = conn.pmsgBox("Producto no tiene existencia, por lo tanto no puede facturarse", "error")
                        MessageText = "alertify.alert('Producto no tiene existencia, por lo tanto no puede facturarse');"
                        ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessageText, True)
                        Exit Sub
                        ddlProducto.Focus()

                    End If

                End If
            End If
            'Me.ltMensajeGrid.Text = String.Empty
            'ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessageText, True)
        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.PmsgBox("Ocurrió un error al intentar cargar el listado de rubros en la tabla." & ex.Message, "error")


        End Try
    End Sub

#End Region

    Protected Sub BtnAdicionar_Click(sender As Object, e As System.EventArgs) Handles BtnAdicionar.Click
        Dim MessageText As String = String.Empty

        If Session("Paridad") = 0 Then
            MessageText = "alertify.alert('Paridad del dia no ha sido ingresado, favor grabarla en catalogo de paridad');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessageText, True)
            Exit Sub
        End If

        If Me.txtNoFac.Text = String.Empty Then
            MessageText = "alertify.alert('El  proceso no puede continuar. no exite No.Factura.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessageText, True)
            Exit Sub
        End If
        If Me.rdbCredito.Checked = False And rdbContado.Checked = False Then
            MessageText = "alertify.alert('El  proceso no puede continuar. debe escojer si factura es de credito o de contado.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessageText, True)
            Exit Sub
        End If
        If Me.rdbCredito.Checked = True Then
            If Me.ddlCliente.Text = "0" Then
                MessageText = "alertify.alert('El  proceso no puede continuar. debe escojer Nombre del cliente, factura es de credito');"
                ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessageText, True)
                Exit Sub
                Me.ddlCliente.Focus()
            End If
        Else
            If Me.TextNomClien.Text = String.Empty Then
                MessageText = "alertify.alert('El  proceso no puede continuar. tiene que escojer al cliente o digitar el nombre');"
                ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessageText, True)
                Exit Sub
            End If
        End If
        If Me.rdbContado.Checked = True Then
            If Me.ddlCliente.Text = String.Empty And Me.TextNomClien.Text = String.Empty Then
                MessageText = "alertify.alert('El  proceso no puede continuar. debe escojer o digitar nombre del cliente');"
                ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessageText, True)
                Exit Sub
            End If
        End If
        'If Me.rdbContado.Checked = True Then
        '    If Me.ddlCliente.Text <> String.Empty And Me.TextNomClien.Text <> String.Empty Then
        '        MessageText = "alertify.alert('El  proceso no puede continuar. tiene digitado el nombre del cliente y otro escojido en el combo, deje solo uno de ellos');"
        '        ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessageText, True)
        '        Exit Sub
        '    End If
        'End If
        If Me.rdbContado.Checked = True Then
            If Me.TextId.Text = String.Empty Then
                MessageText = "alertify.alert('El  proceso no puede continuar. debe de digitar Id de cliente');"
                ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessageText, True)
                Exit Sub
            End If
        End If
        If Me.ddlProducto.Text = String.Empty Then
            MessageText = "alertify.alert('El  proceso no puede continuar. debe escojer Producto');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessageText, True)
            Exit Sub
        End If
        If Me.TextCantidad.Text = String.Empty Then
            MessageText = "alertify.alert('El  proceso no puede continuar. debe de digitar Cantidad');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessageText, True)
            Exit Sub
        End If
        If Me.TextBultos.Text = String.Empty Then
            MessageText = "alertify.alert('El  proceso no puede continuar. debe de digitar Bultos');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessageText, True)
            Exit Sub
        End If
        If Me.TextPrecio.Text = String.Empty Then
            MessageText = "alertify.alert('El  proceso no puede continuar. debe de digitar Precio');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessageText, True)
            Exit Sub
        End If
        If Me.TextTotal.Text = String.Empty Then
            MessageText = "alertify.alert('El  proceso no puede continuar. debe de establecer el total');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessageText, True)
            Exit Sub
        End If

        GuardarTmpFact()
    End Sub

#Region "PROCESOS Y EVENTOS DEL GRIDVIEW"
    Private Sub Load_GridView()
        Try
            Dim vCPais As String = String.Empty
            vCPais = Context.Request.Cookies("CKSMFACTURA")("codPais")
            Dim SQL As String = String.Empty
            SQL &= "EXEC Factura @opcion=3," &
                  "@codigoPais =  " & vCPais & " ," &
                  "@codigoPuesto =  " & Request.Cookies("CKSMFACTURA")("CodPuesto") & "  ," &
                  "@codigoEmpresa = " & Request.Cookies("CKSMFACTURA")("CodEmpresa") & "  ," &
                  "@no_factura = " & txtNoFac.Text.Trim & "," &
                  "@fecha =  NULL ," &
                  "@cod_producto =  NULL ," &
                  "@consecutivoUsuario =  NULL ," &
                  "@porc_descuento =  NULL ," &
                  "@valor_descuento =  NULL ," &
                  "@porc_iva =  NULL ," &
                  "@valor_iva =  NULL ," &
                  "@anulada =  NULL ," &
                  "@fechaHora_anulacion =  NULL ," &
                  "@sub_total =  NULL ," &
                  "@paridad =  NULL ," &
                  "@codcliente =  NULL ," &
                  "@cantidad =  NULL ," &
                  "@bultos =  NULL ," &
                  "@cod_und_medida =  NULL ," &
                  "@precio_unidad =  NULL ," &
                  "@Notas=  NULL ," &
                  "@Contado =  NULL ," &
                  "@codvendedor =  NULL ," &
                  "@cedularuc =  NULL ," &
                  "@externo =  NULL ," &
                  "@desc_imprimir =  NULL ," &
                  "@NombreCliente =  NULL "



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

            If e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Attributes.Add("OnMouseOver", "On(this);")
                e.Row.Attributes.Add("OnMouseOut", "Off(this);")
                'e.Row.Attributes.Add("OnClick", "" & Page.ClientScript.GetPostBackClientHyperlink(Me.GridViewOne, "Select$" + e.Row.RowIndex.ToString) & ";")
            End If

        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.PmsgBox("Ocurrió un error al disparar el evento RowDataBound. " & ex.Message, "error")

        End Try
    End Sub

    Protected Sub GridViewOne_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles GridViewOne.RowDeleting
        Try
            'pasar la información del Gridview hacia otro control, en este caso el control HiddenField,
            Me.hdfCodigo.Value = Me.GridViewOne.DataKeys(e.RowIndex).Value


            ProdusctoCodpais = GridViewOne.DataKeys(e.RowIndex).Item(0).ToString()
            ProductoEmpre = GridViewOne.DataKeys(e.RowIndex).Item(1).ToString()
            ProductoPuesto = GridViewOne.DataKeys(e.RowIndex).Item(2).ToString()
            ProductoNoFac = GridViewOne.DataKeys(e.RowIndex).Item(3).ToString()
            ProductoFecha = GridViewOne.DataKeys(e.RowIndex).Item(4).ToString()
            ProductoCodProd = GridViewOne.DataKeys(e.RowIndex).Item(5).ToString()
            Eliminar(ProductoCodProd)

            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "smf_Script", "responsive_grid();", True)

        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.PmsgBox("Ocurrió un error al disparar el evento RowDeleting del grid " & ex.Message, "error")

        End Try
    End Sub
    Protected Sub GridViewOne_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridViewOne.SelectedIndexChanged
        Try
            '"cod_pais,cod_empresa,cod_puesto,no_factura,fecha,cod_producto"  
            Me.hdfCodigo.Value = Me.GridViewOne.SelectedValue.ToString
            ProdusctoCodpais = GridViewOne.SelectedDataKey.Values(0)
            ProductoEmpre = GridViewOne.SelectedDataKey.Values(1)
            ProductoPuesto = GridViewOne.SelectedDataKey.Values(2)
            ProductoNoFac = GridViewOne.SelectedDataKey.Values(3)
            ProductoFecha = GridViewOne.SelectedDataKey.Values(4)
            ProductoCodProd = GridViewOne.SelectedDataKey.Values(5)


            'ProdusctoCodpais = Me.GridViewOne.SelectedRow.Cells(1).Text()
            'ProductoEmpre = Me.GridViewOne.SelectedRow.Cells(2).Text()
            'ProductoPuesto = Me.GridViewOne.SelectedRow.Cells(3).Text()
            'ProductoNoFac = Me.GridViewOne.SelectedRow.Cells(4).Text()
            'ProductoFecha = Me.GridViewOne.SelectedRow.Cells(5).Text()
            'ProductoCodProd = Me.GridViewOne.SelectedRow.Cells(6).Text()

            Eliminar(ProductoCodProd)

            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "smf_Script", "responsive_grid();", True)

        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.PmsgBox("Ocurrió un error al disparar el evento SelectedIndexChanged. " & ex.Message, "error")

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

        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.PmsgBox("Ocurrió un error al disparar el evento SelectedIndexChanged. " & ex.Message, "error")

        End Try
    End Sub
#End Region

    Private Sub Eliminar(ProductoCodProd As String)

        'Dim ProdusctoCodpais As Integer
        'Dim ProductoEmpre As Integer
        'Dim ProductoPuesto As Integer
        'Dim ProductoNoFac As Integer
        'Dim ProductoFecha As Date
        'Dim ProductoCodProd As String

        Dim MessageText As String = String.Empty
        Dim dbCon As New System.Data.OleDb.OleDbConnection(conn.Conn)
        Try
            If dbCon.State = ConnectionState.Closed Then
                dbCon.Open()
            End If

            If Me.TextPorDesc.Text = String.Empty Then
                Me.TextPorDesc.Text = 0
            End If

            Dim desc As Decimal = 0
            Dim v_iva As Decimal = 0
            Dim TotProdu As Decimal = 0


            Dim vUss As String = String.Empty
            vUss = Context.Request.Cookies("CKSMFACTURA")("cod_usuario")

            Dim Contado As Integer
            If rdbContado.Checked = True Then
                Contado = 1
            Else
                Contado = 0
            End If

            Dim vext As Integer
            If rdbInterno.Checked = True Then
                vext = 0
            Else
                vext = 1
            End If

            Dim sql As String = "SET DATEFORMAT DMY " & vbCrLf
            sql &= "EXEC Factura @opcion=4," &
                  "@codigoPais = " & Session("cod_pais") & "," &
                  "@codigoPuesto = " & Session("cod_puesto") & "," &
                  "@codigoEmpresa = " & Session("cod_empresa") & "," &
                  "@no_factura = " & txtNoFac.Text.Trim & "," &
                  "@fecha = '01/01/1900'," &
                  "@cod_producto = '" & ProductoCodProd & "'," &
                  "@consecutivoUsuario = " & vUss & "," &
                  "@porc_descuento = " & CDec(TextPorDesc.Text.Trim) & "," &
                  "@valor_descuento = " & CDec(desc) & "," &
                  "@porc_iva = " & Session("porcImp") & "," &
                  "@valor_iva = " & v_iva & "," &
                  "@anulada =  0," &
                  "@fechaHora_anulacion = '01/01/1900'," &
                  "@sub_total = " & CDec(TotProdu) & "," &
                  "@paridad = " & Session("Paridad") & "," &
                  "@codcliente = '" & Me.ddlCliente.SelectedValue & "'," &
                  "@cantidad = 0," &
                  "@bultos = 0," &
                  "@cod_und_medida = ''," &
                  "@precio_unidad = 0," &
                  "@Notas=  NULL ," &
                  "@Contado = " & Contado & "," &
                  "@codvendedor  = '" & Me.ddVendedor.SelectedValue & "'," &
                  "@cedularuc= '" & TextId.Text.Trim & "'," &
                  "@externo= " & vext & "," &
                  "@desc_imprimir = '" & Session("desimprimir") & "'," &
                  "@NombreCliente = '" & TextNomClien.Text & "'"

            Dim cmd As New OleDb.OleDbCommand(sql, dbCon)
            Dim netdol As Decimal
            Dim dr As System.Data.OleDb.OleDbDataReader
            dr = cmd.ExecuteReader
            If dr.Read() Then
                TextLibras.Text = dr("Cantidad").ToString().Trim
                TextSubtotal.Text = dr("Subtotal").ToString().Trim
                TextIVA.Text = dr("Iva").ToString().Trim
                TextNeto.Text = dr("Neto").ToString().Trim
                netdol = dr("NetoDol").ToString.Trim
                TextNetoDol.Text = dr("NetoDol").ToString.Trim
                Session("DatosFact") = "Neto Factura en: C$ " + TextNeto.Text + " $ " + netdol.ToString("N2") + " Paridad " + Session("Paridad")
            End If

            dr.Close()
            Load_GridView()


            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "EXECUTE", "EnableButtom()", True)

        Catch ex As Exception
            MessageText = "alertify.error('Ha ocurrido un error al intentar eliminar los datos. Si el problema persiste, contacte con el administrador. " & Replace(ex.Message, "'", "") & "');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessageText, True)
        Finally
            If dbCon.State = ConnectionState.Open Then
                dbCon.Close()
            End If
        End Try

    End Sub

    Private Sub GuardarTmpFact()
        Dim dbCon As New System.Data.OleDb.OleDbConnection(conn.Conn)
        Try
            If dbCon.State = ConnectionState.Closed Then
                dbCon.Open()
            End If

            If Me.TextPorDesc.Text = String.Empty Then
                Me.TextPorDesc.Text = 0
            End If

            Dim desc As Decimal
            Dim v_iva As Decimal
            Dim TotProdu As Decimal
            TotProdu = CDec(Me.TextTotal.Text)
            desc = TotProdu * (CDec(TextPorDesc.Text) / 100)
            If Session("Imtos") = True Then   ''variable se llena desde el inventario 
                v_iva = (TotProdu - desc) * (Session("porcImp") / 100)
            Else
                v_iva = 0
            End If

            Dim vUss As String = String.Empty
            vUss = Context.Request.Cookies("CKSMFACTURA")("cod_usuario")

            Dim Contado As Integer
            Dim cliente As String = Me.ddlCliente.Text.ToString
            Dim Codcliente As String
            If rdbContado.Checked = True Then
                Contado = 1
                Session("condicion") = "Factura es de Contado"
                If Me.ddlCliente.Text.ToString = String.Empty Or Me.ddlCliente.Text.ToString = "0" Then
                    cliente = TextNomClien.Text.Trim
                    Codcliente = "0"
                Else
                    cliente = Me.ddlCliente.Text.ToString
                    Codcliente = Me.ddlCliente.SelectedValue
                End If
            Else
                Contado = 0
                Session("condicion") = "Factura es de Credito"
                cliente = Me.ddlCliente.Text.ToString
                Codcliente = Me.ddlCliente.SelectedValue
            End If
            ' If TextId.Text = String.Empty Then

            'End If

            If Me.ddVendedor.SelectedValue = String.Empty Then
                Me.ddVendedor.SelectedValue = Session("vendedor")
            End If



            Dim vext As Integer
            If rdbInterno.Checked = True Then
                vext = 0
            Else
                vext = 1
            End If
            Session("NoFactura") = txtNoFac.Text
            Dim sql As String = "SET DATEFORMAT DMY " & vbCrLf
            sql &= "EXEC Factura @opcion=1," &
                  "@codigoPais = " & Session("cod_pais") & "," &
                  "@codigoPuesto = " & Session("cod_puesto") & "," &
                  "@codigoEmpresa = " & Session("cod_empresa") & "," &
                  "@no_factura = " & txtNoFac.Text.Trim & "," &
                  "@fecha = '01/01/1900'," &
                  "@cod_producto = '" & Me.ddlProducto.SelectedValue & "'," &
                  "@consecutivoUsuario = " & vUss & "," &
                  "@porc_descuento = " & CDec(TextPorDesc.Text.Trim) & "," &
                  "@valor_descuento = " & CDec(desc) & "," &
                  "@porc_iva = " & Session("porcImp") & "," &
                  "@valor_iva = " & v_iva & "," &
                  "@anulada =  0," &
                  "@fechaHora_anulacion = '01/01/1900'," &
                  "@sub_total = " & CDec(TotProdu) & "," &
                  "@paridad = " & Session("Paridad") & "," &
                  "@codcliente = '" & Codcliente & "'," &
                  "@cantidad = " & CDec(Me.TextCantidad.Text) & "," &
                  "@bultos = " & CDec(Me.TextBultos.Text) & "," &
                  "@cod_und_medida = " & Session("Unidad") & "," &
                  "@precio_unidad = " & CDec(Me.TextPrecio.Text) & "," &
                  "@Notas=   = NULL ," &
                  "@Contado = " & Contado & "," &
                  "@codvendedor  = '" & Me.ddVendedor.SelectedValue & "'," &
                  "@cedularuc= '" & TextId.Text.Trim & "'," &
                  "@externo= " & vext & "," &
                  "@desc_imprimir = '" & Session("desimprimir") & "'," &
                  "@NombreCliente = '" & cliente & "'"


            'Dim ds As DataSet
            'ds = DataBase.GetDataSet(sql)
            'dtTabla = ds.Tables(0)
            'Me.GridViewOne.DataSource = dtTabla.DefaultView
            'Me.GridViewOne.DataBind()
            'ds.Dispose()
            'ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "smf_Script", "responsive_grid();", True)
            'Dim netdol As Decimal
            'If dtTabla.Rows.Count >= 1 Then
            '    TextLibras.Text = dtTabla.Rows(0).Item("CantTota").ToString().Trim
            '    TextSubtotal.Text = dtTabla.Rows(0).Item("Subtotal").ToString().Trim
            '    TextIVA.Text = dtTabla.Rows(0).Item("Iva").ToString().Trim
            '    TextNeto.Text = dtTabla.Rows(0).Item("Neto").ToString().Trim
            '    netdol = dtTabla.Rows(0).Item("NetoDol").ToString.Trim
            '    Session("DatosFact") = "Neto Factura en: C$ " + TextNeto.Text + " $ " + netdol.ToString("N2") + " Paridad " + Session("Paridad")
            'End If

            Dim cmd As New OleDb.OleDbCommand(sql, dbCon)
            Dim netdol As Decimal
            Dim dr As System.Data.OleDb.OleDbDataReader
            dr = cmd.ExecuteReader
            If dr.Read() Then
                TextLibras.Text = dr("Cantidad").ToString().Trim
                TextSubtotal.Text = dr("Subtotal").ToString().Trim
                TextIVA.Text = dr("Iva").ToString().Trim
                TextNeto.Text = dr("Neto").ToString().Trim
                netdol = dr("NetoDol").ToString.Trim
                TextNetoDol.Text = dr("NetoDol").ToString.Trim
                TextSaldoFact.Text = dr("Neto").ToString().Trim
                Session("DatosFact") = "Neto Factura en: C$ " & TextNeto.Text & " $ " & netdol.ToString("N2") & " Paridad " & Session("Paridad")
            End If

            dr.Close()
            Load_GridView()
            nuevo()
            'Response.Redirect(ResolveClientUrl("../Movimientos/Reportes/Factura/ImprimirFactura.aspx"))
            '-------------------------------------------------------------------------

            '-------------------------------------------------------------------------
            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "EXECUTE", "EnableButtom()", True)
            'Me.ltMensaje.Text = conn.pmsgBox("El registro se ha guardado de forma correcta.", "exito")
            Dim MessageText As String
            ddlProducto.Focus()
            'MessageText = "alertify.alert('El registro se ha guardado de forma correcta');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessageText, True)
            Exit Sub


        Catch ex As Exception
            'Me.ltMensaje.Text = conn.pmsgBox("Ocurrio un error al intentar guardar el registro." & ex.Message, "error")

        Finally
            If dbCon.State = ConnectionState.Open Then
                dbCon.Close()
            End If

        End Try
    End Sub

    Private Sub ProducExistEnInv()
        Try
            Dim MessageText As String = String.Empty
            If Session("INV") = True Then
                Dim Sql As String
                Sql = "EXEC VerifiInventario @opcion=1," &
                          "@codigoPais =  " & Session("cod_pais") & "  ," &
                          "@codigoEmpresa =  " & Session("cod_empresa") & "  ," &
                          "@codigoPuesto =  " & Session("cod_puesto") & " "

                Dim dt As New DataTable
                dt = DataBase.GetDateTableProcedimiento(Sql)

                If dt.Rows.Count Then
                    Dim DR As DataRow
                    DR = dt.Rows(0)
                    ' CKSMFACTURA("Productos") = DR("ProdPrecio")
                    Session("TableInven") = dt
                Else
                    MessageText = "alertify.alert('El  proceso no puede continuar. no exite Productos en inventario', function () { window.location.href = '../Default.aspx'; });"
                    ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessageText, True)
                    Session("HayInv") = "No"

                    'Server.Transfer("../Default.aspx")
                    'Response.Redirect("../Default.aspx", True)
                    ' Response.Redirect(ResolveClientUrl("../Default.aspx"))


                    Exit Sub
                End If
            End If

            'If Session("HayInv") = "No" Then
            '    Dim MessageText As String
            '    MessageText = "alertify.alert('El  proceso no puede continuar. no exite Productos en inventario');"
            '    ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessageText, True)
            '    Response.Redirect(ResolveClientUrl("../Default.aspx"))
            'End If



            'CKSMFACTURA.Expires = Now.AddDays(1)
            'Response.Cookies.Add(CKSMFACTURA)

        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.PmsgBox("Ocurrió un error al intentar cargar la tabla de inventario." & ex.Message, "error")

        End Try
    End Sub

    Private Sub ProductosPrecio()
        Try
            'Dim CKSMFACTURA As HttpCookie = Request.Cookies.Get("CKSMFACTURA")


            Dim Sql As String
            Sql = "SELECT cod_sector_mercado,cod_producto,precio FROM Precios  WHERE cod_pais= " & Session("cod_pais") & "  " &
                                           " and cod_empresa= " & Session("cod_empresa") & " " &
                                           " and cod_puesto= " & Session("cod_puesto") & " "

            Dim dt As New DataTable
            dt = DataBase.GetDateTable(Sql)

            If dt.Rows.Count Then
                Dim DR As DataRow
                DR = dt.Rows(0)
                ' CKSMFACTURA("Productos") = DR("ProdPrecio")
                Session("dataTable") = dt
            End If


            'CKSMFACTURA.Expires = Now.AddDays(1)
            'Response.Cookies.Add(CKSMFACTURA)

        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.PmsgBox("Ocurrió un error al intentar cargar el listado de rubros en la tabla." & ex.Message, "error")

        End Try
    End Sub

    Private Sub precio()
        Dim neto As Decimal
        Dim MessageText As String
        Dim dtDataTable As New DataTable
        dtDataTable = Session("dataTable")

        Dim dv As DataView = New DataView(dtDataTable)
        Dim dtNew As DataTable = dtDataTable.Clone

        Dim ProEsc As String
        ProEsc = ddlProducto.SelectedValue

        Dim merv As Integer = Session("Mercado")

        dv.Sort = "cod_sector_mercado,cod_producto"

        dv.RowFilter = ("[cod_sector_mercado]='" & merv & "' AND [cod_producto] ='" & ProEsc & "' ")

        dtNew = dv.ToTable("", True, "cod_producto", "Precio")
        dv.RowFilter = ""
        Dim vprecio As Integer
        If dtNew.Rows.Count >= 1 Then
            vprecio = dtNew.Rows(0).Item("Precio").ToString.Trim
            TextBultos.Text = 1
            neto = CDec(TextCantidad.Text.Trim) * CDec(vprecio)
            Me.TextPrecio.Text = CDec(vprecio)
            Me.TextTotal.Text = CDec(neto)
        Else
            vprecio = 0
            MessageText = "alertify.alert('Producto a Facturar, no tiene precio');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessageText, True)
            ddlProducto.Focus()
            Exit Sub

        End If


    End Sub

    Protected Sub ddlProducto_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlProducto.SelectedIndexChanged

        If Not Me.ddlProducto.SelectedValue = String.Empty Then
            nuevo()
            precio()
        End If
    End Sub
    Protected Sub ddformaPago_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddformaPago.SelectedIndexChanged
        If ddformaPago.Text = "TARJETA" Or ddformaPago.SelectedValue() = "T" Then
            ddTarjeta.Visible = True
            ddbanco.Visible = True
            Me.lblTarjeta.Visible = True
            Me.lblBanco.Visible = True
        End If
    End Sub
    Protected Sub ddMoneda_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddMoneda.SelectedIndexChanged
        If ddMoneda.Text.ToString = "DOLARES" Or ddMoneda.SelectedValue() = 2 Then
            TextValCor.Text = 0
            TextValDol.Text = 0
            TextCambioCor.Text = 0

            TextValCor.Enabled = False
            LblDolar.Visible = True
            TextValDol.Visible = True
            If CDec(TextValDol.Text) * Session("Paridad") > CDec(TextSaldoFact.Text) Then
                TextValCor.Text = CDec(TextSaldoFact.Text)
            Else
                TextValCor.Text = CDec(TextValDol.Text) * Session("Paridad")
            End If


            TextCambioCor.Text = CDec(TextValDol.Text) * Session("Paridad")
            lblConverDolACor.Visible = True
            TextCambioCor.Visible = True
            TextValCor.BackColor = Drawing.Color.AntiqueWhite
            TextValDol.Enabled = True
            'TextVuelto.Text = CDec(TextSaldoFact.Text) - CDec(TextValCor.Text)
        Else
            TextValCor.Text = 0
            TextValDol.Text = 0
            TextCambioCor.Text = 0
            LblDolar.Visible = False
            TextValDol.Visible = False
            TextValCor.Enabled = True
            TextCambioCor.Visible = False
            TextValDol.BackColor = Drawing.Color.AntiqueWhite
            TextValCor.BackColor = Drawing.Color.WhiteSmoke
            lblConverDolACor.Visible = False
            TextValDol.Visible = False
            TextValDol.Enabled = False

            'TextValDol.Text = 0
            'TextCambioCor.Text = 0
            'TextVuelto.Text = CDec(TextSaldoFact.Text) - CDec(TextValCor.Text)
        End If
    End Sub
    Protected Sub TextValDol_TextChanged(sender As Object, e As EventArgs) Handles TextValDol.TextChanged
        TextCambioCor.Text = CDec(TextValDol.Text) * Session("Paridad")
        If CDec(TextCambioCor.Text) > CDec(TextSaldoFact.Text) Then
            'TextValCor.Text = CDec(TextCambioCor.Text) - CDec(TextSaldoFact.Text)
            TextValCor.Text = CDec(TextSaldoFact.Text)
        Else
            'TextValCor.Text = CDec(TextSaldoFact.Text) - CDec(TextCambioCor.Text)
            TextValCor.Text = CDec(TextCambioCor.Text)
        End If
    End Sub
    Protected Sub TextValCor_TextChanged(sender As Object, e As EventArgs) Handles TextValCor.TextChanged
        'TextVuelto.Text = CDec(TextSaldoFact.Text) - CDec(TextValCor.Text)
    End Sub

    Protected Sub ddlCliente_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCliente.SelectedIndexChanged
        Try
            Dim codcl As String = ddlCliente.SelectedValue()
            Dim vext As Integer
            If rdbInterno.Checked = True Then
                vext = 0
            Else
                vext = 1
            End If
            TextNomClien.Text = String.Empty
            If rdbContado.Checked = True And (Me.ddlCliente.Text = String.Empty) Then
                ddlCliente.SelectedValue = "0"
                TextNomClien.Enabled = True
                TextNomClien.Focus()
                TextId.Enabled = True
                Session("Mercado") = Session("lista")
            Else
                Dim Sql As String
                If vext = "0" Then
                    Sql = "SELECT cod_sector_mercado as Merca,cedula_ruc,cod_vendedor FROM clientes  WHERE cod_cliente= '" & codcl & "'"

                Else
                    Sql = "SELECT cod_sector_mercado as Merca,cedula_ruc,cod_vendedor FROM clientes  WHERE cod_cliente= '" & codcl & "'" &
                                                    "AND externo= " & vext & ""
                End If


                Dim fr As System.Data.OleDb.OleDbDataReader = DataBase.GetDataReader(Sql)
                'If fr.Read = Nothing Then
                If fr.Read() Then
                    Session("Mercado") = fr("Merca").ToString().Trim
                    Session("vendedor") = fr("cod_vendedor").ToString().Trim
                    TextId.Text = fr("cedula_ruc").ToString().Trim
                    ddVendedor.Enabled = False
                Else
                    Session("Mercado") = Session("lista")
                    'Session("cedulaRuc") = TextId.Text.ToString
                    ddVendedor.Enabled = True
                End If
                fr.Close()
            End If
            If rdbCredito.Checked = True Then
                TextNomClien.Text = String.Empty
                TextId.Enabled = False


                Dim Sql As String
                Sql = "SELECT cod_sector_mercado as Merca,cedula_ruc FROM clientes  WHERE cod_cliente= '" & codcl & "'" &
                                                    "AND externo= " & vext & ""

                Dim fr As System.Data.OleDb.OleDbDataReader = DataBase.GetDataReader(Sql)
                If fr.Read() Then
                    Session("Mercado") = fr("Merca").ToString().Trim
                    Session("vendedor") = fr("cod_vendedor").ToString().Trim
                    TextId.Text = fr("cedula_ruc").ToString().Trim
                    ddVendedor.Enabled = False
                Else
                    Session("Mercado") = Session("lista")
                    'Session("cedulaRuc") = TextId.Text.ToString
                End If
                fr.Close()
            End If

        Catch ex As Exception
            Me.ltMensajeGrid.Text = conn.PmsgBox("Ocurrió un error en signacion de cliente eventual, combo  del cliente, evento selectChange." & ex.Message, "error")
        End Try
    End Sub

    Protected Sub TextNomClien_TextChanged(sender As Object, e As EventArgs) Handles TextNomClien.TextChanged
        ddlCliente.SelectedValue = "0"
        TextId.Enabled = True
    End Sub

    Protected Sub TextCantidad_TextChanged(sender As Object, e As EventArgs) Handles TextCantidad.TextChanged
        Inventario()
        If Me.TextPrecio.Text = String.Empty Then
            precio()
        End If
    End Sub

    Protected Sub TextPrecio_TextChanged(sender As Object, e As EventArgs) Handles TextPrecio.TextChanged
        TextTotal.Text = CDec(TextCantidad.Text) * CDec(TextPrecio.Text)
    End Sub
    Private Sub ImprimirGuardar()

        Dim MessageText As String
        Dim Sql As String
        Dim dbCon As New System.Data.OleDb.OleDbConnection(conn.Conn)
        Try
            If dbCon.State = ConnectionState.Closed Then
                dbCon.Open()
            End If
            Dim Contado As Integer
            If rdbContado.Checked = True Then
                Contado = 1
            Else
                Contado = 0
            End If


            '"@cod_und_medida = '" & Session("Unidad") & "'," & _
            Sql = ""
            If Session("Continuar") = 1 Then
                Sql = "SET DATEFORMAT DMY " & vbCrLf
                Sql &= "EXEC Factura @opcion=2," &
                      "@codigoPais = " & Session("cod_pais") & "," &
                      "@codigoPuesto = " & Session("cod_puesto") & "," &
                      "@codigoEmpresa = " & Session("cod_empresa") & "," &
                      "@no_factura = " & txtNoFac.Text.Trim & "," &
                      "@fecha = '01/01/1900'," &
                      "@cod_producto = '" & Me.ddlProducto.SelectedValue & "'," &
                      "@consecutivoUsuario = '0'," &
                      "@porc_descuento = " & CDec(TextPorDesc.Text.Trim) & "," &
                      "@valor_descuento = 0," &
                      "@porc_iva = " & Session("porcImp") & "," &
                      "@valor_iva = 0," &
                      "@anulada =  0," &
                      "@fechaHora_anulacion = '01/01/1900'," &
                      "@sub_total = 0," &
                      "@paridad = " & Session("Paridad") & "," &
                      "@codcliente = '" & Me.ddlCliente.SelectedValue & "'," &
                      "@cantidad = " & CDec(Me.TextCantidad.Text) & "," &
                      "@bultos = " & CDec(Me.TextBultos.Text) & "," &
                      "@cod_und_medida = 0," &
                      "@precio_unidad = " & CDec(Me.TextPrecio.Text) & "," &
                      "@Notas=   '0'," &
                      "@Contado = " & Contado & "," &
                      "@codvendedor  = '" & Me.ddVendedor.SelectedValue & "'," &
                      "@cedularuc= '" & TextId.Text.Trim & "'," &
                      "@externo= 0," &
                      "@desc_imprimir = '" & Session("desimprimir") & "'," &
                      "@NombreCliente = '" & TextNomClien.Text & "'"

                Dim cmd As New OleDb.OleDbCommand(Sql, dbCon)
                cmd.ExecuteNonQuery()
            Else
                ddlProducto.Focus()
                Return
            End If

            TextPorDesc.Enabled = True
            txtNoFac.Text = CDec(txtNoFac.Text) + 1
            TextNomClien.Text = String.Empty
            'TextNotas.Text = String.Empty
            TextId.Text = String.Empty
            ProducExistEnInv()
            Response.Redirect(ResolveClientUrl("../Movimientos/Reportes/Factura/ImprimirFactura.aspx"))

        Catch ex As Exception
            MessageText = "alertify.error('Ha ocurrido un error al intentar guardar la factura. Si el problema persiste, contacte con el administrador. " & Replace(ex.Message, "'", "") & "');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessageText, True)

        Finally
            If dbCon.State = ConnectionState.Open Then
                dbCon.Close()
            End If

        End Try
    End Sub
    Protected Sub BtnGuardar_Click(sender As Object, e As EventArgs) Handles BtnGuardar.Click
        Dim MessageText As String
        Dim Sql As String
        Try
            Dim formap As String = Session("Cod_FormaPago")
            Dim monecod As Integer = Session("Cod_Moneda")
            Dim prue As String = ddformaPago.SelectedValue
            'Me.CFormaPago.SelectedValue = formap
            'Me.CMoneda.SelectedValue = monecod
            Me.ddformaPago.SelectedValue = formap
            Me.ddMoneda.SelectedValue = monecod

            Sql = "select  sub_total as Valor " &
                " FROM  factura_enc  WHERE cod_pais= " & Session("cod_pais") & " " &
                                    " AND cod_pais= " & Session("cod_pais") & " " &
                                    " AND cod_empresa= " & Session("cod_empresa") & " " &
                                    " and no_factura= " & txtNoFac.Text.Trim & " "


            Dim DataSet As New DataSet
            DataSet = DataBase.GetDataSet(Sql)
            If DataSet.Tables(0).Rows.Count <> 0 Then
                MessageText = "alertify.alert('Numero de Factura ya existe proceso se cancelara');"
                ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessageText, True)
                Exit Sub
            End If



            Sql = "select  isnull(sum(sub_total),0) as Valor " &
                  " FROM  TmpDetFact  WHERE cod_pais= " & Session("cod_pais") & " " &
                                      " AND cod_pais= " & Session("cod_pais") & " " &
                                      " AND cod_empresa= " & Session("cod_empresa") & " " &
                                      " and no_factura= " & txtNoFac.Text.Trim & " "


            Dim fr As System.Data.OleDb.OleDbDataReader = DataBase.GetDataReader(Sql)
            If fr.Read() Then
                If rdbContado.Checked = True Then
                    TextPorDesc.Enabled = False
                    DistrPago()
                    LblResp.Text = Session("DatosFact")
                    ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Facturacion", "open_popup();", True)
                    'If Session("Continuar") = 1 Then
                    '    ImprimirGuardar()
                    'End If

                End If
                If rdbCredito.Checked = True Then
                    ImprimirGuardar()
                    'Session("Continuar") = 1
                End If
            Else
                MessageText = "alertify.alert('Registre Productos a Facturar');"
                ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessageText, True)
                Exit Sub
            End If
            fr.Close()
        Catch ex As Exception
            MessageText = "alertify.error('Ha ocurrido un error al intentar guardar la factura. Si el problema persiste, contacte con el administrador. " & Replace(ex.Message, "'", "") & "');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessageText, True)

        Finally
            'If dbCon.State = ConnectionState.Open Then
            '    dbCon.Close()
            'End If

        End Try
    End Sub

    Protected Sub BtnRegresa_Click(sender As Object, e As EventArgs) Handles BtnRegresa.Click
        Session("Continuar") = 0
        Dim dt As New DataTable
        dt = Session("DistriPago")
        dt.Clear()
    End Sub

    Protected Sub BtnPopupImpGuar_Click(sender As Object, e As EventArgs) Handles BtnPopupImpGuar.Click
        Dim MessageText As String
        Session("Continuar") = 1
        Dim dt As New DataTable
        dt = Session("DistriPago")
        TextValCor.Text = String.Empty
        Dim NO As Integer = txtNoFac.Text
        If dt.Rows.Count <> 0 Then
            'dt.Tables("Ordenes").Compute("Sum(Total)", "CodigoVendedor = 5") filtra
            Dim sumObject As Object = dt.Compute("Sum(ValorRecibidoCor)", "No_Factura= " & NO & " ")
            Dim valor As Decimal = IIf(IsDBNull(sumObject) = True, 0, sumObject)
            'valor = dt.Compute("Sum(ValorRecibidoCor)", "No_Factura= " & txtNoFac.Text.Trim & " ")
            If CDec(TextSaldoFact.Text) <> 0 Then
                If CDec(TextNeto.Text) - CDec(valor) > 1 Then
                    MessageText = "alertify.alert('Saldo de la factura no esta cancelada');"
                    ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessageText, True)
                    Exit Sub
                End If
            Else
                GuarFomaPago()
                ImprimirGuardar()
            End If
        Else
            MessageText = "alertify.alert('No ha ingresado forma de pago de Factura');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessageText, True)
            Exit Sub
        End If
    End Sub
    Private Sub GuarFomaPago()
        Dim MessageText As String
        Dim Sql As String
        Dim dbCon As New System.Data.OleDb.OleDbConnection(conn.Conn)
        Try
            If dbCon.State = ConnectionState.Closed Then
                dbCon.Open()
            End If

            Dim dt As New DataTable
            dt = Session("DistriPago")
            Dim codformapa As String

            If rdbContado.Checked = True Then

            End If
            Sql = ""
            'If Session("Continuar") = 1 Then

            For Each row As DataRow In dt.Rows
                '@Cod_ForPago =Convert.ToString(row("cod_FormaPago")))
                'cmd.Parameters.AddWithValue("@param2", Convert.ToInt32(row("campo2")))
                ' COD_FORMA = Convert.ToString(row("cod_FormaPago"))

                Sql = "SET DATEFORMAT DMY " & vbCrLf
                Sql &= "EXEC GuardarFomaPago @opcion=1," &
                      "@codigoPais = " & Session("cod_pais") & "," &
                      "@codigoPuesto = " & Session("cod_puesto") & "," &
                      "@codigoEmpresa = " & Session("cod_empresa") & "," &
                      "@no_factura = " & Convert.ToString(row("No_Factura")) & "," &
                      "@Fecha_Factura = '01/01/1900'," &
                      "@cod_ForPago = '" & Convert.ToString(row("cod_FormaPago")) & "'," &
                      "@Desc_ForPago = '" & Convert.ToString(row("cod_FormaPago")) & "'," &
                      "@Cod_Moneda = '" & Convert.ToString(row("Cod_Moneda")) & "'," &
                      "@Contado = 1," &
                      "@ValorFacturaCor = '" & Convert.ToDecimal(row("ValorFacturaCor")) & "'," &
                      "@ValorFacturaDol = '" & Convert.ToDecimal(row("ValorFacturaDOL")) & "'," &
                      "@ValorRecibidoCor = '" & Convert.ToDecimal(row("ValorRecibidoCor")) & "'," &
                      "@ValorRecibidoDol = '" & Convert.ToDecimal(row("ValorRecibidoDOL")) & "'," &
                      "@Vuelto = '" & Convert.ToDecimal(row("Vuelto")) & "'," &
                      "@Paridad = '" & Convert.ToDecimal(row("Paridad")) & "'," &
                      "@Anulada = 0," &
                      "@ComisionTarjeta = '" & Convert.ToDecimal(row("ComisionTarjeta")) & "'," &
                      "@Cod_tarjeta = '" & Convert.ToDecimal(row("Cod_tarjeta")) & "'"


            Next
            '            End Using


            Dim cmd As New OleDb.OleDbCommand(Sql, dbCon)
            cmd.ExecuteNonQuery()


        Catch ex As Exception
            MessageText = "alertify.error('Ha ocurrido un error al intentar guardar la factura. Si el problema persiste, contacte con el administrador. " & Replace(ex.Message, "'", "") & "');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessageText, True)

        Finally
            If dbCon.State = ConnectionState.Open Then
                dbCon.Close()
            End If

        End Try
    End Sub

    Private Sub DistrPago()
        'Dim table As New DataTable()
        'table.Columns.Add(New DataColumn("MyColumn"))
        'Dim primaryKey(1) As DataColumn
        'primaryKey(0) = table.Columns("MyColumn")
        'table.PrimaryKey = primaryKey
        Try
            Dim Contado As Integer
            If rdbContado.Checked = True Then
                Contado = 1
            Else
                Contado = 0
            End If

            ddTarjeta.Visible = False
            ddbanco.Visible = False
            Me.lblTarjeta.Visible = False
            Me.lblBanco.Visible = False

            Dim dt As New DataTable()
            'columnas
            dt.Columns.Add("cod_FormaPago")
            dt.Columns.Add("No_Factura")
            'primaryKey(0) = dt.Columns("cod_FormaPago")
            'dt.PrimaryKey = primaryKey

            dt.PrimaryKey = New DataColumn() {dt.Columns("cod_FormaPago"),
            dt.Columns("No_Factura")}
            dt.Columns.Add("Cod_Pais")
            dt.Columns.Add("Cod_Empresa")
            dt.Columns.Add("Cod_Puesto")


            dt.Columns.Add("Cod_Moneda")
            dt.Columns.Add("contado_credito")
            dt.Columns.Add("ValorFacturaCor")

            dt.Columns.Add("ValorFacturaDol")
            'ValorRecibido,ValorRecibidoDol
            dt.Columns.Add("ValorRecibidoCor", Type.GetType("System.Decimal"))
            dt.Columns.Add("ValorRecibidoDol", Type.GetType("System.Decimal"))
            dt.Columns.Add("Vuelto")

            dt.Columns.Add("Paridad", Type.GetType("System.Decimal"))
            dt.Columns.Add("Anulada")
            dt.Columns.Add("ComisionTarjeta", Type.GetType("System.Decimal"))
            dt.Columns.Add("Cod_tarjeta")

            dt.Columns.Add("FormaPago")
            dt.Columns.Add("Moneda")

            ''Guarda
            Dim row As DataRow = dt.NewRow()
            row("No_Factura") = txtNoFac.Text.Trim
            row("Cod_Pais") = Session("cod_pais")
            row("Cod_Empresa") = Session("cod_empresa")
            row("Cod_Puesto") = Session("cod_puesto")


            'CDec(TextValCor.Text)

            Dim desForma As String = ddformaPago.SelectedItem.Text
            Dim desMoneda As String = ddMoneda.SelectedItem.Text
            row("cod_FormaPago") = ddformaPago.SelectedValue
            row("Cod_Moneda") = ddMoneda.SelectedValue
            row("contado_credito") = Contado
            row("ValorFacturaCor") = CDec(TextNeto.Text)

            row("ValorFacturaDol") = CDec(TextNeto.Text) / Session("Paridad")
            row("ValorRecibidoCor") = CDec(TextNeto.Text)
            row("ValorRecibidoDol") = CDec(TextNeto.Text) / Session("Paridad")
            row("Vuelto") = 0

            row("Paridad") = Session("Paridad")
            row("Anulada") = 0
            row("ComisionTarjeta") = 0
            row("cod_Tarjeta") = ddTarjeta.SelectedValue
            row("FormaPago") = desForma
            row("Moneda") = desMoneda

            dt.Rows.Add(row)


            Me.GridViewpop.DataSource = dt
            Me.GridViewpop.DataBind()

            Session("DistriPago") = dt
            Session("Continuar") = 1
        Catch ex As Exception
            Me.LiteralGridpop.Text &= conn.PmsgBox("Ocurrió un error al intentar cargar la tabla del pop. forma de pago" & ex.Message, "error")
            Session("Continuar") = 0
        End Try
    End Sub
#Region "PROCESOS Y EVENTOS DEL GRIDVIEW FORMA DE PAGO "
    Protected Sub GridViewpop_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridViewpop.DataBound
        Try
            If GridViewpop.Rows.Count > 0 Then
                Dim pagerRow As GridViewRow = GridViewpop.BottomPagerRow
                Dim pageLabel As Label = CType(pagerRow.Cells(0).FindControl("CurrentPageLabel"), Label)
                If Not pageLabel Is Nothing Then
                    Dim currentPage As Integer = GridViewpop.PageIndex + 1
                    pageLabel.Text = "&nbsp;&nbsp; Pagina " & currentPage.ToString() &
                        " de " & GridViewpop.PageCount.ToString()
                End If
            End If
        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.PmsgBox("Ocurrió un error al disparar el evento DataBound. DEL GRID FORMA DE PAGO" & ex.Message, "error")

        End Try
    End Sub

    Protected Sub GridViewpop_PageIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridViewpop.PageIndexChanged
        Try

            Me.GridViewpop.SelectedIndex = -1
            Me.HiddenField2.Value = String.Empty

            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "smf_Script", "responsive_grid();", True)

        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.PmsgBox("Ocurrió un error al disparar el evento PageIndexChanged. DEL GRID FORMA DE PAGO" & ex.Message, "error")

        End Try
    End Sub

    Protected Sub GridViewpop_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridViewpop.PageIndexChanging
        Try
            Me.GridViewpop.PageIndex = e.NewPageIndex

            'Para usar la de caché guardada en la variable de sesion
            If (IsPostBack) AndAlso (Not dtTabla Is Nothing) Then
                If Not dtTabla Is Nothing AndAlso dtTabla.Rows.Count > 0 Then
                    If dtTabla.Rows.Count > 0 Then
                        Me.GridViewpop.DataSource = dtTabla
                        Me.GridViewpop.DataBind()
                    End If
                End If
            End If

            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "smf_Script", "responsive_grid();", True)

        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.PmsgBox("Ocurrió un error al disparar el evento PageIndexChanging. DEL GRID FORMA DE PAGO" & ex.Message, "error")

        End Try
    End Sub

    Protected Sub GridViewpop_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridViewpop.RowDataBound
        Try

            If e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Attributes.Add("OnMouseOver", "On(this);")
                e.Row.Attributes.Add("OnMouseOut", "Off(this);")
                'e.Row.Attributes.Add("OnClick", "" & Page.ClientScript.GetPostBackClientHyperlink(Me.GridViewOne, "Select$" + e.Row.RowIndex.ToString) & ";")
            End If

        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.PmsgBox("Ocurrió un error al disparar el evento RowDataBound. DEL GRID FORMA DE PAGO " & ex.Message, "error")

        End Try
    End Sub

    'Protected Sub GridViewpop_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles GridViewpop.RowDeleting
    '    Try
    '        'pasar la información del Gridview hacia otro control, en este caso el control HiddenField,
    '        Me.HiddenField2.Value = Me.GridViewpop.DataKeys(e.RowIndex).Value

    '        EliminarFormaPago = GridViewpop.SelectedDataKey.Values(0)
    '        'ProductoEmpre = GridViewpop.SelectedDataKey.Values(1)
    '        'ProductoPuesto = GridViewpop.SelectedDataKey.Values(2)
    '        ProductoNoFac = GridViewpop.SelectedDataKey.Values(1)
    '        ContainsArray()

    '        ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "smf_Script", "responsive_grid();", True)

    '    Catch ex As Exception
    '        Me.ltMensajeGrid.Text &= conn.pmsgBox("Ocurrió un error al disparar el evento RowDeleting del grid " & ex.Message, "error")

    '    End Try
    'End Sub
    'Private Sub RemoveFoundRow(ByVal table As DataTable)
    '    Dim rowCollection As DataRowCollection = table.Rows
    '    ''borrar
    '    ' Test to see if the collection contains the value.
    '    If rowCollection.Contains(EliminarFormaPago) Then
    '        Dim foundRow As DataRow = rowCollection.Find(TextBox1.Text)
    '        rowCollection.Remove(foundRow)
    '        Console.WriteLine("Row Deleted")
    '    Else
    '        Console.WriteLine("No such row found.")
    '    End If
    'End Sub
    Private Sub ContainsArray()
        Dim arrKeyVals(1) As Object
        'Dim table As DataTable = CType(GridViewpop.DataSource, DataTable)
        Dim table As New DataTable
        table = Session("DistriPago")
        Dim rowCollection As DataRowCollection = table.Rows
        arrKeyVals(0) = EliminarFormaPago
        arrKeyVals(1) = ProductoNoFac
        'arrKeyVals(2) = ProductoEmpre
        'arrKeyVals(3) = ProductoPuesto
        Dim foundRow As DataRow = table.Rows.Find((arrKeyVals))
        table.Rows.Remove(foundRow)
        Me.GridViewpop.DataSource = table
        Me.GridViewpop.DataBind()

        Dim dt As New DataTable
        dt = Session("DistriPago")

        If (dt.Rows.Count <> 0) Then
            Dim valor As Decimal
            valor = dt.Compute("Sum(ValorRecibidoCor)", "No_Factura= " & txtNoFac.Text.Trim & " ")
            If CDec(TextNeto.Text) - CDec(valor) <> 0 Then
                TextSaldoFact.Text = CDec(TextNeto.Text) - CDec(valor)
            Else
                TextSaldoFact.Text = 5
            End If
        Else
            TextSaldoFact.Text = CDec(TextNeto.Text)
        End If
        TextValDol.Text = 0
        TextValCor.Text = 0
        'Dim foundRow As DataRow = rowCollection.Find((arrKeyVals(1)).ToString())
        'rowCollection.Remove(foundRow)

        Dim valsal As Decimal = TextSaldoFact.Text

        TextSaldoFact.Text = valsal

    End Sub

    Protected Sub GridViewpop_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridViewpop.SelectedIndexChanged
        Try
            '"cod_pais,cod_empresa,cod_puesto,no_factura,fecha,cod_producto"  
            Me.HiddenField2.Value = Me.GridViewpop.SelectedValue.ToString
            EliminarFormaPago = GridViewpop.SelectedDataKey.Values(0)
            'ProductoEmpre = GridViewpop.SelectedDataKey.Values(1)
            'ProductoPuesto = GridViewpop.SelectedDataKey.Values(2)
            ProductoNoFac = GridViewpop.SelectedDataKey.Values(1)

            ContainsArray()

            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "smf_Script", "responsive_grid();", True)

        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.PmsgBox("Ocurrió un error al disparar el evento SelectedIndexChanged. " & ex.Message, "error")

        End Try
    End Sub
    Protected Sub GridViewpop_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles GridViewpop.RowCancelingEdit
        Me.GridViewpop.EditIndex = -1
        '    Load_GridView()
    End Sub

    Protected Sub GridViewpop_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles GridViewpop.RowEditing
        Try
            Me.GridViewpop.EditIndex = e.NewEditIndex
            'Load_GridView()
        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.PmsgBox("Ocurrió un error al disparar el evento SelectedIndexChanged. " & ex.Message, "error")
        End Try
    End Sub
#End Region

    Protected Sub BtnAdiciFormaPago_Click(sender As Object, e As EventArgs) Handles BtnAdiciFormaPago.Click
        Try
            'Dim suma As Object = objetoDataTable.Compute("Sum(monto)", "Cuenta=numeroCuenta") " & CDec(txtNoFac.Text) & "
            Dim valor As Object
            Dim dt As New DataTable
            dt = Session("DistriPago")
            If (dt.Rows.Count <> 0) Then
                valor = dt.Compute("SUM(ValorRecibidoCor)", "No_Factura= " & txtNoFac.Text.Trim & " ")
            Else
                valor = 0
            End If

            'Dim Sql As String
            'Sql = "select sum(valorRecibidoCor) as Valor " & _
            '      " FROM Distribucion_Pago WHERE cod_pais= " & Session("cod_pais") & " " & _
            '                          " AND cod_pais= " & Session("cod_pais") & " " & _
            '                          " AND cod_empresa= " & Session("cod_empresa") & " " & _
            '                          " and no_factura= " & txtNoFac.Text.Trim & " "


            'Dim fr As System.Data.OleDb.OleDbDataReader = DataBase.GetDataReader(Sql)

            'If fr.Read() Then
            '    valor = fr.Item("valor").ToString()
            'End If
            'fr.Close()

            Dim MessageText As String
            If CDec(TextNeto.Text) < (CDec(valor) + CDec(TextValCor.Text)) Then
                MessageText = "alertify.alert('El valor del pago es mayor que la factura, registro no se guardara');"
                ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessageText, True)
                Exit Sub
            End If
            If CDec(TextNeto.Text) = (CDec(valor)) Then
                MessageText = "alertify.alert('La suma de las diferentes formas de pago, es igual al neto de la factura, registro no se guardara');"
                ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessageText, True)
                Exit Sub
            End If
            If CDec(TextNeto.Text) < (CDec(valor) + CDec(TextSaldoFact.Text)) Then
                MessageText = "alertify.alert('Lo digitado excede al valor de la factura');"
                ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessageText, True)
                Exit Sub
            End If

            If Me.ddformaPago.Text = String.Empty Then
                MessageText = "alertify.alert('Tiene que escojer una Forma de pago');"
                ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessageText, True)
                Exit Sub
            End If

            If Me.ddformaPago.SelectedValue = "T" Then
                If Me.ddTarjeta.Text = String.Empty Then
                    MessageText = "alertify.alert('Tiene que escojer una tarjeta');"
                    ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessageText, True)
                    Exit Sub
                Else

                End If
            End If

            Dim Contado As Integer
            If rdbContado.Checked = True Then
                Contado = 1
            Else
                Contado = 0
            End If

            'Agregar Datos EN DATATABLE
            'Dim dr As DataRow
            'dr = dt.Rows.Find(New Object() {"" & ddformaPago.SelectedValue & "", " " & txtNoFac.Text.Trim & ""})
            'dt.Rows.Remove(dr)

            Dim arrKeyVals(1) As Object
            Dim rowCollection As DataRowCollection = dt.Rows
            arrKeyVals(0) = ddformaPago.SelectedValue
            arrKeyVals(1) = txtNoFac.Text.Trim
            Dim foundRow As DataRow = dt.Rows.Find((arrKeyVals))

            'If Not foundRow(0).ToString = String.Empty Then
            If IsNothing(foundRow) = False Then
                dt.Rows.Remove(foundRow)
            End If

            'If dr Is Nothing Then
            'No se encontró la fila. Crear nueva fila
            Dim row As DataRow = dt.NewRow()
            row("No_Factura") = txtNoFac.Text.Trim
            row("Cod_Pais") = Session("cod_pais")
            row("Cod_Empresa") = Session("cod_empresa")
            row("Cod_Puesto") = Session("cod_puesto")

            row("cod_FormaPago") = ddformaPago.SelectedValue
            row("Cod_Moneda") = ddMoneda.SelectedValue
            row("contado_credito") = Contado
            row("ValorFacturaCor") = CDec(TextNeto.Text)

            row("ValorFacturaDol") = CDec(TextNeto.Text) / Session("Paridad")
            row("ValorRecibidoCor") = CDec(TextValCor.Text)
            row("ValorRecibidoDol") = CDec(TextValCor.Text) / Session("Paridad")
            row("Vuelto") = 0

            row("Paridad") = Session("Paridad")
            row("Anulada") = 0
            row("ComisionTarjeta") = 0
            row("cod_Tarjeta") = ddTarjeta.SelectedValue

            row("FormaPago") = ddformaPago.Text
            row("Moneda") = ddMoneda.Text

            dt.Rows.Add(row)
            Me.GridViewpop.DataSource = dt.DefaultView
            Me.GridViewpop.DataBind()
            Dim dif As Decimal

            valor = dt.Compute("Sum(ValorRecibidoCor)", "No_Factura= " & CDec(txtNoFac.Text) & " ")
            If CDec(TextNeto.Text) - CDec(valor) > 0 Then
                dif = CDec(TextNeto.Text) - CDec(valor)
                If dif > TextSaldoFact.Text Then
                    TextValCor.Text = TextSaldoFact.Text
                End If
                dif = 0
                dif = CDec(TextSaldoFact.Text) - CDec(TextValCor.Text)
                TextSaldoFact.Text = dif
                'TextSaldoFact.Text = CDec(TextNeto.Text) - CDec(valor) Then
            Else
                TextSaldoFact.Text = 0
            End If

            TextValDol.Text = 0
            'TextValCor.Text = 0
            'Me.GridViewpop.DataSource = dt
            'Me.GridViewpop.DataBind()


            'Else
            ' dt.Rows.Remove(dr)
            'Fila encontrada
            'MiTabla.PrimaryKey = New DataColumn() {MiTabla.Columns("ID")}
            'Dim f As DataRow = MiTabla.Rows.Find(codigoabuscar)
            'MiTabla.Rows.Remove(f)
            ' End If


        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.PmsgBox("Ocurrió un error al intentar cargar el listado de rubros en la tabla." & ex.Message, "error")

        End Try
    End Sub


End Class
