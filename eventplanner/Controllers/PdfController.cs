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
                // Subscrição ao evento para que o controller seja notificado quando o evento for despoletado pelo PdfModel.
                model.PdfGenerated += OnPdfGenerated;

                // É este método que vai despoletar um evento.
                model.GerarPdf();
            }

            return View("Index", model);
        }

        // Método que lida com os dados que vêm do evento. 
        //Caso o PDF tenha sido gerado o método vai gerir a exibição do PDF ao utilizador.
        //Caso haja erro, vai exibir o erro.
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
