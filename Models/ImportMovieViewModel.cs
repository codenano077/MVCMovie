using System.ComponentModel.DataAnnotations;

namespace MVCMovie.Models
{
    public class ImportMovieViewModel
    {
        public string? SearchString { get; set; }

        public string? ImbdId { get; set; }

        [Required]
        public string? Title { get; set; }

        [Required]
        public string? Genre { get; set; }

        [Required]
        public string? Rating { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }

        [Required]
        [Range(0.01, 1000)]
        public decimal Price { get; set; }

        public string? PosterUrl { get; set; }
    }
}