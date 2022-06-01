using CSP.Lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using CSP.Lib.Diagnostic;

namespace Register.DOPA.Biz
{
    public class DOPABiz
    {
        public System.Diagnostics.TraceSwitch tsw = new System.Diagnostics.TraceSwitch("mySwitch", "");
        private bool EnableDopaService
        {
            get
            {
                var cfg = ConfigurationManager.AppSettings["EnableDopaService"] ?? "";
                return cfg.ToUpper() == "Y";
            }
        }

        private int DopaServiceTimeout
        {
            get
            {
                var cfg = ConfigurationManager.AppSettings["DopaServiceTimeout"] ?? "30";
                int i = 30;
                if (int.TryParse(cfg, out i))
                {

                }
                return i;
            }
        }

        public async Task<DopaResultModel> VerifyCitizenCard(string CID, string firstName, string lastName, string birthDay, string laser)
        {
            var result = new DopaResultModel() { IsError = true, Code = -1};
            if (this.EnableDopaService)
            {
                try
                {
                    using (var client = new DOPAService.CheckCardBankServiceSoapClient())
                    {
                        client.InnerChannel.OperationTimeout = new TimeSpan(0, 0, this.DopaServiceTimeout);
                        var dopaResult = await client.CheckCardByLaserAsync(CID, firstName, lastName, birthDay, laser);
                        if (dopaResult != null)
                        {
                            // iserror , 0 = ปกติ
                            result.IsError = dopaResult.IsError;
                            result.Code = dopaResult.Code;
                            result.Desc = dopaResult.Desc;
                            result.ErrorMessage = dopaResult.ErrorMessage;
                        }
                        else
                        {
                            result.IsError = true;
                            result.Code = -999;
                            result.Desc = "Incorrect response";
                            result.ErrorMessage = "Incorrect response";
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteErrorLog(tsw.TraceError, ex);
                    throw ex;
                }
            }
            else
            {
                result.IsError = false;
                result.Code = 0;
                result.Desc = "สถานะปกติ";
            }
            return result;
        }
    }
}
