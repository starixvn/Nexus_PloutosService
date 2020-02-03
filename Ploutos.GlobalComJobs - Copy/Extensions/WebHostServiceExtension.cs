using Microsoft.AspNetCore.Hosting;
using Ploutos.GlobalComJobs.App_Configs;
using System.ServiceProcess;

namespace Ploutos.GlobalComJobs.Extensions
{
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
