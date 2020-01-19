namespace GSMdtTools
{
    class GSStringToken : IGSToken
    {
        public string Content { get; }

        public GSStringToken(string content)
        {
            Content = content;
        }

        public string Type()
        {
            return "GSString";
        }
    }
}
