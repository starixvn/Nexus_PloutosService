namespace Ploutos.GlobalComJobs
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using Fanex.Data;
    using Fanex.Data.SqlServer;
    using Fanex.Logging;
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.AspNetCore;
    using Fanex.Logging.Extensions.Database;
    using Fanex.Logging.Sentry;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using NLog.Web;
    using ONEbook.UM.AspNetCore;
    using ONEbook.UM.Configurations;
    using ONEbook.UM.Helpers;
    using ONEbook.UM.Services;
    using ONEbook.UM.Transport;
    using Ploutos.Common.Models;
    using Ploutos.Common.Repository;
    using Ploutos.Common.Services;
    using Ploutos.Common.Utils;
    using Ploutos.GlobalComJobs.Models;
    using Ploutos.GlobalComJobs.Services;
    using Ploutos.GlobalComJobs.Services.Interfaces;
    using Ploutos.GlobalComJobs.Workers;
    using RestSharp;
    using Sentry;

    public static class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            var isService = !(Debugger.IsAttached || args.Contains("--console"));
            if (isService)
            {
                var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
                var pathToContentRoot = Path.GetDirectoryName(pathToExe);
                Directory.SetCurrentDirectory(pathToContentRoot);
            }

            var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

            try
            {
                logger.Info("Application start.");
                Host.CreateDefaultBuilder(args)
               .UseWindowsService()
               .ConfigureAppConfiguration(builder => builder.AddConfiguration(configuration))
               .UseNLog()
               .ConfigureServices((hostContext, services) =>
               {
                   AddConfiguration(services, configuration);
                   AddDependencyInjection(services, configuration);
                   AddWorker(services);
               })
               .Build().Run();

            }
            catch (Exception exception)
            {
                logger.Error(exception, "Application terminated unexpectedly.");
                throw;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }

        private static void AddConfiguration(IServiceCollection services, IConfiguration configuration)
        {
            var connections = configuration.GetSection("ConnectionStrings")
                .GetChildren()
                .AsEnumerable()
                .Where(x => !string.IsNullOrWhiteSpace(x.GetValue<string>("Name")))
                .ToDictionary(x => x.GetValue<string>("Name"),
                    y => new ConnectionConfiguration(y.GetValue<string>("Name"), y.GetValue<string>("ConnectionString"), y.GetValue<string>("ProviderName")));

            DbSettingProviderManager
               .StartNewSession()
               .Use(connections)
               .WithSqlServer()
               .UseDefaultDbSettingProvider("~/App_Data/SpNames.xml")
               .Run();

            LogManager
                .SetDefaultLogCategory(configuration.GetSection("LogCategory").Value)
                .Use(new List<ILogEngine>
                {
                    new SentryLogging(new SentryEngineOptions
                    {
                        Dsn = new Dsn(configuration.GetSection("SentryEngineOptions:Dsn").Value)
                    })
                });

            services.Configure<IConfiguration>(configuration);

            ExceptionHandler
                .UseConfig(new LoggingConfiguration())
                .UseGetDbExceptionInfo(DatabaseInfoGetters.GetDbExceptionInfo);

            //services.Configure<AppJobSettings>(options => configuration.GetSection("AppSettings").Bind(options));
            services.Configure<List<JobSetting>>(configuration.GetSection("JobSettings"));
            services.Configure<ProxySettings>(configuration.GetSection("ProxySettings"));
            services.Configure<ApiConfigs>(configuration.GetSection("ApiConfigs"));

        }

        private static void AddDependencyInjection(IServiceCollection services, IConfiguration configuration)
        {
            #region ONEbookUM

            services.AddSingleton(new UMClientConfiguration().Initialize(configuration));
            services.AddSingleton<IHttpContextHelper, HttpContextHelper>();
            services.AddSingleton<IUMCacheService, UMCacheService>();
            services.AddSingleton<IUMRestClient, UMRestClient>();
            services.AddSingleton<IMaintenanceService, MaintenanceService>();

            #endregion ONEbookUM

            services.AddSingleton(configuration);
            services.AddSingleton<ILogServiceExtension, LogServiceExtension>();
            services.AddTransient<Fanex.Logging.ILogger, Logger>();

            services.AddScoped<I12BTicketService, L12BTicketService>();
            services.AddScoped<IOpal88TicketService, Opal88TicketService>();
            services.AddScoped<INova88TicketService, Nova88TicketService>();
            services.AddScoped<IIvorybetTicketService, IvorybetTicketService>();
            services.AddSingleton<IRestClient, RestClient>();
            services.AddScoped<IWebClientService, WebClientService>();
            services.AddScoped<IDataService, DataService>();
            services.AddScoped<IUtil, Util>();

            services.AddScoped<IBaseRepository, BaseRepository>();
            services.AddScoped<Fanex.Logging.ILogger, Logger>(); 

            services.AddScoped<IIvorybetTicketService, IvorybetTicketService>();
        }

        private static void AddWorker(IServiceCollection services)
        {
            services.AddHostedService<SendTicketIvorybetWorker>();
            
        }
    }
}
