Imports System.Xml
Imports FACTURACION_CLASS

Partial Public Class catalogos_paridad
    Inherits System.Web.UI.Page
    Dim conn As New seguridad
    Dim DataBase As New database

#Region "PROPIEDADES DEL FORMULARIO"
    ''' <summary>
    ''' UTILIZADO PARA LLENAR EL CONTROL DATAGridViewOne
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Property dtTabla() As DataTable
        Get
            Return ViewState("dtTabla")
        End Get
        Set(ByVal value As DataTable)
            ViewState("dtTabla") = value
        End Set
    End Property

    Dim _Name As String = String.Empty
    Private Property Name() As String
        Get
            Dim arrPath() As String = HttpContext.Current.Request.RawUrl.Split("/")

            _Name = arrPath(arrPath.GetUpperBound(0))

            Return _Name
        End Get
        Set(ByVal value As String)
            _Name = value
        End Set
    End Property

    Dim _Puesto_Name As String = String.Empty
    Private Property Puesto_Name() As String
        Get

            Return _Puesto_Name
        End Get
        Set(ByVal value As String)
            _Puesto_Name = value
        End Set
    End Property
#End Region

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then

            Puesto_Name = "Industrial Comercial San Martin"

            Me.txtDescripcion.Attributes.Add("placeholder", "Digite valor de Paridad")
            Me.txtDescripcion.Attributes.Add("requerid", "requerid")

            Load_GridView()

        End If
    End Sub

#Region "PROCESOS Y EVENTOS DEL GRIDVIEW"
    Private Sub Load_GridView()
        Try

            dtTabla = Me.ObtenerTC_Tabla(Now.Date)

            Me.GridViewOne.DataSource = dtTabla.DefaultView
            Me.GridViewOne.DataBind()

            'ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "smf_Script", "responsive_grid();", True)

        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.PmsgBox("Ocurrió un error al intentar cargar el listado de rubros en la tabla." & ex.Message, "error")

        End Try
    End Sub

    Protected Sub GridViewOne_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridViewOne.DataBound
        Try
            If GridViewOne.Rows.Count > 0 Then
                Dim pagerRow As GridViewRow = GridViewOne.BottomPagerRow
                Dim pageLabel As Label = CType(pagerRow.Cells(0).FindControl("CurrentPageLabel"), Label)
                If Not pageLabel Is Nothing Then
                    Dim currentPage As Integer = GridViewOne.PageIndex + 1
                    pageLabel.Text = "&nbsp;&nbsp; Pagina " & currentPage.ToString() &
                        " de " & GridViewOne.PageCount.ToString()
                End If
            End If
        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.PmsgBox("Ocurrió un error al disparar el evento DataBound. " & ex.Message, "error")

        End Try
    End Sub

    Protected Sub GridViewOne_PageIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridViewOne.PageIndexChanged
        Try

            Me.GridViewOne.SelectedIndex = -1
            Me.hdfCodigo.Value = String.Empty

            ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "smf_Script", "responsive_grid();", True)

        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.PmsgBox("Ocurrió un error al disparar el evento PageIndexChanged." & ex.Message, "error")

        End Try
    End Sub

    Protected Sub GridViewOne_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridViewOne.PageIndexChanging
        Try
            If e.NewPageIndex >= 0 Then
                Me.GridViewOne.PageIndex = e.NewPageIndex

                'Para usar la de caché guardada en la variable de sesion
                If (IsPostBack) AndAlso (Not dtTabla Is Nothing) Then
                    If Not dtTabla Is Nothing AndAlso dtTabla.Rows.Count > 0 Then
                        If dtTabla.Rows.Count > 0 Then
                            Me.GridViewOne.DataSource = dtTabla
                            Me.GridViewOne.DataBind()
                        End If
                    End If
                End If

                ScriptManager.RegisterStartupScript(Me, Me.Page.GetType, "smf_Script", "responsive_grid();", True)
            End If
        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.PmsgBox("Ocurrió un error al disparar el evento PageIndexChanging." & ex.Message, "error")

        End Try
    End Sub

    Protected Sub GridViewOne_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridViewOne.RowDataBound
        Try

            If e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Attributes.Add("OnClick", "" & Page.ClientScript.GetPostBackClientHyperlink(Me.GridViewOne, "Select$" + e.Row.RowIndex.ToString) & ";")
            End If

        Catch ex As Exception
            Me.ltMensajeGrid.Text &= conn.PmsgBox("Ocurrió un error al disparar el evento RowDataBound. " & ex.Message, "error")

        End Try
    End Sub
#End Region

#Region "TIPO DE CAMBIO"
    Public Function ObtenerTC_Dia(ByVal Fecha As Date) As Double
        Dim objServ As New TipoCambio.Tipo_Cambio_BCNSoapClient

        Try
            Return objServ.RecuperaTC_Dia(Year(Fecha), Month(Fecha), DatePart(DateInterval.Day, Fecha))
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try



    End Function

    Public Function ObtenerTC_Tabla(ByVal Fecha As Date) As DataTable
        Dim objServ As New TipoCambio.Tipo_Cambio_BCNSoapClient
        Dim objElement As XmlElement
        Dim xmlNodLista As XmlNodeList
        Dim dt As New DataTable
        Try

            ' CONSUMIMOS EL SERVICIO
            objElement = objServ.RecuperaTC_Mes(Year(Fecha), Month(Fecha))
            xmlNodLista = objElement.GetElementsByTagName("Tc")


            ' AGREGAMOS LAS COLUMNAS AL DATATABLE 
            For Each Node As XmlNode In xmlNodLista.Item(0).ChildNodes
                Dim Col As New DataColumn(Node.Name, System.Type.GetType("System.String"))
                dt.Columns.Add(Col)
            Next

            ' AGREGAR LA INFORMACION AL DATATABLE 
            For IntVal As Integer = 0 To xmlNodLista.Count - 1
                Dim dr As DataRow = dt.NewRow
                For Col As Integer = 0 To dt.Columns.Count - 1
                    If Not IsDBNull(xmlNodLista.Item(IntVal).ChildNodes(Col).InnerText) Then
                        dr(Col) = xmlNodLista.Item(IntVal).ChildNodes(Col).InnerText
                    Else
                        dr(Col) = Nothing
                    End If
                Next
                dt.Rows.Add(dr)
            Next
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        Return dt
    End Function
#End Region
End Class
