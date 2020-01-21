using Microsoft.Extensions.Options;
using Ploutos.GlobalComJobs.Models;
using Ploutos.GlobalComJobs.Services.Interfaces;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ploutos.GlobalComJobs.Services
{
    public class WebClientService : IWebClientService
    {
        private readonly IRestClient webClient;

        public WebClientService(IOptionsSnapshot<ProxySettings> proxySettings)
        {
            this.webClient = new RestClient { Encoding = Encoding.UTF8 };
            if (proxySettings.Value.IsUseProxy)
            {
                this.webClient.Proxy = new WebProxy(proxySettings.Value.ProxyAddress, proxySettings.Value.BypassOnLocal);
            }
        }

        public async Task<IRestResponse> GetAsync(string domain, string url, string token, List<Parameter> parameters)
        {
            webClient.BaseUrl = new Uri(domain);
            var request = new RestRequest(url, Method.GET);
            request.AddHeader("authorization", string.Format("Bearer {0}", token));
            foreach (var param in parameters)
            {
                request.AddQueryParameter(param.Name, param.Value.ToString());
            }

            var result = await webClient.ExecuteGetTaskAsync(request);
            return result;
        }

        public async Task<IRestResponse> PostAsync<T>(string domain, string url, string token, T parameters)
        {
            webClient.BaseUrl = new Uri(domain);
            var request = new RestRequest(url, Method.POST);
            request.AddHeader("x-api-key", token);
            request.AddJsonBody(parameters);

            var result = await webClient.ExecutePostTaskAsync(request);
            return result;
        }
    }
}
