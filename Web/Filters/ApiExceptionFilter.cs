using System.Text.Json;
using Domain.Exceptions;
using Domain.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Web.Models;

namespace Web.Filters;

public class ApiExceptionFilter : IExceptionFilter
{
    private readonly ILogger<ApiExceptionFilter> _logger;

    public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        var exception = context.Exception;
        var statusCode = 400;
        ApiErrorResponse response;

        switch (true)
        {
            case { } when exception is DuplicateEntityException:
            {
                response = new ApiErrorResponse
                {
                    Code = 10,
                    Message = exception.Message,
                    Description = exception.ToText()
                };
                break;
            }

            case { } when exception is EntityNotFoundException:
            {
                statusCode = 404;
                response = new ApiErrorResponse
                {
                    Code = 20,
                    Message = exception.Message,
                    Description = exception.ToText()
                };
                break;
            }

            case { } when exception is SoftEntityNotFoundException:
            {
                statusCode = 400;
                response = new ApiErrorResponse
                {
                    Code = 30,
                    Message = exception.Message,
                    Description = exception.ToText()
                };
                break;
            }

            default:
            {
                response = new ApiErrorResponse
                {
                    Code = 666,
                    Message = exception.Message,
                    Description = exception.ToText()
                };
                break;
            }
        }

        _logger.LogError($"Api method {context.HttpContext.Request.Path} " +
                         $"finished with code {statusCode} and error: {JsonSerializer.Serialize(response)}");
        context.Result = new JsonResult(new {response}) {StatusCode = statusCode};
    }
}