using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSP.Lib.Extension;

namespace Register.Models
{
    public class InquiryModel
    {
        public int TestTypeID { get; set; }
        public string CitizenID { get; set; }
        public string Password { get; set; }
        public string LaserCode { get; set; }
        public string Mobile { get; set; }
        public string SeatNo { get; set; }
        public string BirthDateChar { get; set; }
        public int? TestingClassID { get; set; }

    }

    public class PaymentStatusModel
    {
        public string TEST_TYPE_NAME { get; set; }
        public string FULLNAME { get; set; }
        public string ACTIVE_FLAG { get; set; }
        public string PAYMENT_STATUS { get; set; }
        public string PAYMENT_STATUS_NAME { get; set; }
        public string PAYIN_BUTTON_FLAG { get; set; }
        public string ENROLL_STATUS { get; set; }
        public string ENROLL_NO { get; set; }
        public string CITIZEN_ID { get; set; }
        public int TEST_TYPE_ID { get; set; }
        public string YEAR { get; set; }
        public int? PERIOD_NO { get; set; }
        public string CLASS_NAME { get; set; }
        public DateTime? PAYIN_EXPIRE_DTM { get; set; }
        public string REF1 { get; set; }
        public string CENTER_EXAM_NAME { get; set; }
        public string SMS_STATUS { get; set; }
        public string PAY_DATE { get; set; }
        public string PAY_DATE_ATM { get; set; }
        public string ENROLL_PASSWORD { get; set; }
        public string GENDER_NAME { get; set; }
    }

    public class RegisterStatusModel
    {
        public string FULLNAME { get; set; }
        public string CLASS_NAME_TH { get; set; }
        public string CENTER_EXAM_NAME_TH { get; set; }
        public string PAYMENT_STATUS { get; set; }
        public string PAYIN_BUTTON_FLAG { get; set; }
        public string UPLOAD_STATUS { get; set; }
        public string EXAM_SITE_NAME { get; set; }
    }

    public class ExamApplicationModel
    {
        public int TEST_TYPE_ID { get; set; }
        public int CENTER_EXAM_ID { get; set; }
        public string CENTER_EXAM_NAME { get; set; }
        public string CLASS_NAME_TH { get; set; }
        public string CLASS_GROUP_NAME { get; set; }
        public string CLASS_LEVEL_NAME_TH { get; set; }       
        public string EXAM_NO { get; set; }
        public string EXAM_SITE_NAME { get; set; }
        public string EXAM_SEAT_NO { get; set; }
        public string EXAM_SEAT_NO_DIGIT_01 { get; set; }
        public string EXAM_SEAT_NO_DIGIT_02 { get; set; }
        public string EXAM_SEAT_NO_DIGIT_03 { get; set; }
        public string EXAM_SEAT_NO_DIGIT_04 { get; set; }
        public string EXAM_SEAT_NO_DIGIT_05 { get; set; }
        public string EXAM_SEAT_NO_DIGIT_06 { get; set; }
        public string EXAM_SEAT_NO_DIGIT_07 { get; set; }
        public string EXAM_SEAT_NO_DIGIT_08 { get; set; }
        public string EXAM_SEAT_NO_DIGIT_09 { get; set; }
        public string EXAM_SEAT_NO_DIGIT_10 { get; set; }
        public string GENDER { get; set; }
        public string TITLE_NAME { get; set; }
        public string FNAME_TH { get; set; }
        public string LNAME_TH { get; set; }
        public string FULL_NAME { get; set; }
        public string NATION_NAME { get; set; }
        public string RELIGION_NAME { get; set; }
        public string STATUS_NAME { get; set; }
        public string PHONE_NO { get; set; }
        public string MOBILE_NO { get; set; }
        public string EMAIL { get; set; }
        public string BIRTH_DATE { get; set; }
        public string BIRTH_DATE_DAY { get; set; }
        public string BIRTH_DATE_MONTH { get; set; }
        public string BIRTH_DATE_YEAR { get; set; }
        public int AGE_YEAR { get; set; }
        public int AGE_MONTH { get; set; }
        public string DEGREE_NAME { get; set; }
        public string MAJOR_NAME { get; set; }
        public string SCHOOL_NAME { get; set; }
        public string GRADUATE_DATE { get; set; }
        public string TESTING_GRAD_DAY { get; set; }
        public string TESTING_GRAD_MONTH { get; set; }
        public int? TESTING_GRAD_YEAR { get; set; }
        public string TESTING_GPA { get; set; }
        public string TESTING_CLASS_NAME { get; set; }
        public string TESTING_CLASS_CENTER_NAME { get; set; }
        public string HAS_DIPLOMA { get; set; }
        public string DIPLOMA_MAJOR { get; set; }
        public string DIPLOMA_ACADEMY { get; set; }
        public string DIPLOMA_TEACHER_ACADEMY { get; set; }
        public string DIPLOMA_TEACHER_GRADUATE_DATE { get; set; }
        public string DOC_TYPE { get; set; }
        public string CERT_TEACHER_NO { get; set; }
        public string CERT_TEACHER_ISSUED_DATE { get; set; }
        public string CERT_TEACHER_EXPIRE_DATE { get; set; }
        public string TEACH_CLASS_LEVEL_NAME { get; set; }
        public string OCCUPATION_NAME { get; set; }
        public string POSITION_NAME { get; set; }
        public string OFFICE_NAME { get; set; }
        public string DIVISION_NAME { get; set; }
        public string OFFICE_PHONE_NO { get; set; }
        public string OFFICE_MOBILE_NO { get; set; }
        public decimal? SALARY { get; set; }
        public string CITIZEN_ID { get; set; }
        public string ISSUE_DATE { get; set; }
        public string EXPIRE_DATE { get; set; }
        public string TBB_ADDRESS_NO { get; set; }
        public string TBB_VILLAGE_NAME { get; set; }
        public string TBB_MOO { get; set; }
        public string TBB_SOI { get; set; }
        public string TBB_STREET { get; set; }
        public string TBB_TUMBON_NAME { get; set; }
        public string TBB_AMPHUR_NAME { get; set; }
        public string TBB_PROVINCE_NAME { get; set; }
        public string TBB_POST_CODE { get; set; }
        public string TBB_PHONE_NO { get; set; }
        public string TBB_PHONE_EX { get; set; }
        public string CURRENT_AMPHUR_NAME { get; set; }
        public string CURRENT_PROVINCE_NAME { get; set; }
        public string CONTACT_ADDRESS_NO { get; set; }
        public string CONTACT_VILLAGE { get; set; }
        public string CONTACT_MOO { get; set; }
        public string CONTACT_SOI { get; set; }
        public string CONTACT_STREET { get; set; }
        public string CONTACT_TUMBON_NAME { get; set; }
        public string CONTACT_AMPHUR_NAME { get; set; }
        public string CONTACT_PROVINCE_NAME { get; set; }
        public string CONTACT_POST_CODE { get; set; }
        public string PPT_SPECIAL_SKILL { get; set; }
        public string DEFECTIVE_NAME { get; set; }
        public string CONTACT_TITLE { get; set; }
        public string CONTACT_NAME { get; set; }
        public string CONTACT_SURNAME { get; set; }
        public string CONTACT_RELATION { get; set; }
        public string CONTACT_PHONE { get; set; }
        public string CONTACT_MOBILE { get; set; }
        public int? PAYMENT_DAY { get; set; }
        public int? PAYMENT_MONTH { get; set; }
        public int? PAYMENT_YEAR { get; set; }
        public string PAYMENT_DATE_DAY { get; set; }
        public string PAYMENT_DATE_MONTH { get; set; }
        public int PAYMENT_DATE_YEAR { get; set; }
        public string FULLNAME_OF_EDU { get; set; }
        public string FACULTY_NAME { get; set; }
        public string SCHOOL_COUNTRY { get; set; }
        public string SCHOOL_PROV_NAME { get; set; }
    }

