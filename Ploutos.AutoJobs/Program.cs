namespace Fanex.NCB.AutoJobs
{
    using System;
    using System.Data;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.EventLog;
    using Ploutos.AutoJobs.Criteria;
    using Ploutos.AutoJobs.Extensions;
    using Ploutos.AutoJobs.Models;
    using Ploutos.AutoJobs.Repository;

    public static class Program
    {
        public static void Main(string[] args)
        {
            System.Console.WriteLine("123");
            //System.Console.ReadKey();

            


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
                       Filter = (x, logLevel) => x.Contains("Ploutos.AutoJobs") && logLevel >= LogLevel.Information
                   });
               })
               .UseContentRoot(Directory.GetCurrentDirectory())
               .UseConfiguration(configuration)
               .UseEnvironment(evn)
               .UseStartup<Startup>()
               .UseUrls($"http://0.0.0.0:{configuration["Port"]}");
        }
    }
}