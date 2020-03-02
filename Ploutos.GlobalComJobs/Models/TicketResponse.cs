using System;
using System.Globalization;

namespace Ploutos.GlobalComJobs.Models
{
    public class TicketResponse
    {
        public string batch_transaction_id { get; set; }
        public DateTime created_date { get; set; }
        public string status { get; set; }

        public string GetErrorMessage()
        {
            return status.ToLower(CultureInfo.CurrentCulture) == "posted" ? string.Empty : "failed";
        }
    }
}
