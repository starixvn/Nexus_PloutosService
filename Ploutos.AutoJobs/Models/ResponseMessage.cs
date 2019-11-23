namespace Ploutos.AutoJobs.Models
{
    using System.Collections.Generic;

    public class ResponseMessage
    {
        public List<ResponseError> Errors { get; set; }
    }
}
