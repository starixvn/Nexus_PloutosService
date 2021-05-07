using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ploutos.Common.Services
{
    public interface IWebClientService
    {
        Task<IRestResponse> PostFormAsync<T>(string domain, string url, string token, T parameters);
        Task<IRestResponse> PostJsonAsync<T>(string domain, string url, string token, T parameters);
        Task<IRestResponse> PostAsync<T>(string domain, string url, string token, T parameters);
        Task<IRestResponse> GetAsync(string domain, string url, string token, List<Parameter> parameters);

        Task<IRestResponse> GetAsync(string domain, string url, List<Parameter> parameters);
    }
}
