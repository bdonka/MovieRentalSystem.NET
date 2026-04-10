using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieRentalSystem.NET.Infrastructure.Mapping;

public static class MovieMapping
{
    public static MovieDto MapToMovieDto(this Movie movie)
    {
        return new MovieDto
        {
            Id = movie.Id,
            Title = movie.Title,
            Description = movie.Description,
            ReleaseYear = movie.ReleaseYear,
            RentalPrice = movie.RentalPrice
        };
    }

    public static Movie MapToMovieEntity(this MovieDto movie)
    {
        return new Movie
        {
            Id = movie.Id,
            Title = movie.Title,
            Description = movie.Description,
            ReleaseYear = movie.ReleaseYear,
            RentalPrice = movie.RentalPrice
        };
    }
}
