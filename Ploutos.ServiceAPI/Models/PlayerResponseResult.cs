namespace Ploutos.ServiceAPI.Models
{
    using Newtonsoft.Json;

    public class PlayerResponseResult
    {
        [JsonIgnore]
        public int RowIndex { get; set; }

        public string Username { get; set; }
        public short ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsSucessfull => ErrorCode == 0;
    }
}