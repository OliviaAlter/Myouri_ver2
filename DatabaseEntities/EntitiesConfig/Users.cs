using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnotherMyouri.DatabaseEntities.DiscordBotContext;
using AnotherMyouri.DatabaseEntities.Entities;
using Microsoft.EntityFrameworkCore;

namespace AnotherMyouri.DatabaseEntities.EntitiesConfig
{
    public class Users
    {
        private static MyouriDbContext _context;

        public Users(MyouriDbContext context)
        {
            _context = context;
        }
        
    }
}