using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SwirlTheoryApi.Data;

namespace SwirlTheoryApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IWebHost host = CreateWebHostBuilder(args).Build();

            // Run seeding every time the server is started up
            // This means no overhead on requests, and no interference with EF commands
            RunSeeding(host);

            host.Run();
        }

        /// <summary>
        /// A method to create seed data in the database
        /// </summary>
        /// <param name="host">Gives us easy access to DI registered services (see Startup.cs -> ConfigureServices())</param>
        private static void RunSeeding(IWebHost host) {
            IServiceScopeFactory scopeFactory = host.Services.GetService<IServiceScopeFactory>();

            using (IServiceScope scope = scopeFactory.CreateScope()) {
                // Get the Seeder service from within the context of the scope object
                SwirlSeeder seeder = scope.ServiceProvider.GetService<SwirlSeeder>();
                seeder.SeedAsync().Wait();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(SetupConfiguration)
                .UseStartup<Startup>();

        /// <summary>
        /// Used to set up the configuration settings for our app (such as where to find a configuration file)
        /// </summary>
        /// <param name="ctx">Context</param>
        /// <param name="builder">Builder</param>
        private static void SetupConfiguration(WebHostBuilderContext ctx, IConfigurationBuilder builder) {
            builder.Sources.Clear();

            // Use a JSON file for config values (name: config.json, optional: false, reloadOnChange: true)
            // You can also load from environment vars
            // This allows us to store all config values in one place, without having to parse flat files
            // Order these by "later overrides earlier", in this case env vars beat local JSON config
            builder.AddJsonFile("config.json", false, true)
                .AddEnvironmentVariables();
        }
    }
}
