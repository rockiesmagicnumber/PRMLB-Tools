using PRMLB.Tools.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PRMLB.Tools.ViewModel
{
    public class TradeHistoryViewModel
    {
        public int Team1ID { get; set; }
        public team Team1 { get; set; }
        public int Team2ID { get; set; }
        public team Team2 { get; set; }
        public List<PlayerTrade> Trades { get; set; }
        public TradeHistoryViewModel(int team1Id, int team2Id)
        {
            Team1ID = team1Id;
            Team2ID = team2Id;

            using (var db = new PRMLBEntities())
            {
                Team1 = db.teams.FirstOrDefault(x => x.team_id == Team1ID);
                Team2 = db.teams.FirstOrDefault(x => x.team_id == Team2ID);
                Trades = db.trade_history
                    .Where(x => (x.team_id_0 == Team1ID && x.team_id_1 == Team2ID) || (x.team_id_0 == Team2ID && x.team_id_1 == Team1ID))
                    .OrderByDescending(x => x.date)
                    .Select(x => new PlayerTrade
                    {
                        Team1Id = x.team_id_0 ?? 0,
                        Team1Name = x.team_id_0 == Team1ID ? Team1.name + " " + Team1.nickname : Team2.name + " " + Team2.nickname,
                        Team2Id = x.team_id_1 ?? 0,
                        Team2Name = x.team_id_1 == Team1ID ? Team1.name + " " + Team1.nickname : Team2.name + " " + Team2.nickname,
                        TradeDate = x.date ?? new DateTime(),
                        Team1Players = db.players
                            .Where(p => (p.player_id == x.player_id_0_0 && x.player_id_0_0 > 0) ||
                                (p.player_id == x.player_id_0_1 && x.player_id_0_1 > 0) ||
                                (p.player_id == x.player_id_0_2 && x.player_id_0_2 > 0) ||
                                (p.player_id == x.player_id_0_3 && x.player_id_0_3 > 0) ||
                                (p.player_id == x.player_id_0_4 && x.player_id_0_4 > 0))
                            .Select(pp => new Player
                            {
                                PlayerId = pp.player_id ?? -1,
                                FirstName = pp.first_name,
                                LastName = pp.last_name,
                                Position = (Positions)pp.position
                            })
                            .ToList(),
                        Team1Picks = new List<double?> {
                            x.draft_round_0_0,
                            x.draft_round_0_1,
                            x.draft_round_0_2,
                            x.draft_round_0_3,
                            x.draft_round_0_4
                        }
                        .Where(p => p > 0)
                        .Select(p => new Pick { Round = p })
                        .ToList(),
                        Team1Cash = x.cash_0 ?? 0,
                        Team2Players = db.players
                            .Where(p => (p.player_id == x.player_id_1_0 && x.player_id_1_0 > 0) ||
                                (p.player_id == x.player_id_1_1 && x.player_id_1_1 > 0) ||
                                (p.player_id == x.player_id_1_2 && x.player_id_1_2 > 0) ||
                                (p.player_id == x.player_id_1_3 && x.player_id_1_3 > 0) ||
                                (p.player_id == x.player_id_1_4 && x.player_id_1_4 > 0))
                            .Select(pp => new Player
                            {
                                PlayerId = pp.player_id ?? -1,
                                FirstName = pp.first_name,
                                LastName = pp.last_name,
                                Position = (Positions)pp.position
                            })
                            .ToList(),
                        Team2Picks = new List<double?> {
                            x.draft_round_1_0,
                            x.draft_round_1_1,
                            x.draft_round_1_2,
                            x.draft_round_1_3,
                            x.draft_round_1_4
                        }
                        .Where(p => p > 0)
                        .Select(p => new Pick { Round = p })
                        .ToList(),
                        Team2Cash = x.cash_1 ?? 0,
                    })
                    .ToList();
            }
        }

        public class PlayerTrade
        {
            public double Team1Id { get; set; }
            public string Team1Name { get; set; }
            public double Team2Id { get; set; }
            public string Team2Name { get; set; }
            public DateTime TradeDate { get; set; }
            public List<Player> Team1Players { get; set; }
            public List<Pick> Team1Picks { get; set; }
            public double Team1Cash { get; set; }
            public List<Player> Team2Players { get; set; }
            public List<Pick> Team2Picks { get; set; }
            public double Team2Cash { get; set; }
        }
        public class Player
        {
            public double PlayerId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string FullName => FirstName + " " + LastName;
            public Positions Position { get; set; }
        }
        public class Pick
        {
            public double? Round { get; set; }
            public string RoundName
            {
                get
                {
                    switch (Round)
                    {
                        case 1:
                            return "1st";
                        case 2:
                            return "2nd";
                        default:
                            return Round.ToString() + "th";
                    }
                }
            }
        }
        public enum Positions : int
        {
            P = 1,
            Catcher,
            FirstBase,
            SecondBase,
            ThirdBase,
            ShortStop,
            LeftField,
            CenterField,
            RightField
        }
    }
}
