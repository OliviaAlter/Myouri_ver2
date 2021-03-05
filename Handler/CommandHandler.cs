using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using AnotherMyouri.DatabaseEntities.EntitiesConfig;
using AnotherMyouri.Extensions;
using Discord;
using Discord.Addons.Hosting;
using Discord.Commands;
using Discord.WebSocket;

namespace AnotherMyouri.Handler
{
    public class CommandHandler : InitializedService
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commandService;
        private readonly IServiceProvider _service;
        private readonly Servers _servers;
        private readonly AutoVoiceChannels _autoVoice;

        public CommandHandler(DiscordSocketClient client, CommandService commandService,
            IServiceProvider service, Servers servers, AutoVoiceChannels autoVoice)
        {
            _client = client;
            _commandService = commandService;
            _service = service;
            _servers = servers;
            _autoVoice = autoVoice;
        }

        public override async Task InitializeAsync(CancellationToken cancellationToken)
        {
            _client.MessageReceived += OnMessageReceived;
            _client.JoinedGuild += OnGuildJoined;
            _client.Ready += OnReady;
            _client.UserVoiceStateUpdated += OnVoiceStateUpdated;

            
            
            _commandService.CommandExecuted += OnCommandExecuted;
            
            /*
            var autoJoinVoice = new Task(async () => await AutoVoiceHandler());
            autoJoinVoice.Start();
            */
            await _client.GetRecommendedShardCountAsync();
            
            await _commandService.AddModulesAsync(Assembly.GetEntryAssembly(), _service);
        }

        private async Task AutoVoiceHandler()
        {
            await Task.Delay(1 * 30 * 10000);
            await AutoVoiceHandler();
        }

        private async Task OnVoiceStateUpdated(SocketUser user, SocketVoiceState voiceStateBefore,
            SocketVoiceState voiceStateAfter)
        {
            
            var guildId = await _servers.GetUserLogChannel(((SocketGuildUser) user).Guild.Id);
            
            var baseName = await _autoVoice.GetBaseVoiceChannelNameAsync(((SocketGuildUser) user).Guild.Id);
            var tempBaseName = await _autoVoice.GetBaseTempChannelNameAsync(((SocketGuildUser) user).Guild.Id);
            var baseChannelId = await _autoVoice.GetBaseVoiceChannelIdAsync(((SocketGuildUser) user).Guild.Id);
            
            var tempName = string.IsNullOrEmpty(await _autoVoice.GetBaseTempChannelNameAsync(((SocketGuildUser) user).Guild.Id))
                ? baseName
                : tempBaseName;
          
            if (!(_client.GetChannel(guildId) is ITextChannel messageChannel)) return;
            if (!Equals(voiceStateBefore.VoiceChannel, voiceStateAfter.VoiceChannel) &&
                voiceStateAfter.VoiceChannel != null && voiceStateBefore.VoiceChannel == null)
            {
                if (voiceStateAfter.VoiceChannel.Id == baseChannelId)
                {
                    var channelCreated = await voiceStateAfter.VoiceChannel.Guild.CreateVoiceChannelAsync(tempName);
                    await channelCreated.ModifyAsync(x =>
                    {
                        x.CategoryId= _autoVoice.GetBaseCategoryAsync(((SocketGuildUser) user).Guild.Id).Result;
                    });
                    await _autoVoice.AddTempChannelAsync(((SocketGuildUser) user).Guild.Id, baseChannelId, channelCreated.Id, tempName);
                    await messageChannel.SendMessageAsync(channelCreated.Id.ToString());
                    var result = _autoVoice.AddTempChannelAsync(((SocketGuildUser) user).Guild.Id, baseChannelId, channelCreated.Id, tempName).IsCompleted;
                    await messageChannel.SendMessageAsync($"Created another temp channel + Result : {result}");
                }
                Console.WriteLine(voiceStateAfter.VoiceChannel.Id == baseChannelId);
                Console.WriteLine($"{voiceStateAfter.VoiceChannel.Id} == {baseChannelId}");

                await EventExtension.UserVoiceJoined(user, voiceStateAfter.VoiceChannel, messageChannel);
            }
            else switch (Equals(voiceStateAfter.VoiceChannel, voiceStateBefore.VoiceChannel))
            {
                case false when voiceStateBefore.VoiceChannel != null && voiceStateAfter.VoiceChannel == null:
                {
                    await EventExtension
                        .UserVoiceLeft(user, voiceStateBefore.VoiceChannel, messageChannel);
                }
                    break;
                case false when voiceStateBefore.VoiceChannel != null && voiceStateAfter.VoiceChannel != null:
                {
                    if (voiceStateAfter.VoiceChannel.Id == baseChannelId)
                    {                    
                        var channelCreated = await voiceStateAfter.VoiceChannel.Guild.CreateVoiceChannelAsync(tempName);
                        await channelCreated.ModifyAsync(x =>
                        {
                            x.CategoryId= _autoVoice.GetBaseCategoryAsync(((SocketGuildUser) user).Guild.Id).Result;
                        });
                        await _autoVoice.AddTempChannelAsync(((SocketGuildUser) user).Guild.Id, baseChannelId, channelCreated.Id, tempName);
                        await messageChannel.SendMessageAsync(channelCreated.Id.ToString());
                        var result = _autoVoice.AddTempChannelAsync(((SocketGuildUser) user).Guild.Id, baseChannelId, channelCreated.Id, tempName).IsCompleted;
                        await messageChannel.SendMessageAsync($"Created another temp channel + Result : {result}");
                    }
                   
                    Console.WriteLine(voiceStateAfter.VoiceChannel.Id == baseChannelId);
                    Console.WriteLine($"{voiceStateAfter.VoiceChannel.Id} == {baseChannelId}");

                    await EventExtension.UserVoicejumped(user, voiceStateBefore.VoiceChannel, voiceStateAfter.VoiceChannel, messageChannel);
                    break;
                }
            }
        }

        
        private async Task OnReady()
        {
            var gameActivity = new[]
            {
                "with Olivia",
                "with knife",
                "nothing",
            };
            await _client.SetStatusAsync(UserStatus.Idle);
            await _client.SetGameAsync($"{gameActivity[new Random().Next(gameActivity.Length - 1)]}");
        }


