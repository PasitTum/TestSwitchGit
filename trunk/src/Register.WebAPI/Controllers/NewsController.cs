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
    [RoutePrefix("api/News")]
    public class NewsController : BaseApiController
    {
        //[HttpGet]
        //[Route("SelectTypes")]
        //public IHttpActionResult NewsSelectType()
        //{
        //    var dal = new NewsDAL();
        //    var lst = dal.NewsSelectType();
        //    return Json(lst);
        //}

        [HttpGet]
        [Route("{testTypeID}/Slides")]
        public async Task<IHttpActionResult> NewsSlides(int testTypeID)
        {
            var dal = new NewsDAL();
            var lst = await dal.NewsSlides(testTypeID);
            return Json(lst);
        }

        [HttpGet]
        [Route("{testTypeID}")]
        public async Task<IHttpActionResult> News(int testTypeID)
        {
            var dal = new NewsDAL();
            var ipAddress = this.GetIp();
            var processCD = "";
            var lst = await dal.News(testTypeID, ipAddress, processCD);
            return Json(lst);
        }

        [HttpGet]
        [Route("{testTypeID}/Details/{newsID}")]
        public async Task<IHttpActionResult> NewsDetails(int testTypeID, int newsID)
        {
            var dal = new NewsDAL();
            var lst = await dal.NewsDetail(testTypeID, newsID);
            return Json(lst);
        }
    }
}
