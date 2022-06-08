using CaptchaMvc.HtmlHelpers;
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

namespace Register.Web.Controllers
{
    public class RegisterController : BaseController
    {
        //[CalendarOpenFilter(CalendarCode = "ENROLL", SystemType = "MASTER")]
        //public ActionResult Index()
        //{
        //    return View("Index");
        //}

        //[CalendarOpenFilter(CalendarCode = "ENROLL", SystemType = "MASTER")]
        //public ActionResult ChooseRegion()
        //{
        //    return View();
        //}

        //[CalendarOpenFilter(CalendarCode = "ENROLL", SystemType = "MASTER")]
        //public ActionResult ChoosePosition(string centerID, string centerName)
        //{
        //    if (string.IsNullOrWhiteSpace(centerID) || string.IsNullOrWhiteSpace(centerName))
        //    {
        //        return RedirectToActionPermanent("ChooseRegion");
        //    }
        //    ViewBag.CenterID = centerID;
        //    ViewBag.CenterName = centerName;
        //    return View();
        //}


        [CalendarOpenFilter(CalendarCode = "ENROLL", SystemType = "MASTER")]
        //public ActionResult Register(int? testTypeID, string centerID, string classID, string centerName, string className)
        public ActionResult Register(int? testTypeID)
        {
            // TODO : validate required parameter such as [regionid, positionid]
            //EncryptHelper encryptHelper = new EncryptHelper();
            RegisterModel model = new RegisterModel();
            //model.TestTypeID = int.Parse(testTypeID.ToString());
            //model.CenterExamID = int.Parse(centerID.ToString());
            //model.RegClassID = int.Parse(classID.ToString());
            //model.CenterExamName = centerName;
            //model.RegClassName = className;
            return View("CheckCitizenID", model);
        }

        [HttpPost]
        [CSPValidateHttpAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            //var rtn = new ResultInfo();
            var api = SysParameterHelper.ApiUrlServerSide;
            //model.LaserCode = model.LaserCode.ToUpper();
            if (!this.IsCaptchaValid("Validate your captcha"))
            {
                ModelState.AddModelError("Captcha", "กรอกตัวอักษรที่อยู่บนหน้าเว็บไม่ถูกต้อง");
                //var captchaValue = this.GenerateCaptchaValue(5);
                return View("CheckCitizenID", model);
            }
            ValidateCitizenIDModel result = null;
            ResultInfo resultDOPA = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = await client.PostAsJsonAsync<RegisterModel>(api + "Register/ValidateCitizenID", model);
                    response.EnsureSuccessStatusCode();
                    result = await response.Content.ReadAsAsync<ValidateCitizenIDModel>() ?? new ValidateCitizenIDModel();
                }
            }
            catch (Exception ex)
            {
                model.ErrorMessage = ex.Message;
                Log.WriteErrorLog(tsw.TraceError, ex);
                return View("CheckCitizenID", model);
            }

            if (result.RESULT_CODE != 0 && result.RESULT_CODE != 9)
            {
                model.ErrorMessage = result.RESULT_MSG;
                return View("CheckCitizenID", model);
            }

