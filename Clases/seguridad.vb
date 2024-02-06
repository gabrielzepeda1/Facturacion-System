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

End Class