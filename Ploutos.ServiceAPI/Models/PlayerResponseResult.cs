namespace Ploutos.ServiceAPI.Models
{
    public class PlayerResponseResult
    {
        public string Username { get; set; }
        public short ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsSucessfull => ErrorCode == 0;
    }
}