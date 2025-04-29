using satelliteBackend.Models.Dto;
using System.Threading.Tasks;

public interface IUserService
{
    /// <summary>Kullanıcı kaydı için bir metot.</summary>
    Task<User> RegisterUser(UserDto userDto);

    /// <summary>Kullanıcı girişini doğrular ve User nesnesini döndürür.</summary>
    Task<User> LoginUser(UserLoginDto userLoginDto);

    /// <summary>JWT Token üretir.</summary>
    string GenerateJwtToken(User user);
}
