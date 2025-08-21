using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IJwtTokenService _jwtTokenService;
    public UserController(IUserService userService, IJwtTokenService jwtTokenService)
    {
        _userService = userService;
        _jwtTokenService = jwtTokenService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(Users user)
    {
        var createdUser = await _userService.RegisterUser(user);

        return Ok(createdUser);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto login)
    {
        var result = await _userService.AuthenticateUserAsync(login.Email, login.Password, login.CompanyId);
        var user = result.User;
        if (!result.IsSuccess)
        {
            return Unauthorized(result.Message);
        }
        var token = _jwtTokenService.GenerateToken(user.Email, user.Role, user.CompanyUniqueId);
        return Ok(new { token });
    }
        
    }
