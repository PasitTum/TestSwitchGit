using CSP.Lib.Diagnostic;
using CSP.Lib.Extension;
using CSP.Lib.Models;
using CWN.OTAS.Lib.Helper;
using Register.Database;
using Register.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Register.DAL
{
    public class InquiryDAL : BaseDAL
    {
        public async Task<PaymentStatusModel> PaymentStatus(int testTypeID, string citizenID)
        {
            EncryptHelper encryptHelper = new EncryptHelper();
            var citizen = string.Empty;
            if (citizenID != null & citizenID != "")
            {
                citizen = encryptHelper.EncryptData(citizenID.ToString());
            }
            try
            {
                using (var db = new RegisterDB())
                {
                    var prms = new List<CommonParameter>();
                    prms.Add(new CommonParameter("TEST_TYPE_ID", testTypeID));
                    prms.Add(new CommonParameter("CITIZEN_ID", citizen));
                    return await db.ExecuteStored<PaymentStatusModel>("ENR_SP_GET_PAYIN @TEST_TYPE_ID, @CITIZEN_ID", prms).FirstOrDefaultAsync();
                }
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, ex);
            }
            return null;
        }

        public async Task<RegisterStatusModel> RegisterStatus(int testTypeID, string citizenID)
        {
            EncryptHelper encryptHelper = new EncryptHelper();
            var citizen = string.Empty;
            if (citizenID != null & citizenID != "")
            {
                citizen = encryptHelper.EncryptData(citizenID.ToString());
            }
            try
            {
                using (var db = new RegisterDB())
                {
                    var prms = new List<CommonParameter>();
                    prms.Add(new CommonParameter("TEST_TYPE_ID", testTypeID));
                    prms.Add(new CommonParameter("CITIZEN_ID", citizen));
                    return await db.ExecuteStored<RegisterStatusModel>("ENR_SP_GET_CHECK_STATUS @TEST_TYPE_ID, @CITIZEN_ID", prms).FirstOrDefaultAsync();
                }
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, ex);
            }
            return null;
        }

        public async Task<EnrollInfoModel> EnrollInfo(int testTypeID, string citizenID)
        {
            EncryptHelper encryptHelper = new EncryptHelper();
            var citizen = string.Empty;
            if (citizenID != null & citizenID != "")
            {
                citizen = encryptHelper.EncryptData(citizenID.ToString());
            }
            try
            {
                using (var db = new RegisterDB())
                {
                    var prms = new List<CommonParameter>();
                    prms.Add(new CommonParameter("TEST_TYPE_ID", testTypeID));
                    prms.Add(new CommonParameter("CITIZEN_ID", citizen));
                    return await db.ExecuteStored<EnrollInfoModel>("ENR_SP_GET_ENROLL @TEST_TYPE_ID, @CITIZEN_ID", prms).FirstOrDefaultAsync();
                }
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, ex);
            }
            return null;
        }

        public async Task<ExamApplicationModel> ExamApplication(int testTypeID, string citizenID, string mobileNo)
        {
            EncryptHelper encryptHelper = new EncryptHelper();
            var citizen = string.Empty;
            if (citizenID != null & citizenID != "")
            {
                citizen = encryptHelper.EncryptData(citizenID.ToString());
            }
            try
            {
                using (var db = new RegisterDB())
                {
                    var prms = new List<CommonParameter>();
                    prms.Add(new CommonParameter("IPADDRESS", ""));
                    prms.Add(new CommonParameter("MACADDRESS", ""));
                    prms.Add(new CommonParameter("PROCESSCD", ""));
                    prms.Add(new CommonParameter("TEST_TYPE_ID", testTypeID));
                    prms.Add(new CommonParameter("CITIZEN_ID", citizen));
                    //ไม่ค้นหาด้วยเลขหลังบัตรประชาชน
                    //prms.Add(new CommonParameter("LASER_CODE", laserCode));
                    prms.Add(new CommonParameter("CONDITION", mobileNo));
                    return await db.ExecuteStored<ExamApplicationModel>("ENR_SP_GET_APPICATION_FORM @IPADDRESS, @MACADDRESS, @PROCESSCD, @TEST_TYPE_ID, @CITIZEN_ID, @CONDITION", prms).FirstOrDefaultAsync();
                }
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, ex);
            }
            return null;
        }

        public async Task<List<PaymentRepeatedModel>> PaymentRepeated(int testTypeID, string citizenID, string laserCode)
        {
            EncryptHelper encryptHelper = new EncryptHelper();
            var citizen = string.Empty;
            if (citizenID != null & citizenID != "")
            {
                citizen = encryptHelper.EncryptData(citizenID.ToString());
            }
            try
            {
                using (var db = new RegisterDB())
                {
                    var prms = new List<CommonParameter>();
                    prms.Add(new CommonParameter("IPADDRESS", ""));
                    prms.Add(new CommonParameter("MACADDRESS", ""));
                    prms.Add(new CommonParameter("PROCESSCD", ""));
                    prms.Add(new CommonParameter("TEST_TYPE_ID", testTypeID));
                    prms.Add(new CommonParameter("CITIZEN_ID", citizen));
                    prms.Add(new CommonParameter("LASER_CODE", laserCode));
                    return await db.ExecuteStored<PaymentRepeatedModel>("ENR_SP_GET_PAYMENT_REPEATED @IPADDRESS, @MACADDRESS, @PROCESSCD, @TEST_TYPE_ID, @CITIZEN_ID, @LASER_CODE", prms).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, ex);
            }
            return null;
        }

        public async Task<UploadStatusModel> UploadStatus(int testTypeID, string citizenID)
        {
            EncryptHelper encryptHelper = new EncryptHelper();
            var citizen = string.Empty;
            if (citizenID != null & citizenID != "")
            {
                citizen = encryptHelper.EncryptData(citizenID.ToString());
            }
            try
            {
                using (var db = new RegisterDB())
                {
                    var prms = new List<CommonParameter>();
                    prms.Add(new CommonParameter("TEST_TYPE_ID", testTypeID));
                    prms.Add(new CommonParameter("CITIZEN_ID", citizen));
                    return await db.ExecuteStored<UploadStatusModel>("ENR_SP_GET_UPLOAD_STATUS @TEST_TYPE_ID, @CITIZEN_ID", prms).FirstOrDefaultAsync();
                }
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, ex);
            }
            return null;
        }

        public async Task<ExamSiteInfoModel> ExamSiteInfo(int testTypeID, string citizenID)
        {
            EncryptHelper encryptHelper = new EncryptHelper();
            var citizen = string.Empty;
            if (citizenID != null & citizenID != "")
            {
                citizen = encryptHelper.EncryptData(citizenID.ToString());
            }
            try
            {
                using (var db = new RegisterDB())
                {
                    var prms = new List<CommonParameter>();
                    prms.Add(new CommonParameter("IPADDRESS", ""));
                    prms.Add(new CommonParameter("PROCESSCD", ""));
                    prms.Add(new CommonParameter("TEST_TYPE_ID", testTypeID));
                    prms.Add(new CommonParameter("CITIZEN_ID", citizen));
                    return await db.ExecuteStored<ExamSiteInfoModel>("ENR_SP_GET_SEARCH_EXAM_SITE_INFO @IPADDRESS, @PROCESSCD, @TEST_TYPE_ID, @CITIZEN_ID", prms).FirstOrDefaultAsync();
                }
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, ex);
            }
            return null;
        }

        public async Task<ExamPassInfoModel> ExamPassInfo(int testTypeID, string citizenID, string mobile)
        {
            EncryptHelper encryptHelper = new EncryptHelper();
            var citizen = string.Empty;
            if (citizenID != null & citizenID != "")
            {
                citizen = encryptHelper.EncryptData(citizenID.ToString());
            }
            try
            {
                using (var db = new RegisterDB())
                {
                    var prms = new List<CommonParameter>();
                    prms.Add(new CommonParameter("IPADDRESS", ""));
                    prms.Add(new CommonParameter("PROCESSCD", ""));
                    prms.Add(new CommonParameter("TEST_TYPE_ID", testTypeID));
                    prms.Add(new CommonParameter("CITIZEN_ID", citizen));
                    prms.Add(new CommonParameter("MOBILE_NO", mobile));
                    return await db.ExecuteStored<ExamPassInfoModel>("ENR_SP_GET_SEARCH_EXAM_PASS_INFO @IPADDRESS, @PROCESSCD, @TEST_TYPE_ID, @CITIZEN_ID, @MOBILE_NO", prms).FirstOrDefaultAsync();
                }
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, ex);
            }
            return null;
        }

        public async Task<ScoreModel> Score(int testTypeID, string citizenID, string password)
        {
            EncryptHelper encryptHelper = new EncryptHelper();
            var citizen = string.Empty;
            if (citizenID != null & citizenID != "")
            {
                citizen = encryptHelper.EncryptData(citizenID.ToString());
            }
            var enrPassword = string.Empty;
            if (password != null & password != "")
            {
                enrPassword = encryptHelper.EncryptData(password.ToString());
            }
            try
            {
                using (var db = new RegisterDB())
                {
                    var prms = new List<CommonParameter>();
                    prms.Add(new CommonParameter("IPADDRESS", ""));
                    prms.Add(new CommonParameter("PROCESSCD", ""));
                    prms.Add(new CommonParameter("TEST_TYPE_ID", testTypeID));
                    prms.Add(new CommonParameter("CITIZEN_ID", citizen));
                    prms.Add(new CommonParameter("PASSWORDS", enrPassword));
                    return await db.ExecuteStored<ScoreModel>("ANN_SP_GET_SCORE @IPADDRESS, @PROCESSCD, @TEST_TYPE_ID, @CITIZEN_ID, @PASSWORDS", prms).FirstOrDefaultAsync();
                }
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, ex);
            }
            return null;
        }

        public ResultInfo InsertRefundPayment(RefundPaymentModel model)
        {
            var rtn = new ResultInfo();
            EncryptHelper encryptHelper = new EncryptHelper();
            var citizen = string.Empty;
            if (model.CitizenID != null & model.CitizenID != "")
            {
                citizen = encryptHelper.EncryptData(model.CitizenID.ToString());
            }
            try
            {
                using (var db = new RegisterDB())
                {
                    var prms = new List<CommonParameter>();
                    prms.Add(new CommonParameter("IPADDRESS", ""));
                    prms.Add(new CommonParameter("MACADDRESS", ""));
                    prms.Add(new CommonParameter("PROCESSCD", ""));
                    prms.Add(new CommonParameter("ENROLL_NO", model.ENROLL_NO));
                    prms.Add(new CommonParameter("CITIZEN_ID", citizen));
                    prms.Add(new CommonParameter("BANK_NAME", model.BANK_NAME));
                    prms.Add(new CommonParameter("ACCT_NO", model.ACCT_NO));
                    prms.Add(new CommonParameter("MOBILE_NO", model.MOBILE_NO));
                    prms.Add(new CommonParameter("EMAIL", model.EMAIL));
                    var result = db.ExecuteStored("ENR_SP_INSERT_REQUEST_REFUND_PAYMENT @IPADDRESS, @MACADDRESS, @PROCESSCD, @ENROLL_NO, @CITIZEN_ID, @BANK_NAME, @ACCT_NO, @MOBILE_NO, @EMAIL", prms, null);
                    rtn.Success = result.Success;
                    return rtn;
                }
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, ex);
            }
            return null;
        }

        public async Task<DocByCitizenModel> UploadDocsStatus(InquiryModel model)
        {
            EncryptHelper encryptHelper = new EncryptHelper();
            var citizen = string.Empty;
            if (model.CitizenID != null & model.CitizenID != "")
            {
                citizen = encryptHelper.EncryptData(model.CitizenID.ToString());
            }
            try
            {
                using (var db = new RegisterDB())
                {
                    var prms = new List<CommonParameter>();
                    prms.Add(new CommonParameter("TEST_TYPE_ID", model.TestTypeID));
                    prms.Add(new CommonParameter("CITIZEN_ID", citizen));
                    prms.Add(new CommonParameter("MOBILE_NO", model.Mobile));
                    prms.Add(new CommonParameter("BIRTH_DATE_CHAR", model.BirthDateChar));
                    prms.Add(new CommonParameter("TESTING_CLASS_ID", model.TestingClassID));
                    return await db.ExecuteStored<DocByCitizenModel>("ENR_SP_GET_DOC_BY_CITIZEN @TEST_TYPE_ID, @CITIZEN_ID, @MOBILE_NO, @BIRTH_DATE_CHAR, @TESTING_CLASS_ID", prms).FirstOrDefaultAsync();
                }
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, ex);
            }
            return null;
        }

        public IEnumerable<dynamic> ListDocByCitizen(InquiryModel model)
        {
            EncryptHelper encryptHelper = new EncryptHelper();
            var citizen = string.Empty;
            if (model.CitizenID != null & model.CitizenID != "")
            {
                citizen = encryptHelper.EncryptData(model.CitizenID.ToString());
            }
            try
            {
                using (var db = new RegisterDB())
                {
                    var prms = new Dictionary<string, object>();
                    prms.Add("@TEST_TYPE_ID", model.TestTypeID);
                    prms.Add("@CITIZEN_ID", citizen);
                    prms.Add("@MOBILE_NO", model.Mobile);
                    prms.Add("@BIRTH_DATE_CHAR", model.BirthDateChar);
                    prms.Add("@TESTING_CLASS_ID", model.TestingClassID);
                    var sql = "ENR_SP_GET_DOC_BY_CITIZEN @TEST_TYPE_ID, @CITIZEN_ID, @MOBILE_NO, @BIRTH_DATE_CHAR, @TESTING_CLASS_ID";
                    var lst = db.DynamicListFromSql(sql, prms).ToList();
                    return lst;
                }
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, ex);
            }
            return null;
        }

        public ResultInfo RenewEnrollDoc(NewUploadDocModel model)
        {
            var rtn = new ResultInfo();
            try
            {
                using (var db = new RegisterDB())
                {
                    var prms = new List<CommonParameter>();
                    prms.Add(new CommonParameter("@IP_ADDRESS", model.IPAddress ?? ""));
                    prms.Add(new CommonParameter("@MAC_ADDRESS", model.MacAddress ?? ""));
                    prms.Add(new CommonParameter("@PROCESSCD", "ReNewUpload"));
                    prms.Add(new CommonParameter("@TEST_TYPE_ID", model.TestTypeID));
                    prms.Add(new CommonParameter("@ENROLL_NO", model.EnrollNo));

                    // เพิ่มส่วนอับโหลดเอกสาร
                    var docXml = model.DOCS.ObjectToXml();
                    prms.Add(new CommonParameter("@DOCS_LIST", docXml));

                    //// เพิ่ม Mode draft, success
                    //prms.Add(new CommonParameter("@mode", mode));

                    var result = db.ExecuteStored("ENR_SP_RENEW_ENROLL_DOC @IP_ADDRESS, @MAC_ADDRESS, @PROCESSCD, @TEST_TYPE_ID, @ENROLL_NO, @DOCS_LIST", prms, null);
                    rtn.Success = result.Success;
                    rtn.ErrorMessage = result.ErrorMessage;
                    return rtn;
                }
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, ex);
            }
            return null;
        }

    }
}
