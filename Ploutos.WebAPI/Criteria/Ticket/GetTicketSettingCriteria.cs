using Fanex.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ploutos.WebAPI.Criteria.Ticket
{
    public class GetTicketSettingCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "SupportApi_SettingsInfoSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}