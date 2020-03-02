using Microsoft.Extensions.Options;
using Ploutos.Common.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Ploutos.Common.Services
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

        private string GetQueryString(object obj)
        {
            var properties = from p in obj.GetType().GetProperties()
                             where p.GetValue(obj, null) != null
                             select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null).ToString());

            return String.Join("&", properties.ToArray());
        }
    }
}
