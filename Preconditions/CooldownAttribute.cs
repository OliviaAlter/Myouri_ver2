using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using AnotherMyouri.Structure;
using Discord.Commands;

namespace AnotherMyouri.Preconditions
{
    public class CooldownAttribute : PreconditionAttribute
    {
        private readonly ConcurrentDictionary<CooldownInfo, DateTime> cooldowns =
            new ConcurrentDictionary<CooldownInfo, DateTime>();

        private TimeSpan CooldownLength { get; }
        
        public CooldownAttribute(int seconds)
        {
            CooldownLength = TimeSpan.FromSeconds(seconds);
        }
        
        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command,
            IServiceProvider services)
        {
            var key = new CooldownInfo(context.User.Id, command.GetHashCode());

            var time = DateTime.Now.Add(CooldownLength);
            if (cooldowns.TryGetValue(key, out var endsAt))
            {
                var difference = endsAt.Subtract(DateTime.Now);
                if (difference.Ticks > 0)
                {
                    return Task.FromResult(
                        PreconditionResult.FromError(
                            $"Please wait {difference:ss} seconds before trying again!"));
                }
                cooldowns.TryUpdate(key, time, endsAt);
            }
            else
            {
                cooldowns.TryAdd(key, DateTime.Now.Add(CooldownLength));
            }

            return Task.FromResult(PreconditionResult.FromSuccess());
        }
    }
}