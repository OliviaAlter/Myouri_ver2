using System.Threading.Tasks;
using AnotherMyouri.DatabaseEntities.EntitiesConfig;
using AnotherMyouri.Preconditions;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Discord.Addons.Interactive;

namespace AnotherMyouri.Modules
{
    public class AutoVoiceChannel : InteractiveBase<SocketCommandContext>
    {
        private readonly AutoVoiceChannels _autoVoice;

        public AutoVoiceChannel(AutoVoiceChannels autoVoice)
        {
            _autoVoice = autoVoice;
        }

        [Command("autovoice", RunMode = RunMode.Async)]
        [RequireBotPermission(GuildPermission.ManageChannels)]
        [RequireBotPermission(GuildPermission.MoveMembers)]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        [Cooldown(10)]
        public async Task AddAutoVoiceChannel([Remainder] string channel = null)
        {
            if (!(Context.Channel is SocketGuildChannel)) return;
            if (channel == null) return;
            
            var baseChannelCheck = await _autoVoice.GetBaseVoiceChannelIdAsync(Context.Guild.Id);
            var baseCategoryCheck = await _autoVoice.GetBaseCategoryAsync(Context.Guild.Id);
            var numberOfChannelCheck = await _autoVoice.GetServerBaseVoiceAsync(Context.Guild.Id);
            var numberOfCategoryCheck = await _autoVoice.GetServerBaseVoiceCategoryAsync(Context.Guild.Id);
            var baseChannelName = await _autoVoice.GetBaseVoiceChannelNameAsync(Context.Guild.Id);

            if (baseChannelCheck == 0 && baseCategoryCheck == 0 
            && numberOfCategoryCheck.Count == 0 && numberOfChannelCheck.Count == 0)
            {
                var category = await Context.Guild.CreateCategoryChannelAsync("Auto voice");
                
                var channelName = await Context.Guild.CreateVoiceChannelAsync(channel);
                await channelName.ModifyAsync(
                    prop => prop.CategoryId = category.Id);
                
                await _autoVoice.AddBaseVoiceAsync(Context.Guild.Id, category.Id, channelName.Id, channel);
                var isComplete = _autoVoice.AddBaseVoiceAsync(Context.Guild.Id, category.Id, channelName.Id, channel)
                    .IsCompleted;
                await ReplyAsync(isComplete.ToString());
                await ReplyAsync($"Name -> {channel}, ID -> {channelName.Id}");
                await ReplyAsync($"Unless you define your auto voice temp name, the default will be {channel} !");
                return;
            } 
            if (numberOfChannelCheck.Count >=1 && numberOfChannelCheck.Count > 0
                                                 && numberOfCategoryCheck.Count >=1 && numberOfCategoryCheck.Count > 0)
            {
                await ReplyAsync(
                    $"You already set channel {MentionUtils.MentionChannel(baseChannelCheck)}, Name : {baseChannelName} as base auto voice channel!" +
                    $"You can only have one base voice channel");
            }
        }

        [Command("get", RunMode = RunMode.Async)]
        public async Task Get()
        {
            var baseChannelId = await _autoVoice.GetBaseVoiceChannelIdAsync(Context.Guild.Id);
            await ReplyAsync(baseChannelId.ToString());
        }
        
        [Command("removevoice", RunMode = RunMode.Async)]
        [RequireBotPermission(GuildPermission.ManageChannels)]
        [RequireBotPermission(GuildPermission.MoveMembers)]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        [Cooldown(10)]
        public async Task RemoveBaseVoiceChannel()
        {
            if (!(Context.Channel is SocketGuildChannel)) return;

            var baseChannelName = await _autoVoice.GetBaseVoiceChannelNameAsync(Context.Guild.Id);
            if (baseChannelName == null)
            {
                await ReplyAsync("No base channel set !!!");
                return;
            }
            await _autoVoice.RemoveBaseVoiceChannelAsync(Context.Guild.Id);
            await ReplyAsync($"Successfully removed : {baseChannelName} from being the base channel!");
        }

        [Command("set")]
        public async Task SetTempChannelName([Remainder] string name)
        {
            var baseChannelId = await _autoVoice.GetBaseVoiceChannelIdAsync(Context.Guild.Id);
            var baseCategoryId = await _autoVoice.GetBaseCategoryAsync(Context.Guild.Id);
            await ReplyAsync(baseCategoryId.ToString());
            if (baseChannelId == 0)
            {
                await ReplyAsync("No voice channel as a base yet");
                return;
            }
            
            await _autoVoice.AddBaseTempNameVoiceAsync(Context.Guild.Id, name, baseCategoryId);
            var isCompleted = _autoVoice.AddBaseTempNameVoiceAsync(Context.Guild.Id, name, baseCategoryId).IsCompleted;
            await ReplyAsync(isCompleted.ToString());
        }
        
    }
}