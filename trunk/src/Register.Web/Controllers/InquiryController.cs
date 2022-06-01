using CaptchaMvc.HtmlHelpers;
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
using CSP.Lib.Diagnostic;

namespace Register.Web.Controllers
{
    public class InquiryController : BaseController
    {
        // GET: Pencil/Inquiry
        public ActionResult Index()
        {
            return View();
        }

        [CalendarOpenFilter(CalendarCode = "PRINT_PAYIN", SystemType = "MASTER")]
        public ActionResult PaymentStatus(int? testTypeID)
        {
            return View();
        }

        [HttpPost]
        [CSPValidateHttpAntiForgeryToken]
        public async Task<ActionResult> PaymentStatus(InquiryModel model)
        {
            var rtn = new ResultInfo();
            PaymentStatusModel result = null;
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
                        var response = await client.GetAsync(api + "Inquiry/PaymentStatus?testTypeID=" + model.TestTypeID + "&citizenID=" + model.CitizenID);
                        response.EnsureSuccessStatusCode();
                        result = await response.Content.ReadAsAsync<PaymentStatusModel>();
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

        [CalendarOpenFilter(CalendarCode = "ENROLL_CHECK_STATUS", SystemType = "MASTER")]
        public ActionResult RegisterStatus(int? testTypeID)
        {
            return View();
        }

        [HttpPost]
        [CSPValidateHttpAntiForgeryToken]
        public async Task<ActionResult> RegisterStatus(InquiryModel model)
        {
            var rtn = new ResultInfo();
            RegisterStatusModel result = null;
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
                        var response = await client.GetAsync(api + "Inquiry/RegisterStatus?testTypeID=" + model.TestTypeID + "&citizenID=" + model.CitizenID);
                        response.EnsureSuccessStatusCode();
                        result = await response.Content.ReadAsAsync<RegisterStatusModel>();
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

        [CalendarOpenFilter(CalendarCode = "SEARCH_SEAT_NO", SystemType = "MASTER")]
        public ActionResult EnrollInfo(int? testTypeID)
        {
            return View();
        }

        [HttpPost]
        [CSPValidateHttpAntiForgeryToken]
        public async Task<ActionResult> EnrollInfo(InquiryModel model)
        {
            var rtn = new ResultInfo();
            EnrollInfoModel result = null;
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
                        var response = await client.GetAsync(api + "Inquiry/EnrollInfo?testTypeID=" + model.TestTypeID + "&citizenID=" + model.CitizenID);
                        response.EnsureSuccessStatusCode();
                        result = await response.Content.ReadAsAsync<EnrollInfoModel>();
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

        [CalendarOpenFilter(CalendarCode = "PRINT_EXAM_APPLICATION", SystemType = "MASTER")]
        public ActionResult ExamApplication(int? testTypeID)
        {
            return View();
        }

        [HttpPost]
        [CSPValidateHttpAntiForgeryToken]
        public async Task<ActionResult> ExamApplication(InquiryModel model)
        {
            //model.LaserCode = "";
            var rtn = new ResultInfo();
            ExamApplicationModel result = null;
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
                        var response = await client.GetAsync(api + "Inquiry/ExamApplication?testTypeID=" + model.TestTypeID + "&citizenID=" + model.CitizenID + "&mobileNo=" + model.Mobile);
                        response.EnsureSuccessStatusCode();
                        result = await response.Content.ReadAsAsync<ExamApplicationModel>();
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

        [CalendarOpenFilter(CalendarCode = "GET_EXAM_SITE_PRINT_EXAM_CARD", SystemType = "MASTER")]
        public ActionResult SeatInfo(int? testTypeID)
        {
            return View();
        }

        [HttpPost]
        [CSPValidateHttpAntiForgeryToken]
        public async Task<ActionResult> SeatInfo(InquiryModel model)
        {
            var rtn = new ResultInfo();
            ExamSiteInfoModel result = null;
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
                        var response = await client.GetAsync(api + "Inquiry/ExamSiteInfo?testTypeID=" + model.TestTypeID + "&citizenID=" + model.CitizenID);
                        response.EnsureSuccessStatusCode();
                        result = await response.Content.ReadAsAsync<ExamSiteInfoModel>();
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

        [CalendarOpenFilter(CalendarCode = "REFUND_REPEATED_PAYMENTS", SystemType = "MASTER")]
        public ActionResult PaymentRepeated(int? testTypeID)
        {
            return View();
        }

        [HttpPost]
        [CSPValidateHttpAntiForgeryToken]
        public async Task<ActionResult> PaymentRepeated(InquiryModel model)
        {
            var rtn = new ResultInfo();
            List<PaymentRepeatedModel> result = null;
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
                        var response = await client.GetAsync(api + "Inquiry/PaymentRepeated?testTypeID=" + model.TestTypeID + "&citizenID=" + model.CitizenID + "&laserCode=" + model.LaserCode);
                        response.EnsureSuccessStatusCode();
                        result = await response.Content.ReadAsAsync<List<PaymentRepeatedModel>>();
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

        [HttpPost]
        [CSPValidateHttpAntiForgeryToken]
        public async Task<ActionResult> RefundPayment(RefundPaymentModel model)
        {
            ResultInfo rtn = new ResultInfo();
            ResultInfo resultRefund = new ResultInfo();
            List<PaymentRepeatedModel> lst = null;
            var api = SysParameterHelper.ApiUrlServerSide;

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = await client.PostAsJsonAsync<RefundPaymentModel>(api + "Inquiry/InsertRefundPayment", model);
                    response.EnsureSuccessStatusCode();
                    resultRefund = await response.Content.ReadAsAsync<ResultInfo>() ?? new ResultInfo();
                }
            }
            catch (Exception ex)
            {
                rtn.Success = false;
                rtn.ErrorMessage = ex.Message;
                Log.WriteErrorLog(tsw.TraceError, ex);
                return Json(new
                {
                    rtn,
                    Data = lst
                });
            }

            if (resultRefund != null || resultRefund.Success)
            {
                rtn.Success = true;
                try
                {
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var response = await client.GetAsync(api + "Inquiry/PaymentRepeated?testTypeID=" + model.TestTypeID + "&citizenID=" + model.CitizenID + "&laserCode=" + model.LaserCode);
                        response.EnsureSuccessStatusCode();
                        lst = await response.Content.ReadAsAsync<List<PaymentRepeatedModel>>();
                    }
                }
                catch (Exception ex)
                {
                    rtn.Success = false;
                    rtn.ErrorMessage = ex.Message;
                    Log.WriteErrorLog(tsw.TraceError, ex);
                }
            }
            else
            {
                rtn.Success = false;
                rtn.ErrorMessage = resultRefund.ErrorMessage;
            }

            return Json(new
            {
                rtn,
                Data = lst
            });
        }

       

      

      
    }
}