Imports System.IO
Imports System.Data.OleDb
Imports AjaxControlToolkit

Public Class database
    Dim conn As New seguridad

    ''' <summary>
    ''' CREA UNA COPIA DEL ARCHIVO QUE SERA ADJUNTO EN MEMORIA, Y LUEGO BORRA DICHA COPIA
    ''' ESTO EVITA EL ERROR DE USO DEL SERVIDOR
    ''' </summary>
    ''' <param name="filePath"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetStreamFile(ByVal filePath As String) As Stream

        Using fileStream As FileStream = File.OpenRead(filePath)
            Dim memStream As MemoryStream = New MemoryStream()

            memStream.SetLength(fileStream.Length)

            fileStream.Read(memStream.GetBuffer(), 0, CInt(fileStream.Length))

            Return memStream
        End Using

    End Function
    ''' <summary>
    ''' Retorna un DataSet
    ''' </summary>
    ''' <param name="query">SENTENCIA SQL QUE SERA ENVIADA A SQL SERVER</param>
    ''' <returns>DataSet</returns>
    ''' <remarks></remarks>
    Public Function GetDataSet(query As String) As DataSet

        Try
            Using dbCon As New OleDbConnection(conn.conn)

                Dim sql = "SET DATEFORMAT DMY "
                sql = sql & vbCrLf & query

                Dim cmd As New OleDbCommand(sql, dbCon)
                cmd.CommandTimeout = 9999

                Dim da As New OleDbDataAdapter(cmd)
                Dim ds As New DataSet()
                da.Fill(ds, "DT0")

                Return ds

            End Using

        Catch ex As Exception
            conn.PmsgBox("Error en clase de database: " & ex.Message, "error")
        End Try
    End Function
    ''' <summary>
    ''' DEVUELVE UN CONTROL DATATABLE
    ''' </summary>
    ''' <param name="query">SENTENCIA SQL QUE SERA ENVIADA A SQL SERVER</param>
    ''' <returns>DataTable</returns>
    ''' <remarks></remarks>
    Public Function GetDataTableProc(query As String) As DataTable

        Try
            Using dbCon As New OleDbConnection(conn.conn)

                Dim sql = "SET DATEFORMAT DMY "

                sql = sql & vbCrLf & query

                'Dim daSrc As New System.Data.OleDb.OleDbDataAdapter(sql, dbCon)
                'Dim dt As New DataTable("Factura")
                'daSrc.Fill(dt)

                'Return dt
                'dt.Dispose()

                Dim cmd As New OleDbCommand(sql, dbCon)
                cmd.CommandTimeout = 9999

                Dim da As New OleDbDataAdapter(cmd)
                Dim ds As New DataSet()
                da.Fill(ds, "DT0")

                Return ds.Tables(0)
                ds.Dispose()

            End Using

        Catch ex As Exception
            Throw New Exception(ex.Message)

        End Try
    End Function
    ''' <summary>
    ''' Retorna un DataTable
    ''' </summary>
    ''' <param name="query"></param>
    ''' <returns>DataTable</returns>
    ''' <remarks></remarks>
    Public Function GetDataTable(query As String) As DataTable
        Dim cnn As String = conn.conn
        Dim dbCon As New System.Data.OleDb.OleDbConnection(cnn)
        Try
            If dbCon.State = ConnectionState.Closed Then
                dbCon.Open()
            End If

            Dim sql As String = "SET DATEFORMAT DMY "

            sql = sql & vbCrLf & query

            Dim daSrc As New System.Data.OleDb.OleDbDataAdapter(sql, dbCon)
            Dim dt As New DataTable("Factura")
            daSrc.Fill(dt)

            Return dt
            dt.Dispose()

        Catch ex As Exception
            Throw New Exception(ex.Message)

        End Try
    End Function

    ''' <summary>Esta función ejecuta un query de sql y retorna un OleDbDataReader</summary>
    ''' <param name="sql">sql query</param>
    ''' <returns>OleDbDataReader</returns>
    ''' <remarks></remarks>
    Public Function GetDataReader(sql As String) As OleDbDataReader
        Try
            Using dbCon As New OleDbConnection(conn.conn)
                dbCon.Open()

                Using cmd As New OleDbCommand(sql, dbCon)
                    cmd.CommandTimeout = 9999
                    Dim dr As OleDbDataReader = cmd.ExecuteReader()

                    Return dr
                    dr.Close()
                End Using
            End Using
        Catch ex As OleDbException
            Console.WriteLine("Error en OleDB")

            Return Nothing

        End Try
    End Function

    ''' <summary>
    ''' DEVUELVE UN VALOR BOOLEANO QUE ES OBTENIDO ATRAVEZ DE UN DATAREADER. SI EL VALOR OBTENIDO ES "SI" DEVUELVE TRUE EN CASO CONTRARIO DEVUELVE FALSE
    ''' </summary>
    ''' <param name="Query">Procedimiento de sql con todos los paramentros. NO PASAR DATAFORMAT</param>
    ''' <param name="campo">NOMBRE DEL CAMPO QUE SERA FILTRADO EN EL DATAREADER</param>
    ''' <returns>BOOLEAN</returns>
    ''' <remarks></remarks>
    Public Function GetBoleano(ByVal Query As String, ByVal campo As String) As Boolean
        Dim cnn As String = conn.conn
        Dim dbCon As New System.Data.OleDb.OleDbConnection(cnn)
        Try
            If dbCon.State = ConnectionState.Closed Then
                dbCon.Open()
            End If

            Dim sql As String = "SET DATEFORMAT DMY "

            sql = sql + vbCrLf + Query

            Dim cmd As New System.Data.OleDb.OleDbCommand(sql, dbCon)
            Dim dr As System.Data.OleDb.OleDbDataReader
            dr = cmd.ExecuteReader

            If dr.Read() Then
                If dr.Item(campo).ToString() = "Si" Then
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If

            dr.Close()

        Catch ex As Exception
            Throw New Exception(ex.Message)
            Return False

        End Try
    End Function

    ''' <summary>
    ''' DEVUELVE UN VALOR STRING QUE ES OBTENIDO ATRAVEZ DE UN DATAREADER
    ''' </summary>
    ''' <param name="Query">Procedimiento de sql con todos los paramentros. NO PASAR DATAFORMAT</param>
    ''' <param name="campo">NOMBRE DEL CAMPO QUE SERA FILTRADO EN EL DATAREADER</param>
    ''' <returns>DATAREADER</returns>
    ''' <remarks></remarks>
    Public Function GetString(ByVal Query As String, ByVal campo As String) As String
        Dim cnn As String = conn.conn
        Dim dbCon As New System.Data.OleDb.OleDbConnection(cnn)
        Try
            If dbCon.State = ConnectionState.Closed Then
                dbCon.Open()
            End If

            Dim sql As String = "SET DATEFORMAT DMY "

            sql = sql + vbCrLf + Query

            Dim cmd As New System.Data.OleDb.OleDbCommand(sql, dbCon)
            Dim dr As System.Data.OleDb.OleDbDataReader
            dr = cmd.ExecuteReader

            If dr.Read() Then
                Return dr.Item(campo).ToString()
            Else
                Return String.Empty
            End If

            dr.Close()

        Catch ex As Exception
            Throw New Exception(ex.Message)
            Return String.Empty

        End Try
    End Function

    Public Function GetPaisesUsuario(CodigoUser As Integer) As List(Of Integer)

        Dim sql = $"SELECT * FROM GetDistinctPaisesUsuario({CodigoUser})"
        Dim reader = GetDataReader(sql)
        Dim ArrCodigoPais As New List(Of Integer)

        Do While reader.Read()
            ArrCodigoPais.Add(reader("cod_pais"))
        Loop

        Debug.WriteLine(ArrCodigoPais)

        Return ArrCodigoPais

    End Function

    Public Function GetEmpresasUsuario(CodigoUser As Integer) As List(Of Integer)

        Dim sql = $"SELECT * FROM GetDistinctEmpresasUsuario({CodigoUser})"
        Dim reader = GetDataReader(sql)
        Dim ArrCodigoEmpresa As New List(Of Integer)

        Do While reader.Read()
            ArrCodigoEmpresa.Add(reader("cod_empresa"))
        Loop

        Debug.WriteLine(ArrCodigoEmpresa)
        Return ArrCodigoEmpresa
    End Function

    Public Function GetPuestosUsuario(CodigoUser As Integer) As List(Of Integer)

        Dim sql = $"SELECT * FROM GetDistinctPuestosUsuario({CodigoUser})"
        Dim reader = GetDataReader(sql)
        Dim ArrCodigoPuesto As New List(Of Integer)

        Do While reader.Read()
            ArrCodigoPuesto.Add(reader("cod_empresa"))
        Loop

        Debug.WriteLine(ArrCodigoPuesto)
        Return ArrCodigoPuesto
    End Function

    'Public Function GetDropdown(sql As String, knownCategoryValues As String, category As String) As CascadingDropDown
    '    Using dbCon As New OleDbConnection(conn.conn)
    '        dbCon.Open()

    '        Using cmd As New OleDbCommand(sql, dbCon)
    '            cmd.CommandTimeout = 9999
    '            cmd.CommandType = CommandType.StoredProcedure
    '            Dim dr As OleDbDataReader = cmd.ExecuteReader()

    '            Dim values As New List(Of String)
    '            Dim names As New List(Of String)

    '            While dr.Read()
    '                values.Add(dr.Item("Codigo").ToString())
    '                names.Add(dr.Item("Nombre").ToString())
    '            End While

    '            Dim valuesArray As String() = values.ToArray()
    '            Dim namesArray As String() = names.ToArray()

    '            Return New CascadingDropDown(valuesArray, namesArray)
    '        End Using
    '    End Using
    'End Function
End Class