    public class SMSStatusModel
    {
        public string CITIZEN_ID { get; set; }
        public string ENROLL_NO { get; set; }
        public string FULL_NAME { get; set; }
        public string FNAME { get; set; }
        public string LNAME { get; set; }
        public int CLASS_ID { get; set; }
        public string CLASS_NAME { get; set; }
        public string CENTER_EXAM_NAME { get; set; }
        public string SMS_STATUS { get; set; }
        public string SMS_PAYMENT { get; set; }
        public string SMS_PAYIN_BUTTON { get; set; }
    }

    public class EnrollInfoModel
    {
        public string EXAM_SEAT_NO { get; set; }
        public string FULL_NAME { get; set; }
        public string CLASS_NAME_TH { get; set; }
        public string CENTER_EXAM_NAME { get; set; }
        public string DEFECTIVE_STATUS { get; set; }
    }

    public class UploadStatusModel
    {
        public string ENROLL_NO { get; set; }
        public string FULLNAME { get; set; }
        public string CLASS_NAME { get; set; }
        public string CENTER_EXAM_NAME { get; set; }
        public string UPLOAD_FLAG { get; set; }
        public string UPLOAD_STATUS { get; set; }
        public DateTime? UPLOAD_DTM { get; set; }
        public string UPLOAD_DTM_TH
        {
            get
            {
                return this.UPLOAD_DTM.ToDateTextThai("dd/MM/yyy เวลา HH:mm:ss");
            }
        }
        public string PHOTO_FILE { get; set; }
        public string PHOTO_FILE_BASE64 { get; set; }

    }

