using System;
using System.Web.Security;
using WickedSick.Data.Members;

public partial class login_register : System.Web.UI.Page
{
    protected void butRegister_Click(object sender, EventArgs e)
    {
        if (IsValid)
        {
            MembersDataContext memberData = new MembersDataContext();

            int Result = memberData.MemberRegister(txtUsername.Text, txtPassword.Text, txtFirstName.Text, txtLastName.Text, txtEmail.Text);
            if (Result == -1)
            {
                lblMessage.Text = "Username Already Registered";
            }
            else
            {
                FormsAuthentication.RedirectFromLoginPage(Result.ToString(), false);
            }
        }
    }
}
