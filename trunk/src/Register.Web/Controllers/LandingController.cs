using Register.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace Register.Web.Controllers
{
    using CSP.Lib.Diagnostic;
    using Models;
    using System.Threading.Tasks;

    public class LandingController : BaseController
    {
        // GET: Landing
        public async Task<ActionResult> Index()
        {
            var model = new LandingPageViewModel();
            var api = SysParameterHelper.ApiUrlServerSide;
            try
            {
                using (var client = new HttpClient())
                {
                    ///client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = await client.GetAsync(api + "Master/ExamTypeHomes");
                    response.EnsureSuccessStatusCode();
                    model.TesttypeList = await response.Content.ReadAsAsync<List<TesttypeListingModel>>();
                }
            }
            catch (Exception ex)
           {
                Log.WriteErrorLog(tsw.TraceError, ex);
            }                    
            return View(model);
        }
    }
}