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
using WickedSick.Data.Mlb;

public partial class mlb_playercontract : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int playerId = int.Parse(Request.QueryString["player"]);

        MlbDataContext mlbData = new MlbDataContext();

        var player = (from p in mlbData.PLAYERs
                      where p.PLAYER_ID == playerId
                      select p).SingleOrDefault();

        var contract = (from c in mlbData.CONTRACTs
                       where c.Player == playerId
                       select c).SingleOrDefault();

        lblPlayer.Text = player.Name;

        if (contract != null)
        {

        }
        else
        {

        }
    }
    
    protected void butSubmit_Click(object sender, EventArgs e)
    {
        int playerId = int.Parse(Request.QueryString["player"]);

        MlbDataContext mlbData = new MlbDataContext();

        CONTRACT con = (from c in mlbData.CONTRACTs
                        where c.Player == playerId
                        select c).SingleOrDefault();

        if (con == null)
        {
            con = new CONTRACT();
            con.Player = playerId;
            mlbData.CONTRACTs.InsertOnSubmit(con);
        }
        con.YearSigned = int.Parse(drpYearSigned.Text);
        mlbData.SubmitChanges();

        foreach (string key in Request.Form.Keys)
        {
            int index = key.IndexOf("Salary");
            if (index >= 0)
            {
                int year = int.Parse(key.Substring(index + 6));
                var conYear = (from cy in mlbData.CONTRACT_YEARs
                               where cy.Contract == con.CONTRACT_ID && cy.Year == year
                               select cy).SingleOrDefault();

                if (conYear == null)
                {
                    conYear = new CONTRACT_YEAR();
                    conYear.Contract = con.CONTRACT_ID;
                    conYear.Year = year;
                    mlbData.CONTRACT_YEARs.InsertOnSubmit(conYear);
                }
                conYear.Salary = decimal.Parse(Request.Form[key]);
            }
        }
        mlbData.SubmitChanges();
    }

    protected void drpYears_SelectedIndexChanged(object sender, EventArgs e)
    {
        pnlYears.Controls.Clear();

        int numYears = int.Parse(drpYears.SelectedValue);
        int yearSigned = int.Parse(drpYearSigned.SelectedValue);
        for (int i = 0; i < numYears; i++)
        {
            Literal br = new Literal();
            br.Text = string.Format("<br />{0}:&nbsp;", yearSigned + i);
            pnlYears.Controls.Add(br);
            TextBox txtSalary = new TextBox();
            txtSalary.ID = string.Format("Salary{0}", yearSigned + i);
            txtSalary.Attributes["onblur"] = "sumSalaries();";
            pnlYears.Controls.Add(txtSalary);
        }
    }
}
