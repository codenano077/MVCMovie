using System.Text.Json.Serialization;

namespace MVCMovie.Models.TMDb
{
    public class TMDbMovieDetails
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        [JsonPropertyName("poster_path")]
        public string? PosterPath { get; set; }

        [JsonPropertyName("release_date")]
        public string? ReleaseDate { get; set; }

        [JsonPropertyName("vote_average")]
        public double VoteAverage { get; set; }

        public List<Genre> Genres { get; set; } = new();
    }

    public class Genre
    {
        public int Id { get; set; }

        public string? Name { get; set; }
    }
}