using Register.Database;
using Register.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CSP.Lib.Diagnostic;
using System.Data.SqlClient;

namespace Register.DAL
{
    public class MasterDAL : BaseDAL
    {
        public IEnumerable<dynamic> GetExamTypeHome()
        {
            try
            {

                using (var db = new RegisterDB())
                {
                    var prms = new Dictionary<string, object>();
                    return db.DynamicListFromSql("ENR_SP_GET_EXAM_TYPE_HOME", prms).ToList();
                }
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, ex);
            }
            return null;
        }

        public IEnumerable<dynamic> GetExamTypeOptional()
        {
            try
            {
                using (var db = new RegisterDB())
                {
                    var prms = new Dictionary<string, object>();
                    return db.DynamicListFromSql("ENR_SP_GET_EXAM_TYPE_OPTIONAL", prms).ToList();
                }
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, ex);
            }
            return null;

        }

        public IEnumerable<dynamic> GetRegConfig(int testTypeID)
        {
            try
            {
                using (var db = new RegisterDB())
                {
                    var prms = new Dictionary<string, object>();
                    prms.Add("@TEST_TYPE_ID", testTypeID);
                    return db.DynamicListFromSql("ENR_SP_GET_REG_CONFIG_TEST_TYPE @TEST_TYPE_ID", prms).ToList();
                }
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, ex);
            }
            return null;

        }

        //public IEnumerable<dynamic> GetExamCenter(int testTypeID)
        //{
        //    using (var db = new RegisterDB())
        //    {
        //        var prms = new Dictionary<string, object>();
        //        prms.Add("@TEST_TYPE_ID", testTypeID);
        //        return db.DynamicListFromSql("ENR_SP_GET_REG_CENTER_INFO @TEST_TYPE_ID", prms).Select(p => new LookupModel()
        //        {
        //            id = p.CENTER_ID,
        //            text = p.CENTER_NAME
        //        }).ToList();
        //    }
        //}

        public async Task<IEnumerable<CenterExamModel>> GetExamCenterAsync(int testTypeID)
        {
            try
            {
                using (var db = new RegisterDB())
                {
                    var prms = new List<SqlParameter>();
                    prms.Add(new SqlParameter("@TEST_TYPE_ID", testTypeID));
                    var sql = "ENR_SP_GET_REG_CENTER_INFO @TEST_TYPE_ID";
                    return await db.Database.SqlQuery<CenterExamModel>(sql, prms.ToArray()).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, ex);
            }
            return null;
        }

        public async Task<IEnumerable<TestingClassModel>> GetTestingClassAsync(int testTypeID, int classLevelID, int centerID)
        {
            try
            {
                using (var db = new RegisterDB())
                {
                    var prms = new List<SqlParameter>();
                    prms.Add(new SqlParameter("@TEST_TYPE_ID", testTypeID));
                    prms.Add(new SqlParameter("@CLASS_LEVEL_ID", classLevelID));
                    prms.Add(new SqlParameter("@CENTER_EXAM_ID", centerID));
                    var sql = "ENR_SP_GET_CLASS_BY_CLASS_LEVEL @TEST_TYPE_ID, @CLASS_LEVEL_ID, @CENTER_EXAM_ID";
                    return await db.Database.SqlQuery<TestingClassModel>(sql, prms.ToArray()).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, ex);
            }
            return null;
        }

        public async Task<TestingClassInfoModel> GetTestingClassInfoAsync(int testTypeID, int centerID)
        {
            try
            {
                using (var db = new RegisterDB())
                {
                    var prms = new List<SqlParameter>();
                    prms.Add(new SqlParameter("@TEST_TYPE_ID", testTypeID));
                    prms.Add(new SqlParameter("@CENTER_EXAM_ID", centerID));
                    var sql = "ENR_SP_GET_REG_CENTER_INFO_PROVINCE_DTL @TEST_TYPE_ID, @CENTER_EXAM_ID";
                    return await db.Database.SqlQuery<TestingClassInfoModel>(sql, prms.ToArray()).FirstOrDefaultAsync();
                }
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, ex);
            }
            return null;
        }

        public IEnumerable<dynamic> GetProvince()
        {
            try
            {
                using (var db = new RegisterDB())
                {
                    var prms = new Dictionary<string, object>();
                    return db.DynamicListFromSql("ENR_SP_GET_REG_PROVINCE_INFO", prms).Select(p => new LookupModel()
                    {
                        id = p.PROV_ID,
                        text = p.PROV_NAME
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, ex);
            }
            return null;
        }

        public IEnumerable<dynamic> GetAmphur(string provID)
        {
            try
            {
                using (var db = new RegisterDB())
                {
                    var prms = new Dictionary<string, object>();
                    prms.Add("@PROVINCE_ID", provID);
                    return db.DynamicListFromSql("ENR_SP_GET_REG_AMPHUR_INFO @PROVINCE_ID", prms).Select(p => new LookupModel()
                    {
                        id = p.AMPHUR_ID,
                        text = p.AMPHUR_NAME_TH
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, ex);
            }
            return null;
        }

        public IEnumerable<dynamic> GetTumbon(string amphID)
        {
            try
            {
                using (var db = new RegisterDB())
                {
                    var prms = new Dictionary<string, object>();
                    prms.Add("@AMPHUR_ID", amphID);
                    return db.DynamicListFromSql("ENR_SP_GET_REG_TUMBOM_INFO @AMPHUR_ID", prms).Select(p => new LookupModel()
                    {
                        id = p.TUMBON_ID,
                        text = p.TUMBON_NAME_TH
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, ex);
            }
            return null;
        }

        public IEnumerable<dynamic> GetAllTumbons()
        {
            try
            {
                using (var db = new RegisterDB())
                {
                    var sql = "select TUMBON_ID, TUMBON_NAME_TH from [dbo].[MST_TUMBON] where active_flag = 'Y'";
                    return db.DynamicListFromSql(sql).Select(p => new LookupModel()
                    {
                        id = p.TUMBON_ID,
                        text = p.TUMBON_NAME_TH
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, ex);
            }
            return null;
        }

        public IEnumerable<dynamic> GetPrefix()
        {
            try
            {
                using (var db = new RegisterDB())
                {
                    var prms = new Dictionary<string, object>();
                    return db.DynamicListFromSql("ENR_SP_GET_REG_PREFIX_INFO", prms).Select(p => new LookupPrefixModel()
                    {
                        id = p.TITLE_ID.ToString(),
                        text = p.TITLE_NAME_TH,
                        gender = p.GENDER
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, ex);
            }
            return null;
        }

        public IEnumerable<dynamic> GetGender()
        {
            try
            {
                using (var db = new RegisterDB())
                {
                    var prms = new Dictionary<string, object>();
                    return db.DynamicListFromSql("ENR_SP_GET_REG_GENDER", prms).Select(p => new LookupModel()
                    {
                        id = p.GENDER_ID,
                        text = p.GENDER_NAME
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, ex);
            }
            return null;
        }

        public IEnumerable<dynamic> GetReligion()
        {
            try
            {
                using (var db = new RegisterDB())
                {
                    var prms = new Dictionary<string, object>();
                    return db.DynamicListFromSql("ENR_SP_GET_REG_RELIGION_INFO", prms).Select(p => new LookupModel()
                    {
                        id = p.RELIGION_ID,
                        text = p.RELIGION_NAME
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, ex);
            }
            return null;
        }
        public IEnumerable<dynamic> GetStatus()
        {
            try
            {
                using (var db = new RegisterDB())
                {
                    var prms = new Dictionary<string, object>();
                    return db.DynamicListFromSql("ENR_SP_GET_REG_STATUS_INFO", prms).Select(p => new LookupModel()
                    {
                        id = p.STATUS_ID,
                        text = p.STATUS_NAME
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, ex);
            }
            return null;
        }

        //public IEnumerable<dynamic> GetRemark()
        //{
        //    ListNewsModel[] remark = new ListNewsModel[3];
        //    remark[0] = new ListNewsModel { Detail = "1. กรุณาระบุข้อมูลให้ครบถ้วน หากไม่สามารถระบุได้ให้ใส่เครื่องหมาย -" };
        //    remark[1] = new ListNewsModel { Detail = "2. กรณีที่ไปชำระค่าธรรมเนียมในการสมัครสอบ ปรากฏว่าศูนย์สอบที่เลือกไว้ครบจำนวนแล้ว ผู้สมัครสอบจะต้องเลือกศูนย์สอบอื่นที่ยังมีที่นั่งเหลืออยู่แทน" };
        //    remark[2] = new ListNewsModel { Detail = "3. การสมัครสอบและเลือกศูนย์สอบจะสมบูรณ์ต่อเมื่อผู้สมัครสอบชำระค่าธรรมเนียมในการสมัครสอบแล้ว" };

        //    using (var db = new RegisterDB())
        //    {
        //        return remark.ToList();

        //    }
        //}

        public IEnumerable<dynamic> GetEducational(int ClassLevelID)
        {
            try
            {
                using (var db = new RegisterDB())
                {
                    var prms = new Dictionary<string, object>();
                    prms.Add("@IPADDRESS", "");
                    prms.Add("@MACADDRESS", "");
                    prms.Add("@PROCESSCD", "");
                    prms.Add("@TESTING_CLASS_ID", ClassLevelID);
                    return db.DynamicListFromSql("ENR_SP_GET_REG_CLASS_LEVEL_INFO @IPADDRESS, @MACADDRESS, @PROCESSCD, @TESTING_CLASS_ID", prms).Select(p => new LookupClassLevelModel()
                    {
                        id = p.CLASS_ID.ToString(),
                        text = p.CLASS_NAME,
                        title = p.TITLE_CLASS_NAME
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, ex);
            }
            return null;
        }

        public IEnumerable<dynamic> GetOCSCEducational(int testTypeID, int ClassLevelID)
        {
            try
            {
                using (var db = new RegisterDB())
                {
                    var prms = new Dictionary<string, object>();
                    prms.Add("@CLASS_LEVEL_ID", ClassLevelID);
                    prms.Add("@TEST_TYPE_ID", testTypeID);
                    return db.DynamicListFromSql("ENR_SP_GET_CLASS_OCS @CLASS_LEVEL_ID, @TEST_TYPE_ID", prms).Select(p => new LookupClassLevelModel()
                    {
                        id = p.CLASS_OCS_ID.ToString(),
                        text = p.CLASS_OCS_NAME_TH,
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, ex);
            }
            return null;
        }

        public IEnumerable<dynamic> GetHighestEducational(int ClassLevelID)
        {
            try
            {
                using (var db = new RegisterDB())
                {
                    var prms = new Dictionary<string, object>();
                    prms.Add("@IPADDRESS", "");
                    prms.Add("@MACADDRESS", "");
                    prms.Add("@PROCESSCD", "");
                    prms.Add("@CLASS_LEVEL_ID", ClassLevelID);
                    return db.DynamicListFromSql("ENR_SP_GET_REG_HIGHEST_CLASS_LEVEL @IPADDRESS, @MACADDRESS, @PROCESSCD, @CLASS_LEVEL_ID", prms).Select(p => new LookupModel()
                    {
                        id = p.HIGHEST_CLASS_LEVEL_ID.ToString(),
                        text = p.HIGHEST_CLASS_LEVEL_NAME
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, ex);
            }
            return null;
        }

        public IEnumerable<dynamic> GetSchoolGroup()
        {
            try
            {
                using (var db = new RegisterDB())
                {
                    var prms = new Dictionary<string, object>();
                    return db.DynamicListFromSql("ENR_SP_GET_SCHOOL_GROUP", prms).Select(p => new LookupModel()
                    {
                        id = p.SCHOOL_GROUP_ID.ToString(),
                        text = p.SCHOOL_GROUP_NAME_TH
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, ex);
            }
            return null;
        }

        public IEnumerable<dynamic> GetSchoolLocation()
        {
            try
            {
                using (var db = new RegisterDB())
                {
                    var prms = new Dictionary<string, object>();
                    return db.DynamicListFromSql("ENR_SP_GET_SCHOOL_LOCATION", prms).Select(p => new LookupModel()
                    {
                        id = p.SCHOOL_LOCATION_CODE,
                        text = p.SCHOOL_LOCATION
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, ex);
            }
            return null;
        }

        public IEnumerable<dynamic> GetSchool(int schGrpID)
        {
            try
            {
                using (var db = new RegisterDB())
                {
                    var prms = new Dictionary<string, object>();
                    prms.Add("@SCHOOL_GROUP_ID", schGrpID);
                    return db.DynamicListFromSql("ENR_SP_GET_SCHOOL @SCHOOL_GROUP_ID", prms).Select(p => new LookupModel()
                    {
                        id = p.SCHOOL_ID.ToString(),
                        text = p.SCHOOL_NAME_TH
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, ex);
            }
            return null;
        }

        public IEnumerable<dynamic> GetDegree(int regClassID, int classLevelID)
        {
            try
            {
                using (var db = new RegisterDB())
                {
                    var prms = new Dictionary<string, object>();
                    prms.Add("@CLASS_LEVEL_ID", classLevelID);
                    prms.Add("@TEST_CLASS_ID", regClassID);
                    return db.DynamicListFromSql("ENR_SP_GET_REG_DEGREE_ALL_INFO @CLASS_LEVEL_ID, @TEST_CLASS_ID", prms).Select(p => new LookupModel()
                    {
                        id = p.DEGREE_ID.ToString(),
                        text = p.DEGREE_NAME
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, ex);
            }
            return null;
        }

        public IEnumerable<dynamic> GetMajor(int regClassID, int classLevelID, string degreeID)
        {
            try
            {
                using (var db = new RegisterDB())
                {
                    var prms = new Dictionary<string, object>();
                    prms.Add("@CLASS_LEVEL_ID", classLevelID);
                    prms.Add("@TEST_CLASS_ID", regClassID);
                    prms.Add("@DEGREE_ID", degreeID);
                    return db.DynamicListFromSql("ENR_SP_GET_REG_MAJOR_ALL_INFO @CLASS_LEVEL_ID, @TEST_CLASS_ID, @DEGREE_ID", prms).Select(p => new LookupModel()
                    {
                        id = p.MAJOR_ID.ToString(),
                        text = p.MAJOR_NAME
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, ex);
            }
            return null;
        }

        public IEnumerable<dynamic> GetOccupation()
        {
            try { 
            using (var db = new RegisterDB())
            {
                var prms = new Dictionary<string, object>();
                return db.DynamicListFromSql("ENR_SP_GET_REG_OCCUPATION_INFO", prms).Select(p => new LookupModel()
                {
                    id = p.OCCU_ID,
                    text = p.OCCU_NAME
                }).ToList();
            }
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, ex);
            }
            return null;
        }

        public IEnumerable<dynamic> GetEduYear()
        {try { 
            using (var db = new RegisterDB())
            {
                var prms = new Dictionary<string, object>();
                return db.DynamicListFromSql("ENR_SP_GET_EDU_YEAR", prms).Select(p => new LookupModel()
                {
                    id = p.EDU_YEAR.ToString(),
                    text = p.REMARK
                }).ToList();
            }
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, ex);
            }
            return null;
        }

        public IEnumerable<dynamic> GetDefectiveHelp()
        {try { 
            using (var db = new RegisterDB())
            {
                var prms = new Dictionary<string, object>();
                return db.DynamicListFromSql("ENR_SP_GET_DEFECTIVE_HELP", prms).Select(p => new LookupModel()
                {
                    id = p.DEFECTIVE_HELP_ID.ToString(),
                    text = p.DEFECTIVE_HELP_NAME
                }).ToList();
            }
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, ex);
            }
            return null;
        }

        public IEnumerable<dynamic> GetQuestionaire(int testTypeID)
        {
            try { 
            using (var db = new RegisterDB())
            {
                var prms = new Dictionary<string, object>();
                prms.Add("@TEST_TYPE_ID", testTypeID);
                return db.DynamicListFromSql("QUE_SP_GET_QUESTIONAIRE @TEST_TYPE_ID", prms).ToList();
            }
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, ex);
            }
            return null;
        }


        public async Task<List<CalendarModel>> GetSchedule(int testTypeID)
        {
            try
            {
                using (var db = new RegisterDB())
                {
                    var prms = new List<CommonParameter>();
                    prms.Add(new CommonParameter("TEST_TYPE_ID", testTypeID));
                    return await db.ExecuteStored<CalendarModel>("PRE_SP_GET_SCHEDULE_OPENMENU @TEST_TYPE_ID", prms).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, ex);
                throw ex;
            }
        }

        public async Task<IEnumerable<TestingClassModel>> GetClassAsync(int testTypeID)
        {
            try
            {
                using (var db = new RegisterDB())
                {
                    var prms = new List<SqlParameter>();
                    prms.Add(new SqlParameter("@TEST_TYPE_ID", testTypeID));
                    var sql = "ENR_SP_GET_CLASS @TEST_TYPE_ID";
                    return await db.Database.SqlQuery<TestingClassModel>(sql, prms.ToArray()).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, ex);
            }
            return null;
        }

        public IEnumerable<dynamic> GetClass(int testTypeID, int classGroupID)
        {
            try
            {
                using (var db = new RegisterDB())
                {
                    var prms = new Dictionary<string, object>();
                    prms.Add("@TEST_TYPE_ID", testTypeID);
                    prms.Add("@CLASS_GROUP_ID", classGroupID == -1 ? DBNull.Value: (object)classGroupID);
                    return db.DynamicListFromSql("ENR_SP_GET_CLASS @TEST_TYPE_ID, @CLASS_GROUP_ID", prms).Select(p => new LookupClassModel()
                    {
                        id = p.CLASS_ID.ToString(),
                        text = p.CLASS_SUBJECT,
                        name = p.CLASS_NAME,
                        sbjgroup = p.SBJ_GROUP.ToString(),
                        groupid = p.CLASS_GROUP_ID.ToString()
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, ex);
            }
            return null;
        }

        public IEnumerable<dynamic> GetTeachClasses(int testTypeID)
        {
            try
            {
                using (var db = new RegisterDB())
                {
                    var prms = new Dictionary<string, object>();
                    prms.Add("@TEST_TYPE_ID", testTypeID);
                    return db.DynamicListFromSql("ENR_SP_GET_TEACH_CLASS_LEVEL  @TEST_TYPE_ID", prms).Select(p => new LookupModel()
                    {
                        id = p.TEACH_CLASS_LEVEL_ID.ToString(),
                        text = p.TEACH_CLASS_LEVEL_NAME,
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, ex);
            }
            return null;
        }

        public IEnumerable<dynamic> GetExamSite(int testTypeID)
        {
            try
            {
                using (var db = new RegisterDB())
                {
                    var prms = new Dictionary<string, object>();
                    prms.Add("@TEST_TYPE_ID", testTypeID);
                    return db.DynamicListFromSql("ENR_SP_GET_EXAM_SITE_NAME  @TEST_TYPE_ID", prms).Select(p => new LookupModel()
                    {
                        id = p.EXAM_SITE_ID.ToString(),
                        text = p.EXAM_SITE_NAME,
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, ex);
            }
            return null;
        }
        
        public IEnumerable<dynamic> GetClassGroup(int testTypeID)
        {
            try
            {
                using (var db = new RegisterDB())
                {
                    var prms = new Dictionary<string, object>();
                    prms.Add("@TEST_TYPE_ID", testTypeID);
                    return db.DynamicListFromSql("ENR_SP_GET_CLASS_GROUP  @TEST_TYPE_ID", prms).Select(p => new LookupModel()
                    {
                        id = p.CLASS_GROUP_ID.ToString(),
                        text = p.CLASS_GROUP_DESC,
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, ex);
            }
            return null;
        }

        public IEnumerable<dynamic> GetCertTeacher(int testTypeID, int classGroupID)
        {
            try
            {
                using (var db = new RegisterDB())
                {
                    var prms = new Dictionary<string, object>();
                    prms.Add("@TEST_TYPE_ID", testTypeID);
                    prms.Add("@CLASS_GROUP_ID", classGroupID);
                    return db.DynamicListFromSql("ENR_SP_GET_CERT_TEACHER @TEST_TYPE_ID, @CLASS_GROUP_ID", prms).Select(p => new LookupClassModel()
                    {
                        id = p.DOC_TYPE_ID.ToString(),
                        text = p.DOC_TYPE
                    }).ToList();
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
