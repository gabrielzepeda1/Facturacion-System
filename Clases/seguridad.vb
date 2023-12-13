Imports System.Data.OleDb
Imports System.Net
Imports System.Net.Dns
Imports System.Security.Principal

Public Class seguridad

#Region "PROPIEDADES DE LA CLASE"

    Dim _conn As String = String.Empty
    Dim _key As String = String.Empty

    Public Shared ReadOnly ConnectionString As String = "Data Source=localhost;Server=GILDEDSWORD\SQLEXPRESS;Database=Facturacion;UID=sa;PWD=ga123"

    Public Property conn_Procedimientos() As String
        Get
            _conn = "Data Source=localhost;Server=GILDEDSWORD\SQLEXPRESS;Database=Facturacion;UID=sa;PWD=ga123"
            Return _conn
        End Get
        Set(ByVal value As String)
            _conn = value
        End Set
    End Property

    Public Property conn() As String
        Get
            _conn = "Provider=SQLOLEDB.1;Server=GILDEDSWORD\SQLEXPRESS;Database=Facturacion;UID=sa;PWD=ga123"
            Return _conn
        End Get
        Set(ByVal value As String)
            _conn = value
        End Set
    End Property

    Public Property CodigoSesion() As String
        Get
            Return _CodigoSesion
        End Get
        Set(ByVal value As String)
            _CodigoSesion = value
        End Set
    End Property

    Public Property CodigoUsuario() As String
        Get
            Return _CodigoUsuario
        End Get
        Set(ByVal value As String)
            _CodigoUsuario = value
        End Set
    End Property

    Public Property Nombre_Usuario() As String
        Get
            Return _User
        End Get
        Set(ByVal value As String)
            _User = value
        End Set
    End Property

    Public Property Persona() As String
        Get
            Return _Persona
        End Get
        Set(ByVal value As String)
            _Persona = value
        End Set
    End Property

    Public Property Password() As String
        Get
            Return _Pass
        End Get
        Set(ByVal value As String)
            _Pass = value
        End Set
    End Property

    Public Property Key() As String
        Get
            _key = "SMARTIN2019"

            Return _key
        End Get
        Set(ByVal value As String)
            _key = value
        End Set
    End Property

#End Region

    ''' <summary>Returns Alert-Success-Error Message.</summary>
    ''' <param name="Mensaje">Message</param>
    ''' <param name="Tipo">Message Type: [info, exito, alerta, error]</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function pmsgBox(ByVal Mensaje As String, ByVal Tipo As String) As String
        Select Case Tipo
            Case "info"
                Return "<div class='info'>" & Mensaje & "</div>"

            Case "exito"
                Return "<div class='exito'>" & Mensaje & "</div>"

            Case "alerta"
                Return "<div class='alerta'>" & Mensaje & "</div>"

            Case "error"
                Return "<div class='error'>" & Mensaje & "</div>"

            Case Else
                Return "<div class='info'>" & Mensaje & "</div>"
        End Select
    End Function

#Region "OBTENER DATOS DE LA MAQUINA CLIENTE"

    'Obtener la direccion IP del PC del usuario.
    Public Function IpHost() As String

        Dim nombrePc As String
        Dim entradasIp As IPHostEntry

        nombrePc = GetHostName()
        entradasIp = GetHostEntry(nombrePc)

        Dim direccionIp As String = entradasIp.AddressList(0).ToString

        Return direccionIp
    End Function

    ' Nombre del PC del usuario logueado
    Public Function NameHost() As String
        Return GetHostName()
    End Function

    ' Retorna el nombre de la maquina cliente que inicia sesion.
    Public Function UserHost() As String
        Return WindowsIdentity.GetCurrent().Name
    End Function

#End Region

#Region "LOGIN"

    '''<summary>Ejecuta el stored procedure "sp_sys_login" en SQL SERVER, si devuelve TRUE, el usuario esta registrado y puede iniciar sesión.</summary>
    '''<param name="usuario">Nombre de Usuario</param>
    '''<param name="password">Password</param>
    '''<param name="IpConexion">Direccion Ip del Usuario</param>
    ''' <param name="Nombre_Host">Nombre del Host</param>
    Public Function IniciarSesion(usuario As String, password As String, IpConexion As String, Nombre_Host As String) As Dictionary(Of String, Object)

        Dim userData As New Dictionary(Of String, Object)

        Try
            Using dbCon As New OleDbConnection(conn)
                dbCon.Open()

                Dim cmd As New OleDbCommand("sp_sys_login", dbCon)
                cmd.Parameters.AddWithValue("@Username", usuario)
                cmd.Parameters.AddWithValue("@Password", password)
                cmd.Parameters.AddWithValue("@IpConexion", IpConexion)
                cmd.Parameters.AddWithValue("@Nombre_Host", Nombre_Host)

                cmd.CommandType = CommandType.StoredProcedure
                Dim dr As OleDbDataReader = cmd.ExecuteReader()

                If dr.Read() Then
                    If dr.Item("Status") = "SESION INICIADA" Then
                        userData("Username") = dr.Item("Username")
                        userData("Password") = dr.Item("Pssword")
                        userData("CodigoSesion") = dr.Item("CodigoSesion")
                        userData("CodigoUser") = dr.Item("CodigoUser")
                        userData("Status") = dr.Item("Status")

                        Return userData

                    ElseIf dr.Item("Status") = "PREVIA SESION ACTIVA" Then

                        userData("PrevCodigoSesion") = dr.Item("PrevCodigoSesion")
                        userData("Username") = dr.Item("Username")
                        userData("CodigoUser") = dr.Item("CodigoUser")
                        userData("Nombre_Host") = dr.Item("Nombre_Host")
                        userData("LastLogin") = dr.Item("LastLogin")
                        userData("TimeSinceLastLogin") = dr.Item("TimeSinceLastLogin")
                        userData("Status") = dr.Item("Status")

                        Return userData

                    ElseIf dr.Item("Status") = "ERROR AL INICIAR SESION" Then
                        Return Nothing
                    End If
                End If

                Return Nothing

            End Using
        Catch ex As Exception
            MsgBox("Error en clase de seguridad: " & ex.Message, MsgBoxStyle.Critical, "Error en Inicio de Sesión")
            Exit Function
        End Try

    End Function

#End Region

#Region "CERRAR SESION"

    Public Function CerrarSesion(codigoUser As String, codigoSesion As String) As Boolean
        Try
            Using dbCon As New OleDbConnection(conn)
                dbCon.Open()

                Dim cmd As New OleDbCommand("sp_sys_logout", dbCon)
                cmd.Parameters.AddWithValue("@CodigoUser", codigoUser)
                cmd.Parameters.AddWithValue("@CodigoSesion", codigoSesion)
                cmd.CommandType = CommandType.StoredProcedure

                ' Check if stored procedure executed successfully
                Dim dr As OleDbDataReader = cmd.ExecuteReader()

                If dr.Read() Then
                    If dr.Item("Status") = "SESION FINALIZADA" Then
                        Return True
                    ElseIf dr.Item("Status") = "SESION NO ENCONTRADA" Then
                        Return False
                    End If
                End If

                Return False

            End Using
        Catch ex As Exception
            MsgBox("Error en clase de seguridad: " & ex.Message, MsgBoxStyle.Critical, "Sistema de Facturacion")
            Return False
        End Try
    End Function

#End Region

End Class