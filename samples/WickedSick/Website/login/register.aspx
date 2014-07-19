<%@ Page Language="C#" MasterPageFile="~/default.master" AutoEventWireup="true" CodeFile="register.aspx.cs" Inherits="login_register" Title="WickedSick Fantasy Baseball" %>
<asp:Content ID="conBody" ContentPlaceHolderID="cphBody" Runat="Server">
    Username:
    <br />
    <asp:TextBox ID="txtUsername" runat="Server" MaxLength="50"></asp:TextBox> <span class="required">*</span>
    <asp:RequiredFieldValidator ID="rfvUsername" runat="Server" Text="Required!" ControlToValidate="txtUsername"></asp:RequiredFieldValidator>
    <br />
    Password:
    <br />
    <asp:TextBox ID="txtPassword" runat="Server" TextMode="password" MaxLength="16"></asp:TextBox> <span class="required">*</span>
    <asp:RequiredFieldValidator ID="rfvPassword" runat="Server" Text="Required!" ControlToValidate="txtPassword"></asp:RequiredFieldValidator>
    <br />
    Email Address:
    <br />
    <asp:TextBox ID="txtEmail" runat="server" MaxLength="100"></asp:TextBox> <span class="required">*</span>
    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" Text="Required!" ControlToValidate="txtEmail"></asp:RequiredFieldValidator>
    <br />
    First Name:
    <br />
    <asp:TextBox ID="txtFirstName" runat="Server" MaxLength="50"></asp:TextBox> <span class="required">*</span>
    <asp:RequiredFieldValidator ID="rfvFirstName" runat="Server" Text="Required!" ControlToValidate="txtFirstName"></asp:RequiredFieldValidator>
    <br />
    Last Name:
    <br />
    <asp:TextBox ID="txtLastName" runat="Server" MaxLength="50"></asp:TextBox> <span class="required">*</span>
    <asp:RequiredFieldValidator ID="rfvLastName" runat="Server" Text="Required!" ControlToValidate="txtLastName"></asp:RequiredFieldValidator>
    <br />
    <br />
    <asp:Button ID="butRegister" runat="Server" Text="Register" OnClick="butRegister_Click"></asp:Button>
    <br />
    <asp:Label ID="lblMessage" runat="Server" Font-Bold="True" ForeColor="Red"></asp:Label>
</asp:Content>

