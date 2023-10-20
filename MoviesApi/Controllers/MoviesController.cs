using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MoviesApi.DTOS;
using MoviesApi.Models;
using MoviesCore.Services;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private List<string> allawExtinstians = new List<string> { ".jpg", ".png" };
        private long MaxAllowPosterSize = 1048576;
        private readonly IBaseRepository<Movie> movieRepository;
        private readonly IBaseRepository<Genre> genreRepository;
        private readonly IMapper mapper;

        public MoviesController(IMapper _mapper, IBaseRepository<Movie> _movieRepository, IBaseRepository<Genre> _genreRepository)
        {
            movieRepository = _movieRepository;
            genreRepository = _genreRepository;
            mapper = _mapper;
        }

        [HttpPost("AddMovie")]
        public async Task<IActionResult> CreateMovie([FromForm] MovieCreateDto moviesDto)
        {
            if (!allawExtinstians.Contains(Path.GetExtension(moviesDto.Poster.FileName).ToLower()))
                return BadRequest("Only .png and .jpg images are allowed!");
            if (moviesDto.Poster.Length > MaxAllowPosterSize)
                return BadRequest("Max allow size 1MB for poster!");

            var geners = await genreRepository.FindAllAsync();

            var isValidGenre = geners.Any(g => g.Id == moviesDto.GenreId);
            if (!isValidGenre)
                return BadRequest("This Genre Not Exist!");

            using var dataStream = new MemoryStream();
            await moviesDto.Poster.CopyToAsync(dataStream);

            var movie = new Movie()
            {
                Title = moviesDto.Title,
                Year = moviesDto.Year,
                Rate = moviesDto.Rate,
                Storeline = moviesDto.Storeline,
                GenreId = moviesDto.GenreId,
                Poster = dataStream.ToArray()
            };

            await movieRepository.AddAsync(movie);
            return Ok(movie);
        }

        [HttpDelete("DeleteMovie{id}")]
        public async Task<IActionResult> DeleteMovie([FromRoute] int id)
        {
            var movie = await movieRepository.FindAsync(m => m.Id == id);
            if (movie == null)
                return NotFound($"No movvie was found with ID : {id}");

            await movieRepository.DeleteAsync(movie);
            return Ok(movie);
        }

        [HttpPut("EditMovie{id}")]
        public async Task<IActionResult> UpdateMovie([FromRoute] int id, [FromForm] MovieUpdateDto moviesDto)
        {
            var movie = await movieRepository.FindAsync(m => m.Id == id, new[] { "Genre" });
            if (movie == null)
                return NotFound($"No movie was found with ID : {id}");

            if (moviesDto.Poster != null)
            {
                if (!allawExtinstians.Contains(Path.GetExtension(moviesDto.Poster.FileName).ToLower()))
                    return BadRequest("Only .png and .jpg images are allowed!");
                if (moviesDto.Poster.Length > MaxAllowPosterSize)
                    return BadRequest("Max allow size 1MB for poster!");

                using var dataStream = new MemoryStream();
                await moviesDto.Poster.CopyToAsync(dataStream);
                movie.Poster = dataStream.ToArray();
            }

            movie.Title = moviesDto.Title;
            movie.Year = moviesDto.Year;
            movie.Rate = moviesDto.Rate;
            movie.Storeline = moviesDto.Storeline;
            movie.GenreId = moviesDto.GenreId;

            await movieRepository.UpdateAsync(movie);

            return Ok(movie);
        }

        [HttpGet]
        public async Task<IActionResult> ShowAllMovies()
        {
            var movies = await movieRepository.FindAllAsync(new[] { "Genre" });
            movies.Select(m => new MovieDetailsDto
            {
                Id = m.Id,
                Title = m.Title,
                Year = m.Year,
                Rate = m.Rate,
                Storeline = m.Storeline,
                GenreId = m.GenreId,
                Poster = m.Poster,
                GenreName = m.Genre.Name
            });
            return Ok(movies);
        }
        [HttpGet("ShowById{id}")]
        public async Task<IActionResult> ShowByIdMovies(int id)
        {
            var movie = await movieRepository.FindAsync(m => m.Id == id, new[] { "Genre" });
            if (movie == null)
                return BadRequest("This movie not exit!");

            var movieDto = new MovieDetailsDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Year = movie.Year,
                Rate = movie.Rate,
                Storeline = movie.Storeline,
                GenreId = movie.GenreId,
                Poster = movie.Poster,
                GenreName = movie.Genre.Name
            };

            return Ok(movieDto);
        }
    }

}
