using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Domain.Entities;
using MovieRentalSystem.NET.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieRentalSystem.NET.Application.Mappings;

public static class RentalMapping
{
    public static RentalDto MapToRentalDto(this Rental rental)
    {
        return new RentalDto
        {
            Id = rental.Id,
            UserId = rental.UserId,
            MoviePhysicalCopyId = rental.MoviePhysicalCopyId,
            MoviePhysicalCopy = new MoviePhysicalCopyDto {
                Id = rental.MoviePhysicalCopy.Id,
                Code = rental.MoviePhysicalCopy.Code,
                Status = rental.MoviePhysicalCopy.Status,
            },
            OrderDate = rental.OrderDate,
            RentalStartDate = rental.RentalStartDate,
            DueDate = rental.DueDate,
            ReturnDate = rental.ReturnDate,
            TotalPrice = rental.TotalPrice,
            Status = rental.Status
        };
    }

    public static Rental MapToRentalEntity(this RentalDto rental)
    {
        return new Rental
        {
            Id = rental.Id,
            UserId = rental.UserId,
            MoviePhysicalCopyId = rental.MoviePhysicalCopyId,
            OrderDate = rental.OrderDate,
            RentalStartDate = rental.RentalStartDate,
            DueDate = rental.DueDate,
            ReturnDate = rental.ReturnDate,
            TotalPrice = rental.TotalPrice,
            Status = rental.Status
        };
    }
}
