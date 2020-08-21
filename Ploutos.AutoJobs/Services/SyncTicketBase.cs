namespace Ploutos.AutoJobs.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Fanex.Logging;
    using Ploutos.AutoJobs.Criteria;
    using Ploutos.AutoJobs.Enums;
    using Ploutos.AutoJobs.Models;
    using Ploutos.Common.Models;
    using Ploutos.Common.Services;
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

        public virtual async Task<PushResult> PushTickets()
        {
            var siteName = this.apiConfigs.BrandName;
            var criteria = GetTicketCriteria(apiConfigs);
            var betTickets = await dataService.GetBetTickets(criteria);
            var newBetTickets = dataService.PreProcessTickets(betTickets);
            var betTicketBatches = util.SplitToBatches<BetTicket>(newBetTickets, apiConfigs.TicketConfigs.SyncBatchSize);
            var betTicketParallelBatches = util.SplitToBatches<List<BetTicket>>(betTicketBatches, apiConfigs.TicketConfigs.SyncParallelSize);

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

            return new PushResult { SuccessCount = successCount, FailedCount = betTickets.Count - successCount };
        }

        private GetBetTicketCriteria GetTicketCriteria(BrandApiConfigs apiConfigs)
        {
            return new GetBetTicketCriteria
            {
                BrandCode = apiConfigs.BrandName,
                TotalRows = apiConfigs.TicketConfigs.TotalRows,
                TotalHistoricalRows = apiConfigs.TicketConfigs.TotalHistoricalRows,
                FromNewlyCutoffDate = apiConfigs.TicketConfigs.FromNewlyCutoffDate,
                ToHistoricalDate = apiConfigs.TicketConfigs.ToHistoricalDate
            };
        }

        private string GetDomainName(int indexOfBatch)
        {
            string domainName;
            if (indexOfBatch % apiConfigs.TicketConfigs.SyncParallelSize == (int)BatchNumber.BatchZero)
            {
                domainName = apiConfigs.PloutosBaseDomain1;
            }
            else if ((indexOfBatch % apiConfigs.TicketConfigs.SyncParallelSize) == (int)BatchNumber.BatchOne)
            {
                domainName = apiConfigs.PloutosBaseDomain2;
            }
            else if ((indexOfBatch % apiConfigs.TicketConfigs.SyncParallelSize) == (int)BatchNumber.BatchTwo)
            {
                domainName = apiConfigs.PloutosBaseDomain3;
            }
            else if ((indexOfBatch % apiConfigs.TicketConfigs.SyncParallelSize) == (int)BatchNumber.BatchThree)
            {
                domainName = apiConfigs.PloutosBaseDomain4;
            }
            else
            {
                domainName = apiConfigs.PloutosBaseDomain5;
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