using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.EventLog;
using NLog.Web;
using Ploutos.GlobalComJobs.Extensions;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Ploutos.GlobalComJobs
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

            try
            {
                logger.Log(NLog.LogLevel.Warn, "Start Service");
                var isService = !(Debugger.IsAttached || args.Contains("--console"));

                if (isService)
                {
                    var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
                    var pathToContentRoot = Path.GetDirectoryName(pathToExe);
                    Directory.SetCurrentDirectory(pathToContentRoot);
                }

                var builder = CreateWebHostBuilder(args.Where(arg => arg != "--console").ToArray());

                var host = builder.Build();

                host.RunAsCustomService(isService);
            }
            catch
            {
                logger.Log(NLog.LogLevel.Warn, "Stopped Service because of exception!");
                throw;
            }
            finally
            {
                logger.Log(NLog.LogLevel.Warn, "Stop Service");
                NLog.LogManager.Shutdown();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();
            string evn = configuration["Environment"] ?? string.Empty;

            return WebHost.CreateDefaultBuilder(args)
               .ConfigureLogging((hostingContext, logging) =>
               {
                   logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                   logging.AddEventLog(new EventLogSettings
                   {
                       Filter = (x, logLevel) => x.Contains("Ploutos.GlobalComJobs") && logLevel >= LogLevel.Information
                   });
               })
               .UseContentRoot(Directory.GetCurrentDirectory())
               .UseConfiguration(configuration)
               .UseEnvironment(evn)
               .UseStartup<Startup>()
               .UseUrls($"http://0.0.0.0:{configuration["Port"]}")
               .UseNLog();
        }
    }
}