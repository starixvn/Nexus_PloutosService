namespace Ploutos.Common.Models
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
    }
}
