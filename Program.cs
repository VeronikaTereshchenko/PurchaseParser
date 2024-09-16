using PurchaseParser.Parsers;
using PurchaseParser.Parsers.Purchases;
using System.Text;
using PurchaseParser.Parsers.Interfaces;

public class Program
{
    private static async Task Main(string[] args)
    {
        var parsedPurchaseList = new List<List<Card>>();
        var parser = new ParserWorker<List<Card>>(new Purchase_Parser());
        IParserSettings settings = new PurchaseSettings("труба", 1, 1);
        parser.ParserSettings = settings;

        parser.OnNewData += Parser_OnNewData;
        parser.OnCompleted += Parser_OnComplete;

        await parser.Start();

        void Parser_OnNewData(List<Card> arg2)
        {
            //добавление данных с карточек на одной странице
            //add data from cards on one page
            parsedPurchaseList.Add(arg2);
        }

        void Parser_OnComplete()
        {
            //поиск по страницам завершён
            // page search complete
            Console.WriteLine("All works done!!!");
        }

        if(parsedPurchaseList != null)
        {
            foreach(List<Card> list in parsedPurchaseList)
            {
                foreach (Card card in list)
                {
                    Console.OutputEncoding = Encoding.UTF8;
                    Console.WriteLine("Card");

                    foreach (var prop in typeof(Card).GetProperties())
                        Console.WriteLine($"{prop.Name}: {prop.GetValue(card)}");

                    Console.WriteLine("\n");
                }
            }
        }
    }
}