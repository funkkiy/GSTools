namespace GSMdtTools
{
    class GSMessageCountToken : IGSToken
    {
        public ushort MessageCount { get; }

        public GSMessageCountToken(ushort messageCount)
        {
            MessageCount = messageCount;
        }

        public string Type()
        {
            return "GSMessageCount";
        }
    }
}
