using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MoviesApi.DTOS;
using MoviesApi.Models;
using MoviesCore.Services;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IBaseRepository<Genre> genreRepository;

        public GenresController(IMapper _mapper, IBaseRepository<Genre> _genreRepository)
        {
            mapper = _mapper;
            genreRepository = _genreRepository;
        }

        [HttpGet]
        public async Task<IActionResult> ShowAllAsync()
        {
            var genres = await genreRepository.FindAllAsync(null, g => g.Name);
            return Ok(genres);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(GenresDto genresDto)
        {
            var genres = mapper.Map<Genre>(genresDto);
            await genreRepository.AddAsync(genres);
            return Ok(genres);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody] GenresDto genresDto)
        {
            var genres = await genreRepository.FindAsync(g => g.Id == id);
            if (genres == null)
                return NotFound($"No genre was found with ID : {id}");

            genres.Name = genresDto.Name;
            await genreRepository.UpdateAsync(genres);

            return Ok(genres);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var genres = await genreRepository.FindAsync(g => g.Id == id);
            if (genres == null)
                return NotFound($"No genre was found with ID : {id}");

            await genreRepository.DeleteAsync(genres);

            return Ok(genres);
        }
    }
}