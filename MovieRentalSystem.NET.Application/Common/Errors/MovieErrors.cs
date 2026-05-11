namespace MovieRentalSystem.NET.Application.Common.Errors;

public class MovieNotFoundError(int movieId)
: NotFoundError($"Movie with id {movieId} does not exist.");

public class MovieAlreadyExistsError(string title)
    : ConflictError($"Movie '{title}' already exists.");

public class MovieHasActiveRentalsError(int movieId)
    : ConflictError($"Movie with id {movieId} cannot be deleted because it has active rentals.");

public class GenreAlreadyAssignedToMovieError(int movieId, int genreId)
    : ConflictError($"Genre {genreId} is already assigned to movie {movieId}.");
