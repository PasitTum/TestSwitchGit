using CSP.Lib.Diagnostic;
using CSP.Lib.Models;
using Register.Database;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Register.DAL
{
    public class FaceDAL : BaseDAL
    {
        public ResultInfo InsertPhoto(string EnrollNo, byte[] image)
        {
            ResultInfo rtn = null;
            try
            {
                using (var db = new RegisterDB())
                {
                    var prms = new List<CommonParameter>();
                    prms.Add(new CommonParameter("IPADDRESS", ""));
                    prms.Add(new CommonParameter("PROCESSCD", ""));
                    prms.Add(new CommonParameter("ENROLL_NO", EnrollNo));
                    prms.Add(new CommonParameter("PHOTO_FILE", image));
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
    }
}
