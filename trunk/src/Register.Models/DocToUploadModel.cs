using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Register.Models
{
    [Serializable]
    public class DocToUploadModel
    {
        public int TEST_TYPE_ID { get; set; }
        public string ENROLL_NO { get; set; }
        public int DOC_TYPE_ID { get; set; }
        public Guid? DOC_GUID { get; set; }
        public string FILE_TYPE { get; set; }
        public string FILE_NAME { get; set; }
        public string ACTIVE_FLAG { get; set; }
        public string UPLOAD_STATUS { get; set; }
        public string DOC_TYPE_NAME { get; set; }
        public string REQUIRE_FLAG { get; set; }
        public string DOC_PATH { get; set; }
    }
}
