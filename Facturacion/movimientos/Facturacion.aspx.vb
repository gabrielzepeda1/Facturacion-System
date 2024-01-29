Imports System.Data
Imports System.Data.OleDb

Partial Public Class movimientos_Factura

    Inherits Page

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
            Dim arrPath As String() = HttpContext.Current.Request.RawUrl.Split("/")

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

        If Not IsPostBack Then

            System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")

            'Paridad()
            CboFormaPago()
            CboProductos()
            CboCliente()
            CboVendedor()
            NoFactura()
            ProductosPrecio()

            txtNoFactura.BackColor = Drawing.Color.AntiqueWhite
            TextTotal.Enabled = False
            TextSaldoFact.BackColor = Drawing.Color.AntiqueWhite
            TextTotal.BackColor = Drawing.Color.AntiqueWhite
            ddlTarjeta.Visible = False
            lblTarjeta.Visible = False
            ddlBanco.Visible = False
            lblBanco.Visible = False

            Session("Mercado") = Session("lista")

            If Session("CambiaPrecio") = "Si" Then
                TextPrecio.Enabled = True
            Else
                TextPrecio.Enabled = False
                TextPrecio.BackColor = Drawing.Color.AntiqueWhite
            End If

            ProducExistEnInv()
            Load_GridView()
            DisableEnable()

            rdbCredito.Focus()

        End If

        If Session("VerifInven") = "Si" Then
            TextCantidad.Attributes.Add("onblur", Me.Page.ClientScript.GetPostBackEventReference(Me.TextCantidad, ""))
        End If

    End Sub

    Private Sub DisableEnable()

        TextId.Enabled = False
        TextNombreCliente.Enabled = False

        If TextId.Enabled = False Then
            TextId.BackColor = Drawing.Color.WhiteSmoke
        End If

        If TextNombreCliente.Enabled = False Then
            TextNombreCliente.BackColor = Drawing.Color.WhiteSmoke
        End If

    End Sub

