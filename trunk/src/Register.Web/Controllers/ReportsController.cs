using CSP.Lib.Extension;
using CSP.Lib.Json;
using CSP.Lib.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Register.Reports;
using Register.Reports.Helpers;
using Register.Web.Helpers;
using System;
using io = System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CSP.Lib.Mvc;
using CSP.Lib.Diagnostic;

namespace Register.Web.Controllers
{
    public class ReportsController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
   
        [HttpPost]
        [CSPValidateHttpAntiForgeryToken]
        public async Task<ActionResult> PrintPayin(int testTypeID, string citizenID)
        {
            ResultInfo result = new ResultInfo();
            var reportOutputFolder = "~/temp";
            var api = SysParameterHelper.ApiUrlServerSide;
            var jsonResult = string.Empty;

            PayinReport rpt = new PayinReport();
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = await client.GetAsync(api + "Reports/Payin?testTypeID=" + testTypeID + "&citizenID=" + citizenID);
                    response.EnsureSuccessStatusCode();
                    jsonResult = await response.Content.ReadAsStringAsync();
                    rpt.IsTesting = SysParameterHelper.IsTesting;
                    rpt.JsonData = jsonResult;
                    rpt.RefreshDataSource();
                }
                result = rpt.GetReport(reportOutputFolder);
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, ex);
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }

            var datas = (JObject)JsonConvert.DeserializeObject(jsonResult);
            var smsStatus = (datas?.GetValueIgnoreCase("SMS_STATUS")).ToBoolean();
            if (smsStatus)
            {
                SMSPayinReport rptSub = new SMSPayinReport();
                try
                {
                    rptSub.IsTesting = SysParameterHelper.IsTesting;
                    rptSub.JsonData = jsonResult;
                    rptSub.RefreshDataSource();

                    var resultSub = rptSub.GetReport(reportOutputFolder);
                    if (resultSub.Success)
                    {
                        // Merge 2 ใบเข้าด้วยกัน
                        var docs = new string[] { (string)result.ReturnValue1, (string)resultSub.ReturnValue1 };
                        var newTarget = (result.ReturnValue1 as string).Replace(".pdf", "WithSMS.pdf");
                        if (PdfHelper.MergePDFs(docs, newTarget))
                        {
                            result.ReturnValue1 = newTarget;
                        }
                    }
                }
                catch (Exception ex)
                {
                    result.Success = false;
                    result.ErrorMessage = ex.Message;
                }
            }

            if (result.Success)
            {
                var pdfName = result.ReturnValue1 as string;
                var clientFileName = System.IO.Path.GetFileName(pdfName);
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment; filename=" + clientFileName);
                Response.TransmitFile(pdfName);
            }

            return Content(result?.ErrorMessage);
        }

        [HttpPost]
        [CSPValidateHttpAntiForgeryToken]
        public async Task<ActionResult> PrintExamCard(int testTypeID, string citizenID)
        {
            ResultInfo result = new ResultInfo();
            var reportOutputFolder = "~/temp";
            var api = SysParameterHelper.ApiUrlServerSide;
            var jsonResult = string.Empty;

            ExamCardReport rpt = new ExamCardReport();
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = await client.GetAsync(api + "Inquiry/ExamSiteInfo?testTypeID=" + testTypeID + "&citizenID=" + citizenID);
                    jsonResult = await response.Content.ReadAsStringAsync();
                    rpt.IsTesting = SysParameterHelper.IsTesting;
                    rpt.JsonData = jsonResult;
                    rpt.RefreshDataSource();
                    rpt.ImageBase64 = GetImageBase64ForReport(testTypeID, citizenID);
                }
                result = rpt.GetReport(reportOutputFolder);

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }

            if (result.Success)
            {
                var pdfName = result.ReturnValue1 as string;
                var clientFileName = System.IO.Path.GetFileName(pdfName);
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment; filename=" + clientFileName);
                Response.TransmitFile(pdfName);
            }

            return Content(result?.ErrorMessage);
        }

        [HttpPost]
        [CSPValidateHttpAntiForgeryToken]
        public async Task<ActionResult> PrintApplication(int testTypeID, string citizenID, string mobileNo)
        {
            ResultInfo result = new ResultInfo();
            var reportOutputFolder = "~/temp";
            var api = SysParameterHelper.ApiUrlServerSide;
            var jsonResult = string.Empty;

            ApplicationReport rpt = new ApplicationReport(testTypeID); 
            try
            { 
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = await client.GetAsync(api + "Inquiry/ExamApplication?testTypeID=" + testTypeID + "&citizenID=" + citizenID + "&laserCode=" + "" + "&mobileNo=" + mobileNo);
                    jsonResult = await response.Content.ReadAsStringAsync();
                    rpt.IsTesting = SysParameterHelper.IsTesting;
                    rpt.JsonData = jsonResult;
                    rpt.RefreshDataSource();
                    rpt.ImageBase64 = GetImageBase64ForReport(testTypeID, citizenID);
                }
                result = rpt.GetReport(reportOutputFolder);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }

            if (result.Success)
            {
                var pdfName = result.ReturnValue1 as string;
                var clientFileName = System.IO.Path.GetFileName(pdfName);
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment; filename=" + clientFileName);
                Response.TransmitFile(pdfName);
            }

            return Content(result?.ErrorMessage);
        }

        [HttpPost]
        [CSPValidateHttpAntiForgeryToken]
        public async Task<ActionResult> PrintSmsPayIn(int testTypeID, string citizenID)
        {
            ResultInfo result = new ResultInfo();
            var reportOutputFolder = "~/temp";
            var api = SysParameterHelper.ApiUrlServerSide;
            var jsonResult = string.Empty;

            SMSPayinReport rpt = new SMSPayinReport();
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = await client.GetAsync(api + "Reports/Payin?testTypeID=" + testTypeID + "&citizenID=" + citizenID);
                    response.EnsureSuccessStatusCode();
                    jsonResult = await response.Content.ReadAsStringAsync();
                    rpt.IsTesting = SysParameterHelper.IsTesting;
                    rpt.JsonData = jsonResult;
                    rpt.RefreshDataSource();
                }
                result = rpt.GetReport(reportOutputFolder);

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }


            if (result.Success)
            {
                var pdfName = result.ReturnValue1 as string;
                var clientFileName = System.IO.Path.GetFileName(pdfName);
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment; filename=" + clientFileName);
                Response.TransmitFile(pdfName);
            }

            return Content(result?.ErrorMessage);
        }

        public string GetImageBase64ForReport(int testTypeID, string citizenID)
        {
            var domain = SysParameterHelper.PhotoDomain;
            var user = SysParameterHelper.PhotoUser;
            var password = SysParameterHelper.PhotoPassword;
            var destPath = string.Format(@"{0}\{1}\{2}\{3}.jpg", SysParameterHelper.PhotoImagePath, testTypeID, citizenID.Right(3), citizenID);
            string convert = "";
            try
            {
                //Log.WriteInformationLog(tsw.TraceInfo, string.Format("Start get image {0:dd-MM-yyyy HH:mm:ss.fff}", System.DateTime.Now));
                convert = CSP.Lib.NetworkFile.NetworkFileHelper.GetBase64FromNetwork(domain, user, password, destPath);
                //Log.WriteInformationLog(tsw.TraceInfo, string.Format("End get image {0:dd-MM-yyyy HH:mm:ss.fff}", System.DateTime.Now));
            }
            catch (Exception ex)
            {
                // TODO : อาจจะต้องเอา Log ที่ Save File ไม่สำเร็จใส่ไว้ใน Table ด้วย ไม่งั้นหายากมาก
                //Log.WriteInformationLog(tsw.TraceInfo, string.Format("Error get image {0:dd-MM-yyyy HH:mm:ss.fff}", System.DateTime.Now));
                Log.WriteErrorLog(tsw.TraceError, ex);
            }
            return convert;
        }
    }
}