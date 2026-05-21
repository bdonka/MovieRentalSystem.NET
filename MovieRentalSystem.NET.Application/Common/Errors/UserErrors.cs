namespace MovieRentalSystem.NET.Application.Common.Errors;

public class UserNotFoundError(string userId)
: NotFoundError($"User with id {userId} does not exist.");

public class UserAlreadyExistsError(string email)
    : ConflictError($"User with email '{email}' already exists.");

public class UserHasAssignedRentalsError(string userId)
    : ConflictError($"User with id {userId} has assigned rentals and cannot be deleted.");
