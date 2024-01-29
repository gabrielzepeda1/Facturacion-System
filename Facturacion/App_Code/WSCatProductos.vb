Imports System.Data
Imports System.Data.OleDb
Imports System.Web.Services
Imports AjaxControlToolkit

' Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente.
<System.Web.Script.Services.ScriptService()>
<WebService(Namespace:="http://tempuri.org/")>
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<CompilerServices.DesignerGenerated()>
Public Class WSCatProductos
    Inherits WebService
    Dim conn As New FACTURACION_CLASS.seguridad
    Dim Database As New FACTURACION_CLASS.database
    ''' <summary>
    ''' ?????
    ''' </summary>
    ''' <param name="knownCategoryValues"></param>
    ''' <param name="category"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()>
    Public Function GetSigla(ByVal knownCategoryValues As String,
                              ByVal category As String) As CascadingDropDownNameValue()

        Dim dbCon As New System.Data.OleDb.OleDbConnection(conn.conn)
        Dim SampleSource As New List(Of CascadingDropDownNameValue)
        Try
            If dbCon.State = ConnectionState.Closed Then
                dbCon.Open()
            End If

            Dim sql As String = String.Empty
            sql = " EXEC CombosProductos " &
                  "@opcion = 5," &
                  "@codigo = null "

            Dim cmd As New OleDb.OleDbCommand(sql, dbCon)
            Dim Reader As OleDb.OleDbDataReader = cmd.ExecuteReader

            Do While Reader.Read
                Dim CategoryName = Reader("Sigla").ToString()
                Dim CategoryValue = Reader("Sigla").ToString()
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
    ''' <summary>
    ''' ?????
    ''' </summary>
    ''' <param name="knownCategoryValues"></param>
    ''' <param name="category"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()>
    Public Function GetOrigen(ByVal knownCategoryValues As String,
                              ByVal category As String) As CascadingDropDownNameValue()

        Dim dbCon As New System.Data.OleDb.OleDbConnection(conn.conn)
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


    ''' <summary>
    ''' ?????
    ''' </summary>
    ''' <param name="knownCategoryValues"></param>
    ''' <param name="category"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()>
    Public Function GetCalidades(ByVal knownCategoryValues As String,
                              ByVal category As String) As CascadingDropDownNameValue()

        Dim dbCon As New System.Data.OleDb.OleDbConnection(conn.conn)
        Dim SampleSource As New List(Of CascadingDropDownNameValue)
        Try
            If dbCon.State = ConnectionState.Closed Then
                dbCon.Open()
            End If

            Dim sql As String = String.Empty
            sql = " EXEC CombosProductos " &
                  "@opcion = 2," &
                  "@codigo = null "

            Dim cmd As New OleDb.OleDbCommand(sql, dbCon)
            Dim Reader As OleDb.OleDbDataReader = cmd.ExecuteReader

            Do While Reader.Read
                Dim CategoryName = Reader("DesCalidad").ToString()
                Dim CategoryValue = Reader("codCalidad").ToString()
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
    ''' <summary>
    ''' ?????
    ''' </summary>
    ''' <param name="knownCategoryValues"></param>
    ''' <param name="category"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()>
    Public Function GetPresentacion(ByVal knownCategoryValues As String,
                              ByVal category As String) As CascadingDropDownNameValue()

        Dim dbCon As New System.Data.OleDb.OleDbConnection(conn.conn)
        Dim SampleSource As New List(Of CascadingDropDownNameValue)
        Try
            If dbCon.State = ConnectionState.Closed Then
                dbCon.Open()
            End If

            Dim sql As String = String.Empty
            sql = " EXEC CombosProductos " &
                  "@opcion = 3," &
                  "@codigo = null "

            Dim cmd As New OleDb.OleDbCommand(sql, dbCon)
            Dim Reader As OleDb.OleDbDataReader = cmd.ExecuteReader

            Do While Reader.Read
                Dim CategoryName = Reader("DesPresentacion").ToString()
                Dim CategoryValue = Reader("codPresentacion").ToString()
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
    ''' <summary>
    ''' ?????
    ''' </summary>
    ''' <param name="knownCategoryValues"></param>
    ''' <param name="category"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()>
    Public Function GetFamilia(ByVal knownCategoryValues As String,
                              ByVal category As String) As CascadingDropDownNameValue()

        Dim dbCon As New System.Data.OleDb.OleDbConnection(conn.conn)
        Dim SampleSource As New List(Of CascadingDropDownNameValue)
        Try
            If dbCon.State = ConnectionState.Closed Then
                dbCon.Open()
            End If

            Dim sql As String = String.Empty
            sql = " EXEC CombosProductos " &
                  "@opcion = 4," &
                  "@codigo = null "

            Dim cmd As New OleDb.OleDbCommand(sql, dbCon)
            Dim Reader As OleDb.OleDbDataReader = cmd.ExecuteReader

            Do While Reader.Read
                Dim CategoryName = Reader("DesFamilia").ToString()
                Dim CategoryValue = Reader("codFamilia").ToString()
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
    <WebMethod()>
    Public Function GetPaises(knownCategoryValues As String, category As String) As CascadingDropDownNameValue()

        Dim Param As StringDictionary = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)
        Dim SampleSource As New List(Of CascadingDropDownNameValue)
        Try

            Using dbCon As New OleDbConnection(conn.conn)
                dbCon.Open()

                Dim vUser As String = Session("CodigoUser")

                Dim cmd As New OleDbCommand("CombosProductos", dbCon)
                cmd.Parameters.AddWithValue("opcion", 6)
                cmd.Parameters.AddWithValue("codigo", vUser)
                cmd.CommandType = CommandType.StoredProcedure

                Using dr As OleDbDataReader = cmd.ExecuteReader()
                    Do While dr.Read
                        Dim CategoryName = dr("Pais").ToString()
                        Dim CategoryValue = dr("CodigoPais").ToString()
                        SampleSource.Add(New CascadingDropDownNameValue(CategoryName, CategoryValue))
                    Loop
                End Using
            End Using

            Return SampleSource.ToArray()
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    ''' <summary></summary>
    ''' <param name="knownCategoryValues"></param>
    ''' <param name="category"></param>
    ''' <returns>CascadingDropDownNameValue</returns>
    ''' <remarks></remarks>
    <WebMethod()>
    Public Function GetEmpresa(knownCategoryValues As String, category As String) As CascadingDropDownNameValue()

        Dim dbCon As New OleDbConnection(conn.conn)
        Dim SampleSource As New List(Of CascadingDropDownNameValue)
        Dim Param As StringDictionary = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)

        Try
            If dbCon.State = ConnectionState.Closed Then
                dbCon.Open()
            End If

            Dim vPais As String = String.Empty
            Dim vUser As String = Session("CodigoUser")

            If Param("CategoryPais") = String.Empty Then
                vPais = Context.Request.Cookies("CKSMFACTURA")("CodigoPais")
            Else
                vPais = Param("CategoryPais")
            End If

            Dim sql = " SELECT distinct e.cod_empresa as codEmpresa, RTRIM(LTRIM(e.descripcion_corta)) AS Empresa " &
                  " FROM empresas AS e " &
                  " INNER JOIN accesos_usuarios AS a ON a.cod_empresa = e.cod_empresa " &
                  " WHERE a.consecutivo_usuario = " & vUser &
                  " AND a.cod_pais = " & vPais & " "

            Dim cmd As New OleDbCommand(sql, dbCon)
            Dim Reader As OleDbDataReader = cmd.ExecuteReader()

            Do While Reader.Read
                Dim CategoryName = Reader("Empresa").ToString()
                Dim CategoryValue = Reader("codEmpresa").ToString()
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

    <WebMethod()>
    Public Function GetPuesto(ByVal knownCategoryValues As String,
                              ByVal category As String) As CascadingDropDownNameValue()

        Dim dbCon As New System.Data.OleDb.OleDbConnection(conn.conn)
        Dim SampleSource As New List(Of CascadingDropDownNameValue)
        Dim Param As StringDictionary = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)
        Try
            If dbCon.State = ConnectionState.Closed Then
                dbCon.Open()
            End If

            Dim vUser As String = Context.Request.Cookies("CKSMFACTURA")("CodigoUser")
            Dim vPais As String
            Dim vEmpresa As String

            If Param("CategoryPais") = String.Empty Then
                vPais = Context.Request.Cookies("CKSMFACTURA")("CodigoPais")
            Else
                vPais = Param("CategoryPais")
            End If

            If Param("CategoryEmpresa") = String.Empty Then
                vEmpresa = Context.Request.Cookies("CKSMFACTURA")("CodigoEmpresa")
            Else
                vEmpresa = Param("CategoryEmpresa")
            End If

            Dim sql As String = "SELECT a.cod_puesto AS codPuesto, RTRIM(LTRIM(p.descripcion)) AS Puesto" &
                                " FROM Accesos_Usuarios AS a " &
                                " INNER JOIN Puestos AS p on p.cod_puesto = a.cod_puesto " &
                                " WHERE a.consecutivo_usuario = " & vUser &
                                " AND a.cod_pais = " & vPais &
                                " AND a.cod_empresa = " & vEmpresa & " "

            Dim cmd As New OleDbCommand(sql, dbCon)
            Dim Reader As OleDbDataReader = cmd.ExecuteReader

            Do While Reader.Read
                Dim CategoryName = Reader("Puesto").ToString()
                Dim CategoryValue = Reader("codPuesto").ToString()
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

    <WebMethod()>
    Public Function GetUsuario(ByVal knownCategoryValues As String,
                              ByVal category As String) As CascadingDropDownNameValue()

        Dim dbCon As New System.Data.OleDb.OleDbConnection(conn.conn)
        Dim SampleSource As New List(Of CascadingDropDownNameValue)
        Try
            If dbCon.State = ConnectionState.Closed Then
                dbCon.Open()
            End If

            Dim sql As String = String.Empty
            sql = " EXEC CombosProductos " &
                  "@opcion = 7," &
                  "@codigo = null "

            Dim cmd As New OleDb.OleDbCommand(sql, dbCon)
            Dim Reader As OleDb.OleDbDataReader = cmd.ExecuteReader()

            Do While Reader.Read()
                Dim CategoryName = Reader("Usuario").ToString()
                Dim CategoryValue = Reader("codUsuario").ToString()
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

    <WebMethod()>
    Public Function GetRol(ByVal knownCategoryValues As String, ByVal category As String) As CascadingDropDownNameValue()

        Dim dbCon As New OleDbConnection(conn.conn)
        Dim SampleSource As New List(Of CascadingDropDownNameValue)

        Try
            If dbCon.State = ConnectionState.Closed Then
                dbCon.Open()
            End If

            Dim sql As String = " EXEC CombosProductos " &
                                "@opcion = 24," &
                                "@codigo = NULL"

            Dim cmd As New OleDbCommand(sql, dbCon)
            Dim Reader As OleDbDataReader = cmd.ExecuteReader()

            Do While Reader.Read()
                Dim CategoryName = Reader("Rol").ToString()
                Dim CategoryValue = Reader("codRol").ToString()
                SampleSource.Add(New CascadingDropDownNameValue(CategoryName, CategoryValue))
            Loop

            Return SampleSource.ToArray()

        Catch ex As Exception
            conn.PmsgBox(ex.Message, "ERROR")
        Finally
            If dbCon.State = ConnectionState.Open Then
                dbCon.Close()
            End If
        End Try

    End Function

    <WebMethod()>
    Public Function GetVendedor(ByVal knownCategoryValues As String,
                              ByVal category As String) As CascadingDropDownNameValue()

        Dim dbCon As New System.Data.OleDb.OleDbConnection(conn.conn)
        Dim SampleSource As New List(Of CascadingDropDownNameValue)
        Try
            If dbCon.State = ConnectionState.Closed Then
                dbCon.Open()
            End If

            Dim sql As String = String.Empty
            sql = " EXEC CombosProductos " &
                  "@opcion = 10," &
                  "@codigo = null "

            Dim cmd As New OleDb.OleDbCommand(sql, dbCon)
            Dim Reader As OleDb.OleDbDataReader = cmd.ExecuteReader

            Do While Reader.Read
                Dim CategoryName = Reader("Vendedor").ToString()
                Dim CategoryValue = Reader("codVendedor").ToString()
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

    <WebMethod()>
    Public Function GetUnMedida(ByVal knownCategoryValues As String,
                              ByVal category As String) As CascadingDropDownNameValue()

        Dim dbCon As New System.Data.OleDb.OleDbConnection(conn.conn)
        Dim SampleSource As New List(Of CascadingDropDownNameValue)
        Try
            If dbCon.State = ConnectionState.Closed Then
                dbCon.Open()
            End If

            Dim sql As String = String.Empty
            sql = " EXEC CombosProductos " &
                  "@opcion = 11," &
                  "@codigo = null "

            Dim cmd As New OleDb.OleDbCommand(sql, dbCon)
            Dim Reader As OleDb.OleDbDataReader = cmd.ExecuteReader

            Do While Reader.Read
                Dim CategoryName = Reader("UnidadMedida").ToString()
                Dim CategoryValue = Reader("cod_und_medida").ToString()
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

    <WebMethod()>
    Public Function GetProductos(ByVal knownCategoryValues As String,
                              ByVal category As String) As CascadingDropDownNameValue()

        Dim dbCon As New System.Data.OleDb.OleDbConnection(conn.conn)
        Dim SampleSource As New List(Of CascadingDropDownNameValue)
        Try
            If dbCon.State = ConnectionState.Closed Then
                dbCon.Open()
            End If

            Dim vCPais As String = Context.Request.Cookies("CKSMFACTURA")("codPais")
            Dim sql As String = String.Empty
            sql = " EXEC CombosProductos " &
                  "@opcion = 17," &
                  "@codigo = " & vCPais & " "

            Dim cmd As New OleDb.OleDbCommand(sql, dbCon)
            Dim Reader As OleDb.OleDbDataReader = cmd.ExecuteReader

            Do While Reader.Read
                Dim CategoryName = Reader("Producto").ToString()
                Dim CategoryValue = Reader("codProduto").ToString()
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

    <WebMethod()>
    Public Function GetMercado(ByVal knownCategoryValues As String,
                              ByVal category As String) As CascadingDropDownNameValue()

        Dim dbCon As New System.Data.OleDb.OleDbConnection(conn.conn)
        Dim SampleSource As New List(Of CascadingDropDownNameValue)
        Try
            If dbCon.State = ConnectionState.Closed Then
                dbCon.Open()
            End If

            Dim vCPais As String = Context.Request.Cookies("CKSMFACTURA")("codPais")
            Dim sql As String = String.Empty
            sql = " EXEC CombosProductos " &
                  "@opcion = 13," &
                  "@codigo = " & vCPais & " "


            Dim cmd As New OleDb.OleDbCommand(sql, dbCon)
            Dim Reader As OleDb.OleDbDataReader = cmd.ExecuteReader

            Do While Reader.Read
                Dim CategoryName = Reader("Mercado").ToString()
                Dim CategoryValue = Reader("codigoSecMerc").ToString()
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

    <WebMethod()>
    Public Function GetMoneda(ByVal knownCategoryValues As String,
                              ByVal category As String) As CascadingDropDownNameValue()

        Dim dbCon As New System.Data.OleDb.OleDbConnection(conn.conn)
        Dim SampleSource As New List(Of CascadingDropDownNameValue)
        Try
            If dbCon.State = ConnectionState.Closed Then
                dbCon.Open()
            End If

            Dim vCPais As String = Context.Request.Cookies("CKSMFACTURA")("codPais")
            Dim sql As String = String.Empty
            sql = " EXEC CombosProductos " &
                  "@opcion = 15," &
                  "@codigo = " & vCPais & " "


            Dim cmd As New OleDb.OleDbCommand(sql, dbCon)
            Dim Reader As OleDb.OleDbDataReader = cmd.ExecuteReader

            Do While Reader.Read
                Dim CategoryName = Reader("Moneda").ToString()
                Dim CategoryValue = Reader("codMoneda").ToString()
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

    <WebMethod()>
    Public Function GetTarjeta(ByVal knownCategoryValues As String,
                              ByVal category As String) As CascadingDropDownNameValue()

        Dim dbCon As New System.Data.OleDb.OleDbConnection(conn.conn)
        Dim SampleSource As New List(Of CascadingDropDownNameValue)
        Try
            If dbCon.State = ConnectionState.Closed Then
                dbCon.Open()
            End If

            Dim vCPais As String = Context.Request.Cookies("CKSMFACTURA")("codPais")
            Dim sql As String = String.Empty
            sql = " EXEC CombosProductos " &
                  "@opcion = 22," &
                  "@codigo = " & vCPais & " "


            Dim cmd As New OleDb.OleDbCommand(sql, dbCon)
            Dim Reader As OleDb.OleDbDataReader = cmd.ExecuteReader

            Do While Reader.Read
                Dim CategoryName = Reader("Tarjeta").ToString()
                Dim CategoryValue = Reader("codTarjeta").ToString()
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
    <WebMethod()>
    Public Function GetFormaPago(ByVal knownCategoryValues As String,
                              ByVal category As String) As CascadingDropDownNameValue()

        Dim dbCon As New System.Data.OleDb.OleDbConnection(conn.conn)
        Dim SampleSource As New List(Of CascadingDropDownNameValue)
        Try
            If dbCon.State = ConnectionState.Closed Then
                dbCon.Open()
            End If

            Dim vCPais As String = Context.Request.Cookies("CKSMFACTURA")("codPais")
            Dim sql As String = String.Empty
            sql = " EXEC CombosProductos " &
                  "@opcion = 21," &
                  "@codigo = " & vCPais & " "


            Dim cmd As New OleDb.OleDbCommand(sql, dbCon)
            Dim Reader As OleDb.OleDbDataReader = cmd.ExecuteReader

            Do While Reader.Read
                Dim CategoryName = Reader("FormaPago").ToString()
                Dim CategoryValue = Reader("codFormaPago").ToString()
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
    <WebMethod()>
    Public Function GetBanco(ByVal knownCategoryValues As String, ByVal category As String) As CascadingDropDownNameValue()
        Dim SampleSource As New List(Of CascadingDropDownNameValue)

        Try
            Using dbCon As New System.Data.OleDb.OleDbConnection(conn.conn)
                dbCon.Open()
                Dim vCPais As String = Context.Request.Cookies("CKSMFACTURA")("codPais")

                Dim sql As String = "EXEC CombosProductos " &
                                "@opcion = 14, " &
                                "@codigo = " & vCPais & " "

                Dim cmd As New OleDb.OleDbCommand(sql, dbCon)


                Dim Reader As OleDb.OleDbDataReader = cmd.ExecuteReader

                Do While Reader.Read
                    Dim CategoryName = Reader("Banco").ToString()
                    Dim CategoryValue = Reader("codBanco").ToString()
                    SampleSource.Add(New CascadingDropDownNameValue(CategoryName, CategoryValue))
                Loop
            End Using

            Return SampleSource.ToArray()
        Catch ex As Exception
            ' Handle or log the exception here
            Throw ex
        End Try
    End Function

    '<WebMethod()>
    'Public Function GetBanco(knownCategoryValues As String, category As String) As CascadingDropDownNameValue()

    '    Dim vCPais As String = Context.Request.Cookies("CKSMFACTURA")("codPais")
    '    Dim query As String = "EXEC CombosProductos @opcion = 14, @codigo = " & vCPais
    '    Dim bancos As List(Of CascadingDropDownNameValue) = GetData(query)
    '    Return bancos.ToArray()

    'End Function

    'Public Function GetCuentaBanco(knownCategoryValues As String, category As String) As CascadingDropDownNameValue()

    '    Dim vCPais As String = Context.Request.Cookies("CKSMFACTURA")("codPais")
    '    Dim Param As StringDictionary = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)
    '    Dim query As String = " EXEC ComboCuentasBanco " &
    '            "@opcion = 1," &
    '            "@codigo = " & vCPais & ", " &
    '            "@codigo_banco = " & Param("CategoryBanco") & " "

    '    Dim cuentas As List(Of CascadingDropDownNameValue) = GetData(query)
    '    Return cuentas.ToArray()

    'End Function

    <WebMethod()>
    Public Function GetData(query As String) As List(Of CascadingDropDownNameValue)

        Dim values As New List(Of CascadingDropDownNameValue)

        Try
            Using dbCon As New OleDbConnection(conn.conn)

                Dim cmd As New OleDbCommand(query, dbCon)

                dbCon.Open()

                Using reader As OleDbDataReader = cmd.ExecuteReader()

                    While reader.Read()

                        Dim CategoryName = reader(1).ToString()
                        Dim CategoryValue = reader(0).ToString()
                        values.Add(New CascadingDropDownNameValue(CategoryName, CategoryValue) With {
                         .name = reader(1).ToString(),
                         .value = reader(0).ToString()
                        })
                    End While

                    reader.Close()
                    dbCon.Close()
                    Return values
                End Using
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    <WebMethod()>
    Public Function GetCuentaBanco(knownCategoryValues As String) As CascadingDropDownNameValue()

        Dim dbCon As New System.Data.OleDb.OleDbConnection(conn.conn)
        Dim Param As StringDictionary = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)
        Dim SampleSource As New List(Of CascadingDropDownNameValue)

        Try
            If dbCon.State = ConnectionState.Closed Then
                dbCon.Open()
            End If

            Dim vCPais As String = Context.Request.Cookies("CKSMFACTURA")("codPais")

            Dim sql As String = " EXEC ComboCuentasBanco " &
                "@opcion = 1," &
                "@codigo = " & vCPais & ", " &
                "@codigo_banco = " & Param("CategoryBanco") & " "

            Dim cmd As New OleDb.OleDbCommand(sql, dbCon)
            Dim Reader As OleDb.OleDbDataReader = cmd.ExecuteReader


            Do While Reader.Read
                Dim CategoryName = Reader("Cuenta").ToString()
                Dim CategoryValue = Reader("codBancoCuenta").ToString()
                SampleSource.Add(New CascadingDropDownNameValue(
                                 CategoryName, CategoryValue))
            Loop

            Return SampleSource.ToArray()


        Catch ex As Exception
            Throw ex

        End Try

    End Function

    <WebMethod()>
    Public Function GetCliente(knownCategoryValues As String, category As String) As CascadingDropDownNameValue()
        Try
            Using dbCon As New OleDbConnection(conn.conn)
                Dim SampleSource As New List(Of CascadingDropDownNameValue)

                Dim sql = " EXEC CombosProductos " &
                  "@opcion = 16," &
                  "@codigo = " & Session("CodigoPais") & " "

                Using cmd As New OleDbCommand(sql, dbCon)
                    Using Reader As OleDbDataReader = cmd.ExecuteReader()
                        Do While Reader.Read
                            Dim CategoryName = Reader("cliente").ToString()
                            Dim CategoryValue = Reader("codcliente").ToString()
                            SampleSource.Add(New CascadingDropDownNameValue(CategoryName, CategoryValue))
                        Loop
                        Return SampleSource.ToArray()
                    End Using
                End Using
            End Using

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    '<WebMethod()> _
    'Public Function GetEmpresaXPais(ByVal knownCategoryValues As String, _
    '                          ByVal category As String) As CascadingDropDownNameValue()

    '    Dim dbCon As New System.Data.OleDb.OleDbConnection(conn.conn)
    '    Dim SampleSource As New List(Of CascadingDropDownNameValue)
    '    Try
    '        If dbCon.State = ConnectionState.Closed Then
    '            dbCon.Open()
    '        End If

    '        Dim vUser As String = Context.Request.Cookies("CKSMFACTURA")("cod_usuario")
    '        ''CType(Session.Item("cod_usuario"), String)
    '        'CKSMFACTURA("cod_usuario"). hay otro por usuario
    '        Dim vCPais As String = Context.Request.Cookies("CKSMFACTURA")("codPais")
    '        Dim sql As String = String.Empty
    '        sql = " EXEC CombosProductos " & _
    '              "@opcion = 18," & _
    '              "@codigo = " & vCPais & " "


    '        Dim cmd As New OleDb.OleDbCommand(sql, dbCon)
    '        Dim Reader As OleDb.OleDbDataReader = cmd.ExecuteReader

    '        Do While Reader.Read
    '            Dim CategoryName = Reader("Empresa").ToString()
    '            Dim CategoryValue = Reader("codEmpresa").ToString()
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


    <WebMethod()>
    Public Function GetPaisesAccesoUsuario(knownCategoryValues As String, category As String) As CascadingDropDownNameValue()

        Dim Param As StringDictionary = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)
        Dim SampleSource As New List(Of CascadingDropDownNameValue)

        Try

            Using dbCon As New OleDbConnection(conn.conn)
                dbCon.Open()

                Dim User = Context.Request.Cookies("CKSMFACTURA")("CodigoUser")

                Dim sql = $"SELECT * FROM GetPaisesAccesoUsuario({User})"

                Using cmd As New OleDbCommand(sql, dbCon)
                    Using dr As OleDbDataReader = cmd.ExecuteReader()
                        Do While dr.Read
                            Dim CategoryName = dr("Descripcion").ToString()
                            Dim CategoryValue = dr("CodigoPais").ToString()
                            SampleSource.Add(New CascadingDropDownNameValue(CategoryName, CategoryValue))
                        Loop
                    End Using
                End Using
            End Using

            Return SampleSource.ToArray()
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    <WebMethod()>
    Public Function GetEmpresasAccesoUsuario(knownCategoryValues As String, category As String) As CascadingDropDownNameValue()

        Dim Param As StringDictionary = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)
        Dim SampleSource As New List(Of CascadingDropDownNameValue)

        Try
            Using dbCon As New OleDbConnection(conn.conn)
                dbCon.Open()


                Dim User = Context.Request.Cookies("CKSMFACTURA")("CodigoUser")

                Dim Pais = If(String.IsNullOrEmpty(Param("CategoryPais")), Context.Request.Cookies("CKSMFACTURA")("CodigoPais").ToString(), Param("CategoryPais"))


                Dim sql = $"SELECT * FROM GetEmpresasAccesoUsuario({User}, {Pais})"

                Using cmd As New OleDbCommand(sql, dbCon)
                    Using dr As OleDbDataReader = cmd.ExecuteReader()

                        Do While dr.Read
                            Dim CategoryName = dr("Descripcion").ToString()
                            Dim CategoryValue = dr("CodigoEmpresa").ToString()
                            SampleSource.Add(New CascadingDropDownNameValue(CategoryName, CategoryValue))
                        Loop

                        Return SampleSource.ToArray()
                    End Using
                End Using
            End Using

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    <WebMethod()>
    Public Function GetPuestosAccesoUsuario(ByVal knownCategoryValues As String,
                                    ByVal category As String) As CascadingDropDownNameValue()

        Dim Param As StringDictionary = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)
        Dim SampleSource As New List(Of CascadingDropDownNameValue)

        Try

            Using dbCon As New OleDbConnection(conn.conn)
                dbCon.Open()

                Dim User = Context.Request.Cookies("CKSMFACTURA")("CodigoUser")

                Dim Empresa = If(String.IsNullOrEmpty(Param("CategoryEmpresa")), Context.Request.Cookies("CKSMFACTURA")("CodigoEmpresa").ToString(), Param("CategoryEmpresa"))

                'If Param("CategoryEmpresa") = String.Empty Then
                '    vCEmpresa = Context.Request.Cookies("CKSMFACTURA")("codPais")
                'Else
                'Param("CategoryEmpresa")
                'End If

                Dim sql = $"SELECT * FROM GetPuestosAccesoUsuario({User}, {Empresa})"

                Using cmd As New OleDbCommand(sql, dbCon)
                    Using dr As OleDbDataReader = cmd.ExecuteReader()
                        Do While dr.Read
                            Dim CategoryName = dr("Descripcion").ToString()
                            Dim CategoryValue = dr("CodigoPuesto").ToString()
                            SampleSource.Add(New CascadingDropDownNameValue(CategoryName, CategoryValue))
                        Loop

                        Return SampleSource.ToArray()

                    End Using
                End Using

            End Using
        Catch ex As Exception
            Throw ex

        End Try
    End Function
End Class