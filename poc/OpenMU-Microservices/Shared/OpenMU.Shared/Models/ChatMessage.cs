namespace OpenMU.Shared.Models;

public class ChatMessage
{
    public Guid Id { get; set; }
    public Guid SenderId { get; set; }
    public string SenderName { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public ChatType Type { get; set; }
}

public enum ChatType
{
    Global,
    Guild,
    Private
}
