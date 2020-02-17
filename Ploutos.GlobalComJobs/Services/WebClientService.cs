using Microsoft.Extensions.Options;
using Ploutos.GlobalComJobs.Models;
using Ploutos.GlobalComJobs.Services.Interfaces;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
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
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls11;

            webClient.BaseUrl = new Uri(domain);
            var request = new RestRequest(url, Method.POST);
            request.AddHeader("x-api-key", token);
            request.AddJsonBody(parameters);

            


            var result = await webClient.ExecutePostTaskAsync(request);
            return result;
        }

        public bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors) { return true; }


        public string PostRequest(string url, string postData, int timeout, string Access_Token)
        {
            if (System.Net.ServicePointManager.DefaultConnectionLimit < 100)
                System.Net.ServicePointManager.DefaultConnectionLimit = 100;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

            req.ContentType = Access_Token == null ? "application/x-www-form-urlencoded" : "application/json";
            if (Access_Token != null)
            {
                req.Headers.Add("Authorization", "Bearer " + Access_Token);
            }

            req.Method = "POST";
            if (timeout > 0)
                req.Timeout = timeout;

            req.KeepAlive = false;

            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls; //)48 | (SecurityProtocolType)192 | (SecurityProtocolType)768 | (SecurityProtocolType)3072;
                ServicePointManager.ServerCertificateValidationCallback = CheckValidationResult;
            }

            req.Proxy = new WebProxy("10.3.252.100", 8801);

            string result_Content = "";
            try
            {
                using (StreamWriter sw = new StreamWriter(req.GetRequestStream()))
                {
                    sw.Write(postData);
                    sw.Close();
                }

                using (WebResponse resp = req.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(resp.GetResponseStream(), System.Text.Encoding.UTF8))
                    {
                        result_Content = sr.ReadToEnd();
                        sr.Close();
                    }
                    resp.Close();
                }

            }
            catch (WebException wEx)
            {
                NLog.Logger WebException_PostRequest = NLog.LogManager.GetLogger("_WebException_log");
                // wEx.Response.GetResponseStream();
                // NLog.Logger webExpectationlogger = NLog.LogManager.GetLogger(siteNamelog + "web_exception_log");
                WebException_PostRequest.Error(wEx.Status);
                WebException_PostRequest.Error(wEx.Message);

                if (wEx.Response != null)
                {
                    Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                    //return "{\"status\":\"unable_to_process_request\"}";
                    StreamReader readStream = new StreamReader(wEx.Response.GetResponseStream(), encode);
                    //WebException_PostRequest.Error(readStream.ReadToEnd());
                    return readStream.ReadToEnd();
                }
                throw wEx;
            }
            catch (Exception e)
            {
                NLog.Logger PostRequest = NLog.LogManager.GetLogger("_PostRequest_log");
                PostRequest.Error(e);
                throw e;
            }
            return result_Content;

        }
    }
}