#Region "COMBOS"
    Private Sub ProducExistEnInv()
        Try
            Dim MessageText As String = String.Empty

            If Session("INV") Then
                Dim Sql As String
                Sql = "EXEC VerifiInventario @opcion=1," &
                          "@codigoPais =  " & Session("cod_pais") & "  ," &
                          "@codigoEmpresa =  " & Session("cod_empresa") & "  ," &
                          "@codigoPuesto =  " & Session("cod_puesto") & " "

                Dim dt As New DataTable
                dt = DataBase.GetDataTableProc(Sql)

                If Sql.Contains(Session("cod_pais")) AndAlso Sql.Contains(Session("cod_empresa")) AndAlso Sql.Contains(Session("cod_puesto")) Then

                    If dt.Rows.Count >= 1 Then
                        Dim DR As DataRow
                        DR = dt.Rows(0)
                        'CKSMFACTURA("Productos") = DR("ProdPrecio")
                        Session("TableInven") = dt
                    Else
                        MessageText = "alertify.alert('El proceso no puede continuar. no existe productos en inventario', function () { window.location.href = '../Default.aspx'; });"
                        ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Message", MessageText, True)
                        Session("HayInv") = "No"


                    End If
                End If
            End If

        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.PmsgBox("Ocurrió un error al intentar cargar la tabla de inventario." & ex.Message, "error")

        End Try
    End Sub

    Private Sub NoFactura()

        Dim vCcod_usuario As String = Context.Request.Cookies("CKSMFACTURA")("cod_usuario")

        'The first line of the sub-routine declares a variable called "vCcod_usuario" And assigns it the value of a cookie called "CKSMFACTURA" that contains a "cod_usuario" field. This line of code assumes that the cookie has already been set elsewhere in the application.

        Dim sql As String
        sql = " SET DATEFORMAT DMY " & "SELECT no_factura+1 as NoFactura,verificar_inventario AS INV FROM puestos WHERE cod_pais= " & Session("CodigoPais") & "  " &
              " and cod_empresa= " & Session("CodigoEmpresa") & " " &
             " and cod_puesto= " & Session("CodigoPuesto") & " "

        'The next line declares a SQL query that will be used to retrieve the next invoice number for a particular user, company, and location. The query selects the "no_factura" field from a database table called "puestos" and adds 1 to it to generate the next invoice number. It also selects a field called "verificar_inventario" from the same table and assigns it to a variable called "INV". The SQL query uses the values of three Session variables called "cod_pais", "cod_empresa", and "cod_puesto" to filter the results.


        '" and consecutivo_usuario= " & vCcod_usuario & " "
        Dim fr As System.Data.OleDb.OleDbDataReader = DataBase.GetDataReader(sql)

        'The next line of code executes the SQL query and retrieves a data reader object called "fr".

        If fr.Read() Then
            txtNoFactura.Text = fr.Item("NoFactura").ToString()
            'rdbContado.Checked = True
            'rdbCredito.Checked = False
            'rdbExterno.Checked = False
            'rdbContado.Checked = False
            Session("INV") = fr.Item("INV").ToString()
            'ddlCliente.Focus()
        End If

        'The next line of code uses the data reader object to read the results of the query. If the query returned any rows, the code sets the value of the "txtNoFactura" textbox to the next invoice number, sets the "rdbContado" radio button to "checked", sets the "rdbCredito" radio button to "unchecked", assigns the value of "INV" to a Session variable called "INV", and sets the focus to a dropdown list called "ddlCliente".
    End Sub

    Private Sub CboVendedor()
        Try
            'The first line of the sub-routine declares a "Try...Catch" block to handle any exceptions that might occur while executing the code.

            Dim dataSet As New DataSet

            'The second line of code declares a new DataSet object called "dataSet".

            dataSet = DataBase.GetDataSet(" EXEC CombosProductos " &
                  "@opcion = 10," &
                  "@codigo = " & Session("cod_pais") & " ")

            'The third line of code uses the "DataBase.GetDataSet" method to retrieve a dataset containing the list of vendors from a database stored procedure called "CombosProductos". The stored procedure takes two input parameters: "opcion" and "codigo". The value of "opcion" is set to 10, and the value of "codigo" is taken from a Session variable called "cod_pais". This code assumes that the "CombosProductos" stored procedure exists and returns a dataset containing a table with columns called "Vendedor" and "codVendedor".


            If dataSet.Tables(0).Rows.Count > 0 Then
                ddlVendedor.DataSource = dataSet.Tables(0)
                ddlVendedor.DataTextField = "Vendedor"
                ddlVendedor.DataValueField = "codVendedor"
                ddlVendedor.DataBind()
            End If

            'ddlVendedor.Items.Insert(0, New ListItem("-Seleccione-", 0))



            'The next line of code checks if the dataset returned any rows. If the dataset contains rows, the code sets the data source of a dropdown list called "ddlVendedor" to the first table in the dataset, sets the "DataTextField" property of the dropdown list to "Vendedor", sets the "DataValueField" property of the dropdown list to "codVendedor", and binds the dropdown list to its data source.

            dataSet.Dispose()

            'The next line of code disposes of the dataset object to free up system resources.

        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.PmsgBox("Ocurrió un error al intentar cargar el listado de rubros en la tabla. de vendedor" & ex.Message, "error")

            'If any exceptions occur during the execution of the "Try...Catch" block, the code in the "Catch" block is executed. This code displays an error message using a custom message box method called "conn.pmsgBox".

        End Try
    End Sub

    Private Sub CboCliente()

        Dim vext As Integer
        Dim dataSet As New DataSet
        Dim Sql As String
        Dim dt As DataTable
        Try

            If rdbInterno.Checked Then
                vext = 0 'SI RADIO BUTTON EXTERNO NO ESTA SELECCTIONADO, variable vext = 0'
            Else
                vext = 1
            End If

            Sql = "SELECT ltrim(rtrim(cod_cliente)) as codcliente," &
                                      "ltrim(rtrim(nombre_comercial)) AS cliente, externo" &
                                      " FROM Clientes WHERE cod_pais= " & Session("cod_pais") & " " &
                                      " AND cod_pais= " & Session("cod_pais") & " " &
                                      " AND cod_empresa= " & Session("cod_empresa") & " " &
                                      " AND externo = " & vext & " " &
                                       " order by ltrim(rtrim(nombre_comercial)) "

            dt = DataBase.GetDataTable(Sql)

            If dt.Rows.Count > 0 Then
                Dim DR As DataRow
                DR = dt.Rows(0)
                Session("TableCliente") = dt
            End If


            dataSet.Tables.Add(dt)
            If dataSet.Tables(0).Rows.Count > 0 Then

                ddlCliente.DataSource = dataSet.Tables(0)
                ddlCliente.DataTextField = "cliente"
                ddlCliente.DataValueField = "codcliente"
                ddlCliente.DataBind()

            End If

            ddlCliente.Items.Insert(0, New ListItem("-SELECCIONE-", "0"))

            dataSet.Dispose()

        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.PmsgBox("Ocurrió un error al intentar cargar el listado de rubros en la tabla. Procedimiento CboCliente()" & ex.Message, "error")

        End Try
    End Sub
    Protected Sub ddlCliente_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCliente.SelectedIndexChanged


        Cliente()
        'Cedula()
        If ddlCliente.SelectedItem.Text <> String.Empty Then
            TextNombreCliente.Enabled = False
            TextNombreCliente.BackColor = Drawing.Color.WhiteSmoke

        End If
    End Sub

    Private Sub Cliente()
        Try

            Dim codcl As String = ddlCliente.SelectedValue
            Dim vext As Integer
            Dim sql As String
            Dim fr As System.Data.OleDb.OleDbDataReader
            Session("cod_cliente") = codcl

            '' CLIENTE EVENTUAL DISABLED EN CREDITO

            'TextNombreCliente.Text = String.Empty


            If rdbInterno.Checked Then
                vext = 0
            ElseIf rdbExterno.Checked Then
                vext = 1
            End If


            If rdbContado.Checked Then

                TextNombreCliente.Enabled = True
                TextNombreCliente.Text = String.Empty
                TextId.Text = String.Empty
                'If rdbContado_CheckedChanged() Then
                '    ddlCliente.SelectedIndex = 0
                'End If


                sql = "SELECT cod_sector_mercado as Merca, cedula_ruc, cod_vendedor FROM clientes  WHERE cod_cliente= '" & codcl & "'" &
                    "AND externo= " & vext & ""
                fr = DataBase.GetDataReader(sql)

                If fr.Read() Then

                    Session("Mercado") = fr("Merca").ToString().Trim

                    If fr("cod_vendedor").ToString() <> String.Empty Then

                        Session("vendedor") = fr("cod_vendedor").ToString().Trim
                        'ddlVendedor.SelectedValue = Session("vendedor") ACTUALIZAR TABLA VENDEDORES
                        ddlVendedor.Enabled = False

                    Else
                        ddlVendedor.Enabled = True
                        Session("vendedor") = ddlVendedor.SelectedValue
                    End If

                    If fr("cedula_ruc").ToString() <> String.Empty Then

                        Session("cedulaRuc") = fr("cedula_ruc").ToString().Trim
                        TextId.Text = Session("cedulaRuc")
                        TextId.Enabled = False
                        TextId.BackColor = Drawing.Color.WhiteSmoke

                    Else
                        TextId.Text = String.Empty
                        TextId.Enabled = True
                        TextId.BackColor = Drawing.Color.White
                        Session("cedulaRuc") = TextId.Text

                    End If

                Else

                    Session("Mercado") = Session("lista")
                    ddlVendedor.Enabled = True

                    If fr("cedula_ruc") = String.Empty Then
                        TextId.Text = String.Empty
                        TextId.Enabled = True
                        TextId.BackColor = Drawing.Color.White
                        Session("cedulaRuc") = TextId.Text

                    End If

                    fr.Close()
                End If


            ElseIf rdbCredito.Checked Then

                TextNombreCliente.Enabled = False
                TextNombreCliente.Text = String.Empty
                'ddlCliente.SelectedIndex = 0
                TextId.Text = String.Empty


                sql = "SELECT cod_sector_mercado as Merca, cedula_ruc, cod_vendedor FROM clientes  WHERE cod_cliente= '" & codcl & "'" &
                                                    "AND externo= " & vext & ""
                fr = DataBase.GetDataReader(sql)

                If fr.Read() Then

                    Session("Mercado") = fr("Merca").ToString().Trim

                    If fr("cod_vendedor").ToString() <> String.Empty Then

                        Session("vendedor") = fr("cod_vendedor").ToString().Trim
                        'ddlVendedor.SelectedItem.Value = Session("vendedor") 'ACTUALIZAR TABLA VENDEDORES
                        ddlVendedor.Enabled = False

                    Else

                        ddlVendedor.Enabled = True
                        Session("vendedor") = ddlVendedor.SelectedValue


                    End If

                    If fr("cedula_ruc").ToString <> String.Empty Then

                        Session("cedulaRuc") = fr("cedula_ruc").ToString().Trim
                        TextId.Text = Session("cedulaRuc")
                        TextId.Enabled = False
                        TextId.BackColor = Drawing.Color.WhiteSmoke
                    Else
                        TextId.Text = String.Empty
                        TextId.Enabled = True
                        TextId.BackColor = Drawing.Color.White
                        Session("cedulaRuc") = TextId.Text

                    End If


                Else
                    Session("Mercado") = Session("lista")
                    ddlVendedor.Enabled = True
                    If fr("cedula_ruc") = String.Empty Then
                        TextId.Text = String.Empty
                        TextId.Enabled = True
                        TextId.BackColor = Drawing.Color.White
                        Session("cedulaRuc") = TextId.Text
                    End If

                    fr.Close()
                End If



            End If

        Catch ex As Exception
            Me.ltMensajeGrid.Text = conn.PmsgBox("Ocurrió un error en ddlCliente SelectedIndexChanged. " & ex.Message, "error")
        End Try
    End Sub

    Private Sub CboProductos()
        Try

            Dim dataSet As New DataSet
            Dim sql As String

            sql = " EXEC CombosProductos " &
                  "@opcion = 17," &
                  "@codigo = " & Session("cod_pais") & " "

            dataSet = DataBase.GetDataSet(sql)


            If dataSet.Tables(0).Rows.Count > 0 Then

                ddlProducto.DataSource = dataSet.Tables(0)
                ddlProducto.DataTextField = "Producto"
                ddlProducto.DataValueField = "codProduto"
                ddlProducto.DataBind()

            End If

            ddlProducto.Items.Insert(0, New ListItem("-SELECCIONE-", "0"))

            dataSet.Dispose()


        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.PmsgBox("Ocurrió un error al intentar cargar el listado de productos en la tabla. Proceso CboProducto()" & ex.Message, "error")

        End Try
    End Sub
    Private Sub ProductosPrecio()

        'Procedimiento que llama los productos que tienen precio en la BDD
        Try

            Dim Sql As String
            Sql = "SELECT cod_sector_mercado,cod_producto,precio FROM Precios  WHERE cod_pais= " & Session("cod_pais") & "  " &
                                           " and cod_empresa= " & Session("cod_empresa") & " " &
                                           " and cod_puesto= " & Session("cod_puesto") & " "

            Dim dt As New DataTable
            dt = DataBase.GetDataTable(Sql)

            If dt.Rows.Count > 0 Then
                Dim DR As DataRow
                DR = dt.Rows(0)

                Session("dataTable") = dt
            End If

        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.PmsgBox("Ocurrió un error al intentar cargar el listado de rubros en la tabla. Procedimiento PRODUCTOSPRECIO()" & ex.Message, "error")

        End Try
    End Sub

    'Private Sub CboProductosMercado()
    '    If IsPostBack Then
    '        Dim CodCl = ddlCliente.SelectedItem.Value

    '        Try
    '            Dim Sql As String
    '            Sql = "SELECT cod_sector_mercado,cod_producto,precio FROM Precios  WHERE cod_pais= " & Session("cod_pais") & "  " &
    '                                           " and cod_empresa= " & Session("cod_empresa") & " " &
    '                                           " and cod_puesto= " & Session("cod_puesto") & " "

    '            Dim Sql2 As String
    '            Sql2 = "SELECT cod_sector_mercado, cod_cliente, nombre_comercial AS cliente FROM clientes  WHERE cod_cliente= '" & CodCl & "'" & "AND cod_sector_mercado = " & Session("Mercado") & ""

    '        Catch ex As Exception

    '        End Try




    '    End If

    'End Sub

    Private Sub CboFormaPago()
        Try

            Dim dataSet As New DataSet
            dataSet = DataBase.GetDataSet(" EXEC CombosProductos " &
                  "@opcion = 21," &
                  "@codigo = " & Session("cod_pais") & " ")

            If dataSet.Tables(0).Rows.Count > 0 Then
                ddlFormaPago.DataSource = dataSet.Tables(0)
                ddlFormaPago.DataTextField = "FormaPago"
                ddlFormaPago.DataValueField = "codFormaPago"
                ddlFormaPago.DataBind()
            End If

            ddlFormaPago.Items.Insert(0, New ListItem("-SELECCIONE-", "0"))

            dataSet.Dispose()

        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.PmsgBox("Ocurrió un error al intentar cargar el listado de rubros en FORMA DE PAGO" & ex.Message, "error")

        End Try

    End Sub

    Private Sub Paridad()
        Dim Sql As String
        Dim MessageText As String
        Dim fr As OleDbDataReader

        Sql = "EXEC Cat_FormaPago @opcion=5," &
                  "@codigoPais =  " & Session("CodigoPais") & " ," &
                  "@codigoEmpresa =  " & Session("CodigoEmpresa") & " ," &
                  "@codigoPuesto =  " & Session("CodigoPuesto") & " ," &
                  "@codigoFormaPago =  '0' ," &
                  "@descripcion =  '0' ," &
                  "@BUSQUEDAD = '0'  ," &
                  "@PorDefecto = 0 "

        fr = DataBase.GetDataReader(Sql)

        If fr.Read() Then
            If fr("codformapago").ToString().Trim = "99" Then
                MessageText = "Thenalertify.alert('Codigo de forma de pago no esta predeterminado. Grabarlo en catalogo de forma de pago', function () { window.location.href = '../Default.aspx'; });"
                ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Message", MessageText, True)

            End If

            If fr("Paridad").ToString().Trim = 0 Then
                MessageText = "alertify.alert('Paridad del dia no ha sido ingresado, favor grabarla en catalogo de paridad', function () { window.location.href = '../Default.aspx'; });"
                ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Message", MessageText, True)

            End If

            If fr("moneda").ToString().Trim = 99 Then
                MessageText = "alertify.alert('El código de Moneda no esta predeterminado, favor grabarla en catálogo de moneda', function () { window.location.href = '../Default.aspx'; });"
                ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Message", MessageText, True)

            End If

            Session("Paridad") = fr("Paridad").ToString().Trim
            TextTipoCambio.Text = Session("Paridad")
            TextTipoCambio.BackColor = Drawing.Color.AntiqueWhite
            Session("Cod_FormaPago") = fr("codformapago").ToString().Trim
            Session("Cod_Moneda") = fr("moneda").ToString().Trim
        End If
        fr.Close()

    End Sub

#End Region

#Region "RADIO"
    Protected Sub rdbCredito_CheckedChanged(sender As Object, e As EventArgs) Handles rdbCredito.CheckedChanged
        If rdbCredito.Checked Then
            TextNombreCliente.Enabled = False
            TextNombreCliente.BackColor = Drawing.Color.WhiteSmoke
            TextNombreCliente.Text = ""
            ClienteEnable()

            ddlCliente.Focus()


        End If

        ddlCliente.SelectedIndex = 0
        TextId.Text = String.Empty

    End Sub

    Protected Sub rdbContado_CheckedChanged(sender As Object, e As EventArgs) Handles rdbContado.CheckedChanged

        If rdbContado.Checked Then
            TextNombreCliente.Enabled = True
            TextNombreCliente.BackColor = Drawing.Color.White
            TextNombreCliente.Focus()
            ClienteEnable()

            ddlCliente.Focus()
        End If

        ddlCliente.SelectedIndex = 0
        TextId.Text = String.Empty
    End Sub


    Private Sub ClienteEnable()
        If (rdbCredito.Checked OrElse rdbContado.Checked) AndAlso (rdbExterno.Checked OrElse rdbInterno.Checked) Then
            ddlCliente.Enabled = True

        End If
    End Sub

    Protected Sub rdbInterno_CheckedChanged(sender As Object, e As EventArgs) Handles rdbInterno.CheckedChanged

        If rdbInterno.Checked AndAlso (rdbCredito.Checked OrElse rdbContado.Checked) Then
            ddlCliente.Enabled = True
        End If

        CboCliente()
        ddlCliente.SelectedIndex = 0
        TextId.Text = String.Empty
    End Sub

    Protected Sub rdbExterno_CheckedChanged(sender As Object, e As EventArgs) Handles rdbExterno.CheckedChanged
        If rdbExterno.Checked AndAlso (rdbCredito.Checked OrElse rdbContado.Checked) Then
            ddlCliente.Enabled = True
        End If

        CboCliente()
        ddlCliente.SelectedIndex = 0
        TextId.Text = String.Empty
    End Sub

