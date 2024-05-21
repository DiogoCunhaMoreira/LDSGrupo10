using Microsoft.AspNetCore.Mvc;
using eventplanner.Models;

namespace eventplanner.Controllers
{
    public class PDFController : Controller
    {
        public IActionResult Index()
        {
            return View(new PdfModel());
        }

        [HttpPost]
        public ActionResult CriarPDF(PdfModel model)
        {
            if (ModelState.IsValid)
            {

                model.PdfGenerated += OnPdfGenerated;
                model.GerarPdf();
            }

            return View("Index", model);
        }

        private void OnPdfGenerated(object sender, PdfGeneratedEventArgs e)
        {
            if (e.PdfStream != null)
            {
                Response.Headers.Add("Content-Disposition", "inline; filename=GeneratedTicket.pdf");
                Response.ContentType = "application/pdf";
                Response.Body.WriteAsync(e.PdfStream.ToArray(), 0, (int)e.PdfStream.Length).Wait();
            }
            else
            {
                ModelState.AddModelError(string.Empty, e.ErrorMessage ?? "Erro desconhecido ao gerar o PDF.");
            }
        }
    }
}

