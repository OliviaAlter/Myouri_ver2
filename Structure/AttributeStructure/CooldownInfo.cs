namespace AnotherMyouri.Structure
{
    public readonly struct CooldownInfo
    {
        private ulong UserId { get; }

        private int CommandHashCode { get; }

        public CooldownInfo(ulong userId, int commandHashCode)
        {
            UserId = userId;
            CommandHashCode = commandHashCode;
        }
    }
}