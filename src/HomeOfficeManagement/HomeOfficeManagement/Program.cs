using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using HomeOfficeManagement.Core.Config.Implementations;
using HomeOfficeManagement.Core.Config.Interfaces;
using HomeOfficeManagement.Core.Models.Config;
using HomeOfficeManagement.Core.Services.Implementations;
using HomeOfficeManagement.Core.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;

namespace HomeOfficeManagement
{
    public static class Program
    {
        private static HomeOfficeManagerConfigDTO _config;

        public static async Task Main(string[] args)
        {
            var logger = LogManager.GetCurrentClassLogger();

            //LogManager.Configuration.AddRule(LogLevel.Fatal, LogLevel.Fatal, "mail");
            try
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory()) //From NuGet Package Microsoft.Extensions.Configuration.Json
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .Build();

                var configloader = new HomeOfficeManagerConfigLoader();
                _config = configloader.GetConfig();

                if (_config is null)
                {
                    logger.Fatal("Config nicht vorhanden - breche Verarbeitung ab");
                    logger.Trace("Exit with Applicationcode 10");
                    LogManager.Shutdown();
                    Environment.Exit(10);
                }

                await using var servicesProvider = new ServiceCollection()
                    .AddSingleton<ManagerEngine>() // Runner is the custom class
                    .AddScoped<IKalenderService, GoogleKalenderService>()
                    .AddScoped<IConfigLoader<HomeOfficeManagerConfigDTO>, HomeOfficeManagerConfigLoader>()
                    .AddScoped<IFritzBoxService, FritzBoxService>()
                    .AddScoped(ImplementationFactory)
                    .AddLogging(loggingBuilder =>
                    {
                        // configure Logging with NLog
                        loggingBuilder.ClearProviders();
                        loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                        loggingBuilder.AddNLog(config);
                    }).BuildServiceProvider();

                logger.Info("# # # # Start von HomeOfficeManagement # # # #");
                logger.Info("Logger initialisiert");
                var runner = servicesProvider.GetRequiredService<ManagerEngine>();
                await runner.StarteEngine();
            }
            catch (Exception ex)
            {
                // NLog: catch any exception and log it.
                logger.Error(ex, "Stopped program because of exception");
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                LogManager.Shutdown();
            }
        }

        private static CalendarService ImplementationFactory(IServiceProvider arg)
        {
            return new CalendarService(new BaseClientService.Initializer
            {
                ApplicationName = "HomeOfficeManager",
                HttpClientInitializer = Task.Run(async () => await GetAuthentication()).Result
            });
        }

        private static async Task<UserCredential> GetAuthentication()
        {
            await using var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read);
            var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(_config.ClientSecrets,
                new[] { CalendarService.Scope.CalendarEventsReadonly },
                "user", CancellationToken.None, new FileDataStore("Books.ListMyLibrary"));

            return credential;
        }
    }
}
