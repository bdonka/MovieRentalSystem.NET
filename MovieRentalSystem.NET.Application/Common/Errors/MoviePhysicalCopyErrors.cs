namespace MovieRentalSystem.NET.Application.Common.Errors;

public class MoviePhysicalCopyNotFoundError(int id)
    : NotFoundError($"Movie physical copy with id {id} does not exist.");

public class MoviePhysicalCopyAlreadyExistsError(string code)
    : ConflictError($"Movie physical copy with code '{code}' already exists.");