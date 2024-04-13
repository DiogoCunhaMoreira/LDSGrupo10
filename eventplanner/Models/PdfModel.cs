namespace eventplanner.Models;

using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System.IO;

public class PdfModel
{
    public string? Content { get; set; }

    public MemoryStream GerarPdf()
    {
        PdfDocument document = new PdfDocument();
        document.Info.Title = "Created with PDFSharp";
        PdfPage page = document.AddPage();
        XGraphics gfx = XGraphics.FromPdfPage(page);
        XFont font = new("Sedan", 20, XFontStyleEx.Regular);

        gfx.DrawString(this.Content, font, XBrushes.Black, new XRect(0, 0, page.Width, page.Height), XStringFormats.TopLeft);

        MemoryStream stream = new MemoryStream();
        document.Save(stream, false);
        stream.Position = 0;

        return stream;
    }
}


