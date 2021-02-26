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
        private readonly DiscordShardedClient _client;
        private readonly CommandService _commandService;
        private readonly IServiceProvider _service;
        private readonly Servers _servers;

        public CommandHandler(DiscordShardedClient client, CommandService commandService,
            IServiceProvider service, Servers servers)
        {
            _client = client;
            _commandService = commandService;
            _service = service;
            _servers = servers;
        }

        public override async Task InitializeAsync(CancellationToken cancellationToken)
        {
            _client.MessageReceived += OnMessageReceived;
            _client.JoinedGuild += OnGuildJoined;
            
            _commandService.CommandExecuted += OnCommandExecuted;
            
            await _client.GetRecommendedShardCountAsync();
            
            await _commandService.AddModulesAsync(Assembly.GetEntryAssembly(), _service);
        }

      
        private static async Task OnGuildJoined(SocketGuild arg)
        {
            await arg.DefaultChannel.SendMessageAsync(
                "Hello, Ｏｌｉｖｉａ is here. " +
                "Thanks for inviting me to your Server!" +
                "For my command please use * as a prefix");
        }

        private static async Task OnCommandExecuted(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            if (command.IsSpecified && !result.IsSuccess)
            {
                if (!(context.Channel is ISocketMessageChannel errorChannel)) return;
                switch (result.Error)
                {
                    case CommandError.UnknownCommand:
                        await errorChannel.SendErrorAsync("Error", "Sorry I don't know that command!");
                        break;
                    case CommandError.BadArgCount:
                        await errorChannel.SendErrorAsync("Error", "You need to provide arguments!");
                        break;
                    case CommandError.Exception:
                        await errorChannel.SendErrorAsync("Error",
                            "An exception happened while executing your command!");
                        break;
                    case CommandError.MultipleMatches:
                        await errorChannel.SendErrorAsync("Error",
                            "Look like there is something has the same name as your wish");
                        break;
                    case CommandError.ObjectNotFound:
                        await errorChannel.SendErrorAsync("Error",
                            "There was an issue finding the information provided! Please make sure it is correct!");
                        break;
                    case CommandError.ParseFailed:
                        await errorChannel.SendErrorAsync("Error",
                            "Failed to convert the data type you input!");
                        break;
                    case CommandError.UnmetPrecondition:
                        await errorChannel.SendErrorAsync("Error",
                            "Look like you or the bot don't have the permission to do that!");
                        break;
                    case CommandError.Unsuccessful:
                        await errorChannel.SendErrorAsync("Error", "The command failed to execute!");
                        break;
                    case null:
                        await errorChannel.SendErrorAsync("Error",
                            "Null exception happened! This shouldn't be happening though!");
                        break;
                    default:
                        await errorChannel.SendErrorAsync("Error",
                            "Big error happened while executing your command!");
                        break;
                }
            }
        }
        private async Task OnMessageReceived(SocketMessage arg)
        {
            if (!(arg is SocketUserMessage message)) return;

            /*
            if (!((SocketGuildChannel) message.Channel)
                .Guild.GetUser(message.Author.Id).GuildPermissions.Administrator)
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
                        $"{message.Author.Mention} has mentioned everyone in channel <#{message.Channel.Id}>, shame on them!");
                }

                if (message.MentionedRoles.Count >= 4)
                {
                    await message.DeleteAsync();
                    await message.Channel.SendErrorAsync("Warning",
                        $"{message.Author.Mention} has mentioned more than 4 roles in channel <#{message.Channel.Id}>, shame on them!");
                }

                if (message.MentionedUsers.Count >= 4)
                {
                    await message.DeleteAsync();
                    await message.Channel.SendErrorAsync("Warning",
                        $"{message.Author.Mention} has mentioned more than 4 people in channel <#{message.Channel.Id}>, shame on them!");
                }
            }
            */
            
            var context = new ShardedCommandContext(_client, message);
            var argPos = 0;
            
            var guild = ((SocketGuildChannel) message.Channel).Guild;
            
            var prefix = await _servers.GetGuildPrefix(((SocketGuildChannel) message.Channel).Guild.Id) ?? "*";
            if (context.Message.HasStringPrefix(prefix, ref argPos,
                    StringComparison.CurrentCultureIgnoreCase) ||
                context.Message.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                await HandleCommandAsync(context, argPos).ConfigureAwait(false);
            }
        }
        
        private async Task HandleCommandAsync(ShardedCommandContext context, int argPos)
        {
            try
            {
                // Searching for the command that should be executed
                var searchResult = _commandService.Search(context, argPos);

                // If no command were found, return
                if (searchResult.Commands == null || searchResult.Commands.Count == 0)
                {
                    return;
                }

                // Execute the command.
                await _commandService.ExecuteAsync(context, argPos, _service).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                // Logging any error that occur and that are not being handled by the error handlers.
            }
        }
        
    }
}