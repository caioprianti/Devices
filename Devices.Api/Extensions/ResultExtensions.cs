using Devices.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace Devices.Api.Extensions;

public static class ErrorExtensions
{
    public static ObjectResult ToProblem(this Error error)
    {
        var statusCode = error.Code switch
        {
            "Device.NotFound" =>
                StatusCodes.Status404NotFound,

            "Device.CannotUpdateInUse" or
                "Device.CannotDeleteInUse" =>
                StatusCodes.Status409Conflict,

            _ =>
                StatusCodes.Status400BadRequest
        };

        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = statusCode switch
            {
                StatusCodes.Status404NotFound =>
                    "Resource not found",

                StatusCodes.Status409Conflict =>
                    "Domain rule conflict",

                _ =>
                    "Request failed"
            },
            Detail = error.Description,
            Extensions =
            {
                ["code"] = error.Code
            }
        };

        return new ObjectResult(problem)
        {
            StatusCode = statusCode
        };
    }
}