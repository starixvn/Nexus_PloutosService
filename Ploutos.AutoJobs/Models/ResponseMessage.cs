using System;
using System.Collections.Generic;
using System.Text;

namespace Ploutos.AutoJobs.Models
{
    public class ResponseMessage
    {
        public List<ResponseError> Errors { get; set; }
    }
}
