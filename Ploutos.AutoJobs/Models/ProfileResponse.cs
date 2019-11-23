namespace Ploutos.AutoJobs.Models
{
    using Ploutos.AutoJobs.Constant;

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
