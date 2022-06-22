using CSP.Lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Register.Reports
{
    public interface IGenaricReport
    {
         string JsonData { get; set; }
        byte[] GetReport();
         //ResultInfo GetReport(string documentPath);
    }
}
