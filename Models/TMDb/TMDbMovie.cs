using System.Text.Json.Serialization;

namespace MVCMovie.Models.TMDb
{
    public class TMDbMovie
    {
        public int Id { get; set; }

        public string? Title { get; set; }
        [JsonPropertyName("release_date")]
        public string? ReleaseDate { get; set; }

        [JsonPropertyName("poster_path")]
        public string? PosterPath { get; set; }
    }
}