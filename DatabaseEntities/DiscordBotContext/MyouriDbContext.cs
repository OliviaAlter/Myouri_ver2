using AnotherMyouri.DatabaseEntities.Entities;
using Microsoft.EntityFrameworkCore;

namespace AnotherMyouri.DatabaseEntities.DiscordBotContext
{
    public class MyouriDbContext : DbContext
    {
        public MyouriDbContext(DbContextOptions<MyouriDbContext> contextOptions) : base(contextOptions)
        { }

        /*
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer("Server=MYOURI\\MYOURI;Database=MyouriAlter;Trusted_Connection=True;MultipleActiveResultSets=True;");
        */
        public DbSet<Server> Servers { get; set; }
        
    }
    
}