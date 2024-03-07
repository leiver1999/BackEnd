namespace ProjectAPI.Models
{
    public class EmailDto
    {
        public string To { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body {get; set; } = string.Empty;

        public EmailDto(string To, string subject, string body) 
        {
            this.To = To;  
            this.Subject = subject;
            this.Body = body;
        }  
    }
}
