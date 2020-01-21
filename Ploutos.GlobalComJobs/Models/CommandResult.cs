using System;
using System.Collections.Generic;
using System.Text;

namespace Ploutos.GlobalComJobs.Models
{
    public class CommandResult
    {
        public CommandResult()
        {
            Error = 1;
            Message = "Invalid Data";
        }

        public int Error { get; set; }

        public string Message { get; set; }

        public bool IsSuccessful => Error == 0;
    }
}
