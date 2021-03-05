using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AnotherMyouri.DatabaseEntities.DiscordBotContext;
using AnotherMyouri.DatabaseEntities.Entities;
using Microsoft.EntityFrameworkCore;

namespace AnotherMyouri.DatabaseEntities.EntitiesConfig
{
    public class AutoVoiceChannels
    {
        private readonly MyouriDbContext _context;

        public AutoVoiceChannels(MyouriDbContext context)
        {
            _context = context;
        }

        public async Task<ulong> GetBaseCategoryAsync(ulong guildId)
        {
            var cChannel = await _context.Servers
                .AsQueryable()
                .Where(x => x.ServerId == guildId)
                .Select(x => x.CategoryId)
                .FirstOrDefaultAsync();
            if (cChannel == 0)
            {
                return 0;
            }
            return await Task.FromResult(cChannel);
        }
        
        public async Task AddBaseVoiceAsync(ulong guildId, ulong categoryId, ulong channelId, string channelName)
        {
            var server = await _context.Servers.FindAsync(guildId);
            if (server == null)
            {
                await _context.AddAsync(new Server
                {
                    ServerId = guildId,
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
                    baseVoiceChannelId = channelId,
                    baseVoiceChannelName = channelName,
                    baseTempVoiceChannelName = "",
                    CategoryId = categoryId
                });
            }
            else
            {
                server.baseVoiceChannelId = channelId;
                server.baseVoiceChannelName = channelName;
                server.CategoryId = categoryId;
            }
            await _context.SaveChangesAsync();
        }

        public async Task AddBaseTempNameVoiceAsync(ulong guildId, string channelName, ulong baseCategoryId)
        {
            var guild = await _context.Servers
                .FindAsync(guildId);
            if (guild == null)
            {
                await _context.Servers.AddAsync(new Server
                {
                    ServerId = guildId,
                    baseTempVoiceChannelName = channelName,
                    CategoryId = baseCategoryId
                });
            }
            else
            {
                guild.CategoryId = baseCategoryId;
                guild.baseTempVoiceChannelName = channelName;
            }
            await _context.SaveChangesAsync();
        }
        
        public async Task AddTempChannelAsync(ulong guildId, ulong baseChannelId, ulong tempId, string tempName)
        {
            var baseChannel = await _context.AutoVoices.FindAsync(guildId);
            if (baseChannel == null)
                _context.Add(new AutoVoice {ServerId = guildId, baseVoiceChannelId = baseChannelId, TempVoiceChannelId = tempId});
            else
            {
                baseChannel.TempVoiceChannelId = tempId;
                baseChannel.TempVoiceChannelName = tempName;
            }
            await _context.SaveChangesAsync();
        }
        
        public async Task<ulong> GetBaseVoiceChannelIdAsync(ulong guildId)
        {
            var baseChannelId = await _context.Servers
                .AsQueryable()
                .Where(x => x.ServerId == guildId)
                .Select(x => x.baseVoiceChannelId)
                .FirstOrDefaultAsync();
            if (baseChannelId == 0)
            {
                return 0;
            }
            return await Task.FromResult(baseChannelId);
        }
        
        public async Task<string> GetBaseVoiceChannelNameAsyncTest(ulong guildId)
        {
            var baseChannel = await _context.Servers
                .AsQueryable()
                .Where(x => x.ServerId == guildId)
                .Select(x => x.baseVoiceChannelName)
                .FirstOrDefaultAsync();
            if (baseChannel == null)
            {
                return "none";
            }
            return await Task.FromResult(baseChannel);
        }
        
        public async Task<string> GetBaseVoiceChannelNameAsync(ulong guildId)
        {
            var baseChannel = await _context.Servers
                .AsQueryable()
                .Where(x => x.ServerId == guildId)
                .Select(x => x.baseVoiceChannelName)
                .FirstOrDefaultAsync();
            return await Task.FromResult(baseChannel);
        }

        public async Task<List<ulong>> GetServerBaseVoiceAsync(ulong guildId)
        {
            var baseChannelId = await _context.Servers
                .AsQueryable()
                .Where(x => x.ServerId == guildId)
                .Select(x => x.baseVoiceChannelId)
                .ToListAsync();
            return await Task.FromResult(baseChannelId);
        }
        
        public async Task<List<ulong>> GetServerBaseVoiceCategoryAsync(ulong guildId)
        {
            var baseChannelId = await _context.Servers
                .AsQueryable()
                .Where(x => x.ServerId == guildId)
                .Select(x => x.CategoryId)
                .ToListAsync();
            return await Task.FromResult(baseChannelId);
        }

        public async Task ModifyBaseChannelAsync(ulong guildId, ulong baseId, string channelName)
        {
            var server = await _context.AutoVoices
                .FindAsync(guildId);
            if (server == null)
                _context.Add(new AutoVoice {ServerId = guildId, baseVoiceChannelId = baseId, baseVoiceChannelName = channelName});
            else
            {
                server.baseVoiceChannelId = baseId;
            }
            await _context.SaveChangesAsync();
        }
        
        public async Task RemoveBaseVoiceChannelAsync(ulong guildId)
        {
            var server = await _context.Servers.FindAsync(guildId);
            if (server == null)
            {
                await _context.AddAsync(new Server
                {
                    ServerId = guildId,
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
                });
            }
            else
            {
                server.baseVoiceChannelId = 0;
            }
            await _context.SaveChangesAsync();
        }

        public async Task<string> GetBaseTempChannelNameAsync(ulong guildId)
        {
            var tempChannelName = await _context.Servers
                .AsQueryable()
                .Where(x => x.ServerId == guildId)
                .Select(x => x.baseTempVoiceChannelName)
                .FirstOrDefaultAsync();
            return await Task.FromResult(tempChannelName);
        }

    }
}