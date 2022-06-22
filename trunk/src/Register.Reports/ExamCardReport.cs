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
using CSP.Lib.Diagnostic;

namespace Register.Reports
{
    public class ExamCardReport : BaseReport
    {
        private JObject enrollDatas { get; set; }
        public bool IsTesting { get; set; }
        BaseFont _baseFont;
        Font Font;
        public string ImageBase64 { get; set; }

        public ExamCardReport()
        {
            this.ReportName = "ExamCard";
            this.SettingFileName = "examcard-setting.json";
            this.TemplateFileName = "ExamCardTemplate.pdf";
            this.ReadSetting();
        }

        public ExamCardReport(string jsonDataText) : base(jsonDataText)
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

        public override byte[] GetReport()
        {
            byte[] buffer;
            var templateFile = this.GetFileNameInReportPath(this.TemplateFileName);
            if (this.enrollDatas != null)
            {
                if (!File.Exists(templateFile))
                {
                    // return no success
                    throw new FileNotFoundException(String.Format("Template not found.[{0}]", templateFile));
                }
            }
            using (var result = new MemoryStream())
            {
                var seatNo = enrollDatas.GetValueIgnoreCase("EXAM_SEAT_NO") == null ? "" : enrollDatas.GetValueIgnoreCase("EXAM_SEAT_NO");
                EncryptHelper encryptHelper = new EncryptHelper();
                var citizen = enrollDatas.GetValueIgnoreCase("CITIZEN_ID") == null ? "" : enrollDatas.GetValueIgnoreCase("CITIZEN_ID");
                if (citizen != null & citizen != "")
                {
                    citizen = encryptHelper.DecryptData(citizen);
                }
                try
                {
                    using (var reader = new PdfReader(templateFile))
                    {
                        using (var stamper = new PdfStamper(reader, result))
                        {

                            // Load Font 
                            this.Initial();
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
                                else
                                {
                                    this.WriteText(canvas, fieldValue, fontStyle, x, y);
                                }
                            }
                            if (IsTesting)
                            {
                                fontStyle = SetFontStyle(fontStyle, "red", 96, "normal");
                                PdfContentByte canvas = stamper.GetOverContent(1);
                                this.WriteTestSystem(canvas, "ทดสอบระบบ", fontStyle, 75, 200);
                            }
                        }
                        buffer = result.ToArray();
                    }
                }
                catch (Exception ex)
                {
                    throw (ex);
                    //Log.WriteErrorLog(tsw.TraceError, ex);
                }
            }
            return buffer;
        }

        public void setImage(PdfContentByte canvas, string base64, float x, float y, float fitW, float fitH)
        {
            byte[] bytes = Convert.FromBase64String(base64);
            Image img = iTextSharp.text.Image.GetInstance(bytes);
            img.ScaleToFit(fitW, fitH);
            var posX = XPos(x);
            var posY = YPos(y);
            img.SetAbsolutePosition(posX, posY);
            canvas.AddImage(img);

        }

    }
}
