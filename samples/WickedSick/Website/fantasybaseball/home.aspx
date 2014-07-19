<%@ Page Language="C#" MasterPageFile="~/default.master" AutoEventWireup="true" CodeFile="home.aspx.cs" Inherits="fantasybaseball_home" Title="WickedSick Fantasy Baseball Home" %>
<asp:Content ID="conBody" ContentPlaceHolderID="cphBody" Runat="Server">
    My Teams
    <br />
    <asp:Repeater ID="rptTeams" runat="server">
        <ItemTemplate>
            <div style="border: solid 1px black; background-color: Silver; margin: 4px; padding: 4px;">
                <a href="roster.aspx?league=<%# DataBinder.Eval(Container.DataItem, "League") %>&team=<%# DataBinder.Eval(Container.DataItem, "TEAM_ID") %>"><%# DataBinder.Eval(Container.DataItem, "Name") %></a>
                <br />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="standings.aspx?league=<%# DataBinder.Eval(Container.DataItem, "League") %>"><%# DataBinder.Eval(Container.DataItem, "LEAGUE1.Name") %></a>
            </div>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <div style="border: solid 1px black; background-color: White; margin: 4px; padding: 4px;">
                <a href="roster.aspx?league=<%# DataBinder.Eval(Container.DataItem, "League") %>&team=<%# DataBinder.Eval(Container.DataItem, "TEAM_ID") %>"><%# DataBinder.Eval(Container.DataItem, "Name") %></a>
                <br />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="standings.aspx?league=<%# DataBinder.Eval(Container.DataItem, "League") %>"><%# DataBinder.Eval(Container.DataItem, "LEAGUE1.Name") %></a>
            </div>
        </AlternatingItemTemplate>
    </asp:Repeater>
</asp:Content>

