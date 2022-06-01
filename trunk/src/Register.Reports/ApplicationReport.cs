using CSP.Lib.Json;
using CSP.Lib.Models;
using CSP.Lib.Extension;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZXing;
using CWN.OTAS.Lib.Helper;
using Register.Reports.Helpers;
using CSP.Lib.Diagnostic;

namespace Register.Reports
{
    public class ApplicationReport : BaseReport
    {
        private JObject enrollDatas { get; set; }
        BaseFont _baseFont;
        Font Font;
        public bool IsTesting { get; set; }
        public string ImageBase64 { get; set; }

        public ApplicationReport(int testtypeID)
        {
            this.ReportName = "Application";
            if (testtypeID != 1)
            {
                this.SettingFileName = "application-setting-tt" + testtypeID + ".json";
                this.TemplateFileName = "ApplicationTemplate_tt" + testtypeID + ".pdf";
            }
            else
            {
                this.SettingFileName = "application-setting.json";
                this.TemplateFileName = "ApplicationTemplate.pdf";
            }
            this.ReadSetting();
        }

        public ApplicationReport(string jsonDataText) : base(jsonDataText)
        {
            this.JsonData = jsonDataText;
            this.ReadSetting();
        }

        public override void RefreshDataSource()
        {
            if (!string.IsNullOrWhiteSpace(JsonData))
            {
                enrollDatas = (JObject)JsonConvert.DeserializeObject(JsonData);
            }
        }

        public void Initial()
        {

        }

        // มา Load Font ทั้งหมดที่นี่กันนะทู๊กกกคนนนนนน
        protected void LoadFont(PdfReader reader)
        {
            //string fontname = "TH Sarabun New";
            // _baseFont = this.FindFontInForm(reader, fontname); // TODO : ยังไม่ work อ่ะ  พอเปิดใช้อันนี้แล้วค่ามันหาย ตัวหนังสือมันขาดๆ            
            if (_baseFont == null)
            {
                _baseFont = BaseFont.CreateFont(this.GetFileNameInReportPath("THSarabunNew.ttf"), BaseFont.IDENTITY_H, true);
            }
            this.Font = new Font(_baseFont);
        }

