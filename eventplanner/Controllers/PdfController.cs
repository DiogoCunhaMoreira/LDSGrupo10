using Microsoft.AspNetCore.Mvc;
using eventplanner.Models;

namespace eventplanner.Controllers;

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
            var (pdfStream, errorMessage) = model.GerarPdf();
            if (pdfStream != null)
            {
                return File(pdfStream.ToArray(), "application/pdf", "GeneratedTicket.pdf");
            }
            ModelState.AddModelError(string.Empty, errorMessage ?? "Erro desconhecido ao gerar o PDF.");
        }

        return View("Index", model);
    }
}
