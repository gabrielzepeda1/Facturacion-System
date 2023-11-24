Imports Microsoft.VisualBasic
Imports System
Imports System.IO
Imports System.Data
Imports System.Text
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Drawing.Printing
Imports System.Collections.Generic
Imports System.Windows.Forms
Imports Microsoft.Reporting.WebForms


Public Class Impresion
    Implements IDisposable
    Private m_currentPageIndex As Integer
    Private m_streams As IList(Of Stream)

    ' Routine to provide to the report renderer, in order to
    ' save an image for each page of the report.
    Private Function CreateStream(ByVal name As String, ByVal fileNameExtension As String, ByVal encoding As Encoding, ByVal mimeType As String, ByVal willSeek As Boolean) As Stream
        Dim stream As Stream = New MemoryStream()
        m_streams.Add(stream)
        Return stream
    End Function

    ' Export the given report as an EMF (Enhanced Metafile) file.
    Private Sub Export(ByVal report As LocalReport)
        ''Dim deviceInfo As String = "" &
        '' "EMF" &
        '' "8.5in" &
        '' "11in" &
        '' "0.25in" &
        '' "0.25in" &
        '' "0.25in" &
        '' "0.25in" &
        '' ""
        Dim deviceInfo As String = "<DeviceInfo>" & _
        "<OutputFormat>EMF</OutputFormat>" & _
        "<PageWidth>3n</PageWidth>" & _
        "<PageHeight>11in</PageHeight>" & _
        "<MarginTop>0.15in</MarginTop>" & _
        "<MarginLeft>0.15in</MarginLeft>" & _
        "<MarginRight>0.15in</MarginRight>" & _
        "<MarginBottom>0.15in</MarginBottom>" & _
        "</DeviceInfo>"
        Dim warnings As Warning()
        m_streams = New List(Of Stream)()
        report.Render("Image", deviceInfo, AddressOf CreateStream, warnings)
        For Each stream As Stream In m_streams
            stream.Position = 0
        Next
    End Sub
    ''Private Sub Export(ByVal report As LocalReport)
    '    Dim deviceInfo As String = "<DeviceInfo>" & _
    '    "<OutputFormat>EMF</OutputFormat>" & _
    '    "<PageWidth>8.5in</PageWidth>" & _
    '    "<PageHeight>11in</PageHeight>" & _
    '    "<MarginTop>0.25in</MarginTop>" & _
    '    "<MarginLeft>0.25in</MarginLeft>" & _
    '    "<MarginRight>0.25in</MarginRight>" & _
    '    "<MarginBottom>0.25in</MarginBottom>" & _
    '    "</DeviceInfo>"
    '    Dim warnings As Warning()
    '    m_streams = New List(Of Stream)()
    '    report.Render("Image", deviceInfo, AddressOf CreateStream, warnings)
    '    For Each stream As Stream In m_streams
    '        stream.Position = 0
    '    Next
    'End Sub

    ' Handler for PrintPageEvents
    Private Sub PrintPage(ByVal sender As Object, ByVal ev As PrintPageEventArgs)
        Dim pageImage As New Metafile(m_streams(m_currentPageIndex))

        ' Adjust rectangular area with printer margins.
        Dim adjustedRect As New Rectangle(ev.PageBounds.Left - CInt(ev.PageSettings.HardMarginX),
            ev.PageBounds.Top - CInt(ev.PageSettings.HardMarginY),
            ev.PageBounds.Width,
            ev.PageBounds.Height)

        ' Draw a white background for the report
        ev.Graphics.FillRectangle(Brushes.White, adjustedRect)

        ' Draw the report content
        ev.Graphics.DrawImage(pageImage, adjustedRect)

        ' Prepare for the next page. Make sure we haven't hit the end.
        m_currentPageIndex += 1
        ev.HasMorePages = (m_currentPageIndex < m_streams.Count)
    End Sub
    

    Private Sub Print()

        If m_streams Is Nothing OrElse m_streams.Count = 0 Then
            Throw New Exception("Error: no se puede imprimir.")
        End If

        Dim printDoc As New PrintDocument()
        'Dim NombreImpresora As String = "EPSON TM-U220 Receipt"
        'printDoc.PrinterSettings.PrinterName = NombreImpresora
        'If Not printDoc.PrinterSettings.IsValid Then
        '    Throw New Exception("Error: no se puede hallar la impresora.")
        'Else
        '    AddHandler printDoc.PrintPage, AddressOf PrintPage
        '    m_currentPageIndex = 0
        '    printDoc.Print()
        'End If
        '
        Dim printerName As [String] = ImpresoraPredeterminada()
        If m_streams Is Nothing OrElse m_streams.Count = 0 Then
            Return
        End If
        printDoc = New PrintDocument()
        printDoc.PrinterSettings.PrinterName = printerName
        If Not printDoc.PrinterSettings.IsValid Then
            Dim msg As String = [String].Format("Can't find printer ""{0}"".", printerName)
            'MessageBox.Show(msg, "Print Error")
            Throw New Exception("Error al imprimir en clase PRINTER")
            Return
        End If

        AddHandler printDoc.PrintPage, New PrintPageEventHandler(AddressOf PrintPage)
        'printDoc.PrintPage += New PrintPageEventHandler(AddressOf PrintPage)
        printDoc.Print()
    End Sub

    Private Function ImpresoraPredeterminada() As String
        For i As Int32 = 0 To PrinterSettings.InstalledPrinters.Count - 1
            Dim a As New PrinterSettings()
            a.PrinterName = PrinterSettings.InstalledPrinters(i).ToString()
            ' If a.IsDefaultPrinter Then
            'Return PrinterSettings.InstalledPrinters(i).ToString()
            Return "EPSON TM-U220 ReceiptE4"
            '    End If
        Next
        'Return ""
    End Function



    ' Create a local report for Report.rdlc, load the data,
    ' export the report to an .emf file, and print it.
    Private Sub Run(ByVal rp As LocalReport)
        Export(rp)
        Print()
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        If m_streams IsNot Nothing Then
            For Each stream As Stream In m_streams
                stream.Close()
            Next
            m_streams = Nothing
        End If
    End Sub

    Public Shared Sub Imprimir(ByRef rp As LocalReport)
        Using demo As New Impresion
            demo.Run(rp)
        End Using
    End Sub

    'Private Sub PrintPO()
    '    Dim report As New LocalReport()
    '    report.ReportPath = ("C:\Users\JOE\Desktop\Deployment\POS - Copy\POS\rptReceipt.rdlc")
    '    report.DataSources.Add(New ReportDataSource("ReceiptDataSet", LoadSalesData()))
    '    Export(report)
    '    Print()

    'End Sub
End Class

