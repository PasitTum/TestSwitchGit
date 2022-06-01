using Register.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Register.WebAPI.Controllers
{
    public partial class RegisterController : BaseApiController
    {
        [HttpGet]
        [Route("DocumentToUploads")]
        public IHttpActionResult ListDocumentToUploads(int testtypeID, int testingClassID, string otherKey)
        {
            var dal = new RegisterDAL();
            var lst = dal.ListDocumentToUploads(testtypeID, testingClassID, otherKey);
            return Json(lst);
        }

    }
}
