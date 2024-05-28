using Microsoft.AspNetCore.Mvc.ViewFeatures;
using eventplanner.Interfaces;


// Implementação de um serviço para manipulação de TempData
namespace eventplanner.Controllers
{
    public class TempDataService : ITempDataService
    {
        private ITempDataDictionary _tempData;

        public TempDataService(ITempDataDictionaryFactory tempDataDictionaryFactory, IHttpContextAccessor httpContextAccessor)
        {
            // Cria uma instância de TempData utilizando o factory e o HttpContext atual
            _tempData = tempDataDictionaryFactory.GetTempData(httpContextAccessor.HttpContext);
        }

        public void Store(string key, object value)
        {
            _tempData[key] = value;
        }

        public object Retrieve(string key)
        {
            _tempData.TryGetValue(key, out var value);
            return value;
        }

        public void Clear(string key)
        {
            _tempData.Remove(key);
        }
    }
}
