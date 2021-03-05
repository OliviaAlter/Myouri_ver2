namespace AnotherMyouri.DatabaseEntities.Entities
{
    public class Server
    {
        public ulong ServerId { get; set; }
        public string Prefix { get; set; }
        public ulong MessageLogChannel { get; set; }
        public ulong EventLogChannel { get; set; }
        public ulong WelcomeChannel { get; set; }
        public ulong LeaveChannel { get; set; }
        public ulong UserUpdateChannel { get; set; }
        public ulong WelcomeUrl { get; set; }
        public string WelcomeMessage { get; set; }
        public string LeaveMessage { get; set; }
        public bool InviteToggle { get; set; }
        public bool RoleMentionToggle { get; set; }
        public bool UserMentionToggle { get; set; }
        public int warningAmount { get; set; }
        public ulong baseVoiceChannelId { get; set; }
        public string baseVoiceChannelName { get; set; }
        public string baseTempVoiceChannelName { get; set; }
        public ulong CategoryId { get; set; }

    }
}