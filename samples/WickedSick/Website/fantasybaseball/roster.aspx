<%@ Page Language="C#" MasterPageFile="~/default.master" AutoEventWireup="true" CodeFile="roster.aspx.cs" Inherits="fantasybaseball_roster" Title="WickedSick Fantasy Baseball - Roster" %>
<asp:Content ID="conBody" ContentPlaceHolderID="cphBody" Runat="Server">
    <table width="100%">
        <tr>
            <td style="width:100px;"><asp:Image ID="imgTeam" runat="server" Height="80" Width="80" /></td>
            <td valign="bottom" align="left"><h2><asp:Label ID="lblTeamName" runat="server"></asp:Label></h2></td>
            <td>
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
                        <td style="width:100%;"></td>
                    </tr>
                </table>
            </td>            
        </tr>
    </table>
    <asp:Panel ID="pnlAddPlayer" runat="server" Visible="false">
    <asp:Label ID="lblAddPlayer" runat="server"></asp:Label>
    <asp:HiddenField ID="hidAddPlayer" runat="server" />
    <asp:HiddenField ID="hidPlayerPos" runat="server" />
    <asp:Button ID="butAddPlayer" runat="server" Text="Add" 
            onclick="butAddPlayer_Click" />
    <br />
    </asp:Panel>
    <asp:GridView ID="grdHitters" runat="server" Width="100%" 
        AutoGenerateColumns="false" onrowdatabound="grdHitters_RowDataBound">
        <HeaderStyle CssClass="tableHeader" />
        <RowStyle CssClass="rowStyle" />
        <AlternatingRowStyle CssClass="altRowStyle" />
        <Columns>
            <asp:TemplateField HeaderText="POS" ItemStyle-Width="50">
                <ItemTemplate>
                    <asp:HiddenField ID="hidPlayerId" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Player") %>' />
                    <asp:HiddenField ID="hidPosition" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Position") %>' />
                    <asp:DropDownList ID="drpPosition" runat="server"></asp:DropDownList>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:HyperLinkField DataNavigateUrlFields="Player" DataNavigateUrlFormatString="player.aspx?id={0}"
                DataTextField="Name" HeaderText="Player" ItemStyle-Width="200"/>
            <asp:TemplateField HeaderText="ELIG" ItemStyle-HorizontalAlign="center" ItemStyle-Width="50">
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
        AutoGenerateColumns="false" onrowdatabound="grdPitchers_RowDataBound">
        <HeaderStyle CssClass="tableHeader" />
        <RowStyle CssClass="rowStyle" />
        <AlternatingRowStyle CssClass="altRowStyle" />
        <Columns>
            <asp:TemplateField HeaderText="POS" ItemStyle-Width="50">
                <ItemTemplate>
                    <asp:HiddenField ID="hidPlayerId" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Player") %>' />
                    <asp:HiddenField ID="hidPosition" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Position") %>' />
                    <asp:DropDownList ID="drpPosition" runat="server">
                    </asp:DropDownList>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:HyperLinkField DataNavigateUrlFields="Player" DataNavigateUrlFormatString="player.aspx?id={0}"
                DataTextField="Name" HeaderText="Player" ItemStyle-Width="200" />
            <asp:TemplateField HeaderText="ELIG" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50">
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
    <br />
    <asp:Button ID="butSubmit" runat="server" Text="Submit Changes" 
        onclick="butSubmit_Click" />
</asp:Content>

