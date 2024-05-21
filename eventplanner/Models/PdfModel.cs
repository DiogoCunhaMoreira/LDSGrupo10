using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System.IO;

namespace eventplanner.Models
{
    public delegate void PdfGeneratedHandler(object sender, PdfGeneratedEventArgs e);

    public class PdfGeneratedEventArgs : EventArgs
    {
        public MemoryStream? PdfStream { get; }
        public string? ErrorMessage { get; }

        public PdfGeneratedEventArgs(MemoryStream? pdfStream, string? errorMessage)
        {
            PdfStream = pdfStream;
            ErrorMessage = errorMessage;
        }
    }

    public class PdfModel
    {
        public event PdfGeneratedHandler? PdfGenerated;

        public string? Nome { get; set; }
        public string? Espetaculo { get; set; }
        public string? Lugar { get; set; }

        public void GerarPdf()
        {
            try
            {
                PdfDocument document = new PdfDocument();
                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);

                string logoPath = "wwwroot/images/logo.png";
                double logoWidth = 300;
                double logoHeight = 300;
                double xLogoPosition = (page.Width - logoWidth) / 2;
                double yLogoPosition = 50;

                if (File.Exists(logoPath))
                {
                    XImage logo = XImage.FromFile(logoPath);
                    gfx.DrawImage(logo, xLogoPosition, yLogoPosition, logoWidth, logoHeight);
                }

                double yTextStart = yLogoPosition + logoHeight + 20;

                XFont fontTitle = new XFont("Helvetica", 24, XFontStyleEx.Bold);
                XFont fontLabelBold = new XFont("Helvetica", 20, XFontStyleEx.Bold);
                XFont fontValueRegular = new XFont("Helvetica", 20, XFontStyleEx.Regular);

                string eventDate = "30-05-2024";

                gfx.DrawString("Bilhete do Espetáculo", fontTitle, XBrushes.Black, new XRect(0, yTextStart, page.Width, page.Height), XStringFormats.TopCenter);

                double yCurrentPosition = yTextStart + 50;

                double labelMargin = 50;
                double valueMargin = 250;

                gfx.DrawString("Nome:", fontLabelBold, XBrushes.Black, new XRect(labelMargin, yCurrentPosition, page.Width - 100, page.Height), XStringFormats.TopLeft);
                gfx.DrawString(Nome ?? "N/A", fontValueRegular, XBrushes.Black, new XRect(valueMargin, yCurrentPosition, page.Width - 100, page.Height), XStringFormats.TopLeft);
                yCurrentPosition += 40; 

                gfx.DrawString("Espetáculo:", fontLabelBold, XBrushes.Black, new XRect(labelMargin, yCurrentPosition, page.Width - 100, page.Height), XStringFormats.TopLeft);
                gfx.DrawString(Espetaculo ?? "N/A", fontValueRegular, XBrushes.Black, new XRect(valueMargin, yCurrentPosition, page.Width - 100, page.Height), XStringFormats.TopLeft);
                yCurrentPosition += 40;

                gfx.DrawString("Lugar:", fontLabelBold, XBrushes.Black, new XRect(labelMargin, yCurrentPosition, page.Width - 100, page.Height), XStringFormats.TopLeft);
                gfx.DrawString(Lugar ?? "N/A", fontValueRegular, XBrushes.Black, new XRect(valueMargin, yCurrentPosition, page.Width - 100, page.Height), XStringFormats.TopLeft);
                yCurrentPosition += 40;

                gfx.DrawString("Data do Espetáculo:", fontLabelBold, XBrushes.Black, new XRect(labelMargin, yCurrentPosition, page.Width - 100, page.Height), XStringFormats.TopLeft);
                gfx.DrawString(eventDate, fontValueRegular, XBrushes.Black, new XRect(valueMargin, yCurrentPosition, page.Width - 100, page.Height), XStringFormats.TopLeft);

                MemoryStream stream = new MemoryStream();
                document.Save(stream, false);
                stream.Position = 0;

                OnPdfGenerated(new PdfGeneratedEventArgs(stream, null));
            }
            catch (Exception ex)
            {
                OnPdfGenerated(new PdfGeneratedEventArgs(null, $"Erro a gerar o PDF: {ex.Message}"));
            }
        }

        protected virtual void OnPdfGenerated(PdfGeneratedEventArgs e)
        {
            PdfGenerated?.Invoke(this, e);
        }
    }
}