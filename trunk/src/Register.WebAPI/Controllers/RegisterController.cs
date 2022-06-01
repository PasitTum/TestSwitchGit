using Register.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Register.Models;
using System.Web;
using System.Web.Http;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.IO;
using Register.WebAPI.Helpers;
using CSP.Lib.Models;
using CSP.Lib.Extension;
using CSP.Lib.Mvc;
using CSP.Lib.Diagnostic;

namespace Register.WebAPI.Controllers
{
    [RoutePrefix("api/Register")]
    public partial class RegisterController : BaseApiController
    {
        [HttpPost]
        [Route("ValidateCitizenID")]
        //[CSPValidateHttpAntiForgeryToken]
        //[System.Web.Mvc.ValidateAntiForgeryToken]
        public async Task<IHttpActionResult> ValidateCitizenID(RegisterModel model)
        {
            var dal = new RegisterDAL();
            var lst = await dal.ValidateCitizenID(model.TestTypeID, model.ExamType, model.CitizenID, model.LaserCode);
            return Json(lst);
        }

        [HttpPost]
        //[CSPValidateHttpAntiForgeryToken]
        [Route("SetEnrollInfo")]
        public IHttpActionResult SetEnrollInfo(RegisterModel model)
        {
            var dal = new RegisterDAL();

            // TODO : Copy Docs from temp to storage

            // TODO : น่าจะ Copy File ภาพให้สำเร็จก่อน Save นะ
            if (model.DOCS != null)
            {
                foreach (var doc in model.DOCS)
                {
                    doc.DOC_PATH = string.Format(@"\{0}\{1}\{2}\", model.TestTypeID, model.CitizenID.Right(3), model.CitizenID);
                    doc.FILE_NAME = System.Net.WebUtility.UrlEncode(doc.FILE_NAME);
                }
            }

            var rtn = new ResultInfo();
            var domain = SysParameterHelper.PhotoDomain;
            var user = SysParameterHelper.PhotoUser;
            var password = SysParameterHelper.PhotoPassword;
            var destPath = string.Format(@"{0}\{3}\{2}\{1}.jpg", SysParameterHelper.PhotoImagePath, model.CitizenID, model.CitizenID.Right(3), model.TestTypeID);
            var srcPath = string.Format(@"{0}\{3}\{2}\{1}.jpg", SysParameterHelper.PhotoImageTempPath, model.CitizenID, model.CitizenID.Right(3), model.TestTypeID);
            try
            {
                //Log.WriteInformationLog(tsw.TraceInfo, string.Format("Start copy image {0:dd-MM-yyyy HH:mm:ss.fff}", System.DateTime.Now));
                CSP.Lib.NetworkFile.NetworkFileHelper.CopyOverNetwork(domain, user, password, srcPath, destPath);
                if (!CSP.Lib.NetworkFile.NetworkFileHelper.IsFileExists(domain, user, password, destPath))
                {
                    //rtn.Success = false;
                    rtn.ErrorMessage = string.Format("ไม่พบไฟล์รูปถ่าย ชื่อ {0}", destPath);
                    Log.WriteErrorLog(tsw.TraceError, rtn.ErrorMessage);
                    return Json(rtn);
                }

                //Log.WriteInformationLog(tsw.TraceInfo, string.Format("End copy image {0:dd-MM-yyyy HH:mm:ss.fff}", System.DateTime.Now));

                if (model.DOCS != null)
                {
                    foreach (var doc in model.DOCS)
                    {
                        if (doc.DOC_GUID != null)
                        {
                            srcPath = string.Format(@"{0}\{1}\{2}\{3}", SysParameterHelper.DocUploadTempPath, model.TestTypeID, doc.DOC_GUID.ToString().Right(3), doc.DOC_GUID);
                            destPath = string.Format(@"{0}\{1}\{2}\{3}\{4}", SysParameterHelper.DocUploadPath, model.TestTypeID, model.CitizenID.Right(3), model.CitizenID, doc.DOC_GUID);
                            CSP.Lib.NetworkFile.NetworkFileHelper.CopyOverNetwork(domain, user, password, srcPath, destPath, true);
                            if (!CSP.Lib.NetworkFile.NetworkFileHelper.IsFileExists(domain, user, password, destPath))
                            {
                                //rtn.Success = false;
                                rtn.ErrorMessage = string.Format("ไม่พบไฟล์เอกสาร ชื่อ {0}", destPath);
                                Log.WriteErrorLog(tsw.TraceError, rtn.ErrorMessage);
                                return Json(rtn);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // TODO : อาจจะต้องเอา Log ที่ Save File ไม่สำเร็จใส่ไว้ใน Table ด้วย ไม่งั้นหายากมาก
                //Log.WriteInformationLog(tsw.TraceInfo, string.Format("Error copy image {0:dd-MM-yyyy HH:mm:ss.fff}", System.DateTime.Now));
                Log.WriteErrorLog(tsw.TraceError, ex);
            }

            var result = dal.SetEnrollInfo(model);
            if (result.Success && model.SMSStatus == "Y")
            {
                model.RegisterNo = result.ReturnValue1;
                var resultSms = this.RequestRegisterToSmsApi(model);
                var resultSmsLog = dal.InsertSmsLog(resultSms);
                result.ErrorMessage = resultSms.ErrorMessage;
            }
            return Json(result);
        }

        [HttpGet]
        [Route("EnrollDate")]
        public IHttpActionResult EnrollDate(int testTypeID, string scheduleTypeCode)
        {
            var dal = new RegisterDAL();
            var rtn = dal.EnrollDate(testTypeID, scheduleTypeCode);
            return Json(rtn);
        }

        [HttpGet]
        [Route("QrcodeInfo")]
        public async Task<IHttpActionResult> QrcodeInfo(int testTypeID, string citizenID)
        {
            string QRCodeApiSMSFormatUrl = SysParameterHelper.QRCodeApiSMSFormatUrl;
            var dal = new ReportsDAL();
            var result = await dal.Payin(testTypeID, citizenID);
            var qrcode = "";
            var smsqrCode = "";
            if (result != null)
            {
                qrcode = string.Format(QRCodeApiSMSFormatUrl, result.TAX_ID + result.SUFFIX, result.REF1, result.REF2, result.AMOUNT.ToString("##0") + "00")
                    + "&fullname=" + HttpUtility.UrlEncode("ผู้สมัคร") + "%20%20" + HttpUtility.UrlEncode(result.TITLE_NAME_TH + result.FNAME_TH) + "%20" + HttpUtility.UrlEncode(result.LNAME_TH);
                if (result.SMS_STATUS == "Y")
                {
                    smsqrCode = string.Format(QRCodeApiSMSFormatUrl, result.SMS_TAX_ID + result.SMS_SUFFIX, result.SMS_REF_1, result.SMS_REF_2, result.SMS_AMOUNT?.ToString("##0") + "00")
                        + "&fullname=" + HttpUtility.UrlEncode("ผู้สมัคร") + "%20%20" + HttpUtility.UrlEncode(result.TITLE_NAME_TH + result.FNAME_TH) + "%20" + HttpUtility.UrlEncode(result.LNAME_TH);
                }
            }
            return Json(new
            {
                QRCode = qrcode,
                SmsStatus = result.SMS_STATUS,
                SMSQRCode = smsqrCode
            });
        }

        private SmsInfo RequestRegisterToSmsApi(RegisterModel model)
        {
            SmsInfo result = null;
            string SMSRegisterService = SysParameterHelper.SMSRegisterService;

            var smsProjectID = SysParameterHelper.SMSProjectID;
            var smsuser = SysParameterHelper.SMSUser;
            var smspassword = SysParameterHelper.SMSPassword;
            var smsApiKey = SysParameterHelper.SMSApiKey;
            var smsInfo = new SmsInfo
            {
                ProjectID = smsProjectID,
                PhoneNo = model.SMSMobile,
                FName = model.FirstName,
                LName = model.LastName,
                Email = "",
                RefNo1 = "",
                RefNo2 = model.SMSMobile,
                CitizenID = model.CitizenID,
                Amount = "0",
                AllowPrivacy = true,
                Username = smsuser,
                Password = smspassword,
                MemberID = "",
                ConsentAcceptDatetime = model.ConsentDate,
                TestTypeID = model.TestTypeID,
                RegisterCspSms = true //เรียกจากเมนู sms จะ true เสมอ เพราะสมัครแบบเสียตังค์
            };

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("X-API-KEY", smsApiKey);
                var response = client.PostAsJsonAsync(SMSRegisterService, smsInfo).Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.
                if (response.IsSuccessStatusCode)
                {
                    var resultSMS = response.Content.ReadAsStringAsync().Result;
                    JObject json = JObject.Parse(resultSMS);
                    bool status = (bool)json["Success"];
                    string errCode = (string)json["ErrorCode"];
                    JToken token = json["ErrorMessage"];
                    List<string> errMessage = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(token.ToString());
                    string returnValue1 = (string)json["ReturnValue1"];
                    string returnValue2 = (string)json["ReturnValue2"];

                    if (status)
                    {
                        result = smsInfo;
                        result.RefNo1 = returnValue2;  //Ref1
                        result.MemberID = returnValue1;
                        result.Success = status;
                        result.ErrorCode = errCode;
                        result.ErrorMessage = errMessage[0];
                    }
                    else
                    {
                        result = smsInfo;
                        result.RefNo1 = returnValue2;  //Ref1
                        result.MemberID = returnValue1;
                        result.Success = status;
                        result.ErrorCode = errCode;
                        result.ErrorMessage = errMessage[0];
                        //throw new InvalidDataException(errMessage[0]);
                    }
                }
                else
                {
                    throw new ApplicationException($"{response.StatusCode}-{response.ReasonPhrase}");
                }
            }

            return result;
        }
    }
}