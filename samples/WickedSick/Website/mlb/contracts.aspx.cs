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

public partial class mlb_contracts : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    
    protected void butSearch_Click(object sender, EventArgs e)
    {
        MlbDataContext mlbData = new MlbDataContext();

        if (txtSearch.Text.Length > 0)
        {
            var players = from p in mlbData.PLAYERs
                          where p.Name.IndexOf(txtSearch.Text) >= 0
                          select p;

            grdPlayerContracts.DataSource = players;
            grdPlayerContracts.DataBind();
        }
    }
}
