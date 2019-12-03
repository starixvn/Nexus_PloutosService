namespace Ploutos.WebAPI.Models
{
    using System.Configuration;

    public static class AppSettings
    {
        public static string AppCredentialId => ConfigurationManager.AppSettings["AppCredentialId"];

        public static string AppCredentialPassword => ConfigurationManager.AppSettings["AppCredentialPassword"];

        public static string Secret => ConfigurationManager.AppSettings["Secret"];
    }
}