namespace API.Entities;

public class Message
{
    public int MessageId { get; set; }
    public int SessionId { get; set; }
    public int SenderId { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime SentAt { get; set; }
}
