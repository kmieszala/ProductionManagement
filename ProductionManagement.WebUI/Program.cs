using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace ProductionManagement.WebUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                           .SetBasePath(Directory.GetCurrentDirectory())
                           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                           .AddUserSecrets<Program>()
                           .AddCommandLine(args)
                           .Build();

            CreateHostBuilder(args)
                 .ConfigureLogging(logging => { logging.ClearProviders(); })
                 .UseSerilog((context, configuration) =>
                 {
                     configuration.ReadFrom.Configuration(context.Configuration);
                 })
                .Build()
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()

                    // .UseKestrel()
                    .UseIISIntegration()
                    .UseUrls();
                }).UseSerilog();
    }
}