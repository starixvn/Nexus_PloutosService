namespace Ploutos.AutoJobs.Services
{
    using System.Threading.Tasks;
    using RestSharp;

    public interface IWebClientService
    {
        Task<IRestResponse> PostAsync<T>(string domain, string url, string token, T parameters);
    }
}