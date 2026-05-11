namespace MovieRentalSystem.NET.Application.Common.Errors;

public class GenreNotFoundError(int genreId)
    : NotFoundError($"Genre with id {genreId} does not exist.");

public class GenreAlreadyExistsError(string name)
    : ConflictError($"Genre '{name}' already exists.");

public class GenreAssociatedWithMoviesError(int genreId)
    : ConflictError($"Genre with id {genreId} cannot be deleted because it is associated with existing movies.");