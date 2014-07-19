<%@ Page Language="C#" MasterPageFile="~/default.master" AutoEventWireup="true" CodeFile="waivers.aspx.cs" Inherits="fantasybaseball_waivers" Title="WickedSick Fantasy Baseball - Waivers" %>
<asp:Content ID="conBody" ContentPlaceHolderID="cphBody" Runat="Server">
    <table width="100%">
        <tr>
            <td><a href="waivers.aspx?team=<%=Request.QueryString["team"] %>&pos=1">C</a></td>
            <td><a href="waivers.aspx?team=<%=Request.QueryString["team"] %>&pos=2">1B</a></td>
            <td><a href="waivers.aspx?team=<%=Request.QueryString["team"] %>&pos=3">2B</a></td>
            <td><a href="waivers.aspx?team=<%=Request.QueryString["team"] %>&pos=4">3B</a></td>
            <td><a href="waivers.aspx?team=<%=Request.QueryString["team"] %>&pos=5">SS</a></td>
            <td><a href="waivers.aspx?team=<%=Request.QueryString["team"] %>&pos=6">LF</a></td>
            <td><a href="waivers.aspx?team=<%=Request.QueryString["team"] %>&pos=7">CF</a></td>
            <td><a href="waivers.aspx?team=<%=Request.QueryString["team"] %>&pos=8">RF</a></td>
            <td><a href="waivers.aspx?team=<%=Request.QueryString["team"] %>&pos=16">DH</a></td>
            <td><a href="waivers.aspx?team=<%=Request.QueryString["team"] %>&pos=13">P</a></td>
            <td><asp:TextBox ID="txtName" runat="server"></asp:TextBox></td>
            <td><asp:Button ID="butSubmit" runat="server" Text="Search" 
                    onclick="butSubmit_Click" /></td>
            <td style="width:100%;"></td>
        </tr>
    </table>
    <asp:GridView ID="grdHitters" runat="server" Width="100%" 
        AutoGenerateColumns="false" onrowcommand="grdHitters_RowCommand" CellPadding="4">
        <HeaderStyle CssClass="tableHeader" />
        <RowStyle CssClass="rowStyle" />
        <AlternatingRowStyle CssClass="altRowStyle" />
        <Columns>
            <asp:ButtonField CommandName="Add" Text="Add" />
            <asp:HyperLinkField DataNavigateUrlFields="Player" DataNavigateUrlFormatString="player.aspx?id={0}"
                DataTextField="Name" HeaderText="Player" />
            <asp:TemplateField HeaderText="ELIG" ItemStyle-HorizontalAlign="center">
                <ItemTemplate>
                    <asp:Label ID="lblEligible" runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="H/AB" ItemStyle-HorizontalAlign="center" DataField="HAB" />
            <asp:BoundField HeaderText="R" ItemStyle-HorizontalAlign="center" DataField="R" />
            <asp:BoundField HeaderText="HR" ItemStyle-HorizontalAlign="center" DataField="HR" />
            <asp:BoundField HeaderText="RBI" ItemStyle-HorizontalAlign="center" DataField="RBI" />
            <asp:BoundField HeaderText="SB" ItemStyle-HorizontalAlign="center" DataField="SB" />
            <asp:BoundField HeaderText="AVG" ItemStyle-HorizontalAlign="center" DataField="AVG" DataFormatString="{0:F3}" />
            <asp:BoundField HeaderText="OBP" ItemStyle-HorizontalAlign="center" DataField="OBP" DataFormatString="{0:F3}" />
            <asp:BoundField HeaderText="SLG" ItemStyle-HorizontalAlign="center" DataField="SLG" DataFormatString="{0:F3}" />
            <asp:BoundField HeaderText="OPS" ItemStyle-HorizontalAlign="center" DataField="OPS" DataFormatString="{0:F3}" />
        </Columns>
    </asp:GridView>
    <br />
    <asp:GridView ID="grdPitchers" runat="server" Width="100%" 
        AutoGenerateColumns="false" CellPadding="4" 
        onrowcommand="grdPitchers_RowCommand">
        <HeaderStyle CssClass="tableHeader" />
        <RowStyle CssClass="rowStyle" />
        <AlternatingRowStyle CssClass="altRowStyle" />
        <Columns>
            <asp:ButtonField CommandName="Add" Text="Add" />
            <asp:HyperLinkField DataNavigateUrlFields="Player" DataNavigateUrlFormatString="player.aspx?id={0}"
                DataTextField="Name" HeaderText="Player" />
            <asp:TemplateField HeaderText="ELIG" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:Label ID="lblEligible" runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="IP" ItemStyle-HorizontalAlign="center" DataField="IP" />
            <asp:BoundField HeaderText="W" ItemStyle-HorizontalAlign="center" DataField="W" />
            <asp:BoundField HeaderText="SV" ItemStyle-HorizontalAlign="center" DataField="SV" />
            <asp:BoundField HeaderText="K" ItemStyle-HorizontalAlign="center" DataField="SO" />
            <asp:BoundField HeaderText="K/9" ItemStyle-HorizontalAlign="center" DataField="K9" DataFormatString="{0:F2}" />
            <asp:BoundField HeaderText="HLD" ItemStyle-HorizontalAlign="center" DataField="HLD" />
            <asp:BoundField HeaderText="ERA" ItemStyle-HorizontalAlign="center" DataField="ERA" DataFormatString="{0:F2}" />
            <asp:BoundField HeaderText="WHIP" ItemStyle-HorizontalAlign="center" DataField="WHIP" DataFormatString="{0:F2}" />
        </Columns>
    </asp:GridView>
</asp:Content>

