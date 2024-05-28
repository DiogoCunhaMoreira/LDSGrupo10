using Microsoft.AspNetCore.Mvc;
using eventplanner.Models;
using eventplanner.Interfaces;
using System.IO;


// Controller para gerar PDF
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
        // Método para criar PDF
        public ActionResult CriarPDF(PdfModel model)
        {
            Console.WriteLine("CriarPDF chamado.");
            if (ModelState.IsValid)
            {
                // Certifique-se de que o PdfModel injetado é utilizado

                if (_pdfService != null)
                {
                    // Transferindo dados do model recebido para o model injetado
                    _pdfService.Nome = model.Nome;
                    _pdfService.Espetaculo = model.Espetaculo;
                    _pdfService.Lugar = model.Lugar;
                    _pdfService.FontName = model.FontName;

                    // Assinando o evento no pdfModel injetado
                    _pdfService.PdfGenerated += OnPdfGenerated;

                    // Chamando o método para gerar PDF
                    _pdfService.GerarPdf(_pdfService);

                    // Verificação e retorno do PDF usando ITempDataService
                    var pdfStream = _tempDataService.Retrieve("PdfStream") as MemoryStream;
                    if (pdfStream != null)
                    {
                        _tempDataService.Clear("PdfStream"); // Limpar dados temporários após o uso
                        return File(pdfStream, "application/pdf", "GeneratedTicket.pdf");
                    }
                }
            }

            // Se não houver PDF ou se algum erro ocorreu, retorne à view com o modelo original
            return View("Index", model);
        }

        // Método para manipular o evento PdfGenerated
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
