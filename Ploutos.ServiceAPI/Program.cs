namespace Ploutos.ServiceAPI
{
    using System.IO;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;

    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();
            string evn = configuration["Environment"] ?? string.Empty;

            return WebHost.CreateDefaultBuilder(args)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseConfiguration(configuration)
                .UseEnvironment(evn)
                .UseStartup<Startup>();
        }
    }
}