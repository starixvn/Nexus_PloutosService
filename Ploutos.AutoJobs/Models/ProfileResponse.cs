using Ploutos.AutoJobs.Constant;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ploutos.AutoJobs.Models
{
    public class ProfileResponse
    {
        public string Status { get; set; }
        public ProfileResponseData Data { get; set; }
        public ResponseMessage Messages { get; set; }
        public bool IsSuccess {
            get
            {
                return Status == Constants.StatusSuccess;
            }
        }
    }
}
