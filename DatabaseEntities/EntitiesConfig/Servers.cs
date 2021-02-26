using System.Linq;
using System.Threading.Tasks;
using AnotherMyouri.DatabaseEntities.DiscordBotContext;
using AnotherMyouri.DatabaseEntities.Entities;
using Microsoft.EntityFrameworkCore;

namespace AnotherMyouri.DatabaseEntities.EntitiesConfig
{
    public class Servers
    {
        private readonly MyouriDbContext _context;

        public Servers(MyouriDbContext context)
        {
            _context = context;
        }

        #region Guild Prefix

        public async Task ModifyPrefix(ulong id, string prefix)
        {
            var server = await _context.Servers.FindAsync(id);
            if (server == null) 
                await _context.Servers.AddAsync(new Server{Id = id, Prefix = prefix});
            else server.Prefix = prefix;
            await _context.SaveChangesAsync();
        }
        
        public async Task<string> GetGuildPrefix(ulong id)
        {
            var prefix = await _context.Servers.AsQueryable()
                .Where(x => x.Id == id)
                .Select(x => x.Prefix)
                .FirstOrDefaultAsync();
            return await Task.FromResult(prefix);
        }
        
        #endregion

    }
}