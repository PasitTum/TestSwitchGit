using CaptchaMvc.HtmlHelpers;
using CSP.Lib.Models;
using CSP.Lib.Mvc;
using Register.Models;
using Register.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Register.Web.Controllers
{
    public class AnnounceController : BaseController
    {
        // GET: Pencil/Announce
        public ActionResult Index()
        {
            return View();
        }

        [CalendarOpenFilter(CalendarCode = "SEARCH_EXAM_PASS", SystemType = "MASTER")]
        public ActionResult PassScore(int? testTypeID)
        {
            return View();
        }

        [HttpPost]
        [CSPValidateHttpAntiForgeryToken]
        public async Task<ActionResult> PassScore(InquiryModel model)
        {
            var rtn = new ResultInfo();
            ExamPassInfoModel result = null;
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
                        var response = await client.GetAsync(api + "Inquiry/ExamPassInfo?testTypeID=" + model.TestTypeID + "&citizenID=" + model.CitizenID);
                        result = await response.Content.ReadAsAsync<ExamPassInfoModel>();
                    }
                }
                catch (Exception ex)
                {
                    rtn.Success = false;
                    rtn.ErrorMessage = ex.Message;
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

        [CalendarOpenFilter(CalendarCode = "SEARCH_SCORE", SystemType = "MASTER")]
        public ActionResult Score(int? testTypeID)
        {
            return View();
        }

        [HttpPost]
        [CSPValidateHttpAntiForgeryToken]
        public async Task<ActionResult> Score(InquiryModel model)
        {
            var rtn = new ResultInfo();
            ExamPassInfoModel result = null;
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
                        var response = await client.GetAsync(api + "Inquiry/ExamPassInfo?testTypeID=" + model.TestTypeID + "&citizenID=" + model.CitizenID + "&mobile=" + model.Mobile);
                        result = await response.Content.ReadAsAsync<ExamPassInfoModel>();
                    }
                }
                catch (Exception ex)
                {
                    rtn.Success = false;
                    rtn.ErrorMessage = ex.Message;
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