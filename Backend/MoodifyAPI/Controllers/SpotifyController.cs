using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class SpotifyController : ControllerBase
{
    private readonly SpotifyService _spotifyService;

    public SpotifyController(SpotifyService spotifyService)
    {
        _spotifyService = spotifyService;
    }

    [HttpGet("token")]
    public async Task<IActionResult> GetToken()
    {
        try
        {
            var token = await _spotifyService.GetAccessTokenAsync();
            return Ok(new { token });
        }
        catch
        {
            return StatusCode(500, "Token konnte nicht abgerufen werden.");
        }
    }
}
