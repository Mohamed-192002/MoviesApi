using MoviesApi.Models;
using System.ComponentModel.DataAnnotations;

namespace MoviesApi.DTOS
{
    public class MovieDetailsDto: BaseMovie
    {
        public int Id { get; set; }
        public string GenreName { get; set; } = default!;
        public byte[] Poster { get; set; } = default!;
    }
}
