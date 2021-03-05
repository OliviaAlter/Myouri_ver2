using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.VisualBasic.CompilerServices;

namespace AnotherMyouri.Extensions
{
    public class EventExtension
    {
        public static async Task UserVoiceJoined(IUser user, IVoiceChannel channel, ITextChannel logChannel)
        {
            var builder = new EmbedBuilder()
                .WithThumbnailUrl(user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl())
                .WithTitle("User voice activity")
                .WithDescription($"{user.Mention} joined voice channel <#{channel.Id}>!")
                .WithFooter($"{user.Username}", user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl())
                .WithCurrentTimestamp()
                .WithColor(new Color(Mischief.Utilities.RandomColor(), Mischief.Utilities.RandomColor(), Mischief.Utilities.RandomColor()));
            await logChannel.SendMessageAsync(embed: builder.Build());
        }
        public static async Task UserVoicejumped(IUser user, IVoiceChannel channelBefore, IVoiceChannel channelAfter, ITextChannel logChannel)
        {
            var builder = new EmbedBuilder()
                .WithThumbnailUrl(user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl())
                .WithTitle("User voice activity")
                .WithDescription($"{user.Mention} jumped from voice channel <#{channelBefore.Id}> to voice channel <#{channelAfter.Id}>!")
                .WithFooter($"{user.Username}", user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl())
                .WithCurrentTimestamp()
                .WithColor(new Color(Mischief.Utilities.RandomColor(), Mischief.Utilities.RandomColor(), Mischief.Utilities.RandomColor()));
            await logChannel.SendMessageAsync(embed: builder.Build());
        }

        public static async Task UserVoiceLeft(IUser user, IVoiceChannel channel, ITextChannel logChannel)
        {
            var builder = new EmbedBuilder()
                .WithThumbnailUrl(user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl())
                .WithTitle("User voice activity")
                .WithDescription($"{user.Mention} left voice channel <#{channel.Id}>!")
                .WithFooter($"{user.Username}", user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl())
                .WithCurrentTimestamp()
                .WithColor(new Color(Mischief.Utilities.RandomColor(), Mischief.Utilities.RandomColor(), Mischief.Utilities.RandomColor()));
            await logChannel.SendMessageAsync(embed: builder.Build());
        }
    }
}