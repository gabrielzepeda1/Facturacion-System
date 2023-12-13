Imports System.Data.SqlClient
Imports FACTURACION_CLASS.Seguridad

Public Class DatabaseHelper

    Public Shared Function ExecuteCatSiglas(ByVal opcion As Integer, ByVal siglas As String, ByVal codusuario As Integer, ByVal codusuarioUlt As Integer, ByVal busquedad As String) As DataTable

        Dim dataTable As New DataTable()

        Using connection As New SqlConnection(ConnectionString)

            Using command As New SqlCommand("Cat_Siglas", connection)

                command.CommandType = CommandType.StoredProcedure

                ' Add parameters to the stored procedure
                command.Parameters.Add("@opcion", SqlDbType.Int).Value = opcion
                command.Parameters.Add("@siglas", SqlDbType.VarChar, 20).Value = siglas
                command.Parameters.Add("@codusuario", SqlDbType.Int).Value = codusuario
                command.Parameters.Add("@codusuarioUlt", SqlDbType.Int).Value = codusuarioUlt
                command.Parameters.Add("@BUSQUEDAD", SqlDbType.VarChar, 200).Value = busquedad

                connection.Open()
                Using adapter As New SqlDataAdapter(command)
                    adapter.Fill(dataTable)
                End Using

            End Using
        End Using


        Return dataTable



    End Function






End Class
