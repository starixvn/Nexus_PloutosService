namespace Ploutos.AutoJobs.Models
{
    using System.Collections.Generic;
    using MimeKit;

    public class MailModel
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public string ReceiverEmail { get; set; }
        public IEnumerable<string> MailCc { get; set; } = new List<string>();
        public AttachmentCollection Attachments { get; set; } = new AttachmentCollection();
    }
}
