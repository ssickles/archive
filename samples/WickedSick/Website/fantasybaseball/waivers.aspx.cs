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
using WickedSick.Data.Mlb;
using System.Text;

public partial class fantasybaseball_waivers : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FantasyBaseballDataContext fantasyData = new FantasyBaseballDataContext();

            int position = int.Parse(Request.QueryString["pos"]);
            if (position == 11 || position == 12 || position == 13)
            {
                var pitchers = from p in fantasyData.GetWaiversPitchersYearly(5, 2007)
                               where p.Position == position && p.Team == null
                               select new
                               {
                                   p.Player,
                                   p.Position,
                                   p.Name,
                                   IP = FantasyBaseball.IP(p.Player, p.OUTS),
                                   p.W,
                                   p.SV,
                                   p.SO,
                                   K9 = FantasyBaseball.K9(p.Player, p.OUTS, p.SO),
                                   p.HLD,
                                   ERA = FantasyBaseball.ERA(p.Player, p.OUTS, p.ER),
                                   WHIP = FantasyBaseball.WHIP(p.Player, p.OUTS, p.H, p.BB)
                               };

                grdPitchers.DataSource = pitchers;
                grdPitchers.DataKeyNames = new string[] { "Player" };
                grdPitchers.DataBind();
            }
            else
            {
                var batters = from b in fantasyData.GetWaiversBattersYearly(5, 2007)
                              where b.Position == position && b.Team == null
                              select new
                              {
                                  b.Player,
                                  b.Position,
                                  b.Name,
                                  HAB = (b.H.HasValue && b.AB.HasValue) ? b.H.ToString() + "/" + b.AB.ToString() : null,
                                  b.R,
                                  b.HR,
                                  b.RBI,
                                  b.SB,
                                  AVG = FantasyBaseball.AVG(b.Player, b.AB, b.H),
                                  OBP = FantasyBaseball.OBP(b.Player, b.AB, b.BB, b.HBP, b.SF, b.H),
                                  SLG = FantasyBaseball.SLG(b.Player, b.AB, b.H, b._2B, b._3B, b.HR),
                                  OPS = FantasyBaseball.OBP(b.Player, b.AB, b.BB, b.HBP, b.SF, b.H) + FantasyBaseball.SLG(b.Player, b.AB, b.H, b._2B, b._3B, b.HR)
                              };

                grdHitters.DataSource = batters;
                grdHitters.DataKeyNames = new string[] { "Player" };
                grdHitters.DataBind();
            }
        }
    }

    protected void grdHitters_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        DataKey data = grdHitters.DataKeys[int.Parse(e.CommandArgument.ToString())];
        Response.Redirect(string.Format("roster.aspx?team={0}&addplayer={1}", Request.QueryString["team"], data.Values["Player"].ToString()));
    }

    protected void grdPitchers_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        DataKey data = grdPitchers.DataKeys[int.Parse(e.CommandArgument.ToString())];
        Response.Redirect(string.Format("roster.aspx?team={0}&addplayer={1}", Request.QueryString["team"], data.Values["Player"].ToString()));
    }

    protected void butSubmit_Click(object sender, EventArgs e)
    {
        string path = Request.Url.LocalPath;
        StringBuilder query = new StringBuilder("?");
        foreach (string key in Request.QueryString.Keys)
        {
            if (key.Equals("name"))
            {
                if (txtName.Text.Length > 0)
                {
                    query.Append(key);
                    query.Append("=");
                    query.Append(txtName.Text);
                    query.Append("&");
                }
            }
            else
            {
                query.Append(key);
                query.Append("=");
                query.Append(Request.QueryString[key]);
                query.Append("&");
            }
        }
        if (!query.ToString().Contains("name=") && txtName.Text.Length > 0)
        {
            query.Append("name=");
            query.Append(txtName.Text);
            query.Append("&");
        }
        query.Remove(query.Length - 1, 1);

        Response.Redirect(path + query.ToString());
    }
}
