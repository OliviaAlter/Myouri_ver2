namespace AnotherMyouri.DatabaseEntities.Entities
{
    public class User
    {
        public ulong userId { get; set; }
        public string UserName { get; set; }
        public ulong Exp { get; set; }
        public ulong Level { get; set; }
        public ulong SteamId { get; set; }
        public long OpenDotaId { get; set; }
    }
}