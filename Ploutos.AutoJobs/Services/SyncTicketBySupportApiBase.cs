using Fanex.Logging;
using Ploutos.AutoJobs.Models;
using Ploutos.AutoJobs.Utils;
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

        public virtual async Task PushTickets()
        {
            var siteName = this.apiConfigs.BrandName;
            fileLogger.Warn(string.Format("Start Push Tickets for {0}", siteName));

            var betTickets = await dataService.GetBetTickets(siteName, apiConfigs.SupportTicketConfigs.QueryBatchSize);

            var newBetTickets = betTickets.Select(t =>
            {
                t.TransactionDate = t.TransactionDate.AddHours(-4);
                return t;
            }).ToList();

            var betTicketBatches = newBetTickets
                    .Select((s, i) => new { Value = s, Index = i })
                    .GroupBy(x => x.Index / apiConfigs.SupportTicketConfigs.SyncBatchSize)
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
            }
            var responseTime = this.util.GetCurrentTimeAtGMTMinus4();

            await this.dataService.UpdateBetTickets(siteName, pushTime, responseTime, responseResult, updateBetTickets);

            return updateBetTickets.Count(t => !t.IsError);
        }
    }
}