using MVCMovie.Models.TMDb;

namespace MVCMovie.Services
{
    public interface IMovieService
    {
        Task<TMDbSearchResponse?> SearchMoviesAsync(string query);

        Task<TMDbMovieDetails?> GetMovieDetailsAsync(int movieId);
    }   
}