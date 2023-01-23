using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProductionManagement.Common.Consts;
using ProductionManagement.Common.Enums;
using ProductionManagement.Model;
using ProductionManagement.Services.Configuration;
using ProductionManagement.Services.Services.Orders;
using ProductionManagement.Services.Services.Parts;
using ProductionManagement.Services.Services.ProductionLine;
using ProductionManagement.Services.Services.Tanks;
using ProductionManagement.Services.Services.Users;
using ProductionManagement.Services.Services.WorkSchedule;
using ProductionManagement.WebUI.Configuration;
using Serilog;

namespace ProductionManagement.WebUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(
                typeof(AutoMapperWebConfig),
                typeof(AutoMapperServiceConfig));

            services.AddDbContext<ProductionManagementContext>(options =>
                options.EnableSensitiveDataLogging(true)
                .UseSqlServer(Configuration.GetConnectionString("DefaultConnection").ToString()));

            services.AddAuthentication(o =>
            {
                o.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                o.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                o.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.ExpireTimeSpan = System.TimeSpan.FromMinutes(300);
                options.AccessDeniedPath = new PathString(string.Empty);

                options.LoginPath = string.Empty;
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                };
            });

            services.AddHttpContextAccessor();

            services.AddControllersWithViews();

            services.AddTransient<IPartsService, PartsService>();
            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<ITanksService, TanksService>();
            services.AddTransient<IProductionLineService, ProductionLineService>();
            services.AddTransient<IOrdersService, OrdersService>();
            services.AddTransient<IWorkScheduleService, WorkScheduleService>();

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(CustomPolicy.Administrator, policy =>
                {
                    policy.RequireClaim(CustomClaimTypesConsts.Roles,
                    RolesEnum.Administrator.ToString());
                });

                options.AddPolicy(CustomPolicy.Calendar, policy =>
                {
                    policy.RequireClaim(CustomClaimTypesConsts.Roles,
                    RolesEnum.Calendar.ToString(),
                    RolesEnum.Administrator.ToString());
                });

                options.AddPolicy(CustomPolicy.CalendarView, policy =>
                {
                    policy.RequireClaim(CustomClaimTypesConsts.Roles,
                    RolesEnum.Orders.ToString(),
                    RolesEnum.OrdersView.ToString(),
                    RolesEnum.Calendar.ToString(),
                    RolesEnum.CalendarView.ToString(),
                    RolesEnum.Administrator.ToString());
                });

                options.AddPolicy(CustomPolicy.ProductionLines, policy =>
                {
                    policy.RequireClaim(CustomClaimTypesConsts.Roles,
                    RolesEnum.ProductionLines.ToString(),
                    RolesEnum.Administrator.ToString());
                });

                options.AddPolicy(CustomPolicy.ProductionLinesView, policy =>
                {
                    policy.RequireClaim(CustomClaimTypesConsts.Roles,
                    RolesEnum.ProductionLines.ToString(),
                    RolesEnum.Settings.ToString(),
                    RolesEnum.SettingsView.ToString(),
                    RolesEnum.Administrator.ToString());
                });

                options.AddPolicy(CustomPolicy.Settings, policy =>
                {
                    policy.RequireClaim(CustomClaimTypesConsts.Roles,
                    RolesEnum.Settings.ToString(),
                    RolesEnum.Administrator.ToString());
                });

                options.AddPolicy(CustomPolicy.SettingsView, policy =>
                {
                    policy.RequireClaim(CustomClaimTypesConsts.Roles,
                    RolesEnum.Settings.ToString(),
                    RolesEnum.SettingsView.ToString(),
                    RolesEnum.Administrator.ToString());
                });

                options.AddPolicy(CustomPolicy.Orders, policy =>
                {
                    policy.RequireClaim(CustomClaimTypesConsts.Roles,
                    RolesEnum.Orders.ToString(),
                    RolesEnum.Administrator.ToString());
                });

                options.AddPolicy(CustomPolicy.OrdersView, policy =>
                {
                    policy.RequireClaim(CustomClaimTypesConsts.Roles,
                    RolesEnum.Orders.ToString(),
                    RolesEnum.OrdersView.ToString(),
                    RolesEnum.Administrator.ToString());
                });
            });

            services.AddHealthChecks();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSerilogRequestLogging();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });
            app.UseHealthChecks("/health");
            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}