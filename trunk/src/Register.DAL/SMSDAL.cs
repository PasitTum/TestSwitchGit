using CSP.Lib.Diagnostic;
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
    public class SMSDAL : BaseDAL
    {
        public async Task<SMSStatusModel> SMSStatus(SmsInfo model)
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
                    prms.Add(new CommonParameter("SMS_REF_1", model.RefNo1));
                    prms.Add(new CommonParameter("SMS_PAYMENT", model.ErrorMessage));
                    prms.Add(new CommonParameter("IPADDRESS", ""));
                    prms.Add(new CommonParameter("PROCESSCD", ""));
                    return await db.ExecuteStored<SMSStatusModel>("ENR_SP_GET_REGISTER_SMS_STATUS @TEST_TYPE_ID, @CITIZEN_ID, @SMS_REF_1, @SMS_PAYMENT, @IPADDRESS, @PROCESSCD", prms).FirstOrDefaultAsync();
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
