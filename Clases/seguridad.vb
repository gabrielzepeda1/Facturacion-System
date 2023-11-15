Imports System.Data.OleDb
Imports System.Security
Imports System.Security.Principal
Imports System.Net
Imports System.Net.Dns
Imports System.Management
Imports Serilog

Public Class seguridad
#Region "PROPIEDADES DE LA CLASE"
    Dim _conn As String = String.Empty

    Public Shared ReadOnly ConnectionString As String = "Data Source=localhost;Server=PC-GABRIEL\SQLENT;Database=Facturacion;UID=sa;PWD=ga123"


    Public Property conn_Procedimientos() As String
        Get
            _conn = "Data Source=localhost;Server=PC-GABRIEL\SQLENT;Database=Facturacion;UID=sa;PWD=ga123"

            '_conn = "Provider=SQLOLEDB.1;Server=WINDOWS-TC0TRCD\SQLEXPRESS;Database=DBGEIN;UID=gein;PWD=7s=3mxC$am?PFB%d%x"

            Return _conn
        End Get
        Set(ByVal value As String)
            _conn = value
        End Set
    End Property
    Public Property conn() As String
        Get
            _conn = "Provider=SQLOLEDB.1;Server=PC-GABRIEL\SQLENT;Database=Facturacion;UID=sa;PWD=ga123"

            '_conn = "Data Source=PC-GABRIEL\SQLENT;Initial Catalog=Facturacion;Persist Security Info=True;User ID=sa;Password=ga123"

            '_conn = "Provider=SQLOLEDB.1;Server=WINDOWS-TC0TRCD\SQLEXPRESS;Database=DBGEIN;UID=gein;PWD=7s=3mxC$am?PFB%d%x"

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

    Dim _key As String = String.Empty
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

    ''' <summary>
    ''' RETORNA UN MENSAJE DE INFORMACIÓN, ALERTA, EXITO O ERROR
    ''' </summary>
    ''' <param name="Mensaje">TEXTO QUE SE MOSTRARA EN EL MENSAJE</param>
    ''' <param name="Tipo">TIPO DE MENSAJE A MOSTRAR. PUEDE SER: [info, exito, alerta, error]</param>
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

    Public Sub LogMessage(ByVal Mensaje As String, ByVal Tipo As String)
        Select Case Tipo
            Case "info"
                Log.Information(Mensaje)
            Case "exito"
                Log.Information(Mensaje)
            Case "alerta"
                Log.Warning(Mensaje)
            Case "error"
                Log.Error(Mensaje)
            Case Else
                Log.Information(Mensaje)

        End Select
    End Sub

#Region "OBTENER DATOS DE LA MAQUINA CLIENTE"

    'Obtener la direccion IP del PC del usuario. 
    Public Function IPHost() As String

        Dim nombrePC As String
        Dim entradasIP As Net.IPHostEntry

        nombrePC = Dns.GetHostName
        entradasIP = Dns.GetHostEntry(nombrePC)

        Dim direccion_Ip As String = entradasIP.AddressList(0).ToString

        Return direccion_Ip
    End Function

    ' Nombre del PC del usuario logueado
    Public Function NameHost() As String
        Return Dns.GetHostName
    End Function

    ' USUARIO ACTUAL DE LA MAQUINA CLIENTE QUE ESTA UTILIZANDO EL SISTEMA
    Public Function UserHost() As String
        Return WindowsIdentity.GetCurrent().Name
    End Function
#End Region

#Region "INICION DE SESIÓN"
    ''' <summary>
    ''' Verifica en el servidor si el Usuario y Password estan registrados
    ''' </summary>
    ''' <param name="Usuario">NOMBRE DE USUARIO</param>
    ''' <param name="Password">CONTRASEñA DE USUARIO</param>
    Public Function IniciarSesion(Usuario As String, Password As String) As Boolean
        Try

            Using dbCon As New OleDbConnection(conn)
                dbCon.Open()

                Dim cmd As New OleDbCommand("sp_sys_login", dbCon)
                cmd.Parameters.AddWithValue("@Usuario", Usuario)
                cmd.Parameters.AddWithValue("@Password", Password)
                cmd.CommandType = CommandType.StoredProcedure

                Dim dr As OleDbDataReader = cmd.ExecuteReader()

                If dr.Read() Then
                    Return True
                End If
            End Using



        Catch ex As Exception
            MsgBox("Error en clase de seguridad: " & ex.Message, MsgBoxStyle.Critical, "Sistema de Corrales de Engorde ")
            LogError(ex)
        End Try
        Return False
    End Function

    ''' <summary>
    ''' REGISTRA LOS DATOS DE UN INICIO DE SESIÓN
    ''' </summary>
    ''' <param name="Usuario">NOMBRE DE USUARIO</param>
    ''' <param name="IpConexion">IP PUBLICA DE LA CONEXION</param>
    ''' <param name="Nombre_Host">NOMBRE DE HOST</param>
    ''' <returns>OleDbDataReader</returns>
    ''' <remarks></remarks>

    Public Function CerrarSesion(codigoSesion As Integer, codigoUsuario As Integer) As Boolean

        Try
            Using dbCon As New OleDbConnection(conn)
                dbCon.Open()

                Dim cmd As New OleDbCommand("sp_sys_cerrar_sesion", dbCon)
                cmd.Parameters.AddWithValue("@cod_sesion", codigoSesion)
                cmd.Parameters.AddWithValue("@cod_usuario", codigoUsuario)
                cmd.CommandType = CommandType.StoredProcedure

                ' Add an output parameter to capture the success indicator (BIT)
                Dim successParam As New OleDbParameter("@success", OleDbType.Boolean)
                successParam.Direction = ParameterDirection.Output
                cmd.Parameters.Add(successParam)

                ' Execute the stored procedure (no result set expected)
                cmd.ExecuteNonQuery()

                ' Retrieve the success indicator from the output parameter
                Dim success As Boolean = CBool(cmd.Parameters("@success").Value)
                Return success

            End Using

        Catch ex As Exception
            LogMessage(ex.Message, "error")
        End Try

    End Function
    Public Function ControlarSesion(Usuario As String,
                                    IpConexion As String,
                                    Nombre_Host As String) As Dictionary(Of String, Object)
        Dim sessionData As New Dictionary(Of String, Object)
        Dim dbCon As New OleDbConnection(conn)

        Try
            If dbCon.State = ConnectionState.Closed Then
                dbCon.Open()
            End If

            Dim sql As String = "SET DATEFORMAT DMY " + vbCrLf &
                                "EXEC sp_sys_Abrir_Sesion " &
                                "@Username = '" & Usuario & "'," &
                                "@IpConexion = '" & IpConexion & "'," &
                                "@Nombre_Host = '" & Nombre_Host & "' "

            Dim cmd As New OleDbCommand(sql, dbCon)

            Dim dr As OleDbDataReader = cmd.ExecuteReader()

            If dr.Read() Then

                sessionData("CodigoSesion") = dr.Item("CodigoSesion")
                sessionData("CodigoUser") = dr.Item("CodigoUser")
                sessionData("Username") = dr.Item("Username")
                sessionData("Password") = dr.Item("Password")
                sessionData("CodigoPais") = dr.Item("CodigoPais")
                sessionData("Pais") = dr.Item("Pais")
                sessionData("CodigoEmpresa") = dr.Item("CodigoEmpresa")
                sessionData("Empresa") = dr.Item("Empresa")
                sessionData("CodigoPuesto") = dr.Item("CodigoPuesto")
                sessionData("Puesto") = dr.Item("Puesto")
                sessionData("Mensaje") = dr.Item("MENSAJE")



            Else
                sessionData("Mensaje") = dr.Item("MENSAJE")
            End If

            Return sessionData

            dr.Close()
        Catch ex As Exception
            '            MsgBox("Error en clase de seguridad: " & ex.Message,
            'MsgBoxStyle.Critical, "Sistema de Corrales de Engorde")

            LogMessage(ex.Message, "error")
            Return Nothing

        End Try
    End Function

    Private Sub LogError(ex As Exception)

    End Sub


#End Region
End Class
