using PurchaseParser.Parsers.Interfaces;

 namespace PurchaseParser.Parsers.Purchases
{
    public class PurchaseSettings : IParserSettings
    {
        //подставляем значения, считанные с консоли
        // substitute the values read from the console
        public PurchaseSettings(int startPoint, int endPoint, string purchaseName)
        {
            ////первая страница
            ////first page
            //StartPoint = startPoint;
            ////последняя страница(включительно)
            ////last page(inclusive)
            //EndPoint = endPoint;
            ////название закупки, по которой производится поиск
            ////name of the purchase to be searched for
            //PurchaseName = purchaseName;
        }

        //в позже значения в скобках {} будут заменены на актуальные данные
        //later the values in brackets {} will be replaced by the actual data
        public string BaseUrl { get; set; } = "https://zakupki.gov.ru/epz/order/extendedsearch/results.html?searchString={purchaseName}&morphology=on&search-filter=%D0%94%D0%B0%D1%82%D0%B5+%D1%80%D0%B0%D0%B7%D0%BC%D0%B5%D1%89%D0%B5%D0%BD%D0%B8%D1%8F&pageNumber={number}&sortDirection=false&recordsPerPage=_10&showLotsInfoHidden=false&sortBy=UPDATE_DATE&fz44=on&fz223=on&af=on&ca=on&pc=on&pa=on&currencyIdGeneral=-1";
        public string PurchaseName { get; set; } = "труба";
        public int StartPoint { get; set; } = 1;
        public int EndPoint { get; set; } = 2;
    }
}
 