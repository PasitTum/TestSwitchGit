using CSP.Lib.Models;
using CSP.Lib.Mvc;
using Newtonsoft.Json.Linq;
using Register.DAL;
using Register.Models;
using Register.WebAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace Register.WebAPI.Controllers
{
    [RoutePrefix("api/SMS")]
    public class SMSController : ApiController
    {
        [HttpGet]
        [Route("SMSStatus")]
        public async Task<IHttpActionResult> SMSStatus(int testTypeID, string citizenID)
        {
            var model = new SmsInfo
            {
                TestTypeID = testTypeID,
                CitizenID = citizenID
            };
            var dal = new SMSDAL();
            var result = this.RequestStatusToSmsApi(model);
            var resultSms = await dal.SMSStatus(result);

            return Json(resultSms);
        }

        [HttpPost]
        [CSPValidateHttpAntiForgeryToken]
        [Route("SMSRegister")]
        public IHttpActionResult SMSRegister(SmsInfo model)
        {
            var rtn = new ResultInfo();
            var dal = new RegisterDAL();
            var result = this.RequestRegisterToSmsApi(model);
            var resultSmsLog = dal.InsertSmsLog(result);
            rtn.Success = result.Success;
            rtn.ErrorMessage = result.ErrorMessage;
            //rtn.Success = true;
            return Json(rtn);
        }

        private SmsInfo RequestStatusToSmsApi(SmsInfo model)
        {
            SmsInfo result = null;
            string smsPaymentStatus = SysParameterHelper.SMSPaymentStatus;

            var smsProjectID = SysParameterHelper.SMSProjectID;
            var smsuser = SysParameterHelper.SMSUser;
            var smspassword = SysParameterHelper.SMSPassword;
            var smsApiKey = SysParameterHelper.SMSApiKey;

            model.ProjectID = smsProjectID;
            model.Username = smsuser;
            model.Password = smspassword;

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("X-API-KEY", smsApiKey);
                var response = client.PostAsJsonAsync(smsPaymentStatus, model).Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.
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
                        result = model;
                        result.RefNo1 = returnValue2;  //Ref1
                        result.MemberID = returnValue1;
                        result.Success = status;
                        result.ErrorCode = errCode;
                        result.ErrorMessage = errMessage[0];
                    }
                    else
                    {
                        result = model;
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

        private SmsInfo RequestRegisterToSmsApi(SmsInfo model)
        {
            SmsInfo result = null;
            string SMSRegisterService = SysParameterHelper.SMSRegisterService;

            var smsProjectID = SysParameterHelper.SMSProjectID;
            var smsuser = SysParameterHelper.SMSUser;
            var smspassword = SysParameterHelper.SMSPassword;
            var smsApiKey = SysParameterHelper.SMSApiKey;
            model.ProjectID = smsProjectID;
            model.RefNo2 = model.PhoneNo;
            model.Email = "";
            model.RefNo1 = "";
            model.Amount = "0";
            model.AllowPrivacy = true;
            model.Username = smsuser;
            model.Password = smspassword;
            model.MemberID = "";
            model.RegisterCspSms = true;
            //var smsInfo = new SmsInfo
            //{
            //    ProjectID = smsProjectID,
            //    PhoneNo = model.SMSMobile,
            //    FName = model.FirstName,
            //    LName = model.LastName,
            //    Email = "",
            //    RefNo1 = "",
            //    RefNo2 = model.SMSMobile,
            //    CitizenID = model.CitizenID,
            //    Amount = "0",
            //    AllowPrivacy = true,
            //    Username = smsuser,
            //    Password = smspassword,
            //    MemberID = "",
            //    ConsentAcceptDatetime = model.ConsentDate,
            //    TestTypeID = model.TestTypeID
            //};

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("X-API-KEY", smsApiKey);
                var response = client.PostAsJsonAsync(SMSRegisterService, model).Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.
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
                        result = model;
                        result.RefNo1 = returnValue2;  //Ref1
                        result.MemberID = returnValue1;
                        result.Success = status;
                        result.ErrorCode = errCode;
                        result.ErrorMessage = errMessage[0];
                    }
                    else
                    {
                        result = model;
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
