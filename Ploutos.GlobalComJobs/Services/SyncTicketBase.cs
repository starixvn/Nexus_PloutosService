using Fanex.Logging;
using Ploutos.Common.Utils;
using Ploutos.GlobalComJobs.Models;
using Ploutos.GlobalComJobs.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Ploutos.GlobalComJobs.Services
{
    public class SyncTicketBase
    {
        private readonly BrandApiConfigs apiConfigs;
        private readonly IWebClientService webClientService;
        private readonly IDataService dataService;
        private readonly ILogger logger;
        private readonly IUtil util;
        private readonly NLog.Logger fileLogger = NLog.LogManager.GetCurrentClassLogger();

        public SyncTicketBase(BrandApiConfigs apiConfigs,
            IWebClientService webClientService,
            IDataService dataService,
            ILogger logger,
            IUtil util)
        {
            this.apiConfigs = apiConfigs;
            this.webClientService = webClientService;
            this.dataService = dataService;
            this.logger = logger;
            this.util = util;
        }

        public virtual async Task PushTickets()
        {
            var siteName = this.apiConfigs.BrandName;
            fileLogger.Warn(string.Format("Start Push Tickets for {0}", siteName));

            await GetTokenFromSupportAPI(this.apiConfigs);

            var betTickets = await dataService.GetBetTickets(siteName, apiConfigs.TicketConfigs.QueryBatchSize);

            var newBetTickets = betTickets.Select(t =>
            {                
                t.Transaction_Date = t.Transaction_Date.AddHours(-4);
                t.Settlement_Date = t.Settlement_Date.AddHours(-4);
                return t;
            }).ToList();

            var betTicketBatches = newBetTickets
                    .Select((s, i) => new { Value = s, Index = i })
                    .GroupBy(x => x.Index / apiConfigs.TicketConfigs.SyncBatchSize)
                    .Select(grp => grp.Select(x => x.Value).ToList())
                    .ToList();

            var successCount = 0;
            foreach (var batch in betTicketBatches)
            {
                var batchResult = await PushTicketBatch(siteName, batch);
                successCount += batchResult;
            }

            fileLogger.Warn(string.Format("Finish Push Tickets for {0}, total: {1}, success: {2}, failed: {3}",
                siteName, betTickets.Count, successCount, betTickets.Count - successCount));
        }

        private async Task<int> PushTicketBatch(string siteName, List<BetTicket> betTickets)
        {
            List<UpdateBetTicket> updateBetTickets = betTickets.Select(t => new UpdateBetTicket(t)).ToList();
            var responseResult = string.Empty;
            var pushTime = this.util.GetCurrentTimeAtGMTMinus4();

            await this.dataService.UpdateBetTickets(siteName, pushTime, this.util.GetCurrentTimeAtGMTMinus4(), responseResult, updateBetTickets);

            return updateBetTickets.Count(t => !t.Is_Error);

            try
            {
                var token = await GetTokenFromSupportAPI(this.apiConfigs);
                var response = await webClientService.PostAsync<List<BetTicket>>(
                        apiConfigs.BaseDomain,
                        apiConfigs.TicketConfigs.Url,
                        token,
                        betTickets);
                responseResult = response.Content;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var ticketResponse = this.util.ConvertJsonResponse<TicketResponse>(response.Content);
                    for (var i = 0; i < updateBetTickets.Count; i++)
                    {
                        //var errorMsg = ticketResponse.GetErrorMessage(updateBetTickets[i].TransactionNo);
                        //updateBetTickets[i].IsError = !string.IsNullOrEmpty(errorMsg);
                        //updateBetTickets[i].ResponseResult = errorMsg;
                    }
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var ticketResponse = this.util.ConvertJsonResponse<TicketResponseBadRequest>(response.Content);

                    for (var i = 0; i < updateBetTickets.Count; i++)
                    {
                        var errorMsg = ticketResponse.GetErrorMessage(i);
                        updateBetTickets[i].Is_Error = true;
                        updateBetTickets[i].Response_Result = errorMsg;
                    }
                }
                else
                {
                    throw new HttpRequestException(response.ErrorMessage ?? response.Content);
                }
            }
            catch (Exception ex)
            {
                responseResult = ex.Message;
                foreach (var updateTicket in updateBetTickets)
                {
                    updateTicket.Response_Result = string.Empty;
                    updateTicket.Is_Error = true;
                }

                await this.logger.ErrorAsync(this.util.BuildLogMessage(ex));
            }
            var responseTime = this.util.GetCurrentTimeAtGMTMinus4();

            //await this.dataService.UpdateBetTickets(siteName, pushTime, responseTime, responseResult, updateBetTickets);

            //return updateBetTickets.Count(t => !t.IsError);
        }

        private async Task<string> GetTokenFromSupportAPI2(BrandApiConfigs brandApiConfigs)
        {
            string url = string.Format("{0}{1}", brandApiConfigs.AuthDomain, brandApiConfigs.AuthConfigs.Url);
            var authParam = new AuthParam
            {
                response_type = "code",
                client_id = brandApiConfigs.ClientId,
                client_secret = brandApiConfigs.ClientSecret
            };
            string sData = webClientService.PostRequest(url, GetQueryString(authParam), 5000, null);

            return null;
        }

        private string GetQueryString(object obj)
        {
            var properties = from p in obj.GetType().GetProperties()
                             where p.GetValue(obj, null) != null
                             select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null).ToString());

            return String.Join("&", properties.ToArray());
        }

        private async Task<string> GetTokenFromSupportAPI(BrandApiConfigs brandApiConfigs)
        {
            var authParam = new AuthParam
            {
                response_type = "code",
                client_id = brandApiConfigs.ClientId,
                client_secret = brandApiConfigs.ClientSecret
            };

            var authResponse = await webClientService.PostAsync<AuthParam>(
                brandApiConfigs.AuthDomain, brandApiConfigs.AuthConfigs.Url, string.Empty, authParam);
            
            if (authResponse.StatusCode == HttpStatusCode.OK)
            {
                var authObj = this.util.ConvertJsonResponse<AuthData>(authResponse.Content);
                return authObj.access_token;                
            }

            return null;
        }
    }
}
