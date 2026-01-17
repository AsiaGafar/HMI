using Microsoft.AspNetCore.Mvc;
using OpenMU.Shared.DTOs;
using OpenMU.Shared.Models;

namespace ChatService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private static readonly List<ChatMessage> Messages = new();

    [HttpPost("send")]
    public async Task<ActionResult<ApiResponse<ChatMessage>>> SendMessage([FromBody] SendMessageRequest request, [FromQuery] Guid senderId)
    {
        var userResponse = await GetUserFromAuthService(senderId);
        if (!userResponse.Success)
            return BadRequest(new ApiResponse<ChatMessage>(false, null, "User not found"));

        var message = new ChatMessage
        {
            Id = Guid.NewGuid(),
            SenderId = senderId,
            SenderName = userResponse.Data?.Username ?? "Unknown",
            Content = request.Content,
            Timestamp = DateTime.UtcNow,
            Type = Enum.Parse<ChatType>(request.ChatType)
        };

        Messages.Add(message);
        return Ok(new ApiResponse<ChatMessage>(true, message, "Message sent"));
    }

    [HttpGet("messages")]
    public ActionResult<ApiResponse<List<ChatMessage>>> GetMessages([FromQuery] string type = "Global")
    {
        var chatType = Enum.Parse<ChatType>(type);
        var messages = Messages.Where(m => m.Type == chatType).OrderByDescending(m => m.Timestamp).Take(50).ToList();
        return Ok(new ApiResponse<List<ChatMessage>>(true, messages, $"Retrieved {messages.Count} messages"));
    }

    private async Task<ApiResponse<User>> GetUserFromAuthService(Guid userId)
    {
        using var client = new HttpClient();
        var response = await client.GetAsync($"http://localhost:5001/api/auth/users/{userId}");
        return await response.Content.ReadFromJsonAsync<ApiResponse<User>>() 
               ?? new ApiResponse<User>(false, null, "Failed to fetch user");
    }
}
