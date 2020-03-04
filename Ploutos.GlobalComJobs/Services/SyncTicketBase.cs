using Fanex.Logging;
using Ploutos.Common.Services;
using Ploutos.Common.Utils;
using Ploutos.GlobalComJobs.Models;
using Ploutos.GlobalComJobs.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

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

            var betTickets = await dataService.GetBetTickets(siteName, apiConfigs.TicketConfigs.QueryBatchSize);
            
            var newBetTickets = betTickets.Select(t =>
            {                
                t.transaction_date = t.transaction_date.AddHours(-4);
                t.settlement_date = t.settlement_date.AddHours(-4);
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
            var param = new ApiParam
            {
                game_transactions = betTickets.Select(t => new BetTicketToPush(t)).ToList(),
                aggregator_code = string.Empty,
                operator_code = apiConfigs.OperatorCode,
                provider_code = apiConfigs.ProviderCode
            };

            try
            {
                var token = await GetTokenFromSupportAPI(this.apiConfigs);
                var response = await webClientService.PostJsonAsync<ApiParam>(
                        apiConfigs.BaseDomain,
                        apiConfigs.TicketConfigs.Url,
                        token,
                        param);
                responseResult = response.Content;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var ticketResponse = this.util.ConvertJsonResponse<TicketResponse>(response.Content);
                    for (var i = 0; i < updateBetTickets.Count; i++)
                    {
                        var errorMsg = ticketResponse.GetErrorMessage();
                        updateBetTickets[i].Is_Error = !string.IsNullOrEmpty(errorMsg);
                        updateBetTickets[i].Response_Result = errorMsg;
                    }
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var ticketResponse = this.util.ConvertJsonResponse<TicketResponseBadRequest>(response.Content);

                    for (var i = 0; i < updateBetTickets.Count; i++)
                    {
                        var errorMsg = ticketResponse.GetErrorMessage(updateBetTickets[i].Transaction_Id);
                        updateBetTickets[i].Is_Error = true;
                        updateBetTickets[i].Response_Result = errorMsg;
                    }
                }
                else if(response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new HttpRequestException(response.StatusCode.ToString());
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

                fileLogger.Warn(string.Format("exception post bettickets {0} {1}", siteName, ex.Message));
                await this.logger.ErrorAsync(this.util.BuildLogMessage(ex));
            }
            var responseTime = this.util.GetCurrentTimeAtGMTMinus4();

            await this.dataService.UpdateBetTickets(siteName, pushTime, responseTime, responseResult, updateBetTickets);

            return updateBetTickets.Count(t => !t.Is_Error);
        }        
        
        private async Task<string> GetTokenFromSupportAPI(BrandApiConfigs brandApiConfigs)
        {
            var authParam = new AuthParam
            {
                response_type = "code",
                client_id = brandApiConfigs.ClientId,
                client_secret = brandApiConfigs.ClientSecret
            };

            var authResponse = await webClientService.PostFormAsync<AuthParam>(
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
