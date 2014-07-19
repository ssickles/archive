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

public partial class fantasybaseball_standings : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FantasyBaseballDataContext fantasyData = new FantasyBaseballDataContext();

            int leagueId = int.Parse(Request.QueryString["league"]);
            var teams = from t in fantasyData.TEAMs
                        where t.League == leagueId
                        select t;

            grdStandings.DataSource = teams;
            grdStandings.DataBind();
        }
    }
}
