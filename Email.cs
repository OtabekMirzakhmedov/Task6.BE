using System.ComponentModel.DataAnnotations;

namespace Task6.BE
{
    public class Email
    {
        public int Id { get; set; }

        [Required]
        public string SenderName { get; set; }

        [Required]
        public string ReceiverName { get; set; }

        [Required]
        public string Subject { get; set; }
        
        [Required]
        public string Body { get; set; }
    }
}
