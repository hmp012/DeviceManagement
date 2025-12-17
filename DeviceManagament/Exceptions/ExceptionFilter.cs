using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DeviceManagament.Exceptions;

public class ExceptionFilter : IExceptionFilter
{
    private readonly ILogger<ExceptionFilter> _logger;

    public ExceptionFilter(ILogger<ExceptionFilter> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        var response = context.Exception switch
        {
            DeviceAlreadyExistsException ex => new ObjectResult(new { error = ex.Message })
            {
                StatusCode = StatusCodes.Status409Conflict
            },
            _ => new ObjectResult(new { error = "An unexpected error occurred." })
            {
                StatusCode = StatusCodes.Status500InternalServerError
            }
        };

        _logger.LogError(context.Exception, "Exception handled by filter");
        context.Result = response;
        context.ExceptionHandled = true;
    }
}