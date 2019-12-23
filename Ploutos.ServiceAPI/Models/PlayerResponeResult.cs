namespace Ploutos.ServiceAPI.Models
{
    public class PlayerResponeResult
    {
        public string Username { get; set; }

        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public bool IsSucessfull
        {
            get
            {
                return ErrorCode == 0;
            }
        }
    }
}
