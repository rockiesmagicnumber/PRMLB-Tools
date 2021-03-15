using PRMLB.Tools.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PRMLB.Tools.TradeCentral.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult TradeCentral()
        {
            var vm = new TradeCentralViewModel();
            return View(vm);
        }
        public ActionResult NewTrade(int team1, int team2)
        {
            var vm = new NewTradeViewModel(team1, team2);
            return PartialView(vm);
        }
        public ActionResult TradeHistory(int team1, int team2)
        {
            if (team1 == 0 || team2 == 0)
            {
                return PartialView("Error");
            }
            var vm = new TradeHistoryViewModel(team1, team2);
            return PartialView(vm);
        }
    }
}