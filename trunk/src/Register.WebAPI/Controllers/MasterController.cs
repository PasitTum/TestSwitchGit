using Newtonsoft.Json;
using Register.DAL;
using Register.Models;
using Register.WebAPI.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Register.WebAPI.Controllers
{
    [RoutePrefix("api/Master")]
    public class MasterController : ApiController
    {
        [HttpGet]
        [Route("ExamTypeHomes")]
        public IHttpActionResult GetExamTypeHome()
        {
            var dal = new MasterDAL();
            var lst = dal.GetExamTypeHome();
            return Json(lst);
        }

        [HttpGet]
        [Route("ExamTypeOptionals")]
        public IHttpActionResult GetExamTypeOptional()
        {
            var dal = new MasterDAL();
            var lst = dal.GetExamTypeOptional();
            return Json(lst);
        }

        [HttpGet]
        [Route("RegConfigs/TestTypeID/{testTypeID}")]
        public IHttpActionResult GetRegConfig(int testTypeID)
        {
            var dal = new MasterDAL();
            var lst = dal.GetRegConfig(testTypeID);
            return Json(lst);
        }

        [HttpGet]
        [Route("TestTypeID/{testTypeID}/ExamCenters")]
        public async Task<IHttpActionResult> GetExamCenter(int testTypeID)
        {
            var dal = new MasterDAL();
            var lst = await dal.GetExamCenterAsync(testTypeID);
            return Json(lst);
        }

        [HttpGet]
        [Route("TestTypeID/{testTypeID}/ExamCenters/{centerID}/TestingClasses")]
        public async Task<IHttpActionResult> GetTestingClasseseByCenter(int testTypeID, int centerID)
        {
            var dal = new MasterDAL();
            var lst = await dal.GetTestingClassAsync(testTypeID, 0, centerID);
            return Json(lst);
        }

        [HttpGet]
        [Route("TestTypeID/{testTypeID}/ExamCenters/{centerID}")]
        public async Task<IHttpActionResult> GetTestingClassInfoByCenter(int testTypeID, int centerID)
        {
            var dal = new MasterDAL();
            var lst = await dal.GetTestingClassInfoAsync(testTypeID, centerID);
            return Json(lst);
        }

        [HttpGet]
        [Route("Provinces/{provID}/Amphurs")]
        public IHttpActionResult GetAmphur(string provID)
        {
            var dal = new MasterDAL();
            var lst = dal.GetAmphur(provID);
            return Json(lst);
        }


        [HttpGet]
        [Route("Amphurs/{amphID}/Tumbons")]
        public IHttpActionResult GetTumbon(string amphID)
        {
            var dal = new MasterDAL();
            var lst = dal.GetTumbon(amphID);
            return Json(lst);
        }

        [HttpGet]
        [Route("Prefixes")]
        public IHttpActionResult GetPrefix()
        {
            var dal = new MasterDAL();
            var lst = dal.GetPrefix();
            return Json(lst);
        }

        [HttpGet]
        [Route("Genders")]
        public IHttpActionResult GetGender()
        {
            var dal = new MasterDAL();
            var lst = dal.GetGender();
            return Json(lst);
        }


        [HttpGet]
        [Route("Religions")]
        public IHttpActionResult GetReligion()
        {
            var dal = new MasterDAL();
            var lst = dal.GetReligion();
            return Json(lst);
        }

        [HttpGet]
        [Route("Statuses")]
        public IHttpActionResult GetStatus()
        {
            var dal = new MasterDAL();
            var lst = dal.GetStatus();
            return Json(lst);
        }

        //[HttpGet]
        //[Route("Remarks")]
        //public IHttpActionResult GetRemark()
        //{
        //    var dal = new MasterDAL();
        //    var lst = dal.GetRemark();
        //    return Json(lst);
        //}

        [HttpGet]
        [Route("Educationals/{classLevelID}")]
        public IHttpActionResult GetEducational(int classLevelID)
        {
            var dal = new MasterDAL();
            var lst = dal.GetEducational(classLevelID);
            return Json(lst);
        }

        [HttpGet]
        [Route("OCSCEducationals/TestTypeID/{testTypeID}/ClassLevelID/{classLevelID}")]
        public IHttpActionResult GetOCSCEducational(int testTypeID, int classLevelID)
        {
            var dal = new MasterDAL();
            var lst = dal.GetOCSCEducational(testTypeID, classLevelID);
            return Json(lst);
        }

        [HttpGet]
        [Route("HighestEducationals/{classLevelID}")]
        public IHttpActionResult GetHighestEducational(int classLevelID)
        {
            var dal = new MasterDAL();
            var lst = dal.GetHighestEducational(classLevelID);
            return Json(lst);
        }

        [HttpGet]
        [Route("SchoolGroups")]
        public IHttpActionResult GetSchoolGroup()
        {
            var dal = new MasterDAL();
            var lst = dal.GetSchoolGroup();
            return Json(lst);
        }

        [HttpGet]
        [Route("SchoolLocations")]
        public IHttpActionResult GetSchoolLocation()
        {
            var dal = new MasterDAL();
            var lst = dal.GetSchoolLocation();
            return Json(lst);
        }

        [HttpGet]
        [Route("Schools/SchoolGroup/{schGrpID}")]
        public IHttpActionResult GetSchool(int schGrpID)
        {
            var dal = new MasterDAL();
            var lst = dal.GetSchool(schGrpID);
            return Json(lst);
        }

        [HttpGet]
        [Route("RegClassID/{regClassID}/ClassLevelID/{classLevelID}/Degrees")]
        public IHttpActionResult GetDegree(int regClassID, int classLevelID)
        {
            var dal = new MasterDAL();
            var lst = dal.GetDegree(regClassID, classLevelID);
            return Json(lst);
        }

        [HttpGet]
        [Route("RegClassID/{regClassID}/ClassLevelID/{classLevelID}/DegreeID/{degreeID}/Majors")]
        public IHttpActionResult GetMajor(int regClassID, int classLevelID, string degreeID)
        {
            var dal = new MasterDAL();
            var lst = dal.GetMajor(regClassID, classLevelID, degreeID);
            return Json(lst);
        }

        [HttpGet]
        [Route("Occupations")]
        public IHttpActionResult GetOccupation()
        {
            var dal = new MasterDAL();
            var lst = dal.GetOccupation();
            return Json(lst);
        }


        [HttpGet]
        [Route("EduYears")]
        public IHttpActionResult GetEduYear()
        {
            var dal = new MasterDAL();
            var lst = dal.GetEduYear();
            return Json(lst);
        }

        [HttpGet]
        [Route("DefectiveHelps")]
        public IHttpActionResult GetDefectiveHelp()
        {
            var dal = new MasterDAL();
            var lst = dal.GetDefectiveHelp();
            return Json(lst);
        }

        [HttpGet]
        [Route("Questionaires/TestTypeID/{testTypeID}")]
        public IHttpActionResult GetQuestionaire(int testTypeID)
        {
            var dal = new MasterDAL();
            var lst = dal.GetQuestionaire(testTypeID);
            return Json(lst);
        }

        [HttpGet]
        [Route("TestTypeID/{testTypeID}/Schedules")]
        //[Route("Schedule/TestTypeID/{testTypeID}")]
        public async Task<IHttpActionResult> Schedule(int testTypeID)
        {
            var dal = new MasterDAL();
            var lst = await dal.GetSchedule(testTypeID);
            return Json(lst);
        }

        [HttpGet]
        [Route("TestTypeID/{testTypeID}/ClassGroupID/{classGroupID}/Classes")]
        public IHttpActionResult GetClassese(int testTypeID, int classGroupID)
        {
            var dal = new MasterDAL();
            var lst = dal.GetClass(testTypeID, classGroupID);
            return Json(lst);
        }

        [HttpGet]
        [Route("TestTypeID/{testTypeID}/TeachClasses")]
        public IHttpActionResult GetTeachClasses(int testTypeID)
        {
            var dal = new MasterDAL();
            var lst = dal.GetTeachClasses(testTypeID);
            return Json(lst);
        }

        [HttpGet]
        [Route("TestTypeID/{testTypeID}/ExamSite")]
        public IHttpActionResult GetExamSite(int testTypeID)
        {
            var dal = new MasterDAL();
            var lst = dal.GetExamSite(testTypeID);
            return Json(lst);
        }

        [HttpGet]
        [Route("TestTypeID/{testTypeID}/ClassGroups")]
        public IHttpActionResult GetClassGroups(int testTypeID)
        {
            var dal = new MasterDAL();
            var lst = dal.GetClassGroup(testTypeID);
            return Json(lst);
        }


        [HttpGet]
        [Route("TestTypeID/{testTypeID}/ClassGroupID/{classGroupID}/Certs")]
        public IHttpActionResult GetCerts(int testTypeID, int classGroupID)
        {
            var dal = new MasterDAL();
            var lst = dal.GetCertTeacher(testTypeID, classGroupID);
            return Json(lst);
        }
    }
}
