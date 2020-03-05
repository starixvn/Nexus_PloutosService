using Fanex.Logging;
using Ploutos.AutoJobs.Models;
using Ploutos.Common.Models;
using Ploutos.Common.Services;
using Ploutos.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Ploutos.AutoJobs.Services
{
    public class SyncTicketBySupportApiBase
    {
        private readonly BrandApiConfigs apiConfigs;
        private readonly IWebClientService webClientService;
        private readonly IDataService dataService;
        private readonly ILogger logger;
        private readonly IUtil util;
        private readonly NLog.Logger fileLogger = NLog.LogManager.GetCurrentClassLogger();

        public SyncTicketBySupportApiBase(BrandApiConfigs apiConfigs,
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
            var siteName = this.apiConfigs.BrandName;
            var betTickets = await dataService.GetBetTickets(siteName, apiConfigs.SupportTicketConfigs.QueryBatchSize);
            var newBetTickets = dataService.PreProcessTickets(betTickets);
            var betTicketBatches = util.SplitToBatches<BetTicket>(newBetTickets, apiConfigs.SupportTicketConfigs.SyncBatchSize);
            
            var successCount = 0;
            foreach (var batch in betTicketBatches)
            {
                successCount += await PushTicketBatch(siteName, batch);                
            }

            return new PushResult { SuccessCount = successCount, FailedCount = betTickets.Count - successCount };
        }

        private async Task<int> PushTicketBatch(string siteName, List<BetTicket> betTickets)
        {
            List<UpdateBetTicket> updateBetTickets = betTickets.Select(t => new UpdateBetTicket(t)).ToList();
            var responseResult = string.Empty;
            var pushTime = this.util.GetCurrentTimeAtGMTMinus4();
            try
            {
                var response = await webClientService.PostAsync<List<BetTicket>>(
                        apiConfigs.SupportApiConfigs.Domain,
                        apiConfigs.SupportTicketConfigs.Url,
                        string.Empty,
                        betTickets);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var ticketResponse = this.util.ConvertJsonResponse<TicketResponse>(response.Content);
                    for (var i = 0; i < updateBetTickets.Count; i++)
                    {
                        var errorMsg = ticketResponse.GetErrorMessage(updateBetTickets[i].TransactionNo);
                        updateBetTickets[i].IsError = !string.IsNullOrEmpty(errorMsg);
                        updateBetTickets[i].ResponseResult = errorMsg;
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
                    updateTicket.ResponseResult = string.Empty;
                    updateTicket.IsError = true;
                }

                await this.logger.ErrorAsync(this.util.BuildLogMessage(ex));
                fileLogger.Warn(string.Format("exception post bettickets by support api {0} {1}", siteName, ex.Message));
            }
            var responseTime = this.util.GetCurrentTimeAtGMTMinus4();

            await this.dataService.UpdateBetTickets(siteName, pushTime, responseTime, responseResult, updateBetTickets);

            return updateBetTickets.Count(t => !t.IsError);
        }
    }
}