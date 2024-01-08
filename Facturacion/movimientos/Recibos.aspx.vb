
Imports FACTURACION_CLASS
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
Imports System.Activities.Expressions
Imports System.ComponentModel.Design
Imports System.Drawing
Imports System.Web.UI.WebControls.Expressions
Imports System.Data.OleDb
Imports System.Security
Imports Microsoft.Data.Edm
Imports AjaxControlToolkit
Imports Microsoft.Ajax.Utilities

Partial Class movimientos_Recibos

    Inherits System.Web.UI.Page

    Dim conn As New FACTURACION_CLASS.Seguridad
    Dim DataBase As New FACTURACION_CLASS.database
    Dim BUSQUEDAD As String

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

#Region "PAGE LOAD"
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not IsPostBack Then

            ddlCliente.Enabled = False
            ddlCliente.BackColor = Drawing.Color.WhiteSmoke

            ' Set ddlCuenta to disabled by default and show "SELECCIONE BANCO" to indicate you should select a value from "ddlBanco" first. 
            ddlCuenta.Enabled = False
            ddlCuenta.Items.Insert(0, New ListItem("-SELECCIONE BANCO-", "0"))

            ' Set txtNoRecibo to disabled by default and show "SELECCIONE VENDEDOR" to indicate you should select a value from "ddlVendedor" first.
            txtNoRecibo.Enabled = False
            txtNoRecibo.Text = "-SELECCIONE VENDEDOR-"
            txtNoRecibo.BackColor = Drawing.Color.WhiteSmoke

            CboVendedor()
            CboCliente()
            CboFormaPago()
            CboMoneda()
            CboBanco()
        End If

    End Sub

#End Region

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

#Region "COMBOS"
    Private Sub CboVendedor()

        Try

            Dim dataSet As New DataSet

            dataSet = DataBase.GetDataSet(" EXEC CombosProductos " &
                  "@opcion = 10," &
                  "@codigo = " & Session("cod_pais") & " ")

            If dataSet.Tables(0).Rows.Count > 0 Then
                ddlVendedor.DataSource = dataSet.Tables(0)
                ddlVendedor.DataTextField = "Vendedor"
                ddlVendedor.DataValueField = "codVendedor"
                ddlVendedor.DataBind()
                ddlVendedor.Items.Insert(0, New ListItem("-SELECCIONE-", "0"))
            End If

            dataSet.Dispose()

        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.PmsgBox("Ocurrió un error al intentar cargar el listado de rubros en la tabla. de vendedor" & ex.Message, "error")
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
    Private Sub CboMoneda()

        Try

            Dim dataSet As New DataSet
            dataSet = DataBase.GetDataSet(" EXEC CombosProductos " &
                  "@opcion = 15," &
                  "@codigo = " & Session("cod_pais") & " ")

            If dataSet.Tables(0).Rows.Count > 0 Then
                ddlMoneda.DataSource = dataSet.Tables(0)
                ddlMoneda.DataTextField = "Moneda"
                ddlMoneda.DataValueField = "codMoneda"
                ddlMoneda.DataBind()
            End If

            ddlMoneda.Items.Insert(0, New ListItem("-SELECCIONE-", "0"))

            dataSet.Dispose()

        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.PmsgBox("Ocurrió un error al intentar cargar el listado de rubros en MONEDA" & ex.Message, " error")

        End Try


    End Sub
    Private Sub CboBanco()
        Try

            Dim dataSet As New DataSet
            dataSet = DataBase.GetDataSet(" EXEC CombosProductos " &
                  "@opcion = 14," &
                  "@codigo = " & Session("cod_pais") & " ")

            If dataSet.Tables(0).Rows.Count > 0 Then
                ddlBanco.DataSource = dataSet.Tables(0)
                ddlBanco.DataTextField = "Banco"
                ddlBanco.DataValueField = "codBanco"
                ddlBanco.DataBind()
            End If

            ddlBanco.Items.Insert(0, New ListItem("-SELECCIONE-", "0"))

            dataSet.Dispose()

        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.PmsgBox("Ocurrió un error al intentar cargar el listado de rubros en Bancos" & ex.Message, "error")

        End Try

    End Sub
    Private Sub CboCuentaBanco()


        Try

            Dim dataSet As New DataSet
            Dim codBanco As Integer = Integer.Parse(ddlBanco.SelectedItem.Value)

            If codBanco >= 1 Then
                Dim query As String = String.Format("SELECT cod_banco_cta, descripcion FROM Bancos_cuenta WHERE cod_banco = {0}", codBanco)

                dataSet = DataBase.GetDataSet(query)

                If dataSet.Tables(0).Rows.Count > 0 Then
                    ddlCuenta.Items.Clear()
                    ddlCuenta.DataSource = dataSet.Tables(0)
                    ddlCuenta.DataTextField = "descripcion"
                    ddlCuenta.DataValueField = "cod_banco_cta"
                    ddlCuenta.DataBind()

                    ddlCuenta.Items.Insert(0, New ListItem("-SELECCIONE-", "0"))
                    ddlCuenta.Enabled = True
                Else
                    ddlCuenta.Items.Clear()
                    ddlCuenta.Items.Insert(0, New ListItem("NO EXISTEN REGISTROS", "0"))
                    ddlCuenta.Enabled = False
                End If


                dataSet.Dispose()

            End If

        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.PmsgBox("Ocurrió un error al intentar cargar el listado de rubros en CboCuentaBanco()" & ex.Message, "error")

        End Try

    End Sub

