using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using AnotherMyouri.DatabaseEntities.DiscordBotContext;
using AnotherMyouri.DatabaseEntities.Entities;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;

namespace AnotherMyouri.DatabaseEntities.EntitiesConfig
{
    public class Servers
    {
        private static MyouriDbContext _context;
        
        public Servers(MyouriDbContext context)
        {
            _context = context;
        }

        #region Guild Prefix

        public async Task ModifyPrefix(ulong id, string prefix)
        {
            var server = await _context.Servers.FindAsync(id);
            if (server == null) 
                await _context.Servers.AddAsync(new Server{ ServerId = id, Prefix = prefix });
            else server.Prefix = prefix;
            await _context.SaveChangesAsync();
        }
        
        public async Task<string> GetGuildPrefix(ulong id)
        {
            var prefix = await _context.Servers.AsQueryable()
                .Where(x => x.ServerId == id)
                .Select(x => x.Prefix)
                .FirstOrDefaultAsync();
            return await Task.FromResult(prefix);
        }
        
        #endregion

        #region Server get/set
        
        public async Task<Server> GetOrCreateServer(ulong serverId)
        {
            var server = await _context.Servers.FindAsync(serverId);
            if (server != null) return await Task.FromResult(server);
            await _context.AddAsync(CreateNewServer(serverId));
            _context.SaveChangesAsync();
            return await Task.FromResult(server);
        }

        private static Server CreateNewServer(ulong id)
        {
            var newServer = new Server
            {
                ServerId = id,
                Prefix = "*",
                MessageLogChannel = 0,
                EventLogChannel = 0,
                WelcomeChannel = 0,
                LeaveChannel = 0,
                UserUpdateChannel = 0,
                WelcomeUrl = 0,
                WelcomeMessage = "Hello ! Thanks for joining. Please check out the rules first then enjoy your stay.",
                LeaveMessage = "Goodbye!",
                InviteToggle = false,
                RoleMentionToggle = false,
                UserMentionToggle = false,
                warningAmount = 3,
                baseVoiceChannelId = 0,
                baseVoiceChannelName = "",
                baseTempVoiceChannelName = ""
            };
            return newServer;
        }
        
        #endregion
        
        
        #region User log channel 
        public async Task<ulong> GetUserLogChannel(ulong id)
        {
            var channelId = await _context.Servers
                .AsQueryable()
                .Where(x => x.ServerId == id)
                .Select(x => x.UserUpdateChannel)
                .FirstOrDefaultAsync();
            return await Task.FromResult(channelId);
        }
      
        public async Task SetUserLogChannel(ulong id, ulong channelId)
        {
            var server = await _context.Servers
                .FindAsync(id);
            if (server == null) _context.Add(new Server {ServerId = id, UserUpdateChannel = channelId});
            await _context.SaveChangesAsync();
        }

        public async Task ModifyUserLogChannel(ulong id, ulong channelId)
        {
            var server = await _context.Servers
                .FindAsync(id);
            if (server == null)
                _context.Add(new Server {ServerId = id, UserUpdateChannel = channelId});
            else
                server.UserUpdateChannel = channelId;
            await _context.SaveChangesAsync();
        }

        public async Task RemoveUserLogChannel(ulong id)
        {
            var channel = await _context.Servers
                .FindAsync(id);
            channel.UserUpdateChannel = 0;
            await _context.SaveChangesAsync();
        }
        #endregion

      
    }
}