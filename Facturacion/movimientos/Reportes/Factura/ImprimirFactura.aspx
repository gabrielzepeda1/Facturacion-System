<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ImprimirFactura.aspx.vb" Inherits="movimientos_Reportes_Factura_ImprimirFactura" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">

    <div>
         <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>  

        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Height="603px" Width="690px" SizeToReportContent="True" PageCountMode="Actual" ZoomMode="PageWidth">
              <ServerReport ReportPath="movimientos\Reportes\Factura\RptFactura.rdlc" DisplayName="Facturas"  />
              <LocalReport ReportEmbeddedResource="" ReportPath="movimientos\Reportes\Factura\RptFactura.rdlc">
                  <DataSources>
                     <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="DataSet1" />
                 </DataSources>
            </LocalReport>
        </rsweb:ReportViewer>
        
         
        
       <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="GetData()" TypeName="FacturacionDataSetTableAdapters.ImpDocEnLineaTableAdapter" >
           <SelectParameters>
               <asp:Parameter DefaultValue="1" Name="opcion" Type="Int32" />
               <asp:SessionParameter DefaultValue="" Name="codigoPais" SessionField="cod_pais" Type="Int32" />
               <asp:SessionParameter Name="codigoPuesto" SessionField="cod_puesto" Type="Int32" />
               <asp:SessionParameter Name="codigoEmpresa" SessionField="cod_empresa" Type="Int32" />
               <asp:SessionParameter Name="no_factura" SessionField="no_factura" Type="Int32" />
               <asp:Parameter Name="fecha" Type="DateTime" DefaultValue="GETDATE()" />
               <asp:SessionParameter DefaultValue="" Name="codcliente" SessionField="cod_cliente" Type="String" />
                 
           </SelectParameters>
        </asp:ObjectDataSource>
        <%--<asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" EnableScriptLocalization="true">--%>
        
        <%--<asp:ObjectDataSource ID="ObjectDataSource1" runat="server"></asp:ObjectDataSource>--%>
    
        <%--<asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>--%>
    
       <asp:Literal ID="ltMensajeGrid" runat="server"></asp:Literal>
        </div>
    </form>
</body>
</html>
    
             
             
             