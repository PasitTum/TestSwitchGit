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
using System.Data;

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
            byte[] result = null;
            var api = SysParameterHelper.ApiUrlServerSide;
            var jsonResult = string.Empty;
            var localFile = "";

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
                result = rpt.GetReport();
                localFile = String.Format("Payin_{0}_{1}.pdf", citizenID, DateTime.Now.ToString("yyyyMMddHHmmss"));
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, ex);
                throw (ex);
            }

            var datas = (JObject)JsonConvert.DeserializeObject(jsonResult);
            var smsStatus = (datas?.GetValueIgnoreCase("SMS_STATUS")).ToBoolean();
            if (smsStatus)
            {
                SMSPayinReport rptSub = new SMSPayinReport();
                try
                {
                    byte[] resultSub = null;
                    rptSub.IsTesting = SysParameterHelper.IsTesting;
                    rptSub.JsonData = jsonResult;
                    rptSub.RefreshDataSource();

                     resultSub = rptSub.GetReport();
                    if (resultSub !=null)
                    {
                        // Merge 2 ใบเข้าด้วยกัน
                        var docs = new byte[][] { result, resultSub };
                        var newTarget = (localFile as string).Replace(".pdf", "WithSMS.pdf");

                        var mergePdf = PdfHelper.MergePDFs(docs, newTarget);
                        if(mergePdf != null)
                        {
                            result = mergePdf;
                            localFile = newTarget;
                        }
                        //if (PdfHelper.MergePDFs(docs, newTarget))
                        //{
                        //    localFile = newTarget;
                        //}

                    }
                }
                catch (Exception ex)
                {
                    Log.WriteErrorLog(tsw.TraceError, ex);
                    throw (ex);
                }
            }

            if (result == null)
            {
                return Content("ไม่มีข้อมูล");
            }
            return File(result, "application/pdf", localFile);
        }

        [HttpPost]
        [CSPValidateHttpAntiForgeryToken]
        public async Task<ActionResult> PrintExamCard(int testTypeID, string citizenID)
        {
            byte[] result = null;
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
                result = rpt.GetReport();

            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, ex);
                throw (ex);
            }

            if (result == null)
            {
                return Content("ไม่มีข้อมูล");
            }
            return File(result, "application/pdf", String.Format("Application_{0}_{1}.pdf", citizenID, DateTime.Now.ToString("yyyyMMddHHmmss")));
        }



        [HttpPost]
        [CSPValidateHttpAntiForgeryToken]
        public async Task<ActionResult> PrintApplication(int testTypeID, string citizenID, string laserCode)
        {
            byte[] result = null;
            var api = SysParameterHelper.ApiUrlServerSide;
            var jsonResult = string.Empty;

            ApplicationReport rpt = new ApplicationReport(testTypeID);
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = await client.GetAsync(api + "Inquiry/ExamCrystalApplication?testTypeID=" + testTypeID + "&citizenID=" + citizenID + "&laserCode=" + laserCode);
                    jsonResult = await response.Content.ReadAsStringAsync();
                    DataSet ds = JsonConvert.DeserializeObject<DataSet>(jsonResult);
                    ds.Tables[0].TableName = "MainInfo";
                    ds.Tables[1].TableName = "SubStudy";
                    ds.Tables[2].TableName = "SubExperience";
                    rpt.ds = ds;
                    rpt.JsonData = jsonResult;
                    rpt.RefreshDataSource();
                    rpt.ImageByte = GetImageByteForReport(testTypeID, citizenID);
                }
                result = rpt.GetCrystalReport(citizenID);

                if (result == null)
                {
                    return Content("ไม่มีข้อมูล"); 
                }
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, ex);
                return Content("ไม่มีข้อมูล");
            }
            return File(result, "application/pdf", String.Format("Application_{0}_{1}.pdf", citizenID, DateTime.Now.ToString("yyyyMMddHHmmss")));

        }

        //[HttpPost]
        //[CSPValidateHttpAntiForgeryToken]
        //public async Task<ActionResult> PrintApplication(int testTypeID, string citizenID, string mobileNo)
        //{
        //    ResultInfo result = new ResultInfo();
        //    var reportOutputFolder = "~/temp";
        //    var api = SysParameterHelper.ApiUrlServerSide;
        //    var jsonResult = string.Empty;

        //    ApplicationReport rpt = new ApplicationReport(testTypeID); 
        //    try
        //    { 
        //        using (var client = new HttpClient())
        //        {
        //            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //            var response = await client.GetAsync(api + "Inquiry/ExamApplication?testTypeID=" + testTypeID + "&citizenID=" + citizenID + "&laserCode=" + "" + "&mobileNo=" + mobileNo);
        //            jsonResult = await response.Content.ReadAsStringAsync();
        //            rpt.IsTesting = SysParameterHelper.IsTesting;
        //            rpt.JsonData = jsonResult;
        //            rpt.RefreshDataSource();
        //            rpt.ImageBase64 = GetImageBase64ForReport(testTypeID, citizenID);
        //        }
        //        result = rpt.GetReport(reportOutputFolder);
        //    }
        //    catch (Exception ex)
        //    {
        //        result.Success = false;
        //        result.ErrorMessage = ex.Message;
        //    }

        //    if (result.Success)
        //    {
        //        var pdfName = result.ReturnValue1 as string;
        //        var clientFileName = System.IO.Path.GetFileName(pdfName);
        //        Response.ContentType = "application/pdf";
        //        Response.AddHeader("content-disposition", "attachment; filename=" + clientFileName);
        //        Response.TransmitFile(pdfName);
        //    }

        //    return Content(result?.ErrorMessage);
        //}

        [HttpPost]
        [CSPValidateHttpAntiForgeryToken]
        public async Task<ActionResult> PrintSmsPayIn(int testTypeID, string citizenID)
        {
            byte[] result = null;
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
                result = rpt.GetReport();

            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, ex);
                throw (ex);
            }

            if (result == null)
            {
                return Content("ไม่มีข้อมูล");
            }
            return File(result, "application/pdf", String.Format("SmsPayin_{0}_{1}.pdf", citizenID, DateTime.Now.ToString("yyyyMMddHHmmss")));
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

        public byte[] GetImageByteForReport(int testTypeID, string citizenID)
        {
            var domain = SysParameterHelper.PhotoDomain;
            var user = SysParameterHelper.PhotoUser;
            var password = SysParameterHelper.PhotoPassword;
            var destPath = string.Format(@"{0}\{3}\{2}\{1}.jpg", SysParameterHelper.PhotoImagePath, citizenID, citizenID.Right(3), testTypeID.ToString());
            byte[] convert = new byte[] { };
            try
            {
                //Log.WriteInformationLog(tsw.TraceInfo, string.Format("Start get image {0:dd-MM-yyyy HH:mm:ss.fff}", System.DateTime.Now));
                convert = CSP.Lib.NetworkFile.NetworkFileHelper.GetBytesFromNetwork(domain, user, password, destPath);
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