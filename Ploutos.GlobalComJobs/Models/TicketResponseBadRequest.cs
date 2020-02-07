using Ploutos.GlobalComJobs.Constant;
using System.Collections.Generic;
using System.Linq;

namespace Ploutos.GlobalComJobs.Models
{
    public class TicketResponseBadRequest
    {
        public string Status { get; set; }

        public Dictionary<string, string[]> ModelState { get; set; }

        public string GetErrorMessage(int index)
        {
            if (Status == Constants.StatusSuccess)
            {
                return string.Empty;
            }

            var errors = new List<string>();
            foreach (KeyValuePair<string, string[]> item in ModelState)
            {
                if (item.Key.Contains(string.Format("{0}", index)))
                {
                    errors.AddRange(item.Value.ToList());
                }
            }

            return string.Join(",", errors);
        }
    }
}
