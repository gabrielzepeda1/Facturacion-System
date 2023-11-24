Imports System.Data

Partial Class usercontrol_menu_catalogos
    Inherits System.Web.UI.UserControl
    Dim conn As New FACTURACION_CLASS.seguridad
    Dim Database As New FACTURACION_CLASS.database

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            'GetMenu()
            GetMenuEncabezado()
        End If

    End Sub

#Region "CREAR EL MENU DE FORMA DINAMICA"
    Private Sub GetMenu()
        Try

            Dim SQL As String = String.Empty
            SQL = " EXEC sp_menu_accesos " & _
                  "@cod_usuario = " & Request.Cookies("CKSMFACTURA")("cod_usuario") & "," & _
                  "@cod_padre = 9"

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

            'Me.ltMenu.Text = HTML

            ds.Dispose()

        Catch ex As Exception
            'Me.ltMenu.Text = "<ul><li>" & ex.Message & "</li></ul>"
        End Try
    End Sub


    Private Sub GetMenuEncabezado()
        Try

            Dim SQL As String = String.Empty
            SQL = " EXEC sp_menu_accesos " & _
                  "@cod_usuario = " & Request.Cookies("CKSMFACTURA")("cod_usuario") & "," & _
                  "@cod_padre = NULL"

            Dim ds As DataSet
            ds = Database.GetDataSet(SQL)

            Dim HTML As String = String.Empty

            HTML = "<ul>"
            HTML &= "<li><a href=""" & ResolveClientUrl("../Default.aspx") & """>Inicio</a></li>"

            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1

                If ds.Tables(0).Rows(i).Item("cod_padre").ToString.Trim = String.Empty Then
                    HTML &= "<li><a>" & ds.Tables(0).Rows(i).Item("etiqueta").ToString.Trim & "</a>"

                    HTML &= GetMenuDetalle(ds.Tables(0).Rows(i).Item("cod_menu").ToString.Trim, ds)

                    HTML &= "</li>"
                End If

            Next i

            HTML &= "</ul>"

            'Me.ltMenu.Text = HTML

            ds.Dispose()

        Catch ex As Exception
            'Me.ltMenu.Text = "<ul><li>" & ex.Message & "</li></ul>"
        End Try
    End Sub

    Function GetMenuDetalle(COD_PADRE As String, ds As DataSet) As String
        Dim HTML As String = String.Empty

        Try
            Dim ruta As String = String.Empty
            Dim clase As String = String.Empty
            Dim label As String = String.Empty

            HTML = "<ul>"
            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                If ds.Tables(0).Rows(i).Item("cod_padre").ToString.Trim = COD_PADRE Then

                    ruta = ds.Tables(0).Rows(i).Item("ruta").ToString.Trim
                    clase = ds.Tables(0).Rows(i).Item("Icono").ToString.Trim
                    label = ds.Tables(0).Rows(i).Item("etiqueta").ToString.Trim

                    HTML &= "<li><a href=""" & ResolveClientUrl(ruta) & """><i class=""" & clase & """></i> " & label & "</a></li>"

                End If
            Next i
            HTML &= "</ul>"

        Catch ex As Exception
            HTML = "<ul><li>Error al crear el detalle: " & ex.Message & "</li></ul>"

        End Try

        Return HTML
    End Function
#End Region

End Class
