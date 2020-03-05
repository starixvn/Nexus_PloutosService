using Ploutos.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ploutos.GlobalComJobs.Services.Interfaces
{
    public interface IOpal88TicketService
    {
        Task<PushResult> PushTickets();
    }
}
