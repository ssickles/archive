using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Text;
using WhiteCliff.WebZinc;
using WhiteCliff.WebZinc.Objects;
using WickedSick.Data.Mlb;

namespace WickedSick.Parsing.Mlb
{
    public class MlbCom
    {
        private const string PLAYERLIST_URL = "http://mlb.mlb.com/searchPlayerSearchServlet?conference=ML&action=searchbyposition&position=";
        private const string PLAYERBIO_URL = "http://mlb.mlb.com/team/player.jsp?player_id={0}";
        private const string PLAYERSTATS_URL = "http://mlb.mlb.com/stats/individual_stats_player.jsp?playerID={0}&statType=2";
        private const string PLAYERSTATS2_URL = "http://mlb.mlb.com/stats/individual_stats_player.jsp?c_id=mlb&playerID={0}&section1=1&statSet1=2&section2=1&section3=1&statSet3=1&statSet2=2";

        MlbDataContext mlbData = new MlbDataContext();
        WebZinc parser = new WebZinc();

        public void LoadPlayers()
        {
            parser.OpenPage(PLAYERLIST_URL);

            foreach (Link link in parser.CurrentPage.Links)
            {
                PLAYER player = null;
                if (link.URL.Contains("individual_stats_player.jsp?"))
                {
                    int playerId = int.Parse(link.URL.Substring(link.URL.ToLower().IndexOf("playerid=") + 9));
                    player = GetPlayer(playerId, link.Text);
                }
                else if (link.URL.Contains("player.jsp?"))
                {
                    int playerId = int.Parse(link.URL.Substring(link.URL.ToLower().IndexOf("player_id=") + 10));
                    player = GetPlayer(playerId, link.Text);
                    LoadPlayerBio(player);
                }

                if (player != null)
                {
                    LoadPlayerStats(player);
                    mlbData.SubmitChanges();
                    LoadPlayerStats2(player);
                }
                mlbData.SubmitChanges();
            }
        }

        private void LoadPlayerBio(PLAYER Player)
        {
        }

        private void LoadPlayerStats(PLAYER Player)
        {
            parser.OpenPage(string.Format(PLAYERSTATS_URL, Player.MlbComId));

            foreach (Table table in parser.CurrentPage.Tables)
            {
                if (table.Rows[0].Text.Length >= 8)
                {
                    if (table.Rows[0].Text.Substring(0, 8).ToLower() == "pitching")
                    {
                        foreach (TableRow row in table.Rows)
                        {
                            int year;
                            if (int.TryParse(row.Cells[0].Text, out year))
                            {
                                TEAM team = GetTeam(row.Cells[1].Text);
                                PLAYER_YEARLY_PITCHING pitching = (from p in mlbData.PLAYER_YEARLY_PITCHINGs
                                                                   where p.Player == Player.PLAYER_ID && p.Year == year && p.Team == team.TEAM_ID
                                                                   select p).SingleOrDefault();

                                if (pitching == null)
                                {
                                    //add new
                                    pitching = new PLAYER_YEARLY_PITCHING();
                                    pitching.Player = Player.PLAYER_ID;
                                    pitching.Year = year;
                                    pitching.Team = team.TEAM_ID;
                                    mlbData.PLAYER_YEARLY_PITCHINGs.InsertOnSubmit(pitching);
                                }

                                pitching.W = ParseInt(row.Cells[2].Text);
                                pitching.L = ParseInt(row.Cells[3].Text);
                                pitching.G = ParseInt(row.Cells[5].Text);
                                pitching.GS = ParseInt(row.Cells[6].Text);
                                pitching.CG = ParseInt(row.Cells[7].Text);
                                pitching.SHO = ParseInt(row.Cells[8].Text);
                                pitching.SV = ParseInt(row.Cells[9].Text);
                                pitching.SVO = ParseInt(row.Cells[10].Text);
                                string[] innings = row.Cells[11].Text.Split('.');
                                pitching.OUTS = (ParseInt(innings[0]) * 3) + ParseInt(innings[1]);
                                pitching.H = ParseInt(row.Cells[12].Text);
                                pitching.R = ParseInt(row.Cells[13].Text);
                                pitching.ER = ParseInt(row.Cells[14].Text);
                                pitching.HR = ParseInt(row.Cells[15].Text);
                                pitching.HBP = ParseInt(row.Cells[16].Text);
                                pitching.BB = ParseInt(row.Cells[17].Text);
                                pitching.SO = ParseInt(row.Cells[18].Text);
                            }
                        }
                    }
                    else if (table.Rows[0].Text.Substring(0, 7).ToLower() == "hitting")
                    {
                        foreach (TableRow row in table.Rows)
                        {
                            int year;
                            if (int.TryParse(row.Cells[0].Text, out year))
                            {
                                TEAM team = GetTeam(row.Cells[1].Text);
                                PLAYER_YEARLY_BATTING batting = (from b in mlbData.PLAYER_YEARLY_BATTINGs
                                                                 where b.Player == Player.PLAYER_ID && b.Year == year && b.Team == team.TEAM_ID
                                                                 select b).SingleOrDefault();
                                if (batting == null)
                                {
                                    //add new
                                    batting = new PLAYER_YEARLY_BATTING();
                                    batting.Player = Player.PLAYER_ID;
                                    batting.Year = year;
                                    batting.Team = team.TEAM_ID;
                                    mlbData.PLAYER_YEARLY_BATTINGs.InsertOnSubmit(batting);
                                }

                                batting.G = ParseInt(row.Cells[2].Text);
                                batting.AB = ParseInt(row.Cells[3].Text);
                                batting.R = ParseInt(row.Cells[4].Text);
                                batting.H = ParseInt(row.Cells[5].Text);
                                batting._2B = ParseInt(row.Cells[6].Text);
                                batting._3B = ParseInt(row.Cells[7].Text);
                                batting.HR = ParseInt(row.Cells[8].Text);
                                batting.RBI = ParseInt(row.Cells[9].Text);
                                batting.BB = ParseInt(row.Cells[11].Text);
                                batting.SO = ParseInt(row.Cells[12].Text);
                                batting.SB = ParseInt(row.Cells[13].Text);
                                batting.CS = ParseInt(row.Cells[14].Text);
                            }
                        }
                    }
                }
            }
        }

