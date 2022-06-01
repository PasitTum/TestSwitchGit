using CSP.Lib.Diagnostic;
using CSP.Lib.Extension;
using Register.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Register.Web.Helpers
{
    public static class TesttypeHelper
    {
        public static TraceSwitch tsw = new TraceSwitch("mySwitch", "");
        public static IHtmlString GetTesttypeShortname(this HtmlHelper helper, string testtypeID)
        {
            var itesttypeID = testtypeID.ToInt();
            var cacheKey = "testtypes";
            // TODO : ถ้าอนาคต ต้องการ Get Field อื่นเพิ่ม  ให้ refactor ช่วงนี้ไปเป็นอีก function นึงนะ
            var testtypeList = CacheHelper.GetCachingData<List<TesttypeListingModel>>(cacheKey);
            if (testtypeList == null)
            {
                var api = SysParameterHelper.ApiUrlServerSide;
                try
                {
                    using (var client = new HttpClient())
                    {
                        ///client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var response = client.GetAsync(api + "Master/ExamTypeHomes").Result;
                        response.EnsureSuccessStatusCode();
                        testtypeList = response.Content.ReadAsAsync<List<TesttypeListingModel>>().Result;
                        CacheHelper.SetCachingData(cacheKey, testtypeList, "00:05:00");
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteErrorLog(tsw.TraceError, ex);
                }
            }
            var result = "";
            if (testtypeList != null )
            {
                result = testtypeList.Where(x => x.TEST_TYPE_ID == itesttypeID).Select(s => s.ABBR_NAME).FirstOrDefault() ?? string.Empty;
            }
            return MvcHtmlString.Create(result);
        }
    }
}