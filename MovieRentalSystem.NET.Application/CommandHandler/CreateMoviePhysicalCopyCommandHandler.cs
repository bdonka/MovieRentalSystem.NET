using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieRentalSystem.NET.Application.Common.Errors;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Mappings;
using MovieRentalSystem.NET.Domain.Entities;

public class CreateMoviePhysicalCopyCommandHandler
    : IRequestHandler<
        CreateMoviePhysicalCopyCommand,
        Result<MoviePhysicalCopyDto>>
{
    private readonly IDbContext _dbContext;

    private readonly ILogger<CreateMoviePhysicalCopyCommandHandler> _logger;

    public CreateMoviePhysicalCopyCommandHandler(
        IDbContext dbContext,
        ILogger<CreateMoviePhysicalCopyCommandHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<MoviePhysicalCopyDto>> Handle(
        CreateMoviePhysicalCopyCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Creating a new MoviePhysicalCopy for MovieId: {MovieId} with Code: {Code}",
            request.MovieId,
            request.Code);

        // Check whether the movie exists.
        var movieExists =
            await _dbContext.Movies
                .AnyAsync(
                    movie => movie.Id == request.MovieId,
                    cancellationToken);

        if (!movieExists)
        {
            _logger.LogWarning(
                "Cannot create MoviePhysicalCopy. Movie {MovieId} does not exist",
                request.MovieId);

            return Result.Fail<MoviePhysicalCopyDto>(
                new MovieNotFoundError(request.MovieId));
        }

        // Check whether a physical copy with the same code
        // already exists.
        var copyExists =
            await _dbContext.MoviePhysicalCopies
                .AnyAsync(
                    copy => copy.Code == request.Code,
                    cancellationToken);

        if (copyExists)
        {
            _logger.LogWarning(
                "MoviePhysicalCopy already exists with code: {Code}",
                request.Code);

            return Result.Fail<MoviePhysicalCopyDto>(
                new MoviePhysicalCopyAlreadyExistsError(
                    request.Code));
        }

        // Create the physical copy.
        var copy = new MoviePhysicalCopy
        {
            MovieId = request.MovieId,
            Code = request.Code
        };

        _dbContext.MoviePhysicalCopies.Add(copy);

        await _dbContext.SaveChangesAsync(
            cancellationToken);

        _logger.LogInformation(
            "MoviePhysicalCopy {CopyId} created successfully",
            copy.Id);

        return Result.Ok(
            copy.MapToMoviePhysicalCopyDto());
    }
}