    public class ExamSiteInfoModel
    {
        public int TEST_TYPE_ID { get; set; }
        public int CENTER_EXAM_ID { get; set; }
        public string CENTER_EXAM_NAME { get; set; }
        public string EXAM_SEAT_NO { get; set; }
        public string TITLE_NAME { get; set; }
        public string FNAME_TH { get; set; }
        public string LNAME_TH { get; set; }
        public string FULL_NAME { get; set; }
        public string TESTING_CLASS_NAME { get; set; }
        public string CITIZEN_ID { get; set; }
        public string EXAM_DATE { get; set; }
        public string EXAM_TIME { get; set; }
        public string EXAM_LOCATION { get; set; }
        public string EXAM_BUILDING { get; set; }
        public string EXAM_FL { get; set; }
        public string EXAM_ROOM { get; set; }
        public string EXAM_ROW { get; set; }
        public string EXAM_LROW { get; set; }
        public string LATITUDE { get; set; }
        public string LONGTITUDE { get; set; }
        public string PHOTO_FILE { get; set; }
    }

    public class ExamPassInfoModel
    {
        public string ENROLL_NAME { get; set; }
        public string CENTER_EXAM_NAME { get; set; }
        public int TESTING_CLASS_ID { get; set; }
        public string CLASS_NAME { get; set; }
        public string EXAM_SEAT_NO { get; set; }
        public string ENROLL_NO { get; set; }
        public string ACTIVE_EDIT { get; set; }
        public string CITIZEN_ID { get; set; }
        public string STUDY_CLASS_NAME { get; set; }
        public string EXAM_STATUS { get; set; }
        public string TESTING_CLASS_NAME { get; set; }
        public string SECTION_SCORE_1 { get; set; }
        public string SECTION_SCORE_2 { get; set; }
        public string SECTION_SCORE_3 { get; set; }
    }

    public class ScoreModel
    {
        public string SEAT_NO { get; set; }
        public string FULL_NAME { get; set; }
        public string CLASS_NAME { get; set; }
        public string SCORE_FULL_GEN { get; set; }
        public string SCORE_GEN { get; set; }
        public string SCORE_FULL_THAI { get; set; }
        public string SCORE_THAI { get; set; }
        public string SCORE_FULL_GENTHAI { get; set; }
        public string SCORE_GENTHAI { get; set; }
        public string SCORE_FULL_ENG { get; set; }
        public string SCORE_ENG { get; set; }
        public string SCORE_FULL_TOTAL { get; set; }
        public string SCORE_TOTAL { get; set; }
        public string EXAM_RESULT { get; set; }
    }

    public class PaymentRepeatedModel
    {
        public int TEST_TYPE_ID { get; set; }
        public string ENROLL_NO { get; set; }
        public string CITIZEN_ID { get; set; }
        public int CENTER_EXAM_ID { get; set; }
        public string CENTER_EXAM_NAME { get; set; }
        public string TITLE_NAME { get; set; }
        public string FNAME_TH { get; set; }
        public string LNAME_TH { get; set; }
        public string FULL_NAME { get; set; }
        public string TESTING_CLASS_NAME { get; set; }
        public int SEQ_NO { get; set; }
        public string PAYMENT_DTM { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public int AMOUNT { get; set; }
        public string BANK_NAME { get; set; }
        public string ACCT_NO { get; set; }
        public string MOBILE_NO { get; set; }
        public string EMAIL { get; set; }
        public string REQUEST_STATUS { get; set; }
        public string REQUEST_DATE { get; set; }
        public string REQUEST_TIME { get; set; }
        public int PAYMENT_CNT { get; set; }
        public int REFUND_AMT { get; set; }
        public int TOTAL_REFUND_AMT { get; set; }
    }

    public class RefundPaymentModel: InquiryModel
    {
        public string ENROLL_NO { get; set; }
        public string BANK_NAME { get; set; }
        public string ACCT_NO { get; set; }
        public string MOBILE_NO { get; set; }
        public string EMAIL { get; set; }
    }

    public class DocByCitizenModel
    {
        public string TITLE_NAME { get; set; }
        public string FNAME_TH { get; set; }
        public string LNAME_TH { get; set; }
        public string ENROLL_NO { get; set; }
        public string CITIZEN_ID { get; set; }
        public string PAYMENT_STATUS { get; set; }
        public int DOC_TYPE_ID { get; set; }
        public string REQUIRE_FLAG { get; set; }
        public string DOCUMENT_NAME_TH { get; set; }
        public int MIN_LIMITED_SIZE { get; set; }
        public string UNIT_MIN_SIZE { get; set; }
        public int MAX_LIMITED_SIZE { get; set; }
        public string UNIT_MAX_SIZE { get; set; }
        public string FILE_TYPE_ACCEPT { get; set; }
        public string UPLOAD_STATUS { get; set; }
        public int CLASS_ID { get; set; }
        public string CLASS_NAME_TH { get; set; }
        public string UPLOAD_DOC_FLAG { get; set; }
        public int CLASS_GROUP_ID { get; set; }
        public Guid? DOC_GUID_ORG { get; set; }
    }

    public class NewUploadDocModel: InquiryModel
    {
        public string IPAddress { get; set; }
        public string MacAddress { get; set; }
        public string EnrollNo { get; set; }
        public List<DocToUploadModel> DOCS { get; set; }
    }
}
