using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRMLB.Tools.Data;

namespace PRMLB.Tools.ViewModel
{
    public class TradeCentralViewModel
    {
        public TradeCentralViewModel()
        {

        }
        private List<team> _teams { get; set; }
        public List<team> Teams
        {
            get
            {
                if (_teams == null)
                {
                    using (var db = new PRMLBEntities())
                    {
                        _teams = db.teams.ToList();
                    }
                }
                return _teams;
            }
            set { _teams = value; }
        }
    }
}
