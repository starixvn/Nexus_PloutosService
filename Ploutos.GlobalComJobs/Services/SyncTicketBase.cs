using Fanex.Logging;
using Ploutos.Common.Models;
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
        private readonly NLog.Logger fileLogger = NLog.LogManager.GetCurrentClassLogger();
        private readonly BrandApiConfigs apiConfigs;
        private readonly IWebClientService webClientService;
        private readonly IDataService dataService;
        private readonly ILogger logger;
        private readonly IUtil util;        

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

        public virtual async Task<PushResult> PushTickets()
        {
            //var betTickets = await dataService.GetBetTickets(apiConfigs.BrandName, apiConfigs.TicketConfigs.QueryBatchSize);
            var betTickets = new List<BetTicket>
            {
                new BetTicket {
                    transaction_date = DateTime.Now,
                    settlement_date = DateTime.Now,
                    player_no = "test pn1",
                    player_name = "test p1",
                    player_type = "Test",
                    transaction_id = "test id",
                    brand = "test brand",
                    brand_url = "test brand url",
                    game_code = 1,
                    game_name = "test game name" ,
                    game_type = "sb",
                    currency_code = "THB",
                    bet_type = "Handicap",
                    bet_amount = 100,
                    debit_amount = 100,
                    credit_amount = 100,
                    net_amount = 100
                }
            };

            var newBetTickets = dataService.PreProcessTickets(betTickets);            
            var betTicketBatches = util.SplitToBatches(newBetTickets, apiConfigs.TicketConfigs.SyncBatchSize);

            var successCount = 0;
            foreach (var batch in betTicketBatches)
            {
                successCount += await PushTicketBatch(apiConfigs.BrandName, batch);                
            }

            return new PushResult { SuccessCount = successCount, FailedCount = betTickets.Count - successCount };
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
                var token = await GetAuthorizedToken(this.apiConfigs);
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

            //await this.dataService.UpdateBetTickets(siteName, pushTime, responseTime, responseResult, updateBetTickets);

            return updateBetTickets.Count(t => !t.Is_Error);
        }        
        
        private async Task<string> GetAuthorizedToken(BrandApiConfigs brandApiConfigs)
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
