using Microsoft.AspNetCore.Http;

namespace MoviesApi.DTOS
{
    public class MovieCreateDto : BaseMovie
    {
        public IFormFile Poster { get; set; } = default!;

    }
}
