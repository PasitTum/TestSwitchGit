using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Register.Models
{
    public class SmsInfo
    {
        public string MemberID { get; set; }
        public int TestTypeID { get; set; }
        public string ProjectID { get; set; }
        public string PhoneNo { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Email { get; set; }
        public string RefNo1 { get; set; }
        public string RefNo2 { get; set; }
        public string Amount { get; set; }
        public string CitizenID { get; set; }
        public bool AllowPrivacy { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConsentAcceptDatetime { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorCode { get; set; }
        public bool RegisterCspSms { get; set; } //ทุกคนยิงสมัคร sms หมด แต่คนที่เลือก จะจ่ายเงินค่า sms ให้ส่งอันนี้เป็น true
    }
}
