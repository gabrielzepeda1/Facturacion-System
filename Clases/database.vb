Imports System
Imports System.Collections
Imports System.Net
Imports System.Net.Mail
Imports System.Net.Mime
Imports System.Data
Imports System.Net.Security
Imports System.Security.Cryptography.X509Certificates
Imports System.IO
Imports System.Data.OleDb

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
    ''' DEVUELVE UN CONTROL DATASET
    ''' </summary>
    ''' <param name="Query">SENTENCIA SQL QUE SERA ENVIADA A SQL SERVER</param>
    ''' <returns>DATASET</returns>
    ''' <remarks></remarks>
    Public Function GetDataSet(ByVal Query As String) As DataSet

        Dim cnn As String = conn.Conn

        Dim dbCon As New System.Data.OleDb.OleDbConnection(cnn)

        Try
            If dbCon.State = ConnectionState.Closed Then
                dbCon.Open()
            End If

            Dim sql As String = "SET DATEFORMAT DMY "

            sql = sql & vbCrLf & Query

            'Dim cmd As New SqlClient.SqlCommand(sql, dbCon)
            'cmd.CommandType = CommandType.StoredProcedure

            Dim cmd As New System.Data.OleDb.OleDbCommand(sql, dbCon)
            'cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandTimeout = 9999

            Dim da As New System.Data.OleDb.OleDbDataAdapter(cmd)
            Dim ds As New DataSet()
            da.Fill(ds, "DT0")

            Return ds

            ds.Dispose()

        Catch ex As Exception
            Throw New Exception(ex.Message)
            Return Nothing

        Finally
            If dbCon.State = ConnectionState.Open Then
                dbCon.Close()
            End If

        End Try
    End Function
    ''' <summary>
    ''' DEVUELVE UN CONTROL DATATABLE
    ''' </summary>
    ''' <param name="Query">SENTENCIA SQL QUE SERA ENVIADA A SQL SERVER</param>
    ''' <returns>DATATABLE</returns>
    ''' <remarks></remarks>
    Public Function GetDateTableProcedimiento(ByVal Query As String) As DataTable
        Dim cnn As String = conn.Conn
        Dim dbCon As New System.Data.OleDb.OleDbConnection(cnn)
        Try
            If dbCon.State = ConnectionState.Closed Then
                dbCon.Open()
            End If

            Dim sql As String = "SET DATEFORMAT DMY "

            sql = sql & vbCrLf & Query

            'Dim daSrc As New System.Data.OleDb.OleDbDataAdapter(sql, dbCon)
            'Dim dt As New DataTable("Factura")
            'daSrc.Fill(dt)

            'Return dt
            'dt.Dispose()
            'aqui 23/07/2021 
            Dim cmd As New System.Data.OleDb.OleDbCommand(sql, dbCon)
            cmd.CommandTimeout = 9999

            Dim da As New System.Data.OleDb.OleDbDataAdapter(cmd)
            Dim ds As New DataSet()
            da.Fill(ds, "DT0")

            Return ds.Tables(0)

            ds.Dispose()
            'fin 23/07/2021
        Catch ex As Exception
            Throw New Exception(ex.Message)
            Return Nothing

        Finally
            If dbCon.State = ConnectionState.Open Then
                dbCon.Close()
            End If

        End Try
    End Function
    ''' <summary>
    ''' DEVUELVE UN CONTROL DATATABLE
    ''' </summary>
    ''' <param name="Query">SENTENCIA SQL QUE SERA ENVIADA A SQL SERVER</param>
    ''' <returns>DATATABLE</returns>
    ''' <remarks></remarks>
    Public Function GetDateTable(ByVal Query As String) As DataTable
        Dim cnn As String = conn.Conn
        Dim dbCon As New System.Data.OleDb.OleDbConnection(cnn)
        Try
            If dbCon.State = ConnectionState.Closed Then
                dbCon.Open()
            End If

            Dim sql As String = "SET DATEFORMAT DMY "

            sql = sql & vbCrLf & Query

            Dim daSrc As New System.Data.OleDb.OleDbDataAdapter(sql, dbCon)
            Dim dt As New DataTable("Factura")
            daSrc.Fill(dt)

            Return dt
            dt.Dispose()
            'aqui 23/07/2021 
            'Dim cmd As New System.Data.OleDb.OleDbCommand(sql, dbCon)
            'cmd.CommandTimeout = 9999

            'Dim da As New System.Data.OleDb.OleDbDataAdapter(cmd)
            'Dim ds As New DataSet()
            'da.Fill(ds, "DT0")
            'Return ds.Tables(0)
            'ds.Dispose()
            'fin 23/07/2021
        Catch ex As Exception
            Throw New Exception(ex.Message)
            Return Nothing

        Finally
            If dbCon.State = ConnectionState.Open Then
                dbCon.Close()
            End If

        End Try
    End Function

    ''' <summary>
    ''' DEVUELVE UN CONTROL DATAREADER
    ''' </summary>
    ''' <param name="Query">SENTENCIA SQL QUE SERA ENVIADA A SQL SERVER</param>
    ''' <returns>DATAREADER</returns>
    ''' <remarks></remarks>
    Public Function GetDataReader(ByVal Query As String) As System.Data.OleDb.OleDbDataReader
        Dim cnn As String = conn.Conn
        Dim dbCon As New System.Data.OleDb.OleDbConnection(cnn)

        Try
            If dbCon.State = ConnectionState.Closed Then
                dbCon.Open()
            End If

            Dim sql As String = "SET DATEFORMAT DMY"

            sql = sql & vbCrLf & Query

            Dim cmd As New System.Data.OleDb.OleDbCommand(sql, dbCon)
            cmd.CommandTimeout = 9999

            Dim dr As System.Data.OleDb.OleDbDataReader
            dr = cmd.ExecuteReader

            Return dr

            dr.Close()

        Catch ex As System.Data.OleDb.OleDbException
            Console.WriteLine("Error en OleDB")

            Return Nothing

        Catch ex As Exception
            Throw New Exception(ex.Message)
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
        Dim cnn As String = conn.Conn
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
        Dim cnn As String = conn.Conn
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

End Class
