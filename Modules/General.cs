using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace AnotherMyouri.Modules
{
    [Summary(":sa:")]
    public class General : ModuleBase<ShardedCommandContext>
    {
        private readonly DiscordShardedClient _client;
        private readonly EmbedBuilder _embed;

        public General(DiscordShardedClient client, EmbedBuilder embed)
        {
            _client = client;
            _embed = embed;
        }

        [Command("ping", true, RunMode = RunMode.Async)]
        [Summary("Get bot latency in ms")]
        public async Task Ping()
        {
            _embed.WithTitle("Info for" + Context.User.Username);
            _embed.WithDescription($"{Context.Client.Latency} ms");
            _embed.WithColor(new Color(255, 255, 255));
            await ReplyAsync("", false, _embed.Build()).ConfigureAwait(false);
            await ReplyAsync(
                $"{Context.Guild.Name}, {Context.Guild.Id}, {Context.Client.GetShardIdFor(Context.Guild)}, {MentionUtils.MentionChannel(Context.Channel.Id)}, {Context.User.Id}");
        }

        [Command("shards", RunMode = RunMode.Async)]
        [RequireBotPermission(GuildPermission.EmbedLinks)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task ShardsAsync()
        {
            _embed.WithTitle("Shard info for " + Context.User.Username);
            foreach (var shard in _client.Shards)
            {
                _embed.AddField($"Shard: {shard.ShardId} {(shard.Latency)}", $"{shard.Latency} ms\n" +
                                                                             $"{shard.Guilds.Count} Servers\n" +
                                                                             $"{shard.Guilds.Sum(x => x.MemberCount)} Members",
                    true);
            }

            _embed.WithDescription($"Average ping: {_client.Shards.Average(x => x.Latency)} ms");
            _embed.WithFooter($"You are on shard: {Context.Client.GetShardFor(Context.Guild)}");
            _embed.WithColor(new Color(255, 255, 255));
            await ReplyAsync("", false, _embed.Build()).ConfigureAwait(false);
            await ReplyAsync(
                $"{Context.Guild.Id}, {Context.Client.GetShardFor(Context.Guild)}, {Context.Channel.Id}, {Context.User.Id}");
        }
    }
}