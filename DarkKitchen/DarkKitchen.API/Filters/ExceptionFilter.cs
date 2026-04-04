using DarkKitchen.Models.ResponseDTOs;
using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DarkKitchen.API.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var statusCode = 500;
        var response = new ResponseDto
        {
            ExecutionSuccessful = false,
            Message = "Internal server error."
        };

        if(context.Exception is InvalidInputException)
        {
            statusCode = 400;
            response.Message = "An error occurred.";
        }

        if(context.Exception is UnauthorizedException)
        {
            statusCode = 401;
            response.Message = "Unauthorized access.";
        }

        if(context.Exception is AccessDeniedException)
        {
            statusCode = 403;
            response.Message = "Not allowed to access this resource.";
        }

        if(context.Exception is NotFoundException)
        {
            statusCode = 404;
            response.Message = "Resource not found.";
        }

        context.Result = new ObjectResult(response)
        {
            StatusCode = statusCode
        };
    }
}
