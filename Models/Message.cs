namespace Catering.Models
{
    public class Message
    {
        public int MessageId { get; set; }
        public string FromUserId { get; set; }
        public ApplicationUser FromUser { get; set; }
        public string ToUserId { get; set; }
        public ApplicationUser ToUser { get; set; }

        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime SentDate { get; set; }
    }
}