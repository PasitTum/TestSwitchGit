using CSP.Lib.Diagnostic;
using CSP.Lib.Extension;
using CSP.Lib.Models;
using CSP.Lib.Mvc;
using Register.DAL;
using Register.Models;
using Register.WebAPI.Controllers;
using Register.WebAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Web.Http;

namespace DLA.Register.WebAPI.Controllers
{
    [RoutePrefix("api/Photo")]
    public class PhotoController : BaseApiController
    {

        [HttpPost]
        [CSPValidateHttpAntiForgeryToken]
        [Route("InsertPhoto")]
        public async Task<IHttpActionResult> InsertPhoto(UploadPhotoModel model)
        {
            var dal = new PhotoDAL();
            model.IP_ADDRESS = SystemHelper.GetClientIP();
            model.MAC_ADDRESS = SystemHelper.GetServerIP();

            if (!string.IsNullOrEmpty(model.ImageBase64))
            {
                // Get old file
                var domain = SysParameterHelper.PhotoDomain;
                var user = SysParameterHelper.PhotoUser;
                var password = SysParameterHelper.PhotoPassword;
                var DestPath = string.Format(@"{0}\{3}\{2}\{1}.jpg", SysParameterHelper.PhotoImagePath, model.CitizenID, model.CitizenID.Right(3), model.TestTypeID);
                var copyDestPath = string.Format(@"{0}\{3}\{2}\{1}.jpg", SysParameterHelper.PhotoImageHistoryPath, (model.CitizenID + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss")), model.CitizenID.Right(3), model.TestTypeID);
                string convert = model.ImageBase64.Replace("data:image/jpeg;base64,", String.Empty);

                // Copy file
                try
                {
                    //Log.WriteInformationLog(tsw.TraceInfo, string.Format("Start save image {0:dd-MM-yyyy HH:mm:ss.fff}", System.DateTime.Now));
                    CSP.Lib.NetworkFile.NetworkFileHelper.CopyOverNetwork(domain, user, password, DestPath, copyDestPath);
                    //Log.WriteInformationLog(tsw.TraceInfo, string.Format("End save image {0:dd-MM-yyyy HH:mm:ss.fff}", System.DateTime.Now));
                }
                catch (Exception ex)
                {
                    // TODO : อาจจะต้องเอา Log ที่ Save File ไม่สำเร็จใส่ไว้ใน Table ด้วย ไม่งั้นหายากมาก
                    Log.WriteInformationLog(tsw.TraceInfo, string.Format("Error save image {0:dd-MM-yyyy HH:mm:ss.fff}", System.DateTime.Now));
                    Log.WriteErrorLog(tsw.TraceError, ex);
                }

                // Save replace file 
                try
                {
                    //Log.WriteInformationLog(tsw.TraceInfo, string.Format("Start save image {0:dd-MM-yyyy HH:mm:ss.fff}", System.DateTime.Now));
                    CSP.Lib.NetworkFile.NetworkFileHelper.SaveBase64ToNetwork(domain, user, password, convert, DestPath, true);
                    //Log.WriteInformationLog(tsw.TraceInfo, string.Format("End save image {0:dd-MM-yyyy HH:mm:ss.fff}", System.DateTime.Now));
                }
                catch (Exception ex)
                {
                    // TODO : อาจจะต้องเอา Log ที่ Save File ไม่สำเร็จใส่ไว้ใน Table ด้วย ไม่งั้นหายากมาก
                    //Log.WriteInformationLog(tsw.TraceInfo, string.Format("Error save image {0:dd-MM-yyyy HH:mm:ss.fff}", System.DateTime.Now));
                    Log.WriteErrorLog(tsw.TraceError, ex);
                }

            }
            var lst = await dal.SubmitEditImage(model);
            var rtn = dal.InsertPhoto(model.EnrollNo, model.CitizenID);
            return Json(rtn);
        }
    }
}
