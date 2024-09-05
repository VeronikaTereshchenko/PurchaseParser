using AngleSharp.Html.Dom;


namespace ПарсерЗакупки.Core.Interfaces
{
    interface IParser<T> where T : class
    {
        T Parse(IHtmlDocument document);
    }
}
