using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Ploutos.AutoJobs.Models;
using RestSharp;

namespace Ploutos.AutoJobs.Services
{
    public class WebClientService : IWebClientService
    {
        private readonly IRestClient webClient;
        

        public WebClientService()
        {
            this.webClient = new RestClient { Encoding = Encoding.UTF8 };
        }

        public async Task<IRestResponse> PostAsync<T>(string domain, string url, string token, T parameters)
        {
            webClient.BaseUrl = new Uri(domain);
            var request = new RestRequest(url, Method.POST);
            request.AddHeader("x-api-key", token);
            request.AddJsonBody(parameters);

            return await webClient.ExecutePostTaskAsync(request);
        }
    }
}
