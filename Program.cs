using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using AnotherMyouri.DatabaseEntities.DiscordBotContext;
using AnotherMyouri.DatabaseEntities.EntitiesConfig;
using AnotherMyouri.Handler;
using AnotherMyouri.Helpers;
using AnotherMyouri.Mischief;
using AnotherMyouri.Modules;
using Discord;
using Discord.Addons.Hosting;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Discord.Addons.Interactive;
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
                    config.DefaultRunMode = RunMode.Sync;
                    config.SeparatorChar = '|';
                })
                .ConfigureServices((context, services) =>
                {
                    services
                        .AddHostedService<CommandHandler>()
                        .AddDbContext<MyouriDbContext>()
                        
                        /*
                        .AddDbContext<MyouriDbContext>
                    
                        (
                            options => options.UseMySql
                            (
                                context.Configuration["database"],
                                new MySqlServerVersion(new Version(8, 0, 21))
                            )
                        )
                        */
                        
                        .AddSingleton<EmbedBuilder>()
                        .AddSingleton<HttpClient>()
                        .AddSingleton<HttpClientService>()
                        .AddScoped<Servers>()
                        .AddSingleton<AutoVoiceChannels>()
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