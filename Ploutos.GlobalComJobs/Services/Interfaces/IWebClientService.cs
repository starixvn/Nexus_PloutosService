using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ploutos.GlobalComJobs.Services.Interfaces
{
    public interface IWebClientService
    {
        Task<IRestResponse> PostAsync<T>(string domain, string url, string token, T parameters);
        Task<IRestResponse> GetAsync(string domain, string url, string token, List<Parameter> parameters);
    }
}
