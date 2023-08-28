Imports System.Security
Imports System.Security.Principal
Imports System.Net
Imports System.Net.Dns
Imports System.Management

Public Class seguridad
#Region "PROPIEDADES DE LA CLASE"
    Dim _conn As String = String.Empty
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
    ''' RETORNA UN MENSAJE DE INFORMACIÓN, ALERTA, EXTIO O ERROR
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

#Region "OPTENER DATOS DE LA MAQUINA CLIENTE"
    ''' <summary>
    ''' DIRECCIÓN IP DE LA MAQUINA CLIENTE QUE ESTA UTILIZANDO EL SISTEMA
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function IPHost() As String
        'Dim ip As Net.Dns
        Dim nombrePC As String
        Dim entradasIP As Net.IPHostEntry
        'nombrePC = ip.GetHostName
        'entradasIP = ip.GetHostByName(nombrePC)
        nombrePC = Dns.GetHostName
        entradasIP = Dns.GetHostByName(nombrePC)
        Dim direccion_Ip As String = entradasIP.AddressList(0).ToString
        Return direccion_Ip
    End Function

    ''' <summary>
    ''' NOMBRE DE LA MAQUINA CLIENTE QUE ESTA UTILIZANDO EL SISTEMA
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function NameHost() As String
        Return Dns.GetHostName
    End Function

    ''' <summary>
    ''' USUARIO ACTUAL DE LA MAQUINA CLIENTE QUE ESTA UTILIZANDO EL SISTEMA
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UserHost() As String
        Return WindowsIdentity.GetCurrent().Name
    End Function
#End Region

#Region "INICION DE SESIÓN"
    ''' <summary>
    ''' ENVIA UNA SOLICITUD AL SERVIDOR PARA VERIFICAR SI EL USUARIO Y EL PASSWORD ESTAN REGISTRADOS
    ''' </summary>
    ''' <param name="Usuario">NOMBRE DE USUARIO</param>
    ''' <param name="Password">CONTRASEñA DE USUARIO</param>
    ''' <returns>BOLIANO</returns>
    ''' <remarks></remarks>
    Public Function IniciarSesion(ByVal Usuario As String, _
                                  ByVal Password As String) As Boolean

        Dim dbCon As New System.Data.OleDb.OleDbConnection(conn)
        Try
            If dbCon.State = ConnectionState.Closed Then
                dbCon.Open()
            End If

            Dim sql As String = String.Empty
            sql = "EXEC sp_sys_login @cuenta = '" & Usuario & "', @password = '" & Password & "'"

            Dim cmd As New System.Data.OleDb.OleDbCommand(sql, dbCon)
            Dim dr As System.Data.OleDb.OleDbDataReader
            dr = cmd.ExecuteReader

            If dr.Read() Then
                Return True
            Else
                Return False
            End If

            dr.Close()

        Catch ex As Exception
            MsgBox("Error en clase de seguridad: " & ex.Message, MsgBoxStyle.Critical, "Sistema de Corrales de Engorde ")
            Return False

        Finally
            If dbCon.State = ConnectionState.Open Then
                dbCon.Close()
            End If
        End Try
    End Function

    ''' <summary>
    ''' REGISTRA LOS DATOS DE UN INICIO DE SESIÓN
    ''' </summary>
    ''' <param name="Usuario">NOMBRE DE USUARIO</param>
    ''' <param name="IpConexion">IP PUBLICA DE LA CONEXION</param>
    ''' <param name="Nombre_Host">NOMBRE DE HOST</param>
    ''' <returns>OleDbDataReader</returns>
    ''' <remarks></remarks>
    Public Function ControlarSesion(Usuario As String, _
                                    IpConexion As String, _
                                    Nombre_Host As String) As System.Data.OleDb.OleDbDataReader

        Dim dbCon As New System.Data.OleDb.OleDbConnection(conn)
        Dim dr As System.Data.OleDb.OleDbDataReader

        Try
            If dbCon.State = ConnectionState.Closed Then
                dbCon.Open()
            End If

            Dim sql As String = "SET DATEFORMAT DMY " + vbCrLf & _
                                "EXEC sp_sys_Abrir_Sesion " & _
                                "@Usuario = '" & Usuario & "'," & _
                                "@IpConexion = '" & IpConexion & "'," & _
                                "@Nombre_Host = '" & Nombre_Host & "' "

            Dim cmd As New System.Data.OleDb.OleDbCommand(sql, dbCon)

            dr = cmd.ExecuteReader

            Return dr

            dr.Close()

        Catch ex As Exception
            MsgBox("Error en clase de seguridad: " & ex.Message, MsgBoxStyle.Critical, "Sistema de Corrales de Engorde")
            Return Nothing

        End Try
    End Function
#End Region
End Class
