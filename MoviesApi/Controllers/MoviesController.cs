using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApi.DTOS;
using MoviesApi.Models;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private new List<string> allawExtinstians = new List<string> { ".jpg", ".png" };
        private long MaxAllowPosterSize = 1048576;
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public MoviesController(ApplicationDbContext _context, IMapper _mapper)
        {
            context = _context;
            mapper = _mapper;
        }

        [HttpPost("AddMovie")]
        public async Task<IActionResult> CreateMovie([FromForm] MovieCreateDto moviesDto)
        {
            if (!allawExtinstians.Contains(Path.GetExtension(moviesDto.Poster.FileName).ToLower()))
                return BadRequest("Only .png and .jpg images are allowed!");
            if (moviesDto.Poster.Length > MaxAllowPosterSize)
                return BadRequest("Max allow size 1MB for poster!");
            var isValidGenre = await context.Genres.AnyAsync(g => g.Id == moviesDto.GenreId);
            if (!isValidGenre)
                return BadRequest("This Genre Not Exist!");

            using var dataStream = new MemoryStream();
            await moviesDto.Poster.CopyToAsync(dataStream);

            //var movie =mapper.Map<Movie>(moviesDto);
            //movie.Poster=dataStream.ToArray();

            var movie = new Movie()
            {
                Title = moviesDto.Title,
                Year = moviesDto.Year,
                Rate = moviesDto.Rate,
                Storeline = moviesDto.Storeline,
                GenreId = moviesDto.GenreId,
                Poster = dataStream.ToArray()
            };

            await context.AddAsync(movie);
            await context.SaveChangesAsync();
            return Ok(movie);
        }

        [HttpDelete("DeleteMovie{id}")]
        public async Task<IActionResult> DeleteMovie([FromRoute] int id)
        {
            var movie = context.Movies.FirstOrDefault(g => g.Id == id);
            if (movie == null)
                return NotFound($"No movvie was found with ID : {id}");
            context.Movies.Remove(movie);
            context.SaveChanges();
            return Ok(movie);
        }

        [HttpPut("EditMovie{id}")]
        public async Task<IActionResult> UpdateMovie([FromRoute] int id, [FromForm] MovieUpdateDto moviesDto)
        {
            var movie = context.Movies.Include(m=>m.Genre).FirstOrDefault(g => g.Id == id);
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

            context.SaveChanges(); 
            return Ok(movie);
        }

        [HttpGet]
        public async Task<IActionResult> ShowAllMovies()
        {
            var movies = await context.Movies
                .Include(m => m.Genre)
                //.Select(m=> mapper.Map<MovieDetailsDto>(m))
                .Select(m => new MovieDetailsDto
                {
                    Id = m.Id,
                    Title = m.Title,
                    Year = m.Year,
                    Rate = m.Rate,
                    Storeline = m.Storeline,
                    GenreId = m.GenreId,
                    Poster = m.Poster,
                    GenreName = m.Genre.Name
                })
                .ToListAsync();
            return Ok(movies);
        }
        [HttpGet("ShowById{id}")]
        public async Task<IActionResult> ShowByIdMovies(int id)
        {
            var movie = await context.Movies.Include(m => m.Genre).FirstOrDefaultAsync(m => m.Id == id);
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
