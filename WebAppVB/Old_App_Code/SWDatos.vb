Imports System.Web.Services
Imports System.Data
Imports AjaxControlToolkit
Imports FACTURACION_CLASS

' Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente.
<Script.Services.ScriptService()>
<WebService(Namespace:="http://tempuri.org/")>
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<CompilerServices.DesignerGenerated()>
Public Class SWDatos
    Inherits WebService

    Private ReadOnly _seguridad As New seguridad
    Private ReadOnly _database As New database

    <WebMethod()>
    Public Function GetOrigen(ByVal knownCategoryValues As String,
                            ByVal category As String) As CascadingDropDownNameValue()

        Dim dbCon As New System.Data.OleDb.OleDbConnection(_seguridad.conn)
        Dim SampleSource As New List(Of CascadingDropDownNameValue)
        Try
            If dbCon.State = ConnectionState.Closed Then
                dbCon.Open()
            End If

            Dim sql As String = String.Empty
            sql = " EXEC CombosProductos " &
                  "@opcion = 1," &
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