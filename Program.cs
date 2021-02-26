using System;
using System.IO;
using System.Threading.Tasks;
using AnotherMyouri.DatabaseEntities.DiscordBotContext;
using AnotherMyouri.DatabaseEntities.Entities;
using AnotherMyouri.Handler;
using Discord;
using Discord.Addons.Hosting;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AnotherMyouri
{
    public static class Program
    {
        private static async Task Main()
        {
            var builder = new HostBuilder()
                .ConfigureAppConfiguration(x =>
                {
                    var config = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", false, true)
                        .Build();
                    x.AddConfiguration(config);
                })
                .ConfigureLogging(x =>
                {
                    x.AddConsole();
                    x.SetMinimumLevel(LogLevel.Warning);
                })
                .ConfigureDiscordHost<DiscordSocketClient>((context, config) =>
                {
                    config.SocketConfig = new DiscordSocketConfig
                    {
                        LogLevel = LogSeverity.Warning,
                        AlwaysDownloadUsers = true,
                        MessageCacheSize = 400,
                        ExclusiveBulkDelete = true,
                    };
                    config.Token = context.Configuration["token"];
                })
                .UseCommandService((context, config) =>
                {
                    config.CaseSensitiveCommands = false;
                    config.LogLevel = LogSeverity.Warning;
                    config.DefaultRunMode = RunMode.Async;
                    config.SeparatorChar = '|';
                })
                .ConfigureServices((context, services) =>
                {
                    services
                        .AddSingleton<CommandHandler>()

                        .AddDbContext<MyouriDbContext>
                        (
                            options => options.UseSqlServer
                            (
                                context.Configuration["database"]
                            )
                        )
                        .AddSingleton<Server>()
                        .AddSingleton<InteractiveService>();
                })
                .UseConsoleLifetime();
            var host = builder.Build();
            using (host)
            {
                await host.RunAsync();
            }
        }
    }
}