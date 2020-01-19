namespace GSMdtTools
{
    class GSOperation
    {
        public string Name { get; }
        public ushort ArgCount { get; }

        public GSOperation(string name, ushort argCount)
        {
            Name = name;
            ArgCount = argCount;
        }
    }
}
