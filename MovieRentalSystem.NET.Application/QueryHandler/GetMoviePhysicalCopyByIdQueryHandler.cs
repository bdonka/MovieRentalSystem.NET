using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Mappings;
using MovieRentalSystem.NET.Application.Query;
public class GetMoviePhysicalCopyByIdQueryHandler : IRequestHandler<GetMoviePhysicalCopyByIdQuery, Result<MoviePhysicalCopyDto>>
{
    private readonly IDbContext _dbContext;
    private readonly ILogger<GetMoviePhysicalCopyByIdQueryHandler> _logger;
    public GetMoviePhysicalCopyByIdQueryHandler(IDbContext dbContext, ILogger<GetMoviePhysicalCopyByIdQueryHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<MoviePhysicalCopyDto>> Handle(
        GetMoviePhysicalCopyByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting movie physical copy {CopyId}", request.Id);

        var copy = await _dbContext.MoviePhysicalCopies.Include(m => m.Movie).FirstOrDefaultAsync(c => c.Id == request.Id);
        if (copy == null)
        {
            _logger.LogWarning("Movie physical copy {CopyId} not found", request.Id);
            return Result.Fail<MoviePhysicalCopyDto>($"Movie physical copy with Id {request.Id} not found.");
        }
        _logger.LogInformation("Movie physical copy {CopyId} retrieved successfully", request.Id);
        return Result.Ok(copy.MapToMoviePhysicalCopyDto());
    }
}