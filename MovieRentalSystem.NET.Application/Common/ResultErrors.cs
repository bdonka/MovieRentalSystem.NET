using FluentResults;

namespace MovieRentalSystem.NET.Application.Common;

public class NotFoundError(string message) : Error(message);

public class ValidationError(string message) : Error(message);

public class UnauthorizedError(string message) : Error(message);

public class ConflictError(string message) : Error(message);
