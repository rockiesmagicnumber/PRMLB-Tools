using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRMLB.Tools.JimUnknownFixer
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<double, string> PlayersAndIds = new Dictionary<double, string>();
            List<JimUnknown> jimUnknowns = new List<JimUnknown>();
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["PRMLBEntities"].ConnectionString))
            {
                string transSql = "SELECT * FROM trade_history";
                using (var cmd = new SqlCommand(transSql, conn))
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            List<string> PlayerNames = new List<string>();
                            List<double> PlayerIds = new List<double>();
                            bool newPlayer = false;
                            string CurrentPlayer = string.Empty;
                            int playerCount = 0;

                            // read "summary" column to extract player names into PlayerNames List<string>
                            foreach (char c in reader.GetString(1))
                            {
                                if (newPlayer)
                                {
                                    CurrentPlayer += c;
                                }
                                if (c == '<')
                                {
                                    newPlayer = true;
                                }
                                else if (c == '>')
                                {
                                    if (CurrentPlayer.Contains("player"))
                                    {
                                        var playerDetails = CurrentPlayer.Split(':');
                                        if (!PlayerNames.Contains(playerDetails[0]))
                                        {
                                            PlayerNames.Add(playerDetails[0]);
                                        }
                                    }
                                    newPlayer = false;
                                    CurrentPlayer = string.Empty;
                                }
                            }

                            // iterate through PlayerId columns to get List<double> of players
                            for (int i = 0; i < 2; i++)
                            {
                                for (int j = 0; j < 5; j++)
                                {
                                    PlayerIds.Add(reader.GetDouble(reader.GetOrdinal($"player_id_{i}_{j}")));
                                }
                            }

                            for (int i = 0; i < PlayerNames.Count; i++)
                            {
                                if (!PlayersAndIds.ContainsKey(PlayerIds.ToArray()[i]))
                                {
                                    PlayersAndIds.Add(PlayerIds.ToArray()[i], PlayerNames.ToArray()[i]);
                                }
                            }
                        }
                    }
                    conn.Close();
                }
                string playerSql = @"select player_id,first_name,last_name from players where first_name= @firstName and last_name=@lastName";
                using (var cmd2 = new SqlCommand(playerSql, conn))
                {
                    cmd2.Parameters.AddWithValue("@firstName", "Jim");
                    cmd2.Parameters.AddWithValue("@lastName", "Unknown");
                    conn.Open();
                    using (var reader = cmd2.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            jimUnknowns.Add(new JimUnknown
                            {
                                PlayerId = reader.GetDouble(0),
                            });
                        }
                    }
                    conn.Close();
                }

                foreach (var ju in jimUnknowns)
                {
                    if (PlayersAndIds.ContainsKey(ju.PlayerId))
                    {
                        ju.FirstName = PlayersAndIds[ju.PlayerId].Split(' ')[0].Replace("'", "''");
                        ju.LastName = PlayersAndIds[ju.PlayerId].Replace(ju.FirstName + " ", string.Empty).Replace("'", "''");
                    }
                }

                string updateSql = string.Empty;
                foreach (var ju in jimUnknowns.Where(x => !string.IsNullOrEmpty(x.FirstName)))
                {
                    updateSql += $"UPDATE players SET first_name = '{ju.FirstName}',last_name ='{ju.LastName}' WHERE player_id = {ju.PlayerId}; ";
                }
                if (!string.IsNullOrEmpty(updateSql))
                {
                    using (var updateCmd = new SqlCommand(updateSql, conn))
                    {
                        conn.Open();
                        updateCmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }

            foreach (var ju in jimUnknowns.Where(x => !string.IsNullOrEmpty(x.FirstName)))
            {
                Console.WriteLine($"Matched PlayerID {ju.PlayerId} to {ju.FirstName + " " + ju.LastName}");
            }
            //foreach (var d in PlayersAndIds.Distinct())
            //{
            //    Console.WriteLine(d.Value.ToString() + "'s playerId is: " + d.Key.ToString());
            //}
            Console.WriteLine("Press any key to conclude");
            Console.ReadKey();
        }

        private class JimUnknown
        {
            public double PlayerId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }
    }
}
