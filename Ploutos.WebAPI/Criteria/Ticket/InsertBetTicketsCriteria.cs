using Fanex.Data.Repository;

namespace Ploutos.WebAPI.Criteria.Ticket
{
    public class InsertBetTicketsCriteria : NonQueryCommand
    {
        public string InsertCommand { get; set; }
        public int Error { get; set; }
        public string Message { get; set; }
        public override string GetSettingKey() => "SupportApi_BetTicketsIns";
        public override bool IsValid() => true;
    }
}