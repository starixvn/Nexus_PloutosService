using Fanex.Data.Repository;

namespace Ploutos.GlobalComJobs.Criteria
{
    public class GetBetTicketCriteria : CriteriaBase
    {
        public string BrandCode { get; set; }
        public int TotalRows { get; set; }
        public override string GetSettingKey() => "Plo_API_BetTicket_SelPush";
        public override bool IsValid() => true;
    }
}