        private async Task OnGuildJoined(SocketGuild arg)
        {
            await arg.DefaultChannel.SendMessageAsync(
                "Hello, Ｏｌｉｖｉａ is here. " +
                "Thanks for inviting me to your Server!" +
                "For my command please use * as a prefix");
        }

        private async Task OnCommandExecuted(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            
            if (command.IsSpecified && !result.IsSuccess)
            {
                if (!(context.Channel is ISocketMessageChannel errorChannel)) return;
                switch (result.Error)
                {
                    case CommandError.UnknownCommand:
                        await errorChannel.SendErrorAsync("Error",
                            $"{result.ErrorReason}");
                        break;
                    case CommandError.BadArgCount:
                        await errorChannel.SendErrorAsync("Error",
                            $"{result.ErrorReason}");
                        break;
                    case CommandError.Exception:
                        await errorChannel.SendErrorAsync("Error",
                            $"{result.ErrorReason}");
                        break;
                    case CommandError.MultipleMatches:
                        await errorChannel.SendErrorAsync("Error",
                            $"{result.ErrorReason}");
                        break;
                    case CommandError.ObjectNotFound:
                        await errorChannel.SendErrorAsync("Error",
                            $"{result.ErrorReason}");
                        break;
                    case CommandError.ParseFailed:
                        await errorChannel.SendErrorAsync("Error",
                            $"{result.ErrorReason}");
                        break;
                    case CommandError.UnmetPrecondition:
                        await errorChannel.SendErrorAsync("Error",
                            $"{result.ErrorReason}");
                        break;
                    case CommandError.Unsuccessful:
                        await errorChannel.SendErrorAsync("Error", 
                            $"{result.ErrorReason}");
                        break;
                    case null:
                        await errorChannel.SendErrorAsync("Error",
                            $"{result.ErrorReason}");
                        break;
                    default:
                        await errorChannel.SendErrorAsync("Error",
                            $"{result.ErrorReason}");
                        break;
                }
            }
        }
        private async Task OnMessageReceived(SocketMessage arg)
        {
            if (!(arg is SocketUserMessage message)) return;

            /*
            if (!((SocketGuildChannel) message.Channel)
                .Guild.GetUser(message.Author.UserId).GuildPermissions.Administrator)
            {   
                if (message.Content.ToLower().Contains("https://discord.gg/"))
                {
                    await message.DeleteAsync();
                    await message.Channel.SendErrorModerationAsync("Invalid invite link",
                        $"{message.Author.Mention} has posted another discord invite link without permission",
                        message.Author);
                }
                
                if (message.MentionedEveryone)
                {
                    await message.DeleteAsync();
                    await message.Channel.SendErrorAsync("Warning",
                        $"{message.Author.Mention} has mentioned everyone in channel <#{message.Channel.UserId}>, shame on them!");
                }

                if (message.MentionedRoles.Count >= 4)
                {
                    await message.DeleteAsync();
                    await message.Channel.SendErrorAsync("Warning",
                        $"{message.Author.Mention} has mentioned more than 4 roles in channel <#{message.Channel.UserId}>, shame on them!");
                }

                if (message.MentionedUsers.Count >= 4)
                {
                    await message.DeleteAsync();
                    await message.Channel.SendErrorAsync("Warning",
                        $"{message.Author.Mention} has mentioned more than 4 people in channel <#{message.Channel.UserId}>, shame on them!");
                }
            }
            */
            
            var argPos = 0;
            var prefix = await _servers.GetGuildPrefix(((SocketGuildChannel) message.Channel).Guild.Id) ?? "*";
            if (!message.HasStringPrefix(prefix, ref argPos) &&
                !message.HasMentionPrefix(_client.CurrentUser, ref argPos)) return;

            var context = new SocketCommandContext(_client, message);
            await _commandService.ExecuteAsync(context, argPos, _service);
        }
        
        
    }
}