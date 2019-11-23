namespace Ploutos.AutoJobs.Criteria
{
    using Fanex.Data.Repository;

    public class GetBetTicketCriteria : CriteriaBase
    {
        public string BrandCode { get; set; }
        public int TotalRows { get; set; }
        public override string GetSettingKey() => "API_BetTicket_SelPush";
        public override bool IsValid() => true;
    }
}
