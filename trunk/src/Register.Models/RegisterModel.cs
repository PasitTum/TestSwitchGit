using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Register.Models
{
    [Serializable]
    public class RegisterModel : ICloneable
    {
        public string Captcha { get; set; }
        public int TestTypeID { get; set; }
        public string ExamType { get; set; }
        public string LaserCode { get; set; }
        public int CenterExamID { get; set; }
        public string CenterExamName { get; set; }
        public string ClassGroupID { get; set; }
        public string ClassGroupName { get; set; }
        public int RegClassID { get; set; }
        public string RegClassName { get; set; }
        public string CitizenProvinceID { get; set; }
        public string CitizenProvinceName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "PropertyValueRequired")]
        [StringLength(13, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "InvalidMaxlength")]
        [RegularExpression("^[0-9]{13}$", ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "InvalidCitizenID")]
        public string CitizenID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "PropertyValueRequired")]
        public string PrefixID { get; set; }
        public string PrefixName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "PropertyValueRequired")]
        [StringLength(255, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "InvalidMaxlength")]
        public string FirstName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "PropertyValueRequired")]
        [StringLength(255, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "InvalidMaxlength")]
        public string LastName { get; set; }
        public string EduPrefixID { get; set; }
        public string EduPrefixName { get; set; }
        public string EduFirstName { get; set; }
        public string EduLastName { get; set; }
        public string Gender { get; set; }
        public string GenderName { get; set; }
        public string NationName { get; set; }
        public string RaceName { get; set; }
        public string ReligionID { get; set; }
        public string ReligionName { get; set; }
        public string StatusID { get; set; }
        public string StatusName { get; set; }
        public string BirthDate { get; set; }
        public string BirthDateChar { get; set; }
        public string BirthDateDay { get; set; }
        public string BirthDateMonth { get; set; }
        public string BirthDateYear { get; set; }
        public string IssueDate { get; set; }
        public string ExpiredDate { get; set; }
        public int AgeDay { get; set; }
        public int AgeMonth { get; set; }
        public int AgeYear { get; set; }
        public string DocTypeID { get; set; }
        public string DocTypeName { get; set; }
        public string ClassLavelID { get; set; }
        public string ClassLavelName { get; set; }
        public string ClassHighestLavelID { get; set; }
        public string ClassHighestLavelName { get; set; }
        public string ClassHighestLavelSchoolName { get; set; }
        public string SchoolGroupID { get; set; }
        public string SchoolFlag { get; set; }
        public string SchoolID { get; set; }
        public string SchoolName { get; set; }
        public string SchoolProvinceID { get; set; }
        public string SchoolProvinceName { get; set; }
        public string SchoolCountryID { get; set; }
        public string DegreeID { get; set; }
        public string DegreeName { get; set; }
        public string MajorID { get; set; }
        public string MajorName { get; set; }
        public string EducationFlag { get; set; }
        public string GraduateDate { get; set; }
        public string GPA { get; set; }
        public string StudyYear { get; set; }
        public string EduYear { get; set; }
        public string DefectiveFlag { get; set; }
        public string DefectiveHelpFlag { get; set; }
        public string DefectiveHelpID { get; set; }
        public string OccuupationID { get; set; }
        public string OccuupationName { get; set; }
        public string PositionName { get; set; }
        public string StationName { get; set; }
        public string DepartmentName { get; set; }
        public string OfficeMobile { get; set; }
        public string OfficeTel { get; set; }
        public string OfficeTelEx { get; set; }
        public string Salary { get; set; }
        public string AddrNo { get; set; }
        public string Village { get; set; }
        public string Moo { get; set; }
        public string Soi { get; set; }
        public string Road { get; set; }
        public string ProvID { get; set; }
        public string ProvName { get; set; }
        public string AmphID { get; set; }
        public string AmphName { get; set; }
        public string TmblID { get; set; }
        public string TmblName { get; set; }
        public string Zipcode { get; set; }
        public string TbbAddrNo { get; set; }
        public string TbbVillage { get; set; }
        public string TbbMoo { get; set; }
        public string TbbSoi { get; set; }
        public string TbbRoad { get; set; }
        public string TbbProvID { get; set; }
        public string TbbProvName { get; set; }
        public string TbbAmphID { get; set; }
        public string TbbAmphName { get; set; }
        public string TbbTmblID { get; set; }
        public string TbbTmblName { get; set; }
        public string TbbZipcode { get; set; }
        public string TbbTel { get; set; }
        public string TbbTelEx { get; set; }
        public string Tel { get; set; }
        public string TelEx { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string EmergencyFirstName { get; set; }
        public string EmergencyLastName { get; set; }
        public string EmergencyRelation { get; set; }
        public string EmergencyTel { get; set; }
        public string EmergencyTelEx { get; set; }
        public string EmergencyMoblie { get; set; }
        public string Password { get; set; }
        public string ComfirmPassword { get; set; }
        public string QuestionaireJson { get; set; }
        public List<QuestionaireModel> Questionaire { get; set; }
        public string XMLQuestionaire { get; set; }
        public string SMSStatus { get; set; }
        public string SMSMobile { get; set; }
        public string SMSEmail { get; set; }
        public string ErrorNum { get; set; }
        public string ErrorMess { get; set; }
        public string RegisterNo { get; set; }
        public string ConsentDate { get; set; }
        public string ErrorMessage { get; set; }
        public bool DialogFlag { get; set; }
        public string TbbFlag { get; set; }
        public string EduFlag { get; set; }
        public string SpecialSkills { get; set; }
        public string Mode { get; set; }
        public string ImageBase64 { get; set; }
        public string IPAddress { get; set; }
        public string MacAddress { get; set; }
        public string OCSCFlag { get; set; }
        public string OCSCLevelID { get; set; }
        public string OCSCLevelName { get; set; }
        public string OCSCCertNo { get; set; }
        public string OCSCExamDate { get; set; }
        public string OCSCPassDate { get; set; }
        public string PhotoFileName { get; set; }
        public string FacultyName { get; set; }
        public string ViewMode { get; set; }
        public string DiplomaMajor { get; set; }
        public string DiplomaAcademy { get; set; }
        public string DiplomaFlag { get; set; }
        public string DiplomaTeachAcademy { get; set; }
        public string DiplomaTeachGraduateDate { get; set; }
        public string CertTeachNo { get; set; }
        public string CertTeachIssuedDate { get; set; }
        public string CertTeachExpireDate { get; set; }
        public string TeachClassLevelID { get; set; }
        public string TeachClassLevelName { get; set; }
        public string OccuupationName4Gov { get; set; }
        public string ExamSiteID { get; set; }
        public string ExamSiteName { get; set; }

        public List<DocToUploadModel> DOCS { get; set; }

        public string LineID { get; set; }
        public string ContactTitle { get; set; }
        public string ContactTitleName { get; set; }
        public string ContactFName { get; set; }
        public string ContactLName { get; set; }
        public string ContactDetail { get; set; }
        public string ContactPhoneNo { get; set; }
        public string ContactPhoneEx { get; set; }

        public string ContactTitle2 { get; set; }
        public string ContactTitleName2 { get; set; }
        public string ContactFName2 { get; set; }
        public string ContactLName2 { get; set; }
        public string ContactDetail2 { get; set; }
        public string ContactPhoneNo2 { get; set; }
        public string ContactPhoneEx2 { get; set; }
        public List<StudyList> gridEducations { get; set; }
        public List<ExperienceList> gridExperiences { get; set; }


        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
    public class StudyList
    {
        public int SEQ_NO { get; set; }
        public string CLASS_LEVEL_ID { get; set; }
        public string CLASS_LEVEL_Name { get; set; }
        public string DEGREE_NAME { get; set; }
        public string MAJOR_NAME { get; set; }
        public string SCHOOL_NAME { get; set; }
    }
    public class ExperienceList
    {
        public int SEQ_NO { get; set; }
        public string START_DATE { get; set; }
        public string END_DATE { get; set; }
        public string POSITION_NAME { get; set; }
        public string WORKPLACE { get; set; }
        public string SALARY { get; set; }
        public string REMARK { get; set; }
    }

    public class QuestionaireModel
    {
        public int TemplateID { get; set; }
        public int PartID { get; set; }
        public int ItemID { get; set; }
        public int ChoiceAnswerID { get; set; }
        public string AnswerOther { get; set; }
    }

}
