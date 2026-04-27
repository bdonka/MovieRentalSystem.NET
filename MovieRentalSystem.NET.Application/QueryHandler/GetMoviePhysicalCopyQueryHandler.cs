using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Mappings;
using MovieRentalSystem.NET.Application.Query;

public class GetMoviePhysicalCopyQueryHandler : IRequestHandler<GetMoviePhysicalCopyQuery, IEnumerable<MoviePhysicalCopyDto>>
{
    private readonly IDbContext _dbContext;
    public GetMoviePhysicalCopyQueryHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<MoviePhysicalCopyDto>> Handle(
        GetMoviePhysicalCopyQuery request, CancellationToken cancellationToken)
    {
        var copies = await _dbContext.MoviePhysicalCopies
            .Include(m => m.Movie)
            .ToListAsync();
        return copies.Select(c => c.MapToMoviePhysicalCopyDto()).ToList();
    }
}