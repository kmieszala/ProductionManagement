using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ProductionManagement.Common.Exceptions;
using Serilog;

namespace ProductionManagement.WebUI.Middlewares;

public class HttpResponseExceptionFilter : IAsyncExceptionFilter
{
    private static readonly ILogger Log = Serilog.Log.ForContext<HttpResponseExceptionFilter>();

    public Task OnExceptionAsync(ExceptionContext context)
    {
        if (context.Exception is ProductionManagementException)
        {
            HandleValidateException(context);
            context.ExceptionHandled = true;
        }
        else
        {
            HandleUnknownException(context);
        }

        return Task.CompletedTask;
    }

    #region Handlers

    private void HandleUnknownException(ExceptionContext context)
    {
        Log.Error(context.Exception, $"Wystąpił nieoczekiwany błąd. {context.Exception.Message} {context.Exception.InnerException}");

        context.Result = new ObjectResult($"Wystąpił nieoczekiwany błąd. {context.Exception.Message} {context.Exception.InnerException}")
        {
            StatusCode = StatusCodes.Status500InternalServerError,
        };
    }

    private void HandleValidateException(ExceptionContext context)
    {
        var ex = context.Exception as ProductionManagementException;

        Log.Debug(context.Exception, $"Wystąpił ProductionManagementException. {context.Exception.Message} {context.Exception.InnerException}");

        context.Result = new ObjectResult(ex!.PMMessage)
        {
            StatusCode = StatusCodes.Status400BadRequest,
        };
    }

    #endregion Handlers
}