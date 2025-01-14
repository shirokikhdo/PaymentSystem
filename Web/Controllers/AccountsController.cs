using Application.Abstractions;
using Application.Models.Authentification;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Web.Controllers;

[Route("accounts")]
public class AccountsController : ApiBaseController
{
    private readonly IAuthService _authService;
    private readonly ILogger<AccountsController> _logger;

    public AccountsController(
        IAuthService authService,
        ILogger<AccountsController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserResponse>> Register(
        [FromBody] UserRegisterDto request)
    {
        _logger.LogInformation("Method api/accounts/register Register started." +
                               $"Request: {JsonSerializer.Serialize(request)}");

        var result = await _authService.Register(request);

        _logger.LogInformation("Method api/accounts/register Register finished." +
                               $"Request: {JsonSerializer.Serialize(request)}" +
                               $"Response: {JsonSerializer.Serialize(result)}");

        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserResponse>> Login(
        [FromBody] UserLoginDto request)
    {
        _logger.LogInformation("Method api/accounts/login Login started." +
                               $"Request: {JsonSerializer.Serialize(request)}");

        var result = await _authService.Login(request);

        _logger.LogInformation("Method api/accounts/login Login finished." +
                               $"Request: {JsonSerializer.Serialize(request)}" +
                               $"Response: {JsonSerializer.Serialize(result)}");

        return Ok(result);
    }
}