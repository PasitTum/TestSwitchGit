using CSP.Lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Register.Models
{
    public class CalendarModel
    {
        public int SCHEDULE_TYPE_ID { get; set; }
        public string SCHEDULE_TYPE_NAME_TH { get; set; }
        public string ACTIVE_FLAG { get; set; }
        public string IS_USED { get; set; }
        public string SCHEDULE_TYPE_CODE { get; set; }
        public ResultInfo ServiceResult { get; set; }
    }
}