#End Region

#Region "SELECTED INDEX CHANGED"
    Private Sub ddlBanco_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlBanco.SelectedIndexChanged
        CboCuentaBanco()

    End Sub

    Private Sub ddlVendedor_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlVendedor.SelectedIndexChanged
        Dim dbCon As New System.Data.OleDb.OleDbConnection(conn.Conn)
        Dim vCPais As String = Context.Request.Cookies("CKSMFACTURA")("codPais")

        Try

            If dbCon.State = ConnectionState.Closed Then
                dbCon.Open()
            End If

            Dim sql As String = String.Empty
            sql = "SELECT cod_vendedor, num_recibo " &
                  "FROM vendedores " &
                  "WHERE cod_pais = " & vCPais & " AND cod_vendedor = " & ddlVendedor.SelectedItem.Value.ToString() & " "

            Dim cmd As New OleDb.OleDbCommand(sql, dbCon)
            Dim Reader As OleDb.OleDbDataReader = cmd.ExecuteReader()

            If Reader.Read Then
                Dim numRecibo = Reader("num_recibo").ToString
                txtNoRecibo.Text = numRecibo
                txtNoRecibo.BackColor = Drawing.Color.AntiqueWhite
            End If

        Catch ex As Exception
            ltMensaje.Text &= conn.PmsgBox("Ocurrió un error al intentar cargar el listado de rubros en la tabla de vendedor" & ex.Message, "error")

        Finally
            If dbCon.State = ConnectionState.Open Then
                dbCon.Close()
            End If

        End Try
    End Sub







#End Region

#Region "RDB CHECKED CHANGED "

    Private Sub rdbExterno_CheckedChanged(sender As Object, e As EventArgs) Handles rdbExterno.CheckedChanged
        ddlCliente.Enabled = True
        ddlCliente.BackColor = Drawing.Color.White
        CboCliente()
    End Sub

    Private Sub rdbInterno_CheckedChanged(sender As Object, e As EventArgs) Handles rdbInterno.CheckedChanged
        ddlCliente.Enabled = True
        ddlCliente.BackColor = Drawing.Color.White
        CboCliente()
    End Sub

#End Region

#Region "PROCESOS Y EVENTOS DEL GRIDVIEW"
    Private Sub LoadGridView()

        Try
            Dim vCPais As String = String.Empty
            vCPais = Context.Request.Cookies("CKSMFACTURA")("codPais")

            Dim SQL As String = String.Empty
            SQL &= "EXEC ProdAbonos @opcion=3," &
              "@Cod_pais =  " & vCPais & " ," &
              "@Cod_puesto =  " & Request.Cookies("CKSMFACTURA")("CodPuesto") & "  ," &
              "@Cod_empresa = " & Request.Cookies("CKSMFACTURA")("CodEmpresa") & "  ," &
              "@Num_Recibo = " & txtNoRecibo.Text.Trim & "," &
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

#End Region

End Class
