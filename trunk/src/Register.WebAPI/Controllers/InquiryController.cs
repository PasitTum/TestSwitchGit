using CSP.Lib.Models;
using Register.DAL;
using CSP.Lib.Extension;
using Register.Models;
using Register.WebAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using CSP.Lib.Diagnostic;

namespace Register.WebAPI.Controllers
{
    [RoutePrefix("api/Inquiry")]
    public class InquiryController : BaseApiController
    {
        [HttpGet]
        [Route("PaymentStatus")]
        public async Task<IHttpActionResult> PaymentStatus(int testTypeID, string citizenID)
        {
            var dal = new InquiryDAL();
            var lst = await dal.PaymentStatus(testTypeID, citizenID);
            return Json(lst);
        }

        [HttpGet]
        [Route("RegisterStatus")]
        public async Task<IHttpActionResult> RegisterStatus(int testTypeID, string citizenID)
        {
            var dal = new InquiryDAL();
            var lst = await dal.RegisterStatus(testTypeID, citizenID);
            return Json(lst);
        }

        [HttpGet]
        [Route("EnrollInfo")]
        public async Task<IHttpActionResult> EnrollInfo(int testTypeID, string citizenID)
        {
            var dal = new InquiryDAL();
            var lst = await dal.EnrollInfo(testTypeID, citizenID);
            return Json(lst);
        }


        [HttpGet]
        [Route("ExamApplication")]
        public async Task<IHttpActionResult> ExamApplication(int testTypeID, string citizenID, string mobileNo)
        {
            var dal = new InquiryDAL();
            var lst = await dal.ExamApplication(testTypeID, citizenID, mobileNo);
            return Json(lst);
        }

        [HttpGet]
        [Route("UploadStatus")]
        public async Task<IHttpActionResult> UploadStatus(int testTypeID, string citizenID)
        {
            var dal = new InquiryDAL();
            var lst = await dal.UploadStatus(testTypeID, citizenID);
            lst.PHOTO_FILE_BASE64 = GetImageBase64(citizenID, testTypeID);
            return Json(lst);
        }

        [HttpGet]
        [Route("ExamSiteInfo")]
        public async Task<IHttpActionResult> ExamSiteInfo(int testTypeID, string citizenID)
        {
            var dal = new InquiryDAL();
            var lst = await dal.ExamSiteInfo(testTypeID, citizenID);
            return Json(lst);
        }

        [HttpGet]
        [Route("ExamPassInfo")]
        public async Task<IHttpActionResult> ExamPassInfo(int testTypeID, string citizenID, string mobile)
        {
            var dal = new InquiryDAL();
            var lst = await dal.ExamPassInfo(testTypeID, citizenID, mobile);
            return Json(lst);
        }

        [HttpGet]
        [Route("Score")]
        public async Task<IHttpActionResult> Score(int testTypeID, string citizenID, string password)
        {
            var dal = new InquiryDAL();
            var lst = await dal.Score(testTypeID, citizenID, password);
            return Json(lst);
        }

        [HttpGet]
        [Route("PaymentRepeated")]
        public async Task<IHttpActionResult> PaymentRepeated(int testTypeID, string citizenID, string laserCode)
        {
            var dal = new InquiryDAL();
            var lst = await dal.PaymentRepeated(testTypeID, citizenID, laserCode);
            return Json(lst);
        }

        [HttpPost]
        [Route("InsertRefundPayment")]
        public IHttpActionResult InsertRefundPayment(RefundPaymentModel model)
        {
            var dal = new InquiryDAL();
            var result = dal.InsertRefundPayment(model);
            return Json(result);
        }
      
        public string GetImageBase64(string citizenID, int testTypeID)
        {
            var domain = SysParameterHelper.PhotoDomain;
            var user = SysParameterHelper.PhotoUser;
            var password = SysParameterHelper.PhotoPassword;
            var destPath = string.Format(@"{0}\{3}\{2}\{1}.jpg", SysParameterHelper.PhotoImagePath, citizenID, citizenID.Right(3), testTypeID);
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
            return "data:image/jpeg;base64," + convert;
        }
    }
}
