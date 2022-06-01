using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Register.Database;
using Register.Models;
using System.Data.SqlClient;
using CSP.Lib.Diagnostic;

namespace Register.DAL
{
    public class DopaDAL : BaseDAL
    {
        public async Task AddLogValidateDopaAsync(DopaLogModel model)
        {
            try
            {
                using (var db = new RegisterDB())
                {
                    var prms = new List<SqlParameter>();
                    prms.Add(new SqlParameter("@USER_ID", model.USER_ID ?? ""));
                    prms.Add(new SqlParameter("@LOGTYPE", model.LOG_TYPE ?? ""));
                    prms.Add(new SqlParameter("@LOGDESC", model.LOG_DESC ?? ""));
                    prms.Add(new SqlParameter("@RESULT_STATUS", model.RESULT_STATUS ?? ""));
                    prms.Add(new SqlParameter("@RESULT_DESC", model.RESULT_DESC ?? ""));
                    prms.Add(new SqlParameter("@PROGRAM_ID", model.PROGRAM_ID ?? ""));
                    prms.Add(new SqlParameter("@IPADDRESS", model.IP_ADDRESS ?? ""));
                    prms.Add(new SqlParameter("@MACADDRESS", model.MAC_ADDRESS ?? ""));
                    await db.Database.ExecuteSqlCommandAsync("AUT_SP_INSERT_LOG_VALIDATE_DOPA @USER_ID,@LOGTYPE,@LOGDESC,@RESULT_STATUS,@RESULT_DESC,@PROGRAM_ID,@IPADDRESS,@MACADDRESS", prms.ToArray());
                }
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, ex);
            }
           
        }
    }
}
