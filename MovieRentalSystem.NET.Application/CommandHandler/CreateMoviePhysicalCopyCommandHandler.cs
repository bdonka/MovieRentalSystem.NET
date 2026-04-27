using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Mappings;
using MovieRentalSystem.NET.Domain.Entities;

public class CreateMoviePhysicalCopyCommandHandler : IRequestHandler<CreateMoviePhysicalCopyCommand, Result<MoviePhysicalCopyDto>>
{
    private readonly IDbContext _dbContext;

    public CreateMoviePhysicalCopyCommandHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<MoviePhysicalCopyDto>> Handle(
        CreateMoviePhysicalCopyCommand request, CancellationToken cancellationToken)
    {
        if (await _dbContext.MoviePhysicalCopies.AnyAsync(c => c.Code == request.Code))
        {
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

        return Result.Ok(copy.MapToMoviePhysicalCopyDto());
    }
}