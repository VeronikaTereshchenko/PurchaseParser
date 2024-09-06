using AngleSharp.Html.Dom;


namespace PurchaseParser.Parsers.Interfaces
{
    interface IParser<T> where T : class
    {
        T Parse(IHtmlDocument document);
    }
}
