using CSP.Lib.Extension;
using CSP.Lib.Json;
using CSP.Lib.Models;
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

namespace Register.Reports
{
    public class SMSPayinReport : BaseReport
    {
        System.Diagnostics.TraceSwitch tsw = new System.Diagnostics.TraceSwitch("mySwitch", "CSP.OSAT.Reports");

        string TemplateFileName = "SMSPayinSlipTemplate.pdf";
        string SettingFileName = "sms-setting.json";
        public bool IsTesting { get; set; }
        private string PdfPath = "";
        private JArray SettingData;
        //private JArray CustomerData;
        private JObject enrollDatas { get; set; }
        Dictionary<string, Font> font = new Dictionary<string, Font>();

        BaseFont _baseFont;
        Font Font;
        public string ReportPath
        {
            get
            {
                return ConfigurationManager.AppSettings["ReportPath"];
            }
        }

        public bool KeepPdfFile
        {
            get
            {
                return false;
                // return (ConfigurationManager.AppSettings["KeepPdfFile"] ?? "") == "Y";
            }
        }

        public string GetFileNameInReportPath(string rptFile)
        {
            return Path.Combine(this.ReportPath, rptFile);
        }

        private string _fileDownloadName = "";
        public string FileDownloadName
        {
            get
            {
                return _fileDownloadName;
            }
            set
            {
                _fileDownloadName = value;
                if (!_fileDownloadName.EndsWith(".pdf"))
                {
                    _fileDownloadName += ".pdf";
                }
            }
        }

        public string LocalFileName
        {
            get
            {
                return Path.Combine(this.PdfPath, this.FileDownloadName);
            }
        }

        public bool IsLocalFileExists()
        {
            return File.Exists(this.LocalFileName);
        }

        public static string GetServerIP()
        {
            var inputString = HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"] ?? "";
            return inputString ?? "";
        }

        public static string GetServerIPLast3()
        {
            var inputString = HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"] ?? "";
            var lastIP = inputString.Substring(inputString.LastIndexOf('.') + 1);
            lastIP = lastIP.PadLeft(3, '0');
            return lastIP ?? "";
        }

        public SMSPayinReport()
        {
            this.ReportName = "SmsPayin";
            this.ReadSetting();
        }

        public SMSPayinReport(string jsonDataText) : base(jsonDataText)
        {
            this.JsonData = jsonDataText;
            this.ReadSetting();
        }

