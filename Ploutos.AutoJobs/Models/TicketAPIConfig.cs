namespace Ploutos.AutoJobs.Models
{
    using System;

    public class TicketAPIConfig
    {
        public bool IsEnabled { get; set; }

        public string Url { get; set; }

        public int TotalRows { get; set; }

        public int TotalHistoricalRows { get; set; }

        public int SyncBatchSize { get; set; }

        public int SyncParallelSize { get; set; }

        public DateTime FromNewlyCutoffDate { get; set; }

        public DateTime ToHistoricalDate { get; set; }
    }
}