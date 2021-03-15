using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRMLB.Tools.ViewModel
{
    public class NewTradeViewModel
    {
        public int Team1 { get; set; }
        public int Team2 { get; set; }

        public NewTradeViewModel(int team1, int team2)
        {
            Team1 = team1;
            Team2 = team2;
        }
    }
}
