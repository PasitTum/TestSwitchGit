using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Register.Web.Controllers
{
    public class CacheController : Controller
    {

        public async Task<ActionResult> Schedules(int testTypeID)
        {
            var biz = new Biz.CalendarBiz();
            return Json(await biz.ListCalendarAsync("MASTER", testTypeID));
        }
    }
}