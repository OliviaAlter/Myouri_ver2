using Discord.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Discord.Addons.Interactive.Criteria
{
    public class Criteria<T> : ICriterion<T>
    {
        private readonly List<ICriterion<T>> _criteria = new List<ICriterion<T>>();

        public Criteria<T> AddCriterion(ICriterion<T> criterion)
        {
            _criteria.Add(criterion);
            return this;
        }

        public async Task<bool> JudgeAsync(SocketCommandContext sourceContext, T parameter)
        {
            foreach (var criterion in _criteria)
            {
                var result = await criterion.JudgeAsync(sourceContext, parameter).ConfigureAwait(false);
                if (!result) return false;
            }
            return true;
        }
    }
}