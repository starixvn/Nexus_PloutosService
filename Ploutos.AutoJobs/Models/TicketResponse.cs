namespace Ploutos.AutoJobs.Models
{
    using System.Collections.Generic;
    using System.Linq;

    public class TicketResponse
    {
        public string Status { get; set; }
        public List<TicketResponseData> Data { get; set; }

        public string GetErrorMessage(string TransactionNo)
        {
            var errors = Data.Where(t => t.TransactionNo == TransactionNo).Select(t => t.ErroMessage);
            return string.Join(",", errors);
        }
    }
}