        public override ResultInfo GetReport(string documentPath)
        {
            var result = new ResultInfo();
            this.PdfPath = HttpContext.Current.Server.MapPath(documentPath);
            var templateFile = this.GetFileNameInReportPath(this.TemplateFileName);
            if (this.enrollDatas != null)
            {
                //var examSeatNo = enrollDatas.GetValueIgnoreCase("EXAM_SEAT_NO") == null ? "" : enrollDatas.GetValueIgnoreCase("EXAM_SEAT_NO");
                //if (examSeatNo != "")
                //{
                //    this.SettingFileName = "application-setting-b.json";
                //    this.TemplateFileName = "ApplicationTemplateB.pdf";
                //}

                //var templateFile = this.GetFileNameInReportPath(this.TemplateFileName);
                //this.ReadSetting();

                if (!File.Exists(templateFile))
                {
                    //Log.WriteErrorLog(tsw.TraceError, string.Format("Template file {0} not found.", templateFile));
                    result.Success = false;
                    result.ErrorMessage = string.Format("Template file {0} not found.", templateFile);
                    return result;
                }

                EncryptHelper encryptHelper = new EncryptHelper();
                var citizen = enrollDatas.GetValueIgnoreCase("CITIZEN_ID") == null ? "" : enrollDatas.GetValueIgnoreCase("CITIZEN_ID");
                if (citizen != null & citizen != "")
                {
                    citizen = encryptHelper.DecryptData(citizen);
                }
                var signName = enrollDatas.GetValueIgnoreCase("FULL_NAME") == null ? "" : enrollDatas.GetValueIgnoreCase("FULL_NAME");
                if (signName != null & signName != "")
                {
                    signName = "(" + enrollDatas.GetValueIgnoreCase("FULL_NAME") + ")";
                }

                var password = enrollDatas.GetValueIgnoreCase("ENROLL_PASSWORD") == null ? "" : enrollDatas.GetValueIgnoreCase("ENROLL_PASSWORD");
                if (password != null & password != "")
                {
                    password = encryptHelper.DecryptData(password);
                }

                this.FileDownloadName = this.ReportName + "_" + citizen + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                if (this.IsLocalFileExists())
                {
                    if (this.KeepPdfFile)
                    {
                        result.Success = true;
                        result.ReturnValue1 = this.LocalFileName;
                        //result.ReturnValue1 = this.FileDownloadName;
                        //result.ReturnValue2 = this.LocalFileName;
                        return result;
                    }
                    else
                    {
                        try
                        {
                            System.IO.File.Delete(this.LocalFileName);
                        }
                        catch (Exception ex)
                        {
                            Log.WriteErrorLog(tsw.TraceError, ex);
                            throw (ex);
                        }
                    }
                }


                var targetFile = this.LocalFileName;
                try
                {
                    using (var reader = new PdfReader(templateFile))
                    {
                        using (var targetFileStream = new FileStream(targetFile, FileMode.Create))
                        using (var stamper = new PdfStamper(reader, targetFileStream))
                        {
                            this.Initial();

                            // Load Font 
                            this.LoadFont(reader);
                            var fontStyle = this.Font;

                            foreach (JObject setting in this.SettingData)
                            {
                                var x = float.Parse(setting.GetValueIgnoreCase("x"));
                                var y = float.Parse(setting.GetValueIgnoreCase("y"));
                                var w = setting.GetValueIgnoreCase("w") == null ? 0 : float.Parse(setting.GetValueIgnoreCase("w"));
                                var h = setting.GetValueIgnoreCase("h") == null ? 0 : float.Parse(setting.GetValueIgnoreCase("h"));
                                //space between lines
                                var lh = setting.GetValueIgnoreCase("lh") == null ? 0 : float.Parse(setting.GetValueIgnoreCase("lh"));

                                var page = setting.GetValueIgnoreCase("page") == null ? 1 : int.Parse(setting.GetValueIgnoreCase("page"));
                                PdfContentByte canvas = stamper.GetOverContent(page);

                                var fieldName = setting.GetValueIgnoreCase("fieldname");
                                var fieldValue = enrollDatas.GetValueIgnoreCase(fieldName) == null ? "" : enrollDatas.GetValueIgnoreCase(fieldName); ;

                                if (setting.GetValueIgnoreCase("size") != null)
                                {
                                    fontStyle = SetFontStyle(fontStyle, setting.GetValueIgnoreCase("color") == null ? "black" : setting.GetValueIgnoreCase("color"), int.Parse(setting.GetValueIgnoreCase("size")), setting.GetValueIgnoreCase("font-style") == null ? "normal" : setting.GetValueIgnoreCase("font-style"));
                                }

                                var objectType = setting.GetValueIgnoreCase("objecttype");
                                var objectValue = setting.GetValueIgnoreCase("objectvalue") == null ? "" : setting.GetValueIgnoreCase("objectvalue");

                                if (string.Compare(objectType, "ip", true) == 0)
                                {
                                    this.WriteText(canvas, GetServerIPLast3(), fontStyle, x, y);
                                }
                                else if (string.Compare(objectType, "date", true) == 0)
                                {
                                    this.WriteText(canvas, DateTime.Now.ToDateTextThai(), fontStyle, x, y);
                                }
                                else if (string.Compare(objectType, "datetext", true) == 0)
                                {
                                    this.WriteText(canvas, DateTime.Now.ToDateTextThai("dd MMMM yyyy"), fontStyle, x, y);
                                }
                                else if (string.Compare(objectType, "sign", true) == 0)
                                {
                                    this.WriteText(canvas, signName, fontStyle, x, y);
                                }
                                else if (string.Compare(objectType, "citizen", true) == 0)
                                {
                                    this.WriteText(canvas, citizen, fontStyle, x, y);
                                }
                                else if (string.Compare(objectType, "photo", true) == 0)
                                {
                                    if (!string.IsNullOrWhiteSpace(this.ImageBase64))
                                    {
                                        this.SetImage(canvas, this.ImageBase64, x, y, w, h);
                                    }
                                    else
                                    {
                                        this.WriteText(canvas, "", fontStyle, x, y);
                                    }
                                }
                                else if (string.Compare(objectType, "money", true) == 0)
                                {
                                    decimal money = 0;
                                    decimal.TryParse(fieldValue, out money);
                                    this.WriteText(canvas, string.Format("{0:#,0.00}", money), fontStyle, x, y);
                                }
                                else if (string.Compare(objectType, "manylines", true) == 0)
                                {
                                    this.WriteTextMultiLines(canvas, fieldValue, fontStyle, x, y, w, h, lh);
                                }
                                else if (string.Compare(objectType, "monthname", true) == 0)
                                {
                                    if (!string.IsNullOrWhiteSpace(fieldValue))
                                    {
                                        var monthName = GetMonthName(fieldValue);
                                        this.WriteText(canvas, monthName, fontStyle, x, y);
                                    }
                                }
                                else if (string.Compare(objectType, "monthabbrname", true) == 0)
                                {
                                    if (!string.IsNullOrWhiteSpace(fieldValue))
                                    {
                                        var monthName = GetMonthAbbrName(fieldValue);
                                        this.WriteText(canvas, monthName, fontStyle, x, y);
                                    }
                                }
                                else if (string.Compare(objectType, "dateyear", true) == 0)
                                {
                                    if (!string.IsNullOrWhiteSpace(fieldValue))
                                    {
                                        int dateyear;
                                        bool success = Int32.TryParse(fieldValue, out dateyear);
                                        if (success)
                                        {
                                            if (dateyear > 2475)
                                            {
                                                dateyear = dateyear + 0;
                                            }
                                            else
                                            {
                                                dateyear = dateyear + 543;
                                            }
                                            this.WriteText(canvas, dateyear.ToString(), fontStyle, x, y);
                                        }
                                        else
                                        {
                                            this.WriteText(canvas, fieldValue, fontStyle, x, y);
                                        }
                                    }
                                }
                                else if (string.Compare(objectType, "waterMark", true) == 0)
                                {
                                    if (IsTesting)
                                    {
                                        var fontStyleTesting = SetFontStyle(fontStyle, "red", 96, "normal");
                                        this.WriteTestSystem(canvas, "ทดสอบระบบ", fontStyleTesting, x, y);
                                    }
                                }
                                else
                                {
                                    this.WriteText(canvas, fieldValue, fontStyle, x, y);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteErrorLog(tsw.TraceError, ex);
                    throw (ex);
                }
                finally
                {
                    //if (stamper != null) stamper.Close();
                    //if (reader != null) reader.Close();
                }

                result.Success = true;
                result.ReturnValue1 = this.LocalFileName;
                //result.ReturnValue1 = this.FileDownloadName;
                //result.ReturnValue2 = this.LocalFileName;
            }
            else
            {
                result.Success = false;
                result.ErrorMessage = "ไม่มีข้อมูล";
            }

            return result;
        }
    }
}
