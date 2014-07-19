using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using WhiteCliff.WebZinc;
using WhiteCliff.WebZinc.Objects;
using WickedSick.Data.Nfl;

namespace WickedSick.Parsing.Nfl
{
    public class Espn
    {
        private const string SCOREBOARD_URL = "http://scores.espn.go.com/nfl/scoreboard?weekNumber={1}&seasonYear={0}&seasonType=2";
        private const string BOXSCORE_URL = "http://mlb.mlb.com/stats/individual_stats_player.jsp?playerID={0}&statType=2";

        NflDataContext nflData = new NflDataContext();
        WebZinc parser = new WebZinc();

        public void LoadGames(int Season)
        {
            for (int i = 0; i < 17; i++)
            {
                LoadGames(Season, i);
            }
        }

        public void LoadGames(int Season, int Week)
        {
            parser.OpenPage(string.Format(SCOREBOARD_URL, Season, Week));

            foreach (Link link in parser.CurrentPage.Links)
            {
                if (link.Text.Equals("Box Score"))
                {
                    int gameId = int.Parse(link.URL.Substring(link.URL.IndexOf("gameId=") + 7));
                    LoadGame(Season, Week, gameId);
                }
            }
        }

        public void LoadGame(int Season, int Week, int GameId)
        {
            GAME game = (from p in nflData.GAMEs
                         where p.EspnId == GameId
                         select p).SingleOrDefault();

            if (game == null)
            {
                game = new GAME();
                game.EspnId = GameId;
                nflData.GAMEs.InsertOnSubmit(game);
            }

            parser.OpenPage(string.Format(BOXSCORE_URL, GameId));

            string awayTeamAbbr = parser.CurrentPage.Tables[0].Rows[1].Cells[0].Links[0].URL;
            awayTeamAbbr = awayTeamAbbr.Substring(awayTeamAbbr.IndexOf("team=") + 5);

            TEAM awayTeam = (from p in nflData.TEAMs
                             where p.EspnAbbr.Equals(awayTeamAbbr)
                             select p).SingleOrDefault();

            if (awayTeam == null)
            {
                //add team
            }

            int awayTeamScore = int.Parse(parser.CurrentPage.Tables[0].Rows[1].Cells[5].Text);

            string homeTeamAbbr = parser.CurrentPage.Tables[0].Rows[2].Cells[0].Links[0].URL;
            homeTeamAbbr = homeTeamAbbr.Substring(homeTeamAbbr.IndexOf("team=") + 5);

            TEAM homeTeam = (from p in nflData.TEAMs
                             where p.EspnAbbr.Equals(homeTeamAbbr)
                             select p).SingleOrDefault();

            if (homeTeam == null)
            {
                //add team
            }

            int homeTeamScore = int.Parse(parser.CurrentPage.Tables[0].Rows[2].Cells[5].Text);

            game.Season = Season;
            game.Week = Week;
            game.AwayTeam = awayTeam.TEAM_ID;
            game.AwayScore = awayTeamScore;
            game.HomeTeam = homeTeam.TEAM_ID;
            game.HomeScore = homeTeamScore;

            nflData.SubmitChanges();

            LoadGameStats(game, parser.CurrentPage);
        }

        private void LoadGameStats(GAME Game, Page GamePage)
        {
            Table statTable = GamePage.Tables[4];
            PLAYER player;
            for (int i = 2; i < statTable.Rows.Count - 1; i++)
            {
                player = GetPlayer(statTable.Rows[i].Cells[0]);
                PLAYER_GAME_PASSING passing = (from p in nflData.PLAYER_GAME_PASSINGs
                                               where p.Game == Game.GAME_ID && p.Player == player.PLAYER_ID && p.Team == Game.AwayTeam
                                               select p).SingleOrDefault();

                if (passing == null)
                {
                    passing = new PLAYER_GAME_PASSING();
                    passing.Game = Game.GAME_ID;
                    passing.Player = player.PLAYER_ID;
                    passing.Team = Game.AwayTeam;
                    nflData.PLAYER_GAME_PASSINGs.InsertOnSubmit(passing);
                }

                passing.Attempts = int.Parse(statTable.Rows[i].Cells[1].Text.Split('/')[0]);
                passing.Completions = int.Parse(statTable.Rows[i].Cells[1].Text.Split('/')[1]);
                passing.Yards = int.Parse(statTable.Rows[i].Cells[2].Text);
                passing.Touchdowns = int.Parse(statTable.Rows[i].Cells[4].Text);
                passing.Interceptions = int.Parse(statTable.Rows[i].Cells[5].Text);
                //set the rest of the stats
            }

            nflData.SubmitChanges();
        }

        private PLAYER GetPlayer(TableCell PlayerCell)
        {
            string playerName = string.Empty;
            int espnId = 0;

            if (PlayerCell.Links.Count == 1)
            {
                playerName = PlayerCell.Links[0].Text;
                int idIndex = PlayerCell.Links[0].URL.IndexOf("playerId=");
                if (idIndex != -1)
                {
                    if (!int.TryParse(PlayerCell.Links[0].URL.Substring(idIndex + 9), out espnId))
                    {
                        //can't cast string to int
                    }
                }
                else
                {
                    //"playerId=" not found in Url
                }
            }
            else
            {
                //no link found, lookup player by name?
            }

            PLAYER player = (from p in nflData.PLAYERs
                             where p.EspnId == espnId
                             select p).SingleOrDefault();

            if (player == null)
            {
                player = new PLAYER();
                player.FullName = playerName;
                player.EspnId = espnId;
                nflData.PLAYERs.InsertOnSubmit(player);
                nflData.SubmitChanges();
            }

            return player;
        }
    }
}
