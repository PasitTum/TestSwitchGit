using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Register.Models
{
    public class LookupModel
    {
        public string id { get; set; }
        public string text { get; set; }
    }

    public class LookupPrefixModel
    {
        public string id { get; set; }
        public string text { get; set; }
        public string gender { get; set; }
    }

    public class LookupClassLevelModel
    {
        public string id { get; set; }
        public string text { get; set; }
        public string title { get; set; }
    }

    public class LookupClassModel
    {
        public string id { get; set; }
        public string text { get; set; }
        public string name { get; set; }
        public string sbjgroup { get; set; }
        public string groupid { get; set; }
    }
}
