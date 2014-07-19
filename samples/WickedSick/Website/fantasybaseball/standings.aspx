<%@ Page Language="C#" MasterPageFile="~/default.master" AutoEventWireup="true" CodeFile="standings.aspx.cs" Inherits="fantasybaseball_standings" Title="WickedSick Fantasy Baseball - League Standings" %>
<asp:Content ID="conBody" ContentPlaceHolderID="cphBody" Runat="Server">
    <asp:GridView ID="grdStandings" runat="server" Width="100%" AutoGenerateColumns="false" CellPadding="2">
        <HeaderStyle CssClass="tableHeader" />
        <RowStyle CssClass="rowStyle" />
        <AlternatingRowStyle CssClass="altRowStyle" />
        <Columns>
            <asp:HyperLinkField DataNavigateUrlFields="TEAM_ID" DataNavigateUrlFormatString="roster.aspx?team={0}" DataTextField="Name" HeaderText="Team" />
        </Columns>
    </asp:GridView>
</asp:Content>

