namespace Ploutos.AutoJobs.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Fanex.Logging;
    using Ploutos.AutoJobs.Models;
    using Ploutos.Common.Utils;

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
                t.TransactionDate = t.TransactionDate.AddHours(-4);
                t.SettlementDate = t.SettlementDate.AddHours(-4);
                return t;
            }).ToList();

            var betTicketBatches = newBetTickets
                    .Select((s, i) => new { Value = s, Index = i })
                    .GroupBy(x => x.Index / apiConfigs.TicketConfigs.SyncBatchSize)
                    .Select(grp => grp.Select(x => x.Value).ToList())
                    .ToList();

            var betTicketParallelBatches = betTicketBatches
                    .Select((s, i) => new { Value = s, Index = i })
                    .GroupBy(x => x.Index / apiConfigs.TicketConfigs.SyncParallelSize)
                    .Select(grp => grp.Select(x => x.Value).ToList())
                    .ToList();

            var successCount = 0;
            foreach (var batch in betTicketParallelBatches)
            {
                var tasks = new Task<int>[batch.Count];
                string domainName;
                for (int i = 0; i < batch.Count; i++)
                {
                    domainName = GetDomainName(i);
                    tasks[i] = PushTicketBatch(siteName, domainName, batch[i]);
                }

                var results = await Task.WhenAll(tasks);
                successCount += results.Sum(r => r);
            }
            
            fileLogger.Warn(string.Format("Finish Push Tickets for {0}, total: {1}, success: {2}, failed: {3}",
                siteName, betTickets.Count, successCount, betTickets.Count - successCount));
        }

        private string GetDomainName(int indexOfBatch)
        {
            string domainName;
            if (indexOfBatch % apiConfigs.TicketConfigs.SyncParallelSize == 0)
            {
                domainName = apiConfigs.PloutosBaseDomain1;
            }
            else if (((indexOfBatch % apiConfigs.TicketConfigs.SyncParallelSize) - 1) == 0)
            {
                domainName = apiConfigs.PloutosBaseDomain2;
            }
            else
            {
                domainName = apiConfigs.PloutosBaseDomain3;
            }

            return domainName;
        }

        private async Task<int> PushTicketBatch(string siteName, string domainName, List<BetTicket> betTickets)
        {
            List<UpdateBetTicket> updateBetTickets = betTickets.Select(t => new UpdateBetTicket(t)).ToList();
            var responseResult = string.Empty;
            var pushTime = this.util.GetCurrentTimeAtGMTMinus4();
            try
            {
                var response = await webClientService.PostAsync<List<BetTicket>>(
                        domainName,
                        apiConfigs.TicketConfigs.Url,
                        apiConfigs.PloutosToken,
                        betTickets);
                responseResult = response.Content;
                
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
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var ticketResponse = this.util.ConvertJsonResponse<TicketResponseBadRequest>(response.Content);

                    for (var i = 0; i < updateBetTickets.Count; i++)
                    {
                        var errorMsg = ticketResponse.GetErrorMessage(i);
                        updateBetTickets[i].IsError = true;
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
                fileLogger.Warn(string.Format("exception post bettickets {0} {1}", siteName, ex.Message));
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