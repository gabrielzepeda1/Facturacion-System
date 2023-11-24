Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Web.UI.HtmlTextWriter
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports System.Collections.Specialized
Imports System.ComponentModel
Imports System.Web.Script.Services
Imports AjaxControlToolkit
Imports System.Xml
Imports System.Web.Script.Serialization

' Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente.
' <System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
<System.Web.Script.Services.ScriptService()> _
Public Class SWDatos
    Inherits System.Web.Services.WebService
    Dim conn As New FACTURACION_CLASS.seguridad
    Dim Database As New FACTURACION_CLASS.database

    <WebMethod()> _
    Public Function GetOrigen(ByVal knownCategoryValues As String, _
                            ByVal category As String) As CascadingDropDownNameValue()

        Dim dbCon As New System.Data.OleDb.OleDbConnection(conn.conn)
        Dim SampleSource As New List(Of CascadingDropDownNameValue)
        Try
            If dbCon.State = ConnectionState.Closed Then
                dbCon.Open()
            End If

            Dim sql As String = String.Empty
            sql = " EXEC CombosProductos " & _
                  "@opcion = 1," & _
                  "@codigo = null "

            Dim cmd As New OleDb.OleDbCommand(sql, dbCon)
            Dim Reader As OleDb.OleDbDataReader = cmd.ExecuteReader
            Do While Reader.Read
                Dim CategoryName = Reader("DesOrigen").ToString()
                Dim CategoryValue = Reader("codOrigen").ToString()
                SampleSource.Add(New CascadingDropDownNameValue(CategoryName, CategoryValue))
            Loop

            Return SampleSource.ToArray()


        Catch ex As Exception
            Throw ex

        Finally
            If dbCon.State = ConnectionState.Open Then
                dbCon.Close()
            End If

        End Try
    End Function


End Class