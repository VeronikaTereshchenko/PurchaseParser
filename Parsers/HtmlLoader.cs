using System.Net;
using System.Web;
using PurchaseParser.Parsers.Interfaces;
using PurchaseParser.Parsers.Exeptions;

namespace PurchaseParser.Parsers
{
    static class HtmlLoader
    {
        private static SocketsHttpHandler socketHandler = new SocketsHttpHandler
        {
            PooledConnectionLifetime = TimeSpan.FromMinutes(2)
        };

        private static HttpClient _httpClient = new HttpClient(socketHandler);

        static HtmlLoader()
        {
            _httpClient.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
        }

        public static async Task<string> GetPageByPageNumAndPurchase(int num, IParserSettings purchaseSettings)
        {
            //кодируем словj, по которому идёт выборка закупок
            //encode the name by which the purchases are selected
            var decodeName = HttpUtility.UrlEncode(purchaseSettings.PurchaseName);

            //вставляем в строку запроса актуальные данные о: наименорвании закупки и номера страницы
            //insert the actual data about: purchase name and page number into the query string 
            var currentUrl = purchaseSettings.BaseUrl.Replace("{purchaseName}", decodeName).Replace("{number}", num.ToString());
            var receeivedInfo = String.Empty;

            try
            {
                var response = await _httpClient.GetAsync(currentUrl, HttpCompletionOption.ResponseHeadersRead);

                if ( ! (response != null && response.StatusCode == HttpStatusCode.OK))
                {
                    throw new HttpRequestError($"Failed to retrieve page data. Error: {response.StatusCode}");
                }

                //get page code
                receeivedInfo = await response.Content.ReadAsStringAsync();
            }

            catch(HttpRequestError ex)
            {
                Console.WriteLine(ex.Message);
            }

            return receeivedInfo;
        }
    }
}
