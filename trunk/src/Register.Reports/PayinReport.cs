﻿using CSP.Lib.Json;
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
    public class PayinReport : BaseReport
    {
        private JObject enrollDatas { get; set; }
        public bool IsTesting { get; set; }
        BaseFont _baseFont;
        Font Font;

        public PayinReport()
        {
            this.ReportName = "Payin";
            this.SettingFileName = "payin-setting.json";
            this.TemplateFileName = "PayinSlipTemplate.pdf";
            this.ReadSetting();
        }

        public PayinReport(string jsonDataText) : base(jsonDataText)
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
                var compCode = enrollDatas.GetValueIgnoreCase("COMP_CODE") == null ? "" : enrollDatas.GetValueIgnoreCase("COMP_CODE");
                var refNo1 = enrollDatas.GetValueIgnoreCase("REF1") == null ? "" : enrollDatas.GetValueIgnoreCase("REF1");
                var refNo2 = enrollDatas.GetValueIgnoreCase("REF2") == null ? "" : enrollDatas.GetValueIgnoreCase("REF2");
                var suffix = enrollDatas.GetValueIgnoreCase("SUFFIX") == null ? "" : enrollDatas.GetValueIgnoreCase("SUFFIX");
                var taxId = enrollDatas.GetValueIgnoreCase("TAX_ID") == null ? "" : enrollDatas.GetValueIgnoreCase("TAX_ID");
                var amount = enrollDatas.GetValueIgnoreCase("AMOUNT") == null ? "" : enrollDatas.GetValueIgnoreCase("AMOUNT");
                var amountTotal = enrollDatas.GetValueIgnoreCase("AMOUNT_TOTAL") == null ? "" : enrollDatas.GetValueIgnoreCase("AMOUNT_TOTAL");
                var titleName = enrollDatas.GetValueIgnoreCase("TITLE_NAME_TH") == null ? "" : enrollDatas.GetValueIgnoreCase("TITLE_NAME_TH");
                var firstName = enrollDatas.GetValueIgnoreCase("FNAME_TH") == null ? "" : enrollDatas.GetValueIgnoreCase("FNAME_TH");

                var amountOnly = "0";
                if (!string.IsNullOrEmpty(amountTotal))
                {

                    var AMT = enrollDatas.GetValueIgnoreCase("IB_IR_AMT") == null ? "" : enrollDatas.GetValueIgnoreCase("IB_IR_AMT");
                    if (!string.IsNullOrEmpty(AMT))
                    {
                        amountOnly = (int.Parse(amountTotal) - int.Parse(AMT)).ToString();
                    }
                }

                EncryptHelper encryptHelper = new EncryptHelper();
                var citizen = enrollDatas.GetValueIgnoreCase("CITIZEN_ID") == null ? "" : enrollDatas.GetValueIgnoreCase("CITIZEN_ID");
                if (citizen != null & citizen != "")
                {
                    citizen = encryptHelper.DecryptData(citizen);
                }
                var password = enrollDatas.GetValueIgnoreCase("ENROLL_PASSWORD") == null ? "" : enrollDatas.GetValueIgnoreCase("ENROLL_PASSWORD");
                if (password != null & password != "")
                {
                    password = encryptHelper.DecryptData(password);
                }

                try
                {
                    using (var reader = new PdfReader(templateFile))
                    {
                        using (var stamper = new PdfStamper(reader, result))
                        {
                            this.Initial();
                            var barcodeText = PaymentExtension.FormatPaymentBarCode(refNo1, refNo2.ToString(), taxId, suffix, Decimal.Parse(amount));
                            var qrcodeText = PaymentExtension.FormatPaymentBarCode(refNo1, refNo2.ToString(), taxId, suffix, Decimal.Parse(amount));
                            var amountTh = ThaiBaht.ToBahtText(Convert.ToDouble(amount));

                            // Load Font 
                            this.LoadFont(reader);
                            var fontStyle = this.Font;
                            if (IsTesting)
                            {
                                fontStyle = SetFontStyle(fontStyle, "red", 96, "normal");
                                PdfContentByte canvas = stamper.GetOverContent(1);
                                this.WriteTestSystem(canvas, "ทดสอบระบบ", fontStyle, 70, 220);
                            }
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

                                if (string.Compare(objectType, "barcode", true) == 0)
                                {
                                    CreateBarcode(canvas, barcodeText, x, y, fontStyle);
                                }
                                else if (string.Compare(objectType, "ip", true) == 0)
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
                                else if (string.Compare(objectType, "qrcode", true) == 0)
                                {
                                    CreateQrcode(canvas, qrcodeText, x, y);
                                }
                                else if (string.Compare(objectType, "price", true) == 0)
                                {
                                    this.WriteText(canvas, Decimal.Parse(fieldValue).ToString("####.00"), fontStyle, x, y);
                                }
                                else if (string.Compare(objectType, "exAmountTh", true) == 0)
                                {
                                    this.WriteText(canvas, "(" + fieldValue + ")", fontStyle, x, y);
                                }
                                else if (string.Compare(objectType, "amountTh", true) == 0)
                                {
                                    this.WriteText(canvas, amountTh, fontStyle, x, y);
                                }
                                else if (string.Compare(objectType, "citizen", true) == 0)
                                {
                                    this.WriteText(canvas, citizen, fontStyle, x, y);
                                }
                                else if (string.Compare(objectType, "firstwithtitle", true) == 0)
                                {
                                    this.WriteText(canvas, titleName + firstName, fontStyle, x, y);
                                }
                                else if (string.Compare(objectType, "password", true) == 0)
                                {
                                    this.WriteText(canvas, password, fontStyle, x, y);
                                }
                                else if (string.Compare(objectType, "manylines", true) == 0)
                                {
                                    this.WriteTextMultiLines(canvas, fieldValue, fontStyle, x, y, w, h, lh);
                                }
                                else if (string.Compare(objectType, "manylines-center", true) == 0)
                                {
                                    this.WriteTextCenterMultiLines(canvas, fieldValue, fontStyle, x, y, w, h, lh);
                                }
                                else if (string.Compare(objectType, "AmountOnly", true) == 0)
                                {
                                    this.WriteText(canvas, amountOnly, fontStyle, x, y);
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
                    Log.WriteErrorLog(tsw.TraceError, ex);
                    throw (ex);
                }
            }
            return buffer;
        }

        protected void WriteTextCenterMultiLines(PdfContentByte canvas, string text, Font font, float x, float y, float w, float h, float lh)
        {
            //lh คือ space between lines
            Chunk chunk = new Chunk(text ?? "", font);
            Phrase phrase = new Phrase(chunk);
            var posX = Utilities.MillimetersToPoints(x);
            var posY = YPos(y);
            ColumnText ct = new ColumnText(canvas);
            ct.SetSimpleColumn(phrase, posX, posY, posX + w, posY + h, lh, Element.ALIGN_CENTER);
            ct.Go();
        }
    }
}
