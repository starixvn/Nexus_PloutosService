namespace Ploutos.ServiceAPI.Models
{
    public class CommandResult
    {
        public CommandResult()
        {
            Error = 1;
            Message = "Invalid data";
        }

        public int Error { get; set; }
        public string Message { get; set; }
        public bool IsSuccessful => Error == 0;
    }
}