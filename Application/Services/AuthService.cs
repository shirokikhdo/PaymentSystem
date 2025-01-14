using System.IdentityModel.Tokens.Jwt;
using Application.Abstractions;
using Application.Models.Authentification;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using Domain.Models;
using Domain.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<UserEntity> _userManager;
    private readonly AuthOptions _authOptions;

    public AuthService(UserManager<UserEntity> userManager, IOptions<AuthOptions> authOptions)
    {
        _userManager = userManager;
        _authOptions = authOptions.Value;
    }

    public async Task<UserResponse> Register(UserRegisterDto userRegisterDto)
    {
        if (await _userManager.FindByEmailAsync(userRegisterDto.Email) != null)
            throw new DuplicateEntityException($"Email {userRegisterDto.Email} already exists");

        var createUserResult = await _userManager.CreateAsync(new UserEntity
        {
            Email = userRegisterDto.Email,
            PhoneNumber = userRegisterDto.Phone,
            UserName = userRegisterDto.UserName
        }, userRegisterDto.Password);

        if (!createUserResult.Succeeded)
            throw new Exception();
        
        var user = await _userManager.FindByEmailAsync(userRegisterDto.Email);

        if (user is null)
            throw new EntityNotFoundException($"User with email {userRegisterDto.Email} not registered");

        var result = await _userManager.AddToRoleAsync(user, RoleConsts.USER);

        if (!result.Succeeded)
            throw new Exception($"Errors: {string.Join(";", result.Errors.Select(x => $"{x.Code} {x.Description}"))}");
        
        var response = new UserResponse
        {
            Id = user.Id,
            Email = user.Email,
            Roles = new[] { RoleConsts.USER },
            UserName = user.UserName,
            Phone = user.PhoneNumber
        };
        return GenerateToken(response);
    }

    public async Task<UserResponse> Login(UserLoginDto userLoginDto)
    {
        var user = await _userManager.FindByEmailAsync(userLoginDto.Email);

        if (user is null)
            throw new EntityNotFoundException($"User with email {userLoginDto.Email} not found");

        var checkPasswordResult = await _userManager.CheckPasswordAsync(user, userLoginDto.Password);

        if (!checkPasswordResult) 
            throw new AuthenticationException();
        
        var userRoles = await _userManager.GetRolesAsync(user);
        
        var response = new UserResponse
        {
            Id = user.Id,
            Email = user.Email,
            Roles = userRoles.ToArray(),
            UserName = user.UserName,
            Phone = user.PhoneNumber
        };

        return GenerateToken(response);

    }

    public UserResponse GenerateToken(UserResponse userRegisterModel)
    {
        var handler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_authOptions.TokenPrivateKey);
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature);

        var claims = new Dictionary<string, object>
        {
            {ClaimTypes.Name, userRegisterModel.Email!},
            {ClaimTypes.NameIdentifier, userRegisterModel.Id.ToString()},
            {JwtRegisteredClaimNames.Aud, "test"},
            {JwtRegisteredClaimNames.Iss, "test"}
        };
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = GenerateClaims(userRegisterModel),
            Expires = DateTime.UtcNow.AddMinutes(_authOptions.ExpireIntervalMinutes),
            SigningCredentials = credentials,
            Claims = claims,
            Audience = "test",
            Issuer = "test"
        };

        var token = handler.CreateToken(tokenDescriptor);
        userRegisterModel.Token = handler.WriteToken(token);

        return userRegisterModel;
    }

    private static ClaimsIdentity GenerateClaims(UserResponse userRegisterModel)
    {
        var claims = new ClaimsIdentity();
        claims.AddClaim(new Claim(ClaimTypes.Name, userRegisterModel.Email!));
        claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, userRegisterModel.Id.ToString()));
        claims.AddClaim(new Claim(JwtRegisteredClaimNames.Aud, "test"));
        claims.AddClaim(new Claim(JwtRegisteredClaimNames.Iss, "test"));

        foreach (var role in userRegisterModel.Roles!)
            claims.AddClaim(new Claim(ClaimTypes.Role, role));

        return claims;
    }
}