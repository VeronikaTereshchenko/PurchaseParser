using System.Collections;

namespace PurchaseParser.Parsers
{
    public class Cards
    {
        public List<Card> CardsList { get; set; }

        public Cards()
        {
            CardsList = new List<Card>();
        }

        public IEnumerator GetEnumerator() => new CardsEnumerator(CardsList);

        public void Add(Card card)
        {
            CardsList.Add(card);
        }

        public int Count => CardsList.Count;
    }
}
