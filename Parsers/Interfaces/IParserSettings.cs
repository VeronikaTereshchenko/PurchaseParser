namespace PurchaseParser.Parsers.Interfaces
{
    interface IParserSettings
    {
        string BaseUrl { get; set; }
        string PurchaseName { get; set; }
        int StartPoint { get; set; }
        int EndPoint { get; set; }
    }
}
