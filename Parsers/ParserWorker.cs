using AngleSharp.Html.Parser;
using PurchaseParser.Parsers.Interfaces;

namespace PurchaseParser.Parsers
{
    class ParserWorker<T> where T : class
    {
        IParser<T> _parser;
        IParserSettings _parserSettings;

        #region Properties
        public IParser<T> Parser
        {
            get { return _parser; }
            set { _parser = value; }
        }

        public IParserSettings ParserSettings
        {
            get { return _parserSettings; }
            set 
            { 
                _parserSettings = value;
            }
        }
        #endregion

        public event Action<T> OnNewData;
        public event Action OnCompleted;

        #region Constructors
        public ParserWorker(IParser<T> parser)
        {
            _parser = parser;
        }
        public ParserWorker(IParser<T> parser, IParserSettings settings) : this(parser)
        {
            _parserSettings = settings;
        }
        #endregion

        public async Task Start()
        {
            await Worker();
        }

        async Task Worker()
        {
            //проходимся по номерам страниц
            //walk through the page numbers
            for (int point = _parserSettings.StartPoint; point <= _parserSettings.EndPoint; point++)
            {
                var source = await HtmlLoader.GetSourceByNumAndName(point, _parserSettings);
                var domParser = new HtmlParser();
                var document = await domParser.ParseDocumentAsync(source);
                //достаём инф. из каждой карточки на странице по тегам и классам
                //retrieve information from each card on the page by tags and classes
                var result = _parser.Parse(document);

                //передаём извлечённую инф. из карточек в метод Main
                // pass the extracted information from cards to the Main method
                OnNewData?.Invoke(result);
            }

            OnCompleted?.Invoke();
        }
    }
}
