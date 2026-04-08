namespace MovieRentalSystem.NET.Application.Interfaces
{
    public interface IMoviePhysicalCopyService
    {
        Task<IEnumerable<MoviePhysicalCopyResponse>> GetAllAsync();
        Task<Result<MoviePhysicalCopyResponse>> GetByIdAsync(int id, int movieId);
        Task<Result<MoviePhysicalCopyResponse>> CreateAsync(CreateMoviePhysicalCopyRequest request);
        Task<Result> UpdateAsync(int id, int movieId, UpdateMoviePhysicalCopyRequest request);
        Task<Result> DeleteAsync(int id, int movieId);
    }
}
