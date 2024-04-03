 namespace Movies.Sevices
{
    public interface IMoviesService
    {
        Task<IEnumerable<Movie>> GetAll(byte genreId=0);
        Task <Movie> GetById(byte Id);
        Task<Movie> Add(Movie Movie);  
        Movie Update(Movie movie);
        Movie Delete(Movie movie);
    }
}
