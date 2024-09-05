namespace ПарсерЗакупки.Core.Exeptions
{
    internal class HttpRequestError : SystemException
    {
        private string _message;
        public HttpRequestError(string message) : base(message) { }

        public void Message()
        {
            Console.WriteLine(_message);
        }
    }
}
