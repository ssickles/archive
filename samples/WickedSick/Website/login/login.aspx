<%@ Page Language="C#" MasterPageFile="~/default.master" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="login_login" Title="WickedSick Fantasy Baseball" %>
<asp:Content ID="conBody" ContentPlaceHolderID="cphBody" Runat="Server">
    Username:
    <br />
    <asp:TextBox ID="txtUsername" runat="server" MaxLength="50"></asp:TextBox>
    <asp:RequiredFieldValidator ID="rfvUsername" runat="Server" ControlToValidate="txtUsername"
        Display="Dynamic" ErrorMessage="Username is required!" Text="Required!"></asp:RequiredFieldValidator>
    <br />
    Password:
    <br />
    <asp:TextBox ID="txtPassword" runat="server" MaxLength="16" TextMode="Password"></asp:TextBox>
    <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword"
        Display="Dynamic" ErrorMessage="Password is required!" Text="Required!"></asp:RequiredFieldValidator>
    <br />
    <br />
    <asp:LinkButton ID="lnkRegister" runat="server" Text="Create new account" PostBackUrl="~/login/register.aspx"
        CausesValidation="false"></asp:LinkButton>
    <br />
    <br />
    <asp:Button ID="butLogin" runat="server" Text="Login" OnClick="butLogin_Click"></asp:Button>
    <br />
    <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
</asp:Content>

