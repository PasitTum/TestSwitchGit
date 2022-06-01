using CaptchaMvc.HtmlHelpers;
using CSP.Lib.Diagnostic;
using CSP.Lib.Models;
using CSP.Lib.Mvc;
using Newtonsoft.Json;
using Register.Models;
using Register.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Register.Web.Controllers
{
    public class SMSController : BaseController
    {
        // GET: Pencil/SMS
        public ActionResult Index()
        {
            return View();
        }

        [CalendarOpenFilter(CalendarCode = "ENROLL", SystemType = "MASTER")]
        public ActionResult Register(int? testTypeID)
        {
            return View("SMSRegister");
        }

        [HttpPost]
        [CSPValidateHttpAntiForgeryToken]
        public async Task<ActionResult> Register(InquiryModel model)
        {
            var rtn = new ResultInfo();
            SMSStatusModel result = null;
            var api = SysParameterHelper.ApiUrlServerSide;
            if (!this.IsCaptchaValid("Validate your captcha"))
            {
                ModelState.AddModelError("Captcha", "กรอกตัวอักษรที่อยู่บนหน้าเว็บไม่ถูกต้อง");
                rtn.Success = false;
                rtn.ErrorMessage = "กรอกตัวอักษรที่อยู่บนหน้าเว็บไม่ถูกต้อง";
            }
            else
            {
                rtn.Success = true;
                try
                {
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var response = await client.GetAsync(api + "SMS/SMSStatus?testTypeID=" + model.TestTypeID + "&citizenID=" + model.CitizenID);
                        var jsonResult = await response.Content.ReadAsStringAsync();
                        result = JsonConvert.DeserializeObject<SMSStatusModel>(jsonResult);
                    }
                }
                catch (Exception ex)
                {
                    rtn.Success = false;
                    rtn.ErrorMessage = ex.Message;
                    Log.WriteErrorLog(tsw.TraceError, ex);
                }
            }
            var captchaValue = this.GenerateCaptchaValue(5);
            return Json(new
            {
                rtn,
                Captcha =
                            new Dictionary<string, string>
                                {
                        {captchaValue.ImageElementId, captchaValue.ImageUrl},
                        {captchaValue.TokenElementId, captchaValue.TokenValue}
                                },
                Data = result
            });
        }
    }
}