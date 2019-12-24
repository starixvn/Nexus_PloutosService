namespace Ploutos.AutoJobs.Models
{
    using System.Collections.Generic;
    using System.Linq;

    public class TicketResponse
    {
        public List<TicketResponseData> Errors { get; set; }

        public string GetErrorMessage(string TransactionNo)
        {
            var errors = Errors.Where(t => t.TransactionNo == TransactionNo).Select(t => t.ErroMessage);
            return string.Join(",", errors);
        }
    }
}
