using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Register.Models
{
    public class CenterExamModel
    {
        public string CENTER_ID { get; set; }

        public string CENTER_NAME { get; set; }

        public string CENTER_NAME_SHOW { get; set; }

        public string QUOTA_QTY { get; set; }

        public string CENTER_EXAM_CODE { get; set; }
    }
}
