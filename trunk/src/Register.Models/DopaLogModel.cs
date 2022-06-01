using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Register.Models
{
    public class DopaLogModel
    {
        public long LOG_ID { get; set; }
        public string USER_ID { get; set; }
        public string LOG_TYPE { get; set; }
        public DateTime? LOG_DTM { get; set; }
        public string LOG_DESC { get; set; }
        public string PROGRAM_ID { get; set; }
        public string IP_ADDRESS { get; set; }
        public string MAC_ADDRESS { get; set; }
        public string RESULT_STATUS { get; set; }
        public string RESULT_DESC { get; set; }
        public int SYSTEM_ID { get; set; }
        public string SP_NAME { get; set; }

    }

}