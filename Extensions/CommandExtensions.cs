using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace AnotherMyouri.Extensions
{
    public static class CommandExtensions
    {
        public static async Task<IMessage> SendErrorAsync(this ISocketMessageChannel channel, string title,
            string description, RequestOptions options = null)
        {
            var embed = new EmbedBuilder()
                .WithColor(Mischief.Utilities.RandomColor(),
                    Mischief.Utilities.RandomColor(),
                    Mischief.Utilities.RandomColor() )
                .WithDescription(description)
                .WithAuthor(author =>
                {
                    author.WithIconUrl("https://cdn.icon-icons.com/icons2/1380/PNG/512/vcsconflicting_93497.png")
                        .WithName(title);
                })
                .WithCurrentTimestamp()
                .Build();
            var message = await channel.SendMessageAsync(embed: embed);
            return message;
        }
    }
}