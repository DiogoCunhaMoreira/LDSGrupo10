using Microsoft.AspNetCore.Mvc;
using eventplanner.Models;
using eventplanner.Interfaces;
using System.IO;

namespace eventplanner.Controllers
{
    public class PDFController : Controller
    {
        private readonly IPdfService _pdfService;

        // Construtor com injeção das dependências de IPdfService e ITempDataService
        public PDFController(IPdfService pdfService)
        {
            _pdfService = pdfService;
        }

        public IActionResult Index()
        {
            // Retorna uma nova instância de PdfModel para bind de dados do formulário
            return View(new PdfModel());
        }

        [HttpPost]
        public ActionResult CriarPDF(PdfModel model)
        {
            Console.WriteLine("CriarPDF chamado.");
            if (ModelState.IsValid)
            {
                // Certifique-se de que o PdfModel injetado é utilizado
                var pdfModel = _pdfService as PdfModel;

                if (pdfModel != null)
                {
                    // Transferindo dados do model recebido para o model injetado
                    pdfModel.Nome = model.Nome;
                    pdfModel.Espetaculo = model.Espetaculo;
                    pdfModel.Lugar = model.Lugar;
                    pdfModel.FontName = model.FontName;

                    // Assinando o evento no pdfModel injetado
                    pdfModel.PdfGenerated += OnPdfGenerated;

                    // Chamando o método para gerar PDF
                    _pdfService.GerarPdf(pdfModel);

                    MemoryStream pdfStream = new MemoryStream();
                    // Verificação e retorno do PDF usando ITempDataService
                    TempData["PdfStream"] = pdfStream;

                    // Para recuperar e retornar o MemoryStream de TempData
                    if (TempData["PdfStream"] is MemoryStream retrievedStream)
                    {
                        TempData.Remove("PdfStream"); // Limpar dados temporários após o uso

                        retrievedStream.Position = 0;
                        return File(retrievedStream, "application/pdf", "GeneratedTicket.pdf");
                    }
                }
            }

            return View("Index", model);
        }

        // Método que lida com os dados que vêm do evento. 
        //Caso o PDF tenha sido gerado o método vai gerir a exibição do PDF ao utilizador.
        //Caso haja erro, vai exibir o erro.
        private void OnPdfGenerated(object sender, PdfGeneratedEventArgs e)
        {
            Console.WriteLine("OnPdfGenerated chamado.");
            if (e.PdfStream != null)
            {
                // Armazenando o stream do PDF em TempData
                TempData["PdfStream"] = e.PdfStream;
            }
            else
            {
                // Armazenando a mensagem de erro e um indicador de erro em TempData
                TempData["FontErrorMessage"] = e.ErrorMessage;
                TempData["FontError"] = true;
            }
        }
    }
}

