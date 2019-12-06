namespace Ploutos.AutoJobs.Services
{
    using Fanex.Logging;
    using Ploutos.AutoJobs.Constant;
    using Ploutos.AutoJobs.Models;
    using Ploutos.AutoJobs.Services.Interfaces;
    using Ploutos.AutoJobs.Utils;
    using RestSharp;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class SupportApiService : ISupportApiService
    {
        private readonly IWebClientService webClientService;
        private readonly ILogger logger;
        private readonly IUtil util;
        
        public SupportApiService(IWebClientService webClientService, ILogger logger, IUtil util)
        {
            this.webClientService = webClientService;
            this.logger = logger;
            this.util = util;
        }

        public async Task<ProfileResponse> UpdateResponseFromSupportAPI(ProfileResponse profileResponse, string userName, SupportApiConfigs supportApiConfigs)
        {
            if (!profileResponse.IsSuccess &&
                profileResponse.Messages.Errors.Count > 0 &&
                profileResponse.Messages.Errors[0].Code == "USER-ERROR-10002" &&
                profileResponse.Messages.Errors[0].Description == "Username already in use.")
            {
                try
                {
                    var token = await GetTokenFromSupportAPI(supportApiConfigs);
                    var parameters = new List<Parameter> { new Parameter { Name = "username", Value = userName } };
                    var dataResponse = await webClientService.GetAsync(
                        supportApiConfigs.Domain, supportApiConfigs.GetUserAccountNoUrl, token, parameters);

                    if (dataResponse.StatusCode == HttpStatusCode.OK)
                    {
                        UpdateProfileResponse(profileResponse, dataResponse.Content);                       
                    }
                    else if (dataResponse.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new HttpRequestException("Unauthorized");
                    }
                    else
                    {
                        throw new HttpRequestException(dataResponse.ErrorMessage ?? dataResponse.Content);
                    }
                }
                catch (Exception ex)
                {
                    await this.logger.ErrorAsync(this.util.BuildLogMessage(ex));
                }
            }

            return profileResponse;
        }

        private void UpdateProfileResponse(ProfileResponse profileResponse, string responseContent)
        {
            if (!string.IsNullOrWhiteSpace(responseContent))
            {
                var playerCode = responseContent.Replace("\"", "");
                profileResponse.Status = Constants.StatusSuccess;
                profileResponse.Data = new ProfileResponseData { PlayerAccountNumber = playerCode };
            }
        }

        private async Task<string> GetTokenFromSupportAPI(SupportApiConfigs supportApiConfigs)
        {
            var authObj = new SupportApiAuthObj
            {
                username = supportApiConfigs.AppCredentialId,
                password = supportApiConfigs.AppCredentialPassword
            };
            var authResponse = await webClientService.PostAsync<SupportApiAuthObj>(
                supportApiConfigs.Domain, supportApiConfigs.AuthUrl, string.Empty, authObj);

            if (authResponse.StatusCode == HttpStatusCode.OK)
            {
                var token = authResponse.Content.Replace("\"", "");
                return token;
            }

            return null;
        }
    }
}
