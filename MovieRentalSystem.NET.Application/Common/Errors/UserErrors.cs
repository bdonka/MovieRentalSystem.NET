namespace MovieRentalSystem.NET.Application.Common.Errors;

public class UserNotFoundError(string userId)
: NotFoundError($"User with id {userId} does not exist.");

public class UserAlreadyExistsError(string email)
    : ConflictError($"User with email '{email}' already exists.");

public class UserHasAssignedRentalsError(string userId)
    : ConflictError($"User with id {userId} has assigned rentals and cannot be deleted.");

public class RoleNotFoundError(string roleName)
    : NotFoundError($"Role '{roleName}' does not exist.");

public class RoleNotAssignToUserError(string roleName, string userId)
    : ConflictError($"Role '{roleName}' does not assign to user '{userId}'.");

public class UserNotHaveRoleError(string userId, string role)
    : ConflictError($"User with id '{userId}' does not have this role '{role}'.");

public class UserNotRemoveRoleError(string userId, string role)
    : ConflictError($"User with id '{userId}' cannot remove this role '{role}'.");

public class RoleNotRemoveFromUser(string userId)
    : ConflictError($"User with id '{userId}' cannot remove from this role.");

public class NewRoleNotAssignToUser(string userId, string role)
    : ConflictError($"User with id '{userId}' cannot assign to this new role '{role}'.");

public class UserAlreadyHasRoleError(string userId)
    : ConflictError($"User with id '{userId}' already has this role.");