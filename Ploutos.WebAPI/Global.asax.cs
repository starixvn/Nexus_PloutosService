namespace Ploutos.WebAPI
{
    using System.Collections.Generic;
    using System.Web.Configuration;
    using System.Web.Http;
    using Fanex.Data;
    using Fanex.Data.MySql;

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

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
    }
}