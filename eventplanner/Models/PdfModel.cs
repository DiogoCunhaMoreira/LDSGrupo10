namespace eventplanner.Models;

using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System.IO;

public class PdfModel
{
    public string? Nome { get; set; }
    public string? Espetaculo { get; set; }
    public string? Lugar { get; set; }

    public (MemoryStream?, string?) GerarPdf()
    {
        try
        {
            PdfDocument document = new PdfDocument();
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Caminho para a imagem do logotipo
            string logoPath = "wwwroot/images/logo.png";
            double logoWidth = 300;
            double logoHeight = 300;
            double xLogoPosition = (page.Width - logoWidth) / 2;
            double yLogoPosition = 50;

            // Desenhe o logotipo, se existir
            if (File.Exists(logoPath))
            {
                XImage logo = XImage.FromFile(logoPath);
                gfx.DrawImage(logo, xLogoPosition, yLogoPosition, logoWidth, logoHeight);
            }

            // Posição inicial para o texto após o logotipo
            double yTextStart = yLogoPosition + logoHeight + 20;

            // Estilos de fonte
            XFont fontTitle = new XFont("Helvetica", 24, XFontStyleEx.Bold);
            XFont fontLabelBold = new XFont("Helvetica", 20, XFontStyleEx.Bold);
            XFont fontValueRegular = new XFont("Helvetica", 20, XFontStyleEx.Regular);

            // Data fixa do evento
            string eventDate = "2024-05-30";

            // Título no topo, centralizado
            gfx.DrawString("Bilhete do Espetáculo", fontTitle, XBrushes.Black, new XRect(0, yTextStart, page.Width, page.Height), XStringFormats.TopCenter);

            // Posição inicial dos detalhes
            double yCurrentPosition = yTextStart + 50;

            // Margem de alinhamento
            double labelMargin = 50;
            double valueMargin = 250;

            // Desenhar os rótulos e valores com espaçamento adequado
            gfx.DrawString("Nome:", fontLabelBold, XBrushes.Black, new XRect(labelMargin, yCurrentPosition, page.Width - 100, page.Height), XStringFormats.TopLeft);
            gfx.DrawString(Nome ?? "N/A", fontValueRegular, XBrushes.Black, new XRect(valueMargin, yCurrentPosition, page.Width - 100, page.Height), XStringFormats.TopLeft);
            yCurrentPosition += 40; // Espaçamento aumentado

            gfx.DrawString("Espetáculo:", fontLabelBold, XBrushes.Black, new XRect(labelMargin, yCurrentPosition, page.Width - 100, page.Height), XStringFormats.TopLeft);
            gfx.DrawString(Espetaculo ?? "N/A", fontValueRegular, XBrushes.Black, new XRect(valueMargin, yCurrentPosition, page.Width - 100, page.Height), XStringFormats.TopLeft);
            yCurrentPosition += 40;

            gfx.DrawString("Lugar:", fontLabelBold, XBrushes.Black, new XRect(labelMargin, yCurrentPosition, page.Width - 100, page.Height), XStringFormats.TopLeft);
            gfx.DrawString(Lugar ?? "N/A", fontValueRegular, XBrushes.Black, new XRect(valueMargin, yCurrentPosition, page.Width - 100, page.Height), XStringFormats.TopLeft);
            yCurrentPosition += 40;

            gfx.DrawString("Data do Espetáculo:", fontLabelBold, XBrushes.Black, new XRect(labelMargin, yCurrentPosition, page.Width - 100, page.Height), XStringFormats.TopLeft);
            gfx.DrawString(eventDate, fontValueRegular, XBrushes.Black, new XRect(valueMargin, yCurrentPosition, page.Width - 100, page.Height), XStringFormats.TopLeft);

            // Salvar o documento no fluxo de memória
            MemoryStream stream = new MemoryStream();
            document.Save(stream, false);
            stream.Position = 0;

            return (stream, null);
        }
        catch (Exception ex)
        {
            return (null, $"Erro a gerar o PDF: {ex.Message}");
        }
    }
}
