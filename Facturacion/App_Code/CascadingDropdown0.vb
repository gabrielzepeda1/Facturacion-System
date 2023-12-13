
Imports System.Web.UI.HtmlTextWriter
Imports System.Data
Imports System.Collections
Imports System.Collections.Specialized
Imports System.ComponentModel
Imports System.Xml
Imports System.Web.Script.Serialization
Imports System.Web.Script.Services
Imports AjaxControlToolkit
Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Collections.Generic

Imports System.Data.SqlClient

' Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente.
' <System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
<System.Web.Script.Services.ScriptService()> _
Public Class CascadingDropdown0
    Inherits System.Web.Services.WebService



    Dim conn As New FACTURACION_CLASS.Seguridad
    Dim Database As New FACTURACION_CLASS.database
    '<WebMethod()> _
    'Public Function GetVendors(ByVal knownCategoryValues As String, ByVal category As String) As CascadingDropDownNameValue()
    '    Dim l As New List(Of CascadingDropDownNameValue)
    '    l.Add(New CascadingDropDownNameValue("International", "1"))
    '    l.Add(New CascadingDropDownNameValue("Electronic Bike Repairs & Supplies", "2"))
    '    l.Add(New CascadingDropDownNameValue("Premier Sport, Inc.", "3"))
    '    Return l.ToArray()

    'End Function


    <WebMethod(MessageName:="GetWelcomeMessageWithName")>
    Public Function GetWelcomeMessage(ByVal name As String) As String

    End Function
    '<WebMethod()> _
    'Public Function GetVendors(ByVal knownCategoryValues As String, ByVal category As String) As CascadingDropDownNameValue()
    '    'Dim conn As New SqlConnection("server=(local)\SQLEXPRESS; Integrated Security=true; Initial Catalog=AdventureWorks")
    '    'conn.Open()
    '    Dim dbCon As New SqlConnection(conn.conn)
    '    dbCon.Open()
    '    Dim comm As New SqlCommand( _
    '    "SELECT cod_origen as codOrigen,Descripcion AS DesOrigen  FROM  Origenes_productos ", dbCon)




    '    Dim dr As SqlDataReader = comm.ExecuteReader()
    '    Dim l As New List(Of CascadingDropDownNameValue)
    '    While (dr.Read())
    '        l.Add(New CascadingDropDownNameValue(dr("DesOrigen").ToString(), dr("codOrigen").ToString()))
    '    End While
    '    dbCon.Close()
    '    Return l.ToArray()
    'End Function

    'Public Function GetVendors(ByVal knownCategoryValues As String, _
    '                       ByVal category As String) As CascadingDropDownNameValue()

    '    Dim dbCon As New System.Data.OleDb.OleDbConnection(conn.conn)
    '    Dim SampleSource As New List(Of CascadingDropDownNameValue)
    '    Try
    '        If dbCon.State = ConnectionState.Closed Then
    '            dbCon.Open()
    '        End If

    '        Dim sql As String = String.Empty
    '        sql = " EXEC CombosProductos " & _
    '              "@opcion = 1," & _
    '              "@codigo = null "

    '        Dim cmd As New OleDb.OleDbCommand(sql, dbCon)
    '        Dim Reader As OleDb.OleDbDataReader = cmd.ExecuteReader

    '        Do While Reader.Read
    '            Dim CategoryName = Reader("codOrigen").ToString()

    '            Dim CategoryValue = Reader("DesOrigen").ToString()

    '            SampleSource.Add(New CascadingDropDownNameValue(CategoryName, CategoryValue))
    '        Loop

    '        Return SampleSource.ToArray()

    '    Catch ex As Exception
    '        Throw ex

    '    Finally
    '        If dbCon.State = ConnectionState.Open Then
    '            dbCon.Close()
    '        End If

    '    End Try
    'End Function

    '<WebMethod()> _
    'Public Function GetContactsForVendor(ByVal knownCategoryValues As String, ByVal category As String) As CascadingDropDownNameValue()
    '    Dim VendorID As Integer
    '    CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)
    '    If Not kv.ContainsKey("Vendor") Or Not Int32.TryParse(kv("Vendor"), VendorID) Then
    '        Throw New ArgumentException("Couldn't find vendor.")
    '    End If

    '    Dim conn As New SqlConnection("server=(local)\SQLEXPRESS; Integrated Security=true; Initial Catalog=AdventureWorks")
    '    conn.Open()
    '    Dim comm As New SqlCommand("SELECT Person.Contact.ContactID, FirstName, LastName FROM Person.Contact,Purchasing.VendorContact WHERE VendorID=@VendorID AND Person.Contact.ContactID=Purchasing.VendorContact.ContactID", conn)
    '    comm.Parameters.AddWithValue("@VendorID", VendorID)
    '    Dim dr As SqlDataReader = comm.ExecuteReader()
    '    Dim l As New List(Of CascadingDropDownNameValue)
    '    While (dr.Read())
    '        l.Add(New CascadingDropDownNameValue(dr("FirstName").ToString() & " " & dr("LastName").ToString(), dr("ContactID").ToString()))
    '    End While
    '    conn.Close()
    '    Return l.ToArray()
    'End Function
End Class

