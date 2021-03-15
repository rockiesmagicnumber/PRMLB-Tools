using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PRMLB.Tools.TradeCentral
{
    public class PRMLBViewEngine : RazorViewEngine
    {
        public PRMLBViewEngine()
        {
            var viewLocations = new[]
            {
                "~/Views/{1}/{0}.cshtml",
                "~/Views/{1}/_{0}.cshtml",
                "~/Views/Home/{0}.cshtml",
                "~/Views/PartialView/_{0}.cshtml",
                "~/Views/Shared/{0}.cshtml",
                "~/Views/{0}.cshtml",
            };
            this.PartialViewLocationFormats = viewLocations;
            this.ViewLocationFormats = viewLocations;
        }
    }
}