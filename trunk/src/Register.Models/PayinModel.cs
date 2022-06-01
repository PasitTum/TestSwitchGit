using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Register.Models
{
    public class PayinModel
    {
        public string TEST_TYPE_NAME { get; set; }
        public string FULLNAME { get; set; }
        public string TITLE_NAME_TH { get; set; }
        public string FNAME_TH { get; set; }
        public string LNAME_TH { get; set; }
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
        public string CENTER_EXAM_NAME { get; set; }
        public string REF1 { get; set; }
        public string REF2 { get; set; }
        public string COMP_CODE { get; set; }
        public decimal AMOUNT { get; set; }
        public decimal IB_IR_AMT { get; set; }
        public decimal AMOUNT_TOTAL { get; set; }
        public string SUFFIX { get; set; }
        public string TAX_ID { get; set; }
        public string PAYIN_CENTER { get; set; }
        public string SMS_STATUS { get; set; }
        public string PAY_DATE { get; set; }
        public string PAY_DATE_ATM { get; set; }
        public string SMS_COMP_CODE { get; set; }
        public string SMS_REF_1 { get; set; }
        public string SMS_REF_2 { get; set; }
        public decimal? SMS_AMOUNT { get; set; }
        public string SMS_SUFFIX { get; set; }
        public string SMS_TAX_ID { get; set; }
        public string ENROLL_PASSWORD { get; set; }
        public string EXPENSE_AMT_TEXT { get; set; }
        public string PAY_IN_DATE { get; set; }
    }
}
