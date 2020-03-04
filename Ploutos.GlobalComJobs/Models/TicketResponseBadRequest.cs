using System.Collections.Generic;
using System.Linq;

namespace Ploutos.GlobalComJobs.Models
{
    public class TicketResponseBadRequest
    {
        public List<BadRequestObject> Errors { get; set; }

        public string GetErrorMessage(string transactionId)
        {
            var item = Errors.FirstOrDefault(t => t.RejectedValue.transaction_id == transactionId);
            return item == null ? string.Empty : item.Message;
        }
    }
}
