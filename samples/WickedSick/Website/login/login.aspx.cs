using System;
using System.Web.Security;
using WickedSick.Data.Members;

public partial class login_login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        txtUsername.Focus();
    }

    protected void butLogin_Click(object sender, EventArgs e)
    {
        if (IsValid)
        {
            try
            {
                FormsAuthentication.RedirectFromLoginPage(Authenticate(txtUsername.Text, txtPassword.Text).ToString(), false);
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }
    }

    protected int Authenticate(string Username, string Password)
    {
        MembersDataContext memberData = new MembersDataContext();

        int Result = memberData.MemberAuthenticate(Username, Password);
        if (Result == -1)
        {
            throw new Exception("Invalid Username");
        }

        if (Result == -2)
        {
            throw new Exception("Invalid Password");
        }

        return Result;
    }
}
