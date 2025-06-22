using Events_system.DTOs;
using Microsoft.AspNetCore.Mvc;
using Events_system.BusinessServices.BusinessInterfaces;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthenticationService _svc;

    public AuthController(IAuthenticationService svc) => _svc = svc;

    [HttpPost("register")]
    public async Task<ActionResult<AuthResultDTO>> Register(AuthRegisterDTO dto)
        => Ok(await _svc.RegisterAsync(dto));

    [HttpPost("login")]
    public async Task<ActionResult<AuthResultDTO>> Login(AuthLoginDTO dto)
        => Ok(await _svc.LoginAsync(dto));

}