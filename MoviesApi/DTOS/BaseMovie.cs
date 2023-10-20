using MoviesApi.Models;
using System.ComponentModel.DataAnnotations;

namespace MoviesApi.DTOS
{
    public class BaseMovie
    {
        public string Title { get; set; } = default!;
        public int Year { get; set; }
        public double Rate { get; set; }
        [MaxLength(2500)]
        public string Storeline { get; set; } = default!;
        public int GenreId { get; set; }
    }
}
