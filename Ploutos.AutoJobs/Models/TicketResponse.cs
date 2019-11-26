using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploutos.AutoJobs.Models
{
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
