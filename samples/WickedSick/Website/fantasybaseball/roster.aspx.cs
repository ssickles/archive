using System;
using System.Linq;
using WickedSick.Data.FantasyBaseball;
using WickedSick.Data.Mlb;
using System.Web.UI.WebControls;
using System.Collections;

public partial class fantasybaseball_roster : System.Web.UI.Page
{
    MlbDataContext mlbData = new MlbDataContext();
    FantasyBaseballDataContext fantasyData = new FantasyBaseballDataContext();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            int teamId = int.Parse(Request.QueryString["team"]);
            var team = (from t in fantasyData.TEAMs
                        where t.TEAM_ID == teamId
                        select t).SingleOrDefault();

            lblTeamName.Text = team.Name;
            imgTeam.ImageUrl = Request.ApplicationPath + "images/mlb/" + team.Logo;

            var batters = from b in fantasyData.GetFantasyTeamBattersYearly(4, teamId, 2007)
                          orderby b.Ordinal
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

            var pitchers = from p in fantasyData.GetFantasyTeamPitchersYearly(4, teamId, 2007)
                           orderby p.Ordinal
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

            grdHitters.DataSource = batters;
            grdHitters.DataKeyNames = new string[] { "Player" };
            grdHitters.DataBind();
            grdPitchers.DataSource = pitchers;
            grdPitchers.DataKeyNames = new string[] { "Player" };
            grdPitchers.DataBind();
        }

        if (Request.QueryString["addplayer"] != null)
        {
            MlbDataContext mlbData = new MlbDataContext();
            
            int playerId = int.Parse(Request.QueryString["addplayer"]);
            PLAYER player = (from p in mlbData.PLAYERs
                             where p.PLAYER_ID == playerId
                             select p).SingleOrDefault();

            lblAddPlayer.Text = player.Name;
            hidAddPlayer.Value = playerId.ToString();
            hidPlayerPos.Value = player.Position;
            pnlAddPlayer.Visible = true;
        }
    }

    protected void butAddPlayer_Click(object sender, EventArgs e)
    {
        FantasyBaseballDataContext fantasyData = new FantasyBaseballDataContext();

        int teamId = int.Parse(Request.QueryString["team"]);
        int playerId = int.Parse(hidAddPlayer.Value);
        var roster = (from r in fantasyData.TEAM_ROSTERs
                      where r.Team == teamId && r.Player == playerId
                      select r).SingleOrDefault();

        if (roster == null)
        {
            roster = new TEAM_ROSTER();
            roster.Player = playerId;
            roster.Team = teamId;
            fantasyData.TEAM_ROSTERs.InsertOnSubmit(roster);
            fantasyData.SubmitChanges();
        }

        Response.Redirect(string.Format("roster.aspx?team={0}", teamId));
    }

    protected void grdHitters_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HiddenField hidPlayer = (HiddenField)e.Row.Cells[0].FindControl("hidPlayerId");
            if (hidPlayer.Value.Length > 0)
            {
                int playerId = int.Parse(hidPlayer.Value);

                DropDownList drpPosition = (DropDownList)e.Row.Cells[0].FindControl("drpPosition");
                Label lblEligible = (Label)e.Row.Cells[2].FindControl("lblEligible");

                var elig = from pe in fantasyData.PLAYER_ELIGIBILITies
                           join p in fantasyData.POSITIONs on pe.Position equals p.POSITION_ID
                           where pe.Player == playerId
                           select new
                           {
                               p.Abbreviation,
                               pe.Position
                           };

                if (elig != null)
                {
                    string elList = string.Empty;
                    foreach (var el in elig)
                    {
                        elList = elList + ", " + el.Abbreviation;
                        drpPosition.Items.Add(new ListItem(el.Abbreviation, el.Position.ToString()));
                    }
                    drpPosition.Items.Add(new ListItem("Bench", "99"));
                    HiddenField hidPosition = (HiddenField)e.Row.Cells[0].FindControl("hidPosition");
                    if (hidPosition.Value.Length > 0)
                    {
                        drpPosition.SelectedValue = hidPosition.Value;
                    }
                    else
                    {
                        drpPosition.SelectedValue = "99";
                    }

                    if (elList.Length > 0)
                    {
                        elList = elList.Substring(2);
                    }

                    lblEligible.Text = elList;
                }
            }
        }
    }

    protected void grdPitchers_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HiddenField hidPlayer = (HiddenField)e.Row.Cells[0].FindControl("hidPlayerId");
            if (hidPlayer.Value.Length > 0)
            {
                int playerId = int.Parse(hidPlayer.Value);

                DropDownList drpPosition = (DropDownList)e.Row.Cells[0].FindControl("drpPosition");
                Label lblEligible = (Label)e.Row.Cells[2].FindControl("lblEligible");

                var elig = from pe in fantasyData.PLAYER_ELIGIBILITies
                           join p in fantasyData.POSITIONs on pe.Position equals p.POSITION_ID
                           where pe.Player == playerId
                           select new
                           {
                               p.Abbreviation,
                               pe.Position
                           };

                if (elig != null)
                {
                    string elList = string.Empty;
                    foreach (var el in elig)
                    {
                        elList = elList + ", " + el.Abbreviation;
                        drpPosition.Items.Add(new ListItem(el.Abbreviation, el.Position.ToString()));
                    }
                    drpPosition.Items.Add(new ListItem("Bench", "99"));
                    HiddenField hidPosition = (HiddenField)e.Row.Cells[0].FindControl("hidPosition");
                    if (hidPosition.Value.Length > 0)
                    {
                        drpPosition.SelectedValue = hidPosition.Value;
                    }
                    else
                    {
                        drpPosition.SelectedValue = "99";
                    }

                    if (elList.Length > 0)
                    {
                        elList = elList.Substring(2);
                    }

                    lblEligible.Text = elList;
                }
            }
        }
    }

    protected void butSubmit_Click(object sender, EventArgs e)
    {
    }
}
