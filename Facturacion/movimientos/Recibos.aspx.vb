
Imports System.Data
Imports System.Data.OleDb
Imports FACTURACION_CLASS
Imports FACTURACION_CLASS.DropdownsClass

Partial Class movimientos_Recibos

    Inherits Page

    Dim conn As New seguridad
    Dim DataBase As New database

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
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not IsPostBack Then
            ddlCliente.BackColor = Drawing.Color.WhiteSmoke

            ddlCuenta.Enabled = False
            ddlCuenta.Items.Insert(0, New ListItem("-SELECCIONE BANCO-", "0"))
            txtNoRecibo.Enabled = False
            txtNoRecibo.Text = "-SELECCIONE VENDEDOR-"
            txtNoRecibo.BackColor = Drawing.Color.WhiteSmoke

            BindDropDownListClientes(ddlCliente, rdbExterno)
            BindDropDownListVendedor(ddlVendedor)
            BindDropDownListFormaPago(ddlFormaPago)
            BindDropDownListMoneda(ddlMoneda)
            BindDropDownListBanco(ddlBanco)
        End If

    End Sub

#Region "VALIDACIONES "
    Private Sub BtnBuscar_Click(sender As Object, e As EventArgs) Handles BtnBuscar.Click

        Dim MessageText As String
        Dim allValidationsPassed As Boolean = True

        If ddlVendedor.Text = String.Empty Then
            MessageText = "alertify.alert('Seleccione el vendedor.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Message", MessageText, True)
            allValidationsPassed = False
            Return
        End If

        If ddlCliente.Text = String.Empty Then
            MessageText = "alertify.alert('Seleccione el cliente.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Message", MessageText, True)
            allValidationsPassed = False
            Return
        End If

        If txtNoRecibo.Text = String.Empty Then
            MessageText = "alertify.alert('.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Message", MessageText, True)
            allValidationsPassed = False
            Return
        End If

        If txtValor.Text = String.Empty Then
            MessageText = "alertify.alert('Ingrese la cantidad del pago.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Message", MessageText, True)
            allValidationsPassed = False
            Return
        End If

        If ddlMoneda.Text = String.Empty Then
            MessageText = "alertify.alert('Seleccione Moneda.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Message", MessageText, True)
            allValidationsPassed = False
            Return
        End If

        If ddlFormaPago.Text = String.Empty Then
            MessageText = "alertify.alert('Seleccione Forma de Pago.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Message", MessageText, True)
            allValidationsPassed = False
            Return
        End If

        If txtCheque.Text = String.Empty Then
            MessageText = "alertify.alert('Ingrese el numero de cheque.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Message", MessageText, True)
            allValidationsPassed = False
            Return
        End If

        allValidationsPassed = True



        LoadGridView()
    End Sub

#End Region

#Region "SELECTED INDEX CHANGED"
    Private Sub ddlBanco_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlBanco.SelectedIndexChanged
        BindDropDownListCuentaBanco(ddlCuenta, ddlBanco.SelectedItem.Value)
    End Sub

    Private Sub ddlVendedor_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlVendedor.SelectedIndexChanged
        Try
            Using dbCon As New OleDbConnection(conn.conn)

                Dim sql = "SELECT cod_vendedor, num_recibo " &
                  "FROM vendedores " &
                  "WHERE cod_pais = " & Session("CodigoPais") & " AND cod_vendedor = " & ddlVendedor.SelectedItem.Value.ToString() & " "

                Using cmd As New OleDbCommand(sql, dbCon)
                    Using dr As OleDbDataReader = cmd.ExecuteReader()
                        While dr.Read
                            txtNoRecibo.Text = dr("num_recibo").ToString()
                            txtNoRecibo.BackColor = Drawing.Color.AntiqueWhite
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            AlertifyClass.AlertifyErrorMessage(Me, "Error al cargar el ddlVendedor")
        End Try
    End Sub
#End Region

#Region "RDB CHECKED CHANGED"
    Private Sub rdbExterno_CheckedChanged(sender As Object, e As EventArgs) Handles rdbExterno.CheckedChanged
        ddlCliente.Enabled = True
        ddlCliente.BackColor = Drawing.Color.White
        BindDropDownListClientes(ddlCliente, rdbExterno)
    End Sub

#End Region

#Region "PROCESOS Y EVENTOS DEL GRIDVIEW"
    Private Sub LoadGridView()
        Try

            Dim sql = "EXEC ProdAbonos @opcion=3," &
              "@Cod_pais =  " & Session("CodigoPais") & " ," &
              "@Cod_puesto =  " & Session("CodigoPuesto") & "  ," &
              "@Cod_empresa = " & Session("CodigoEmpresa") & "  ," &
              "@Num_Recibo = " & txtNoRecibo.Text.Trim() & "," &
              "@Tipo_Docum =  NULL ," &
              "@Num_Docum =  NULL ," &
              "@fechadia =  NULL ," &
              "@Interno_Externo =  NULL ," &
              "@Cod_cliente =  NULL ," &
              "@Valor_Pend =  NULL ," &
              "@Valor_Apli =  NULL ," &
              "@Saldo =  NULL ," &
              "@consecutivo_usuario =  NULL ," &
              "@Fecha_docum =  NULL ," &
              "@cod_sector_mercado =  NULL ,"

            Dim ds As DataSet = DataBase.GetDataSet(sql)

            dtTabla = ds.Tables(0)

            GridViewOne.DataSource = dtTabla.DefaultView
            GridViewOne.DataBind()
            ds.Dispose()

            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "smf_Script", "responsive_grid()", True)
        Catch ex As Exception
            AlertifyClass.AlertifyAlertMessage(Me, "Ocurrió un error al cargar el listado de rubros en la tabla.")
        End Try
    End Sub
#End Region
End Class
