using System.IO;
using eventplanner.Models;

/*Esta interface permite que o controller nao chama-se o model diretamente para gerar o PDF
Ver no chat e ele explica melhor isto. Mas na realidade tirou-se a dependencia que o controller tinha do model, 
ou seja, torna o progama mais modular e facil de manter e bla bla...o chat fala melhor.
*/

namespace eventplanner.Interfaces
{
    public interface IPdfService
    {
        void GerarPdf(PdfModel model);
    }
}
