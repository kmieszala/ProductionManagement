using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace ProductionManagement.WebUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
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
                    .UseKestrel()
                    .UseUrls();
                }).UseSerilog();
    }
}

//using Microsoft.EntityFrameworkCore;
//using ProductionManagement.Model;
//using ProductionManagement.Services.Configuration;
//using ProductionManagement.Services.Services.Parts;
//using ProductionManagement.WebUI.Configuration;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddControllersWithViews();

//builder.Services.AddAutoMapper(typeof(AutoMapperWebConfig), typeof(AutoMapperServiceConfig));

//builder.Services.AddTransient<IPartsService, PartsService>();

//builder.Services.AddDbContext<ProductionManagementContext>(options =>
//  options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionStrings")));

//builder.WebHost.ConfigureKestrel(options =>
//{
//    options.ListenAnyIP(5001); // to listen for incoming http connection on port 5001
//    options.ListenAnyIP(7001, configure => configure.UseHttps()); // to listen for incoming https connection on port 7001
//});


//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

//app.UseHttpsRedirection();
//app.UseStaticFiles();
//app.UseRouting();


//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller}/{action=Index}/{id?}");

//app.MapFallbackToFile("index.html"); ;

//app.Run();
