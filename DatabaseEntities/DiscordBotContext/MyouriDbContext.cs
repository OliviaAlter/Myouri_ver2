using System;
using AnotherMyouri.DatabaseEntities.Entities;
using Microsoft.EntityFrameworkCore;

namespace AnotherMyouri.DatabaseEntities.DiscordBotContext
{
    public class MyouriDbContext : DbContext
    {
        //public MyouriDbContext(DbContextOptions<MyouriDbContext> contextOptions) : base(contextOptions)
        //{ }

        
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseMySql("Server=localhost;User=root;database=MyouriAlter;port=3306;Connect Timeout=5", new MySqlServerVersion(new Version(8, 0, 21)));
        
        public DbSet<Server> Servers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<AutoVoice> AutoVoices { get; set; }
    }
    
}