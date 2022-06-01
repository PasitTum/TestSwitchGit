using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Register.Reports.Helpers
{
    public class ColorHelper
    {
        public static Color ConvertToColor(string colorV)
        {
            return ColorTranslator.FromHtml(colorV);
        }
    }
}
