using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace ProductionManagement.Web
{
    public static class Program
    {
        public static void Main(string[] args)
        {

            var config = new ConfigurationBuilder()
                           .SetBasePath(Directory.GetCurrentDirectory())
                           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                           .AddCommandLine(args)
                           .Build();

            var logFile = config.GetSection("AppSettings")["LogFile"] ?? "webUI";
            var logLevel = config.GetSection("AppSettings")["LogLevel"] ?? "Info";
            var logDirectory = config.GetSection("AppSettings")["LogDirectory"] ?? "";
            var storeDays = config.GetSection("AppSettings")["StoreDays"] ?? "7";
            var timeout = config.GetSection("AppSettings")["RequestTimeOut"] ?? "20";

            var host = WebHost.CreateDefaultBuilder(args)
                        .UseKestrel(opt =>
                        {
                            opt.Limits.MinRequestBodyDataRate = new MinDataRate(bytesPerSecond: 240, gracePeriod: TimeSpan.Parse(timeout));
                        })
                        .UseConfiguration(config)
                        .UseContentRoot(Directory.GetCurrentDirectory())
                        .UseStartup<Startup>()
                        .UseSerilog((context, configuration) =>
                        {
                            configuration
                                .MinimumLevel.Information()
                                .Enrich.FromLogContext()
                                .WriteTo.File(Path.Combine(logDirectory, logFile), rollingInterval: RollingInterval.Day, retainedFileCountLimit: int.Parse(storeDays))
                                .WriteTo.Console(
                                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
                                    theme: AnsiConsoleTheme.Literate);

                            if (logLevel == "Debug")
                            {
                                configuration.MinimumLevel.Debug();
                            }
                            else if (logLevel == "Warning")
                            {
                                configuration.MinimumLevel.Warning();
                            }
                        })
                        .Build();

            Log.Information("WebUI - starting...");
            host.Run();
        }
    }
}