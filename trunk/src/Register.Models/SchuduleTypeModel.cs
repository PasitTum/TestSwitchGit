using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Register.DAL
{
    public class SchuduleTypeModel
    {
        public int TEST_TYPE_ID { get; set; }
        public int? SCHEDULE_TYPE_ID { get; set; }
        public string SCHEDULE_TYPE_NAME_TH { get; set; }
        public string SCHEDULE_TYPE_CODE { get; set; }
        public DateTime START_DTM { get; set; }
        public DateTime END_DTM { get; set; }
        
    }
}
