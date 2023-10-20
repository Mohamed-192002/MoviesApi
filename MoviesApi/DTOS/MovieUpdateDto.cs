namespace MoviesApi.DTOS
{
    public class MovieUpdateDto: BaseMovie
    {
        public IFormFile? Poster { get; set; } = default!;

    }
}
