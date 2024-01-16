Imports System.Data

Partial Class [Default]
    Inherits Page
    Dim _conn As New FACTURACION_CLASS.Seguridad
    Dim _database As New FACTURACION_CLASS.database

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            GetMenuEncabezado()
        End If

    End Sub

#Region "CREAR EL MENU DE FORMA DINAMICA"
    Private Sub GetMenuEncabezado()
        Try

            Dim sql = "EXEC sp_menu_accesos " &
                  "@cod_usuario = " & Session("CodigoUser") & "," &
                  "@cod_padre = NULL"

            Dim ds As DataSet
            ds = _database.GetDataSet(sql)

            Dim html As String

            html = "<ul>"

            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1

                If ds.Tables(0).Rows(i).Item("cod_padre").ToString.Trim = String.Empty Then

                    html &= "<div class=""accordion-item"">"
                    html &= "<h2 class= ""accordion-header"">"
                    html &= "<button class=""accordion-button collapsed"" type=""button"" data-bs-toggle=""collapse"" data-bs-target=""#collapse" & i & """ aria-expanded=""false"" aria-controls=""collapse" & i & """>"

                    html &= "<li>" ''Accordion Item #i
                    html &= "<a><i class=""fas fa-caret-right""></i> " & ds.Tables(0).Rows(i).Item("etiqueta").ToString.Trim & "</a>"
                    html &= "</button>"
                    html &= "</h2>"

                    html &= "<div id=""collapse" & i & """ class=""accordion-collapse collapse "" data-bs-parent=""#accordionExample"" >"
                    html &= "<div class=""accordion-body"">"
                    html &= GetMenuDetalle(ds.Tables(0).Rows(i).Item("cod_menu").ToString.Trim, ds)

                    html &= "</div>" ''body
                    html &= "</div>" ''collapse

                    html &= "</li>" ''Accordion item
                    html &= "</div>" ''class=""accordion-item
                End If

            Next i

            html &= "</ul>"

            Me.ltMenu.Text = html

            ds.Dispose()

        Catch ex As Exception
            Me.ltMenu.Text = "<ul><li>" & ex.Message & "</li></ul>"
        End Try
    End Sub

    Function GetMenuDetalle(codPadre As String, ds As DataSet) As String
        Dim html As String = String.Empty

        Try
            Dim ruta As String = String.Empty
            Dim clase As String = String.Empty
            Dim label As String = String.Empty

            html = "<div class=""container"">"
            html &= "<div class=""row align-items-center justify-content-between"">"

            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1

                If ds.Tables(0).Rows(i).Item("cod_padre").ToString.Trim = codPadre Then

                    ruta = ds.Tables(0).Rows(i).Item("ruta").ToString.Trim
                    clase = ds.Tables(0).Rows(i).Item("Icono").ToString.Trim
                    label = ds.Tables(0).Rows(i).Item("etiqueta").ToString.Trim

                    html &= "<div class=""col col-3 mx-2 my-2"">"
                    html &= "<ul>"
                    html &= "<li><a class=""text-decoration-none a-default"" href=""" & ResolveClientUrl(ruta) & """> <i class=""" & clase & """></i> " & label & "</a></li>"
                    html &= "</ul>"
                    html &= "</div>"

                End If

            Next i

            html &= "</div>"
            html &= "</div>"

        Catch ex As Exception
            html = "<ul><li>Error al crear el detalle: " & ex.Message & "</li></ul>"

        End Try

        Return html
    End Function
#End Region

End Class
