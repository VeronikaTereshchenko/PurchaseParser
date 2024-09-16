namespace PurchaseParser.Parsers.Interfaces
{
    interface IParserSettings
    {
        string BaseUrl { get; set; }
        string PurchaseName { get; set; }
        int FirstPageNum { get; set; }
        int LastPageNum { get; set; }
    }
}