#End Region

#Region "Proceso verificar inventario"

    Private Sub Inventario()
        Try
            If Session("INV") = "True" Then
                Dim SQL As String = "EXEC VerifiInventario @opcion=1," &
                                "@codigoPais = " & Session("cod_pais") & "," &
                                "@codigoEmpresa = " & Session("cod_empresa") & "," &
                                "@codigoPuesto = " & Session("cod_puesto")

                If SQL.Contains(Session("cod_pais")) AndAlso SQL.Contains(Session("cod_empresa")) AndAlso SQL.Contains(Session("cod_puesto")) Then
                    Dim dt As DataTable = DataBase.GetDataSet(SQL).Tables(0)
                    Dim v_producto As String = Me.ddlProducto.SelectedValue
                    Dim busca_Producto As DataRow()
                    Dim vcantida As String

                    If dt.Rows.Count >= 1 Then
                        busca_Producto = dt.Select("cod_producto='" & v_producto & "'")
                        If busca_Producto.Length > 0 Then
                            vcantida = CStr(busca_Producto(0).Item(1))
                            Session("Imtos") = busca_Producto(0).Item(2)
                            Session("desimprimir") = busca_Producto(0).Item(3)
                            Session("Unidad") = busca_Producto(0).Item(4)

                            If CDec(vcantida.Trim) < CDec(TextCantidad.Text.Trim) Then
                                TextCantidad.Text = vcantida
                                Dim errorMessage As String = "Cantidad a vender es mayor que la existencia=" & vcantida
                                Me.ltMensajeGrid.Text = conn.PmsgBox(errorMessage, "Sin Inventario")
                                ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Message", GetAlertifyScript(errorMessage), True)
                            End If

                            Precio()
                        Else
                            Nuevo()
                            Dim errorMessage As String = "Producto no tiene existencia, por lo tanto no puede facturarse"
                            Me.ltMensajeGrid.Text = conn.PmsgBox(errorMessage, "error")
                            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Message", GetAlertifyScript(errorMessage), True)
                            ddlProducto.Focus()
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Dim errorMessage As String = "Ocurrió un error al intentar cargar el listado de rubros en la tabla." & ex.Message
            Me.ltMensajeGrid.Text &= conn.PmsgBox(errorMessage, "error")
        End Try
    End Sub

    Private Function GetAlertifyScript(message As String) As String
        Return "alertify.alert('" & message & "');"
    End Function

    'Private Sub Inventario()

    '    'Session("VerifInven")
    '    Try
    '        Dim MessageText As String = String.Empty
    '        If Session("INV") = "True" Then

    '            Dim SQL As String = String.Empty
    '            SQL = "EXEC VerifiInventario @opcion=1," &
    '                  "@codigoPais =  " & Session("cod_pais") & "  ," &
    '                  "@codigoEmpresa =  " & Session("cod_empresa") & "  ," &
    '                  "@codigoPuesto =  " & Session("cod_puesto") & " "

    '            If SQL.Contains(Session("cod_pais")) AndAlso SQL.Contains(Session("cod_empresa")) AndAlso SQL.Contains(Session("cod_puesto")) Then

    '                Dim dt As DataTable
    '                dt = DataBase.GetDataSet(SQL).Tables(0)

    '                'Dim dt As New DataTable
    '                'dt = Session("TableInven")
    '                Dim v_producto As String = Me.ddlProducto.SelectedValue
    '                Dim busca_Producto As DataRow()
    '                Dim vcantida As String

    '                If dt.Rows.Count >= 1 Then
    '                    busca_Producto = dt.Select("cod_producto='" & v_producto & "'")
    '                    If busca_Producto.Length > 0 Then
    '                        vcantida = (CStr(busca_Producto(0).Item(1)))
    '                        Session("Imtos") = busca_Producto(0).Item(2)
    '                        Session("desimprimir") = busca_Producto(0).Item(3)
    '                        Session("Unidad") = busca_Producto(0).Item(4)
    '                        If CDec(vcantida.Trim) < CDec(TextCantidad.Text.Trim) Then
    '                            TextCantidad.Text = vcantida
    '                            Me.ltMensajeGrid.Text = conn.pmsgBox("CANTIDAD A VENDER MAYOR QUE EXISTENCIA. EXISTENCIA=" & vcantida, "Sin Inventario")
    '                            MessageText = "alertify.alert('Cantidad a vender es mayor que la existencia=" & vcantida & "');"
    '                            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Message", MessageText, True)

    '                        End If

    '                        Precio()

    '                    Else
    '                        Nuevo()
    '                        'Me.ltMensajeGrid.Text = conn.pmsgBox("Producto no tiene existencia, por lo tanto no puede facturarse", "error")
    '                        MessageText = "alertify.alert('Producto no tiene existencia, por lo tanto no puede facturarse');"
    '                        ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Message", MessageText, True)

    '                        ddlProducto.Focus()

    '                    End If

    '                End If
    '            End If
    '        End If
    '        'Me.ltMensajeGrid.Text = String.Empty
    '        'ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Message", MessageText, True)
    '    Catch ex As Exception
    '        Me.ltMensajeGrid.Text &= conn.pmsgBox("Ocurrió un error al intentar cargar el listado de rubros en la tabla." & ex.Message, "error")


    '    End Try

    'End Sub


    Private Sub Precio()

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
            'TextBultos.Text = 1
            neto = CDec(TextCantidad.Text.Trim) * CDec(vprecio)
            Me.TextPrecio.Text = CDec(vprecio)
            Me.TextTotal.Text = CDec(neto)
        Else

            vprecio = 0

            MessageText = "alertify.alert('Producto a Facturar, no tiene precio');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessageText, True)
            ddlProducto.Focus()
            Return

        End If


    End Sub

    Private Sub Nuevo()
        'ddlProducto.SelectedIndex = 0
        TextCantidad.Text = 0
        TextBultos.Text = 0
        TextPrecio.Text = 0.00
        TextTotal.Text = 0.00
        Session("Imtos") = 0
        Session("desimprimir") = ""
        Session("Unidad") = ""
        'ddlProducto.Focus()


    End Sub

    Private Sub Cedula()
        Try


            Dim Cedula As String
            Dim sql As String
            Dim codCliente As String = ddlCliente.SelectedValue
            Dim db As System.Data.OleDb.OleDbDataReader

            sql = "SELECT cod_cliente, nombre_comercial As nombre, cedula_ruc FROM Clientes WHERE cod_cliente = " & codCliente & " " &
               "AND cod_empresa = " & Session("cod_empresa") & " " &
               "And cod_pais = " & Session("cod_pais") & " "

            db = DataBase.GetDataReader(sql)

            If db.Read Then
                Session("cedula_ruc") = db("cedula_ruc").ToString.Trim()
                Cedula = Session("cedula_ruc")
                TextId.Text = Cedula
            Else
                TextId.Text = String.Empty
                ltMensaje.Text = conn.PmsgBox("Ingrese la cedula del cliente si la requiere.", "alerta")

            End If


        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.PmsgBox("Ocurrió un error al intentar cargar la Cedula RUC del cliente. Procedimiento CEDULA()" & ex.Message, "error")
        End Try

    End Sub

#End Region

