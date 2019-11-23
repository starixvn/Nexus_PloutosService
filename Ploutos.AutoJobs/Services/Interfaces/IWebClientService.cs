using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ploutos.AutoJobs.Services
{
    public interface IWebClientService
    {
        Task<IRestResponse> PostAsync<T>(string domain, string url, string token, T parameters);
    }
}
