using System.Net;
using System.Web;
using PurchaseParser.Parsers.Exeptions;
using PurchaseParser.Parsers.Interfaces;
using Microsoft.Extensions.Http.Resilience;
using Polly;


namespace PurchaseParser.Parsers
{
    static class HtmlLoader
    {
        static readonly HttpClient _client;

        static HtmlLoader()
        {
            var retryPipeline = new ResiliencePipelineBuilder<HttpResponseMessage>()
                .AddRetry(new HttpRetryStrategyOptions
            {
                BackoffType = DelayBackoffType.Exponential,
                MaxRetryAttempts = 3
            })
            .Build();

            var socketHandler = new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(15)
            };

            var resilienceHandler = new ResilienceHandler(retryPipeline)
            {
                InnerHandler = socketHandler,
            };

            _client = new HttpClient(resilienceHandler);
            _client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
        }

        public static async Task<string> GetSourceByNumAndName(int num, IParserSettings purchaseSettings)
        {
            //кодируем слова, по которому идёт выборка закупок
            //encode the name by which the purchases are selected
            var decodeName = HttpUtility.UrlEncode(purchaseSettings.PurchaseName);

            //вставляем в строку запроса актуальные данные о: наименорвании закупки и номера страницы
            //insert the actual data about: purchase name and page number into the query string 
            var currentUrl = purchaseSettings.BaseUrl.Replace("{purchaseName}", decodeName).Replace("{number}", num.ToString());

            var response = await _client.GetAsync(currentUrl, HttpCompletionOption.ResponseHeadersRead);
            var source = String.Empty;

            try
            {
                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {
                    //get page code
                    source = await response.Content.ReadAsStringAsync();
                    return source;
                }

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
