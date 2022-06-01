using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Register.Reports
{
    public static class PaymentExtension
    {
        public static string FormatPaymentBarCode(string orderRef1, string ref2, string taxId, string subfix, decimal totalPrice)
        {
            string enterString = Encoding.ASCII.GetString(new byte[] { 13 });
            string code = "|" + taxId + subfix + enterString;
            code += orderRef1 + enterString; //code += rowData["REFNO1"].ToString().Substring(6, 13) + enterString;
            code += ref2 + enterString;
            code += totalPrice.ToString("######0.00").Replace(".", "");

            return code;
        }
    }
}
