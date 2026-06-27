using System.ComponentModel.DataAnnotations;

namespace MVCMovie.Models
{
    public class ImportMovieViewModel
    {
        public string? SearchString { get; set; }

        public string? ImbdId { get; set; }

        public string? Title { get; set; }

        public string? Genre { get; set; }

        public string? Rating { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public decimal? Price { get; set; }

        public string? PosterUrl { get; set; }
    }
}