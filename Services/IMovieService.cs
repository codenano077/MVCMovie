using MVCMovie.Models.TMDb;

namespace MVCMovie.Services
{
    public interface IMovieService
    {
        Task<TMDbSearchResponse?> SearchResponse(string query);

        Task<TMDbMovieDetails?> GetMovieDetails(string movieId);
    }   
}