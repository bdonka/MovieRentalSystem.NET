using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Mappings;
using MovieRentalSystem.NET.Domain.Entities;

public class CreateMoviePhysicalCopyCommandHandler : IRequestHandler<CreateMoviePhysicalCopyCommand, Result<MoviePhysicalCopyDto>>
{
    private readonly IDbContext _dbContext;
    private readonly ILogger<CreateMoviePhysicalCopyCommandHandler> _logger;

    public CreateMoviePhysicalCopyCommandHandler(IDbContext dbContext, ILogger<CreateMoviePhysicalCopyCommandHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<MoviePhysicalCopyDto>> Handle(
        CreateMoviePhysicalCopyCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating a new MoviePhysicalCopy for MovieId: {MovieId} with Code: {Code}", request.MovieId, request.Code);

        if (await _dbContext.MoviePhysicalCopies.AnyAsync(c => c.Code == request.Code))
        {
            _logger.LogWarning("MoviePhysicalCopy already exists with code: {Code}", request.Code);
            return Result.Fail<MoviePhysicalCopyDto>($"Code '{request.Code}' is already used.");
        }

        var copy = new MoviePhysicalCopy
        {
            MovieId = request.MovieId,
            Code = request.Code
        };

        _dbContext.MoviePhysicalCopies.Add(copy);
        await _dbContext.SaveChangesAsync();

        copy = await _dbContext.MoviePhysicalCopies.Include(m => m.Movie).SingleAsync(c => c.Id == copy.Id);

        _logger.LogInformation("MoviePhysicalCopy {CopyId} created successfully", copy.Id);

        return Result.Ok(copy.MapToMoviePhysicalCopyDto());
    }
}