using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Register.Models
{
    public class DetectFaceModel
    {
        public int TestTypeID { get; set; }
        public string CitizenID { get; set; }
        public string Image { get; set; }
        public string EnrollNo { get; set; }
    }
}
