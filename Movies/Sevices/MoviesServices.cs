using Microsoft.EntityFrameworkCore;
using Movies.Models;

namespace Movies.Sevices
{
    public class MoviesServices : IMoviesService
    {
        private readonly ApplicationDbContext _context;

        public MoviesServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Movie> Add(Movie Movie)
        {
           await _context.AddAsync(Movie);
            _context.SaveChanges();

            return Movie;

        }

        public Movie Delete(Movie movie)
        {
            _context.Remove(movie);
            _context.SaveChanges();
            return movie;
        }

        public async Task<IEnumerable<Movie>> GetAll(byte genreId = 0)
        {
            return await _context.movies
                .Where(m=>m.GenreId==genreId || genreId== 0)
                 .OrderByDescending(m=> m.Rate)
                 .Include(m => m.Genre)
                 .ToListAsync();
        }

        public async Task<Movie> GetById(byte Id)
        {
            return await _context.movies.Include(m => m.Genre).SingleOrDefaultAsync(m => m.Id == Id); //the best way because we search by pk

        }

        public Movie Update(Movie movie)
        {
            _context.Update(movie);
            _context.SaveChanges();
            return movie;
        }
    }
}

