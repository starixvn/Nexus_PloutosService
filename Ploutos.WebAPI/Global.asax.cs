namespace Ploutos.WebAPI
{
    using System;
    using System.Collections.Generic;
    using System.Web.Configuration;
    using System.Web.Http;
    using System.Web.Routing;
    using Fanex.Data;
    using Fanex.Data.MySql;
    using Ploutos.WebAPI.Services;

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var connectionString = WebConfigurationManager.ConnectionStrings["blueberrydb"].ConnectionString;
            var providerName = WebConfigurationManager.ConnectionStrings["blueberrydb"].ProviderName;
            var connection = new Dictionary<string, ConnectionConfiguration>()
            {
                {"blueberrydb", new ConnectionConfiguration("blueberrydb", connectionString, providerName) }
            };
            DbSettingProviderManager
                .StartNewSession()
                .Use(connection)
                .WithMySql()
                .UseDefaultDbSettingProvider("~/App_Data", true)
                .Run();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError().GetBaseException();
            WriteLogToFile.Writelog(ex.Message);
        }
    }
}