namespace Ploutos.Common.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web;
    using Microsoft.Extensions.Options;
    using Ploutos.Common.Models;
    using RestSharp;

    public class WebClientService : IWebClientService
    {
        private readonly IRestClient webClient;

        public WebClientService(IRestClient webClient, IOptionsSnapshot<ProxySettings> proxySettings)
        {
            this.webClient = webClient;

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

        public async Task<IRestResponse> GetAsync(string domain, string url, List<Parameter> parameters)
        {
            webClient.BaseUrl = new Uri(domain);
            var request = new RestRequest(url, Method.GET);
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

        public async Task<IRestResponse> PostFormAsync<T>(string domain, string url, string token, T parameters)
        {
            webClient.BaseUrl = new Uri(domain);
            var request = new RestRequest(url, Method.POST);

            var paramStr = GetQueryString(parameters);

            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("application/x-www-form-urlencoded", paramStr, ParameterType.RequestBody);

            var result = await webClient.ExecutePostTaskAsync(request);
            return result;
        }

        public async Task<IRestResponse> PostJsonAsync<T>(string domain, string url, string token, T parameters)
        {
            webClient.BaseUrl = new Uri(domain);
            var request = new RestRequest(url, Method.POST);
            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddJsonBody(parameters);

            var result = await webClient.ExecutePostTaskAsync(request);
            return result;
        }

        private static string GetQueryString(object obj)
        {
            var properties = from p in obj.GetType().GetProperties()
                             where p.GetValue(obj, null) != null
                             select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null).ToString());

            return properties == null ? string.Empty : String.Join("&", properties.ToArray());
        }
    }
}
