using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Register.Models
{
    public class UploadPhotoModel
    {
        public int TestTypeID { get; set; }
        public string CitizenID { get; set; }
        public string ImageBase64 { get; set; }
        public string ImageFileName { get; set; }
        public string EnrollNo { get; set; }
        public string IP_ADDRESS { get; set; }
        public string MAC_ADDRESS { get; set; }
    }

    public class ApplicationDetailModel
    {
        public string ENROLL_NO { get; set; }
        public string CITIZEN_ID { get; set; }
        public int TITLE_ID { get; set; }
        public string TITLE_NAME { get; set; }
        public string FNAME_TH { get; set; }
        public string LNAME_TH { get; set; }
        public string GENDER { get; set; }
        public int AGE_DAY { get; set; }
        public int AGE_MONTH { get; set; }
        public int AGE_YEAR { get; set; }
        public DateTime BIRTH_DATE { get; set; }
        public string BIRTH_DATE_CHAR { get; set; }
        public string CLASS_ABBR_NAME_TH { get; set; }
        public string CENTER_EXAM_NAME { get; set; }
        public string PHOTO_FILE_NAME { get; set; }

    }
}
