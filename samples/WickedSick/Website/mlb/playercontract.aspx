<%@ Page Language="C#" MasterPageFile="~/default.master" AutoEventWireup="true" CodeFile="playercontract.aspx.cs" Inherits="mlb_playercontract" Title="WickedSick MLB Player Contract" %>
<asp:Content ID="conBody" ContentPlaceHolderID="cphBody" Runat="Server">
    <script language="javascript" type="text/javascript">
        function sumSalaries()
        {
            var items = document.getElementsByTagName("input");
            var i = 0;
            var total = 0;
            
            for (i; i < items.length; i++)
            {
                if (items[i].id.indexOf("Salary") != -1)
                {
                    total += Number(items[i].value);
                }
            }
            document.getElementById("contractTotal").innerHTML = total;
        }
    </script>
    <asp:Label ID="lblPlayer" runat="server"></asp:Label>
    <br />
    <br />
    Contract Signed: <asp:DropDownList ID="drpYearSigned" runat="server">
        <asp:ListItem Text="2000" Value="2000"></asp:ListItem>
        <asp:ListItem Text="2001" Value="2001"></asp:ListItem>
        <asp:ListItem Text="2002" Value="2002"></asp:ListItem>
        <asp:ListItem Text="2003" Value="2003"></asp:ListItem>
        <asp:ListItem Text="2004" Value="2004"></asp:ListItem>
        <asp:ListItem Text="2005" Value="2005"></asp:ListItem>
        <asp:ListItem Text="2006" Value="2006"></asp:ListItem>
        <asp:ListItem Text="2007" Value="2007"></asp:ListItem>
        <asp:ListItem Text="2008" Value="2008" Selected="True"></asp:ListItem>
    </asp:DropDownList>
    <br />
    <asp:DropDownList ID="drpYears" runat="server" 
        onselectedindexchanged="drpYears_SelectedIndexChanged" AutoPostBack="True">
        <asp:ListItem Text="Select" Value="0"></asp:ListItem>
        <asp:ListItem Text="1" Value="1"></asp:ListItem>
        <asp:ListItem Text="2" Value="2"></asp:ListItem>
        <asp:ListItem Text="3" Value="3"></asp:ListItem>
        <asp:ListItem Text="4" Value="4"></asp:ListItem>
        <asp:ListItem Text="5" Value="5"></asp:ListItem>
        <asp:ListItem Text="6" Value="6"></asp:ListItem>
        <asp:ListItem Text="7" Value="7"></asp:ListItem>
        <asp:ListItem Text="8" Value="8"></asp:ListItem>
        <asp:ListItem Text="9" Value="9"></asp:ListItem>
        <asp:ListItem Text="10" Value="10"></asp:ListItem>
        <asp:ListItem Text="11" Value="11"></asp:ListItem>
        <asp:ListItem Text="12" Value="12"></asp:ListItem>
        <asp:ListItem Text="13" Value="13"></asp:ListItem>
        <asp:ListItem Text="14" Value="14"></asp:ListItem>
        <asp:ListItem Text="15" Value="15"></asp:ListItem>
    </asp:DropDownList>
    year(s)&nbsp;/&nbsp;$<span id="contractTotal">0</span>M
    <br />
    <asp:Panel ID="pnlYears" runat="server"></asp:Panel>
    <br />
    <br />
    <asp:Button ID="butSubmit" runat="server" Text="Submit" 
        onclick="butSubmit_Click" />
</asp:Content>

