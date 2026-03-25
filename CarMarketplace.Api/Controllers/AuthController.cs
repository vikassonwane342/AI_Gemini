using System.Diagnostics;
using CarMarketplace.Api.Common;
using CarMarketplace.Api.Dtos;
using CarMarketplace.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarMarketplace.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService, ICurrentUserService currentUserService) : ControllerBase
{
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<UserResponseDto>>> Register(RegisterRequestDto request, CancellationToken cancellationToken)
    {
        var sw = Stopwatch.StartNew();
        var user = await authService.RegisterAsync(request, cancellationToken);
        sw.Stop();
        Console.WriteLine($"[AuthController.Register] Execution Time: {sw.ElapsedMilliseconds}ms");
        return Created(string.Empty, ApiResponse<UserResponseDto>.SuccessResponse(user, "User registered successfully."));
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Login(LoginRequestDto request, CancellationToken cancellationToken)
    {
        var sw = Stopwatch.StartNew();
        var response = await authService.LoginAsync(request, cancellationToken);
        sw.Stop();
        Console.WriteLine($"[AuthController.Login] Execution Time: {sw.ElapsedMilliseconds}ms");
        return Ok(ApiResponse<AuthResponseDto>.SuccessResponse(response, "Login successful."));
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<UserResponseDto>>> Me(CancellationToken cancellationToken)
    {
        var sw = Stopwatch.StartNew();
        var user = await authService.GetCurrentUserAsync(currentUserService.UserId, cancellationToken);
        sw.Stop();
        Console.WriteLine($"[AuthController.Me] Execution Time: {sw.ElapsedMilliseconds}ms");
        return Ok(ApiResponse<UserResponseDto>.SuccessResponse(user));
    }
}
