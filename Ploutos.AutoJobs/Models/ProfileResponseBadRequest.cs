namespace Ploutos.AutoJobs.Models
{
    using System.Collections.Generic;
    using System.Linq;

    public class ProfileResponseBadRequest
    {
        public string Message { get; set; }
        public Dictionary<string, string[]> ModelState { get; set; }

        public string GetErrorMessage()
        {
            var errors = new List<string>();
            foreach (KeyValuePair<string, string[]> item in ModelState)
            {
                errors.AddRange(item.Value.ToList());
            }

            return string.Join(",", errors);
        }
    }
}