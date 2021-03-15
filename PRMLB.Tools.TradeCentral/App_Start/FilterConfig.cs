using System.Web;
using System.Web.Mvc;

namespace PRMLB.Tools.TradeCentral
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
