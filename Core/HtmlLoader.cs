
using System.Net;
using System.Web;

using ПарсерЗакупки.Core.Exeptions;
using ПарсерЗакупки.Core.Interfaces;

namespace ПарсерЗакупки.Core
{
    class HtmlLoader
    {
        readonly HttpClient _client;
        readonly string _url;

        public HtmlLoader(IParserSettings settings)
        {
            //ошибка вызова GetAsync решилась при изменение заголовков класса клиент
            // the GetAsync call error was solved by changing the client class headers
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
            _url = settings.BaseUrl;
        }

        public async Task<string> GetSourceByNumAndName(int num, string name)
        {
            //кодируем слова, по которому идёт выборка закупок
            //encode the name by which the purchases are selected
            var decodeName = HttpUtility.UrlEncode(name);
            //вставляем в строку запроса актуальные данные о: наименорвании закупки и номера страницы
            //insert the actual data about: purchase name and page number into the query string 
            var currentUrl = _url.Replace("{purchaseName}", decodeName);
            currentUrl = currentUrl.Replace("{number}", num.ToString());
            //осуществляем GET запрос
            //make a GET request
            var response = _client.GetAsync(currentUrl, HttpCompletionOption.ResponseHeadersRead).Result;
            //store the page code
            string source = null;

            try
            {
                if (response != null && response.StatusCode == HttpStatusCode.OK)
                    //get page code
                    source = response.Content.ReadAsStringAsync().Result.ToString();
                else
                    throw new HttpRequestError($"Failed to retrieve page data. Error: {response.StatusCode}");
            }

            catch(HttpRequestError ex)
            {
                Console.WriteLine(ex.Message);
            }

            return source;
        }
    }
}
