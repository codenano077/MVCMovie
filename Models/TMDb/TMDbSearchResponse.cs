namespace MVCMovie.Models.TMDb
{
    public class TMDbSearchResponse
    {
        public List<TMDbMovie> Results { get; set; } = new();
    }
}