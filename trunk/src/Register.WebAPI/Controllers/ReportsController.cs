using Register.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Register.WebAPI.Controllers
{
    [RoutePrefix("api/Reports")]
    public class ReportsController : ApiController
    {
        [HttpGet]
        [Route("Payin")]
        public async Task<IHttpActionResult> Payin(int testTypeID, string citizenID)
        {
            var dal = new ReportsDAL();
            var lst = await dal.Payin(testTypeID, citizenID);
            return Json(lst);
        }
    }
}
