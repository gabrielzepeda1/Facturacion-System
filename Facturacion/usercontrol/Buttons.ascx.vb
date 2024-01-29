
Partial Class usercontrol_Buttons
    Inherits UserControl

    Public Event Exportar_Click As EventHandler
    Public Event Buscar_TextChanged As EventHandler
    Public Property txtSearchText As String
        Get
            If txtSearch IsNot Nothing Then
                Return txtSearch.Text.Trim()
            Else
                Return String.Empty
            End If
        End Get
        Set(value As String)
            If txtSearch IsNot Nothing Then
                txtSearch.Text = value
            End If
        End Set
    End Property

    Protected Sub btnExportar_Click(sender As Object, e As EventArgs) Handles btnExportar.Click
        RaiseEvent Exportar_Click(sender, e)
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        RaiseEvent Buscar_TextChanged(sender, e)
    End Sub
End Class
