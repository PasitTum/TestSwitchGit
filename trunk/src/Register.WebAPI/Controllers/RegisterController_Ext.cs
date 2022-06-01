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
using Register.DOPA.Biz;
using CSP.Lib.Diagnostic;
using Newtonsoft.Json;

namespace Register.WebAPI.Controllers
{
    public partial class RegisterController : BaseApiController
    {
        [HttpPost]
        [Route("VaidateCitizenDOPA")]
        // TODO : จะตรวจยังไงว่ามาจากฝั่ง Server เท่านั้น
        public async Task<IHttpActionResult> VaidateCitizenDOPA(RegisterModel model)
        {
            var result = new ResultInfo();
            DopaResultModel dopaResult = null;
            var logModel = new DopaLogModel()
            {
                USER_ID = model.CitizenID,
                IP_ADDRESS = model.IPAddress,
                MAC_ADDRESS = string.IsNullOrEmpty(model.MacAddress) ? SystemHelper.GetServerIP() : model.MacAddress,
                LOG_TYPE = "VALIDATE",
                PROGRAM_ID = "VaidateCitizenDOPA",
                LOG_DESC = JsonConvert.SerializeObject(new
                {
                    CID = model.CitizenID,
                    firstName = model.FirstName,
                    lastName = model.LastName,
                    birthDay = model.BirthDateChar,
                    laser = model.LaserCode
                })
            };
            try
            {
                var biz = new DOPABiz();
                dopaResult = await biz.VerifyCitizenCard(model.CitizenID, model.FirstName, model.LastName, model.BirthDateChar, model.LaserCode);
                result.Success = (!dopaResult.IsError && (dopaResult.Code == 0));
                if (!result.Success)
                {
                    result.ErrorMessage = "ข้อมูลบัตรประชาชนไม่ถูกต้อง";
                }
                logModel.RESULT_STATUS = (result.Success ? "1" : "0");
                logModel.RESULT_DESC = JsonConvert.SerializeObject(dopaResult);
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, ex);
                result.Success = false;
                result.ErrorMessage = "ขณะนี้การเชื่อมต่อฐานข้อมูลทะเบียนราษฎร์ขัดข้อง กรุณารอสักครู่ แล้วลองใหม่อีกครั้ง";
                logModel.RESULT_STATUS = "-1";
                logModel.RESULT_DESC = ExceptionHelper.GetFullErrorMessage(ex);
            }
            finally
            {
                var dal = new DAL.DopaDAL();
                await dal.AddLogValidateDopaAsync(logModel);
            }
            return Ok(result);
        }
    }
}
