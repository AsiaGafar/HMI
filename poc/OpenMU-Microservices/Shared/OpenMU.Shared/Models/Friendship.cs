namespace OpenMU.Shared.Models;

public class Friendship
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid FriendId { get; set; }
    public DateTime CreatedAt { get; set; }
    public FriendshipStatus Status { get; set; }
}

public enum FriendshipStatus
{
    Pending,
    Accepted,
    Blocked
}
