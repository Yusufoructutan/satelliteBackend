using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyAspNetCoreProject.Data;
using satelliteBackend.Models.Dto;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public UserService(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<User> RegisterUser(UserDto userDto)
    {
        if (_context.Users.Any(u => u.Username == userDto.Username))
        {
            throw new Exception("Kullanıcı adı zaten mevcut.");
        }

        using var hmac = new HMACSHA512();
        var user = new User
        {
            Username = userDto.Username,
            PasswordHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(userDto.Password))),
            PasswordSalt = Convert.ToBase64String(hmac.Key),
            Email = userDto.Email
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<User> LoginUser(UserLoginDto userLoginDto)
    {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == userLoginDto.Username);
        if (user == null || !VerifyPasswordHash(userLoginDto.Password, user.PasswordHash, user.PasswordSalt))
        {
            throw new Exception("Giriş bilgileri hatalı veya kullanıcı mevcut değil.");
        }

        return user;
    }

    private bool VerifyPasswordHash(string password, string storedHash, string storedSalt)
    {
        using var hmac = new HMACSHA512(Convert.FromBase64String(storedSalt));
        var computedHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
        return computedHash == storedHash;
    }

    public string GenerateJwtToken(User user)
    {
        var key = _configuration["Jwt:Key"];
        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];

        if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
        {
            throw new ArgumentNullException("JWT ayarlarından biri boş.");
        }

        var expiryInMinutes = Convert.ToInt32(_configuration["Jwt:ExpiryInMinutes"]);
        var symKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var creds = new SigningCredentials(symKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            expires: DateTime.Now.AddMinutes(expiryInMinutes),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
