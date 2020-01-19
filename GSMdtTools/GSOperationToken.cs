namespace GSMdtTools
{
    class GSOperationToken : IGSToken
    {
        public string Name { get; }
        public ushort Opcode { get; }
        public ushort[] Args { get; }

        public GSOperationToken(string name, ushort opcode, ushort[] args)
        {
            Opcode = opcode;
            Args = args;
            Name = name;
        }

        public string Type()
        {
            return "GSOperation";
        }
    }
}
