Imports System.Data

Partial Class usercontrol_menu_tools
    Inherits System.Web.UI.UserControl
    Dim conn As New FACTURACION_CLASS.seguridad
    Dim Database As New FACTURACION_CLASS.database


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            GetMenu()

        End If

    End Sub

#Region "CREAR EL MENU DE FORMA DINAMICA"
    Private Sub GetMenu()
        Try

            Dim SQL As String = String.Empty
            SQL = " EXEC sp_menu_accesos " & _
                  "@cod_usuario = " & Request.Cookies("CKSMFACTURA")("cod_usuario") & "," & _
                  "@cod_padre = 3"

            Dim ds As DataSet
            ds = Database.GetDataSet(SQL)

            Dim HTML As String = String.Empty
            Dim ruta As String = String.Empty
            Dim clase As String = String.Empty
            Dim label As String = String.Empty

            HTML = "<ul>"

            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1

                ruta = ds.Tables(0).Rows(i).Item("ruta").ToString.Trim
                clase = ds.Tables(0).Rows(i).Item("Icono").ToString.Trim
                label = ds.Tables(0).Rows(i).Item("etiqueta").ToString.Trim

                HTML &= "<li><a href=""" & ResolveClientUrl(ruta) & """><i class=""" & clase & """></i> " & label & "</a></li>"

                HTML &= "</li>"

            Next i

            HTML &= "</ul>"

            Me.ltMenu.Text = HTML

            ds.Dispose()

        Catch ex As Exception
            Me.ltMenu.Text = "<ul><li>" & ex.Message & "</li></ul>"
        End Try
    End Sub
#End Region
End Class