        private void ReadSetting()
        {
            var settingFile = this.GetFileNameInReportPath(this.SettingFileName);
            var jsonSetting = File.ReadAllText(settingFile);
            this.SettingData = (JArray)JsonConvert.DeserializeObject(jsonSetting);
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
            BaseFont thSarabun = BaseFont.CreateFont(this.GetFileNameInReportPath("THSarabunNew.ttf"), BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            Font thSara8 = new Font(thSarabun, 8);
            Font thSara12 = new Font(thSarabun, 12);
            Font thSara14 = new Font(thSarabun, 14);
            Font thSara16 = new Font(thSarabun, 16);
            Font thSara8Bold = new Font(thSarabun, 8, Font.BOLD);
            Font thSara12Bold = new Font(thSarabun, 12, Font.BOLD);
            Font thSara14Bold = new Font(thSarabun, 14, Font.BOLD);
            Font thSara16Bold = new Font(thSarabun, 16, Font.BOLD);

            this.font = new Dictionary<string, Font>() {
                            { "8", thSara8 },
                            { "8-bold", thSara8Bold },
                            { "12", thSara12 },
                            { "12-bold", thSara12Bold },
                            { "14", thSara14 },
                            { "14-bold", thSara14Bold },
                            { "16", thSara16 },
                            { "16-bold", thSara16Bold }
                        };

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
                var compCode = enrollDatas.GetValueIgnoreCase("SMS_COMP_CODE");
                var refNo1 = enrollDatas.GetValueIgnoreCase("SMS_REF_1");
                var refNo2 = enrollDatas.GetValueIgnoreCase("SMS_REF_2");
                var suffix = enrollDatas.GetValueIgnoreCase("SMS_SUFFIX");
                var taxId = enrollDatas.GetValueIgnoreCase("SMS_TAX_ID");
                var amount = enrollDatas.GetValueIgnoreCase("SMS_AMOUNT");

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
                            var barcodeText = PaymentExtension.FormatPaymentBarCode(refNo1, refNo2.ToString(), taxId, suffix, Decimal.Parse(amount));
                            var amountTh = ThaiBaht.ToBahtText(Convert.ToDouble(amount));

                            //var page = setting.GetValueIgnoreCase("page") == null ? 1 : int.Parse(setting.GetValueIgnoreCase("page"));
                            PdfContentByte canvas = stamper.GetOverContent(1);


                            //PdfStamper stamper = null;

                            //reader = new PdfReader(templateFile);
                            //var targetFileStream = new FileStream(targetFile, FileMode.Create);
                            //stamper = new PdfStamper(reader, targetFileStream);
                            //PdfContentByte canvas = stamper.GetOverContent(1);


                            if (IsTesting)
                            {
                                fontStyle = SetFontStyle(fontStyle, "red", 96, "normal");
                                this.WriteTestSystem(canvas, "ทดสอบระบบ", fontStyle, 70, 200);
                            }
                            foreach (JObject setting in this.SettingData)
                            {
                                var x = float.Parse(setting.GetValueIgnoreCase("x"));
                                var y = float.Parse(setting.GetValueIgnoreCase("y"));

                                var fieldName = setting.GetValueIgnoreCase("fieldname");
                                var fieldValue = enrollDatas.GetValueIgnoreCase(fieldName);

                                //var page = setting.GetValueIgnoreCase("page") == null ? 1 : int.Parse(setting.GetValueIgnoreCase("page"));
                                //PdfContentByte canvas = stamper.GetOverContent(page);
                                //var fontStyle = new Font();

                                if (setting.GetValueIgnoreCase("size") != null)
                                {
                                    fontStyle = font[setting.GetValueIgnoreCase("size") + (setting.GetValueIgnoreCase("weight") == null ? "" : "-" + setting.GetValueIgnoreCase("weight"))];
                                }

                                var objectType = setting.GetValueIgnoreCase("objecttype");

                                if (string.Compare(objectType, "barcode", true) == 0)
                                {
                                    createBarcode(canvas, barcodeText, x, y, fontStyle);
                                }
                                else if (string.Compare(objectType, "ip", true) == 0)
                                {
                                    this.WriteText(canvas, GetServerIPLast3(), fontStyle, x, y);
                                }
                                else if (string.Compare(objectType, "date", true) == 0)
                                {
                                    this.WriteText(canvas, DateTime.Now.ToDateTextThai(), fontStyle, x, y);
                                }
                                else if (string.Compare(objectType, "qrcode", true) == 0)
                                {
                                    createQrcode(canvas, barcodeText, x, y);
                                }
                                else if (string.Compare(objectType, "price", true) == 0)
                                {
                                    this.WriteText(canvas, Decimal.Parse(fieldValue).ToString("####.00"), fontStyle, x, y);
                                }
                                else if (string.Compare(objectType, "amountTh", true) == 0)
                                {
                                    this.WriteText(canvas, amountTh, fontStyle, x, y);
                                }
                                else
                                {
                                    this.WriteText(canvas, fieldValue, fontStyle, x, y);
                                }
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

        private void WriteText(PdfContentByte canvas, string text, Font font, float x, float y)
        {
            Phrase phrase = new Phrase(text ?? "", font);
            var posX = Utilities.MillimetersToPoints(x);
            var posY = YPos(y);
            ColumnText.ShowTextAligned(canvas, Element.ALIGN_LEFT, phrase, posX, posY, 0);
        }

        private float XPos(float xPosInMM)
        {
            return Utilities.MillimetersToPoints(xPosInMM);
        }
        private float YPos(float yPosInMM)
        {
            return Utilities.InchesToPoints(11.692f) - Utilities.MillimetersToPoints(yPosInMM);
        }

        // ค้นหา Font ที่ Embeded มาใน pdf
        public BaseFont FindFontInForm(PdfReader reader, string fontname)
        {
            PdfDictionary root = reader.Catalog;
            PdfDictionary dic = reader.GetPageN(1);
            PdfDictionary resources = dic.GetAsDict(PdfName.RESOURCES);
            PdfDictionary fonts = resources.GetAsDict(PdfName.FONT);
            if (fonts == null) return null;
            PdfDictionary font;
            foreach (PdfName key in fonts.Keys)
            {
                font = fonts.GetAsDict(key);
                String name = font.GetAsName(PdfName.BASEFONT).ToString();
                if (name.Contains(fontname))
                {
                    return BaseFont.CreateFont((PRIndirectReference)fonts.GetAsIndirectObject(key));
                }
            }
            return null;
        }


        public void createBarcode(PdfContentByte canvas, String code, float x, float y, Font textFont)
        {
            Barcode128 code128 = new Barcode128();
            code128.CodeType = iTextSharp.text.pdf.Barcode.CODE128_UCC;
            code128.ChecksumText = true;
            code128.GenerateChecksum = true;
            code128.StartStopText = false;
            code128.Code = code;

            code128.BarHeight = Utilities.MillimetersToPoints(10f);         // great! but what about width???
            code128.X = 0.65f;                                              // Sets the minimum bar width.

            //var img = code128.CreateImageWithBarcode(canvas, BaseColor.BLACK, BaseColor.GRAY);
            code128.Font = null;
            var img = code128.CreateImageWithBarcode(canvas, null, null);
            var posX = XPos(x);
            var posY = YPos(y);
            img.SetAbsolutePosition(posX, posY);
            canvas.AddImage(img);

            var displayCode = code.Replace("\r", " ");
            var textX = x + 18f;
            var textY = y + 4f;
            this.WriteText(canvas, displayCode, textFont, textX, textY);
        }

        public void createQrcode(PdfContentByte canvas, String code, float x, float y)
        {
            var QCwriter = new BarcodeWriter();
            QCwriter.Format = BarcodeFormat.QR_CODE;
            var result = QCwriter.Write(code);

            var barcodeBitmap = new System.Drawing.Bitmap(result);

            Image img = Image.GetInstance(barcodeBitmap, System.Drawing.Imaging.ImageFormat.Jpeg);

            var posX = XPos(x);
            var posY = YPos(y);
            img.SetAbsolutePosition(posX, posY);
            canvas.AddImage(img);

        }

    }
}
