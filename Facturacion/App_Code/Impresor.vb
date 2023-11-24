'Imports Microsoft.VisualBasic
'Imports System.Drawing.Imaging
'Imports System.Drawing.Printing
'Imports System
'Imports System.IO
'Imports System.Data
'Imports System.Text
'Imports System.Collections.Generic
''Imports Microsoft.Reporting.WinForms

'Public Class Impresor
'    'https://www.vbforums.com/showthread.php?686407-RESOLVED-Print-Directly-to-Printer-with-RDLC-Report

'    Implements IDisposable
'    Public PicLocation As String
'    Dim Ctrl1, Ctrl2 As Control
'    Dim CN As New OleDb.OleDbConnection
'    Dim CMD As New OleDb.OleDbCommand
'    Dim DataR As OleDb.OleDbDataReader

'    'To display into datagrid purpose
'    Dim dataS As New DataSet
'    Dim dataAd As New OleDb.OleDbDataAdapter
'    Public SqlStr, SqlStr1, DBPath, DBStatus, SearchBox As String
'    Dim X, Y, SqlH, Onh As Integer
'    Dim SqlUser As String
'    Dim DataP As Decimal
'    Dim Balance As Integer

'    Private m_currentPageIndex As Integer
'    Private m_streams As IList(Of Stream)

'    ' Routine to provide to the report renderer, in order to
'    ' save an image for each page of the report.
'    Private Function CreateStream(ByVal name As String, ByVal fileNameExtension As String, ByVal encoding As Encoding, ByVal mimeType As String, ByVal willSeek As Boolean) As Stream
'        Dim stream As Stream = New MemoryStream()
'        m_streams.Add(stream)
'        Return stream
'    End Function
'    'Private Sub Receipt_Load(sender As Object, e As EventArgs) Handles MyBase.Load
'    '    'Sets the program resolution to 1024 x 768 & centers the form screen via code.
'    '    Me.Size = New System.Drawing.Size(270, 500)

'    '    'TODO: This line of code loads data into the 'ProductListDataSet' table.
'    '    DBPath = (Application.StartupPath & "\ProductList.accdb")
'    '    If CN.State = ConnectionState.Open Then
'    '        CN.Close()
'    '        DBStatus = ("Not Connected")
'    '    End If
'    '    SqlStr = ("Provider = Microsoft.ACE.OLEDB.12.0;Data Source =" & DBPath)
'    '    CN.ConnectionString = SqlStr
'    '    CN.Open()
'    '    Onh = Nothing

'    '    lblTime.Text = CStr(DateAndTime.Now)
'    '    lblUser.Text = Login.userTB.Text

'    '    Dim sql As String
'    '    sql = "SELECT * Receipt"
'    '    Me.ReceiptTableAdapter.ClearBeforeFill = True
'    '    Me.ReceiptTableAdapter.Connection = CN
'    '    Me.ReceiptTableAdapter.Connection.CreateCommand.CommandText = sql
'    '    Me.ReceiptTableAdapter.Fill(Me.ProductListDataSet.Receipt)
'    '    Me.ReportViewer1.RefreshReport()
'    '    Me.Refresh()

'    'End Sub

'    Private Function LoadReceiptData() As DataTable
'        ' Create a new DataSet and read sales data file 
'        ' data.xml into the first DataTable.

'        Me.ReceiptTableAdapter.Fill(Me.ProductListDataSet.Receipt)

'        Return ProductListDataSet.Tables(0)

'    End Function

'    ' Export the given report as an EMF (Enhanced Metafile) file.
'    Private Sub Export(ByVal report As LocalReport)
'        Dim deviceInfo As String = "<DeviceInfo>" & _
'            "<OutputFormat>EMF</OutputFormat>" & _
'            "<PageWidth>8cm</PageWidth>" & _
'            "<PageHeight>29.7cm</PageHeight>" & _
'            "<MarginTop>0in</MarginTop>" & _
'            "<MarginLeft>0in</MarginLeft>" & _
'            "<MarginRight>0in</MarginRight>" & _
'            "<MarginBottom>0in</MarginBottom>" & _
'            "</DeviceInfo>"
'    End Sub
'    Private Sub PrintPage(ByVal sender As Object, ByVal ev As PrintPageEventArgs)
'        Dim pageImage As New Metafile(m_streams(m_currentPageIndex))

'        ' Adjust rectangular area with printer margins.
'        Dim adjustedRect As New Rectangle(ev.PageBounds.Left - CInt(ev.PageSettings.HardMarginX), _
'                                          ev.PageBounds.Top - CInt(ev.PageSettings.HardMarginY), _
'                                          ev.PageBounds.Width, _
'                                          ev.PageBounds.Height)

'        ' Draw a white background for the report
'        ev.Graphics.FillRectangle(Brushes.White, adjustedRect)

'        ' Draw the report content
'        ev.Graphics.DrawImage(pageImage, adjustedRect)

'        ' Prepare for the next page. Make sure we haven't hit the end.
'        m_currentPageIndex += 1
'        ev.HasMorePages = (m_currentPageIndex < m_streams.Count)
'    End Sub


'    Private Sub Print()
'        If m_streams Is Nothing OrElse m_streams.Count = 0 Then
'            Throw New Exception("Error: no stream to print.")
'        End If
'        Dim printDoc As New PrintDocument()
'        If Not printDoc.PrinterSettings.IsValid Then
'            Throw New Exception("Error: cannot find the default printer.")
'        Else
'            AddHandler printDoc.PrintPage, AddressOf PrintPage
'            m_currentPageIndex = 0
'            printDoc.Print()
'        End If
'    End Sub

'    ' Create a local report for Report.rdlc, load the data,
'    ' export the report to an .emf file, and print it.
'    'Private Sub Run()

'    Private Sub PrintPO()
'        Dim report As New LocalReport()
'        report.ReportPath = ("C:\Users\KerrieMcConaghy\Desktop\Epos Project (061113)   LATEST\BEDECK\Receipt.vb")
'        report.DataSources.Add(New ReportDataSource("ProductListDataSet", LoadReceiptData()))
'        Export(report)
'        Print()

'    End Sub
'    Public Sub Dispose() Implements IDisposable.Dispose
'        If m_streams IsNot Nothing Then
'            For Each stream As Stream In m_streams
'                stream.Close()
'            Next
'            m_streams = Nothing
'        End If
'        'End Sub

'        'Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
'        '    Me.ReceiptBindingSource.EndEdit()
'        '    PrintPO()
'        'End Sub
'End Class
