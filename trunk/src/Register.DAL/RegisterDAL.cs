using CSP.Lib.Diagnostic;
using CSP.Lib.Extension;
using CSP.Lib.Models;
using CWN.OTAS.Lib.Helper;
using Register.Database;
using Register.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Register.DAL
{
    public partial class RegisterDAL : BaseDAL
    {
        public async Task<ValidateCitizenIDModel> ValidateCitizenID(int testTypeID, string examType, string citizenID, string laserCode)
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
                    prms.Add(new CommonParameter("IPADDRESS", ""));
                    prms.Add(new CommonParameter("MACADDRESS", ""));
                    prms.Add(new CommonParameter("PROCESSCD", ""));
                    prms.Add(new CommonParameter("LOG_TYPE", ""));
                    prms.Add(new CommonParameter("TEST_TYPE_ID", testTypeID));
                    prms.Add(new CommonParameter("EXAM_TYPE", examType));
                    prms.Add(new CommonParameter("CITIZEN_ID", citizen));
                    prms.Add(new CommonParameter("LASER_CODE", laserCode));
                    return await db.ExecuteStored<ValidateCitizenIDModel>("ENR_SP_GET_CHECK_ENROLL @IPADDRESS, @MACADDRESS, @PROCESSCD, @LOG_TYPE, @TEST_TYPE_ID, @EXAM_TYPE, @CITIZEN_ID, @LASER_CODE", prms).FirstOrDefaultAsync();
                }
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, ex);
            }
            return null;
        }

        public ResultInfo SetEnrollInfo(RegisterModel model)
        {
            var rtn = new ResultInfo();
            EncryptHelper encryptHelper = new EncryptHelper();
            var citizen = string.Empty;
            if (model.CitizenID != null & model.CitizenID != "")
            {
                citizen = encryptHelper.EncryptData(model.CitizenID.ToString());
            }
            model.Salary = model.Salary.Replace(",", "");
            try
            {
                using (var db = new RegisterDB())
                {
                    var prms = new List<CommonParameter>();
                    prms.Add(new CommonParameter("@TEST_TYPE_ID", model.TestTypeID));
                    prms.Add(new CommonParameter("@EXAM_TYPE", model.ExamType));
                    prms.Add(new CommonParameter("@CITIZEN_ID", citizen));
                    prms.Add(new CommonParameter("@REAL_CITIZEN_ID", model.CitizenID));
                    prms.Add(new CommonParameter("@LASER_CODE", model.LaserCode));
                    prms.Add(new CommonParameter("@TITLE_ID", model.PrefixID));
                    prms.Add(new CommonParameter("@TITLE_NAME", model.PrefixName));
                    prms.Add(new CommonParameter("@FNAME_TH", model.FirstName));
                    prms.Add(new CommonParameter("@LNAME_TH", model.LastName));
                    prms.Add(new CommonParameter("@BIRTH_DATE", model.BirthDate.ToDateTimeFromThai()));
                    prms.Add(new CommonParameter("@AGE_DAY", model.AgeDay));
                    prms.Add(new CommonParameter("@AGE_MONTH", model.AgeMonth));
                    prms.Add(new CommonParameter("@AGE_YEAR", model.AgeYear));
                    prms.Add(new CommonParameter("@CENTER_EXAM_ID_TMP", model.CenterExamID));
                    prms.Add(new CommonParameter("@CITIZEN_PROVINCE_ID", model.CitizenProvinceID));
                    prms.Add(new CommonParameter("@GENDER", model.Gender));
                    prms.Add(new CommonParameter("@NATION_NAME", model.NationName));
                    prms.Add(new CommonParameter("@RACE_NAME", model.RaceName));
                    prms.Add(new CommonParameter("@RELIGION_ID", model.ReligionID));
                    prms.Add(new CommonParameter("@RELIGION_NAME", model.ReligionName));
                    prms.Add(new CommonParameter("@STATUS_ID", model.StatusID));
                    prms.Add(new CommonParameter("@STATUS_NAME", model.StatusName));
                    prms.Add(new CommonParameter("@CLASS_LEVEL_ID", model.ClassLavelID));
                    prms.Add(new CommonParameter("@CLASS_LEVEL_NAME", model.ClassLavelName));
                    prms.Add(new CommonParameter("@SCHOOL_GROUP_ID", model.SchoolGroupID));
                    prms.Add(new CommonParameter("@SCHOOL_FLAG", model.SchoolFlag));
                    prms.Add(new CommonParameter("@SCHOOL_ID", model.SchoolID));
                    prms.Add(new CommonParameter("@SCHOOL_NAME", model.SchoolName));
                    prms.Add(new CommonParameter("@SCHOOL_PROVINCE_ID", model.SchoolProvinceID));
                    prms.Add(new CommonParameter("@SCHOOL_COUNTRY_ID", model.SchoolCountryID));
                    prms.Add(new CommonParameter("@DEGREE_ID", model.DegreeID));
                    prms.Add(new CommonParameter("@DEGREE_NAME", model.DegreeName));
                    prms.Add(new CommonParameter("@MAJOR_NAME", model.MajorName));
                    prms.Add(new CommonParameter("@GRADUATED_FLAG", model.EducationFlag));
                    prms.Add(new CommonParameter("@GRADUATE_DATE", model.GraduateDate.ToDateTimeFromThai()));
                    prms.Add(new CommonParameter("@GRADUATE_GPA", model.GPA));
                    prms.Add(new CommonParameter("@STUDY_YEAR", model.StudyYear));
                    prms.Add(new CommonParameter("@EDU_YEAR", model.EduYear));
                    prms.Add(new CommonParameter("@DEFECTIVE_FLAG", model.DefectiveFlag));
                    prms.Add(new CommonParameter("@DEFECTIVE_HELP_FLAG", model.DefectiveHelpFlag));
                    prms.Add(new CommonParameter("@DEFECTIVE_HELP_ID", model.DefectiveHelpID));
                    prms.Add(new CommonParameter("@OCCUPATION_ID", model.OccuupationID));
                    prms.Add(new CommonParameter("@OCCUPATION_NAME", model.OccuupationID == "1" ? model.OccuupationName4Gov : model.OccuupationName));
                    prms.Add(new CommonParameter("@ADDRESS_NO", model.AddrNo));
                    prms.Add(new CommonParameter("@VILLAGE_NAME", model.Village));
                    prms.Add(new CommonParameter("@MOO", model.Moo));
                    prms.Add(new CommonParameter("@SOI", model.Soi));
                    prms.Add(new CommonParameter("@STREET", model.Road));
                    prms.Add(new CommonParameter("@PROVINCE_ID", model.ProvID));
                    prms.Add(new CommonParameter("@AMPHUR_ID", model.AmphID));
                    prms.Add(new CommonParameter("@TUMBON_ID", model.TmblID));
                    prms.Add(new CommonParameter("@POST_CODE", model.Zipcode));
                    prms.Add(new CommonParameter("@PHONE_NO", model.Tel));
                    prms.Add(new CommonParameter("@MOBILE_NO", model.Mobile));
                    prms.Add(new CommonParameter("@EMAIL", model.Email));
                    prms.Add(new CommonParameter("@CONTACT_NAME", model.EmergencyFirstName));
                    prms.Add(new CommonParameter("@CONTACT_SURNAME", model.EmergencyLastName));
                    prms.Add(new CommonParameter("@CONTACT_RELATION", model.EmergencyRelation));
                    prms.Add(new CommonParameter("@CONTACT_PHONE", model.EmergencyTel));
                    prms.Add(new CommonParameter("@CONTACT_MOBILE", model.EmergencyMoblie));
                    prms.Add(new CommonParameter("@ENROLL_PASSWORD", null));
                    prms.Add(new CommonParameter("@XML_QUESTIONAIRE", model.XMLQuestionaire));
                    prms.Add(new CommonParameter("@CREATE_BY", model.CitizenID));
                    prms.Add(new CommonParameter("@UPDATE_BY", model.CitizenID));
                    prms.Add(new CommonParameter("@IP_ADDRESS", model.IPAddress ?? ""));
                    prms.Add(new CommonParameter("@MAC_ADDRESS", model.MacAddress ?? ""));
                    prms.Add(new CommonParameter("@PROGRAM_ID", "Register"));

                    prms.Add(new CommonParameter("@CENTER_EXAM_ID", model.CenterExamID));
                    prms.Add(new CommonParameter("@TESTING_CLASS_ID", model.RegClassID));
                    prms.Add(new CommonParameter("@CARD_DATE_RELEASE", model.IssueDate.ToDateTimeFromThai()));
                    prms.Add(new CommonParameter("@CARD_DATE_EXPIRE", model.ExpiredDate.ToDateTimeFromThai()));
                    prms.Add(new CommonParameter("@CERT_TITLE_ID", model.EduPrefixID));
                    prms.Add(new CommonParameter("@CERT_TITLE_NAME", model.EduPrefixName));
                    prms.Add(new CommonParameter("@CERT_FNAME_TH", model.EduFirstName));
                    prms.Add(new CommonParameter("@CERT_LNAME_TH", model.EduLastName));
                    prms.Add(new CommonParameter("@PPT_EDU_STATUS_HIGHEST_DEGREE_NAME", model.ClassHighestLavelName));
                    prms.Add(new CommonParameter("@PPT_EDU_STATUS_HIGHEST_SCHOOL_NAME", model.ClassHighestLavelSchoolName));
                    prms.Add(new CommonParameter("@PPT_SPECIAL_SKILL", model.SpecialSkills));
                    prms.Add(new CommonParameter("@POSITION_NAME", model.PositionName));
                    prms.Add(new CommonParameter("@OFFICE_NAME", model.StationName));
                    prms.Add(new CommonParameter("@DIVISION_NAME", model.DepartmentName));
                    prms.Add(new CommonParameter("@OFFICE_PHONE_NO", model.OfficeTel));
                    prms.Add(new CommonParameter("@OFFICE_PHONE_EX", model.OfficeTelEx));
                    prms.Add(new CommonParameter("@OFFICE_MOBILE_NO", model.OfficeMobile));
                    prms.Add(new CommonParameter("@SARALY_AMT", model.Salary));
                    prms.Add(new CommonParameter("@TBB_ADDRESS_NO", model.TbbAddrNo));
                    prms.Add(new CommonParameter("@TBB_BUILDING", ""));
                    prms.Add(new CommonParameter("@TBB_ROOM", ""));
                    prms.Add(new CommonParameter("@TBB_MOO", model.TbbMoo));
                    prms.Add(new CommonParameter("@TBB_VILLAGE_NAME", model.TbbVillage));
                    prms.Add(new CommonParameter("@TBB_SOI", model.TbbSoi));
                    prms.Add(new CommonParameter("@TBB_JUNCTION", ""));
                    prms.Add(new CommonParameter("@TBB_STREET", model.TbbRoad));
                    prms.Add(new CommonParameter("@TBB_TUMBON_NAME", model.TbbTmblID));
                    prms.Add(new CommonParameter("@TBB_AMPHUR_NAME", model.TbbAmphID));
                    prms.Add(new CommonParameter("@TBB_PROVINCE_ID", model.TbbProvID));
                    prms.Add(new CommonParameter("@TBB_POST_CODE", model.TbbZipcode));
                    prms.Add(new CommonParameter("@TBB_PHONE_NO", model.TbbTel));
                    prms.Add(new CommonParameter("@TBB_PHONE_EX", model.TbbTelEx));
                    prms.Add(new CommonParameter("@PHONE_EX", model.TelEx));
                    prms.Add(new CommonParameter("@CONTACT_PHONE_EX", model.EmergencyTelEx));
                    prms.Add(new CommonParameter("@BIRTH_DATE_CHAR", model.BirthDateChar));

                    prms.Add(new CommonParameter("@HIGHEST_CLASS_NAME", model.ClassHighestLavelName));
                    prms.Add(new CommonParameter("@HIGHEST_DEGREE_NAME", ""));
                    prms.Add(new CommonParameter("@HIGHEST_MAJOR_NAME", ""));
                    prms.Add(new CommonParameter("@HIGHEST_SCHOOL_NAME", model.ClassHighestLavelSchoolName));
                    prms.Add(new CommonParameter("@HIGHEST_SCHOOL_PROVINCE", ""));
                    prms.Add(new CommonParameter("@HIGHEST_SCHOOL_FLAG", ""));
                    prms.Add(new CommonParameter("@HIGHEST_GRADUATE_DATE", null));
                    prms.Add(new CommonParameter("@HIGHEST_GRADUATE_GPA", ""));

                    prms.Add(new CommonParameter("@OCSC_LEVEL_ID", model.OCSCLevelID));
                    prms.Add(new CommonParameter("@OCSC_CERT_NO", model.OCSCCertNo));
                    prms.Add(new CommonParameter("@OCSC_EXAM_DTM", model.OCSCExamDate.ToDateTimeFromThai()));
                    prms.Add(new CommonParameter("@OCSC_PASS_DTM", model.OCSCPassDate.ToDateTimeFromThai()));
                    prms.Add(new CommonParameter("@PHOTO_FILE", model.CitizenID + ".jpg"));
                    prms.Add(new CommonParameter("@FACULTY_NAME", model.FacultyName));
                    prms.Add(new CommonParameter("@CLASS_GROUP_ID", model.ClassGroupID));
                    prms.Add(new CommonParameter("@DOC_TYPE_ID", model.DocTypeID));

                    prms.Add(new CommonParameter("@DIPLOMA_MAJOR", model.DiplomaMajor));
                    prms.Add(new CommonParameter("@DIPLOMA_ACADEMY", model.DiplomaAcademy));
                    prms.Add(new CommonParameter("@DIPLOMA_TEACHER_ACADEMY", model.DiplomaTeachAcademy));
                    prms.Add(new CommonParameter("@DIPLOMA_TEACHER_GRADUATE_DATE", model.DiplomaTeachGraduateDate.ToDateTimeFromThai()));
                    prms.Add(new CommonParameter("@CERT_TEACHER_NO", model.CertTeachNo));
                    prms.Add(new CommonParameter("@CERT_TEACHER_ISSUED_DATE", model.CertTeachIssuedDate.ToDateTimeFromThai()));
                    prms.Add(new CommonParameter("@CERT_TEACHER_EXPIRE_DATE", model.CertTeachExpireDate.ToDateTimeFromThai()));
                    prms.Add(new CommonParameter("@TEACH_CLASS_LEVEL_ID", model.TeachClassLevelID));
                    prms.Add(new CommonParameter("@EXAM_SITE_ID", model.ExamSiteID));

                    var prmsOut = new List<CommonParameter>();
                    prmsOut.Add(new CommonParameter("@ERRORNUM", DBNull.Value, System.Data.DbType.Int32, sizeof(Int32)));
                    prmsOut.Add(new CommonParameter("@ERRORMESS", DBNull.Value, System.Data.DbType.String, 4000));
                    prmsOut.Add(new CommonParameter("@REGISTER_NO", DBNull.Value, System.Data.DbType.String, 20));

                    // เพิ่มส่วนอับโหลดเอกสาร
                    var docXml = model.DOCS.ObjectToXml();
                    prms.Add(new CommonParameter("@DOCS_LIST", docXml));

                    var result = db.ExecuteStored("ENR_SP_SET_ENROLL_INFO @TEST_TYPE_ID, @EXAM_TYPE, @CITIZEN_ID, @REAL_CITIZEN_ID, @LASER_CODE, @TITLE_ID, @TITLE_NAME, @FNAME_TH, @LNAME_TH, @BIRTH_DATE, @AGE_DAY, @AGE_MONTH, @AGE_YEAR, @CENTER_EXAM_ID_TMP, @CITIZEN_PROVINCE_ID, @GENDER, @NATION_NAME, @RACE_NAME, @RELIGION_ID, @RELIGION_NAME, @STATUS_ID, @STATUS_NAME, @CLASS_LEVEL_ID, @CLASS_LEVEL_NAME, @SCHOOL_GROUP_ID, @SCHOOL_FLAG, @SCHOOL_ID, @SCHOOL_NAME, @SCHOOL_PROVINCE_ID, @SCHOOL_COUNTRY_ID, @DEGREE_ID, @DEGREE_NAME, @MAJOR_NAME, @GRADUATED_FLAG, @GRADUATE_DATE, @GRADUATE_GPA, @STUDY_YEAR, @EDU_YEAR, @DEFECTIVE_FLAG, @DEFECTIVE_HELP_FLAG, @DEFECTIVE_HELP_ID, @OCCUPATION_ID, @OCCUPATION_NAME, @ADDRESS_NO, @VILLAGE_NAME, @MOO, @SOI, @STREET, @PROVINCE_ID, @AMPHUR_ID, @TUMBON_ID, @POST_CODE, @PHONE_NO, @MOBILE_NO, @EMAIL, @CONTACT_NAME, @CONTACT_SURNAME, @CONTACT_RELATION, @CONTACT_PHONE, @CONTACT_MOBILE, @ENROLL_PASSWORD, @XML_QUESTIONAIRE, @CREATE_BY, @UPDATE_BY, @IP_ADDRESS, @MAC_ADDRESS, @PROGRAM_ID, @ERRORNUM OUTPUT, @ERRORMESS OUTPUT, @REGISTER_NO OUTPUT, @CENTER_EXAM_ID, @TESTING_CLASS_ID, @CARD_DATE_RELEASE, @CARD_DATE_EXPIRE, @CERT_TITLE_ID, @CERT_TITLE_NAME, @CERT_FNAME_TH, @CERT_LNAME_TH, @PPT_EDU_STATUS_HIGHEST_DEGREE_NAME, @PPT_EDU_STATUS_HIGHEST_SCHOOL_NAME, @PPT_SPECIAL_SKILL, @POSITION_NAME, @OFFICE_NAME, @DIVISION_NAME, @OFFICE_PHONE_NO, @OFFICE_PHONE_EX, @OFFICE_MOBILE_NO, @SARALY_AMT, @TBB_ADDRESS_NO, @TBB_BUILDING, @TBB_ROOM, @TBB_MOO, @TBB_VILLAGE_NAME, @TBB_SOI, @TBB_JUNCTION, @TBB_STREET, @TBB_TUMBON_NAME, @TBB_AMPHUR_NAME, @TBB_PROVINCE_ID, @TBB_POST_CODE, @TBB_PHONE_NO, @TBB_PHONE_EX, @PHONE_EX, @CONTACT_PHONE_EX, @BIRTH_DATE_CHAR, @HIGHEST_CLASS_NAME, @HIGHEST_DEGREE_NAME, @HIGHEST_MAJOR_NAME, @HIGHEST_SCHOOL_NAME, @HIGHEST_SCHOOL_PROVINCE, @HIGHEST_SCHOOL_FLAG, @HIGHEST_GRADUATE_DATE, @HIGHEST_GRADUATE_GPA, @OCSC_LEVEL_ID, @OCSC_CERT_NO, @OCSC_EXAM_DTM, @OCSC_PASS_DTM, @PHOTO_FILE, @FACULTY_NAME, @DOCS_LIST, @CLASS_GROUP_ID, @DOC_TYPE_ID, @DIPLOMA_MAJOR, @DIPLOMA_ACADEMY, @DIPLOMA_TEACHER_ACADEMY, @DIPLOMA_TEACHER_GRADUATE_DATE, @CERT_TEACHER_NO, @CERT_TEACHER_ISSUED_DATE, @CERT_TEACHER_EXPIRE_DATE, @TEACH_CLASS_LEVEL_ID, @EXAM_SITE_ID", prms, prmsOut);
                    rtn.Success = result.Success;
                    rtn.ErrorMessage = result.ErrorMessage;
                    rtn.ReturnValue1 = prmsOut[2].Value.ToString();
                    return rtn;
                }
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, ex);
            }
            return null;
        }

        public ResultInfo InsertSmsLog(SmsInfo model)
        {
            var rtn = new ResultInfo();
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
                    prms.Add(new CommonParameter("REF_1", model.RefNo1));
                    prms.Add(new CommonParameter("REF_2", model.RefNo2));
                    prms.Add(new CommonParameter("RESULT", model.Success));
                    prms.Add(new CommonParameter("ERROR_CODE", model.ErrorCode));
                    prms.Add(new CommonParameter("ERROR_MESSAGE", model.ErrorMessage));
                    prms.Add(new CommonParameter("RETURN_VALUE_1", model.MemberID));
                    prms.Add(new CommonParameter("RETURN_VALUE_2", model.RefNo1));
                    prms.Add(new CommonParameter("IPADDRESS", ""));
                    prms.Add(new CommonParameter("CITIZEN_ID", citizen));
                    prms.Add(new CommonParameter("CONSENT_DATE", model.ConsentAcceptDatetime));
                    var result = db.ExecuteStored("ENR_SP_INSERT_SMS_LOG @TEST_TYPE_ID, @REF_1, @REF_2, @RESULT, @ERROR_CODE, @ERROR_MESSAGE, @RETURN_VALUE_1, @RETURN_VALUE_2, @IPADDRESS, @CITIZEN_ID, @CONSENT_DATE", prms, null);
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


        public string EnrollDate(int testTypeID, string scheduleTypeCode)
        {
            try
            {
                using (var db = new RegisterDB())
                {
                    var prms = new List<CommonParameter>();
                    prms.Add(new CommonParameter("TEST_TYPE_ID", testTypeID));
                    prms.Add(new CommonParameter("@SCHEDULE_TYPE_CODE", scheduleTypeCode));
                    var rtn = db.ExecuteStored<SchuduleTypeModel>("ENR_SP_GET_SCHEDULE_TYPE @TEST_TYPE_ID, @SCHEDULE_TYPE_CODE", prms).FirstOrDefault();
                    return rtn.END_DTM.ToDateTextThai("dd/MM/yyyy");
                }
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, ex);
            }
            return null;
        }

        //public string GetDateTime()
        //{
        //    var rtn = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        //    return rtn;
        //}

        private string SetQuestionaireXML(List<QuestionaireModel> lst)
        {
            var xml = string.Empty;
            var lstXML = new List<RegQueXml>();
            foreach (var model in lst)
            {
                var xmlItem = new RegQueXml()
                {
                    TEMPLATE_ID = model.TemplateID,
                    PART_ID = model.PartID,
                    ITEM_ID = model.ItemID,
                    CHOICE_ANSWER_ID = model.ChoiceAnswerID,
                    ANSWER_OTHER = model.AnswerOther
                };
                lstXML.Add(xmlItem);
            }
            xml = lstXML.ObjectToXml();
            return xml;
        }
    }
}
