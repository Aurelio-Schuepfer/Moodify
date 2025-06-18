using MoodifyAPI.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

public class SpotifyService
{
    private readonly HttpClient _http;
    private readonly IConfiguration _config;

    public SpotifyService(HttpClient http, IConfiguration config)
    {
        _http = http;
        _config = config;
    }

    public async Task<string> GetAccessTokenAsync()
    {
        var clientId = _config["Spotify:ClientId"];
        var clientSecret = _config["Spotify:ClientSecret"];
        var auth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));

        var request = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token");
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", auth);
        request.Content = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");

        var response = await _http.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(content);
        return doc.RootElement.GetProperty("access_token").GetString();
    }

    public async Task<List<Song>> SearchSongsByMoodAsync(string mood, string token)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.spotify.com/v1/search?q={Uri.EscapeDataString(mood)}&type=track&limit=10");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _http.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(content);

        var songs = new List<Song>();

        foreach (var item in doc.RootElement.GetProperty("tracks").GetProperty("items").EnumerateArray())
        {
            var title = item.GetProperty("name").GetString();
            var artist = item.GetProperty("artists")[0].GetProperty("name").GetString();

            songs.Add(new Song
            {
                Mood = mood,
                Title = title,
                Artist = artist
            });
        }

        return songs;
    }
}
