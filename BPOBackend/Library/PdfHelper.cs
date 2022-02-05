using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.IO;
using System.Text;

namespace BPOBackend
{
    public class PdfHelper
    {
        private static PdfHelper instance = null;
        public static PdfHelper Instance
        {
            get
            {
                return instance ?? new PdfHelper();
            }
        }

        public void SaveImageAsPdf(string imageFileName, string pdfFileName, int width = 600)
        {
            try
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                using (var document = new PdfDocument())
                {
                    PdfPage page = document.AddPage();
                    using (XImage img = XImage.FromFile(imageFileName))
                    {
                        // Calculate new height to keep image ratio
                        var height = (int)(((double)width / (double)img.PixelWidth) * img.PixelHeight);

                        // Change PDF Page size to match image
                        page.Width = width;
                        page.Height = height;

                        XGraphics gfx = XGraphics.FromPdfPage(page);
                        gfx.DrawImage(img, 0, 0, width, height);
                    }
                    document.Save(pdfFileName);
                }

                File.Delete(imageFileName);
            }
            catch(Exception ex)
            {

            }
        }
        public void FileFromBase64(string yourBase64String,string filename)
        {
            try
            {
                File.WriteAllBytes(filename, Convert.FromBase64String(yourBase64String));
            }
            catch (Exception ex)
            {

            }
        }
    }
}
