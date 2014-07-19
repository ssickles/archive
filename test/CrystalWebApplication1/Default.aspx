<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CrystalWebApplication1._Default" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
    <body>
   <form id="form1" runat="server" style="background:lightgreen;">
<table cellpadding="0" cellspacing="0" width="100%">
    <tr>
   <td>
        <CR:CrystalReportViewer ID="rptviewer" DisplayGroupTree="False" 
    runat="server" AutoDataBind="True" Height="799px" oninit="rptviewer_Init" 
            ReportSourceID="CrystalReportSource1" Width="1029px" />
        <CR:CrystalReportSource ID="CrystalReportSource1" runat="server">
            <Report FileName="CrystalReport1.rpt">
            </Report>
        </CR:CrystalReportSource>
   </td>
    </tr>
                </table>
</form>
</body>
</html>
