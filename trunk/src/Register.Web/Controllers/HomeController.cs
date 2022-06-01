using CSP.Lib.Diagnostic;
using CSP.Lib.Extension;
using Register.Models;
using Register.Web.Helpers;
using Register.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Register.Web.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View("Index");
        }

        [CalendarOpenFilter(CalendarCode = "QA", SystemType = "MASTER")]
        public ActionResult Faq()
        {
            return View();
        }

        public async Task<ActionResult> ReadNews(string testTypeID)
        {
            var model = new NewsViewModel();
            var api = SysParameterHelper.ApiUrlServerSide;
            try
            {
                using (var client = new HttpClient())
                {
                    ///client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = await client.GetAsync(api + "News/" + testTypeID);
                    response.EnsureSuccessStatusCode();
                    model.News = await response.Content.ReadAsAsync<List<NewsModel>>();

                    var response2 = await client.GetAsync(api + "News/" + testTypeID + "/Details/-1");
                    response2.EnsureSuccessStatusCode();
                    model.SubNews = await response2.Content.ReadAsAsync<List<SubNewsModel>>();
                }
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, ex);
            }
            return PartialView("_News", model);
        }
    }
}