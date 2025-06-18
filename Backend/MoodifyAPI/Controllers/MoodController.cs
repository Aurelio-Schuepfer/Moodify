using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class MoodController : ControllerBase
{
    private readonly SpotifyService _spotifyService;

    public MoodController(SpotifyService spotifyService)
    {
        _spotifyService = spotifyService;
    }

    [HttpGet("{mood}")]
    public async Task<IActionResult> GetSongsByMood(string mood, [FromHeader(Name = "Authorization")] string authHeader)
    {
        if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer "))
            return BadRequest("Token fehlt oder ist ungültig.");

        var token = authHeader.Substring("Bearer ".Length).Trim();

        try
        {
            var songs = await _spotifyService.SearchSongsByMoodAsync(mood, token);
            return Ok(songs);
        }
        catch
        {
            return StatusCode(500, "Songs konnten nicht geladen werden.");
        }
    }
}
