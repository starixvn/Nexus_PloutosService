using System.Collections.Generic;

namespace Ploutos.WebAPI.Models
{
    public class TicketResponse
    {
        public string Status { get; set; }
        public List<TicketResponseData> Data { get; set; }
    }
}