        private void LoadPlayerStats2(PLAYER Player)
        {
            parser.OpenPage(string.Format(PLAYERSTATS2_URL, Player.MlbComId));

            foreach (Table table in parser.CurrentPage.Tables)
            {
                if (table.Rows[0].Text.Length >= 8)
                {
                    if (table.Rows[0].Text.Substring(0, 8).ToLower() == "pitching")
                    {
                        foreach (TableRow row in table.Rows)
                        {
                            int year;
                            if (int.TryParse(row.Cells[0].Text, out year))
                            {
                                TEAM team = GetTeam(row.Cells[1].Text);
                                PLAYER_YEARLY_PITCHING pitching = (from p in mlbData.PLAYER_YEARLY_PITCHINGs
                                                                   where p.Player == Player.PLAYER_ID && p.Year == year && p.Team == team.TEAM_ID
                                                                   select p).SingleOrDefault();

                                if (pitching == null)
                                {
                                    //error, a record should have been added from the primary page
                                }
                                else
                                {
                                    pitching.TB = ParseInt(row.Cells[3].Text);
                                    pitching.BK = ParseInt(row.Cells[4].Text);
                                    pitching.WP = ParseInt(row.Cells[5].Text);
                                    pitching.IBB = ParseInt(row.Cells[6].Text);
                                    pitching.SB = ParseInt(row.Cells[7].Text);
                                    pitching.CS = ParseInt(row.Cells[8].Text);
                                    pitching.PK = ParseInt(row.Cells[9].Text);
                                    pitching.GO = ParseInt(row.Cells[10].Text);
                                    pitching.AO = ParseInt(row.Cells[11].Text);
                                }
                            }
                        }
                    }
                    else if (table.Rows[0].Text.Substring(0, 7).ToLower() == "hitting")
                    {
                        foreach (TableRow row in table.Rows)
                        {
                            int year;
                            if (int.TryParse(row.Cells[0].Text, out year))
                            {
                                TEAM team = GetTeam(row.Cells[1].Text);
                                PLAYER_YEARLY_BATTING batting = (from b in mlbData.PLAYER_YEARLY_BATTINGs
                                                                 where b.Player == Player.PLAYER_ID && b.Year == year && b.Team == team.TEAM_ID
                                                                 select b).SingleOrDefault();
                                if (batting == null)
                                {
                                    //error, a record should have been added from the primary page
                                }
                                else
                                {
                                    batting.SF = ParseInt(row.Cells[2].Text);
                                    batting.SH = ParseInt(row.Cells[3].Text);
                                    batting.HBP = ParseInt(row.Cells[4].Text);
                                    batting.IBB = ParseInt(row.Cells[5].Text);
                                    batting.GDP = ParseInt(row.Cells[6].Text);
                                    batting.PA = ParseInt(row.Cells[7].Text);
                                    batting.NP = ParseInt(row.Cells[8].Text);
                                    batting.GO = ParseInt(row.Cells[11].Text);
                                    batting.AO = ParseInt(row.Cells[12].Text);
                                }
                            }
                        }
                    }
                }
            }
        }

        private PLAYER GetPlayer(int PlayerId, string PlayerName)
        {
            PLAYER player = (from p in mlbData.PLAYERs
                             where p.MlbComId == PlayerId
                             select p).SingleOrDefault();

            if (player == null)
            {
                player = new PLAYER();
                player.MlbComId = PlayerId;
                player.Name = PlayerName;
                mlbData.PLAYERs.InsertOnSubmit(player);
                mlbData.SubmitChanges();
            }

            return player;
        }

        private TEAM GetTeam(string TeamName)
        {
            TEAM team = (from t in mlbData.TEAMs
                         where t.City + " " + t.Nickname == TeamName
                         select t).SingleOrDefault();

            if (team == null)
            {
                //add team
                team = new TEAM();
                string[] teamParts = TeamName.Split(' ');
                team.City = teamParts[0];
                for (int i = 1; i < teamParts.Length; i++)
                {
                    team.Nickname += teamParts[i];
                }
                team.Franchise = 31;
                team.Division = 1;
                team.YearStarted = 2000;
                team.YearEnded = 2000;
                mlbData.TEAMs.InsertOnSubmit(team);
                mlbData.SubmitChanges();
            }

            return team;
        }

        private int ParseInt(string Value)
        {
            int result = 0;
            int.TryParse(Value, out result);
            return result;
        }
    }
}
