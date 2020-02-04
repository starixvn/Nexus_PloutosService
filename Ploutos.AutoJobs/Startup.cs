namespace Fanex.NCB.AutoJobs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Fanex.Data;
    using Fanex.Data.MySql;
    using Fanex.Data.SqlServer;
    using Fanex.Logging;
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.AspNetCore;
    using Fanex.Logging.Extensions.Database;
    using Fanex.Logging.Sentry;
    using Hangfire;
    using Hangfire.MySql.Core;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;
    using NLog.Web;
    using ONEbook.UM.AspNetCore;
    using ONEbook.UM.Configurations;
    using ONEbook.UM.Helpers;
    using ONEbook.UM.Services;
    using ONEbook.UM.Transport;
    using Ploutos.AutoJobs.App_Configs;
    using Ploutos.AutoJobs.Extensions;
    using Ploutos.AutoJobs.Models;
    using Ploutos.AutoJobs.Repository;
    using Ploutos.AutoJobs.Services;
    using Ploutos.AutoJobs.Services.Interfaces;
    using Ploutos.AutoJobs.Utils;
    using Sentry;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var connections = Configuration.GetSection("ConnectionStrings")
                                    .GetChildren()
                                    .AsEnumerable()
                                    .Where(x => !string.IsNullOrWhiteSpace(x.GetValue<string>("Name")))
                                    .ToDictionary(x => x.GetValue<string>("Name"),
                                    y => new ConnectionConfiguration(y.GetValue<string>("Name"), y.GetValue<string>("ConnectionString"), y.GetValue<string>("ProviderName")));

            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

            string evn = Configuration["Environment"];
            logger.Warn(string.Format("ENVIRONMENT {0}", evn));
            DbSettingProviderManager
               .StartNewSession()
               .Use(connections)
               .WithMySql()
               .WithSqlServer()
               .UseDefaultDbSettingProvider($"~/App_Data/{evn}")
               .Run();

            LogManager
            .SetDefaultLogCategory(Configuration.GetSection("LogCategory").Value)
            .Use(new List<ILogEngine>
            {
                new SentryLogging(new SentryEngineOptions
                {
                    Dsn = new Dsn(Configuration.GetSection("SentryEngineOptions:Dsn").Value)
                })
            });

            ExceptionHandler
                .UseConfig(new LoggingConfiguration())
                .UseGetDbExceptionInfo(DatabaseInfoGetters.GetDbExceptionInfo);

            JsonConvert.DefaultSettings = () =>
            {
                return new JsonSerializerSettings
                {
                    DateTimeZoneHandling = DateTimeZoneHandling.Utc
                };
            };
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
            {
                options.LoginPath = Configuration.GetSection("AuthSettings:LoginPath").Value;
                options.LogoutPath = Configuration.GetSection("AuthSettings:LogoutPath").Value;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(int.Parse(Configuration.GetSection("AuthSettings:CookieTimeOut").Value));
                options.Cookie = new CookieBuilder
                {
                    Name = "Ploutos.AutoJobs.Cookie",
                    SecurePolicy = CookieSecurePolicy.SameAsRequest
                };
                options.Events.OnSigningOut = context =>
                {
                    context.Response.Redirect(options.LoginPath);
                    return Task.CompletedTask;
                };
            });

            services.AddSession(options =>
            {
                options.Cookie.IsEssential = true;
                options.IdleTimeout = TimeSpan.FromMinutes(int.Parse(Configuration.GetSection("AuthSettings:SessionTimeOut").Value));
                options.Cookie.Name = "Ploutos.AutoJobs.Session";
            });

            services.AddSingleton(Configuration);
            services.Configure<IdentifyModel>(Configuration.GetSection("HangfireAuthenticate"));
            services.Configure<CronJob>(Configuration.GetSection("CronJob"));
            services.Configure<ApiConfigs>(Configuration.GetSection("ApiConfigs"));
            services.Configure<ProxySettings>(Configuration.GetSection("ProxySettings"));
            services.Configure<MailConfigs>(options => Configuration.GetSection("MailConfigs").Bind(options));
            services.AddTransient<ILogger, Logger>();
            services.AddSingleton(new UMClientConfiguration().Initialize(Configuration));
            RegisterService(services);
            RegisterRepository(services);

            RegisterHangfire(services, connections.FirstOrDefault(x => x.Key.Equals("Hangfire")).Value.ConnectionString);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseCookiePolicy();
            app.UseAuthentication();

            app.UseHangfireDashboard(options: new DashboardOptions
            {
                Authorization = new[] { new HangfireAuthorizationFilter() }
            }).UseHangfireServer(options: new BackgroundJobServerOptions
            {
                WorkerCount = Configuration.GetValue<int>("WorkerCount"),
                ServerName = Environment.MachineName
            });

            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Auth}/{action=Login}/{id?}");
            });
        }

        //Extensions
        private static void RegisterService(IServiceCollection services)
        {
            #region ONEbookUM

            services.AddSingleton<IUMCacheService, UMCacheService>();
            services.AddSingleton<IUMRestClient, UMRestClient>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IHttpContextHelper, HttpContextHelper>();
            services.AddSingleton<IMaintenanceService, MaintenanceService>();
            services.AddSession();

            #endregion ONEbookUM

            services.AddScoped<ISupportApiService, SupportApiService>();
            services.AddScoped<IOpal88PlayerService, Opal88PlayerService>();
            services.AddScoped<IOpal88TicketService, Opal88TicketService>();
            services.AddScoped<INova88PlayerService, Nova88PlayerService>();
            services.AddScoped<INova88TicketService, Nova88TicketService>();
            services.AddScoped<IIvorybetPlayerService, IvorybetPlayerService>();
            services.AddScoped<IIvorybetTicketService, IvorybetTicketService>();
            services.AddScoped<IIvorybetTicketBySupportApiService, IvorybetTicketBySupportApiService>();
            services.AddScoped<INova88TicketBySupportApiService, Nova88TicketBySupportApiService>();
            services.AddScoped<IOpal88TicketBySupportApiService, Opal88TicketBySupportApiService>();
            services.AddScoped<IJobService, JobService>();
            services.AddScoped<IWebClientService, WebClientService>();
            services.AddScoped<IDataService, DataService>();
            services.AddScoped<IUtil, Util>();

            services.AddTransient<IRecurringJobManager, RecurringJobManager>();
            services.AddTransient<IJobRunningChecker, JobRunningChecker>();
        }

        private static void RegisterRepository(IServiceCollection services)
        {
            services.AddSingleton<IRedisRepository, RedisRepository>();
            services.AddScoped<IBaseRepository, BaseRepository>();
        }

        private static void RegisterHangfire(IServiceCollection services, string connectionString)
        {
            var connection = Cryptography.DecryptConnectionString(connectionString);
            services.AddHangfire(x => x.UseStorage(new MySqlStorage(connection)));
        }
    }
}