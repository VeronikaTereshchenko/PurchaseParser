
namespace PurchaseParser.Parsers
{
    //считываем с консоли имя товара по которому будет производиться поиск
    //и номера страниц, по которым будет осуществлён поиск
    //read the name of the product and the page numbers to be searched from the console
    public class ReadInputSettings
    {
        //номер первой страницы
        public int FirstPageNum { get; private set; }
        //номер послденей страницы
        public int LastPageNum { get; private set; }

        public string PurchaseName { get; private set; }

        //хранит текущее введённое число
        //stores the current entered number
        private int InputNum;

        public void ReadInputQueryPerams()
        {
            //просим ввести страницу с указанным порядком (номер первой/последней страницы)
            // ask to enter the page with the specified order (first/last page number)
            ReadNum("first");
            ReadNum("last");

            Console.Write($"Please state the purchase name: ");
            PurchaseName = Console.ReadLine();
        }

        public void ReadNum(string order)
        {
            Console.WriteLine($"Please state the number of {order} page");
            string input = Console.ReadLine();
            int pageNum;

            if(Int32.TryParse(input, out pageNum)) 
            {
                if(order == "first")
                {
                    FirstPageNum = pageNum;
                    return;
                }
                
                LastPageNum = pageNum;
            }

            else
            {
                //е-и было введено не число, то просим ввести значение заново
                //if the value entered is not a number, ask to enter the value again
                Console.WriteLine($"Invalid page number value\n");
                ReadNum(order);
            }
        }
    }
}
