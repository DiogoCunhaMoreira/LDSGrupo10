
public delegate void ProcessamentoCompraHandler(string mensagem);

public class BilheteController : Controller
{
    public static event ProcessamentoCompraHandler AntesDaCompra;
    public static event ProcessamentoCompraHandler AposCompra;


    public IActionResult Comprar(BilheteCompraModel model)
    {
        if (!ModelState.IsValid)
        {
            // Retorna a view com o modelo para mostrar erros de validação
            return View(model);
        }

        try
        {
            AntesDaCompra?.Invoke("A iniciar o processo de compra do bilhete. ")
            // implementar a lógica para processar a compra do bilhete
            // Pverificar a disponibilidade do bilhete, calcular o preço, etc.

            AposCompra?.Invoke("Processo de compra do bilhete concluido com sucesso.");

            return RedirectToAction("ConfirmacaoCompra"); 
        }
        catch (Exception ex)
        {
            // Descrever r o erro (considere usar ILogger<BilheteController>)
            // Retornar a view de erro com a mensagem de exceção
            return View("Error", new ErrorViewModel { Message = ex.Message });
        }
    }

    public IActionResult ConfirmacaoCompra()
    {
        // Retorna a view de confirmação de compra
        return View();
    }
}