#Region "Click de BtnAdicionar y GUARDADO DE PRODUCTO en FACTURA"

    Protected Sub BtnAdicionar_Click(sender As Object, e As System.EventArgs) Handles BtnAdicionar.Click
        Dim MessageText As String
        Dim allValidationsPassed As Boolean = True

        ' Check if Paridad has been entered
        If Session("Paridad") = 0 Then
            MessageText = "alertify.alert('Paridad del día no ha sido ingresada, por favor grabarla en catálogo de paridad.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Message", MessageText, True)
            allValidationsPassed = False
            Return
        End If

        ' Check if No Factura is empty
        If Me.txtNoFactura.Text = String.Empty Then
            MessageText = "alertify.alert('No existe un número de factura.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Message", MessageText, True)
            allValidationsPassed = False
            Return
        End If

        ' Check if a type of invoice is selected
        If Not rdbCredito.Checked AndAlso Not rdbContado.Checked Then
            MessageText = "alertify.alert('Seleccione el tipo de factura.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Message", MessageText, True)
            allValidationsPassed = False
            Return
        End If

        ' Check if a client is selected
        If (rdbCredito.Checked Or rdbContado.Checked) AndAlso ddlCliente.SelectedItem.Text = String.Empty Then
            MessageText = "alertify.alert('Debe seleccionar el cliente.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Message", MessageText, True)
            ddlCliente.Focus()
            allValidationsPassed = False
            Return
        End If

        ' Check if client name is empty when Contado is selected
        If rdbContado.Checked AndAlso ddlCliente.SelectedItem.Text = "" Then
            MessageText = "alertify.alert('Debe introducir el nombre del cliente como Cliente Eventual.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Message", MessageText, True)
            allValidationsPassed = False
            Return
        End If

        ' Check if both product and client fields are not empty
        If ddlProducto.SelectedItem.Text <> String.Empty AndAlso TextNombreCliente.Text <> String.Empty Then
            MessageText = "alertify.alert('Sólo es permitido ingresar un campo de cliente. Por favor eliminar uno.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Message", MessageText, True)
            allValidationsPassed = False
            Return
        End If

        ' Check if a product is selected
        If ddlProducto.SelectedIndex = 0 Then
            MessageText = "alertify.alert('Debe escoger un producto.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Message", MessageText, True)
            allValidationsPassed = False
            Return
        End If

        ' Check if quantity is empty
        If Me.TextCantidad.Text = String.Empty Then
            MessageText = "alertify.alert('Ingrese la cantidad del producto.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Message", MessageText, True)
            allValidationsPassed = False
            Return
        End If

        ' Check if bultos is empty
        If Me.TextBultos.Text = String.Empty Then
            MessageText = "alertify.alert('Ingrese los bultos.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Message", MessageText, True)
            allValidationsPassed = False
            Return
        End If

        ' Check if price is empty
        If Me.TextPrecio.Text = String.Empty Then
            MessageText = "alertify.alert('El producto seleccionado no tiene precio.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Message", MessageText, True)
            allValidationsPassed = False
            Return
        End If

        ' Check if total is empty
        If TextTotal.Text = String.Empty Then
            MessageText = "alertify.alert('No puede calcularse el total. Verifique los datos.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Message", MessageText, True)
            allValidationsPassed = False
            Return
        End If

        ' If all validations passed, display success message and call the function
        If allValidationsPassed Then
            ltMensaje.Text = conn.PmsgBox("No se encontraron errores en las validaciones.", "exito")
            GuardarTmpFact2()
        End If
    End Sub


    Private Sub GuardarTmpFact()
        Dim dbCon As New System.Data.OleDb.OleDbConnection(conn.conn)
        Try
            If dbCon.State = ConnectionState.Closed Then
                dbCon.Open()
            End If

            If TextPorDesc.Text = "" Then
                TextPorDesc.Text = 0
            End If

            Dim decPorcDesc As Decimal
            Dim decValorDesc As Decimal
            Dim decIva As Decimal
            Dim decTotalProd As Decimal

            decTotalProd = CDec(TextTotal.Text)
            decPorcDesc = CDec(TextPorDesc.Text)
            decValorDesc = decTotalProd * (decPorcDesc / 100)

            If Session("Imtos") Then   ''variable se llena desde el inventario
                decIva = (decTotalProd - decValorDesc) * (Session("porcImp") / 100)
            Else
                decIva = 0
            End If


            Dim vUss As String = String.Empty
            vUss = Context.Request.Cookies("CKSMFACTURA")("cod_usuario")

            Dim Contado As Integer
            Dim Cliente As String = Me.ddlCliente.Text.ToString()
            Dim CodCliente As String

            If rdbContado.Checked Then
                Contado = 1
                Session("condicion") = "La factura es de contado"

                If ddlCliente.SelectedItem.Text = String.Empty OrElse ddlCliente.Text.ToString = "0" Then
                    Cliente = TextNombreCliente.Text.Trim
                    CodCliente = "0"
                Else
                    Cliente = ddlCliente.SelectedItem.Text
                    CodCliente = Me.ddlCliente.SelectedValue
                End If

            Else

                Contado = 0
                Session("condicion") = "Factura es de Credito"
                Cliente = ddlCliente.SelectedItem.Text
                CodCliente = ddlCliente.SelectedValue()

            End If

            If Me.ddlVendedor.SelectedValue = String.Empty Then
                Me.ddlVendedor.SelectedValue = Session("vendedor")
            End If

            Dim vext As Integer

            If rdbInterno.Checked Then
                vext = 0
            Else
                vext = 1
            End If



            Session("NoFactura") = txtNoFactura.Text
            Dim sql As String
            sql = " SET DATEFORMAT DMY EXEC Factura @opcion= 1," &
                  "@codigoPais = " & MyBase.Session("cod_pais") & "," &
                  "@codigoPuesto = " & MyBase.Session("cod_puesto") & "," &
                  "@codigoEmpresa = " & MyBase.Session("cod_empresa") & "," &
                  "@no_factura = " & txtNoFactura.Text.Trim & "," &
                  "@fecha = '01/01/1900'," &
                  "@cod_producto = '" & Me.ddlProducto.SelectedValue & "'," &
                  "@consecutivoUsuario = '" & vUss & "'," &
                  "@porc_descuento = " & decPorcDesc & "," &
                  "@valor_descuento = " & decValorDesc & "," &
                  "@porc_iva = " & MyBase.Session("porcImp") & "," &
                  "@valor_iva = " & decIva & "," &
                  "@anulada =  0," &
                  "@fechaHora_anulacion = '01/01/1900'," &
                  "@sub_total = " & decTotalProd & "," &
                  "@paridad = " & MyBase.Session("Paridad") & "," &
                  "@codcliente = '" & CodCliente & "'," &
                  "@cantidad = " & CDec(Me.TextCantidad.Text) & "," &
                  "@bultos = " & CDec(Me.TextBultos.Text) & "," &
                  "@cod_und_medida =1," &
                  "@precio_unidad = " & CDec(Me.TextPrecio.Text) & "," &
                  "@Notas= '0'," &
                  "@Contado = " & Contado & "," &
                  "@codvendedor  = '" & Me.ddlVendedor.SelectedValue & "'," &
                  "@cedularuc= '" & TextId.Text.Trim & "'," &
                  "@externo= " & vext & "," &
                  "@desc_imprimir = '" & MyBase.Session("desimprimir") & "'," &
                  "@NombreCliente = '" & Cliente & "'"

            '" & Session("Unidad") & "
            Dim cmd As New OleDbCommand(sql, dbCon)
            Dim netdol As Decimal
            Dim dr As OleDbDataReader
            dr = cmd.ExecuteReader
            If dr.Read() Then
                TextTotalLibras.Text = dr("Cantidad").ToString().Trim
                TextSubtotal.Text = dr("Subtotal").ToString().Trim
                TextValorDesc.Text = dr("Descuento")
                TextIVA.Text = dr("Iva").ToString().Trim
                TextNeto.Text = dr("Neto").ToString().Trim
                netdol = dr("NetoDol").ToString.Trim
                TextNetoDolar.Text = Math.Round(dr("NetoDol"), 2)
                TextSaldoFact.Text = dr("Neto").ToString().Trim
                Session("DatosFact") = "Neto Factura en: C$ " & TextNeto.Text & " $ " & netdol.ToString("N2") & " Paridad " & Session("Paridad")
                Session("SaldoFact") = dr("Neto").ToString().Trim
            End If

            dr.Close()

            Load_GridView()

            Nuevo()
            'Response.Redirect(ResolveClientUrl("../Movimientos/Reportes/Factura/ImprimirFactura.aspx"))



            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "EXECUTE", "EnableButtom()", True)
            'Me.ltMensaje.Text = conn.pmsgBox("El registro se ha guardado de forma correcta.", "exito")

            ddlProducto.Focus()

            'MessageText = "alertify.alert('El registro se ha guardado de forma correcta');"
            'ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Message", MessageText, True)
            ltMensaje.Text = conn.PmsgBox("El registro se ha guardado de forma correcta.", "exito")

        Catch ex As Exception
            Me.ltMensaje.Text = conn.PmsgBox("Ocurrió un error al intentar guardar el registro." & ex.Message, "error")

        Finally
            If dbCon.State = ConnectionState.Open Then
                dbCon.Close()
            End If

        End Try
    End Sub

    Private Sub GuardarTmpFact2()

        Dim dbCon As New System.Data.OleDb.OleDbConnection(conn.conn)

        Try
            If dbCon.State = ConnectionState.Closed Then
                dbCon.Open()
            End If

            ' Calculate percentage discount and VAT
            Dim precio As Decimal = If(String.IsNullOrEmpty(TextPrecio.Text), 0, CDec(TextPrecio.Text))
            Dim porcentajeDescuento As Decimal = If(String.IsNullOrEmpty(TextPorDesc.Text), 0, CDec(TextPorDesc.Text))
            Dim valorDescuento As Decimal = CDec(TextTotal.Text) * (porcentajeDescuento / 100)
            Dim iva As Decimal = If(Session("Imtos"), (CDec(TextTotal.Text) - valorDescuento) * (Session("porcImp") / 100), 0)
            'Dim subTotal As Decimal =

            ' User and payment information
            Dim usuario As String = Context.Request.Cookies("CKSMFACTURA")("cod_usuario")
            Dim contado As Integer = If(rdbContado.Checked, 1, 0)
            Dim cliente As String = If(rdbContado.Checked AndAlso String.IsNullOrEmpty(ddlCliente.SelectedItem.Text) OrElse ddlCliente.SelectedItem.Text = "0", TextNombreCliente.Text.Trim(), ddlCliente.SelectedItem.Text)
            Dim codCliente As String = If(rdbContado.Checked AndAlso (String.IsNullOrEmpty(ddlCliente.SelectedItem.Text) OrElse ddlCliente.SelectedItem.Text = "0"), "0", ddlCliente.SelectedValue())

            ' Check external/internal
            Dim vext As Integer = If(rdbInterno.Checked, 0, 1)
            Session("NoFactura") = txtNoFactura.Text

            ' Construct and execute SQL query with parameters
            Dim sql As String = "SET DATEFORMAT DMY " &
                                "EXEC Factura " &
                                "@opcion = 1, " &
                                "@codigoPais = " & MyBase.Session("cod_pais") & ", " &
                                "@codigoPuesto = " & MyBase.Session("cod_puesto") & ", " &
                                "@codigoEmpresa = " & MyBase.Session("cod_empresa") & ", " &
                                "@no_factura = " & txtNoFactura.Text.Trim & ", " &
                                "@fecha = '01/01/1900', " &
                                "@cod_producto = '" & Me.ddlProducto.SelectedValue & "', " &
                                "@consecutivoUsuario = '" & usuario & "', " &
                                "@porc_descuento = " & porcentajeDescuento & ", " &
                                "@valor_descuento = " & valorDescuento & ", " &
                                "@porc_iva = " & MyBase.Session("porcImp") & ", " &
                                "@valor_iva = " & iva & ", " &
                                "@anulada =  0, " &
                                "@fechaHora_anulacion = '01/01/1900', " &
                                "@sub_total = " & CDec(Me.TextTotal.Text) & ", " &
                                "@paridad = " & MyBase.Session("Paridad") & ", " &
                                "@codcliente = '" & codCliente & "', " &
                                "@cantidad = " & CDec(Me.TextCantidad.Text) & ", " &
                                "@bultos = " & CDec(Me.TextBultos.Text) & ", " &
                                "@cod_und_medida = 1, " &
                                "@precio_unidad = " & CDec(Me.TextPrecio.Text) & ", " &
                                "@Notas = '0', " &
                                "@Contado = " & contado & ", " &
                                "@codvendedor = '" & Me.ddlVendedor.SelectedValue & "', " &
                                "@cedularuc = '" & TextId.Text.Trim & "', " &
                                "@externo = " & vext & ", " &
                                "@desc_imprimir = '" & MyBase.Session("desimprimir") & "', " &
                                "@NombreCliente = '" & cliente & "'"

            Dim cmd As New OleDbCommand(sql, dbCon)
            Dim netdol As Decimal
            Dim dr As OleDbDataReader
            dr = cmd.ExecuteReader
            If dr.Read() Then
                TextTotalLibras.Text = dr("Cantidad").ToString().Trim
                TextSubtotal.Text = dr("Subtotal").ToString().Trim
                TextValorDesc.Text = dr("Descuento")
                TextIVA.Text = dr("Iva").ToString().Trim
                TextNeto.Text = dr("Neto").ToString().Trim
                netdol = dr("NetoDol").ToString.Trim
                TextNetoDolar.Text = Math.Round(dr("NetoDol"), 2)
                TextSaldoFact.Text = dr("Neto").ToString().Trim
                Session("DatosFact") = "Neto Factura en: C$ " & TextNeto.Text & " $ " & netdol.ToString("N2") & " Paridad " & Session("Paridad")
                Session("SaldoFact") = dr("Neto").ToString().Trim
            End If

            dr.Close()

            Load_GridView()

            Nuevo()
            'Response.Redirect(ResolveClientUrl("../Movimientos/Reportes/Factura/ImprimirFactura.aspx"))

            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "EXECUTE", "EnableButtom()", True)
            'Me.ltMensaje.Text = conn.pmsgBox("El registro se ha guardado de forma correcta.", "exito")

            ddlProducto.Focus()

            'MessageText = "alertify.alert('El registro se ha guardado de forma correcta');"
            'ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Message", MessageText, True)
            ltMensaje.Text = conn.PmsgBox("El registro se ha guardado de forma correcta.", "exito")

        Catch ex As Exception
            Me.ltMensaje.Text = conn.PmsgBox("Ocurrió un error al intentar guardar el registro." & ex.Message, "error")

        Finally
            If dbCon.State = ConnectionState.Open Then
                dbCon.Close()
            End If
        End Try







    End Sub


