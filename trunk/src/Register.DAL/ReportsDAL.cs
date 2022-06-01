using CWN.OTAS.Lib.Helper;
using Register.Database;
using Register.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSP.Lib.Diagnostic;

namespace Register.DAL
{
    public class ReportsDAL : BaseDAL
    {
        public async Task<PayinModel> Payin(int testTypeID, string citizenID)
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
                    return await db.ExecuteStored<PayinModel>("ENR_SP_GET_PAYIN @TEST_TYPE_ID, @CITIZEN_ID", prms).FirstOrDefaultAsync();
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
