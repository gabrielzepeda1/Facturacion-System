Imports System.Threading
Imports System.Data.SqlClient
Imports Microsoft.Reporting.WebForms
Imports FACTURACION_CLASS


Partial Class movimientos_Reportes_Factura_ImprimirFactura
    Inherits System.Web.UI.Page
    Dim conn As New seguridad
    Dim DataBase As New database
    Public imprint As New Impresion ' llamando a la clase
    'Dim impresor As New Impresor
    Private Sub DosResport()
        'TODO: esta línea de código carga datos en la tabla 'DataSetImprirFactura.Factura' Puede moverla o quitarla según sea necesario.
        'Me.FacturaTableAdapter.Fill(Me.DataSetImprirFactura.Factura)
        Dim ds As New DataSet()
        Dim da As SqlDataAdapter
        Dim sql As String = String.Empty
        ReportViewer1.ProcessingMode = ProcessingMode.Remote 'Tipo de procesamiento del visor

        ReportViewer1.Visible = True
        'ReportViewer1.SetDisplayMode(DisplayMode.Normal)
        'ReportViewer1.LocalReport.ReportEmbeddedResource = "WindowsApplication1.RptVtasClientes.rdlc"

        sql = " SET DATEFORMAT DMY EXEC ImpDocEnLinea  @opcion = 1, " &
                 "  @codigoPais=" & Session("cod_pais") & ", " &
                 "  @codigoPuesto=" & Session("cod_puesto") & ", " &
                 "  @codigoEmpresa=" & Session("cod_empresa") & ", " &
                 "  @no_factura='" & Session("NoFactura") & "', " &
                 "  @fecha='" & Date.Today & "', " &
                 "  @codcliente='1'  "

        Me.ReportViewer1.LocalReport.DataSources.Clear()


        'Dim parameters As ReportParameter() = New ReportParameter(0) {}
        'parameters(0) = New ReportParameter("CondicionFactura", Condicion)
        'parameters(1) = New ReportParameter("parameterEmpresa", Empresa)
        'da = (New SqlDataAdapter(sql, conn.Conexion_Procedimientos))
        da = (New SqlDataAdapter(sql, conn.Sql_conn))
        da.Fill(ds)
        Dim data1 As New ReportDataSource("DataSet1", ds.Tables(0)) ' DataSet1creasDataSetRptTotal el data source y le asignas al consulta--CostosDataSet
        ReportViewer1.LocalReport.DataSources.Add(data1) 'luego lo agregas
        'ReportViewer1.LocalReport.SetParameters(parameters)
        Me.ReportViewer1.LocalReport.Refresh()


    End Sub
    Private Sub Reporte()
        Dim dbcom As New System.Data.OleDb.OleDbConnection(conn.conn) ''nombre de la conexion
        Try
            If dbcom.State = ConnectionState.Closed Then
                dbcom.Open()
            End If
            Dim sql As String = String.Empty

            sql = " SET DATEFORMAT DMY EXEC ImpDocEnLinea  @opcion = 1, " &
                 "  @codigoPais=" & Session("cod_pais") & ", " &
                 "  @codigoPuesto=" & Session("cod_puesto") & ", " &
                 "  @codigoEmpresa=" & Session("cod_empresa") & ", " &
                 "  @no_factura='" & Session("NoFactura") & "', " &
                 "  @fecha='" & Date.Today & "', " &
                 "  @codcliente='1'  "

            Dim dts As FacturacionDataSet = New FacturacionDataSet ''dts

            Dim cmd As New System.Data.OleDb.OleDbCommand(sql, dbcom)
            cmd.CommandTimeout = 9999

            Dim da As New System.Data.OleDb.OleDbDataAdapter(cmd)

            'Dim da As New System.Data.OleDb.OleDbDataAdapter(sql, dbcom)



            da.Fill(dts, "DataSetImpFact") ''nombre de la tabla de sql es decir la consulta q se llama
            Dim dtDatos As DataTable = dts.Tables(0)
            'ReportViewer1.Reset()
            ReportViewer1.ProcessingMode = ProcessingMode.Local
            Me.ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/movimientos/Reportes/Factura/RptFactura.rdlc")
            ReportViewer1.LocalReport.DataSources.Clear()
            Dim datasource As ReportDataSource = New ReportDataSource("DataSetImpFact", dtDatos)
            ReportViewer1.LocalReport.DataSources.Add(datasource)
            Me.ReportViewer1.LocalReport.Refresh()


            'Me.ReportViewer1.LocalReport.Refresh()
            'ReportViewer1.LocalReport.DataSources.Add(New ReportDataSource("DataSetImpFact", data.Tables(0))
            'dts.DataSetName = "FacturacionDataSet"
            'Me.ReportViewer1.LocalReport.DataSources.Add("ImpDocEnLinea")


            'Me.ReportViewer1.LocalReport.ReportEmbeddedResource = "RptFactura.rdlc"




        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.PmsgBox("Ocurrió un error al intentar cargar el reporte." & ex.Message, "error")

        End Try
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            '', ReportViewer1.DataBinding
            Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("es-NI")
            System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("es-NI")
            'Reporte()
            Dim dbcom As New System.Data.OleDb.OleDbConnection(conn.conn) ''nombre de la conexion
            Try
                If dbcom.State = ConnectionState.Closed Then
                    dbcom.Open()
                End If
                Dim sql As String = String.Empty

                sql = " SET DATEFORMAT DMY EXEC ImpDocEnLinea  @opcion = 1, " &
                     "  @codigoPais=" & Session("cod_pais") & ", " &
                     "  @codigoPuesto=" & Session("cod_puesto") & ", " &
                     "  @codigoEmpresa=" & Session("cod_empresa") & ", " &
                     "  @no_factura='" & Session("NoFactura") & "', " &
                     "  @fecha='" & Date.Today & "', " &
                     "  @codcliente='" & Session("cod_cliente").ToString & "' "

                Dim dts As FacturacionDataSet = New FacturacionDataSet ''dts

                Dim cmd As New System.Data.OleDb.OleDbCommand(sql, dbcom)
                ' cmd.CommandTimeout = 9999
                Dim da As New System.Data.OleDb.OleDbDataAdapter(cmd)


                da.Fill(dts, "ImpDocEnLinea") ''nombre de la tabla de sql es decir la consulta q se llama
                Dim dtDatos As DataTable = dts.Tables(0)


                Dim report As New LocalReport()
                ReportViewer1.ProcessingMode = ProcessingMode.Local
                'ReportViewer1.ShowParameterPrompts = True
                'ReportViewer1.ShowPromptAreaButton = True
                'ReportViewer1.ZoomMode = ZoomMode.Percent
                'ReportViewer1.ZoomPercent = 150
                report.ReportPath = Server.MapPath("~/movimientos/Reportes/Factura/RptFactura.rdlc")
                report.DataSources.Clear()


                Dim datasource As ReportDataSource = New ReportDataSource()
                datasource.Name = "DataSetImpFact"
                datasource.Value = dtDatos
                report.DataSources.Add(datasource)
                report.Refresh()
                Impresion.Imprimir(report)
                Response.Redirect(ResolveClientUrl("~/movimientos/Facturacion.aspx"))

                'ReportViewer1.ProcessingMode = ProcessingMode.Local
                'ReportViewer1.ShowParameterPrompts = True
                'ReportViewer1.ShowPromptAreaButton = True
                'Me.ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/movimientos/Reportes/Factura/RptFactura.rdlc")
                'ReportViewer1.LocalReport.DataSources.Clear()
                'Dim datasource As ReportDataSource = New ReportDataSource("DataSetImpFact", dtDatos)
                'ReportViewer1.LocalReport.DataSources.Add(datasource)
                'Me.ReportViewer1.LocalReport.Refresh()
                'Response.Redirect(ResolveClientUrl("~/movimientos/factura.aspx"))

                '-------------------------------








                '-------------------------------
                'ReportViewer1.LocalReport.Print()
                'Impresion.Imprimir(ReportViewer1.LocalReport)
                'Impresion.Imprimir(Reportt.ReportViewer1.LocalReport) ' ejecuta la libreria
                'ReportViewer1.DataBind()
                'Me.ReportViewer1.LocalReport.Refresh

            Catch ex As Exception
                Me.ltMensajeGrid.Text &= conn.PmsgBox("Ocurrió un error al intentar cargar el reporte." & ex.Message, "error")

            End Try

        End If

    End Sub



    'Dim tel As String = "123"
    'Dim condi As String = Session("condicion").ToString

    'Dim parameters As ReportParameter() = New ReportParameter(0) {}
    ''parameters(0) = New ReportParameter("telefono", tel)
    'parameters(0) = New ReportParameter("Condicion", condi)
    'ReportViewer1.LocalReport.SetParameters(parameters(0))

    'Dim parameters As New List(Of ReportParameter)()
    'parameters.Add(New ReportParameter("condicion", condi))
    'ReportViewer1.ServerReport.SetParameters(parameters)

    'Dim parameters(1) As ReportParameter
    'parameters(0) = New ReportParameter("condicion", condi)
    'parameters(1) = New ReportParameter("phone", tel)
    'ReportViewer1.LocalReport.SetParameters(parameters)

    'Dim p1 As New ReportParameter("condicion", Convert.ToString(condi))
    'Dim p2 As New ReportParameter("phone", tel.ToString)
    ' ReportViewer1.LocalReport.SetParameters(New ReportParameter() {p1, p2})



End Class
