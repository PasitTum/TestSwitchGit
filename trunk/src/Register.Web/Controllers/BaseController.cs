using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using BrockAllen.CookieTempData;

namespace Register.Web.Controllers
{
    public class BaseController : Controller
    {
        protected TraceSwitch tsw = new TraceSwitch("mySwitch", "");

        protected override ITempDataProvider CreateTempDataProvider()
        {
            return new CookieTempDataProvider();
        }
    }
}