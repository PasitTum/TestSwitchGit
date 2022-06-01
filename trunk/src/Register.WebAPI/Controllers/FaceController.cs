using CSP.Lib.Models;
using CSP.Lib.Mvc;
using OCS.Register.DAL;
using OCS.Register.Models;
using DLA.Register.WebAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Web.Http;

namespace DLA.Register.WebAPI.Controllers
{
    [RoutePrefix("api/Face")]
    public class FaceController : ApiController
    {
        [HttpPost]
        [CSPValidateHttpAntiForgeryToken]
        [Route("DetectFace")]
        public IHttpActionResult DetectFace(DetectFaceModel model)
        {
            var rtn = this.RequestFaceService(model.TestTypeID, model.CitizenID, model.Image);
            return Json(rtn);
        }

        [HttpPost]
        //[CSPValidateHttpAntiForgeryToken]
        [Route("InsertPhoto")]
        public IHttpActionResult InsertPhoto(DetectFaceModel model)
        {
            var dal = new FaceDAL();
            var image64 = model.Image;
            image64 = image64.Replace("data:image/jpeg;base64,", "");
            image64 = image64.Replace("data:image/png;base64,", "");
            var image = Convert.FromBase64String(image64);
            var rtn = dal.InsertPhoto(model.EnrollNo, image);
            return Json(rtn);
        }

        private ResultInfo RequestFaceService(int testTypeID, string citizenID, string image)
        {
            var rtn = new ResultInfo();
            var faceServiceUrl = SysParameterHelper.FaceServiceUrl;
            var enableAutoBrightness = (SysParameterHelper.EnableAutoBrightness == "Y");
            var validateFaceRatio = (SysParameterHelper.ValidateFaceRatio == "Y");
            var validateWidthDivideHeight = (SysParameterHelper.ValidateWidthDivideHeight == "Y");

            image = image.Replace("data:image/jpeg;base64,", "");
            image = image.Replace("data:image/png;base64,", "");


            byte[] srcImg = Convert.FromBase64String(image);

            using (var proxy = new FaceDetectionService.FaceDetectionService())
            {
                proxy.Url = faceServiceUrl;
                FaceDetectionService.FaceDetectConfig FaceConfig = new FaceDetectionService.FaceDetectConfig()
                {
                    //TODO: TransactionID is PROJECT_CODE + citizenID
                    EnableAutoBrightness = enableAutoBrightness,
                    TransactionID = "OCS" + testTypeID + "_" + citizenID,
                    ClientIpAddress = GetLocalIPAddress(),
                    SourceImage = srcImg,
                    ValidateFaceRatio = validateFaceRatio,
                    ValidateWidthDivideHeight = validateWidthDivideHeight,

                };
                if (FaceConfig.ValidateFaceRatio)
                {
                    FaceConfig.MinFacePercent = decimal.Parse(SysParameterHelper.MinFacePercent);
                    FaceConfig.MaxFacePercent = decimal.Parse(SysParameterHelper.MaxFacePercent);
                }
                if (FaceConfig.ValidateWidthDivideHeight)
                {
                    FaceConfig.MinWidthHeightRatio = decimal.Parse(SysParameterHelper.MinWidthHeightRatio);
                    FaceConfig.MaxWidthHeightRatio = decimal.Parse(SysParameterHelper.MaxWidthHeightRatio);
                }

                FaceDetectionService.FaceDetectResult FaceDetectInfos = proxy.FaceDetect(FaceConfig);
                if (FaceDetectInfos != null)
                {
                    if (FaceDetectInfos.IsFaceDetected)
                    {
                        rtn.Success = true;
                        if (!FaceDetectInfos.Success)
                        {
                            rtn.Success = false;
                            rtn.ErrorMessage = "รูปถ่ายไม่ถูกต้อง";
                        }
                    }
                    else
                    {
                        rtn.Success = false;
                        rtn.ErrorMessage = "รูปถ่ายไม่ถูกต้อง";
                    }
                }
            }
            return rtn;
        }

        private string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
    }
}
