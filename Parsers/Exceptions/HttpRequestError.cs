namespace PurchaseParser.Parsers.Exeptions
{
    internal class HttpRequestError : SystemException
    {
        public HttpRequestError(string message) : base(message) { }
    }
}
