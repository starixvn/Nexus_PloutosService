using Fanex.Data.Repository;

namespace Ploutos.WebAPI.Criteria.Ticket
{
    public class GetDuplicateTicketDataCriteria : CriteriaBase
    {
        public string TransactionNoList { get; set; }

        public override string GetSettingKey()
        {
            return "SupportApi_BetTicketsDupSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}