namespace Ploutos.AutoJobs.Criteria
{
    using Fanex.Data.Repository;
    using System.Data;

    public class UpdateBetTicketCriteria : NonQueryCommand
    {
        public string BrandCode { get; set; }
        public DataTable BetTicket { get; set; }
        public int Error { get; set; }
        public string Message { get; set; }
        public override string GetSettingKey() => "API_BetTicket_Update";
        public override bool IsValid() => true;
    }
}
