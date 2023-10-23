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

    public class GenresController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;

        public GenresController(IMapper _mapper, IUnitOfWork _unitOfWork)
        {
            mapper = _mapper;
            unitOfWork = _unitOfWork;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ShowAllAsync()
        {
            var genres = await unitOfWork.Genres.FindAllAsync(null, g => g.Name);
            return Ok(genres);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(GenresDto genresDto)
        {
            var genres = mapper.Map<Genre>(genresDto);
            await unitOfWork.Genres.AddAsync(genres);
            return Ok(genres);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody] GenresDto genresDto)
        {
            var genres = await unitOfWork.Genres.FindAsync(g => g.Id == id);
            if (genres == null)
                return NotFound($"No genre was found with ID : {id}");

            genres.Name = genresDto.Name;
            await unitOfWork.Genres.UpdateAsync(genres);

            return Ok(genres);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var genres = await unitOfWork.Genres.FindAsync(g => g.Id == id);
            if (genres == null)
                return NotFound($"No genre was found with ID : {id}");

            await unitOfWork.Genres.DeleteAsync(genres);

            return Ok(genres);
        }
    }
}