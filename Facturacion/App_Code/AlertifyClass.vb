Imports Microsoft.VisualBasic

Public Class AlertifyClass

    Public Shared Sub AlertifySuccessMessage(page As Page, message As String)
        Dim script As String = $"alertify.success('{message}');"
        ScriptManager.RegisterStartupScript(page, page.GetType(), "alertifySuccess", script, True)
    End Sub

    Public Shared Sub AlertifyErrorMessage(page As Page, message As String)
        Dim script As String = $"alertify.error('{message}');"
        ScriptManager.RegisterStartupScript(page, page.GetType(), "alertifyError", script, True)
    End Sub

End Class
