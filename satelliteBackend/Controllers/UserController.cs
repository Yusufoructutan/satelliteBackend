using Microsoft.AspNetCore.Mvc;
using satelliteBackend.Models.Dto;
using System;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService; 

    public UserController(IUserService userService) // Constructor'da IUserService alıyoruz
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserDto userDto)
    {
        try
        {
            var user = await _userService.RegisterUser(userDto);
            return Ok(new { message = "Kullanıcı başarıyla kaydedildi!", userId = user.UserId });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message }); // Hata mesajını daha iyi bir yapıda döndürüyoruz
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginDto userLoginDto)
    {
        var user = await _userService.LoginUser(userLoginDto);
        var token = _userService.GenerateJwtToken(user);
        return Ok(new { Token = token });
    }

}
