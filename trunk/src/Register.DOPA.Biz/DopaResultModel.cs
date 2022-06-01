using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Register.DOPA.Biz
{
    public class DopaResultModel
    {
        public int Code { get; set; }
        public string Desc { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsError { get; set; }
    }
}
