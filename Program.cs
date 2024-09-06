using PurchaseParser.Parsers;
using PurchaseParser.Parsers.Purchases;
using System.Text;
using PurchaseParser.Parsers.Interfaces;

public class Program
{
    private static async Task Main(string[] args)
    {
        var parsedPurchaseList = new List<Cards>();

        //Read page numbers and purchase name from the console
        ReadInputSettings readInputSettings = new ReadInputSettings();
        readInputSettings.ReadInputQueryPerams();

        ParserWorker<Cards> parser = new ParserWorker<Cards>(new Purchase_Parser());

        parser.OnNewData += Parser_OnNewData;
        parser.OnCompleted += Parser_OnComplete;

        IParserSettings settings = new PurchaseSettings(readInputSettings.FirstPageNum, readInputSettings.LastPageNum, readInputSettings.PurchaseName);
        parser.ParserSettings = settings;
        await parser.Start();

        void Parser_OnNewData(Cards arg2)
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

        Console.WriteLine($"Total number of pages found: {parsedPurchaseList.Count}\n\n");
        Console.WriteLine("PAGE\n");

        foreach (Cards page in parsedPurchaseList)
        {
            Console.OutputEncoding = Encoding.UTF8;

            foreach (Card element in page)
            {
                Console.WriteLine("Card");

                foreach (var prop in typeof(Card).GetProperties())
                    Console.WriteLine($"{prop.Name}: {prop.GetValue(element)}");

                Console.WriteLine("\n");
            }

            Console.WriteLine("\n\n");
        }
    }
}