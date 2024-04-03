using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies.Models;
using Movies.Sevices;
using System.Diagnostics.Contracts;

namespace Movies.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    { 
        private readonly IMapper _mapper;
        private readonly IMoviesService _moviesService;
        private readonly IGenresService _genresService;

        private new List<string> _allowedExtentions = new List<string> { ".jpg", ".png" };
        private long _MaxallowedExtentions = 1048576;//1mp=>1024*1024

        public MoviesController(IMoviesService moviesService, IGenresService genresService, IMapper mapper)
        {

            _moviesService = moviesService;
            _genresService = genresService;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var movies = await _moviesService.GetAll();
            var data = _mapper.Map<IEnumerable<MoviesDetaliesDto>>(movies);
            return Ok(data);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(byte id)
        {
            var movie = await _moviesService.GetById(id);
            if (movie == null)
                return NotFound();
            var dto =_mapper.Map<MoviesDetaliesDto>(movie);



            return Ok(dto);


        }

        [HttpGet("GetByGenreId")]
        public async Task<IActionResult> GetByGenreIdAsync(byte GenreId)
        {
            var Movies = await _moviesService.GetAll(GenreId);
            var data = _mapper.Map<IEnumerable<MoviesDetaliesDto>>(Movies);
            return Ok(data);
        }


        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] MovieDto dto)
        {
            if (dto.Poster == null)
                return BadRequest("Poster Is Required!");

            if (!_allowedExtentions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                return BadRequest("Only .png and jpg image are allowed");

            if (dto.Poster.Length > _MaxallowedExtentions)
                return BadRequest("Max Allowed size for poster is 1mB! ");

            var IsValiedGenre = await _genresService.IsvalidGenre(dto.GenreId);
            if (!IsValiedGenre)
                return BadRequest("Invalid GenreId");

            using var datastream = new MemoryStream();
            await dto.Poster.CopyToAsync(datastream);

            var movie = _mapper.Map<Movie>(dto);
            movie.Poster = datastream.ToArray();

            _moviesService.Add(movie);

            return Ok(movie);

        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateAsync(byte id,[FromForm]  MovieDto dto) 
        {
            var movie = await _moviesService.GetById(id);
            if (movie == null) return NotFound($"No Movie Was Found With Id : {id}");


            var IsValiedGenre = await _genresService.IsvalidGenre(dto.GenreId);
            if (!IsValiedGenre)
                return BadRequest("Invalid GenreId");

            if(dto.Poster!= null)
            {
                if (!_allowedExtentions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                    return BadRequest("Only .png and jpg image are allowed");

                if (dto.Poster.Length > _MaxallowedExtentions)
                    return BadRequest("Max Allowed size for poster is 1mB ");

                using var dataStream = new MemoryStream();

                await dto.Poster.CopyToAsync(dataStream); 

                movie.Poster=dataStream.ToArray();
            }
            movie.Title=dto.Title;
            movie.GenreId=dto.GenreId;
            movie.Year=dto.Year;
            movie.StoreLine=dto.StoreLine;
            movie.Rate=dto.Rate;

            _moviesService.Update(movie);
           
            return Ok(movie);

        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(byte id)
        {
            var movie = await _moviesService.GetById(id);
            if (movie == null)
                return NotFound($"No Movie Was Found With Id:{id}");
             
            _moviesService.Delete(movie);

            return Ok(movie);


        }




    }
}
