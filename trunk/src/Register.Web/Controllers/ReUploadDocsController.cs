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
    public class ReUploadDocsController : BaseController
    {
        [CalendarOpenFilter(CalendarCode = "REUPLOAD_DOC", SystemType = "MASTER")]
        public ActionResult Index(int? testTypeID)
        {
            return View();
        }

        [HttpPost]
        [CSPValidateHttpAntiForgeryToken]
        public async Task<ActionResult> Index(InquiryModel model)
        {
            var rtn = new ResultInfo();
            DocByCitizenModel result = null;
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
                    //using (var client = new HttpClient())
                    //{
                    //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //    var url = string.Format(api + "ReUploadDocs/{0}/Status/{1}", model.TestTypeID, model.CitizenID);
                    //    var response = await client.GetAsync(url);
                    //    response.EnsureSuccessStatusCode();
                    //    result = await response.Content.ReadAsAsync<DocByCitizenModel>();
                    //}
                    result = await this.GetStatusAsync(model);
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
        public async Task<ActionResult> ConfirmUpload(NewUploadDocModel model)
        {
            var rtn = new ResultInfo();
            DocByCitizenModel result = null;

            // Check อีกทีว่า อับโหลด require เอกสารมาครบไหม
            var passed = false;
            if (model.DOCS.Count > 0)
            {
                passed = !model.DOCS.Exists(x => x.REQUIRE_FLAG == "Y" && x.DOC_GUID == null);
            }
            if (!passed)
            {
                rtn.Success = false;
                rtn.ErrorMessage = "กรุณาอัปโหลดเอกสารที่จำเป็นให้ครบถ้วน";
                return Json(new
                {
                    rtn,
                    Data = result
                });
            }
            foreach (var doc in model.DOCS)
            {
                if (doc.FILE_NAME != null)
                {
                    doc.FILE_NAME = System.Web.HttpUtility.UrlEncode(doc.FILE_NAME);
                }
            }

            ResultInfo resultRenew = new ResultInfo();
            
            var api = SysParameterHelper.ApiUrlServerSide;
            model.IPAddress = SystemHelper.GetClientIP();
            model.MacAddress = SystemHelper.GetServerIP();
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = await client.PostAsJsonAsync<NewUploadDocModel>(api + "/ReUploadDocs", model);
                    response.EnsureSuccessStatusCode();
                    resultRenew = await response.Content.ReadAsAsync<ResultInfo>() ?? new ResultInfo();
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
                    Data = result
                });
            }

            if (resultRenew != null && resultRenew.Success)
            {
                rtn.Success = true;
                try
                {
                    result = await this.GetStatusAsync(model);
                }
                catch (Exception ex)
                {
                    Log.WriteErrorLog(tsw.TraceError, ex);
                }                
            }
            else
            {
                rtn.Success = false;
                rtn.ErrorMessage = resultRenew.ErrorMessage;
            }

            return Json(new
            {
                rtn,
                Data = result
            });
        }

        private async Task<DocByCitizenModel> GetStatusAsync(InquiryModel model)
        {
            DocByCitizenModel result = null;
            var api = SysParameterHelper.ApiUrlServerSide;
            using (var client = new HttpClient())
            {
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //var url = string.Format(api + "ReUploadDocs/{0}/Status/{1}", testtypeId, citizenId);
                //var response = await client.GetAsync(url);
                //response.EnsureSuccessStatusCode();
                //result = await response.Content.ReadAsAsync<DocByCitizenModel>();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.PostAsJsonAsync<InquiryModel>(api + "ReUploadDocs/Status", model);
                response.EnsureSuccessStatusCode();
                result = await response.Content.ReadAsAsync<DocByCitizenModel>();
            }
            return result;
        }
    }
}