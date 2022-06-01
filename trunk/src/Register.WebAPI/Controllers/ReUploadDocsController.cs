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
    [RoutePrefix("api/ReUploadDocs")]
    public class ReUploadDocsController : BaseApiController
    {
        [HttpPost]
        [Route("Status")]
        public async Task<IHttpActionResult> GetUploadDocsStatus(InquiryModel model)
        {
            var dal = new InquiryDAL();
            var lst = await dal.UploadDocsStatus(model);
            return Json(lst);
        }

        [HttpPost]
        [Route("DocsByCitizen")]
        public IHttpActionResult ListDocByCitizen(InquiryModel model)
        {
            var dal = new InquiryDAL();
            var lst = dal.ListDocByCitizen(model);
            if (lst != null)
            {
                var domain = SysParameterHelper.PhotoDomain;
                var user = SysParameterHelper.PhotoUser;
                var password = SysParameterHelper.PhotoPassword;
                var destPath = "";
                //var enrollNo = "";
                var citizenId = model.CitizenID;
                bool isExists = false;

                foreach (var x in lst)
                {
                    // น่าจะไปเช็คการคงอยู่ของไฟล์แล้วตอบกลับไปเพื่อแสดงสถานะด้วยก็ดีนะ
                    isExists = false;
                    if (x.UPLOAD_STATUS == "S")
                    {
                        try
                        {
                            //enrollNo = x.ENROLL_NO; 
                            destPath = string.Format(@"{0}\{1}\{2}", SysParameterHelper.DocUploadPath, x.DOC_PATH, x.DOC_GUID);
                            isExists = CSP.Lib.NetworkFile.NetworkFileHelper.IsFileExists(domain, user, password, destPath);
                        }
                        finally
                        {
                            if (!isExists)
                            {
                                x.UPLOAD_STATUS = "E";
                            }
                        }
                    }
                    // -----------------------------------------------

                    if (x.LAST_UPLOAD_FILENAME != null)
                    {
                        x.LAST_UPLOAD_FILENAME = System.Web.HttpUtility.UrlDecode(x.LAST_UPLOAD_FILENAME);
                    }

                    // เอามาใช้แค่เช็คไฟล์  ไม่ต้องตอบกลับลงไป  เดี๋ยวกระบวนการอับโหลดใหม่มันจะเพี้ยน
                    x.DOC_GUID = "";
                }
            }
            return Json(lst);
        }

        [HttpPost]
        public IHttpActionResult Post(NewUploadDocModel model)
        {
            // ถ้าทำเป็น Save สองทีล่ะ ทีแรก insert record เป็น pending ก่อน  แล้วพอ copy file สำเร็จค่อย update เป็น complete ดีไหมอ่ะ
            // 
            ResultInfo result = null;
            var dal = new InquiryDAL();

            if (model.DOCS != null)
            {
                foreach (var doc in model.DOCS)
                {
                    doc.DOC_PATH = string.Format(@"\{0}\{1}\{2}\", model.TestTypeID, model.CitizenID.Right(3), model.CitizenID);
                    //doc.FILE_NAME = System.Net.WebUtility.UrlEncode(doc.FILE_NAME);
                }
            }
            var domain = SysParameterHelper.PhotoDomain;
            var user = SysParameterHelper.PhotoUser;
            var password = SysParameterHelper.PhotoPassword;
            var destPath = "";
            var srcPath = "";
            try
            {
                // MUST !!!!!!!!!!!!!  ต้อง copy file เก่าไปเก็บใน history ก่อนนะ
                var citizenId = model.CitizenID;
                var oldPath = string.Format(@"{0}\{1}\{2}\{3}", SysParameterHelper.DocUploadPath, model.TestTypeID, citizenId.Right(3), citizenId);
                var histPath = string.Format(@"{0}\{1}\{2}\{3}\{4:yyyyMMddHHmm}", SysParameterHelper.DocUploadHistoryPath, model.TestTypeID, citizenId.Right(3), citizenId, DateTime.Now);
                CSP.Lib.NetworkFile.NetworkFileHelper.CopyFolder(domain, user, password, oldPath, histPath, true);

                if (model.DOCS != null)
                {
                    foreach (var doc in model.DOCS)
                    {
                        if (doc.DOC_GUID != null)
                        {
                            srcPath = string.Format(@"{0}\{1}\{2}\{3}", SysParameterHelper.DocUploadTempPath, model.TestTypeID, doc.DOC_GUID.ToString().Right(3), doc.DOC_GUID);
                            destPath = string.Format(@"{0}\{1}\{2}\{3}\{4}", SysParameterHelper.DocUploadPath, model.TestTypeID, citizenId.Right(3), citizenId, doc.DOC_GUID);
                            CSP.Lib.NetworkFile.NetworkFileHelper.CopyOverNetwork(domain, user, password, srcPath, destPath, true);
                            if (!CSP.Lib.NetworkFile.NetworkFileHelper.IsFileExists(domain, user, password, destPath))
                            {
                                result.ErrorMessage = string.Format("ไม่พบไฟล์เอกสาร ชื่อ {0}", destPath);
                                Log.WriteErrorLog(tsw.TraceError, result.ErrorMessage);
                                return Json(result);
                            }
                        }
                    }
                }             
            }
            catch (Exception ex)
            {
                // TODO : อาจจะต้องเอา Log ที่ Save File ไม่สำเร็จใส่ไว้ใน Table ด้วย ไม่งั้นหายากมาก
                //Log.WriteInformationLog(tsw.TraceInfo, string.Format("Error copy image {0:dd-MM-yyyy HH:mm:ss.fff}", System.DateTime.Now));
                result.ErrorMessage = "อัปโหลดเอกสารไม่สำเร็จ กรุณาลองใหม่อีกครั้ง";
                Log.WriteErrorLog(tsw.TraceError, ex);
                return Json(result);
            }

            result = dal.RenewEnrollDoc(model);
            return Json(result);
        }
    }
}