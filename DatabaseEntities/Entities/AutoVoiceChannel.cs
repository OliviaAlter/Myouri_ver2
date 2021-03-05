using JikanDotNet;

namespace AnotherMyouri.DatabaseEntities.Entities
{
    public class AutoVoice
    {
        public ulong id { get; set; }
        public ulong ServerId { get; set; }
        public ulong baseVoiceChannelId { get; set; }
        public string baseVoiceChannelName { get; init; }
        public string baseTempVoiceChannelName { get; set; }
        public string TempVoiceChannelName { get; set; }
        public ulong TempVoiceChannelId { get; set; }
        public ulong CategoryId { get; set; }
        public ulong userIdCreate { get; set; }
    }
}