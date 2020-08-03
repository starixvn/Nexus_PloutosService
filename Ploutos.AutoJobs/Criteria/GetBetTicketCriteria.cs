namespace Ploutos.AutoJobs.Criteria
{
    using Fanex.Data.Repository;
    using System;

    public class GetBetTicketCriteria : CriteriaBase
    {
        public string BrandCode { get; set; }

        public int TotalRows { get; set; }

        public int TotalHistoricalRows { get; set; }

        public DateTime FromNewlyCutoffDate { get; set; }

        public DateTime ToHistoricalDate { get; set; }

        public override string GetSettingKey() => "Plo_API_BetTicket_SelPush";

        public override bool IsValid() => !string.IsNullOrWhiteSpace(BrandCode) && FromNewlyCutoffDate > ToHistoricalDate;
    }
}
