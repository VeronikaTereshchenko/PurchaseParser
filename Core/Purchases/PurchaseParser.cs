using AngleSharp.Html.Dom;
using ПарсерЗакупки.Core.Interfaces;
using System.Text.RegularExpressions;

namespace ПарсерЗакупки.Core.Purchases
{
    public class Element
    {
        public string[] Info { get; set; }
    }

    public class PurchaseParser : IParser<Element[]>
    {
        //содержит названия классов по которым можно извлечь инф. о
        //номере заказа, объекта закупки, заказчике ...
        //contains class names by which you can retrieve information about order number, purchase object, customer
        private List<string> HtmlClassesList { get; set; }
        //содержит инф. каждой карточки на странице
        //contains the information of each card on the page
        private List<Element> PageElementsList { get; set; }
        //содержит инф. текущей карточки
        //contains the current card information 
        private List<string> CurrentElementList { get; set; }

        public PurchaseParser()
        {
            HtmlClassesList = new List<string>()
            {
                "col-9 p-0 registry-entry__header-top__title text-truncate",
                "registry-entry__header-mid__number",
                "registry-entry__body-value",
                "registry-entry__body-href",
                "price-block__value"
            };
            //инфоромация каждой карточи лежит в массиве string[]
            //все карточки на странице лежат в List<string[]>
            //information of each card is in array string[]
            //all cards on the page are in List<string[]>
            PageElementsList = new List<Element>();
            //хранит информацию текущей карточке 
            //в нашей задаче это: номер, объект закупки, организация
            //stores information of the current card 
            //in our task it is: number, purchase object, organisation
            CurrentElementList = new List<string>();
        }

        public Element[] Parse(IHtmlDocument document)
        {
            //ищем карточку (карточка хранит инф. об одном объекте, полученному после поиска)
            //search for a card
            var items = document.QuerySelectorAll("div").Where(item => item.ClassName != null
                                                                //из всего кода страницы выбираются только те классы, которые
                                                                //хранят карточки объектов
                                                                //from the whole page code only those classes are selected that
                                                                //store cards 
                                                                && item.ClassName == "row no-gutters registry-entry__form mr-0");

            foreach ( var item in items )
            {
                //находим классы из HtmlClassesList в коде странице
                //для извлечения необходимой инф. из карточки
                //find classes from HtmlClassesList in the page code
                //to extract the necessary information from the card
                FindChildren(item);

                //инф. каждой карточки переносится в новый объект Element
                //info of each card is transferred to a new Element object
                if (CurrentElementList.Count > 0)
                {
                    var arr = CurrentElementList.ToArray();
                    Element newElement = new Element();
                    newElement.Info = arr;
                    //добавляем сформированную инф. из карточки в общий список
                    // add the generated information from the card to the general list
                    PageElementsList.Add(newElement);
                }

                //удаляем данные из текущей карточки для последующих карточек
                //delete data from the current card for subsequent cards
                CurrentElementList.Clear();
            }

            Element[] elements = PageElementsList.ToArray();
            PageElementsList.Clear();

            //отправляем массив, содержащий инф. по каждой из карточек на странице
            // send an array containing information for each card on the page
            return elements;
        }

        private void FindChildren(AngleSharp.Dom.IElement item)
        {
            //извлекаем вложенные теги
            //extract nested tags
            var childrenNodes = item.Children;
            var className = item.ClassName;

            //е-и нашли класс, в котором лежит нужная инф., то забираем её
            //if we have found a class that contains the required information, then we take it
            if (HtmlClassesList.Contains(className))
            {
                var elementText = item.TextContent;
                elementText = elementText.Trim(new char[] {' ', '\n'});
                elementText = Regex.Replace(elementText, @"[\r\n\t]", " ");
                CurrentElementList.Add(elementText);
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
