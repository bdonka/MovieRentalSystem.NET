using FluentResults;
using Microsoft.AspNetCore.Mvc;
using MovieRentalSystem.NET.Application.Common;

namespace MovieRentalSystem.NET.WebApi.Common
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class ResultsControllerBase : ControllerBase
    {
        protected ActionResult<T> ToOkOrErrorResponse<T>(Result<T> result) =>
            result.IsSuccess
                ? base.Ok(result.Value)
                : HandleFailedResult(result.Errors[0]);

        protected ActionResult ToNoContentOrErrorResponse(Result result) =>
            result.IsSuccess
                ? base.NoContent()
                : HandleFailedResult(result.Errors[0]);

        protected ActionResult<T> ToCreatedAtActionOrErrorResponse<T>(string actionName, object routeValues, Result<T> result) =>
            result.IsSuccess
                ? base.CreatedAtAction(actionName, routeValues, result.Value)
                : HandleFailedResult(result.Errors[0]);

        private ObjectResult HandleFailedResult(IError error)
        {
            return error switch
            {
                NotFoundError => Problem(detail: error.Message, statusCode: StatusCodes.Status404NotFound),
                ValidationError => Problem(detail: error.Message, statusCode: StatusCodes.Status400BadRequest),
                UnauthorizedError => Problem(detail: error.Message, statusCode: StatusCodes.Status401Unauthorized),
                ConflictError => Problem(detail: error.Message, statusCode: StatusCodes.Status409Conflict),
                _ => throw new Exception(error.ToString())
            };
        }
    }
}
