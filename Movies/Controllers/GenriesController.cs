using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies.Sevices;

namespace Movies.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenriesController : ControllerBase
    {
        private readonly IGenresService _genresService;

        public GenriesController(IGenresService genresService)
        {
            _genresService = genresService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {

            var Genres = await _genresService.GetAll();
            return Ok(Genres);

        }
        [HttpPost]  
        public async Task<IActionResult> CreateAsync([FromBody] GenreDto dto)
        {
            var genre = new Genre{Name = dto.Name};
            await _genresService.Add(genre);
            
            return Ok(genre); 
        }
        [HttpPut("{id}")]
        //api/genries/id
        public async Task<IActionResult> UpdateAsync(byte id, [FromBody] GenreDto dto)
        {
            var genre = await _genresService.GetById(id);
            if(genre == null) 
            {
                return NotFound($"No Genre Was Found With Id:{id}");
            }
            genre.Name=dto.Name;
            _genresService.Update(genre);
            return Ok(genre);

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(byte id)
        {
            var genre = await _genresService.GetById(id);
            if(genre == null)
            {
                return NotFound($"No Genre Was Found With Id:{id}");
            }
            _genresService.Delete(genre);
            return Ok(genre);
        }
    }
}