            if (result.RESULT_CODE != 9)
            {
                model.IPAddress = SystemHelper.GetClientIP();
                model.MacAddress = SystemHelper.GetServerIP();
                try
                {
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var response = await client.PostAsJsonAsync<RegisterModel>(api + "Register/VaidateCitizenDOPA", model);
                        response.EnsureSuccessStatusCode();
                        resultDOPA = await response.Content.ReadAsAsync<ResultInfo>();
                    }
                }
                catch (Exception ex)
                {
                    model.ErrorMessage = ex.Message;
                    Log.WriteErrorLog(tsw.TraceError, ex);
                    return View("CheckCitizenID", model);
                }
            }
            else
            {
                resultDOPA = new ResultInfo();
                resultDOPA.Success = true;
            }

            if (!resultDOPA.Success)
            {
                model.ErrorMessage = resultDOPA.ErrorMessage;
                return View("CheckCitizenID", model);
            }

            model.ErrorMessage = null;
            model.ViewMode = "New";
            TempData["student"] = model;
            return RedirectToAction("RegisterStep1", new { testTypeID = model.TestTypeID });
        }

        [CalendarOpenFilter(CalendarCode = "ENROLL", SystemType = "MASTER")]
        public ActionResult RegisterStep1(int? testTypeID)
        {
            var model = (RegisterModel)TempData["student"];
            if (model == null || string.IsNullOrWhiteSpace(model.CitizenID) || string.IsNullOrWhiteSpace(model.FirstName) || string.IsNullOrWhiteSpace(model.LastName)
                 || string.IsNullOrWhiteSpace(model.BirthDate) || string.IsNullOrWhiteSpace(model.BirthDateChar))
            //if (model == null || string.IsNullOrWhiteSpace(model.CitizenID) || string.IsNullOrWhiteSpace(model.FirstName) || string.IsNullOrWhiteSpace(model.LastName)
            //    || string.IsNullOrWhiteSpace(model.LaserCode) || string.IsNullOrWhiteSpace(model.BirthDate) || string.IsNullOrWhiteSpace(model.BirthDateChar))
            {
                return View("ErrorRegisterData");
            }
            model.Mode = "Register";
            TempData["student"] = model;

            if (testTypeID == 1)
            {
                return View("Register", model);
            }
            else
            {
                return View($"~/Views/Register/{testTypeID}/Register.cshtml", model);
            }
        }

        [HttpPost]
        [CSPValidateHttpAntiForgeryToken]
        public ActionResult RegisterStep1(RegisterModel model)
        {
            //var rtn = new ResultInfo();
            var modelTemp = (RegisterModel)TempData["student"];
            if (model == null || modelTemp == null || string.IsNullOrWhiteSpace(model.CitizenID) || string.IsNullOrWhiteSpace(model.FirstName) || string.IsNullOrWhiteSpace(model.LastName)
                || string.IsNullOrWhiteSpace(model.BirthDate) || string.IsNullOrWhiteSpace(model.BirthDateChar)
                || model.CitizenID != modelTemp.CitizenID || model.TestTypeID != modelTemp.TestTypeID)
            //if (model == null || modelTemp == null || string.IsNullOrWhiteSpace(model.CitizenID) || string.IsNullOrWhiteSpace(model.FirstName) || string.IsNullOrWhiteSpace(model.LastName)
            //    || string.IsNullOrWhiteSpace(model.LaserCode) || string.IsNullOrWhiteSpace(model.BirthDate) || string.IsNullOrWhiteSpace(model.BirthDateChar)
            //    || model.CitizenID != modelTemp.CitizenID)
            {
                return View("ErrorRegisterData");
            }
            if (!string.IsNullOrEmpty(model.ImageBase64))
            {
                var domain = SysParameterHelper.PhotoDomain;
                var user = SysParameterHelper.PhotoUser;
                var password = SysParameterHelper.PhotoPassword;
                var destPath = string.Format(@"{0}\{3}\{2}\{1}.jpg", SysParameterHelper.PhotoImageTempPath, model.CitizenID, model.CitizenID.Right(3), model.TestTypeID);
                string convert = model.ImageBase64.Replace("data:image/jpeg;base64,", String.Empty);
                try
                {
                    //Log.WriteInformationLog(tsw.TraceInfo, string.Format("Start save image {0:dd-MM-yyyy HH:mm:ss.fff}", System.DateTime.Now));
                    CSP.Lib.NetworkFile.NetworkFileHelper.SaveBase64ToNetwork(domain, user, password, convert, destPath, true);
                    if (!CSP.Lib.NetworkFile.NetworkFileHelper.IsFileExists(domain, user, password, destPath))
                    {
                        Log.WriteErrorLog(tsw.TraceError, "ไม่พบไฟล์รูปถ่าย");
                        return View("Register", model);
                    }
                    //Log.WriteInformationLog(tsw.TraceInfo, string.Format("End save image {0:dd-MM-yyyy HH:mm:ss.fff}", System.DateTime.Now));
                }
                catch (Exception ex)
                {
                    // TODO : อาจจะต้องเอา Log ที่ Save File ไม่สำเร็จใส่ไว้ใน Table ด้วย ไม่งั้นหายากมาก
                    //Log.WriteInformationLog(tsw.TraceInfo, string.Format("Error save image {0:dd-MM-yyyy HH:mm:ss.fff}", System.DateTime.Now));
                    Log.WriteErrorLog(tsw.TraceError, ex);
                }
            }

            model.ImageBase64 = null;
            TempData.Clear();
            model.ViewMode = "Edit";
            var tmpModel = (RegisterModel)model.Clone();
            tmpModel = ClearName(tmpModel);
            TempData["student"] = tmpModel;
            //model = tmpModel;

            ViewBag.ImageBase64 = GetImageBase64(model.CitizenID, model.TestTypeID);
            TempData.Keep();

            if (model.TestTypeID == 1)
            {
                return View("ConfirmRegister", model);
            }
            else
            {
                return View($"~/Views/Register/{model.TestTypeID}/ConfirmRegister.cshtml", model);
            }
            //return RedirectToAction("RegisterStep2", new { testTypeID = model.TestTypeID });
        }


        public RegisterModel ClearName(RegisterModel model)
        {
            model.PrefixName = null;
            model.GenderName = null;
            model.StatusName = null;
            //model.ClassGroupName = null;
            model.RegClassName = null;
            model.ClassLavelName = null;
            model.DocTypeName = null;
            model.TeachClassLevelName = null;
            model.OccuupationName = null;
            model.ProvName = null;
            model.AmphName = null;
            model.TmblName = null;
            model.ExamSiteName = null;

            return model;
        }




        [CalendarOpenFilter(CalendarCode = "ENROLL", SystemType = "MASTER")]
        public ActionResult RegisterStep2(int? testTypeID)
        {
            var model = (RegisterModel)TempData["student"];
            if (model == null || string.IsNullOrWhiteSpace(model.CitizenID) || string.IsNullOrWhiteSpace(model.FirstName) || string.IsNullOrWhiteSpace(model.LastName)
                || string.IsNullOrWhiteSpace(model.BirthDate) || string.IsNullOrWhiteSpace(model.BirthDateChar) || string.IsNullOrWhiteSpace(model.GraduateDate))
            //if (model == null || string.IsNullOrWhiteSpace(model.CitizenID) || string.IsNullOrWhiteSpace(model.FirstName) || string.IsNullOrWhiteSpace(model.LastName)
            //    || string.IsNullOrWhiteSpace(model.LaserCode) || string.IsNullOrWhiteSpace(model.BirthDate) || string.IsNullOrWhiteSpace(model.BirthDateChar) || string.IsNullOrWhiteSpace(model.GraduateDate))
            {
                return View("ErrorRegisterData");
            }
            model.Mode = "Confirm";

            model.ImageBase64 = null;
            model.gridEducations = null;
            model.gridExperiences = null;
            TempData.Clear();
            TempData["student"] = model;
            ViewBag.ImageBase64 = GetImageBase64(model.CitizenID, model.TestTypeID);
            if (model.TestTypeID == 1)
            {
                return View("ConfirmRegister", model);
            }
            else
            {
                return View($"~/Views/Register/{model.TestTypeID}/ConfirmRegister.cshtml", model);
            }
        }

        [HttpPost]
        [CSPValidateHttpAntiForgeryToken]
        public async Task<ActionResult> RegisterStep2(RegisterModel model)
        {
            var modelTemp = (RegisterModel)TempData["student"];
            if (model == null || modelTemp == null || string.IsNullOrWhiteSpace(model.CitizenID) || string.IsNullOrWhiteSpace(model.FirstName) || string.IsNullOrWhiteSpace(model.LastName)
                || string.IsNullOrWhiteSpace(model.BirthDate) || string.IsNullOrWhiteSpace(model.BirthDateChar) || string.IsNullOrWhiteSpace(model.GraduateDate)
                || model.CitizenID != modelTemp.CitizenID || model.TestTypeID != modelTemp.TestTypeID)
            //if (model == null || modelTemp == null || string.IsNullOrWhiteSpace(model.CitizenID) || string.IsNullOrWhiteSpace(model.FirstName) || string.IsNullOrWhiteSpace(model.LastName)
            //    || string.IsNullOrWhiteSpace(model.LaserCode) || string.IsNullOrWhiteSpace(model.BirthDate) || string.IsNullOrWhiteSpace(model.BirthDateChar) || string.IsNullOrWhiteSpace(model.GraduateDate)
            //    || model.CitizenID != modelTemp.CitizenID)
            {
                return View("ErrorRegisterData");
            }

            if (!this.IsCaptchaValid("Validate your captcha"))
            {
                ModelState.AddModelError("Captcha", "กรอกตัวอักษรที่อยู่บนหน้าเว็บไม่ถูกต้อง");
                //var captchaValue = this.GenerateCaptchaValue(5);
                model.ErrorMessage = "กรอกตัวอักษรที่อยู่บนหน้าเว็บไม่ถูกต้อง";
                model.gridEducations = null;
                model.gridExperiences = null;
                TempData["student"] = model;
                ViewBag.ImageBase64 = GetImageBase64(model.CitizenID, model.TestTypeID);
                model.Mode = "Confirm";
                if (model.TestTypeID == 1)
                {
                    return View("ConfirmRegister", model);
                }
                else
                {
                    return View($"~/Views/Register/{model.TestTypeID}/ConfirmRegister.cshtml", model);
                }
            }

            //if (model.TestTypeID == 1)
            //{
            //    if (model.DOCS != null && model.DOCS.Count > 0)
            //    {
            //        foreach (var item in model.DOCS)
            //        {
            //            if (item.REQUIRE_FLAG == "Y" && string.IsNullOrWhiteSpace(item.DOC_GUID.ToString()))
            //            {
            //                return View("ErrorRegisterData");
            //            }
            //        }
            //    }

            //    else
            //    {
            //        return View("ErrorRegisterData");
            //    }
            //}

            ResultInfo result = null;
            var api = SysParameterHelper.ApiUrlServerSide;

            model.IPAddress = SystemHelper.GetClientIP();
            model.MacAddress = SystemHelper.GetServerIP();

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = await client.PostAsJsonAsync<RegisterModel>(api + "Register/SetEnrollInfo", model);
                    response.EnsureSuccessStatusCode();
                    result = await response.Content.ReadAsAsync<ResultInfo>() ?? new ResultInfo();
                }
            }
            catch (Exception ex)
            {
                model.ErrorMessage = "บันทึกข้อมูลไม่สำเร็จ";
                Log.WriteErrorLog(tsw.TraceError, ex);
                model.Mode = "Confirm";
                model.gridEducations = null;
                model.gridExperiences = null;
                TempData["student"] = model;
                ViewBag.ImageBase64 = GetImageBase64(model.CitizenID, model.TestTypeID);
                if (model.TestTypeID == 1)
                {
                    return View("ConfirmRegister", model);
                }
                else
                {
                    return View($"~/Views/Register/{model.TestTypeID}/ConfirmRegister.cshtml", model);
                }
            }
            if (result == null || !result.Success)
            {
                model.Mode = "Confirm";
                model.ErrorMessage = "บันทึกข้อมูลไม่สำเร็จ";
                model.gridEducations = null;
                model.gridExperiences = null;
                Log.WriteErrorLog(tsw.TraceError, (result != null ? result.ErrorMessage : ""));
                TempData["student"] = model;
                ViewBag.ImageBase64 = GetImageBase64(model.CitizenID, model.TestTypeID);
                if (model.TestTypeID == 1)
                {
                    return View("ConfirmRegister", model);
                }
                else
                {
                    return View($"~/Views/Register/{model.TestTypeID}/ConfirmRegister.cshtml", model);
                }
            }
            model.ErrorMessage = null;
            model.ImageBase64 = null;
            model.gridEducations = null;
            model.gridExperiences = null;
            TempData.Clear();
            TempData["student"] = model;
            return RedirectToAction("RegisterFinal", new { testTypeID = model.TestTypeID });
        }

        [HttpPost]
        public async Task<ActionResult> RegisterStep2Test(RegisterModel model)
        {
            ResultInfo result = null;
            var api = SysParameterHelper.ApiUrlServerSide;

            if (!string.IsNullOrEmpty(model.ImageBase64))
            {
                var domain = SysParameterHelper.PhotoDomain;
                var user = SysParameterHelper.PhotoUser;
                var password = SysParameterHelper.PhotoPassword;
                var destPath = string.Format(@"{0}\{2}\{1}.jpg", SysParameterHelper.PhotoImageTempPath, model.CitizenID, model.CitizenID.Right(3));
                string convert = model.ImageBase64.Replace("data:image/jpeg;base64,", String.Empty);
                try
                {
                    //Log.WriteInformationLog(tsw.TraceInfo, string.Format("Start save image {0:dd-MM-yyyy HH:mm:ss.fff}", System.DateTime.Now));
                    CSP.Lib.NetworkFile.NetworkFileHelper.SaveBase64ToNetwork(domain, user, password, convert, destPath, true);
                    //Log.WriteInformationLog(tsw.TraceInfo, string.Format("End save image {0:dd-MM-yyyy HH:mm:ss.fff}", System.DateTime.Now));
                }
                catch (Exception ex)
                {
                    //Log.WriteInformationLog(tsw.TraceInfo, string.Format("Error save image {0:dd-MM-yyyy HH:mm:ss.fff}", System.DateTime.Now));
                    Log.WriteErrorLog(tsw.TraceError, ex);
                }
            }

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = await client.PostAsJsonAsync<RegisterModel>(api + "Register/SetEnrollInfo", model);
                    response.EnsureSuccessStatusCode();
                    result = await response.Content.ReadAsAsync<ResultInfo>() ?? new ResultInfo();
                }
            }
            catch (Exception ex)
            {
                model.ErrorMessage = ex.Message;
                Log.WriteErrorLog(tsw.TraceError, ex);
                return Json(result);
            }
            if (result == null || !result.Success)
            {
                return Json(result);
            }

            model.ErrorMessage = null;
            return Json(result);
        }

        [CalendarOpenFilter(CalendarCode = "ENROLL", SystemType = "MASTER")]
        public ActionResult RegisterFinal(int? testTypeID)
        {
            var model = (RegisterModel)TempData["student"];
            if (model == null || string.IsNullOrWhiteSpace(model.CitizenID) || string.IsNullOrWhiteSpace(model.FirstName) || string.IsNullOrWhiteSpace(model.LastName)
                || string.IsNullOrWhiteSpace(model.BirthDate) || string.IsNullOrWhiteSpace(model.BirthDateChar))

            //if (model == null || string.IsNullOrWhiteSpace(model.CitizenID) || string.IsNullOrWhiteSpace(model.FirstName) || string.IsNullOrWhiteSpace(model.LastName)
            //    || string.IsNullOrWhiteSpace(model.LaserCode) || string.IsNullOrWhiteSpace(model.BirthDate) || string.IsNullOrWhiteSpace(model.BirthDateChar))
            {
                return View("ErrorRegisterData");
            }
            TempData["student"] = model;
            return View("RegisterFinal", model);
        }

        public string GetImageBase64(string citizenID, int testTypeID)
        {
            var domain = SysParameterHelper.PhotoDomain;
            var user = SysParameterHelper.PhotoUser;
            var password = SysParameterHelper.PhotoPassword;
            var destPath = string.Format(@"{0}\{3}\{2}\{1}.jpg", SysParameterHelper.PhotoImageTempPath, citizenID, citizenID.Right(3), testTypeID);
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

        //public ActionResult SMSDialog()
        //{
        //    return PartialView("_SMSDialogModal");
        //}

    }
}