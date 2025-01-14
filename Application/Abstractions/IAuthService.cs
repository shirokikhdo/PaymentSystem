using Application.Models.Authentification;

namespace Application.Abstractions;

public interface IAuthService
{
    Task<UserResponse> Register(UserRegisterDto  userRegisterDto);

    Task<UserResponse> Login(UserLoginDto  userLoginDto);
}