#End Region

    Private Sub Eliminar(ProductoCodProd As String)

        Dim MessageText As String = String.Empty
        Dim dbCon As New System.Data.OleDb.OleDbConnection(conn.conn)
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
            If rdbContado.Checked Then
                Contado = 1
            Else
                Contado = 0
            End If

            Dim vext As Integer
            If rdbInterno.Checked Then
                vext = 0
            Else
                vext = 1
            End If

            Dim sql As String = "SET DATEFORMAT DMY " & vbCrLf
            sql &= "EXEC Factura @opcion=4," &
                  "@codigoPais = " & MyBase.Session("cod_pais") & "," &
                  "@codigoPuesto = " & MyBase.Session("cod_puesto") & "," &
                  "@codigoEmpresa = " & MyBase.Session("cod_empresa") & "," &
                  "@no_factura = " & txtNoFactura.Text.Trim & "," &
                  "@fecha = '01/01/1900'," &
                  "@cod_producto = '" & ProductoCodProd & "'," &
                  "@consecutivoUsuario = " & vUss & "," &
                  "@porc_descuento = " & CDec(TextPorDesc.Text.Trim) & "," &
                  "@valor_descuento = " & desc & "," &
                  "@porc_iva = " & MyBase.Session("porcImp") & "," &
                  "@valor_iva = " & v_iva & "," &
                  "@anulada =  0," &
                  "@fechaHora_anulacion = '01/01/1900'," &
                  "@sub_total = " & CDec(TotProdu) & "," &
                  "@paridad = " & MyBase.Session("Paridad") & "," &
                  "@codcliente = '" & Me.ddlCliente.SelectedValue & "'," &
                  "@cantidad = 0," &
                  "@bultos = 0," &
                  "@cod_und_medida = ''," &
                  "@precio_unidad = 0," &
                  "@Notas=  NULL ," &
                  "@Contado = " & Contado & "," &
                  "@codvendedor  = '" & Me.ddlVendedor.SelectedValue & "'," &
                  "@cedularuc= '" & TextId.Text.Trim & "'," &
                  "@externo= " & vext & "," &
                  "@desc_imprimir = '" & MyBase.Session("desimprimir") & "'," &
                  "@NombreCliente = '" & TextNombreCliente.Text & "'"

            Dim cmd As New OleDb.OleDbCommand(sql, dbCon)
            Dim netdol As Decimal
            Dim dr As System.Data.OleDb.OleDbDataReader
            dr = cmd.ExecuteReader
            If dr.Read() Then
                TextTotalLibras.Text = dr("Cantidad").ToString().Trim
                TextSubtotal.Text = dr("Subtotal").ToString().Trim
                TextIVA.Text = dr("Iva").ToString().Trim
                TextNeto.Text = dr("Neto").ToString().Trim
                netdol = dr("NetoDol").ToString.Trim
                TextNetoDolar.Text = dr("NetoDol").ToString.Trim
                Session("DatosFact") = "Neto Factura en: C$ " & TextNeto.Text & " $ " & netdol.ToString("N2") & " Paridad " & Session("Paridad")
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

#Region "Procesos y Eventos del GridView"


    '' REALIZAR UN UPDATE EN ESTE PROCEDIMIENTO PARA ACTUALIZAR LOS CAMPOS, CORRER EL SQL EN SSMS PARA VERIFICAR QUE FUNCIONE
    Private Sub Load_GridView()
        Try
            Dim vCPais As String = String.Empty
            vCPais = Context.Request.Cookies("CKSMFACTURA")("codPais")

            Dim SQL As String = String.Empty
            SQL &= "EXEC Factura @opcion=3," &
                  "@codigoPais =  " & vCPais & " ," &
                  "@codigoPuesto =  " & Request.Cookies("CKSMFACTURA")("CodPuesto") & "  ," &
                  "@codigoEmpresa = " & Request.Cookies("CKSMFACTURA")("CodEmpresa") & "  ," &
                  "@no_factura = " & txtNoFactura.Text.Trim & "," &
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



            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "smf_Script", "responsive_grid()", True)

        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.PmsgBox("Ocurrió un error al intentar cargar el listado de rubros en la tabla." & ex.Message, "error")
        End Try
    End Sub

    Protected Sub GridViewOne_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridViewOne.DataBound
        Try
            If GridViewOne.Rows.Count > 0 Then
                Dim pagerRow As GridViewRow = GridViewOne.BottomPagerRow
                Dim pageLabel As Label = CType(pagerRow.Cells(0).FindControl("CurrentPageLabel"), Label)
                If pageLabel IsNot Nothing Then
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
            If (IsPostBack) AndAlso (dtTabla IsNot Nothing) Then

                If dtTabla IsNot Nothing AndAlso dtTabla.Rows.Count > 0 Then


                    Me.GridViewOne.DataSource = dtTabla
                    Me.GridViewOne.DataBind()

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
                'e.Row.Attributes.Add("OnMouseOver", "On(this);")
                'e.Row.Attributes.Add("OnMouseOut", "Off(this);")
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

    Protected Sub GridViewOne_SelectedIndexChanged(sender As Object, e As EventArgs) Handles GridViewOne.SelectedIndexChanged

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
    Protected Sub ddlProducto_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlProducto.SelectedIndexChanged

        If ddlProducto.SelectedValue <> String.Empty Then

            Nuevo()
            Precio()
            Inventario()
            TextCantidad.Text = ""
            TextCantidad.Focus()
        End If
    End Sub


    Protected Sub TextNombreCliente_TextChanged(sender As Object, e As EventArgs) Handles TextNombreCliente.TextChanged
        ddlCliente.SelectedValue = "0"
        TextId.Enabled = True

        'If TextNombreCliente.Text.Length > 0 Then
        '    ddlCliente.Items.Insert(1, New ListItem("Cliente Eventual", 1))
        '    ddlCliente.SelectedIndex = 1


        'End If
    End Sub

    Protected Sub TextCantidad_TextChanged(sender As Object, e As EventArgs) Handles TextCantidad.TextChanged
        'Inventario()
        'Precio()
        'TextBultos.Text = ""
        'TextBultos.Focus()
    End Sub

    Protected Sub TextPrecio_TextChanged(sender As Object, e As EventArgs) Handles TextPrecio.TextChanged
        TextTotal.Text = CDec(TextCantidad.Text) * CDec(TextPrecio.Text)
    End Sub

