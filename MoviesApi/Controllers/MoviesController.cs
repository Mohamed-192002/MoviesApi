using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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

        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;

        public MoviesController(IMapper _mapper, IUnitOfWork _unitOfWork)
        {
            mapper = _mapper;
            unitOfWork = _unitOfWork;
        }

        [HttpPost("AddMovie")]
        public async Task<IActionResult> CreateMovie([FromForm] MovieCreateDto moviesDto)
        {
            if (!allawExtinstians.Contains(Path.GetExtension(moviesDto.Poster.FileName).ToLower()))
                return BadRequest("Only .png and .jpg images are allowed!");
            if (moviesDto.Poster.Length > MaxAllowPosterSize)
                return BadRequest("Max allow size 1MB for poster!");

            var geners = await unitOfWork.Genres.FindAllAsync();

            var isValidGenre = geners.Any(g => g.Id == moviesDto.GenreId);
            if (!isValidGenre)
                return BadRequest("This Genre Not Exist!");

            using var dataStream = new MemoryStream();
            await moviesDto.Poster.CopyToAsync(dataStream);

            var movie =mapper.Map<Movie>(moviesDto);
            movie.Poster= dataStream.ToArray();

            await unitOfWork.Movies.AddAsync(movie);
            return Ok(movie);
        }

        [HttpDelete("DeleteMovie{id}")]
        public async Task<IActionResult> DeleteMovie([FromRoute] int id)
        {
            var movie = await unitOfWork.Movies.FindAsync(m => m.Id == id);
            if (movie == null)
                return NotFound($"No movvie was found with ID : {id}");

            await unitOfWork.Movies.DeleteAsync(movie);
            return Ok(movie);
        }

        [HttpPut("EditMovie{id}")]
        public async Task<IActionResult> UpdateMovie([FromRoute] int id, [FromForm] MovieUpdateDto moviesDto)
        {
            var movie = await unitOfWork.Movies.FindAsync(m => m.Id == id, new[] { "Genre" });
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

            await unitOfWork.Movies.UpdateAsync(movie);
            return Ok(movie);
        }

        [HttpGet]
        public async Task<IActionResult> ShowAllMovies()
        {
            var movies = await unitOfWork.Movies.FindAllAsync(new[] { "Genre" });
            var data = mapper.Map<IEnumerable<MovieDetailsDto>>(movies);
            return Ok(data);
        }

        [HttpGet("ShowById{id}")]
        public async Task<IActionResult> ShowByIdMovies(int id)
        {
            var movie = await unitOfWork.Movies.FindAsync(m => m.Id == id, new[] { "Genre" });
            if (movie == null)
                return BadRequest("This movie not exit!");

            var movieDto = mapper.Map<MovieDetailsDto>(movie);
            return Ok(movieDto);
        }
    }

}
