using CustomerRewards.Auth.Dtos;
using CustomerRewards.Auth.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomerRewards.Auth.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        this.authenticationService =
            authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] UserLoginDto userLogin)
    {
        if (userLogin == null)
            return BadRequest("Invalid client request");

        var token = await authenticationService.Login(userLogin.UserName, userLogin.Password);

        return Ok(token);
    }

    [HttpPost]
    [Route("Revoke")]
    public async Task<IActionResult> Revoke([FromBody] Token token)
    {
        await authenticationService.Revoke(token.RefreshToken);

        return NoContent();
    }

    [HttpPost]
    [Route("RefreshToken")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDTO token)
    {
        if (token is null || token.RefreshToken == null)
            return BadRequest("Invalid client request");

        var newToken = await authenticationService.RefreshTokenAsync(
            token.RefreshToken,
            token.AccessToken
        );

        if (newToken is null)
        {
            return BadRequest("Invalid client request");
        }

        return new ObjectResult(newToken);
    }
}
