Imports System.Web

Namespace HelperClasses
    Public Class MySession
        ' Properties
        Public Property Username As String
        Public Property CodigoUser As Integer
        Public Property CodigoRol As Integer
        Public Property CodigoSesion As Integer
        Public Property CodigoPais As Object
        Public Property CodigoEmpresa As Object
        Public Property CodigoPuesto As Object

        Private Sub New()
            Username = HttpContext.Current.Session("Username").ToString()
            CodigoUser = Convert.ToInt32(HttpContext.Current.Session("CodigoUser"))
            CodigoRol = Convert.ToInt32(HttpContext.Current.Session("CodigoRol"))
            CodigoSesion = Convert.ToInt32(HttpContext.Current.Session("CodigoSesion"))
            CodigoPais = Convert.ToInt32(HttpContext.Current.Session("CodigoPais"))
            CodigoEmpresa = Convert.ToInt32(HttpContext.Current.Session("CodigoEmpresa"))
            CodigoPuesto = Convert.ToInt32(HttpContext.Current.Session("CodigoPuesto"))
        End Sub

        ' Get Current Session
        Public Shared ReadOnly Property Current As MySession
            Get
                Dim session As MySession = DirectCast(HttpContext.Current.Session("__MySession__"), MySession)

                If session Is Nothing Then

                    session = New MySession()
                    HttpContext.Current.Session("__MySession__") = session

                End If

                Return session
            End Get
        End Property
    End Class
End Namespace
