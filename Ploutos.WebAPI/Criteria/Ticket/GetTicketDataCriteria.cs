using Fanex.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ploutos.WebAPI.Criteria.Ticket
{
    public class GetTicketDataCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "SupportApi_BetTicketsInfoSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}