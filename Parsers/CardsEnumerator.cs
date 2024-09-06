using System.Collections;

namespace PurchaseParser.Parsers
{
    public class CardsEnumerator : IEnumerator
    {
        List<Card> _cards;
        int _position = -1;
        public CardsEnumerator(List<Card> cards) => _cards = cards;

        public object Current
        {
            get
            {
                if(_position == -1 || _position >= _cards.Count)
                    throw new ArgumentException();
                return _cards[_position];
            }
        }

        public bool MoveNext()
        {
            if(_position < _cards.Count - 1)
            {
                _position++; return true;
            }
            return false;
        }

        public void Reset() => _position = -1; 
    }
}
