namespace MovieRentalSystem.NET.Application.Common.Errors;

public class RentalNotFoundError(int rentalId)
: NotFoundError($"Rental with id {rentalId} does not exist.");