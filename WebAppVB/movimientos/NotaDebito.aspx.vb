
Imports FACTURACION_CLASS
Imports System.Data
Imports System.Data.OleDb
Imports System.Drawing


Partial Class movimientos_NotaDebito
    Inherits System.Web.UI.Page

    Dim conn As New seguridad
    Dim DataBase As New database
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


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not IsPostBack Then


            txtFecha().Text = DateTime.Now.ToString("dd/MM/yyyy")
            No_Nota_Debito()
            CboCliente()

        End If

    End Sub
    Private Sub CboCliente()

        Dim vext As Integer
        Dim dataSet As New DataSet
        Dim Sql As String
        Dim dt As DataTable

        Try

            'If rdbInterno.Checked Then
            '    vext = 0 'SI RADIO BUTTON EXTERNO NO ESTA SELECCTIONADO, variable vext = 0'
            'Else
            '    vext = 1
            'End If


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
            'Me.ltMensajeGrid.Text &= conn.pmsgBox("Ocurrió un error al intentar cargar el listado de rubros en la tabla. Procedimiento CboCliente()" & ex.Message, "error")

        End Try
    End Sub

    Private Sub ddlCliente_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCliente.SelectedIndexChanged


        '' En este evento se debe traer el valor de txtNotaDebito de la base de datos SQL.
        '' Cuando se incrementa el INDEX de la tabla C_Not_De. Se debe hacer un UPDATE en la tabla





    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click

        Dim MessageText As String
        Dim allValidationsPassed As Boolean
        allValidationsPassed = False

        '' IF RDB EXTERNO IS NOT CHECKED, THEN CHECK IF RDB INTERNO IS NOT CHECKED. IF BOTH ARE NOT CHECKED, THEN SHOW MESSAGE. IF ONE OF THEM IS CHECKED THEN CONTINUE

        'If rdbExterno.Checked = False And rdbInterno.Checked = False Then
        '    MessageText = "alertify.alert('Seleccione si el cliente es EXTERNO o INTERNO.');"
        '    ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Message", MessageText, True)
        '    Return

        'End If

        If ddlCliente.Text = String.Empty Then
            MessageText = "alertify.alert('Seleccione el nombre del CLIENTE.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Message", MessageText, True)
            Return
        End If

        If txtNotaDebito.Text = String.Empty Then
            MessageText = "alertify.alert('No existe el numero de NOTA DE DEBITO.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Message", MessageText, True)
            Return
        End If

        If txtMonto.Text = String.Empty Then
            MessageText = "alertify.alert('Ingrese el DÉBITO A CUENTA $.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Message", MessageText, True)
            Return
        End If

        If txtConcepto.Text = String.Empty Then
            MessageText = "alertify.alert('Ingrese el CONCEPTO de la nota de débito.');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Message", MessageText, True)
            Return
        End If

        allValidationsPassed = True

        If (allValidationsPassed) Then

            InsertNotaDebito()


        End If


    End Sub

    Private Sub InsertNotaDebito()
        Dim Message As String = String.Empty
        Dim dbCon As New OleDb.OleDbConnection(conn.conn)
        Dim query As String = String.Empty
        Dim ExternoInterno As Integer

        Try

            If dbCon.State = ConnectionState.Closed Then
                dbCon.Open()
            End If

            'If rdbInterno.Checked Then
            '    ExternoInterno = 0 'SI RADIO BUTTON EXTERNO NO ESTA SELECCTIONADO, variable vext = 0'
            'ElseIf rdbExterno.Checked Then
            '    ExternoInterno = 1
            'End If

            query = "SET DATEFORMAT DMY " & vbCrLf

            query &= "EXECUTE [Insert_Nota_Debito]" &
                      "@Valor_Neto =  " & txtMonto.Text & " ," &
                      "@Cod_Cliente = '" & ddlCliente.SelectedItem.Value.ToString() & "' ," &
                      "@Cod_Puesto =  " & Request.Cookies("CKSMFACTURA")("CodPuesto") & " ," &
                      "@CodigoUser =  " & Request.Cookies("CKSMFACTURA")("cod_usuario") & " ," &
                      "@Fecha_Canc =  NULL ," &
                      "@Concepto =   '" & txtConcepto.Text & "' ," &
                      "@Interno_Externo =   " & ExternoInterno & " ," &
                      "@Cod_Empresa =   " & Request.Cookies("CKSMFACTURA")("CodEmpresa") & " ," &
                      "@Codigo_Pais =   " & Request.Cookies("CKSMFACTURA")("CodPais") & " "


            Dim cmd As New OleDbCommand(query, dbCon)
            cmd.ExecuteNonQuery()

        Catch ex As Exception
            Message = "alertify.error('Ha ocurrido un error al guardar la nota de debito. " & Replace(ex.Message, "'", "") & "');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Message", Message, True)
        Finally
            If dbCon.State = ConnectionState.Open Then
                dbCon.Close()
            End If
        End Try

        ltMensaje.Text = conn.PmsgBox("La nota de debito se ha guardado correctamente.", "success")

    End Sub

    Private Sub No_Nota_Debito()
        Dim Message As String = String.Empty
        Dim query As String = String.Empty
        Dim dbCon As New OleDb.OleDbConnection(conn.conn)

        Try

            If dbCon.State = ConnectionState.Closed Then
                dbCon.Open()
            End If


            query = "SELECT no_nota_debito+1 AS no_nota_debito FROM Puestos " &
                   "WHERE cod_puesto = " & Request.Cookies("CKSMFACTURA")("CodPuesto") &
                    "AND cod_empresa = " & Request.Cookies("CKSMFACTURA")("CodEmpresa") &
                    "AND cod_pais = " & Request.Cookies("CKSMFACTURA")("CodPais")

            Dim cmd As New OleDbCommand(query, dbCon)
            Dim dr As OleDbDataReader = cmd.ExecuteReader()

            If dr.Read() Then
                txtNotaDebito.Text = dr("no_nota_debito").ToString()
            End If

        Catch ex As Exception
            Message = "alertify.error('Ha ocurrido un error al guardar la nota de debito. " & Replace(ex.Message, "'", "") & "');"
            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "Message", Message, True)

        Finally
            If dbCon.State = ConnectionState.Open Then
                dbCon.Close()
            End If

        End Try

    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click





    End Sub

    Private Sub Clear()
        txtMonto.Text = String.Empty
        ddlCliente.ClearSelection()



    End Sub
End Class
