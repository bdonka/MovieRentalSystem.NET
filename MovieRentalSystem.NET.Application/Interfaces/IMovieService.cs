namespace MovieRentalSystem.NET.Application.Interfaces
{
    public interface IMovieService
    {
        Task<IEnumerable<MovieResponse>> GetAllAsync();
        Task<Result<MovieResponse>> GetByIdAsync(int id);
        Task<Result<MovieResponse>> CreateAsync(CreateMovieRequest request);
        Task<Result> UpdateAsync(int id, UpdateMovieRequest request);
        Task<Result> DeleteAsync(int id);


        Task<Result<IEnumerable<GenreResponse>>> GetGenresAsync(int movieId);
        Task<Result> AssignGenreAsync(int movieId, int genreId);
        Task<Result> RemoveGenreAsync(int movieId, int genreId);
    }
}
