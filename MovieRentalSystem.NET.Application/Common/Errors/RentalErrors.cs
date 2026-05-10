namespace MovieRentalSystem.NET.Application.Common.Errors;

public class RentalNotFoundError(int rentalId)
: NotFoundError($"Rental with id {rentalId} does not exist.");

public class MovieAlreadyRentedError(int moviePhysicalCopyId)
    : ConflictError($"Movie physical copy with id {moviePhysicalCopyId} is already rented.");

public class RentalInvalidStateError(int rentalId)
    : ValidationError($"Rental with id {rentalId} is in invalid state for this operation.");
