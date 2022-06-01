using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using io = System.IO;
using ZXing.QrCode;

namespace Register.Web.Controllers
{
    public class QrCodeController : Controller
    {
        // GET: Paper/QrCode
        public ActionResult GetQrCodeImage(string id)
        {
            var qrCodeWriter = new ZXing.BarcodeWriterPixelData
            {
                Format = ZXing.BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Height = 200,
                    Width = 200,
                    Margin = 1
                }
            };
            var qrText = id;
            var pixelData = qrCodeWriter.Write(qrText);
            var bmpTemp = pixelData.ToBitmap();

            byte[] b = null;
            using (var stream = new io.MemoryStream())
            {
                bmpTemp.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                b = stream.ToArray();
            }

            return File(b, "image/png");
        }
    }
}