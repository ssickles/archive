<%@ Page Language="C#" MasterPageFile="~/default.master" AutoEventWireup="true" CodeFile="contracts.aspx.cs" Inherits="mlb_contracts" Title="Major League Baseball Contracts" %>
<asp:Content ID="conBody" ContentPlaceHolderID="cphBody" Runat="Server">
    Player: <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox> 
    <asp:Button ID="butSearch" runat="server" Text="Search" 
        onclick="butSearch_Click" />
    <br />
    <br />
    <asp:GridView ID="grdPlayerContracts" runat="server" AutoGenerateColumns="false" Width="100%">
        <Columns>
            <asp:HyperLinkField DataTextField="Name" HeaderText="Player" DataNavigateUrlFields="PLAYER_ID" DataNavigateUrlFormatString="playercontract.aspx?player={0}" />
        </Columns>
    </asp:GridView>
</asp:Content>