#Region "SECCION FormaPago"

    Protected Sub ddlformapago_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlFormaPago.SelectedIndexChanged

        If ddlFormaPago.Text = "TARJETA" OrElse ddlFormaPago.SelectedValue() = "T" Then

            ddlTarjeta.Visible = True
            ddlBanco.Visible = True
            lblTarjeta.Visible = True
            lblBanco.Visible = True

        ElseIf ddlFormaPago.Text = "EFECTIVO" OrElse ddlFormaPago.SelectedValue() = "E" Then
            ddlTarjeta.Visible = False
            lblTarjeta.Visible = False
            ddlBanco.Visible = False
            lblBanco.Visible = False
        End If
    End Sub
    Protected Sub ddlMoneda_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlMoneda.SelectedIndexChanged

        TextCordobasRec.Text = 0.00
        'If ddlMoneda.Text.ToString = "CORDOBAS" OrElse ddlMoneda.SelectedValue() = 1 Then

        '    lblCordobasRec.Visible = True
        '    TextCordobasRec.Visible = True
        '    TextCordobasRec.Enabled = True
        '    TextCordobasRec.Text = 0

        'ElseIf ddlMoneda.Text.ToString = "DOLARES" OrElse ddlMoneda.SelectedValue() = 2 Then


        '    lblCordobasRec.Visible = True
        '    TextCordobasRec.Visible = True
        '    TextCordobasRec.Enabled = True
        '    TextCordobasRec.Text = 0




        'End If


        'If ddlMoneda.Text.ToString = "DOLARES" OrElse ddlMoneda.SelectedValue() = 2 Then

        '    lblCordobasRec.Visible = False
        '    TextCordobasRec.Visible = False
        '    TextCordobasRec.Text = 0
        '    'TextCordobasRec.BackColor = Drawing.Color.WhiteSmoke

        '    TextDolar.Visible = True
        '    TextDolar.Enabled = True
        '    TextDolar.Text = 0
        '    lblDolar.Visible = True

        '    TextConverDolACor.Text = 0

        '    If CDec(TextDolar.Text) * Session("Paridad") > CDec(TextSaldoFact.Text) Then
        '        TextCordobasRec.Text = CDec(TextSaldoFact.Text)
        '    Else
        '        TextCordobasRec.Text = CDec(TextDolar.Text) * Session("Paridad")
        '    End If

        '    lblConverDolACor.Visible = True
        '    TextConverDolACor.Visible = True
        '    TextConverDolACor.Text = CDec(TextDolar.Text) * Session("Paridad")

        '    'TextVuelto.Text = CDec(TextSaldoFact.Text) - CDec(TextCordobasRec.Text)

        'ElseIf ddlMoneda.Text.ToString = "CORDOBAS" OrElse ddlMoneda.SelectedValue() = 1 Then

        '    lblCordobasRec.Visible = True
        '    TextCordobasRec.Visible = True
        '    TextCordobasRec.Enabled = True
        '    TextCordobasRec.Text = 0


        '    lblDolar.Visible = False
        '    TextDolar.Visible = False
        '    TextDolar.Enabled = False
        '    TextDolar.Text = 0
        '    'TextDolar.BackColor = Drawing.Color.WhiteSmoke

        '    lblConverDolACor.Visible = False
        '    TextConverDolACor.Visible = False
        '    TextConverDolACor.Text = 0

        '    'TextVuelto.Text = CDec(TextSaldoFact.Text) - CDec(TextCordobasRec.Text)
        'End If
    End Sub

    'Protected Sub TextDolar_TextChanged(sender As Object, e As EventArgs) Handles TextDolar.TextChanged

    '    'TextConverDolACor.Text = CDec(TextDolar.Text) * Session("Paridad")

    '    'If CDec(TextConverDolACor.Text) > CDec(TextSaldoFact.Text) Then
    '    '    'TextCordobasRec.Text = CDec(TextConverDolACor.Text) - CDec(TextSaldoFact.Text)
    '    '    TextCordobasRec.Text = CDec(TextSaldoFact.Text)
    '    'Else
    '    '    'TextCordobasRec.Text = CDec(TextSaldoFact.Text) - CDec(TextConverDolACor.Text)
    '    '    TextCordobasRec.Text = CDec(TextConverDolACor.Text)
    '    'End If

    '    'If ddlMoneda.Text.ToString = "DOLARES" OrElse ddlMoneda.SelectedValue() = 2 Then
    '    '    If CDec(TextSaldoFact.Text) / Session("Paridad") < CDec(TextDolar.Text) Then
    '    '        TextVuelto.Text = (CDec(TextDolar.Text) * Session("Paridad")) - CDec(TextSaldoFact.Text)
    '    '    Else

    '    '        TextVuelto.Text = CDec(TextSaldoFact.Text) - (CDec(TextDolar.Text) * Session("Paridad"))

    '    '    End If
    '    'End If


    'End Sub

    Protected Sub TextCordobasRec_TextChanged(sender As Object, e As EventArgs) Handles TextCordobasRec.TextChanged

        'If ddlMoneda.Text.ToString = "CORDOBAS" OrElse ddlMoneda.SelectedValue() = 1 Then

        '    If CDec(TextSaldoFact.Text) < CDec(TextCordobasRec.Text) Then
        '        TextVuelto.Text = CDec(TextCordobasRec.Text) - CDec(TextSaldoFact.Text)

        '    Else

        '        TextVuelto.Text = CDec(TextSaldoFact.Text) - CDec(TextCordobasRec.Text)
        '    End If
        'End If

    End Sub

    Protected Sub btnDatosComp_Click(sender As Object, e As EventArgs) Handles btnDatosComp.Click

        Dim MessageText As String

        Dim Sql As String


        Dim formaPago As String = Session("Cod_FormaPago")
        ddlFormaPago.SelectedValue = formaPago


        Dim codMoneda As Integer = Session("Cod_Moneda")
        ddlMoneda.SelectedValue = codMoneda

        If TextNeto.Text <> String.Empty Then

            Try

                Sql = "SELECT  sub_total as Valor " &
                    " FROM  factura_enc  WHERE cod_pais= " & Session("cod_pais") & " " &
                                        " AND cod_pais= " & Session("cod_pais") & " " &
                                        " AND cod_empresa= " & Session("cod_empresa") & " " &
                                        " and no_factura= " & txtNoFactura.Text.Trim & " "

                Dim DataSet As New DataSet
                DataSet = DataBase.GetDataSet(Sql)

                If DataSet.Tables(0).Rows.Count <> 0 Then

                    MessageText = "alertify.alert('Numero de Factura ya existe proceso se cancelara');"

                    ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessageText, True)

                End If



                Sql = "SELECT  isnull(sum(sub_total),0) as Valor " &
                      " FROM  TmpDetFact  WHERE cod_pais= " & Session("cod_pais") & " " &
                                          " AND cod_pais= " & Session("cod_pais") & " " &
                                          " AND cod_empresa= " & Session("cod_empresa") & " " &
                                          " and no_factura= " & txtNoFactura.Text.Trim & " "


                Dim fr As OleDbDataReader = DataBase.GetDataReader(Sql)

                If fr.Read() Then

                    If rdbContado.Checked Then

                        'TextPorDesc.Enabled = False

                        'DistrPago()

                        'TextDatos.Text = Session("DatosFact")
                        TextSaldoFact.Text = Session("SaldoFact")
                        TextVuelto.BackColor = Drawing.Color.WhiteSmoke


                        ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Facturacion", "open_popup();", True)

                    ElseIf rdbCredito.Checked Then

                        Session("Continuar") = 1
                        ImprimirGuardar()
                    End If

                Else

                    MessageText = "alertify.alert('Registre Productos a Facturar');"
                    ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessageText, True)

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

        Else
            MessageText = "alertify.alert('Verifique los campos antes de continuar.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Message", MessageText, True)
        End If


    End Sub
    Protected Sub BtnAdiciFormaPago_Click(sender As Object, e As EventArgs) Handles BtnAdiciFormaPago.Click
        Try
            'Dim suma As Object = objetoDataTable.Compute("Sum(monto)", "Cuenta=numeroCuenta") " & CDec(txtNoFac.Text) & "

            Dim MessageText As String

            'If CDec(TextNeto.Text) < (CDec(valor) + CDec(TextCordobasRec.Text)) Then
            '    MessageText = "alertify.alert('El valor del pago es mayor que la factura, registro no se guardara');"
            '    ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessageText, True)
            '    Return
            'End If

            'If CDec(TextNeto.Text) = (CDec(valor)) Then
            '    MessageText = "alertify.alert('La suma de las diferentes formas de pago, es igual al neto de la factura, registro no se guardara');"
            '    ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessageText, True)
            '    Return
            'End If

            'If CDec(TextNeto.Text) < (CDec(valor) + CDec(TextSaldoFact.Text)) Then
            '    MessageText = "alertify.alert('Lo digitado excede al valor de la factura');"
            '    ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessageText, True)
            '    Return
            'End If

            If Me.ddlFormaPago.Text = String.Empty Then
                MessageText = "alertify.alert('Seleccione una Forma de pago');"
                ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessageText, True)
                Return
            End If

            If Me.ddlFormaPago.SelectedValue = "T" Then

                If Me.ddlTarjeta.Text = String.Empty Then
                    MessageText = "alertify.alert('Seleccione una tarjeta');"
                    ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessageText, True)
                    Return

                End If
            End If



            DistrPago()




            'Dim dt As New DataTable


            'dt = Session("DistriPago")

            'Dim arrKeyVals(1) As Object
            'Dim rowCollection As DataRowCollection = dt.Rows
            'arrKeyVals(0) = ddlFormaPago.SelectedValue
            'arrKeyVals(1) = txtNoFactura.Text.Trim
            'Dim foundRow As DataRow = dt.Rows.Find((arrKeyVals))

            ''If Not foundRow(0).ToString = String.Empty Then

            'If foundRow Is Nothing Then
            '    'No se encontró la fila. Crear nueva fila

            '    Dim row As DataRow = dt.NewRow()
            '    row("No_Factura") = txtNoFactura.Text.Trim
            '    row("Cod_Pais") = Session("cod_pais")
            '    row("Cod_Empresa") = Session("cod_empresa")
            '    row("Cod_Puesto") = Session("cod_puesto")

            '    row("cod_FormaPago") = ddlFormaPago.SelectedValue
            '    row("Cod_Moneda") = ddlMoneda.SelectedValue
            '    row("contado_credito") = If(rdbContado.Checked, 1, 0)

            '    row("ValorFacturaCor") = CDec(TextNeto.Text)
            '    row("ValorFacturaDol") = CDec(TextNeto.Text) / Session("Paridad")

            '    row("ValorRecibidoCor") = CDec(TextCordobasRec.Text)
            '    row("ValorRecibidoDol") = CDec(TextDolar.Text)
            '    row("Vuelto") = CDec(TextVuelto.Text)

            '    row("Paridad") = Session("Paridad")
            '    row("Anulada") = 0
            '    row("ComisionTarjeta") = 0
            '    row("cod_Tarjeta") = ddlTarjeta.SelectedValue

            '    row("FormaPago") = ddlFormaPago.Text
            '    row("Moneda") = ddlMoneda.Text

            '    dt.Rows.Add(row)

            '    Me.GridViewpop.DataSource = dt.DefaultView
            '    Me.GridViewpop.DataBind()

            '    'ElseIf foundRow IsNot Nothing Then
            '    '    dt.Rows.Remove(foundRow)
            'End If



            'Dim dif As Decimal

            'valor = dt.Compute("Sum(ValorRecibidoCor)", "No_Factura= " & CDec(txtNoFactura.Text) & " ")

            'If CDec(TextNeto.Text) - CDec(valor) > 0 Then

            '    dif = CDec(TextNeto.Text) - CDec(valor)

            '    If dif > TextSaldoFact.Text Then

            '        TextCordobasRec.Text = TextSaldoFact.Text

            '    End If

            '    dif = 0
            '    dif = CDec(TextSaldoFact.Text) - CDec(TextCordobasRec.Text)
            '    TextSaldoFact.Text = dif
            '    'TextSaldoFact.Text = CDec(TextNeto.Text) - CDec(valor) Then
            'Else
            '    TextSaldoFact.Text = 0
            'End If

            'TextDolar.Text = 0

            'TextCordobasRec.Text = 0
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

    Private Sub DistrPago()

        Try
            Me.LiteralGridpop.Text = ""

            'columnas
            '//////////////////////////////////////////////////////////////////////////////////////////'
            Dim dt As DataTable


            dt = If(Session("DistriPago") IsNot Nothing, DirectCast(Session("DistriPago"), DataTable), New DataTable())

            If Not Page.IsPostBack Then
                dt.Rows.Clear()
            End If

            If dt.Columns.Count = 0 Then

                dt.Columns.Add("Cod_FormaPago")
                dt.Columns.Add("No_Factura")

                'dt.PrimaryKey = {
                '    dt.Columns("cod_FormaPago")}
                'dt.Columns("No_Factura")

                ' Create an array of DataColumn objects representing the columns in the primary key
                Dim primaryKeyColumns As DataColumn() = {
                 dt.Columns("Cod_FormaPago"),
                 dt.Columns("No_Factura"),
                 dt.Columns("Cod_Moneda")
                   }

                ' Set the primary key constraint on the DataTable
                dt.PrimaryKey = primaryKeyColumns


                dt.Columns.Add("Cod_Pais")
                dt.Columns.Add("Cod_Empresa")
                dt.Columns.Add("Cod_Puesto")
                dt.Columns.Add("Cod_Moneda")

                dt.Columns.Add("Contado_credito")

                dt.Columns.Add("ValorFacturaCor")
                dt.Columns.Add("ValorFacturaDol")

                dt.Columns.Add("ValorRecibidoCor", Type.GetType("System.Decimal"))
                dt.Columns.Add("ValorRecibidoDol", Type.GetType("System.Decimal"))

                dt.Columns.Add("SaldoFacturaCor", Type.GetType("System.Decimal"))
                dt.Columns.Add("SaldoFacturaDol", Type.GetType("System.Decimal"))

                dt.Columns.Add("ConversionUS$aC$", Type.GetType("System.Decimal"))

                dt.Columns.Add("Vuelto")

                dt.Columns.Add("Paridad", Type.GetType("System.Decimal"))

                dt.Columns.Add("Anulada")

                dt.Columns.Add("ComisionTarjeta", Type.GetType("System.Decimal"))
                dt.Columns.Add("Cod_tarjeta")

                dt.Columns.Add("FormaPago")
                dt.Columns.Add("Moneda")

            End If

            'ROW
            '///////////////////////////////////////////////////////////////////////////////////////////'

            Dim row As DataRow = dt.NewRow()

            Dim formaPago As String = ddlFormaPago.SelectedValue
            Dim desFormaPago As String = ddlFormaPago.SelectedItem.Text
            Session("Cod_FormaPago") = formaPago

            Dim codMoneda As Integer = ddlMoneda.SelectedValue
            Dim desMoneda As String = ddlMoneda.SelectedItem.Text
            Session("Cod_Moneda") = codMoneda

            row("No_Factura") = txtNoFactura.Text.Trim
            row("Cod_Pais") = Session("cod_pais")
            row("Cod_Empresa") = Session("cod_empresa")
            row("Cod_Puesto") = Session("cod_puesto")

            row("Cod_FormaPago") = formaPago
            row("Cod_Moneda") = codMoneda
            row("contado_credito") = If(rdbContado.Checked, 1, 0)

            Dim ValorFacturaCor = CDec(TextNeto.Text)
            Dim ValorFacturaDol = CDec(TextNetoDolar.Text)

            row("ValorFacturaCor") = ValorFacturaCor
            row("ValorFacturaDol") = ValorFacturaDol



            If ddlMoneda.SelectedValue = 1 Then

                row("ValorRecibidoCor") = CDec(TextCordobasRec.Text)
                row("ValorRecibidoDol") = 0.00

            ElseIf ddlMoneda.SelectedValue = 2 Then

                row("ValorRecibidoCor") = 0.00
                row("ValorRecibidoDol") = CDec(TextCordobasRec.Text)

            End If

            'row("ValorRecibidoDol") = CDec(TextDolar.Text)

            Dim SaldoFacturaCor = CDec(TextSaldoFact.Text)

            If ddlMoneda.SelectedValue = 1 Then

                row("SaldoFacturaCor") = SaldoFacturaCor - row("ValorRecibidoCor")

            ElseIf ddlMoneda.SelectedValue = 2 Then

                row("SaldoFacturaCor") = SaldoFacturaCor - (row("ValorRecibidoDol") * Session("Paridad"))

            End If

            row("SaldoFacturaDol") = Math.Round(row("SaldoFacturaCor") / Session("Paridad"), 4)

            row("ConversionUS$aC$") = (row("ValorRecibidoDol") * Session("Paridad"))

            row("Vuelto") = 0

            row("Paridad") = Session("Paridad")
            row("Anulada") = 0
            row("ComisionTarjeta") = 0
            row("cod_Tarjeta") = ddlTarjeta.SelectedValue

            row("FormaPago") = desFormaPago
            row("Moneda") = desMoneda

            dt.Rows.Add(row)

            '///////////////////////////////////////////////////////////////////////////////////////'

            Session("DistriPago") = dt

            Me.GridViewpop.DataSource = dt
            Me.GridViewpop.DataBind()

            Session("Continuar") = 1

            Dim MessageText As String
            Dim valorRecibidoCor As Object = 0
            Dim valorRecibidoDol As Object = 0
            Dim valorRecibidoTotal As Decimal
            Dim saldoFact As Decimal = CDec(TextSaldoFact.Text)
            Dim highestIndex As Integer = dt.Rows.Count - 1

            valorRecibidoCor = dt.Compute("SUM(ValorRecibidoCor)", "No_Factura= " & txtNoFactura.Text.Trim & " ")
            valorRecibidoDol = dt.Compute("SUM(ValorRecibidoDol)", "No_Factura= " & txtNoFactura.Text.Trim & " ")
            valorRecibidoTotal = valorRecibidoCor + valorRecibidoDol

            saldoFact -= valorRecibidoTotal

            TextSaldoFact.Text = dt.Rows(highestIndex)(11).ToString
            TextCordobasRec.Text = "0.00"

            If saldoFact = 0 Then

                MessageText = "alertify.alert('La factura ha sido cancelada.');"
                ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessageText, True)

                dt.Rows.Clear()

            ElseIf saldoFact < 0 AndAlso dt.Rows(highestIndex)(19) = "EFECTIVO" Then

                Dim Cambio As Integer = dt.Rows(highestIndex)(11) * -1.0

                If dt.Rows(highestIndex)(20) = "CORDOBAS" Then

                    TextVuelto.Text = Cambio.ToString


                ElseIf dt.Rows(highestIndex)(20) = "DOLARES" Then

                    TextVuelto.Text = Cambio.ToString()

                End If

                MessageText = "alertify.alert('La factura ha sido cancelada. Vuelto es de " & TextVuelto.Text & " C$');"
                ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessageText, True)

                dt.Rows.Clear()

            Else

                MessageText = "alertify.alert('La factura ha sido cancelada.');"
                ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessageText, True)

                dt.Rows.Clear()

            End If

        Catch ex As Exception

            Me.LiteralGridpop.Text &= conn.PmsgBox("Ocurrió un error al intentar cargar la tabla del pop. forma de pago" & ex.Message, "error")
            Session("Continuar") = 0

        End Try
    End Sub

    Private Sub MergeRow()
        Dim dt As DataTable
        dt = If(Session("DistriPago") IsNot Nothing, DirectCast(Session("DistriPago"), DataTable), New DataTable())


        If ddlMoneda.SelectedValue = 1 AndAlso ddlFormaPago.SelectedValue = "E" Then

            dt.Compute("SUM(ValorRecibidoCor)", "Cod_FormaPago = E")
            dt.NewRow()

        End If

        If ddlMoneda.SelectedValue = 2 AndAlso ddlFormaPago.SelectedValue = "E" Then

        End If
    End Sub

    Protected Sub BtnPopupImpGuar_Click(sender As Object, e As EventArgs) Handles BtnPopupImpGuar.Click

        Dim MessageText As String
        Session("Continuar") = 1
        Dim dt As New DataTable
        dt = Session("DistriPago")
        Dim NoFactura As Integer = txtNoFactura.Text
        TextCordobasRec.Text = String.Empty

        If dt.Rows.Count <> 0 Then
            'dt.Tables("Ordenes").Compute("Sum(Total)", "CodigoVendedor = 5") filtra
            Dim sumObject As Object = dt.Compute("Sum(ValorRecibidoCor)", "No_Factura= " & NoFactura & " ")
            Dim valor As Decimal = IIf(IsDBNull(sumObject) = True, 0, sumObject)
            'valor = dt.Compute("Sum(ValorRecibidoCor)", "No_Factura= " & txtNoFac.Text.Trim & " ")

            If CDec(TextSaldoFact.Text) <> 0 Then

                If CDec(TextNeto.Text) - CDec(valor) > 1 Then

                    MessageText = "alertify.alert('La factura no esta cancelada');"
                    ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessageText, True)

                End If

            Else

                GuarFomaPago()
                ImprimirGuardar()

            End If

        ElseIf dt.Rows.Count = 0 Then

            MessageText = "alertify.alert('No ha ingresado forma de pago de Factura');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessageText, True)

        End If
    End Sub

    Private Sub GuarFomaPago()

        Dim MessageText As String
        Dim Sql As String
        Dim dbCon As New OleDbConnection(conn.conn)
        Try
            If dbCon.State = ConnectionState.Closed Then
                dbCon.Open()
            End If

            Dim dt As New DataTable
            dt = Session("DistriPago")

            Sql = ""


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
                      "@ValorRecibidoDol = '" & Convert.ToDecimal(row("ValorRecibidoDol")) & "'," &
                      "@Vuelto = '" & Convert.ToDecimal(row("Vuelto")) & "'," &
                      "@Paridad = '" & Convert.ToDecimal(row("Paridad")) & "'," &
                      "@Anulada = 0," &
                      "@ComisionTarjeta = '" & Convert.ToDecimal(row("ComisionTarjeta")) & "'," &
                      "@Cod_tarjeta = '" & Convert.ToDecimal(row("Cod_tarjeta")) & "'"


            Next


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

    Private Sub ImprimirGuardar()

        Dim MessageText As String
        Dim Sql As String
        Dim dbCon As New OleDbConnection(conn.conn)
        Try
            If dbCon.State = ConnectionState.Closed Then
                dbCon.Open()
            End If

            Dim Contado As Integer

            If rdbContado.Checked Then
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
                      "@no_factura = " & txtNoFactura.Text.Trim & "," &
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
                      "@codvendedor  = '" & Me.ddlVendedor.SelectedValue & "'," &
                      "@cedularuc= '" & TextId.Text.Trim & "'," &
                      "@externo= 0," &
                      "@desc_imprimir = '" & Session("desimprimir") & "'," &
                      "@NombreCliente = '" & TextNombreCliente.Text & "'"

                Dim cmd As New OleDb.OleDbCommand(Sql, dbCon)
                cmd.ExecuteNonQuery()
            Else
                ddlProducto.Focus()
                Return
            End If

            TextPorDesc.Enabled = True
            txtNoFactura.Text = CDec(txtNoFactura.Text) + 1
            TextNombreCliente.Text = String.Empty
            'TextNotas.Text = String.Empty
            TextId.Text = String.Empty
            ProducExistEnInv()
            Response.Redirect(ResolveClientUrl("../Movimientos/Reportes/Factura/ImprimirFactura.aspx"))

        Catch ex As Exception
            MessageText = "alertify.alert('Ha ocurrido un error al intentar guardar la factura. Si el problema persiste, contacte con el administrador. " & Replace(ex.Message, "'", "") & "');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Messege", MessageText, True)

        Finally
            If dbCon.State = ConnectionState.Open Then
                dbCon.Close()
            End If

        End Try
    End Sub

    Protected Sub BtnRegresa_Click(sender As Object, e As EventArgs) Handles BtnRegresa.Click
        Session("Continuar") = 0
        Dim dt As DataTable

        dt = If(Session("DistriPago") IsNot Nothing, DirectCast(Session("DistriPago"), DataTable), New DataTable())

        dt.Rows.Clear()


    End Sub
