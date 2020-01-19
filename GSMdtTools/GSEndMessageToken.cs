namespace GSMdtTools
{
    class GSEndMessageToken : IGSToken
    {
        public bool messageEnd = true;

        public string Type()
        {
            return "GSEndMessage";
        }
    }
}
