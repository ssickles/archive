using System;
using System.Web.UI.WebControls;
using WickedSick.Data.FantasyBaseball;
using System.Linq;
using System.Web.Security;
using System.Web;

public partial class _default : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //PopulateMenu();
        }

        if (HttpContext.Current.User.Identity.IsAuthenticated)
        {
            UserTable.Visible = true;
        }
        else
        {
            UserTable.Visible = false;
        }
    }

    private void PopulateMenu()
    {
        MenuItem item = new MenuItem("MLB Player Contracts", "", "", "/mlb/contracts.aspx");
        menMain.Items.Add(item);
    }
}
