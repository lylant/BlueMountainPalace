using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using BlueMountainPalace.Data; // Add this namespace to find SeedRoles class here
using Microsoft.Extensions.DependencyInjection;

namespace BlueMountainPalace
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Create the WebHost object
            var host = CreateHostBuilder(args).Build();

            // Invoke the SeedRoles.CreateRoles() here
            using (var scope = host.Services.CreateScope())
            {
                // Get the ServiceProvider
                var services = scope.ServiceProvider;
                try
                {
                    // Bind the ServiceProvider and the Configuration(appsetting.json)
                    var serviceProvider = services.GetRequiredService<IServiceProvider>();
                    var configuration = services.GetRequiredService<IConfiguration>();

                    // Pass the ServiceProvider and the Configuration object to CreateRoles()
                    SeedRoles.CreateRoles(serviceProvider, configuration).Wait();
                }
                catch (Exception exception)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(exception, "Error - Creating Roles");
                }
            }

            // Start the Web Application
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
