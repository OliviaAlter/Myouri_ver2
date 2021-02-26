using Discord.Commands;
using System.Threading.Tasks;

namespace Discord.Addons.Interactive.Criteria
{
    public interface ICriterion<in T>
    {
        Task<bool> JudgeAsync(SocketCommandContext sourceContext, T parameter);
    }
}