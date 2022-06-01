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
using CSP.Lib.Extension;
using System.IO;

namespace Register.Web.Controllers
{
    using CSP.Lib.NetworkFile;
    using Models;

    public class UploadController : BaseController
    {
        [HttpPost]
        public ActionResult UploadDocs()
        {
            var model = new DocToUploadModel()
            {
                UPLOAD_STATUS = "N"
            };
            try
            {
                if (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
                {
                    model.TEST_TYPE_ID = Request.Form["testtypeid"].ToInt();
                    model.DOC_TYPE_ID = Request.Form["doctypeid"].ToInt();
                    model.DOC_GUID = Guid.NewGuid();
                    var fi = Request.Files[0];
                    model.FILE_NAME = Path.GetFileName(fi.FileName);
                    model.FILE_TYPE = fi.ContentType;

                    var dest = Path.Combine(SysParameterHelper.DocUploadTempPath, model.TEST_TYPE_ID.ToString(), model.DOC_GUID.ToString().Right(3), model.DOC_GUID.ToString());
                    var domain = SysParameterHelper.PhotoDomain;
                    var user = SysParameterHelper.PhotoUser;
                    var password = SysParameterHelper.PhotoPassword;
                    NetworkFileHelper.SaveFileOverNetwork(domain, user, password, fi.InputStream, dest);

                    // check file ลง temp สำเร็จหรือไม่
                    if (NetworkFileHelper.IsFileExists(domain, user, password, dest))
                    {
                        model.UPLOAD_STATUS = "Y";
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, ex);
            }
            return Json(model);
        }

        //[HttpPost]
        //public ActionResult DownloadFromTemp(DocToUploadModel model)
        public ActionResult DownloadFromTemp(int testTypeID, string docGuid, string filename, string fileType)
        {
            try
            {
                //var dest = Path.Combine(SysParameterHelper.DocUploadTempPath, model.TEST_TYPE_ID.ToString(), model.DOC_GUID.ToString());
                var dest = Path.Combine(SysParameterHelper.DocUploadTempPath, testTypeID.ToString(), docGuid.ToString().Right(3), docGuid);
                var domain = SysParameterHelper.PhotoDomain;
                var user = SysParameterHelper.PhotoUser;
                var password = SysParameterHelper.PhotoPassword;

                return File(NetworkFileHelper.ReadFile(domain, user, password, dest), fileType, filename);
                //return File(NetworkFileHelper.ReadFile(domain, user, password, dest), model.FILE_TYPE, model.FILE_NAME);
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, ex);
            }
            return new EmptyResult();
        }


    }
}