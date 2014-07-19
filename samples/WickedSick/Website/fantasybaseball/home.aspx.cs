using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using WickedSick.Data.FantasyBaseball;

public partial class fantasybaseball_home : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        FantasyBaseballDataContext fantasyData = new FantasyBaseballDataContext();

        var teams = from t in fantasyData.TEAMs
                    where t.Owner == int.Parse(Context.User.Identity.Name)
                    select t;

        rptTeams.DataSource = teams;
        rptTeams.DataBind();
    }
}
