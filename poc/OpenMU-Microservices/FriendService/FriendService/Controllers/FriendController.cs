using Microsoft.AspNetCore.Mvc;
using OpenMU.Shared.DTOs;
using OpenMU.Shared.Models;

namespace FriendService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FriendController : ControllerBase
{
    private static readonly List<Friendship> Friendships = new();

    [HttpPost("request")]
    public async Task<ActionResult<ApiResponse<Friendship>>> SendFriendRequest([FromBody] FriendRequest request, [FromQuery] Guid userId)
    {
        var userResponse = await GetUserFromAuthService(userId);
        var friendResponse = await GetUserFromAuthService(request.FriendId);

        if (!userResponse.Success || !friendResponse.Success)
            return BadRequest(new ApiResponse<Friendship>(false, null, "User not found"));

        var friendship = new Friendship
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            FriendId = request.FriendId,
            CreatedAt = DateTime.UtcNow,
            Status = FriendshipStatus.Pending
        };

        Friendships.Add(friendship);
        return Ok(new ApiResponse<Friendship>(true, friendship, "Friend request sent"));
    }

    [HttpPost("accept/{friendshipId}")]
    public ActionResult<ApiResponse<Friendship>> AcceptFriendRequest(Guid friendshipId)
    {
        var friendship = Friendships.FirstOrDefault(f => f.Id == friendshipId);
        if (friendship == null)
            return NotFound(new ApiResponse<Friendship>(false, null, "Friendship not found"));

        friendship.Status = FriendshipStatus.Accepted;
        return Ok(new ApiResponse<Friendship>(true, friendship, "Friend request accepted"));
    }

    [HttpGet("list")]
    public ActionResult<ApiResponse<List<Friendship>>> GetFriends([FromQuery] Guid userId)
    {
        var friends = Friendships.Where(f => 
            (f.UserId == userId || f.FriendId == userId) && 
            f.Status == FriendshipStatus.Accepted).ToList();
        
        return Ok(new ApiResponse<List<Friendship>>(true, friends, $"Retrieved {friends.Count} friends"));
    }

    private async Task<ApiResponse<User>> GetUserFromAuthService(Guid userId)
    {
        using var client = new HttpClient();
        var response = await client.GetAsync($"http://localhost:5001/api/auth/users/{userId}");
        return await response.Content.ReadFromJsonAsync<ApiResponse<User>>() 
               ?? new ApiResponse<User>(false, null, "Failed to fetch user");
    }
}
