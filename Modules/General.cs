using System;
using System.Linq;
using System.Threading.Tasks;
using AnotherMyouri.DatabaseEntities.EntitiesConfig;
using AnotherMyouri.Preconditions;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace AnotherMyouri.Modules
{
    [Summary(":sa:")]
    public class General : ModuleBase<ICommandContext>
    {
        private readonly DiscordSocketClient _client;
        private readonly EmbedBuilder _embed;
        private readonly Servers _servers;

        public General(DiscordSocketClient client
            , EmbedBuilder embed, Servers servers)
        {
            _client = client;
            _embed = embed;
            _servers = servers;
        }
        
        [Command("dm3T", RunMode = RunMode.Async)]
        [Cooldown(69)]
        [Name("Admin")]
        [RequireContext(ContextType.Guild)]
        public async Task Dm3T()
        {
            _embed.WithDescription("Dit me 3T");
            _embed.WithTimestamp(DateTime.Now);
            await ReplyAsync(embed: _embed.Build());
        }

        [Command("ulog", RunMode = RunMode.Async)]
        [Summary("set User log channel")]
        [RequireBotPermission(GuildPermission.ManageChannels)]
        public async Task SetUserLogChannel(SocketGuildChannel channel = null)
        {
            if (!(Context.Channel is SocketGuildChannel)) return;
            
            channel ??= (SocketGuildChannel) Context.Channel;
            var channelLog = await _servers.GetUserLogChannel(Context.Guild.Id);
            if (channelLog == 0)
                await _servers.SetUserLogChannel(Context.Guild.Id, channel.Id);
            else
                await _servers.ModifyUserLogChannel(Context.Guild.Id, channel.Id);
            await ReplyAsync($"Channel <#{channel.Id}> has been set as User log");
        }

        [Command("rulog", RunMode = RunMode.Async)]
        [Summary("remove User log channel")]
        [RequireBotPermission(GuildPermission.ManageChannels)]
        public async Task RemoveUserLogChannel(SocketGuildChannel channel = null)
        {
            if (!(Context.Channel is SocketGuildChannel)) return;

            channel ??= (SocketGuildChannel) Context.Channel;
            var channelLog = await _servers.GetUserLogChannel(Context.Guild.Id);
            if (channelLog == 0)
            {
                await ReplyAsync("This Server doesn't have any User log channel!");
                return;
            }

            if (channel.Id != channelLog)
            {
                await ReplyAsync("That channel is not set as User log channel");
            }
            else
            {
                await _servers.RemoveUserLogChannel(Context.Guild.Id);
                await ReplyAsync($"Removed channel <#{channel.Id}> as User log channel!");
            }
        }
       
    }
}