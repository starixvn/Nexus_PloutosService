using System;
using System.Globalization;

namespace Ploutos.GlobalComJobs.Models
{
    public class TicketResponse
    {
        public string Batch_transaction_id { get; set; }
        public DateTime Created_date { get; set; }
        public string Status { get; set; }

        public string GetErrorMessage()
        {
            return Status.ToLower(CultureInfo.CurrentCulture) == "posted" ? string.Empty : "failed";
        }
    }
}
