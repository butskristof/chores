using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace Chores.Api.Extensions;

internal static class ResultExtensions
{
    // if the result doesn't have errors, the function that's passed in to get to an IResult is used
    // in case of errors, a mapping to ProblemDetails is done to provide a meaningful error response 
    internal static IResult MapToValueOrProblem<T>(this ErrorOr<T> result, Func<T, IResult> onValue)
        => result.Match(onValue, MapErrorsToProblemDetailsResult);

    private static IResult MapErrorsToProblemDetailsResult(List<Error> errors)
    {
        // start with a general problem details response
        var problemDetails = new ProblemDetails();

        // in case the list of errors contains validation errors, the problem details variable is replaced with 
        // a specific validation problem details implementation which contains the errors as well
        // since we have to check whether there's any validation errors anyway, we attach the logic for grouping them
        // by Code (which we assume to be the property name)
        var validationErrors = errors
            .Where(e => e.Type is ErrorType.Validation)
            .GroupBy(e => e.Code)
            .ToDictionary(group => group.Key, group => group.Select(e => e.Description).ToArray());
        if (validationErrors.Count != 0)
            problemDetails = new ValidationProblemDetails(validationErrors);
        else
        {
            // in other cases, we only go by the first error in the list
            // this may be an oversimplification, but the problem details response only allows for one detail and 
            // it makes processing easier for the consumer
            // assuming the list of errors will contain at least one error may be optimistic, but this method is 
            // meant to be called in the error part of .Match, which should only happen if there's actually some errors 
            var error = errors.First();
            var statusCode = error.Type switch
            {
                // validation is already handled above
                // ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.Unauthorized => StatusCodes.Status403Forbidden,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError,
            };

            problemDetails.Status = statusCode;
            problemDetails.Detail = error.Description;
        }

        return Results.Problem(problemDetails);
    }
}