using ПарсерЗакупки.Core;
using ПарсерЗакупки.Core.Purchases;
using System.Text;
using ПарсерЗакупки.Core.Interfaces;

internal class Program
{
    private static void Main(string[] args)
    {
        var purchaseList = new List<Element[]>();
        var cardsAmount = 0;
        //Read page numbers and purchase name from the console
        ReadInputSettings readInputSettings = new ReadInputSettings();
        readInputSettings.ReadInputQueryPerams();

        ParserWorker<Element[]> parser = new ParserWorker<Element[]>(new PurchaseParser());

        parser.OnNewData += Parser_OnNewData;
        parser.OnCompleted += Parser_OnComplete;

        IParserSettings settings = new PurchaseSettings(readInputSettings.FirstPageNum, readInputSettings.LastPageNum, readInputSettings.PurchaseName);
        parser.ParserSettings = settings;
        parser.Start();

        void Parser_OnNewData(object arg1, Element[] arg2)
        {
            //добавление данных с карточек на одной странице
            //add data from cards on one page
            purchaseList.Add(arg2);
            //counting the total number of cards found
            cardsAmount += arg2.Length;
        }

        void Parser_OnComplete(object obj)
        {
            //поиск по страницам завершён
            // page search complete
            Console.WriteLine("All works done!!!");
        }

        Console.WriteLine($"Total number of pages found: {purchaseList.Count}\n");
        Console.WriteLine($"Total number of cards found: {cardsAmount}\n");
        Console.WriteLine("Page\n");

        foreach (Element[] page in purchaseList)
        {
            Console.OutputEncoding = Encoding.UTF8;

            foreach (Element element in page)
            {
                Console.WriteLine("Element");

                for(int i = 0; i < element.Info.Length; i++)
                {
                    switch (i)
                    {
                        case 0:
                            Console.WriteLine($"Закон: {element.Info[i]}"); break;
                        case 1:
                            Console.WriteLine($"Номер закупки: {element.Info[i]}"); break;
                        case 2:
                            Console.WriteLine($"Объект закупки: {element.Info[i]}"); break;
                        case 3:
                            Console.WriteLine($"Организация: {element.Info[i]}"); break;
                        case 4:
                            Console.WriteLine($"Начальная цена: {element.Info[i]}"); break;
                    }
                }

                Console.WriteLine("\n");
            }

            Console.WriteLine("\n\n");
        }
    }
}