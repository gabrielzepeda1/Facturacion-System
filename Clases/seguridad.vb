Imports System.Data.OleDb
Imports System.Net
Imports System.Net.Dns
Imports System.Security.Principal

Public Class seguridad

#Region "PROPIEDADES DE LA CLASE"

    Dim _conn As String = String.Empty
    Dim _key As String = String.Empty

    Public Shared ReadOnly ConnectionString As String = "Data Source=localhost;Server=GILDEDSWORD\SQLEXPRESS;Database=Facturacion;UID=sa;PWD=ga1234"

    Public Property Sql_conn() As String
        Get
            _conn = "Data Source=localhost;Server=GILDEDSWORD\SQLEXPRESS;Database=Facturacion;UID=sa;PWD=ga1234"
            Return _conn
        End Get
        Set(value As String)
            _conn = value
        End Set
    End Property

    Public Property conn() As String
        Get
            _conn = "Provider=SQLOLEDB.1;Server=GILDEDSWORD\SQLEXPRESS;Database=Facturacion;UID=sa;PWD=ga1234"
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
    ''' <param name="Tipo">Message Type: ["info", "exito", "alerta", "error"]</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function PmsgBox(Mensaje As String, Tipo As String) As String
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
    ''' <summary>
    ''' REGISTRA LOS DATOS DE UN INICIO DE SESIÓN
    ''' </summary>
    ''' <param name="Username">NOMBRE DE USUARIO</param>
    ''' <param name="IpConexion">IP PUBLICA DE LA CONEXION</param>
    ''' <param name="nombreHost">NOMBRE DE HOST</param>
    ''' <returns>Dictionary(Of String, Object)</returns>
    ''' <remarks></remarks>
    Public Function ControlarSesion(username As String, ipConexion As String, nombreHost As String) As Dictionary(Of String, Object)
        Dim sessionData As New Dictionary(Of String, Object)

        Try
            Using dbCon As New OleDbConnection(conn)
                dbCon.Open()

                Using cmd As New OleDbCommand("sp_sys_Abrir_Sesion", dbCon)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.AddWithValue("@Username", username)
                    cmd.Parameters.AddWithValue("@IpConexion", ipConexion)
                    cmd.Parameters.AddWithValue("@Nombre_Host", nombreHost)

                    Dim dr As OleDbDataReader = cmd.ExecuteReader()
                    If dr.Read() Then
                        If dr.Item("Status") = "SESION INICIADA" Then
                            sessionData("CodigoSesion") = dr.Item("CodigoSesion")
                            sessionData("CodigoUser") = dr.Item("CodigoUser")
                            sessionData("Username") = dr.Item("Username")
                            sessionData("Password") = dr.Item("Password")
                            sessionData("CodigoRol") = dr.Item("CodigoRol")
                            sessionData("Status") = dr.Item("Status")
                            sessionData("CodigoPais") = dr.Item("CodigoPais")
                            sessionData("CodigoEmpresa") = dr.Item("CodigoEmpresa")
                            sessionData("CodigoPuesto") = dr.Item("CodigoPuesto")

                            Return sessionData
                        Else
                            sessionData("Status") = dr.Item("Status")
                            Return sessionData
                        End If
                    End If

                    Return Nothing
                End Using
            End Using
        Catch ex As Exception
            PmsgBox("Error en clase de seguridad: " & ex.Message, "error")
            Return Nothing
        End Try
    End Function


#End Region

End Class