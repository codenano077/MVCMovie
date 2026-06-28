using System.Text.Json;
using MVCMovie.Models.TMDb; 

namespace MVCMovie.Services
{
    public class TMDBMovieService : IMovieService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public TMDBMovieService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

public async Task<TMDbSearchResponse?> SearchMoviesAsync(string query)
{
    var apiKey = _configuration["TMDb:ApiKey"];

    var url =
        $"https://api.themoviedb.org/3/search/movie?api_key={apiKey}&query={Uri.EscapeDataString(query)}";

    Console.WriteLine($"URL     : {url}");
    using var client = new HttpClient();

    client.DefaultRequestHeaders.Add("User-Agent", "MVCMovie");

    var json = await client.GetStringAsync(url);

    return JsonSerializer.Deserialize<TMDbSearchResponse>(
        json,
        new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
}

        public async Task<TMDbMovieDetails?> GetMovieDetailsAsync(int movieId)
        {
            var apiKey = _configuration["TMDb:ApiKey"];

            var url =
                $"https://api.themoviedb.org/3/movie/{movieId}?api_key={apiKey}";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<TMDbMovieDetails>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
        }
    }
    
}