using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Register.Web.Models
{
    public class TesttypeListingModel
    {
        public int TEST_TYPE_ID { get; set; }
        public string EXAM_TYPE { get; set; }
        public string YEAR { get; set; }
        public int? PERIOD_NO { get; set; }
        public string TEST_TYPE_NAME { get; set; }
        public DateTime? START_DTM { get; set; }
        public DateTime? END_DTM { get; set; }
        public string DISABLE_FLAG { get; set; }
        public string REMARK { get; set; }

        public string ABBR_NAME { get; set; }
    }
}