#End Region

#Region "PROCESOS Y EVENTOS DEL GRIDVIEW FORMA DE PAGO "
    Protected Sub GridViewpop_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridViewpop.DataBound
        Try
            If GridViewpop.Rows.Count > 0 Then

                Dim pagerRow As GridViewRow = GridViewpop.BottomPagerRow
                Dim pageLabel As Label = CType(pagerRow.Cells(0).FindControl("CurrentPageLabel"), Label)

                If pageLabel IsNot Nothing Then

                    Dim currentPage As Integer = GridViewpop.PageIndex + 1
                    pageLabel.Text = "&nbsp;&nbsp; Pagina " & currentPage.ToString() &
                        " de " & GridViewpop.PageCount.ToString()
                End If
            End If
        Catch ex As Exception

            Me.ltMensajeGrid.Text &= conn.PmsgBox("Ocurrió un error al disparar el evento DataBounnd grid Forma de Pago." & ex.Message, "error")

        End Try
    End Sub

    Protected Sub GridViewpop_PageIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridViewpop.PageIndexChanged
        Try

            Me.GridViewpop.SelectedIndex = -1
            Me.HiddenField2.Value = String.Empty

            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "smf_Script", "responsive_grid();", True)

        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.PmsgBox("Ocurrió un error al disparar el evento PageIndexChanged del grid Forma de Pago." & ex.Message, "error")

        End Try
    End Sub

    Protected Sub GridViewpop_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridViewpop.PageIndexChanging
        Try
            Me.GridViewpop.PageIndex = e.NewPageIndex

            'Para usar la de caché guardada en la variable de sesion
            If (IsPostBack) AndAlso (dtTabla IsNot Nothing) Then

                If dtTabla IsNot Nothing AndAlso dtTabla.Rows.Count > 0 Then
                    Me.GridViewpop.DataSource = dtTabla
                    Me.GridViewpop.DataBind()
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
                'e.Row.Attributes.Add("OnMouseOver", "On(this);")
                'e.Row.Attributes.Add("OnMouseOut", "Off(this);")
                'e.Row.Attributes.Add("OnClick", "" & Page.ClientScript.GetPostBackClientHyperlink(Me.GridViewOne, "Select$" + e.Row.RowIndex.ToString) & ";")
            End If

        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.PmsgBox("Ocurrió un error al disparar el evento RowDataBound. DEL GRID FORMA DE PAGO " & ex.Message, "error")

        End Try
    End Sub

    Protected Sub GridViewpop_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles GridViewpop.RowDeleting
        Try
            Dim rowIndex As Integer = e.RowIndex
            DeleteRow(rowIndex)

            Dim dt As DataTable = If(Session("DistriPago") IsNot Nothing, DirectCast(Session("DistriPago"), DataTable), New DataTable())
            Dim valorRecibidoCor As Object = 0
            Dim valorRecibidoDol As Object = 0
            Dim valorRecibidoTotal As Decimal
            Dim saldoFact As Decimal
            Dim highestIndex As Integer = dt.Rows.Count - 1

            dt = Session("DistriPago")

            If dt.Rows.Count > 0 Then

                valorRecibidoCor = dt.Compute("SUM(ValorRecibidoCor)", "No_Factura= " & txtNoFactura.Text.Trim & " ")
                valorRecibidoDol = dt.Compute("SUM(ValorRecibidoDol)", "No_Factura= " & txtNoFactura.Text.Trim & " ")
                valorRecibidoTotal = valorRecibidoCor + valorRecibidoDol

                saldoFact = CDec(TextSaldoFact.Text)
                saldoFact -= valorRecibidoTotal
                TextSaldoFact.Text = dt.Rows(highestIndex)(11).ToString


            ElseIf dt.Rows.Count = 0 Then
                dt.Rows.Clear()
                saldoFact = CDec(TextNeto.Text)
                TextSaldoFact.Text = saldoFact
            Else

                saldoFact = CDec(TextNeto.Text)
                TextSaldoFact.Text = saldoFact

            End If



            TextCordobasRec.Text = "0.00"
            TextVuelto.Text = "0.00"
            'TextConverDolACor.Text = "0.00"

            GridViewpop.DataSource = dt
            GridViewpop.DataBind()

            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "smf_Script", "responsive_grid();", True)
        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.PmsgBox("Ocurrió un error al disparar el evento RowDeleting del grid " & ex.Message, "error")

        End Try
    End Sub

    Private Sub DeleteRow(rowIndex As Integer)

        Dim dt As DataTable = If(Session("DistriPago") IsNot Nothing, DirectCast(Session("DistriPago"), DataTable), New DataTable())

        If rowIndex >= 0 AndAlso rowIndex < dt.Rows.Count Then
            dt.Rows.RemoveAt(rowIndex)
            Session("DistriPago") = dt
            GridViewpop.DataSource = dt
            GridViewpop.DataBind()

            Session("DistriPago") = dt

            Dim MessageText As String = "Registro eliminado."
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Message", MessageText, True)
        End If
    End Sub

    Private Sub ContainsArray()

        Dim arrKeyVals(1) As Object

        'Dim table As DataTable = CType(GridViewpop.DataSource, DataTable)

        Dim table As New DataTable
        table = Session("DistriPago")

        arrKeyVals(0) = EliminarFormaPago
        arrKeyVals(1) = ProductoNoFac

        'Dim rowCollection As DataRowCollection = table.Rows

        'If rowCollection.Contains(EliminarFormaPago) Then

        '    Dim foundRow As DataRow = rowCollection.Find(0)
        '    rowCollection.Remove(foundRow)
        '    Console.WriteLine("Row Deleted")

        'Else
        '    Console.WriteLine("No such row found.")
        'End If

        Dim foundRow As DataRow = table.Rows.Find(arrKeyVals)
        table.Rows.Remove(foundRow)
        Me.GridViewpop.DataSource = table
        Me.GridViewpop.DataBind()

        'Dim dt As New DataTable
        'dt = Session("DistriPago")

    End Sub

    Protected Sub GridViewpop_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridViewpop.SelectedIndexChanged
        Try
            ''"cod_pais,cod_empresa,cod_puesto,no_factura,fecha,cod_producto"
            'Me.HiddenField2.Value = Me.GridViewpop.SelectedValue.ToString
            'EliminarFormaPago = GridViewpop.SelectedDataKey.Values(0)
            ''ProductoEmpre = GridViewpop.SelectedDataKey.Values(1)
            ''ProductoPuesto = GridViewpop.SelectedDataKey.Values(2)
            'ProductoNoFac = GridViewpop.SelectedDataKey.Values(1)

            'ContainsArray()


            'Dim rowIndex As String = GridViewpop.SelectedDataKey.Values(0)


            'DeleteRow(rowIndex)


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

    Private Sub TextSaldoFact_TextChanged(sender As Object, e As EventArgs) Handles TextSaldoFact.TextChanged
        'TextSaldoFactDol.Text = CDec(TextSaldoFact.Text) * Session("Paridad")
    End Sub

#End Region

End Class


