using Microsoft.AspNetCore.Mvc;
using System.IO;
using eventplanner.Models;

namespace eventplanner.Controllers;

public class PDFController : Controller
{
    public IActionResult Index()
    {
        return View(new PdfModel()); // Pass an empty model to the view initially
    }

    [HttpPost]
    public ActionResult CriarPDF(PdfModel model)
    {
        if (ModelState.IsValid)
        {
            MemoryStream pdfStream = model.GerarPdf();
            return File(pdfStream.ToArray(), "application/pdf", "GeneratedDocument.pdf");
        }
        return View("Index", model); // Return with the same model if something is wrong
    }
}
