using CSP.Lib.Models;
using Register.Reports.Helpers;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.Web;
using Newtonsoft.Json;
using System.Data;

namespace Register.Reports
{
    public abstract class BaseReport : IGenaricReport, IDisposable
    {
        public System.Diagnostics.TraceSwitch tsw = new System.Diagnostics.TraceSwitch("mySwitch", "");
        protected string PdfPath = "";
        public string ReportName { get; set; }
        //public JObject JsonData { get; set; }
        public string JsonData { get; set; }
        public string SettingFileName { get; set; }

        public string TemplateFileName { get; set; }
        public DataSet ds { get; set; }

        public abstract byte[] GetReport();
        //public abstract ResultInfo GetReport(string documentPath);

        public abstract void RefreshDataSource();

        protected JArray SettingData;

        public BaseReport() { }
        public BaseReport(string jsonDataText)
        {

        }

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
                return (ConfigurationManager.AppSettings["KeepPdfFile"] ?? "") == "Y";
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


        private bool _disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {

                // ถ้า inherit แล้วต้องการเคลียร์ object ที่เป็น managed ให้มาเติม code ตรงนี้ด้วย 
                // เช่น _safeHandle?.Dispose();
            }

            _disposed = true;
        }

        public void Dispose()
        {
            // Dispose of unmanaged resources.
            Dispose(true);
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        protected void ReadSetting()
        {
            var settingFile = this.GetFileNameInReportPath(this.SettingFileName);
            var jsonSetting = File.ReadAllText(settingFile);
            this.SettingData = (JArray)JsonConvert.DeserializeObject(jsonSetting);
        }

        protected void WriteText(PdfContentByte canvas, string text, Font font, float x, float y)
        {
            Phrase phrase = new Phrase(text ?? "", font);
            var posX = Utilities.MillimetersToPoints(x);
            var posY = YPos(y);
            ColumnText.ShowTextAligned(canvas, Element.ALIGN_LEFT, phrase, posX, posY, 0);
        }

        protected float XPos(float xPosInMM)
        {
            return Utilities.MillimetersToPoints(xPosInMM);
        }
        protected float YPos(float yPosInMM)
        {
            return Utilities.InchesToPoints(11.692f) - Utilities.MillimetersToPoints(yPosInMM);
        }

        // ค้นหา Font ที่ Embeded มาใน pdf
        protected BaseFont FindFontInForm(PdfReader reader, string fontname)
        {
            var embededFonts = BaseFont.GetDocumentFonts(reader);
            if (embededFonts != null)
            {
                foreach (var f in embededFonts)
                {
                    var name = f[0] as string;
                    if (name.Contains(fontname))
                    {                        
                        return BaseFont.CreateFont((PRIndirectReference)f[1]);
                    }
                }
            }
            return null;

            //PdfDictionary root = reader.Catalog;
            //PdfDictionary dic = reader.GetPageN(1);
            //PdfDictionary resources = dic.GetAsDict(PdfName.RESOURCES);
            //PdfDictionary fonts = resources.GetAsDict(PdfName.FONT);
            //if (fonts == null) return null;
            //PdfDictionary font;
            //foreach (PdfName key in fonts.Keys)
            //{
            //    font = fonts.GetAsDict(key);
            //    var name = font.GetAsName(PdfName.BASEFONT).ToString();
            //    if (name.Contains(fontname))
            //    {
            //        return BaseFont.CreateFont((PRIndirectReference)fonts.GetAsIndirectObject(key));
            //    }
            //}
            //return null;
        }

        /// <summary>
        /// set style ด้วย normal, bold, italic, underline, strike ถ้าต้องการ หลาย style ให้ใส่ style ที่ต้องการแล้วแยกด้วยเว้นวรรค
        /// เช่น ถ้าต้องการ ตัวหนา และ ตัวเอียง ให้ส่งค่ามาเป็น "bold italic"
        /// </summary>
        protected Font SetFontStyle(Font font, string color, int size, string style)
        {
            var f = new Font(font);
            f.SetStyle(Font.NORMAL);
            f.Size = size;
            if (style != "")
            {
                f.SetStyle(style.ToLower());
            }
            var resultColor = ColorHelper.ConvertToColor(color);
            f.SetColor(resultColor.R, resultColor.G, resultColor.B);
            return f;

            //reset font style to normal           
            //font.SetStyle(Font.NORMAL);
            //font.Size = size;
            //if (style != "")
            //{
            //    font.SetStyle(style.ToLower());
            //}
            //var resultColor = ColorHelper.ConvertToColor(color);
            //font.SetColor(resultColor.R, resultColor.G, resultColor.B);
            //return font;
        }

        //พิมพ์ข้อความหลายบรรทัด
        protected void WriteTextMultiLines(PdfContentByte canvas, string text, Font font, float x, float y, float w, float h, float lh)
        {
            //lh คือ space between lines
            Chunk chunk = new Chunk(text ?? "", font);
            Phrase phrase = new Phrase(chunk);
            var posX = Utilities.MillimetersToPoints(x);
            var posY = YPos(y);
            ColumnText ct = new ColumnText(canvas);
            ct.SetSimpleColumn(phrase, posX, posY, posX + w, posY + h, lh, Element.ALIGN_LEFT);
            ct.Go();
        }

        protected void CreateBarcode(PdfContentByte canvas, String code, float x, float y, Font textFont)
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

        protected void CreateQrcode(PdfContentByte canvas, String code, float x, float y)
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

        protected void SetImage(PdfContentByte canvas, string base64, float x, float y, float fitW, float fitH)
        {
            byte[] bytes = Convert.FromBase64String(base64);
            Image img = iTextSharp.text.Image.GetInstance(bytes);
            img.ScaleToFit(fitW, fitH);
            var posX = XPos(x);
            var posY = YPos(y);
            img.SetAbsolutePosition(posX, posY);
            canvas.AddImage(img);
        }

        public string GetMonthName(string pMonth)
        {
            int iMonth;
            bool success = Int32.TryParse(pMonth, out iMonth);
            switch (iMonth)
            {
                case 1:
                    return "มกราคม";
                case 2:
                    return "กุมภาพันธ์";
                case 3:
                    return "มีนาคม";
                case 4:
                    return "เมษายน";
                case 5:
                    return "พฤษภาคม";
                case 6:
                    return "มิถุนายน";
                case 7:
                    return "กรกฎาคม";
                case 8:
                    return "สิงหาคม";
                case 9:
                    return "กันยายน";
                case 10:
                    return "ตุลาคม";
                case 11:
                    return "พฤศจิกายน";
                case 12:
                    return "ธันวาคม";
                default:
                    return pMonth;
            }
        }

        public string GetMonthAbbrName(string pMonth)
        {
            int iMonth;
            bool success = Int32.TryParse(pMonth, out iMonth);
            switch (iMonth)
            {
                case 1:
                    return "ม.ค.";
                case 2:
                    return "ก.พ.";
                case 3:
                    return "มี.ค.";
                case 4:
                    return "เม.าย.";
                case 5:
                    return "พ.ค.";
                case 6:
                    return "มิ.ย.";
                case 7:
                    return "ก.ค.";
                case 8:
                    return "ส.ค.";
                case 9:
                    return "ก.ย.";
                case 10:
                    return "ต.ค.";
                case 11:
                    return "พ.ย.";
                case 12:
                    return "ธ.ค.";
                default:
                    return pMonth;
            }
        }

        //(PdfContentByte canvas, string text, Font font, float x, float y)
        protected void WriteTestSystem(PdfContentByte canvas, string text, Font font, float x, float y)
        {
            Phrase phrase = new Phrase(text ?? "", font);
            var posX = Utilities.MillimetersToPoints(x);
            var posY = YPos(y);
            ColumnText.ShowTextAligned(canvas, Element.ALIGN_LEFT, phrase, posX, posY, 45);
        }
    }
}
