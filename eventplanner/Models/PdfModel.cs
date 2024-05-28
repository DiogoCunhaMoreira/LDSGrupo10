using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System;
using System.IO;
using eventplanner.Interfaces;


// essa classe é responsável por gerar um PDF com as informações de um bilhete de espetáculo
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

    // Implementa a interface IPdfService e define propriedades para o nome do cliente, nome do espetáculo e lugar do espetáculo
    public class PdfModel : IPdfService
    {
        public string? Nome { get; set; }
        public string? Espetaculo { get; set; }
        public string? Lugar { get; set; }
        public string FontName { get; set; } = "Helvetic";

        public event PdfGeneratedHandler? PdfGenerated;

        // Método para gerar o PDF
        public void GerarPdf(IPdfService model)
        {
            // Inicia a geração do PDF
            Console.WriteLine("Iniciando a geração do PDF...");
            try
            {

                Console.WriteLine("Processando geração do PDF...");

                // Cria um novo documento PDF
                PdfDocument document = new PdfDocument();
                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);

                // Define o caminho do logo, dimensões e posição
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

                XFont fontTitle = new XFont(model.FontName, 24, XFontStyleEx.Bold);
                XFont fontLabelBold = new XFont(model.FontName, 20, XFontStyleEx.Bold);
                XFont fontValueRegular = new XFont(model.FontName, 20, XFontStyleEx.Regular);

                string eventDate = "30-05-2024"; // Exemplo de data do espetáculo

                gfx.DrawString("Bilhete do Espetáculo", fontTitle, XBrushes.Black, new XRect(0, yTextStart, page.Width, page.Height), XStringFormats.TopCenter);

                double yCurrentPosition = yTextStart + 50;

                double labelMargin = 50;
                double valueMargin = 250;

                gfx.DrawString("Nome:", fontLabelBold, XBrushes.Black, new XRect(labelMargin, yCurrentPosition, page.Width - 100, page.Height), XStringFormats.TopLeft);
                gfx.DrawString(model.Nome ?? "N/A", fontValueRegular, XBrushes.Black, new XRect(valueMargin, yCurrentPosition, page.Width - 100, page.Height), XStringFormats.TopLeft);
                yCurrentPosition += 40;

                gfx.DrawString("Espetáculo:", fontLabelBold, XBrushes.Black, new XRect(labelMargin, yCurrentPosition, page.Width - 100, page.Height), XStringFormats.TopLeft);
                gfx.DrawString(model.Espetaculo ?? "N/A", fontValueRegular, XBrushes.Black, new XRect(valueMargin, yCurrentPosition, page.Width - 100, page.Height), XStringFormats.TopLeft);
                yCurrentPosition += 40;

                gfx.DrawString("Lugar:", fontLabelBold, XBrushes.Black, new XRect(labelMargin, yCurrentPosition, page.Width - 100, page.Height), XStringFormats.TopLeft);
                gfx.DrawString(model.Lugar ?? "N/A", fontValueRegular, XBrushes.Black, new XRect(valueMargin, yCurrentPosition, page.Width - 100, page.Height), XStringFormats.TopLeft);
                yCurrentPosition += 40;

                gfx.DrawString("Data do Espetáculo:", fontLabelBold, XBrushes.Black, new XRect(labelMargin, yCurrentPosition, page.Width - 100, page.Height), XStringFormats.TopLeft);
                gfx.DrawString(eventDate, fontValueRegular, XBrushes.Black, new XRect(valueMargin, yCurrentPosition, page.Width - 100, page.Height), XStringFormats.TopLeft);

                MemoryStream stream = new MemoryStream();
                document.Save(stream, false);
                stream.Position = 0;

                OnPdfGenerated(new PdfGeneratedEventArgs(stream, null));            }
            catch (Exception ex)
            {
                // Em caso de erro, exibe uma mensagem de erro
                Console.WriteLine($"Erro ao gerar PDF: {ex.Message}");
                string errorMessage = "Error generating PDF: " + ex.Message;
                OnPdfGenerated(new PdfGeneratedEventArgs(null, errorMessage));
            }
        }
        // Método para chamar o evento de PDF gerado
        public virtual void OnPdfGenerated(PdfGeneratedEventArgs e)
        {
            PdfGenerated?.Invoke(this, e);
        }
    }
}
