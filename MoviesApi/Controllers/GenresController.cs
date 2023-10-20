using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApi.AutoMapper;
using MoviesApi.DTOS;
using MoviesApi.Models;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public GenresController(ApplicationDbContext _context,IMapper _mapper)
        {
            context = _context;
            mapper = _mapper;
        }

        [HttpGet]
        public async Task<IActionResult> ShowAllAsync()
        {
            var genres=await context.Genres.OrderBy(g=>g.Name).ToListAsync();
            return Ok(genres); 
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync(GenresDto genresDto)
        {
            var genres = mapper.Map<Genre>(genresDto);
            await context.Genres.AddAsync(genres);
            await context.SaveChangesAsync();
            return Ok(genres);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int id,[FromBody]GenresDto genresDto)
        {
            var genres = context.Genres.FirstOrDefault(g=>g.Id==id);
            if (genres == null)
                return NotFound($"No genre was found with ID : {id}");
            genres.Name = genresDto.Name;
            context.SaveChanges();

            return Ok(genres);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var genres = context.Genres.FirstOrDefault(g => g.Id == id);
            if (genres == null)
                return NotFound($"No genre was found with ID : {id}");
            context.Genres.Remove(genres);
            context.SaveChanges();

            return Ok(genres);
        }
    }
}