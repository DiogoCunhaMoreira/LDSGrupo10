using Microsoft.AspNetCore.Mvc;
using eventplanner.Models;
using eventplanner.Interfaces;
using System.IO;

namespace eventplanner.Controllers
{
    public class PDFController : Controller
    {
        private readonly IPdfService _pdfService;

        private readonly ITempDataService _tempDataService;

        // Construtor com injeção das dependências de IPdfService e ITempDataService
        public PDFController(IPdfService pdfService, ITempDataService tempDataService)
        {
            _pdfService = pdfService;
            _tempDataService = tempDataService;
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

                    // Verificação e retorno do PDF usando ITempDataService
                    var pdfStream = _tempDataService.Retrieve("PdfStream") as MemoryStream;
                    if (pdfStream != null)
                    {
                        _tempDataService.Clear("PdfStream"); // Limpar dados temporários após o uso
                        return File(pdfStream, "application/pdf", "GeneratedTicket.pdf");
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
                _tempDataService.Store("PdfStream", e.PdfStream);
            }
            else
            {
                _tempDataService.Store("FontErrorMessage", e.ErrorMessage);
                _tempDataService.Store("FontError", true);
            }
        }
    }
}

