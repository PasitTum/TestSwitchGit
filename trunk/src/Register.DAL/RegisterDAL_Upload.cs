using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSP.Lib.Diagnostic;
using CSP.Lib.Extension;

namespace Register.DAL
{
    using Database;
    public partial class RegisterDAL : BaseDAL
    {
        public IEnumerable<dynamic> ListDocumentToUploads(int testtypeID, int testingClassID, string otherKey)
        {
            try
            {
                using (var db = new RegisterDB())
                {
                    var prms = new Dictionary<string, object>();
                    prms.Add("@TEST_TYPE_ID", testtypeID);
                    prms.Add("@CLASS_ID", testingClassID);
                    prms.Add("@OTHER_KEY", otherKey.ToDBNull());
                    var sql = "ENR_SP_GET_UPLOAD_DOCUMENT_INFO @TEST_TYPE_ID, @CLASS_ID, @OTHER_KEY";
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
    }
}
