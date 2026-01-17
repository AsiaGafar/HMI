namespace OpenMU.Shared.DTOs;

public record LoginRequest(string Username, string Password);
public record LoginResponse(bool Success, string Token, Guid UserId, string Message);
public record SendMessageRequest(string Content, string ChatType, Guid? ReceiverId = null);
public record FriendRequest(Guid FriendId);
public record ApiResponse<T>(bool Success, T? Data, string Message);
