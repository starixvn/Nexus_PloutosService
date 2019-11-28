namespace Ploutos.AutoJobs.Services
{
    using Microsoft.Extensions.Options;
    using Ploutos.AutoJobs.Models;
    using RestSharp;
    using System;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;

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
