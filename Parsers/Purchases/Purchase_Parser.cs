using AngleSharp.Html.Dom;
using PurchaseParser.Parsers.Interfaces;
using System.Text.RegularExpressions;


namespace PurchaseParser.Parsers.Purchases
{
    public class Purchase_Parser : IParser<Cards>
    {
        private Card CurrentCard { get; set; }
        private Dictionary<string, System.Reflection.PropertyInfo> ClassPropertyDictionary;

        public Purchase_Parser()
        {
            var cardProp = typeof(Card).GetProperties();

            ClassPropertyDictionary = new Dictionary<string, System.Reflection.PropertyInfo> ()
            { 
                //names of classes in the page code, by which we search for the required information
                //law
                { "col-9 p-0 registry-entry__header-top__title text-truncate", cardProp[0] },
                //number
                { "registry-entry__header-mid__number", cardProp[1] },
                //purchaseObject
                { "registry-entry__body-value", cardProp[2] },
                //Organization
                { "registry-entry__body-href", cardProp[3] },
                //StartPrice
                { "price-block__value", cardProp[4] }
            };
        }

        public Cards Parse(IHtmlDocument document)
        {
            //ищем карточку (карточка хранит инф. об одном объекте, полученному после поиска)
            //search for a card
            var items = document.QuerySelectorAll("div").Where(item => item.ClassName != null
                                                                //из всего кода страницы выбираются только те классы, которые
                                                                //хранят карточки объектов
                                                                //from the whole page code only those classes are selected that
                                                                //store cards 
                                                                && item.ClassName == "row no-gutters registry-entry__form mr-0");
            Cards cards = new Cards();

            foreach (var item in items)
            {
                CurrentCard = new Card();

                //находим классы из HtmlClassesList в коде странице
                //для извлечения необходимой инф. из карточки
                //find classes from HtmlClassesList in the page code
                //to extract the necessary information from the card
                FindChildren(item);

                //If a card contains a filled price field, it means that it is not empty
                if (CurrentCard.StartPrice != null)
                {
                    cards.Add(CurrentCard);
                }
            }

            return cards;
        }

        private void FindChildren(AngleSharp.Dom.IElement item)
        {
            //извлекаем вложенные теги
            //extract nested tags
            var childrenNodes = item.Children;
            var className = item.ClassName;

            //е-и нашли класс, в котором лежит нужная инф., то забираем её
            //if we have found a class that contains the required information, then we take it
            if (className != null && ClassPropertyDictionary.ContainsKey(className))
            {
                var elementText = item.TextContent;
                elementText = elementText.Trim(new char[] { ' ', '\n' });
                elementText = Regex.Replace(elementText, @"[\r\n\t]", " ");
                elementText = Regex.Replace(elementText, @"\s+", " ");

                var cardProperty = ClassPropertyDictionary[className];
                cardProperty.SetValue(CurrentCard, elementText);
            }
            //иначе в каждой вложенной группе тегов ищем нужные нам классы
            //т.е. доходим до самого конца вложений
            //otherwise, in each nested group of tags we look for the classes we need
            //that is, we go to the very end of the attachments
            else
            {
                foreach (var child in childrenNodes)
                {
                    FindChildren(child);
                }
            }
        }
    }
}

