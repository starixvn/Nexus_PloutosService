namespace Ploutos.AutoJobs.Services
{
    using RestSharp;
    using System.Threading.Tasks;

    public interface IWebClientService
    {
        Task<IRestResponse> PostAsync<T>(string domain, string url, string token, T parameters);
    }
}
