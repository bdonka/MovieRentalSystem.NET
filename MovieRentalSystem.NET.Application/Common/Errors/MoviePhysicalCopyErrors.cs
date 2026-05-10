namespace MovieRentalSystem.NET.Application.Common.Errors;

public class MoviePhysicalCopyNotFoundError(int id)
    : NotFoundError($"Movie physical copy with id {id} does not exist.");

public class MoviePhysicalCopyAlreadyExistsError(string code)
    : ConflictError($"Movie physical copy with code '{code}' already exists.");

public class MoviePhysicalCopyInvalidStateError(int id)
    : ValidationError($"Movie physical copy with id {id} is in invalid state for this operation.");
