namespace Ploutos.AutoJobs.Extensions
{
    using System.ServiceProcess;
    using Microsoft.AspNetCore.Hosting;
    using Ploutos.AutoJobs.App_Configs;

    public static class WebHostServiceExtension
    {
        public static void RunAsCustomService(this IWebHost host, bool isService)
        {
            var webHostService = new CustomWebHostService(host);
            if (isService)
            {
                ServiceBase.Run(webHostService);
            }
            else
            {
                webHostService.RunHangfireJobs();
                host.Run();
            }
        }
    }
}