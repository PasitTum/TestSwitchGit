using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Register.Reports.Helpers
{
    public class PdfHelper
    {
        public static byte[] MergePDFs(IEnumerable<byte[]> fileByte, string targetPdf)
        {
            using (var ms = new MemoryStream())
            {
                using (var doc = new Document())
                {
                    using (var copy = new PdfSmartCopy(doc, ms))
                    {
                        doc.Open();
                        foreach (var p in fileByte)
                        {
                            using (var reader = new PdfReader(p))
                            {
                                copy.AddDocument(reader);
                            }
                        }
                        doc.Close();
                    }
                }
                return ms.ToArray();
            }


            //byte[] buffer;
            //using (var result = new MemoryStream())
            //{
            //    Document document = new Document();
            //    PdfCopy pdf = new PdfCopy(document, result);
            //    PdfReader reader = null;
            //    document.Open();
            //    foreach (byte[] file in fileNames)
            //    {
            //        reader = new PdfReader(file);
            //        pdf.AddDocument(reader);
            //        reader.Close();
            //    }
            //    buffer = result.ToArray();
            //}
            
            //using (FileStream stream = new FileStream(targetPdf, FileMode.Create))
            //{
            //    Document document = new Document();
            //    PdfCopy pdf = new PdfCopy(document, stream);
            //    PdfReader reader = null;
            //    try
            //    {
            //        document.Open();
            //        foreach (byte[] file in fileNames)
            //        {
            //            reader = new PdfReader(file);
            //            pdf.AddDocument(reader);
            //            reader.Close();
            //        }
            //    }
            //    catch (Exception)
            //    {
            //        merged = false;
            //        if (reader != null)
            //        {
            //            reader.Close();
            //        }
            //    }
            //    finally
            //    {
            //        if (document != null)
            //        {
            //            document.Close();
            //        }
            //    }
            //}
            //return buffer;


        }
    }
}
