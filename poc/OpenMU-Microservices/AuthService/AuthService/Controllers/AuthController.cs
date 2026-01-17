using Microsoft.AspNetCore.Mvc;
using OpenMU.Shared.DTOs;
using OpenMU.Shared.Models;

namespace AuthService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private static readonly List<User> Users = new()
    {
        new User { Id = Guid.NewGuid(), Username = "admin", Email = "admin@openmu.com", CreatedAt = DateTime.UtcNow }
    };

    [HttpPost("login")]
    public ActionResult<LoginResponse> Login([FromBody] LoginRequest request)
    {
        var user = Users.FirstOrDefault(u => u.Username == request.Username);
        
        if (user == null)
            return Ok(new LoginResponse(false, string.Empty, Guid.Empty, "Invalid credentials"));

        var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        user.IsOnline = true;
        
        return Ok(new LoginResponse(true, token, user.Id, "Login successful"));
    }

    [HttpPost("logout")]
    public ActionResult<ApiResponse<bool>> Logout([FromQuery] Guid userId)
    {
        var user = Users.FirstOrDefault(u => u.Id == userId);
        if (user != null) user.IsOnline = false;
        
        return Ok(new ApiResponse<bool>(true, true, "Logout successful"));
    }

    [HttpGet("users/{id}")]
    public ActionResult<ApiResponse<User>> GetUser(Guid id)
    {
        var user = Users.FirstOrDefault(u => u.Id == id);
        return user == null 
            ? Ok(new ApiResponse<User>(false, null, "User not found"))
            : Ok(new ApiResponse<User>(true, user, "User found"));
    }
}
