using CSP.Lib.Diagnostic;
using CSP.Lib.Models;
using Register.Database;
using CWN.OTAS.Lib.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Register.Models;

namespace Register.DAL
{
    public class PhotoDAL : BaseDAL
    {
        public ResultInfo InsertPhoto(string enrollNo, string citizenID)
        {
            ResultInfo rtn = new ResultInfo();
            try
            {
                using (var db = new RegisterDB())
                {
                    var prms = new List<CommonParameter>();
                    prms.Add(new CommonParameter("IPADDRESS", ""));
                    prms.Add(new CommonParameter("PROCESSCD", ""));
                    prms.Add(new CommonParameter("ENROLL_NO", enrollNo));
                    prms.Add(new CommonParameter("PHOTO_FILE", citizenID + ".jpg"));
                    var result = db.ExecuteStored("ENR_SP_INSERT_ENROLL_PHOTO @IPADDRESS, @PROCESSCD, @ENROLL_NO, @PHOTO_FILE", prms, null);
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

        public async Task<ApplicationDetailModel> SubmitEditImage(UploadPhotoModel model)
        {
            try
            {
                EncryptHelper encryptHelper = new EncryptHelper();
                using (var db = new RegisterDB())
                {
                    var remark = "UPDATE: citizenID = " + model.CitizenID + " enrollNo = " + model.EnrollNo;

                    var prms = new List<CommonParameter>();
                    prms.Add(new CommonParameter("@ENROLL_NO", model.EnrollNo));
                    prms.Add(new CommonParameter("@CITIZEN_ID", encryptHelper.EncryptData(model.CitizenID)));
                    prms.Add(new CommonParameter("@TITLE_ID", ""));
                    prms.Add(new CommonParameter("@TITLE_NAME", ""));
                    prms.Add(new CommonParameter("@FNAME_TH", ""));
                    prms.Add(new CommonParameter("@LNAME_TH", ""));
                    prms.Add(new CommonParameter("@PROGRAM_ID", "Photo/Upload"));
                    prms.Add(new CommonParameter("@IP_ADDRESS", model.IP_ADDRESS));
                    prms.Add(new CommonParameter("@LOG_TYPE", "UPDATE_PHOTO"));
                    prms.Add(new CommonParameter("@REMARK", remark));
                    prms.Add(new CommonParameter("@BIRTH_DATE", ""));
                    prms.Add(new CommonParameter("@AGE_DAY", ""));
                    prms.Add(new CommonParameter("@AGE_MONTH", ""));
                    prms.Add(new CommonParameter("@AGE_YEAR", ""));
                    prms.Add(new CommonParameter("@GENDER", ""));
                    prms.Add(new CommonParameter("@PHOTO_FILE_NAME", model.ImageFileName));
                    var sql = "ENR_SP_INSERT_EDIT_APPLICATION_NEW_LOG @ENROLL_NO, @CITIZEN_ID, @TITLE_ID, @TITLE_NAME, @FNAME_TH, @LNAME_TH, @PROGRAM_ID, @IP_ADDRESS, @LOG_TYPE, @REMARK, @BIRTH_DATE, @AGE_DAY, @AGE_MONTH, @AGE_YEAR, @GENDER, @PHOTO_FILE_NAME";
                    var lst = await db.ExecuteStored<ApplicationDetailModel>(sql, prms).FirstOrDefaultAsync();
                    return lst;
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
