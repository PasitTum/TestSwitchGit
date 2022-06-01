using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Register.Models
{
    public class TestingClassModel
    {
        public int TEST_TYPE_ID { get; set; }
        public int? CENTER_EXAM_ID { get; set; }

        public int? CLASS_GROUP_ID { get; set; }
        public string CLASS_GROUP_NAME_TH { get; set; }
        public int CLASS_LEVEL_ID { get; set; }
        public string CLASS_LEVEL_NAME_TH { get; set; }
        public int CLASS_ID { get; set; }
        public string CLASS_NAME { get; set; }
        public string QUOTA_QTY { get; set; }
        public string REMARK { get; set; }
    }

    public class TestingClassInfoModel
    {      
        public string PROVINCE_DTL { get; set; }       
    }
}
