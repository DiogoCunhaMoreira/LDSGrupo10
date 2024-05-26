namespace eventplanner.Controllers
{
    /*Esta interface permite que não se use diretamente a propriedade Tempdata,
    tirando esta dependencia do código, ou seja, digamos que no futuro queriamos usar
    outra propriedade diferente, só teriamos que mudar na interface e na classe que a implementa, o código 
    não....usar o chat para dar dicas mais bonitas.*/
    public interface ITempDataService
    {
        void Store(string key, object value);
        object Retrieve(string key);
        void Clear(string key);
    }
}
